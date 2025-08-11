using Dalamud.Game.ClientState.Objects.Enums;
using Dalamud.Game.ClientState.Objects.Types;
using ECommons;
using ECommons.DalamudServices;
using Lumina.Excel.Sheets;
using System;
using System.Numerics;
using UltimateCombo.Data;
using UltimateCombo.Services;
using ObjectKind = Dalamud.Game.ClientState.Objects.Enums.ObjectKind;

namespace UltimateCombo.ComboHelper.Functions
{
	internal abstract partial class CustomComboFunctions
	{
		public static IGameObject? CurrentTarget => Service.TargetManager.Target;
		public static IGameObject SafeCurrentTarget => CurrentTarget ?? throw new InvalidOperationException("SafeCurrentTarget is not available");

		public static bool HasTarget()
		{
			return CurrentTarget is not null;
		}

		public static float GetTargetDistance()
		{
			if (CurrentTarget is null || LocalPlayer is null)
			{
				return 0;
			}

			if (CurrentTarget is not IBattleChara chara)
			{
				return 0;
			}

			if (CurrentTarget.GameObjectId == SafeLocalPlayer.GameObjectId)
			{
				return 0;
			}

			Vector2 position = new(chara.Position.X, chara.Position.Z);
			Vector2 selfPosition = new(SafeLocalPlayer.Position.X, SafeLocalPlayer.Position.Z);

			return Math.Max(0, Vector2.Distance(position, selfPosition) - chara.HitboxRadius - SafeLocalPlayer.HitboxRadius);
		}

		public static bool InMeleeRange()
		{
			if (SafeLocalPlayer.TargetObject == null)
			{
				return false;
			}

			var distance = GetTargetDistance();

			return distance <= 3;
		}

		public static bool OutOfMeleeRange()
		{
			if (SafeLocalPlayer.TargetObject == null)
			{
				return false;
			}

			var distance = GetTargetDistance();

			return distance > Service.Configuration.RangedAttackRange;
		}

		public static bool InMeleeRangeNoMovement()
		{
			if (SafeLocalPlayer.TargetObject == null)
			{
				return false;
			}

			var distance = GetTargetDistance();

			return distance <= 0;
		}

		public static float GetTargetHPPercent(IGameObject? OurTarget = null)
		{
			if (OurTarget is null)
			{
				OurTarget = SafeCurrentTarget;
				if (OurTarget is null)
				{
					return 0;
				}
			}

			if (OurTarget is not IBattleChara chara)
			{
				return 0;
			}

			return (float) chara.CurrentHp / chara.MaxHp * 100;
		}

		public static float EnemyHealthMaxHp()
		{
			if (SafeCurrentTarget is null)
			{
				return 0;
			}

			if (SafeCurrentTarget is not IBattleChara chara)
			{
				return 0;
			}

			return chara.MaxHp;
		}

		public static float EnemyHealthCurrentHp()
		{
			if (SafeCurrentTarget is null)
			{
				return 0;
			}

			if (SafeCurrentTarget is not IBattleChara chara)
			{
				return 0;
			}

			return chara.CurrentHp;
		}

		public static float PlayerHealthPercentageHp()
		{
			return (float) ((float) SafeLocalPlayer.CurrentHp / SafeLocalPlayer.MaxHp * 100);
		}

		public static float PlayerHealthPercentageHpPvP()
		{
			return (float) ((float) SafeLocalPlayer.CurrentHp / (SafeLocalPlayer.MaxHp - 15000) * 100);
		}

		public static bool HasBattleTarget()
		{
			return SafeCurrentTarget is not null and IBattleNpc { BattleNpcKind: BattleNpcSubKind.Enemy };
		}

		public static bool HasFriendlyTarget()
		{
			return SafeCurrentTarget is not null && SafeCurrentTarget.ObjectKind is ObjectKind.Player;
		}

		public static bool CanInterruptEnemy()
		{
			return SafeCurrentTarget is not null && SafeCurrentTarget is IBattleChara chara && chara.IsCasting && chara.IsCastInterruptible;
		}

		public static void SetTarget(IGameObject? target)
		{
			Service.TargetManager.Target = target;
		}

		public static bool IsInRange(IGameObject? target)
		{
			return target != null && target.YalmDistanceX < 30;
		}

