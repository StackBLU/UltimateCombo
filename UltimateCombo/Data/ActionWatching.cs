using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Hooking;
using ECommons.DalamudServices;
using ECommons.GameFunctions;
using FFXIVClientStructs.FFXIV.Client.Game;
using Lumina.Excel.Sheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Core;

namespace UltimateCombo.Data;

internal static class ActionWatching
{
    internal static Dictionary<uint, Lumina.Excel.Sheets.Action> ActionSheet = Service.DataManager.GetExcelSheet<Lumina.Excel.Sheets.Action>()!
        .Where(i => i.RowId is not 7)
        .ToDictionary(i => i.RowId, i => i);

    internal static Dictionary<uint, Lumina.Excel.Sheets.Status> StatusSheet = Service.DataManager.GetExcelSheet<Lumina.Excel.Sheets.Status>()!
        .ToDictionary(i => i.RowId, i => i);

    internal static Dictionary<uint, Trait> TraitSheet = Service.DataManager.GetExcelSheet<Trait>()!
        .ToDictionary(i => i.RowId, i => i);

    internal static Dictionary<uint, BNpcBase> BNpcSheet = Service.DataManager.GetExcelSheet<BNpcBase>()!
        .ToDictionary(i => i.RowId, i => i);

    private static readonly Dictionary<string, List<uint>> StatusCache = [];
    internal static readonly List<uint> CombatActions = [];

    private delegate void ReceiveActionEffectDelegate(ulong sourceObjectId, IntPtr sourceActor, IntPtr position, IntPtr effectHeader, IntPtr effectArray, IntPtr effectTrail);
    private static readonly Hook<ReceiveActionEffectDelegate>? ReceiveActionEffectHook;


    private static void ReceiveActionEffectDetour(ulong sourceObjectId, IntPtr sourceActor, IntPtr position, IntPtr effectHeader, IntPtr effectArray, IntPtr effectTrail)
    {
        ReceiveActionEffectHook!.Original(sourceObjectId, sourceActor, position, effectHeader, effectArray, effectTrail);
        ActionEffectHeader header = Marshal.PtrToStructure<ActionEffectHeader>(effectHeader);

        if (ActionType is 13 or 2)
        {
            return;
        }

        if (header.ActionId != 7 &&
            header.ActionId != 8 &&
            sourceObjectId == Service.ObjectTable.LocalPlayer?.GameObjectId)
        {
            TimeLastActionUsed = DateTime.Now;
            LastActionUseCount++;
            if (header.ActionId != LastAction)
            {
                LastActionUseCount = 1;
            }

            LastAction = header.ActionId;

            if (ActionSheet.TryGetValue(header.ActionId, out Lumina.Excel.Sheets.Action sheet))
            {
                switch (sheet.ActionCategory.Value.RowId)
                {
                    case 2:
                        LastSpell = header.ActionId;
                        LastGCD = header.ActionId;
                        break;
                    case 3:
                        LastWeaponskill = header.ActionId;
                        LastGCD = header.ActionId;
                        break;
                    case 4:
                        LastAbility = header.ActionId;
                        break;
                    default:
                        break;
                }
            }

            CombatActions.Add(header.ActionId);

            if (Service.Configuration.EnabledOutputLog)
            {
                OutputLog();
            }
        }
    }

    private delegate void SendActionDelegate(ulong targetObjectId, byte actionType, uint actionId, ushort sequence, long a5, long a6, long a7, long a8, long a9);
    private static readonly Hook<SendActionDelegate>? SendActionHook;

    private static unsafe void SendActionDetour(ulong targetObjectId, byte actionType, uint actionId, ushort sequence, long a5, long a6, long a7, long a8, long a9)
    {
        try
        {
            SendActionHook!.Original(targetObjectId, actionType, actionId, sequence, a5, a6, a7, a8, a9);
            TimeLastActionUsed = DateTime.Now;
            ActionType = actionType;
        }
        catch (Exception ex)
        {
            Service.PluginLog.Error(ex, "SendActionDetour");
            SendActionHook!.Original(targetObjectId, actionType, actionId, sequence, a5, a6, a7, a8, a9);
        }
    }

    internal static unsafe bool OutOfRange(uint actionId, IGameObject source, IGameObject target)
    {
        return ActionManager.GetActionInRangeOrLoS(actionId, source.Struct(), target.Struct()) is 566;
    }

    internal static uint WhichOfTheseActionsWasLast(params uint[] actions)
    {
        if (CombatActions.Count == 0)
        {
            return 0;
        }

        var currentLastIndex = 0;
        foreach (var action in actions)
        {
            if (CombatActions.Any(x => x == action))
            {
                var index = CombatActions.LastIndexOf(action);

                if (index > currentLastIndex)
                {
                    currentLastIndex = index;
                }
            }
        }

        return CombatActions[currentLastIndex];
    }

