using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using UltimateCombo.Combos;

namespace UltimateCombo.Core;

internal static class PerformanceProfiler
{
    [ThreadStatic]
    private static int _currentComboIndex;

    private static ComboCounter[] _comboCounters = [];
    private static int _recording;
    private static long _startedAt;

    private static long _iconCalls;
    private static long _iconTicks;
    private static long _comboChecks;
    private static long _replacements;

    private static long _actionReadyCalls;
    private static long _actionReadyTicks;

    private static long _statusHits;
    private static long _statusHitTicks;
    private static long _statusMisses;
    private static long _statusMissTicks;

    private static int _frameworkThread;
    private static int _iconThread;
    private static int _actionReadyThread;
    private static int _statusThread;
    private static int _multipleFrameworkThreads;
    private static int _multipleIconThreads;
    private static int _multipleActionReadyThreads;
    private static int _multipleStatusThreads;

    internal static bool IsRecording => Volatile.Read(ref _recording) != 0;

    internal static void RegisterCombos(IReadOnlyList<CustomComboBase> combos)
    {
        ComboCounter[] counters = new ComboCounter[combos.Count + 1];
        counters[0] = new ComboCounter(default);

        for (var i = 0; i < combos.Count; i++)
        {
            counters[i + 1] = new ComboCounter(combos[i].Preset);
            combos[i].ProfilerIndex = i + 1;
        }

        _comboCounters = counters;
    }

    internal static bool Start()
    {
        if (IsRecording)
        {
            return false;
        }

        _iconCalls = 0;
        _iconTicks = 0;
        _comboChecks = 0;
        _replacements = 0;
        _actionReadyCalls = 0;
        _actionReadyTicks = 0;
        _statusHits = 0;
        _statusHitTicks = 0;
        _statusMisses = 0;
        _statusMissTicks = 0;

        _frameworkThread = 0;
        _iconThread = 0;
        _actionReadyThread = 0;
        _statusThread = 0;
        _multipleFrameworkThreads = 0;
        _multipleIconThreads = 0;
        _multipleActionReadyThreads = 0;
        _multipleStatusThreads = 0;

        foreach (ComboCounter counter in _comboCounters)
        {
            counter.Visits = 0;
            counter.StatusLookups = 0;
            counter.Replacements = 0;
        }

        _currentComboIndex = 0;

        _startedAt = Stopwatch.GetTimestamp();
        Volatile.Write(ref _recording, 1);
        return true;
    }

    internal static string? Stop()
    {
        if (!IsRecording)
        {
            return null;
        }

        Volatile.Write(ref _recording, 0);
        var elapsedSeconds = (Stopwatch.GetTimestamp() - _startedAt) / (double) Stopwatch.Frequency;

        var iconCalls = Interlocked.Read(ref _iconCalls);
        var actionReadyCalls = Interlocked.Read(ref _actionReadyCalls);
        var statusHits = Interlocked.Read(ref _statusHits);
        var statusMisses = Interlocked.Read(ref _statusMisses);

        StringBuilder report = new();
        report.AppendLine("UltimateCombo performance sample");
        report.AppendLine($"Duration: {elapsedSeconds:F1} seconds");
        report.AppendLine($"Current job ID: {IconReplacer.ThreadSafeCache.LocalPlayerClassJobID}");
        report.AppendLine($"Enabled combos: {Service.IconReplacer.EnabledComboCount}");
        report.AppendLine($"Enabled combos for current job: "
            + Service.IconReplacer.CountCombosForJob(IconReplacer.ThreadSafeCache.LocalPlayerClassJobID));
        report.AppendLine();

        report.AppendLine($"Icon detour: {iconCalls:N0} calls ({Rate(iconCalls, elapsedSeconds):N0}/s), "
            + $"{Milliseconds(Interlocked.Read(ref _iconTicks)):F1} ms total, "
            + $"{MicrosecondsPerCall(Interlocked.Read(ref _iconTicks), iconCalls):F2} us/call");
        report.AppendLine($"Combos checked: {Interlocked.Read(ref _comboChecks):N0} "
            + $"({PerCall(Interlocked.Read(ref _comboChecks), iconCalls):F1}/icon call)");
        report.AppendLine($"Icons replaced: {Interlocked.Read(ref _replacements):N0}");
        report.AppendLine();

        report.AppendLine($"ActionReady: {actionReadyCalls:N0} calls ({Rate(actionReadyCalls, elapsedSeconds):N0}/s), "
            + $"{Milliseconds(Interlocked.Read(ref _actionReadyTicks)):F1} ms total, "
            + $"{MicrosecondsPerCall(Interlocked.Read(ref _actionReadyTicks), actionReadyCalls):F2} us/call");
        report.AppendLine();

        report.AppendLine($"Status lookups: {statusHits + statusMisses:N0} calls "
            + $"({Rate(statusHits + statusMisses, elapsedSeconds):N0}/s)");
        report.AppendLine($"  Cache hits: {statusHits:N0}, "
            + $"{Milliseconds(Interlocked.Read(ref _statusHitTicks)):F1} ms total");
        report.AppendLine($"  Cache misses: {statusMisses:N0}, "
            + $"{Milliseconds(Interlocked.Read(ref _statusMissTicks)):F1} ms total");
        report.AppendLine();

        report.AppendLine("Presets with the most status lookups:");
        var presetTotals = _comboCounters.Skip(1)
            .GroupBy(counter => counter.Preset)
            .Select(group => new
            {
                Preset = group.Key,
                Lookups = group.Sum(counter => Interlocked.Read(ref counter.StatusLookups)),
                Visits = group.Sum(counter => Interlocked.Read(ref counter.Visits)),
                Replacements = group.Sum(counter => Interlocked.Read(ref counter.Replacements))
            })
            .OrderByDescending(group => group.Lookups)
            .ToArray();

        foreach (var preset in presetTotals.Take(12))
        {
            if (preset.Lookups == 0)
            {
                break;
            }

            report.AppendLine($"  {preset.Preset}: {preset.Lookups:N0} lookups "
                + $"({PerCall(preset.Lookups, statusHits + statusMisses) * 100:F1}% of all), "
                + $"{preset.Visits:N0} checks, {preset.Replacements:N0} replacements");
        }

        var attributedLookups = presetTotals.Sum(preset => preset.Lookups);
        report.AppendLine($"  Outside combo calls: {Math.Max(0, statusHits + statusMisses - attributedLookups):N0}");
        report.AppendLine();

        report.AppendLine("Managed thread IDs (different IDs indicate calls on different threads):");
        report.AppendLine($"  Framework: {ThreadDescription(_frameworkThread, _multipleFrameworkThreads)}");
        report.AppendLine($"  Icon detour: {ThreadDescription(_iconThread, _multipleIconThreads)}");
        report.AppendLine($"  ActionReady: {ThreadDescription(_actionReadyThread, _multipleActionReadyThreads)}");
        report.AppendLine($"  Status lookup: {ThreadDescription(_statusThread, _multipleStatusThreads)}");
        report.AppendLine();
        report.AppendLine("Timing is diagnostic and includes the profiler's own overhead. "
            + "ActionReady and status times may also be included in icon detour time.");
        report.AppendLine("Preset attribution adds overhead. Compare lookup counts, "
            + "not detour timing, with earlier samples.");

        return report.ToString();
    }

