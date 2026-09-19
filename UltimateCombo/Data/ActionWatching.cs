using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Hooking;
using ECommons.DalamudServices;
using ECommons.GameFunctions;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using Lumina.Excel.Sheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Core;

namespace UltimateCombo.Data;

internal static class ActionWatching
{
    internal static Dictionary<uint, Lumina.Excel.Sheets.Action> ActionSheet = Service.DataManager.GetExcelSheet<Lumina.Excel.Sheets.Action>()
        .Where(i => i.RowId is not 7)
        .ToDictionary(i => i.RowId, i => i);

    internal static Dictionary<uint, Lumina.Excel.Sheets.Status> StatusSheet = Service.DataManager.GetExcelSheet<Lumina.Excel.Sheets.Status>()
        .ToDictionary(i => i.RowId, i => i);

    internal static Dictionary<uint, Trait> TraitSheet = Service.DataManager.GetExcelSheet<Trait>()
        .ToDictionary(i => i.RowId, i => i);

    internal static Dictionary<uint, BNpcBase> BNpcSheet = Service.DataManager.GetExcelSheet<BNpcBase>()
        .ToDictionary(i => i.RowId, i => i);

    private static readonly Dictionary<string, List<uint>> StatusCache = [];
    private static readonly Dictionary<uint, string> BlueMageIndexCache = [];
    private static readonly Dictionary<uint, ActionAttackType> AttackTypeCache = ActionSheet.ToDictionary(
        pair => pair.Key,
        pair => pair.Value.ActionCategory.Value.RowId switch
        {
            2 => ActionAttackType.Spell,
            3 => ActionAttackType.Weaponskill,
            4 => ActionAttackType.Ability,
            _ => ActionAttackType.Unknown,
        });

    internal static readonly List<uint> CombatActions = [];

    private static Hook<ActionEffectHandler.Delegates.Receive>? _receiveActionEffectHook;

    private static bool _enabled;
    private static int _numberOfGcdsUsed;
    private static int _recordedCombatActionCount;


    private static unsafe void ReceiveActionEffectDetour(uint sourceObjectId, Character* sourceActor, Vector3* position,
        ActionEffectHandler.Header* effectHeader, ActionEffectHandler.TargetEffects* effectArray, GameObjectId* effectTrail)
    {
        _receiveActionEffectHook!.Original(sourceObjectId, sourceActor, position, effectHeader, effectArray, effectTrail);
        ActionEffectHandler.Header header = *effectHeader;

        if (ActionType is 13 or 2)
        {
            return;
        }

        if (header.ActionId != 7 &&
            header.ActionId != 8 &&
            sourceObjectId == Service.ObjectTable.LocalPlayer?.GameObjectId)
        {
            SynchronizeGcdCount();
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
                        _numberOfGcdsUsed++;
                        break;
                    case 3:
                        LastWeaponskill = header.ActionId;
                        LastGCD = header.ActionId;
                        _numberOfGcdsUsed++;
                        break;
                    case 4:
                        LastAbility = header.ActionId;
                        break;
                    default:
                        break;
                }
            }

            CombatActions.Add(header.ActionId);
            _recordedCombatActionCount = CombatActions.Count;

