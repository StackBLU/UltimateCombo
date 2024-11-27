using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Game.ClientState.Statuses;
using UltimateCombo.Data;
using UltimateCombo.Services;

namespace UltimateCombo.ComboHelper.Functions
{
	internal abstract partial class CustomComboFunctions
	{
		public static bool HasEffect(ushort effectID)
		{
			return FindEffect(effectID) is not null;
		}

		public static byte GetBuffStacks(ushort effectId)
		{
			Status? eff = FindEffect(effectId);
			return eff?.StackCount ?? 0;
		}

		public static float GetBuffRemainingTime(ushort effectId)
		{
			Status? eff = FindEffect(effectId);
			return eff?.RemainingTime ?? 0;
		}

		public static float GetTargetsBuffRemainingTime(ushort effectId)
		{
			Status? eff = FindTargetEffectAny(effectId);
			return eff?.RemainingTime ?? 0;
		}

		public static Status? FindEffect(ushort effectID)
		{
			return FindEffect(effectID, LocalPlayer, LocalPlayer?.GameObjectId);
		}

		public static bool TargetHasEffect(ushort effectID)
		{
			return FindTargetEffect(effectID) is not null;
		}

		public static bool TargetsTargetHasEffect(ushort effectID)
		{
			return FindTargetsTargetEffect(effectID) is not null;
		}

		public static Status? FindTargetEffect(ushort effectID)
		{
			return FindEffect(effectID, CurrentTarget, LocalPlayer?.GameObjectId);
		}

		public static byte FindTargetEffectStacks(ushort effectID)
		{
			Status? effect = FindEffect(effectID, CurrentTarget, LocalPlayer?.GameObjectId);
			if (effect == null)
			{
				return 0;
			}

			return effect.StackCount;
		}

		public static Status? FindTargetsTargetEffect(ushort effectID)
		{
			return FindEffect(effectID, CurrentTarget.TargetObject, null);
		}

		public static float GetDebuffRemainingTime(ushort effectId)
		{
			Status? eff = FindTargetEffect(effectId);
			return eff?.RemainingTime ?? 0;
		}

		public static bool HasEffectAny(ushort effectID)
		{
			return FindEffectAny(effectID) is not null;
		}

		public static Status? FindEffectAny(ushort effectID)
		{
			return FindEffect(effectID, LocalPlayer, null);
		}

		public static bool TargetHasEffectAny(ushort effectID)
		{
			return FindTargetEffectAny(effectID) is not null;
		}

		public static Status? FindTargetEffectAny(ushort effectID)
		{
			return FindEffect(effectID, CurrentTarget, null);
		}

		public static Status? FindEffect(ushort effectID, IGameObject? obj, ulong? sourceID)
		{
			return Service.ComboCache.GetStatus(effectID, obj, sourceID);
		}

		public static Status? FindEffectOnMember(ushort effectID, IGameObject? obj)
		{
			return Service.ComboCache.GetStatus(effectID, obj, null);
		}

		public static string GetStatusName(uint id)
		{
			return ActionWatching.GetStatusName(id);
		}

		public static bool HasSilence()
		{
			foreach (uint status in ActionWatching.GetStatusesByName(ActionWatching.GetStatusName(7)))
			{
				if (HasEffectAny((ushort)status))
				{
					return true;
				}
			}

			return false;
		}

		public static bool HasPacification()
		{
			foreach (uint status in ActionWatching.GetStatusesByName(ActionWatching.GetStatusName(6)))
			{
				if (HasEffectAny((ushort)status))
				{
					return true;
				}
			}

			return false;
		}

		public static bool HasAmnesia()
		{
			foreach (uint status in ActionWatching.GetStatusesByName(ActionWatching.GetStatusName(5)))
			{
				if (HasEffectAny((ushort)status))
				{
					return true;
				}
			}

			return false;
		}

		public static bool TargetHasDamageDown(IGameObject? target)
		{
			foreach (uint status in ActionWatching.GetStatusesByName(GetStatusName(62)))
			{
				if (FindEffectOnMember((ushort)status, target) is not null)
				{
					return true;
				}
			}

			return false;
		}

		public static bool TargetHasRezWeakness(IGameObject? target)
		{
			foreach (uint status in ActionWatching.GetStatusesByName(GetStatusName(43)))
			{
				if (FindEffectOnMember((ushort)status, target) is not null)
				{
					return true;
				}
			}
			foreach (uint status in ActionWatching.GetStatusesByName(GetStatusName(44)))
			{
				if (FindEffectOnMember((ushort)status, target) is not null)
				{
					return true;
				}
			}

			return false;
		}


		public static bool HasCleansableDebuff(IGameObject? OurTarget = null)
		{
			OurTarget ??= CurrentTarget;
			if (HasFriendlyTarget(OurTarget) && (OurTarget is IBattleChara chara))
			{
				foreach (Status status in chara.StatusList)
				{
					if (ActionWatching.StatusSheet.TryGetValue(status.StatusId, out Lumina.Excel.Sheets.Status statusItem) && statusItem.CanDispel)
					{
						return true;
					}
				}
			}
			return false;
		}

		public static bool NoBlockingStatuses(uint actionId)
		{
			switch (ActionWatching.GetAttackType(actionId))
			{
				case ActionWatching.ActionAttackType.Weaponskill:
					if (HasPacification())
					{
						return false;
					}

					return true;
				case ActionWatching.ActionAttackType.Spell:
					if (HasSilence())
					{
						return false;
					}

					return true;
				case ActionWatching.ActionAttackType.Ability:
					if (HasAmnesia())
					{
						return false;
					}

					return true;

			}

			return true;
		}
	}
}