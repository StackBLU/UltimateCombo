using Dalamud.Game.ClientState.JobGauge.Types;
using System.Linq;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Combos.Content;
using UltimateCombo.Core;
using UltimateCombo.Data;

namespace UltimateCombo.Combos.PvE;

internal static class GNB
{
    internal const byte JobID = 37;

    internal static int MaxCartridges(byte level)
    {
        if (level >= 88)
        {
            return 3;
        }

        return 2;
    }

    internal const uint
        KeenEdge = 16137,
        NoMercy = 16138,
        BrutalShell = 16139,
        DemonSlice = 16141,
        SolidBarrel = 16145,
        GnashingFang = 16146,
        SavageClaw = 16147,
        DemonSlaughter = 16149,
        WickedTalon = 16150,
        Superbolide = 16152,
        SonicBreak = 16153,
        Continuation = 16155,
        JugularRip = 16156,
        AbdomenTear = 16157,
        EyeGouge = 16158,
        BowShock = 16159,
        HeartOfLight = 16160,
        HeartOfStone = 16161,
        BurstStrike = 16162,
        FatedCircle = 16163,
        Aurora = 16151,
        DoubleDown = 25760,
        DangerZone = 16144,
        BlastingZone = 16165,
        Bloodfest = 16164,
        Hypervelocity = 25759,
        LionHeart = 36939,
        NobleBlood = 36938,
        ReignOfBeasts = 36937,
        FatedBrand = 36936,
        Trajectory = 36934,
        LightningShot = 16143;

    internal static class Buffs
    {
        internal const ushort
            NoMercy = 1831,
            Aurora = 1835,
            ReadyToRip = 1842,
            ReadyToTear = 1843,
            ReadyToGouge = 1844,
            ReadyToRaze = 3839,
            ReadyToBreak = 3886,
            ReadyToReign = 3840,
            ReadyToBlast = 2686;
    }

    internal static class Debuffs
    {
        internal const ushort
            BowShock = 1838,
            SonicBreak = 1837;
    }

    private static GNBGauge Gauge => CustomComboFunctions.GetJobGauge<GNBGauge>();

    internal static class Config
    {
        internal static UserInt
            GNB_ST_Invuln = new("GNB_ST_Invuln", 10),
            GNB_AoE_Invuln = new("GNB_AoE_Invuln", 10);
    }

