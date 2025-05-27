using Dalamud.Game.ClientState.Objects;
using Dalamud.Game.ClientState.Objects.Enums;
using Dalamud.Game.ClientState.Objects.Types;
using ECommons;
using ECommons.DalamudServices;
using FFXIVClientStructs.FFXIV.Client.System.Framework;
using Lumina.Excel.Sheets;
using System;
using System.Linq;
using System.Numerics;
using UltimateCombo.Data;
using UltimateCombo.Services;
using ObjectKind = Dalamud.Game.ClientState.Objects.Enums.ObjectKind;
using StructsObject = FFXIVClientStructs.FFXIV.Client.Game.Object;

namespace UltimateCombo.ComboHelper.Functions
{
	internal abstract partial class CustomComboFunctions
	{
		public static IGameObject? CurrentTarget
		{
			get
			{
				return Service.TargetManager.Target;
			}
		}

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

			if (CurrentTarget.GameObjectId == LocalPlayer.GameObjectId)
			{
				return 0;
			}

			Vector2 position = new(chara.Position.X, chara.Position.Z);
			Vector2 selfPosition = new(LocalPlayer.Position.X, LocalPlayer.Position.Z);

			return Math.Max(0, Vector2.Distance(position, selfPosition) - chara.HitboxRadius - LocalPlayer.HitboxRadius);
		}

		public static bool InMeleeRange()
		{
			if (LocalPlayer.TargetObject == null)
			{
				return false;
			}

			float distance = GetTargetDistance();

			return distance <= 3;
		}

		public static bool OutOfMeleeRange()
		{
			if (LocalPlayer.TargetObject == null)
			{
				return false;
			}

			float distance = GetTargetDistance();

			return distance > Service.Configuration.RangedAttackRange;
		}

		public static bool InMeleeRangeNoMovement()
		{
			if (LocalPlayer.TargetObject == null)
			{
				return false;
			}

			float distance = GetTargetDistance();

			return distance <= 0;
		}

		public static float GetTargetHPPercent(IGameObject? OurTarget = null)
		{
			if (OurTarget is null)
			{
				OurTarget = CurrentTarget;
				if (OurTarget is null)
				{
					return 0;
				}
			}

			return OurTarget is not IBattleChara chara
				? 0
				: (float)chara.CurrentHp / chara.MaxHp * 100;
		}

		public static float EnemyHealthMaxHp()
		{
			return CurrentTarget is null ? 0 : CurrentTarget is not IBattleChara chara ? 0 : (float)chara.MaxHp;
		}

		public static float EnemyHealthCurrentHp()
		{
			return CurrentTarget is null ? 0 : CurrentTarget is not IBattleChara chara ? 0 : (float)chara.CurrentHp;
		}

		public static float PlayerHealthPercentageHp()
		{
			return (float)LocalPlayer.CurrentHp / LocalPlayer.MaxHp * 100;
		}

		public static bool HasBattleTarget()
		{
			return CurrentTarget is IBattleNpc { BattleNpcKind: BattleNpcSubKind.Enemy or (BattleNpcSubKind)1 };
		}

		public static bool HasFriendlyTarget(IGameObject? OurTarget = null)
		{
			if (OurTarget is null)
			{
				OurTarget = CurrentTarget;
				if (OurTarget is null)
				{
					return false;
				}
			}

			if (OurTarget.ObjectKind is ObjectKind.Player)
			{
				return true;
			}
			return OurTarget is IBattleNpc
				   && (OurTarget as IBattleNpc).BattleNpcKind is not BattleNpcSubKind.Enemy and not (BattleNpcSubKind)1;
		}

		public static unsafe IGameObject? GetHealTarget(bool checkMOPartyUI = false, bool restrictToMouseover = false)
		{
			IGameObject? healTarget = null;
			ITargetManager tm = Service.TargetManager;

			if (HasFriendlyTarget(tm.SoftTarget))
			{
				healTarget = tm.SoftTarget;
			}

			if (healTarget is null && HasFriendlyTarget(CurrentTarget) && !restrictToMouseover)
			{
				healTarget = CurrentTarget;
			}
			if (checkMOPartyUI)
			{
				StructsObject.GameObject* t = Framework.Instance()->GetUIModule()->GetPronounModule()->UiMouseOverTarget;
				if (t != null && t->GetGameObjectId().ObjectId != 0)
				{
					IGameObject? uiTarget = Service.ObjectTable.Where(x => x.GameObjectId == t->GetGameObjectId().ObjectId).FirstOrDefault();
					if (uiTarget != null && HasFriendlyTarget(uiTarget))
					{
						healTarget = uiTarget;
					}

					if (restrictToMouseover)
					{
						return healTarget;
					}
				}

				if (restrictToMouseover)
				{
					return healTarget;
				}
			}
			healTarget ??= LocalPlayer;
			return healTarget;
		}

