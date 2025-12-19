using Dalamud.Hooking;
using FFXIVClientStructs.FFXIV.Client.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UltimateCombo.Core;

public sealed partial class IconReplacer : IDisposable
{
    private readonly List<CustomComboBase> _customCombos;
    private readonly Hook<IsIconReplaceableDelegate> _isIconReplaceableHook;
    private readonly Hook<GetIconDelegate> _getIconHook;
    private IntPtr _actionManager = IntPtr.Zero;
    private readonly IntPtr _module = IntPtr.Zero;


    internal IconReplacer()
    {
        _customCombos = [.. Assembly.GetAssembly(typeof(CustomComboBase))!.GetTypes()
                .Where(t => !t.IsAbstract && t.BaseType == typeof(CustomComboBase))
                .Select(Activator.CreateInstance)
                .Cast<CustomComboBase>()
                .OrderByDescending(x => x.Preset)];

        _getIconHook = Service.GameInteropProvider.HookFromAddress<GetIconDelegate>(ActionManager.Addresses.GetAdjustedActionId.Value, GetIconDetour);
        _isIconReplaceableHook = Service.GameInteropProvider.HookFromAddress<IsIconReplaceableDelegate>(Service.Address.IsActionIdReplaceable, IsIconReplaceableDetour);
        _getIconHook.Enable();
        _isIconReplaceableHook.Enable();
    }

    private delegate ulong IsIconReplaceableDelegate(uint actionID);
    private delegate uint GetIconDelegate(IntPtr actionManager, uint actionID);

    internal void Dispose()
    {
        _getIconHook?.Dispose();
        _isIconReplaceableHook?.Dispose();
    }

    internal uint OriginalHook(uint actionID)
    {
        return _getIconHook.Original(_actionManager, actionID);
    }

    private unsafe uint GetIconDetour(IntPtr actionManager, uint actionID)
    {
        _actionManager = actionManager;
        try
        {
            if (Service.ObjectTable.LocalPlayer == null)
            {
                return OriginalHook(actionID);
            }
            var lastComboMove = ActionManager.Instance()->Combo.Action;
            var comboTime = ActionManager.Instance()->Combo.Timer;
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

    private ulong IsIconReplaceableDetour(uint actionID)
    {
        return 1;
    }

    void IDisposable.Dispose()
    {
        Dispose();
    }
}
