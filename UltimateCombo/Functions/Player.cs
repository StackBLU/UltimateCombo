using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Objects.SubKinds;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using UltimateCombo.Core;
using UltimateCombo.Data;
using GameMain = FFXIVClientStructs.FFXIV.Client.Game.GameMain;

namespace UltimateCombo.ComboHelper.Functions;

internal abstract partial class CustomComboFunctions
{
    internal static IPlayerCharacter? LocalPlayer => Service.ObjectTable.LocalPlayer;
    internal static ulong LocalPlayerId => LocalPlayer?.GameObjectId ?? 0;

    internal static uint CurrentJobId => LocalPlayer?.ClassJob.Value.RowId ?? 0;
    internal static byte Level => LocalPlayer?.Level ?? 0;
    internal static uint CurrentHP => LocalPlayer?.CurrentHp ?? 0;
    internal static uint MaxHP => LocalPlayer?.MaxHp ?? 0;
    internal static uint CurrentMP => LocalPlayer?.CurrentMp ?? 0;
    internal static uint MaxMP => LocalPlayer?.MaxMp ?? 0;
    internal static uint ShieldPercentage => LocalPlayer?.ShieldPercentage ?? 0;

    internal static float PlayerHealthPercentageHp()
    {
        IPlayerCharacter? player = LocalPlayer;
        return player is { MaxHp: > 0 }
            ? (float) player.CurrentHp / player.MaxHp * 100f
            : 0;
    }

    internal static float PlayerHealthPercentageHpPvP()
    {
        IPlayerCharacter? player = LocalPlayer;
        if (player == null || player.MaxHp <= 15000)
        {
            return 0;
        }

        return player.CurrentHp / (player.MaxHp - 15000f) * 100f;
    }

    internal static bool HasCondition(ConditionFlag flag)
    {
        return Service.Condition[flag];
    }

    internal static bool InCombat()
    {
        return HasCondition(ConditionFlag.InCombat);
    }

    internal static bool IsCasting()
    {
        return HasCondition(ConditionFlag.Casting);
    }

    internal static bool HasPetPresent()
    {
        return Service.BuddyList.PetBuddy != null;
    }

    internal static bool HasCompanionPresent()
    {
        return Service.BuddyList.CompanionBuddy != null;
    }

    internal static bool InPvP()
    {
        return GameMain.IsInPvPArea() || GameMain.IsInPvPInstance();
    }

    internal static unsafe bool IsActionUnlocked(uint actionID)
    {
        if (!ActionWatching.ActionSheet.TryGetValue(actionID, out Lumina.Excel.Sheets.Action action))
        {
            return false;
        }

        var unlockLink = action.UnlockLink.RowId;
        if (unlockLink == 0)
        {
            return true;
        }

        UIState* uiState = UIState.Instance();
        return uiState != null && uiState->IsUnlockLinkUnlockedOrQuestCompleted(unlockLink);
    }
}
