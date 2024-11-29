﻿using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.Game.InstanceContent;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using System;
using UltimateCombo.Data;
using UltimateCombo.Services;

namespace UltimateCombo.ComboHelper.Functions
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

		public static bool MaxActionRange(uint id)
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
						return radius <= 0 || (HasTarget() && GetTargetDistance() >= (radius - 2f));
					}
				default:
					return GetTargetDistance() >= range - 2;
			}
		}

		public static int GetTraitLevel(uint id)
		{
			return ActionWatching.GetTraitLevel(id);
		}

		public static bool ActionReady(uint id)
		{
			return LevelChecked(id) && (HasCharges(id) || GetCooldown(id).CooldownTotal <= 5);
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

		public static bool CanWeave(uint actionID, double weaveTime = 0.6)
		{
			if (Service.Configuration.DisableTripleWeaving)
			{
				return (GetCooldown(actionID).CooldownRemaining >= weaveTime && !ActionWatching.HasDoubleWeaved())
				|| HasSilence() || HasPacification();
			}

			return GetCooldown(actionID).CooldownRemaining >= weaveTime
				|| HasSilence() || HasPacification();
		}

		public static bool CanDelayedWeave(uint actionID, double weaveTime = 0.6, double weaveStart = 0.8)
		{
			return (GetCooldown(actionID).CooldownRemaining <= weaveStart && GetCooldown(actionID).CooldownRemaining >= weaveTime)
				|| HasSilence() || HasPacification();
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

		public static unsafe bool AutoSheatheState()
		{
			return UIState.Instance()->WeaponState.AutoSheatheState;
		}

		public static unsafe float AutoSheathTimer()
		{
			return UIState.Instance()->WeaponState.AutoSheathTimer;
		}

		public static unsafe bool IsAutoAttacking()
		{
			return UIState.Instance()->WeaponState.IsAutoAttacking;
		}

		public static unsafe bool IsUnsheathed()
		{
			return UIState.Instance()->WeaponState.IsUnsheathed;
		}

		public static unsafe float SheatheCooldown()
		{
			return UIState.Instance()->WeaponState.SheatheCooldown;
		}

		public static unsafe int FindHolsterItem(uint itemID)
		{
			return PublicContentBozja.GetState()->HolsterActions.IndexOf((byte)itemID);
		}

		public static unsafe bool HasHolsterItem(uint itemID)
		{
			return FindHolsterItem(itemID) >= 0 ? true : false;
		}

		public static unsafe bool UseFromHolster(uint holsterIndex, uint slot)
		{
			return PublicContentBozja.GetInstance()->UseFromHolster(holsterIndex, slot);
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