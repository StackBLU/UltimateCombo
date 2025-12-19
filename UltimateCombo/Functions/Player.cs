using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Objects.SubKinds;
using ECommons.DalamudServices;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using UltimateCombo.Core;
using GameMain = FFXIVClientStructs.FFXIV.Client.Game.GameMain;

namespace UltimateCombo.ComboHelper.Functions;

internal abstract partial class CustomComboFunctions
{
    // Core player reference
    internal static IPlayerCharacter? LocalPlayer => Service.ObjectTable.LocalPlayer;

    internal static ulong LocalPlayerId => LocalPlayer?.GameObjectId ?? 0;

    // Player stats
    internal static uint CurrentJobId => LocalPlayer?.ClassJob.Value.RowId ?? 0;

    internal static byte Level => LocalPlayer?.Level ?? 0;

    internal static uint CurrentHP => LocalPlayer?.CurrentHp ?? 0;

    internal static uint MaxHP => LocalPlayer?.MaxHp ?? 0;

    internal static uint CurrentMP => LocalPlayer?.CurrentMp ?? 0;

    internal static uint MaxMP => LocalPlayer?.MaxMp ?? 0;

    internal static uint ShieldPercentage => LocalPlayer?.ShieldPercentage ?? 0;

    // Player health helpers
    internal static float PlayerHealthPercentageHp()
    {
        return MaxHP > 0 ? (float) CurrentHP / MaxHP * 100 : 0;
    }

    internal static float PlayerHealthPercentageHpPvP()
    {
        return MaxHP > 15000 ? CurrentHP / (MaxHP - 15000f) * 100 : 0;
    }

    // Player conditions
    internal static bool HasCondition(ConditionFlag flag)
    {
        return Service.Condition[flag];
    }

    internal static bool InCombat()
    {
        return Service.Condition[ConditionFlag.InCombat];
    }

    internal static bool IsCasting()
    {
        return Service.Condition[ConditionFlag.Casting];
    }

    // Player companions
    internal static bool HasPetPresent()
    {
        return Service.BuddyList.PetBuddy != null;
    }

    internal static bool HasCompanionPresent()
    {
        return Service.BuddyList.CompanionBuddy != null;
    }

    // Player context
    internal static bool InPvP()
    {
        return GameMain.IsInPvPArea() || GameMain.IsInPvPInstance();
    }

    internal static unsafe bool IsActionUnlocked(uint id)
    {
        var unlockLink = Svc.Data.GetExcelSheet<Lumina.Excel.Sheets.Action>().GetRow(id).UnlockLink.RowId;
        return unlockLink == 0 || UIState.Instance()->IsUnlockLinkUnlockedOrQuestCompleted(unlockLink);
    }
}
