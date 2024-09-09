using FFXIVClientStructs.FFXIV.Client.Game;
using StackCombo.Data;
using StackCombo.Services;
using System.Linq;

namespace StackCombo.ComboHelper.Functions
{
	internal abstract partial class CustomComboFunctions
	{
		public static uint OriginalHook(uint actionID)
		{
			return Service.IconReplacer.OriginalHook(actionID);
		}

		public static bool IsOriginal(uint actionID)
		{
			return Service.IconReplacer.OriginalHook(actionID) == actionID;
		}

		public static bool LevelChecked(uint actionid)
		{
			return LocalPlayer.Level >= GetLevel(actionid) && NoBlockingStatuses(actionid) && IsActionUnlocked(actionid);
		}

		public static bool TraitActionReady(uint traitid)
		{
			return LocalPlayer.Level >= GetTraitLevel(traitid);
		}

		public static string GetActionName(uint id)
		{
			return ActionWatching.GetActionName(id);
		}

		public static int GetLevel(uint id)
		{
			return ActionWatching.GetLevel(id);
		}

		internal static unsafe float GetActionCastTime(uint id)
		{
			return ActionWatching.GetActionCastTime(id);
		}

		public static bool InActionRange(uint id)
		{
			int range = ActionWatching.GetActionRange(id);
			switch (range)
			{
				case -2:
					return false;
				case -1:
					return InMeleeRange();
				case 0:
					{
						float radius = ActionWatching.GetActionEffectRange(id);
						return radius <= 0 || (HasTarget() && GetTargetDistance() <= (radius - 0.5f));
					}
				default:
					return GetTargetDistance() <= range;
			}
		}

		public static int GetTraitLevel(uint id)
		{
			return ActionWatching.GetTraitLevel(id);
		}

		public static bool ActionReady(uint id)
		{
			return LevelChecked(id) && (HasCharges(id) || GetCooldown(id).CooldownTotal <= 3);
		}

		public static bool WasLastAction(uint id)
		{
			return ActionWatching.LastAction == id;
		}

		public static int LastActionCounter()
		{
			return ActionWatching.LastActionUseCount;
		}

		public static bool WasLastWeaponskill(uint id)
		{
			return ActionWatching.LastWeaponskill == id;
		}

		public static bool WasLastSpell(uint id)
		{
			return ActionWatching.LastSpell == id;
		}

		public static bool WasLastAbility(uint? id)
		{
			return ActionWatching.LastAbility == id;
		}

		public static bool IsSpellActive(uint id)
		{
			return Service.Configuration.ActiveBLUSpells.Contains(id);
		}

		public static uint CalcBestAction(uint original, params uint[] actions)
		{
			static (uint ActionID, CooldownData Data) Compare(
				uint original,
				(uint ActionID, CooldownData Data) a1,
				(uint ActionID, CooldownData Data) a2)
			{
				return !a1.Data.IsCooldown && !a2.Data.IsCooldown
					? original == a1.ActionID ? a1 : a2
					: a1.Data.IsCooldown && a2.Data.IsCooldown
					? a1.Data.HasCharges && a2.Data.HasCharges
						? a1.Data.RemainingCharges == a2.Data.RemainingCharges
							? a1.Data.ChargeCooldownRemaining < a2.Data.ChargeCooldownRemaining
								? a1 : a2
							: a1.Data.RemainingCharges > a2.Data.RemainingCharges
							? a1 : a2
						: a1.Data.HasCharges
							? a1.Data.RemainingCharges > 0
													? a1
													: a1.Data.ChargeCooldownRemaining < a2.Data.CooldownRemaining
													? a1 : a2
							: a2.Data.HasCharges
													? a2.Data.RemainingCharges > 0
																			? a2
																			: a2.Data.ChargeCooldownRemaining < a1.Data.CooldownRemaining
																			? a2 : a1
													: a1.Data.CooldownRemaining < a2.Data.CooldownRemaining
																			? a1 : a2
					: a1.Data.IsCooldown ? a2 : a1;
			}

			static (uint ActionID, CooldownData Data) Selector(uint actionID)
			{
				return (actionID, GetCooldown(actionID));
			}

			return actions
				.Select(Selector)
				.Aggregate((a1, a2) => Compare(original, a1, a2))
				.ActionID;
		}

		public static bool CanWeave(uint actionID, double weaveTime = 0.6)
		{
			return (GetCooldown(actionID).CooldownRemaining > weaveTime) || HasSilence() || HasPacification();
		}

		public static bool CanDelayedWeave(uint actionID, double start = 1.25, double end = 0.5)
		{
			return GetCooldown(actionID).CooldownRemaining <= start && GetCooldown(actionID).CooldownRemaining >= end;
		}

		public static unsafe float ComboTimer
		{
			get
			{
				return ActionManager.Instance()->Combo.Timer;
			}
		}

		public static unsafe uint ComboAction
		{
			get
			{
				return ActionManager.Instance()->Combo.Action;
			}
		}

		public static unsafe bool UseAction(ActionType actionType, uint actionID)
		{
			return ActionManager.Instance()->UseAction(actionType, actionID);
		}

		public static unsafe bool UseActionNow(ActionType actionType, uint actionID)
		{
			return ActionManager.Instance()->UseActionLocation(actionType, actionID);
		}

		public static unsafe float AnimationLock()
		{
			return ActionManager.Instance()->AnimationLock;
		}

		public static unsafe bool ActionQueued()
		{
			return ActionManager.Instance()->ActionQueued;
		}

		public static unsafe bool IsActionHighlighted(ActionType actionType, uint actionID)
		{
			return ActionManager.Instance()->IsActionHighlighted(actionType, actionID);
		}

		public static unsafe void AssignBlueMageActionToSlot(int slot, uint actionID)
		{
			ActionManager.Instance()->AssignBlueMageActionToSlot(slot, actionID);
		}

		public static unsafe uint GetActiveBlueMageActionInSlot(int slot)
		{
			return ActionManager.Instance()->GetActiveBlueMageActionInSlot(slot);
		}

		public static unsafe bool SetBlueMageActions(uint* actionArray)
		{
			return ActionManager.Instance()->SetBlueMageActions(actionArray);
		}

		public static unsafe void SwapBlueMageActionSlots(int slotA, int slotB)
		{
			ActionManager.Instance()->SwapBlueMageActionSlots(slotA, slotB);
		}
	}
}