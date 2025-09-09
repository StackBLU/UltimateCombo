using Dalamud.Game.ClientState.Objects.Enums;
using Dalamud.Game.ClientState.Objects.Types;
using ECommons;
using ECommons.DalamudServices;
using Lumina.Excel.Sheets;
using System;
using System.Numerics;
using UltimateCombo.Core;
using ObjectKind = Dalamud.Game.ClientState.Objects.Enums.ObjectKind;

namespace UltimateCombo.ComboHelper.Functions;

internal abstract partial class CustomComboFunctions
{
    // Core target references
    internal static IGameObject? CurrentTarget => Service.TargetManager.Target;
    internal static IGameObject? PlayerTargetObject => LocalPlayer?.TargetObject;

    // Target data helpers
    internal static uint? TargetDataId => CurrentTarget?.DataId;
    internal static IGameObject? TargetOfTarget => CurrentTarget?.TargetObject;

    // Target existence checks
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

    // Target health helpers
    internal static float TargetMaxHp => CurrentTarget is IBattleChara chara ? chara.MaxHp : 0;
    internal static float TargetCurrentHp => CurrentTarget is IBattleChara chara ? chara.CurrentHp : 0;
    internal static float TargetHpPercent => TargetMaxHp > 0 ? TargetCurrentHp / TargetMaxHp * 100 : 0;

    internal static float EnemyHealthMaxHp()
    {
        return TargetMaxHp;
    }

    internal static float EnemyHealthCurrentHp()
    {
        return TargetCurrentHp;
    }

    internal static float GetTargetHPPercent(IGameObject? OurTarget = null)
    {
        if (OurTarget != null)
        {
            return OurTarget is IBattleChara chara ? chara.CurrentHp / chara.MaxHp * 100 : 0;
        }

        return TargetHpPercent;
    }

    // Target state checks
    internal static bool TargetIsCasting => CurrentTarget is IBattleChara chara && chara.IsCasting;
    internal static bool CanInterruptEnemy()
    {
        return CurrentTarget is IBattleChara chara && chara.IsCasting && chara.IsCastInterruptible;
    }

    internal static bool IsTargetOfTarget()
    {
        return CurrentTarget?.TargetObject == LocalPlayer;
    }

    // Distance calculations
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

    // Range helpers
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

    // Positional checks
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
        if (HasBattleTarget() && TargetDataId.HasValue)
        {
            //Striking Dummy
            if (EnemyHealthMaxHp() == 44)
            {
                return true;
            }

            //Is Boss ID
            if (Svc.Data.GetExcelSheet<BNpcBase>().GetRow(TargetDataId.Value).Rank is 2)
            {
                //Raiding
                if (GetPartyMembers().Length == 8 && EnemyHealthMaxHp() > MaxHP * 10)
                {
                    return true;
                }

                //Dungeoning
                if (GetPartyMembers().Length == 4 && EnemyHealthMaxHp() > MaxHP * 5)
                {
                    return true;
                }

                //Side content
                if (EnemyHealthCurrentHp() > MaxHP * 5)
                {
                    return true;
                }
            }

            //Is NOT Boss ID
            if (Svc.Data.GetExcelSheet<BNpcBase>().GetRow(TargetDataId.Value).Rank is not 2)
            {
                //Raiding - Ads
                if (GetPartyMembers().Length == 8 && EnemyHealthMaxHp() > MaxHP * 20)
                {
                    return true;
                }

                //Dungeoning - Ads - Checks current hp instead of max
                if (GetPartyMembers().Length is > 1 and < 8 && EnemyHealthCurrentHp() > MaxHP * 10)
                {
                    return true;
                }
            }
        }

        return false;
    }

    internal static unsafe bool TargetWorthDoT()
    {
        if (HasBattleTarget() && TargetDataId.HasValue)
        {
            //Striking Dummy
            if (EnemyHealthMaxHp() == 44)
            {
                return true;
            }

            //Is Boss ID
            if (Svc.Data.GetExcelSheet<BNpcBase>().GetRow(TargetDataId.Value).Rank is 2)
            {
                //Raiding
                if (GetPartyMembers().Length == 8 && EnemyHealthCurrentHp() > MaxHP * 20)
                {
                    return true;
                }

                //Dungeoning
                if (GetPartyMembers().Length == 4 && EnemyHealthCurrentHp() > MaxHP * 10)
                {
                    return true;
                }

                //Side content
                if (EnemyHealthCurrentHp() > MaxHP * 5)
                {
                    return true;
                }
            }

            //Is NOT Boss ID
            if (Svc.Data.GetExcelSheet<BNpcBase>().GetRow(TargetDataId.Value).Rank is not 2)
            {
                //Raiding - Ads
                if (GetPartyMembers().Length == 8 && EnemyHealthCurrentHp() > MaxHP * 20)
                {
                    return true;
                }

                //Dungeoning - Ads - Checks current hp instead of max
                if (GetPartyMembers().Length is > 1 and < 8 && EnemyHealthCurrentHp() > MaxHP * 10)
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