    internal class GNB_ST_DPS : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.GNB_ST_DPS;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is KeenEdge or BrutalShell or SolidBarrel) && IsEnabled(Presets.GNB_ST_DPS))
            {
                if (IsEnabled(Presets.GNB_ST_Invuln) && PlayerHealthPercentageHp() <= GetOptionValue(Config.GNB_ST_Invuln)
                    && ActionReady(Superbolide))
                {
                    return Superbolide;
                }

                if (IsEnabled(Presets.GNB_ST_LightningShot) && !InCombat() && ActionReady(LightningShot) && !InMeleeRange())
                {
                    return LightningShot;
                }

                if (CanWeave(actionID, ActionWatching.LastGCD) && (ActionWatching.NumberOfGcdsUsed >= 1 || Service.Configuration.IgnoreGCDChecks || LevelIgnoreGCD()))
                {
                    if (ActionReady(Continuation)
                        && (HasEffect(Buffs.ReadyToRip) || HasEffect(Buffs.ReadyToTear)
                        || HasEffect(Buffs.ReadyToGouge) || HasEffect(Buffs.ReadyToBlast)))
                    {
                        return OriginalHook(Continuation);
                    }

                    if (!HasEffect(Bozja.Buffs.BloodRage))
                    {
                        if (IsEnabled(Presets.GNB_ST_Bloodfest) && ActionReady(Bloodfest) && Gauge.Ammo == 0 && TargetIsBoss()
                            && (ActionReady(NoMercy) || HasEffect(Buffs.NoMercy)) && (IsActionEnabled(Bozja.BloodRage) || !HasEffect(Bozja.Buffs.Reminiscence)))
                        {
                            return Bloodfest;
                        }

                        if (ActionWatching.NumberOfGcdsUsed >= 3 || Service.Configuration.IgnoreGCDChecks || LevelIgnoreGCD())
                        {
                            if (IsEnabled(Presets.GNB_ST_NoMercy) && ActionReady(NoMercy) && CanLateWeave(actionID, ActionWatching.LastGCD))
                            {
                                return NoMercy;
                            }
                        }

                        if (ActionWatching.NumberOfGcdsUsed >= 4 || Service.Configuration.IgnoreGCDChecks || LevelIgnoreGCD())
                        {
                            if (IsEnabled(Presets.GNB_ST_BowShock) && ActionReady(BowShock))
                            {
                                return BowShock;
                            }

                            if (IsEnabled(Presets.GNB_ST_BlastingZone) && ActionReady(OriginalHook(BlastingZone)))
                            {
                                return OriginalHook(BlastingZone);
                            }
                        }

                        if (IsEnabled(Presets.GNB_ST_Aurora) && ActionReady(Aurora)
                            && (GetRemainingCharges(Aurora) == GetMaxCharges(Aurora)
                            || (GetRemainingCharges(Aurora) == 1 && GetCooldownChargeRemainingTime(Aurora) < 30)))
                        {
                            return Aurora;
                        }

                        if (IsEnabled(Presets.GNB_ST_HeartOfStone) && ActionReady(OriginalHook(HeartOfStone)))
                        {
                            if (IsTargetOfTarget() || (!IsTargetOfTarget()
                                && GetPartyMembers().Any(x => x.GameObject == TargetOfTarget)))
                            {
                                return OriginalHook(HeartOfStone);
                            }
                        }
                    }
                }

                if (IsEnabled(Presets.GNB_ST_Gnashing) && ActionReady(OriginalHook(GnashingFang)) && Gauge.Ammo > 0
                    && Gauge.AmmoComboStep == 0 && (ActionWatching.NumberOfGcdsUsed >= 3 || Service.Configuration.IgnoreGCDChecks || LevelIgnoreGCD()))
                {
                    return OriginalHook(GnashingFang);
                }

                if (IsEnabled(Presets.GNB_ST_DoubleDown) && ActionReady(DoubleDown) && Gauge.Ammo >= 1 && HasEffect(Buffs.NoMercy))
                {
                    return DoubleDown;
                }

                if (Gauge.AmmoComboStep is 1 or 2)
                {
                    return OriginalHook(GnashingFang);
                }

                if (IsEnabled(Presets.GNB_ST_Bloodfest) && ActionReady(ReignOfBeasts)
                    && ((HasEffect(Buffs.NoMercy) && HasEffect(Buffs.ReadyToReign))
                    || WasLastWeaponskill(ReignOfBeasts) || WasLastWeaponskill(NobleBlood)))
                {
                    return OriginalHook(ReignOfBeasts);
                }

                if (IsEnabled(Presets.GNB_ST_NoMercy) && ActionReady(SonicBreak) && HasEffect(Buffs.ReadyToBreak))
                {
                    return SonicBreak;
                }

                if (IsEnabled(Presets.GNB_ST_Burst) && ActionReady(BurstStrike) && Gauge.Ammo > 0
                    && ((Gauge.Ammo == MaxCartridges(Level) && lastComboMove == BrutalShell)
                    || (HasEffect(Buffs.NoMercy) && GetCooldownRemainingTime(OriginalHook(GnashingFang)) > 5 && GetCooldownRemainingTime(DoubleDown) > 10)
                    || BossAlmostDead()))
                {
                    return BurstStrike;
                }

                if (IsEnabled(Presets.GNB_ST_LightningShot) && ActionReady(LightningShot) && ProjectileThresholdDistance())
                {
                    return LightningShot;
                }

                if (ComboTime > 0)
                {
                    if (lastComboMove is KeenEdge && ActionReady(BrutalShell))
                    {
                        return BrutalShell;
                    }

                    if (lastComboMove is BrutalShell && ActionReady(SolidBarrel))
                    {
                        return SolidBarrel;
                    }
                }

                return KeenEdge;
            }

            return actionID;
        }
    }

    internal class GNB_AoE_DPS : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.GNB_AoE_DPS;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is DemonSlice or DemonSlaughter) && IsEnabled(Presets.GNB_AoE_DPS))
            {
                if (IsEnabled(Presets.GNB_AoE_Invuln) && PlayerHealthPercentageHp() <= GetOptionValue(Config.GNB_AoE_Invuln)
                    && ActionReady(Superbolide))
                {
                    return Superbolide;
                }

                if (IsEnabled(Presets.GNB_AoE_Trajectory) && ActionReady(Trajectory) && !InMeleeRange()
                    && !InCombat())
                {
                    return Trajectory;
                }

                if (CanWeave(actionID, ActionWatching.LastGCD))
                {
                    if (ActionReady(Continuation) && HasEffect(Buffs.ReadyToRaze))
                    {
                        return OriginalHook(Continuation);
                    }

                    if (IsEnabled(Presets.GNB_AoE_Bloodfest) && ActionReady(Bloodfest) && Gauge.Ammo == 0
                        && (ActionReady(NoMercy) || HasEffect(Buffs.NoMercy)))
                    {
                        return Bloodfest;
                    }

                    if (IsEnabled(Presets.GNB_AoE_NoMercy) && ActionReady(NoMercy))
                    {
                        return NoMercy;
                    }

                    if (IsEnabled(Presets.GNB_AoE_BlastingZone) && ActionReady(OriginalHook(BlastingZone)))
                    {
                        return OriginalHook(BlastingZone);
                    }

                    if (IsEnabled(Presets.GNB_AoE_BowShock) && ActionReady(BowShock))
                    {
                        return BowShock;
                    }

                    if (IsEnabled(Presets.GNB_AoE_Aurora) && ActionReady(Aurora)
                        && (GetRemainingCharges(Aurora) == GetMaxCharges(Aurora)
                        || (GetRemainingCharges(Aurora) == 1 && GetCooldownChargeRemainingTime(Aurora) < 30)))
                    {
                        return Aurora;
                    }

                    if (IsEnabled(Presets.GNB_AoE_HeartOfStone) && ActionReady(OriginalHook(HeartOfStone)))
                    {
                        if (IsTargetOfTarget() || (!IsTargetOfTarget() && GetPartyMembers().Any(x => x.GameObject == TargetOfTarget)))
                        {
                            return OriginalHook(HeartOfStone);
                        }
                    }
                }

                if (IsEnabled(Presets.GNB_AoE_DoubleDown) && ActionReady(DoubleDown) && Gauge.Ammo >= 1)
                {
                    return DoubleDown;
                }

                if (IsEnabled(Presets.GNB_AoE_Bloodfest) && ActionReady(ReignOfBeasts)
                    && ((HasEffect(Buffs.NoMercy) && HasEffect(Buffs.ReadyToReign))
                    || WasLastWeaponskill(ReignOfBeasts) || WasLastWeaponskill(NobleBlood)))
                {
                    return OriginalHook(ReignOfBeasts);
                }

                if (IsEnabled(Presets.GNB_AoE_Fated) && ActionReady(FatedCircle) && Gauge.Ammo > 0
                    && ((Gauge.Ammo == MaxCartridges(Level) && lastComboMove is DemonSlice)
                    || (HasEffect(Buffs.NoMercy) && GetCooldownRemainingTime(DoubleDown) > 10)))
                {
                    return FatedCircle;
                }

                if (ComboTime > 0 && lastComboMove is DemonSlice && ActionReady(DemonSlaughter))
                {
                    return DemonSlaughter;
                }
            }

            return actionID;
        }
    }

    internal class GNB_BurstCont : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.GNB_BurstCont;

        protected override uint Invoke(uint actionID, uint lastComboActionID)
        {
            if (actionID is BurstStrike && IsEnabled(Presets.GNB_BurstCont))
            {
                if (ActionReady(Continuation) && HasEffect(Buffs.ReadyToBlast))
                {
                    return OriginalHook(Continuation);
                }

                if (ActionReady(BurstStrike))
                {
                    return BurstStrike;
                }
            }

            return actionID;
        }
    }

    internal class GNB_GnashCont : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.GNB_GnashCont;

        protected override uint Invoke(uint actionID, uint lastComboActionID)
        {
            if (actionID is GnashingFang && IsEnabled(Presets.GNB_GnashCont))
            {
                if (ActionReady(Continuation) && (HasEffect(Buffs.ReadyToRip) || HasEffect(Buffs.ReadyToTear) || HasEffect(Buffs.ReadyToGouge)))
                {
                    return OriginalHook(Continuation);
                }

                if (ActionReady(OriginalHook(GnashingFang)))
                {
                    return OriginalHook(GnashingFang);
                }
            }

            return actionID;
        }
    }

    internal class GNB_FatedCont : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.GNB_FatedCont;

        protected override uint Invoke(uint actionID, uint lastComboActionID)
        {
            if (actionID is FatedCircle && IsEnabled(Presets.GNB_FatedCont))
            {
                if (ActionReady(Continuation) && HasEffect(Buffs.ReadyToRaze))
                {
                    return OriginalHook(Continuation);
                }

                if (ActionReady(FatedCircle))
                {
                    return FatedCircle;
                }
            }

            return actionID;
        }
    }

    internal class GNB_AuroraProtection : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.GNB_AuroraProtection;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (actionID is Aurora)
            {
                if ((HasFriendlyTarget() && TargetHasEffectAny(Buffs.Aurora)) || (!HasFriendlyTarget() && HasEffectAny(Buffs.Aurora)))
                {
                    return OriginalHook(11);
                }
            }

            return actionID;
        }
    }
}
