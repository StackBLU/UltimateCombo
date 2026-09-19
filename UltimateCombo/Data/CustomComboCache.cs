using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Game.ClientState.Statuses;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.Game;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using UltimateCombo.Core;

namespace UltimateCombo.Data;

internal partial class CustomComboCache : IDisposable
{
    private const uint InvalidObjectID = 0xE000_0000;

    private readonly ConcurrentDictionary<(uint StatusID, ulong? TargetID, ulong? SourceID), StatusCacheEntry> _statusCache = new();
    private readonly ConcurrentDictionary<uint, CooldownData> _cooldownCache = new();
    private readonly ConcurrentDictionary<Type, JobGaugeBase> _jobGaugeCache = new();
    private bool _disposed;

    internal CustomComboCache()
    {
        Service.Framework.Update += OnFrameworkUpdate;
    }

    internal void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        Service.Framework.Update -= OnFrameworkUpdate;
        _statusCache.Clear();
        _cooldownCache.Clear();
        _jobGaugeCache.Clear();
    }

    internal T GetJobGauge<T>() where T : JobGaugeBase
    {
        JobGaugeBase gauge = _jobGaugeCache.GetOrAdd(typeof(T), _ => Service.JobGauges.Get<T>());
        return (T) gauge;
    }

    internal IStatus? GetStatus(uint statusID, IGameObject? target, ulong? sourceID)
    {
        var recording = PerformanceProfiler.IsRecording;
        var startedAt = recording ? Stopwatch.GetTimestamp() : 0;
        var cacheHit = false;

        try
        {
            (uint StatusID, ulong? TargetID, ulong? SourceID) key = (StatusID: statusID, TargetID: target?.GameObjectId, SourceID: sourceID);
            if (_statusCache.TryGetValue(key, out StatusCacheEntry cached))
            {
                cacheHit = true;
                return cached.Status;
            }

            IStatus? found = null;

            if (target is IBattleChara character)
            {
                foreach (IStatus status in character.StatusList)
                {
                    if (status.StatusId == statusID && IsMatchingSource(status, sourceID))
                    {
                        found = status;
                        break;
                    }
                }
            }

            _statusCache[key] = new StatusCacheEntry(found);
            return found;
        }
        finally
        {
            if (recording)
            {
                PerformanceProfiler.RecordStatusLookup(Stopwatch.GetTimestamp() - startedAt, cacheHit);
            }
        }
    }

    internal CooldownData GetCooldown(uint actionID)
    {
        return _cooldownCache.GetOrAdd(actionID, static id => new CooldownData
        {
            ActionID = id
        });
    }

    internal static unsafe int GetResourceCost(uint actionID)
    {
        if (ActionManager.Instance() == null)
        {
            return 0;
        }

        return ActionManager.GetActionCost(ActionType.Action, actionID, 0, 0, 0, 0);
    }

    private static bool IsMatchingSource(IStatus status, ulong? sourceID)
    {
        return !sourceID.HasValue
            || status.SourceId == 0
            || status.SourceId == InvalidObjectID
            || status.SourceId == sourceID;
    }

    private void OnFrameworkUpdate(IFramework framework)
    {
        _statusCache.Clear();
    }

    void IDisposable.Dispose()
    {
        Dispose();
    }

    private readonly record struct StatusCacheEntry(IStatus? Status);
}