		public static bool CanInterruptEnemy()
		{
			return CurrentTarget is not null && CurrentTarget is IBattleChara chara && chara.IsCasting && chara.IsCastInterruptible;
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

			if (Svc.Data.Excel.GetSheet<BNpcBase>().TryGetFirst(x => x.RowId == CurrentTarget.DataId, out BNpcBase bnpc) && !bnpc.IsOmnidirectional)
			{
				return true;
			}

			return false;
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
			if (CurrentTarget is null || LocalPlayer is null)
			{
				return 0;
			}

			if (CurrentTarget is not IBattleChara || CurrentTarget.ObjectKind != ObjectKind.BattleNpc)
			{
				return 0;
			}

			float angle = PositionalMath.AngleXZ(CurrentTarget.Position, LocalPlayer.Position) - CurrentTarget.Rotation;

			double regionDegrees = PositionalMath.Degrees(angle);
			if (regionDegrees < 0)
			{
				regionDegrees = 360 + regionDegrees;
			}

			return regionDegrees is >= 45 and <= 135
				? 1
				: regionDegrees is >= 135 and <= 225 ? 2 : regionDegrees is >= 225 and <= 315 ? 3 : regionDegrees is >= 315 or <= 45 ? 4 : 0;
		}

		public static bool OnTargetsRear()
		{
			if (CurrentTarget is null || LocalPlayer is null)
			{
				return false;
			}

			if (CurrentTarget is not IBattleChara || CurrentTarget.ObjectKind != ObjectKind.BattleNpc)
			{
				return false;
			}

			float angle = PositionalMath.AngleXZ(CurrentTarget.Position, LocalPlayer.Position) - CurrentTarget.Rotation;

			double regionDegrees = PositionalMath.Degrees(angle);
			if (regionDegrees < 0)
			{
				regionDegrees = 360 + regionDegrees;
			}

			return regionDegrees is >= 135 and <= 225;
		}

		public static bool OnTargetsFlank()
		{
			if (CurrentTarget is null || LocalPlayer is null)
			{
				return false;
			}

			if (CurrentTarget is not IBattleChara || CurrentTarget.ObjectKind != ObjectKind.BattleNpc)
			{
				return false;
			}

			float angle = PositionalMath.AngleXZ(CurrentTarget.Position, LocalPlayer.Position) - CurrentTarget.Rotation;

			double regionDegrees = PositionalMath.Degrees(angle);
			if (regionDegrees < 0)
			{
				regionDegrees = 360 + regionDegrees;
			}

			if (regionDegrees is >= 45 and <= 135)
			{
				return true;
			}
			return regionDegrees is >= 225 and <= 315;
		}

		internal static class PositionalMath
		{
			internal static float Radians(float degrees)
			{
				return (float)Math.PI * degrees / 180.0f;
			}

			internal static double Degrees(float radians)
			{
				return 180 / Math.PI * radians;
			}

			internal static float AngleXZ(Vector3 a, Vector3 b)
			{
				return (float)Math.Atan2(b.X - a.X, b.Z - a.Z);
			}
		}

		internal unsafe bool OutOfRange(uint actionID, IGameObject target)
		{
			return ActionWatching.OutOfRange(actionID, Service.ClientState.LocalPlayer!, target);
		}

		internal static unsafe byte? GetMobType(IGameObject target)
		{
			if (HasBattleTarget())
			{
				return Svc.Data.GetExcelSheet<BNpcBase>()?.GetRow(target.DataId).Rank;
			}

			return 0;
		}

		internal static unsafe bool TargetIsBoss()
		{
			if (HasBattleTarget())
			{
				if (Svc.Data.GetExcelSheet<BNpcBase>().GetRow(CurrentTarget.DataId).Rank is 2 || EnemyHealthMaxHp() == 44)
				{
					return true;
				}
			}

			return false;
		}
	}
}
