using Dalamud.Hooking;
using Dalamud.Utility;
using FFXIVClientStructs.FFXIV.Client.Game;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UltimateCombo.Attributes;
using UltimateCombo.Combos;
using UltimateCombo.Combos.Content;
using UltimateCombo.Combos.General;

namespace UltimateCombo.Core;

public sealed partial class IconReplacer : IDisposable
{
    private readonly List<CustomComboBase> _customCombos;
    private readonly HashSet<Presets> _enabledPresetSnapshot = [];
    private CustomComboBase[] _enabledCombos = [];
    private CustomComboBase[] _currentJobCombos = [];
    private uint _currentJobID;
    private uint _currentTerritoryID;
    private bool _currentPvPState;
    private bool _inOccult;
    private bool _inBozja;
    private bool _inEureka;
    private bool _inVariant;
    private Hook<IsIconReplaceableDelegate>? _isIconReplaceableHook;
    private Hook<GetIconDelegate>? _getIconHook;
    private IntPtr _actionManager;
    private bool _enabled;
    private bool _disposed;

    internal int EnabledComboCount => _enabledCombos.Length;

    internal int CountCombosForJob(uint jobID)
    {
        return _enabledCombos.Count(combo => combo.AppliesToJob(jobID));
    }

    internal static class ThreadSafeCache
    {
        internal static bool LocalPlayerAvailable;
        internal static ulong LocalPlayerObjectID;
        internal static uint LocalPlayerClassJobID;
    }

    internal IconReplacer()
    {
        _customCombos = [.. Assembly.GetAssembly(typeof(CustomComboBase))!.GetTypes()
            .Where(t => !t.IsAbstract && t.BaseType == typeof(CustomComboBase))
            .Select(Activator.CreateInstance)
            .Cast<CustomComboBase>()
            .OrderByDescending(x => x.Preset)];

        foreach (CustomComboBase combo in _customCombos)
        {
            byte jobID = combo.Preset.GetAttribute<CustomComboInfoAttribute>()?.JobID ?? Common.JobID;
            combo.InitializeJobID(jobID);
        }

        PerformanceProfiler.RegisterCombos(_customCombos);
        RefreshEnabledCombos();

        try
        {
            _getIconHook = Service.GameInteropProvider.HookFromAddress<GetIconDelegate>(
                ActionManager.Addresses.GetAdjustedActionId.Value, GetIconDetour);

            _isIconReplaceableHook = Service.GameInteropProvider.HookFromAddress<IsIconReplaceableDelegate>(
                Service.Address.IsActionIdReplaceable, IsIconReplaceableDetour);

            Enable();
        }
        catch
        {
            Dispose();
            throw;
        }
    }

    internal void Enable()
    {
        if (_disposed || _enabled)
        {
            return;
        }

        _enabled = true;

        _getIconHook?.Enable();
        _isIconReplaceableHook?.Enable();
    }

    internal void Disable()
    {
        if (!_enabled)
        {
            return;
        }

        _enabled = false;

        try { _getIconHook?.Disable(); } catch { }
        try { _isIconReplaceableHook?.Disable(); } catch { }
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        Disable();

        try { _getIconHook?.Dispose(); } catch { }
        _getIconHook = null;

        try { _isIconReplaceableHook?.Dispose(); } catch { }
        _isIconReplaceableHook = null;

        _actionManager = IntPtr.Zero;
    }

    private delegate ulong IsIconReplaceableDelegate(uint actionID);
    private delegate uint GetIconDelegate(IntPtr actionManager, uint actionID);

    internal uint OriginalHook(uint actionID)
    {
        return _getIconHook == null ? actionID : _getIconHook.Original(_actionManager, actionID);
    }

    internal void RefreshEnabledCombos()
    {
        HashSet<Presets> enabledPresets = Service.Configuration.EnabledActions;
        if (_enabledPresetSnapshot.SetEquals(enabledPresets))
        {
            return;
        }

        _enabledPresetSnapshot.Clear();
        _enabledPresetSnapshot.UnionWith(enabledPresets);
        _enabledCombos = [.. _customCombos.Where(combo => enabledPresets.Contains(combo.Preset))];
        RebuildCurrentJobCombos();
    }

    internal void RefreshJobCombos(uint jobID)
    {
        uint territoryID = Service.ClientState.TerritoryType;
        bool inPvP = Service.ClientState.IsPvP;
        if (_currentJobID == jobID && _currentTerritoryID == territoryID && _currentPvPState == inPvP)
        {
            return;
        }

        _currentJobID = jobID;
        _currentPvPState = inPvP;
        if (_currentTerritoryID != territoryID)
        {
            _currentTerritoryID = territoryID;
            RefreshContentTerritory();
        }

        RebuildCurrentJobCombos();
    }