    internal static int HowManyTimesUsedAfterAnotherAction(uint lastUsedIDToCheck, uint idToCheckAgainst)
    {
        if (CombatActions.Count < 2)
        {
            return 0;
        }

        if (WhichOfTheseActionsWasLast(lastUsedIDToCheck, idToCheckAgainst) != lastUsedIDToCheck)
        {
            return 0;
        }

        var startingIndex = CombatActions.LastIndexOf(idToCheckAgainst);
        if (startingIndex == -1)
        {
            return 0;
        }

        var count = 0;
        for (var i = startingIndex + 1; i < CombatActions.Count; i++)
        {
            if (CombatActions[i] == lastUsedIDToCheck)
            {
                count++;
            }
        }

        return count;
    }

    internal static int NumberOfGcdsUsed => CombatActions.Count(x => GetAttackType(x) is ActionAttackType.Weaponskill or ActionAttackType.Spell);

    internal static bool HasDoubleWeaved()
    {
        if (CombatActions.Count < 2)
        {
            return false;
        }

        var lastAction = CombatActions.Last();
        var secondLastAction = CombatActions[^2];

        return GetAttackType(lastAction) == GetAttackType(secondLastAction) && GetAttackType(lastAction) == ActionAttackType.Ability;
    }

    internal static uint LastAction { get; set; } = 0;
    internal static int LastActionUseCount { get; set; } = 0;
    internal static uint ActionType { get; set; } = 0;
    internal static uint LastWeaponskill { get; set; } = 0;
    internal static uint LastAbility { get; set; } = 0;
    internal static uint LastSpell { get; set; } = 0;
    internal static uint LastGCD { get; set; } = 0;

    internal static TimeSpan TimeSinceLastAction => DateTime.Now - TimeLastActionUsed;

    private static DateTime TimeLastActionUsed { get; set; } = DateTime.Now;

    internal static void OutputLog()
    {
        Service.ChatGui.Print($"You just used: {GetActionName(LastAction)} x{LastActionUseCount}");
    }

    internal static void Dispose()
    {
        ReceiveActionEffectHook?.Dispose();
        SendActionHook?.Dispose();
    }


    static unsafe ActionWatching()
    {
        ReceiveActionEffectHook ??= Service.GameInteropProvider.HookFromSignature<ReceiveActionEffectDelegate>("E8 ?? ?? ?? ?? 48 8B 8D ?? ?? ?? ?? 48 33 CC E8 ?? ?? ?? ?? 48 81 C4 00 05 00 00", ReceiveActionEffectDetour);
        SendActionHook ??= Service.GameInteropProvider.HookFromSignature<SendActionDelegate>("48 89 5C 24 ?? 48 89 6C 24 ?? 48 89 74 24 ?? 57 48 81 EC ?? ?? ?? ?? 48 8B 05 ?? ?? ?? ?? 48 33 C4 48 89 84 24 ?? ?? ?? ?? 48 8B E9 41 0F B7 D9", SendActionDetour);
    }

    internal static void Enable()
    {
        ReceiveActionEffectHook?.Enable();
        SendActionHook?.Enable();
        Svc.Condition.ConditionChange += ResetActions;
    }

    internal static void Disable()
    {
        ReceiveActionEffectHook?.Disable();
        SendActionHook?.Disable();
        Svc.Condition.ConditionChange -= ResetActions;
    }

    private static void ResetActions(ConditionFlag flag, bool value)
    {
        if (flag is ConditionFlag.InCombat && !value)
        {
            _ = CheckInCombat();
        }

        if (flag is ConditionFlag.BeingMoved or ConditionFlag.BetweenAreas or ConditionFlag.Mounted
            or ConditionFlag.OccupiedInCutSceneEvent or ConditionFlag.Unconscious or ConditionFlag.WatchingCutscene)
        {
            CombatActions.Clear();
            LastAbility = 0;
            LastAction = 0;
            LastWeaponskill = 0;
            LastSpell = 0;
        }
    }

    internal static async Task CheckInCombat()
    {
        await Task.Delay(5000);
        if (!CustomComboFunctions.InCombat())
        {
            CombatActions.Clear();
            LastAbility = 0;
            LastAction = 0;
            LastWeaponskill = 0;
            LastSpell = 0;
        }
    }

    internal static int GetLevel(uint id)
    {
        if (ActionSheet.TryGetValue(id, out Lumina.Excel.Sheets.Action action) && action.ClassJobCategory.IsValid)
        {
            return action.ClassJobLevel;
        }

        return 255;
    }

    internal static float GetActionCastTime(uint id)
    {
        if (ActionSheet.TryGetValue(id, out Lumina.Excel.Sheets.Action action))
        {
            return action.Cast100ms / 10f;
        }

        return 0;
    }

