using Dalamud.Game.ClientState.Objects.Enums;
using Dalamud.Game.ClientState.Objects.Types;
using ECommons;
using ECommons.DalamudServices;
using Lumina.Excel.Sheets;
using System;
using System.Numerics;
using UltimateCombo.Combos.General;
using UltimateCombo.Core;
using ObjectKind = Dalamud.Game.ClientState.Objects.Enums.ObjectKind;

namespace UltimateCombo.ComboHelper.Functions;

internal abstract partial class CustomComboFunctions
{
    internal static IGameObject? CurrentTarget => Service.TargetManager.Target;
    internal static IGameObject? PlayerTargetObject => LocalPlayer?.TargetObject;
    internal static uint? TargetDataId => CurrentTarget?.DataId;
    internal static IGameObject? TargetOfTarget => CurrentTarget?.TargetObject;

    internal static bool HasTarget()
    {
        return CurrentTarget is not null;
    }

    internal static bool HasBattleTarget()
    {
        return CurrentTarget is not null and IBattleNpc { BattleNpcKind: BattleNpcSubKind.Enemy };
    }

    internal static bool HasFriendlyTarget()
    {
        return CurrentTarget is not null && CurrentTarget.ObjectKind is ObjectKind.Player;
    }

    internal static string EnemyName()
    {
        return CurrentTarget is IBattleChara chara ? chara.Name.ToString() : "none";
    }

    internal static float EnemyMaxHP()
    {
        return CurrentTarget is IBattleChara chara ? chara.MaxHp : 0;
    }

    internal static float EnemyCurrentHP()
    {
        return CurrentTarget is IBattleChara chara ? chara.CurrentHp : 0;
    }

    internal static float EnemyPercentHP()
    {
        return EnemyCurrentHP() / EnemyMaxHP() * 100;
    }

    internal static bool TargetIsCasting()
    {
        return CurrentTarget is IBattleChara chara && chara.IsCasting;
    }

    internal static bool CanInterruptEnemy()
    {
        return CurrentTarget is IBattleChara chara && chara.IsCasting && chara.IsCastInterruptible;
    }

    internal static bool IsTargetOfTarget()
    {
        return CurrentTarget?.TargetObject == LocalPlayer;
    }

    internal static float GetTargetDistance()
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

        var position = new Vector2(chara.Position.X, chara.Position.Z);
        var selfPosition = new Vector2(LocalPlayer.Position.X, LocalPlayer.Position.Z);