            if (Service.Configuration.EnabledOutputLog)
            {
                OutputLog();
            }
        }
    }

    private delegate void SendActionDelegate(ulong targetObjectId, byte actionType, uint actionId, ushort sequence, long a5, long a6, long a7, long a8, long a9);
    private static Hook<SendActionDelegate>? _sendActionHook;

    private static void SendActionDetour(ulong targetObjectId, byte actionType, uint actionId, ushort sequence, long a5, long a6, long a7, long a8, long a9)
    {
        try
        {
            _sendActionHook!.Original(targetObjectId, actionType, actionId, sequence, a5, a6, a7, a8, a9);
            TimeLastActionUsed = DateTime.Now;
            ActionType = actionType;
        }
        catch (Exception ex)
        {
            Service.PluginLog.Error(ex, "SendActionDetour");
            _sendActionHook!.Original(targetObjectId, actionType, actionId, sequence, a5, a6, a7, a8, a9);
        }
    }

    internal static unsafe bool OutOfRange(uint actionId, IGameObject source, IGameObject target)
    {
        return ActionManager.GetActionInRangeOrLoS(actionId, source.Struct(), target.Struct()) is 566;
    }

    internal static uint WhichOfTheseActionsWasLast(params uint[] actions)
    {
        for (var i = CombatActions.Count - 1; i >= 0; i--)
        {
            if (Array.IndexOf(actions, CombatActions[i]) >= 0)
            {
                return CombatActions[i];
            }
        }

        return 0;
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

    internal static int NumberOfGcdsUsed
    {
        get
        {
            SynchronizeGcdCount();
            return _numberOfGcdsUsed;
        }
    }

    private static void SynchronizeGcdCount()
    {
        if (CombatActions.Count == _recordedCombatActionCount)
        {
            return;
        }

        _numberOfGcdsUsed = CombatActions.Count(actionID =>
            GetAttackType(actionID) is ActionAttackType.Spell or ActionAttackType.Weaponskill);
        _recordedCombatActionCount = CombatActions.Count;
    }

    internal static bool HasDoubleWeaved()
    {
        if (CombatActions.Count < 2)
        {
            return false;
        }

        var lastAction = CombatActions[^1];
        var secondLastAction = CombatActions[^2];
        var lastAttackType = GetAttackType(lastAction);
        var secondLastAttackType = GetAttackType(secondLastAction);

        return lastAttackType == ActionAttackType.Ability && secondLastAttackType == ActionAttackType.Ability;
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

    private static unsafe void EnsureHooks()
    {
        // Create only once per process, but allow recreation after Dispose by setting fields null.
        _receiveActionEffectHook ??= Service.GameInteropProvider.HookFromAddress<ActionEffectHandler.Delegates.Receive>(
            ActionEffectHandler.MemberFunctionPointers.Receive,
            ReceiveActionEffectDetour);

        _sendActionHook ??= Service.GameInteropProvider.HookFromSignature<SendActionDelegate>(
            "48 89 5C 24 ?? 48 89 6C 24 ?? 48 89 74 24 ?? 57 48 81 EC ?? ?? ?? ?? 48 8B 05 ?? ?? ?? ?? 48 33 C4 48 89 84 24 ?? ?? ?? ?? 48 8B E9 41 0F B7 D9",
            SendActionDetour);
    }

    internal static void Enable()
    {
        if (_enabled)
        {
            return;
        }

        _enabled = true;

        try
        {
            EnsureHooks();
            _receiveActionEffectHook?.Enable();
            _sendActionHook?.Enable();
            Svc.Condition.ConditionChange += ResetActions;
        }
        catch
        {
            Dispose();
            throw;
        }
    }

    internal static void Disable()
    {
        if (!_enabled)
        {
            return;
        }

        _enabled = false;

        Svc.Condition.ConditionChange -= ResetActions;
        try { _receiveActionEffectHook?.Disable(); } catch { }
        try { _sendActionHook?.Disable(); } catch { }
    }

    internal static void Dispose()
    {
        Disable();

        try { _receiveActionEffectHook?.Dispose(); } catch { }
        _receiveActionEffectHook = null;

        try { _sendActionHook?.Dispose(); } catch { }
        _sendActionHook = null;
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
            ResetActionHistory();
        }
    }

    internal static async Task CheckInCombat()
    {
        await Task.Delay(5000);
        if (!CustomComboFunctions.InCombat())
        {
            ResetActionHistory();
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
        if (BlueMageIndexCache.TryGetValue(id, out string? cachedIndex))
        {
            return cachedIndex;
        }

        var aozKey = Svc.Data.GetExcelSheet<AozAction>().First(x => x.Action.RowId == id).RowId;
        var index = Svc.Data.GetExcelSheet<AozActionTransient>().GetRow(aozKey).Number;
        var displayIndex = $"#{index} ";

        BlueMageIndexCache[id] = displayIndex;
        return displayIndex;
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
        return AttackTypeCache.TryGetValue(id, out ActionAttackType attackType)
            ? attackType
            : ActionAttackType.Unknown;
    }

    private static void ResetActionHistory()
    {
        CombatActions.Clear();
        _numberOfGcdsUsed = 0;
        _recordedCombatActionCount = 0;
        LastAbility = 0;
        LastAction = 0;
        LastWeaponskill = 0;
        LastSpell = 0;
    }

    internal enum ActionAttackType
    {
        Ability,
        Spell,
        Weaponskill,
        Unknown
    }
}

internal static class ActionManagerHelper
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

    static unsafe ActionManagerHelper()
    {
        ActionMgrPtr = (IntPtr) ActionManager.Instance();
    }
}