    internal static int GetActionRange(uint id)
    {
        if (ActionSheet.TryGetValue(id, out Lumina.Excel.Sheets.Action action))
        {
            return action.Range;
        }

        return -2;
    }

    internal static int GetActionEffectRange(uint id)
    {
        if (ActionSheet.TryGetValue(id, out Lumina.Excel.Sheets.Action action))
        {
            return action.EffectRange;
        }

        return -1;
    }

    internal static int GetTraitLevel(uint id)
    {
        if (TraitSheet.TryGetValue(id, out Trait trait))
        {
            return trait.Level;
        }

        return 255;
    }

    internal static string GetActionName(uint id)
    {
        if (ActionSheet.TryGetValue(id, out Lumina.Excel.Sheets.Action action))
        {
            return action.Name.ToString();
        }

        return "Unknown Ability";
    }

    internal static string GetBLUIndex(uint id)
    {
        var aozKey = Svc.Data.GetExcelSheet<AozAction>()!.First(x => x.Action.RowId == id).RowId;
        var index = Svc.Data.GetExcelSheet<AozActionTransient>().GetRow(aozKey).Number;

        return $"#{index} ";
    }

    internal static string GetStatusName(uint id)
    {
        if (StatusSheet.TryGetValue(id, out Lumina.Excel.Sheets.Status status))
        {
            return status.Name.ToString();
        }

        return "Unknown Status";
    }

    internal static List<uint>? GetStatusesByName(string status)
    {
        if (StatusCache.TryGetValue(status, out List<uint>? list))
        {
            return list;
        }

        var added = StatusCache.TryAdd(
            status,
            [.. StatusSheet.Where(x => x.Value.Name.ToString().Equals(status, StringComparison.CurrentCultureIgnoreCase)).Select(x => x.Key)]);

        if (added)
        {
            return StatusCache[status];
        }

        return null;
    }

    internal static ActionAttackType GetAttackType(uint id)
    {
        if (!ActionSheet.TryGetValue(id, out Lumina.Excel.Sheets.Action action))
        {
            return ActionAttackType.Unknown;
        }

        return action.ActionCategory.Value.RowId switch
        {
            2 => ActionAttackType.Spell,
            3 => ActionAttackType.Weaponskill,
            4 => ActionAttackType.Ability,
            _ => ActionAttackType.Unknown,
        };
    }

    internal enum ActionAttackType
    {
        Ability,
        Spell,
        Weaponskill,
        Unknown
    }
}

internal static unsafe class ActionManagerHelper
{
    private static readonly IntPtr ActionMgrPtr;
    internal static IntPtr FpUseAction => ActionManager.Addresses.UseAction.Value;
    internal static IntPtr FpUseActionLocation => ActionManager.Addresses.UseActionLocation.Value;
    internal static IntPtr CheckActionResources => ActionManager.Addresses.CheckActionResources.Value;

    internal static ushort CurrentSeq
    {
        get
        {
            if (ActionMgrPtr != IntPtr.Zero)
            {
                return (ushort) Marshal.ReadInt16(ActionMgrPtr + 0x110);
            }

            return 0;
        }
    }

    internal static ushort LastRecievedSeq
    {
        get
        {
            if (ActionMgrPtr != IntPtr.Zero)
            {
                return (ushort) Marshal.ReadInt16(ActionMgrPtr + 0x112);
            }

            return 0;
        }
    }

    internal static bool IsCasting
    {
        get
        {
            if (ActionMgrPtr != IntPtr.Zero && Marshal.ReadByte(ActionMgrPtr + 0x28) != 0)
            {
                return true;
            }

            return false;
        }
    }

    internal static uint CastingActionId
    {
        get
        {
            if (ActionMgrPtr != IntPtr.Zero)
            {
                return (uint) Marshal.ReadInt32(ActionMgrPtr + 0x24);
            }

            return 0u;
        }
    }

    internal static uint CastTargetObjectId
    {
        get
        {
            if (ActionMgrPtr != IntPtr.Zero)
            {
                return (uint) Marshal.ReadInt32(ActionMgrPtr + 0x38);
            }

            return 0u;
        }
    }

    static ActionManagerHelper()
    {
        ActionMgrPtr = (IntPtr) ActionManager.Instance();
    }
}

[StructLayout(LayoutKind.Explicit)]
internal struct ActionEffectHeader
{
    [FieldOffset(0x0)] internal long TargetObjectId;
    [FieldOffset(0x8)] internal uint ActionId;
    [FieldOffset(0x14)] internal uint UnkObjectId;
    [FieldOffset(0x18)] internal ushort Sequence;
    [FieldOffset(0x1A)] internal ushort Unk_1A;
}
