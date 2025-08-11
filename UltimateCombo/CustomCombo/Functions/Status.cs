using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Game.ClientState.Statuses;
using System;
using System.Linq;
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

		public static ushort GetBuffStacks(ushort effectId)
		{
			Status? eff = FindEffect(effectId);
			return eff?.Param ?? 0;
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
			return FindEffect(effectID, SafeLocalPlayer, SafeLocalPlayer.GameObjectId);
		}

		public static Status SafeFindEffect(ushort effectID)
		{
			Status? effect = FindEffect(effectID);
			return effect ?? throw new InvalidOperationException($"Effect {effectID} not found");
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
			return FindEffect(effectID, SafeCurrentTarget, SafeLocalPlayer.GameObjectId);
		}

		public static ushort FindTargetEffectStacks(ushort effectID)
		{
			Status? effect = FindEffect(effectID, SafeCurrentTarget, SafeLocalPlayer.GameObjectId);

			if (effect == null)
			{
				return 0;
			}

			return effect.Param;
		}

		public static Status? FindTargetsTargetEffect(ushort effectID)
		{
			return FindEffect(effectID, SafeCurrentTarget.TargetObject, null);
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
			return ActionWatching.GetStatusesByName(ActionWatching.GetStatusName(7))
				?.Any(status => HasEffectAny((ushort) status)) ?? false;
		}

		public static bool HasPacification()
		{
			return ActionWatching.GetStatusesByName(ActionWatching.GetStatusName(6))
				?.Any(status => HasEffectAny((ushort) status)) ?? false;
		}

		public static bool HasAmnesia()
		{
			return ActionWatching.GetStatusesByName(ActionWatching.GetStatusName(5))
				?.Any(status => HasEffectAny((ushort) status)) ?? false;
		}
	}
}