        return Math.Max(0, Vector2.Distance(position, selfPosition) - chara.HitboxRadius - LocalPlayer.HitboxRadius);
    }

    internal static bool InRange(float range)
    {
        return GetTargetDistance() <= range;
    }

    internal static bool OutOfRange(double range)
    {
        return GetTargetDistance() > range;
    }

    internal static bool InMeleeRange()
    {
        return PlayerTargetObject != null && InRange(3);
    }

    internal static bool OutOfMeleeRange()
    {
        return PlayerTargetObject != null && OutOfRange(Service.Configuration.RangedAttackRange);
    }

    internal static bool InMeleeRangeNoMovement()
    {
        return PlayerTargetObject != null && InRange(0);
    }

    internal static bool OnTargetsRear()
    {
        if (CurrentTarget is null || LocalPlayer is null)
        {
            return false;
        }

        if (CurrentTarget is not IBattleChara || CurrentTarget.ObjectKind != ObjectKind.BattleNpc)
        {
            return false;
        }

        var angle = PositionalMath.AngleXZ(CurrentTarget.Position, LocalPlayer.Position) - CurrentTarget.Rotation;
        var regionDegrees = PositionalMath.Degrees(angle);
        if (regionDegrees < 0)
        {
            regionDegrees = 360 + regionDegrees;
        }

        return regionDegrees is >= 135 and <= 225;
    }

    internal static bool OnTargetsFlank()
    {
        if (CurrentTarget is null || LocalPlayer is null)
        {
            return false;
        }

        if (CurrentTarget is not IBattleChara || CurrentTarget.ObjectKind != ObjectKind.BattleNpc)
        {
            return false;
        }

        var angle = PositionalMath.AngleXZ(CurrentTarget.Position, LocalPlayer.Position) - CurrentTarget.Rotation;
        var regionDegrees = PositionalMath.Degrees(angle);
        if (regionDegrees < 0)
        {
            regionDegrees = 360 + regionDegrees;
        }

        return regionDegrees is (>= 45 and <= 135) or (>= 225 and <= 315);
    }

    internal static byte EnemyRank()
    {
        return TargetDataId.HasValue ? Svc.Data.GetExcelSheet<BNpcBase>().GetRow(TargetDataId.Value).Rank : (byte) 0;
    }

    internal static bool TargetIsBoss()
    {
        if (HasBattleTarget() && TargetDataId.HasValue)
        {
            if (EnemyName() == "Striking Dummy" || Service.Configuration.IgnoreGCDChecks || HasEffect(Common.Buffs.EpicEcho))
            {
                return true;
            }

            if (EnemyCurrentHP() == 1)
            {
                return false;
            }

            if (EnemyRank() == 2)
            {
                return true;
            }

            if (EnemyRank() != 2)
            {
                if (PartyMemberLength() == 1 && EnemyMaxHP() > MaxHP * 10 && EnemyPercentHP() > 15)
                {
                    return true;
                }

                if (PartyMemberLength() is 2 or 3 && EnemyMaxHP() > MaxHP * 10)
                {
                    return true;
                }

                if (PartyMemberLength() == 4 && EnemyMaxHP() > MaxHP * 10 && EnemyPercentHP() > 50)
                {
                    return true;
                }

                if (PartyMemberLength() is 5 or 6 or 7 && EnemyMaxHP() > MaxHP * 10)
                {
                    return true;
                }

                if (PartyMemberLength() == 8 && EnemyMaxHP() > MaxHP * 25 && EnemyPercentHP() > 50)
                {
                    return true;
                }
            }
        }

        return false;
    }

    internal static bool TargetWorthDoT()
    {
        if (HasBattleTarget() && TargetDataId.HasValue)
        {
            if (EnemyName() == "Striking Dummy")
            {
                return true;
            }

            if (EnemyCurrentHP() == 1)
            {
                return false;
            }

            if (EnemyRank() == 2)
            {
                if (PartyMemberLength() == 4 && EnemyPercentHP() > 10)
                {
                    return true;
                }

                if (PartyMemberLength() != 4 && EnemyPercentHP() > 5)
                {
                    return true;
                }
            }

            if (EnemyRank() != 2)
            {
                if (PartyMemberLength() == 1 && EnemyMaxHP() > MaxHP * 5 && EnemyPercentHP() > 75)
                {
                    return true;
                }

                if (PartyMemberLength() is 2 or 3 && EnemyMaxHP() > MaxHP * 10 && EnemyPercentHP() > 50)
                {
                    return true;
                }

                if (PartyMemberLength() == 4 && EnemyMaxHP() > MaxHP * 10 && EnemyPercentHP() > 50)
                {
                    return true;
                }

                if (PartyMemberLength() is 5 or 6 or 7 && EnemyMaxHP() > MaxHP * 10 && EnemyPercentHP() > 75)
                {
                    return true;
                }

                if (PartyMemberLength() == 8 && EnemyMaxHP() > MaxHP * 8 && EnemyPercentHP() > 75)
                {
                    return true;
                }
            }
        }

        return false;
    }

    internal static bool TargetCloseToDeath()
    {
        if (HasBattleTarget() && TargetDataId.HasValue)
        {
            if (EnemyName() == "Striking Dummy")
            {
                return true;
            }

            if (EnemyCurrentHP() == 1)
            {
                return false;
            }

            if (EnemyRank() == 2)
            {
                if (PartyMemberLength() == 4 && EnemyPercentHP() < 10)
                {
                    return true;
                }

                if (PartyMemberLength() != 4 && EnemyPercentHP() < 3)
                {
                    return true;
                }
            }
        }

        return false;
    }

    internal static bool TargetNeedsPositionals()
    {
        if (!HasBattleTarget() || !TargetDataId.HasValue)
        {
            return false;
        }

        if (HasEffectAny(3808))
        {
            return false;
        }

        return Svc.Data.Excel.GetSheet<BNpcBase>().TryGetFirst(x => x.RowId == TargetDataId.Value, out BNpcBase bnpc) && !bnpc.IsOmnidirectional;
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
}