    internal static void RecordFrameworkThread()
    {
        if (IsRecording)
        {
            ObserveThread(ref _frameworkThread, ref _multipleFrameworkThreads);
        }
    }

    internal static int EnterCombo(int index)
    {
        var previousIndex = _currentComboIndex;
        _currentComboIndex = index;
        Interlocked.Increment(ref _comboCounters[index].Visits);
        return previousIndex;
    }

    internal static void ExitCombo(int previousIndex)
    {
        _currentComboIndex = previousIndex;
    }

    internal static void RecordReplacement()
    {
        Interlocked.Increment(ref _comboCounters[_currentComboIndex].Replacements);
    }

    internal static void RecordIcon(long ticks, int checks, bool replaced)
    {
        Interlocked.Increment(ref _iconCalls);
        Interlocked.Add(ref _iconTicks, ticks);
        Interlocked.Add(ref _comboChecks, checks);
        if (replaced)
        {
            Interlocked.Increment(ref _replacements);
        }

        ObserveThread(ref _iconThread, ref _multipleIconThreads);
    }

    internal static void RecordActionReady(long ticks)
    {
        Interlocked.Increment(ref _actionReadyCalls);
        Interlocked.Add(ref _actionReadyTicks, ticks);
        ObserveThread(ref _actionReadyThread, ref _multipleActionReadyThreads);
    }

    internal static void RecordStatusLookup(long ticks, bool hit)
    {
        if (hit)
        {
            Interlocked.Increment(ref _statusHits);
            Interlocked.Add(ref _statusHitTicks, ticks);
        }
        else
        {
            Interlocked.Increment(ref _statusMisses);
            Interlocked.Add(ref _statusMissTicks, ticks);
        }

        if (_currentComboIndex != 0)
        {
            Interlocked.Increment(ref _comboCounters[_currentComboIndex].StatusLookups);
        }

        ObserveThread(ref _statusThread, ref _multipleStatusThreads);
    }

    private static void ObserveThread(ref int firstThread, ref int multipleThreads)
    {
        var currentThread = Environment.CurrentManagedThreadId;
        var observedThread = Interlocked.CompareExchange(ref firstThread, currentThread, 0);
        if (observedThread != 0 && observedThread != currentThread)
        {
            Volatile.Write(ref multipleThreads, 1);
        }
    }

    private static string ThreadDescription(int thread, int multipleThreads)
    {
        return thread == 0 ? "no calls" : multipleThreads != 0 ? $"{thread} and others" : thread.ToString();
    }

    private static double Rate(long calls, double seconds)
    {
        return seconds > 0 ? calls / seconds : 0;
    }

    private static double PerCall(long count, long calls)
    {
        return calls > 0 ? (double) count / calls : 0;
    }

    private static double Milliseconds(long ticks)
    {
        return ticks * 1000d / Stopwatch.Frequency;
    }

    private static double MicrosecondsPerCall(long ticks, long calls)
    {
        return calls > 0 ? ticks * 1000000d / Stopwatch.Frequency / calls : 0;
    }

    private sealed class ComboCounter(Presets preset)
    {
        internal Presets Preset { get; } = preset;
        internal long Visits;
        internal long StatusLookups;
        internal long Replacements;
    }
}
