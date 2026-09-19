
using Dalamud.Game.ClientState.Objects.Enums;
using Dalamud.Game.ClientState.Objects.Types;
using Lumina.Excel.Sheets;
using System;
using System.Numerics;
using UltimateCombo.Combos.General;
using UltimateCombo.Core;
using UltimateCombo.Data;

namespace UltimateCombo.ComboHelper.Functions;

internal abstract partial class CustomComboFunctions
{
    internal static IGameObject? CurrentTarget => Service.TargetManager.Target;
    internal static IGameObject? PlayerTargetObject => LocalPlayer?.TargetObject;
    internal static uint? TargetDataId => CurrentTarget?.BaseId;
    internal static IGameObject? TargetOfTarget => CurrentTarget?.TargetObject;

    internal static bool HasTarget()
    {
        return CurrentTarget is not null;
    }

    internal static bool HasBattleTarget()
    {
        return CurrentTarget is not null and IBattleNpc { BattleNpcKind: BattleNpcSubKind.Combatant };
    }

    internal static bool HasFriendlyTarget()
    {
        return CurrentTarget is not null && CurrentTarget.ObjectKind is ObjectKind.Pc;
    }

    internal static string EnemyName()
    {
        return CurrentTarget is IBattleChara chara ? chara.Name.ToString() : "none";
    }

    internal static float EnemyLevel()
    {
        return CurrentTarget is IBattleChara chara ? chara.Level : 0;
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
        return CurrentTarget is IBattleChara character && character.MaxHp > 0
            ? (float) character.CurrentHp / character.MaxHp * 100f
            : 0;
    }

    internal static bool TargetIsCasting()
    {
        return CurrentTarget is IBattleChara chara && chara.IsCasting;
    }

    internal static bool CanInterrupt()
    {
        return CurrentTarget is IBattleChara chara && chara.IsCasting && chara.IsCastInterruptible && chara.CurrentCastTime >= chara.TotalCastTime / 3;
    }

    internal static bool ShouldInterrupt()
    {
        return CurrentTarget is IBattleChara chara && chara.IsCasting && chara.IsCastInterruptible && chara.TotalCastTime > 2;
    }

    internal static bool IsTargetOfTarget()
    {
        return CurrentTarget?.TargetObject == LocalPlayer;
    }

    internal static float GetTargetDistanceCenterToCenter()
    {
        IGameObject? target = CurrentTarget;
        IGameObject? player = LocalPlayer;

        if (target is not IBattleChara character || player == null || target.GameObjectId == player.GameObjectId)
        {
            return 0;
        }

        var position = new Vector2(character.Position.X, character.Position.Z);
        var selfPosition = new Vector2(player.Position.X, player.Position.Z);

        return Vector2.Distance(position, selfPosition);
    }

    internal static float GetTargetDistanceHitboxToHitbox()
    {
        IGameObject? target = CurrentTarget;
        IGameObject? player = LocalPlayer;

        if (target is not IBattleChara character || player == null || target.GameObjectId == player.GameObjectId)
        {
            return 0;
        }

        var position = new Vector2(character.Position.X, character.Position.Z);
        var selfPosition = new Vector2(player.Position.X, player.Position.Z);

        return MathF.Max(0, Vector2.Distance(position, selfPosition) - character.HitboxRadius - player.HitboxRadius);
    }

    internal static bool InMeleeRange()
    {
        return CurrentTarget is IBattleChara && GetTargetDistanceHitboxToHitbox() <= 3f;
    }

    internal static bool InMeleeRangeNoMovement()
    {
        return CurrentTarget is IBattleChara && GetTargetDistanceHitboxToHitbox() <= 0;
    }

    internal static bool ProjectileThresholdDistance()
    {
        return CurrentTarget is IBattleChara
            && GetTargetDistanceHitboxToHitbox() >= Service.Configuration.RangedAttackRange;
    }

    internal static bool OnTargetsRear()
    {
        IGameObject? target = CurrentTarget;
        IGameObject? player = LocalPlayer;

        if (target is not IBattleChara || target.ObjectKind != ObjectKind.BattleNpc || player == null)
        {
            return false;
        }

        var regionDegrees = GetPositionDegrees(target, player);
        return regionDegrees is >= 135 and <= 225;
    }

    internal static bool OnTargetsFlank()
    {
        IGameObject? target = CurrentTarget;
        IGameObject? player = LocalPlayer;

        if (target is not IBattleChara || target.ObjectKind != ObjectKind.BattleNpc || player == null)
        {
            return false;
        }

        var regionDegrees = GetPositionDegrees(target, player);
        return regionDegrees is (>= 45 and <= 135) or (>= 225 and <= 315);
    }

    internal static byte EnemyRank()
    {
        return TargetDataId.HasValue && ActionWatching.BNpcSheet.TryGetValue(TargetDataId.Value, out BNpcBase enemy)
            ? enemy.Rank
            : (byte) 0;
    }

    internal static bool LevelIgnoreGCD()
    {
        if (CurrentTarget is not IBattleChara target || LocalPlayer is not { } player)
        {
            return false;
        }

        if (target.Name.ToString() == "Striking Dummy" || target.CurrentHp == 1)
        {
            return false;
        }

        return player.Level == 100 && player.Level - target.Level >= 10;
    }

