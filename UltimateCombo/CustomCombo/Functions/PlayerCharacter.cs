using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Objects.SubKinds;
using ECommons.DalamudServices;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using Lumina.Excel.GeneratedSheets;
using UltimateCombo.Services;
using GameMain = FFXIVClientStructs.FFXIV.Client.Game.GameMain;

namespace UltimateCombo.ComboHelper.Functions
{
	internal abstract partial class CustomComboFunctions
	{
		public static IPlayerCharacter? LocalPlayer
		{
			get
			{
				return Service.ClientState.LocalPlayer;
			}
		}

		public static bool HasCondition(ConditionFlag flag)
		{
			return Service.Condition[flag];
		}

		public static bool InCombat()
		{
			return Service.Condition[ConditionFlag.InCombat];
		}

		public static bool IsCasting()
		{
			return Service.Condition[ConditionFlag.Casting];
		}

		public static bool HasPetPresent()
		{
			return Service.BuddyList.PetBuddy != null;
		}

		public static bool HasCompanionPresent()
		{
			return Service.BuddyList.CompanionBuddy != null;
		}

		public static bool InPvP()
		{
			return GameMain.IsInPvPArea() || GameMain.IsInPvPInstance();
		}

		public static unsafe bool IsActionUnlocked(uint id)
		{
			uint unlockLink = Svc.Data.GetExcelSheet<Action>().GetRow(id).UnlockLink;
			return unlockLink == 0 || UIState.Instance()->IsUnlockLinkUnlockedOrQuestCompleted(unlockLink);
		}
	}
}