		public static bool TargetNeedsPositionals()
		{
			if (!HasBattleTarget())
			{
				return false;
			}

			if (TargetHasEffectAny(3808))
			{
				return false; // Directional Disregard Effect (Patch 7.01)
			}

			return Svc.Data.Excel.GetSheet<BNpcBase>().TryGetFirst(x => x.RowId == SafeCurrentTarget.DataId, out BNpcBase bnpc) && !bnpc.IsOmnidirectional;
		}

		public static void TargetObject(IGameObject? target)
		{
			if (IsInRange(target))
			{
				SetTarget(target);
			}
		}

		public enum TargetType
		{
			Target,
			SoftTarget,
			FocusTarget,
			UITarget,
			FieldTarget,
			TargetsTarget,
			Self,
			LastTarget,
			LastEnemy,
			LastAttacker,
			P2,
			P3,
			P4,
			P5,
			P6,
			P7,
			P8
		}

		public static float AngleToTarget()
		{
			if (SafeCurrentTarget is null || LocalPlayer is null)
			{
				return 0;
			}

			if (SafeCurrentTarget is not IBattleChara || SafeCurrentTarget.ObjectKind != ObjectKind.BattleNpc)
			{
				return 0;
			}

			var angle = PositionalMath.AngleXZ(SafeCurrentTarget.Position, LocalPlayer.Position) - SafeCurrentTarget.Rotation;

			var regionDegrees = PositionalMath.Degrees(angle);

			if (regionDegrees < 0)
			{
				regionDegrees = 360 + regionDegrees;
			}

			if (regionDegrees is >= 45 and <= 135)
			{
				return 1;
			}

			if (regionDegrees is >= 135 and <= 225)
			{
				return 2;
			}

			if (regionDegrees is >= 225 and <= 315)
			{
				return 3;
			}

			if (regionDegrees is >= 315 or <= 45)
			{
				return 4;
			}

			return 0;
		}

		public static bool OnTargetsRear()
		{
			if (SafeCurrentTarget is null || LocalPlayer is null)
			{
				return false;
			}

			if (SafeCurrentTarget is not IBattleChara || SafeCurrentTarget.ObjectKind != ObjectKind.BattleNpc)
			{
				return false;
			}

			var angle = PositionalMath.AngleXZ(SafeCurrentTarget.Position, LocalPlayer.Position) - SafeCurrentTarget.Rotation;

			var regionDegrees = PositionalMath.Degrees(angle);
			if (regionDegrees < 0)
			{
				regionDegrees = 360 + regionDegrees;
			}

			return regionDegrees is >= 135 and <= 225;
		}

		public static bool OnTargetsFlank()
		{
			if (SafeCurrentTarget is null || LocalPlayer is null)
			{
				return false;
			}

			if (SafeCurrentTarget is not IBattleChara || SafeCurrentTarget.ObjectKind != ObjectKind.BattleNpc)
			{
				return false;
			}

			var angle = PositionalMath.AngleXZ(SafeCurrentTarget.Position, LocalPlayer.Position) - SafeCurrentTarget.Rotation;

			var regionDegrees = PositionalMath.Degrees(angle);
			if (regionDegrees < 0)
			{
				regionDegrees = 360 + regionDegrees;
			}

			return regionDegrees is (>= 45 and <= 135) or (>= 225 and <= 315);
		}

		internal static class PositionalMath
		{
			internal static float Radians(float degrees)
			{
				return (float) Math.PI * degrees / 180.0f;
			}

			internal static double Degrees(float radians)
			{
				return 180 / Math.PI * radians;
			}

			internal static float AngleXZ(Vector3 a, Vector3 b)
			{
				return (float) Math.Atan2(b.X - a.X, b.Z - a.Z);
			}
		}

		internal static unsafe bool OutOfRange(uint actionID, IGameObject target)
		{
			return ActionWatching.OutOfRange(actionID, Service.ClientState.LocalPlayer!, target);
		}

		internal static unsafe byte? GetMobType(IGameObject target)
		{
			if (HasBattleTarget())
			{
				return Svc.Data.GetExcelSheet<BNpcBase>()?.GetRow(target.DataId).Rank;
			}

			return (byte?) 0;
		}

		internal static unsafe bool TargetIsBoss()
		{
			if (HasBattleTarget())
			{
				if ((Svc.Data.GetExcelSheet<BNpcBase>().GetRow(SafeCurrentTarget.DataId).Rank is 2
					|| EnemyHealthMaxHp() == 44
					|| EnemyHealthMaxHp() > SafeLocalPlayer.MaxHp * 10) && EnemyHealthCurrentHp() > 1)
				{
					return true;
				}
			}

			return false;
		}
	}
}