    internal static bool TargetIsBoss()
    {
        if (CurrentTarget is IBattleNpc { BattleNpcKind: BattleNpcSubKind.Combatant } target)
        {
            if (target.Name.ToString() == "Striking Dummy" || Service.Configuration.IgnoreGCDChecks || LevelIgnoreGCD() || HasEffect(Common.Buffs.EpicEcho))
            {
                return true;
            }

            if (target.CurrentHp == 1)
            {
                return false;
            }

            var maxHP = target.MaxHp;
            var percentHP = maxHP > 0 ? (float) target.CurrentHp / maxHP * 100f : 0;
            var partySize = EffectivePartySize();
            var rank = ActionWatching.BNpcSheet.TryGetValue(target.BaseId, out BNpcBase enemy) ? enemy.Rank : (byte) 0;

            if (HasEffect(BLU.Buffs.BasicInstinct) && maxHP > MaxHP * 10 && percentHP > 10)
            {
                return true;
            }

            if (rank == 2)
            {
                return true;
            }

            if (rank != 2)
            {
                if (partySize == 1 && maxHP > MaxHP * 10 && percentHP > 15)
                {
                    return true;
                }

                if (partySize is 2 or 3 && maxHP > MaxHP * 10)
                {
                    return true;
                }

                if (partySize == 4 && maxHP > MaxHP * 10 && percentHP > 50)
                {
                    return true;
                }

                if (partySize is 5 or 6 or 7 && maxHP > MaxHP * 10)
                {
                    return true;
                }

                if (partySize == 8 && maxHP > MaxHP * 25 && percentHP > 20)
                {
                    return true;
                }
            }
        }

        return false;
    }

    internal static bool TargetWorthDoT()
    {
        if (CurrentTarget is IBattleNpc { BattleNpcKind: BattleNpcSubKind.Combatant } target)
        {
            if (target.Name.ToString() == "Striking Dummy")
            {
                return true;
            }

            if (target.CurrentHp == 1)
            {
                return false;
            }

            var maxHP = target.MaxHp;
            var percentHP = maxHP > 0 ? (float) target.CurrentHp / maxHP * 100f : 0;
            var partySize = EffectivePartySize();
            var rank = ActionWatching.BNpcSheet.TryGetValue(target.BaseId, out BNpcBase enemy) ? enemy.Rank : (byte) 0;

            if (HasEffect(BLU.Buffs.BasicInstinct) && maxHP > MaxHP * 10 && percentHP > 10)
            {
                return true;
            }

            if (rank == 2)
            {
                if (partySize == 4 && percentHP > 10)
                {
                    return true;
                }

                if (partySize != 4 && percentHP > 5)
                {
                    return true;
                }
            }

            if (rank != 2)
            {
                if (partySize == 1 && maxHP > MaxHP * 5 && percentHP > 75)
                {
                    return true;
                }

                if (partySize is 2 or 3 && maxHP > MaxHP * 10 && percentHP > 50)
                {
                    return true;
                }

                if (partySize == 4 && maxHP > MaxHP * 10 && percentHP > 50)
                {
                    return true;
                }

                if (partySize is 5 or 6 or 7 && maxHP > MaxHP * 10 && percentHP > 75)
                {
                    return true;
                }

                if (partySize == 8 && maxHP > MaxHP * 8 && percentHP > 75)
                {
                    return true;
                }
            }
        }

        return false;
    }

    internal static bool BossAlmostDead()
    {
        if (CurrentTarget is IBattleNpc { BattleNpcKind: BattleNpcSubKind.Combatant } target)
        {
            if (target.Name.ToString() == "Striking Dummy")
            {
                return false;
            }

            if (target.CurrentHp == 1)
            {
                return false;
            }

            var rank = ActionWatching.BNpcSheet.TryGetValue(target.BaseId, out BNpcBase enemy) ? enemy.Rank : (byte) 0;
            if (rank == 2)
            {
                var percentHP = target.MaxHp > 0 ? (float) target.CurrentHp / target.MaxHp * 100f : 0;
                var partySize = EffectivePartySize();

                if (partySize == 4 && percentHP < 10)
                {
                    return true;
                }

                if (partySize != 4 && percentHP < 5)
                {
                    return true;
                }
            }
        }

        return false;
    }

    internal static bool TargetNeedsPositionals()
    {
        if (CurrentTarget is not IBattleNpc { BattleNpcKind: BattleNpcSubKind.Combatant } target)
        {
            return false;
        }

        if (HasEffectAny(3808))
        {
            return false;
        }

        return ActionWatching.BNpcSheet.TryGetValue(target.BaseId, out BNpcBase enemy)
            && !enemy.IsOmnidirectional;
    }

    private static int EffectivePartySize()
    {
        return Math.Max(1, Service.PartyList.Length);
    }

    private static double GetPositionDegrees(IGameObject target, IGameObject player)
    {
        var angle = PositionalMath.AngleXZ(target.Position, player.Position) - target.Rotation;
        var degrees = PositionalMath.Degrees(angle);
        return degrees < 0 ? degrees + 360 : degrees;
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

    internal static bool GCDCheck(int requiredGCDs)
    {
        return ActionWatching.NumberOfGcdsUsed >= requiredGCDs || Service.Configuration.IgnoreGCDChecks || LevelIgnoreGCD();
    }
}