    private void RebuildCurrentJobCombos()
    {
        _currentJobCombos = [.. _enabledCombos.Where(combo => combo.AppliesToJob(_currentJobID)
            && IsAvailableInCurrentTerritory(combo.Preset))];
    }

    private void RefreshContentTerritory()
    {
        _inOccult = false;
        _inBozja = false;
        _inEureka = false;
        _inVariant = Variant.VariantRaise != 0;

        if (_currentTerritoryID == 0)
        {
            return;
        }

        // Read the English place name once on zone changes, never for each icon.
        var territory = Service.DataManager
            .GetExcelSheet<Lumina.Excel.Sheets.TerritoryType>(Dalamud.Game.ClientLanguage.English)
            .GetRow(_currentTerritoryID);
        string name = territory.PlaceName.Value.Name.ToString();

        _inOccult = name.Contains("South Horn", StringComparison.OrdinalIgnoreCase)
            || name.Contains("North Horn", StringComparison.OrdinalIgnoreCase)
            || name.Contains("Forked Tower", StringComparison.OrdinalIgnoreCase);

        _inBozja = name.Contains("Bozjan Southern Front", StringComparison.OrdinalIgnoreCase)
            || name.Contains("Zadnor", StringComparison.OrdinalIgnoreCase)
            || name.Contains("Castrum Lacus Litore", StringComparison.OrdinalIgnoreCase)
            || name.Contains("Dalriada", StringComparison.OrdinalIgnoreCase)
            || name.Contains("Delubrum Reginae", StringComparison.OrdinalIgnoreCase);

        _inEureka = name.Contains("Eureka Anemos", StringComparison.OrdinalIgnoreCase)
            || name.Contains("Eureka Pagos", StringComparison.OrdinalIgnoreCase)
            || name.Contains("Eureka Pyros", StringComparison.OrdinalIgnoreCase)
            || name.Contains("Eureka Hydatos", StringComparison.OrdinalIgnoreCase)
            || name.Contains("Baldesion Arsenal", StringComparison.OrdinalIgnoreCase);
    }

    private bool IsAvailableInCurrentTerritory(Presets preset)
    {
        return (!PresetStorage.IsPvP(preset) || _currentPvPState)
            && (!PresetStorage.IsOccult(preset) || _inOccult)
            && (!PresetStorage.IsBozja(preset) || _inBozja)
            && (!PresetStorage.IsEureka(preset) || _inEureka)
            && (!PresetStorage.IsVariant(preset) || _inVariant);
    }

    private unsafe uint GetIconDetour(IntPtr actionManager, uint actionID)
    {
        _actionManager = actionManager;
        var recording = PerformanceProfiler.IsRecording;
        var startedAt = recording ? Stopwatch.GetTimestamp() : 0;
        var checkedCombos = 0;
        var replaced = false;

        try
        {
            if (_disposed || !_enabled || !ThreadSafeCache.LocalPlayerAvailable || _currentJobCombos.Length == 0)
            {
                return OriginalHook(actionID);
            }

            try
            {
                ActionManager* manager = ActionManager.Instance();
                if (manager == null)
                {
                    return OriginalHook(actionID);
                }

                if (manager->QueuedActionType == ActionType.Action
                    && manager->QueuedActionId != actionID
                    && !Service.ClientState.IsPvP)
                {
                    return OriginalHook(actionID);
                }

                if (!recording)
                {
                    foreach (CustomComboBase combo in _currentJobCombos)
                    {
                        if (combo.TryInvoke(actionID, out var newActionID))
                        {
                            return newActionID;
                        }
                    }

                    return OriginalHook(actionID);
                }

                foreach (CustomComboBase combo in _currentJobCombos)
                {
                    checkedCombos++;
                    var previousIndex = PerformanceProfiler.EnterCombo(combo.ProfilerIndex);

                    try
                    {
                        if (combo.TryInvoke(actionID, out var newActionID))
                        {
                            replaced = true;
                            PerformanceProfiler.RecordReplacement();
                            return newActionID;
                        }
                    }
                    finally
                    {
                        PerformanceProfiler.ExitCombo(previousIndex);
                    }
                }

                return OriginalHook(actionID);
            }
            catch (Exception ex)
            {
                Service.PluginLog.Error(ex, "Preset error");
                return OriginalHook(actionID);
            }
        }
        finally
        {
            if (recording)
            {
                PerformanceProfiler.RecordIcon(Stopwatch.GetTimestamp() - startedAt, checkedCombos, replaced);
            }
        }
    }

    private ulong IsIconReplaceableDetour(uint _)
    {
        return 1;
    }
}
