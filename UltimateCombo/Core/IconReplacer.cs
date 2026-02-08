using Dalamud.Hooking;
using FFXIVClientStructs.FFXIV.Client.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UltimateCombo.Core;

public sealed partial class IconReplacer : IDisposable
{
    private readonly List<CustomComboBase> _customCombos;
    private Hook<IsIconReplaceableDelegate>? _isIconReplaceableHook;
    private Hook<GetIconDelegate>? _getIconHook;
    private IntPtr _actionManager;
    private bool _enabled;
    private bool _disposed;

    internal static class ThreadSafeCache
    {
        internal static bool LocalPlayerAvailable;
        internal static ulong LocalPlayerObjectId;
    }

    internal IconReplacer()
    {
        _customCombos = [.. Assembly.GetAssembly(typeof(CustomComboBase))!.GetTypes()
            .Where(t => !t.IsAbstract && t.BaseType == typeof(CustomComboBase))
            .Select(Activator.CreateInstance)
            .Cast<CustomComboBase>()
            .OrderByDescending(x => x.Preset)];

        _getIconHook = Service.GameInteropProvider.HookFromAddress<GetIconDelegate>(
            ActionManager.Addresses.GetAdjustedActionId.Value, GetIconDetour);

        _isIconReplaceableHook = Service.GameInteropProvider.HookFromAddress<IsIconReplaceableDelegate>(
            Service.Address.IsActionIdReplaceable, IsIconReplaceableDetour);

        Enable();
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

    private unsafe uint GetIconDetour(IntPtr actionManager, uint actionID)
    {
        _actionManager = actionManager;

        if (_disposed || !_enabled || !ThreadSafeCache.LocalPlayerAvailable)
        {
            return OriginalHook(actionID);
        }

        try
        {
            var lastComboMove = ActionManager.Instance()->Combo.Action;

            foreach (CustomComboBase combo in _customCombos)
            {
                if (combo.TryInvoke(actionID, lastComboMove, out var newActionID))
                {
                    return newActionID;
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

    private ulong IsIconReplaceableDetour(uint _)
    {
        return 1;
    }
}
