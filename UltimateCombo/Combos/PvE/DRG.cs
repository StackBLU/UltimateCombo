using Dalamud.Game.ClientState.JobGauge.Types;

using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Core;
using UltimateCombo.Data;

namespace UltimateCombo.Combos.PvE;

internal class DRG
{
    internal const byte JobID = 22;

    internal const uint
        PiercingTalon = 90,
        ElusiveJump = 94,
        LanceCharge = 85,
        BattleLitany = 3557,
        Jump = 92,
        LifeSurge = 83,
        HighJump = 16478,
        MirageDive = 7399,
        BloodOfTheDragon = 3553,
        Stardiver = 16480,
        CoerthanTorment = 16477,
        DoomSpike = 86,
        SonicThrust = 7397,
        ChaosThrust = 88,
        RaidenThrust = 16479,
        TrueThrust = 75,
        Disembowel = 87,
        FangAndClaw = 3554,
        WheelingThrust = 3556,
        FullThrust = 84,
        VorpalThrust = 78,
        WyrmwindThrust = 25773,
        DraconianFury = 25770,
        ChaoticSpring = 25772,
        DragonfireDive = 96,
        Geirskogul = 3555,
        Nastrond = 7400,
        HeavensThrust = 25771,
        Drakesbane = 36952,
        RiseOfTheDragon = 36953,
        LanceBarrage = 36954,
        SpiralBlow = 36955,
        Starcross = 36956;

    internal static class Buffs
    {
        internal const ushort
            LanceCharge = 1864,
            BattleLitany = 786,
            DiveReady = 1243,
            RaidenThrustReady = 1863,
            PowerSurge = 2720,
            LifeSurge = 116,
            DraconianFire = 1863,
            NastrondReady = 3844,
            StarcrossReady = 3846,
            DragonsFlight = 3845;
    }

    internal static class Debuffs
    {
        internal const ushort
            ChaosThrust = 118,
            ChaoticSpring = 2719;
    }

    internal static DRGGauge Gauge => CustomComboFunctions.GetJobGauge<DRGGauge>();

    internal static class Config
    {

    }

    internal class DRG_ST_DPS : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.DRG_ST_DPS;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is TrueThrust or VorpalThrust or LanceBarrage or Disembowel or SpiralBlow or FullThrust
                or HeavensThrust or ChaosThrust or ChaoticSpring or FangAndClaw or WheelingThrust) && IsEnabled(Presets.DRG_ST_DPS))
            {
                if (CanWeave(actionID, ActionWatching.LastGCD))
                {
                    if (ActionWatching.NumberOfGcdsUsed >= 2 || Service.Configuration.IgnoreGCDChecks || LevelIgnoreGCD())
                    {
                        if (IsEnabled(Presets.DRG_ST_LanceCharge) && ActionReady(LanceCharge))
                        {
                            return LanceCharge;
                        }
                    }

                    if (ActionWatching.NumberOfGcdsUsed >= 3 || Service.Configuration.IgnoreGCDChecks || LevelIgnoreGCD())
                    {
                        if (IsEnabled(Presets.DRG_ST_BattleLitany) && ActionReady(BattleLitany) && TargetIsBoss()
                            && !HasEffectAny(Buffs.BattleLitany))
                        {
                            return BattleLitany;
                        }

                        if (IsEnabled(Presets.DRG_ST_Geirskogul) && ActionReady(Geirskogul))
                        {
                            return Geirskogul;
                        }

                        if (IsEnabled(Presets.DRG_ST_HighJump) && ActionReady(OriginalHook(HighJump))
                            && OriginalHook(HighJump) != MirageDive)
                        {
                            return OriginalHook(HighJump);
                        }

                        if (IsEnabled(Presets.DRG_ST_LifeSurge) && ActionReady(LifeSurge)
                            && (HasEffect(Buffs.LanceCharge) || GetRemainingCharges(LifeSurge) == GetMaxCharges(LifeSurge)
                            || (GetRemainingCharges(LifeSurge) == GetMaxCharges(LifeSurge) - 1 && GetCooldownChargeRemainingTime(LifeSurge) < 5))
                            && !HasEffect(Buffs.LifeSurge)
                            && ((WasLastWeaponskill(WheelingThrust) && LevelChecked(Drakesbane))
                            || WasLastWeaponskill(OriginalHook(LanceBarrage))))
                        {
                            return LifeSurge;
                        }

                        if (IsEnabled(Presets.DRG_ST_Dragonfire) && ActionReady(DragonfireDive)
                            && (InMeleeRangeNoMovement() || Gauge.LOTDTimer < 10))
                        {
                            return DragonfireDive;
                        }

                        if (IsEnabled(Presets.DRG_ST_Geirskogul) && ActionReady(Nastrond)
                            && HasEffect(Buffs.NastrondReady))
                        {
                            return Nastrond;
                        }

                        if (IsEnabled(Presets.DRG_ST_Stardiver) && ActionReady(Stardiver) && Gauge.IsLOTDActive
                            && ActionWatching.GetAttackType(ActionWatching.LastAction) != ActionWatching.ActionAttackType.Ability
                            && (InMeleeRangeNoMovement() || Gauge.LOTDTimer < 10))
                        {
                            return Stardiver;
                        }

                        if (IsEnabled(Presets.DRG_AoE_Stardiver) && HasEffect(Buffs.StarcrossReady))
                        {
                            return Starcross;
                        }

                        if (IsEnabled(Presets.DRG_ST_Dragonfire) && HasEffect(Buffs.DragonsFlight))
                        {
                            return RiseOfTheDragon;
                        }

                        if (IsEnabled(Presets.DRG_ST_HighJump) && HasEffect(Buffs.DiveReady))
                        {
                            return MirageDive;
                        }

                        if (IsEnabled(Presets.DRG_ST_Wyrmwind) && ActionReady(WyrmwindThrust) && Gauge.FirstmindsFocusCount == 2)
                        {
                            return WyrmwindThrust;
                        }
                    }
                }

                if (IsEnabled(Presets.DRG_ST_PiercingTalon) && ActionReady(PiercingTalon) && ProjectileThresholdDistance())
                {
                    return PiercingTalon;
                }

                if (ComboTime > 0)
                {
                    if (lastComboMove is TrueThrust or RaidenThrust)
                    {
                        if (ActionReady(OriginalHook(Disembowel))
                            && ((ActionReady(ChaosThrust) && TargetEffectRemainingTime(Debuffs.ChaosThrust) < 7 && !LevelChecked(ChaoticSpring))
                                || (ActionReady(ChaoticSpring) && TargetEffectRemainingTime(Debuffs.ChaoticSpring) < 7)
                                || !HasEffect(Buffs.PowerSurge)))
                        {
                            return OriginalHook(Disembowel);
                        }

                        return OriginalHook(VorpalThrust);
                    }

                    if ((lastComboMove is Disembowel or SpiralBlow) && ActionReady(OriginalHook(ChaosThrust)))
                    {
                        return OriginalHook(ChaosThrust);
                    }

                    if ((lastComboMove is ChaosThrust or ChaoticSpring) && ActionReady(WheelingThrust))
                    {
                        return WheelingThrust;
                    }

                    if ((lastComboMove is VorpalThrust or LanceBarrage) && ActionReady(OriginalHook(FullThrust)))
                    {
                        return OriginalHook(FullThrust);
                    }

                    if ((lastComboMove is FullThrust or HeavensThrust) && ActionReady(FangAndClaw))
                    {
                        return FangAndClaw;
                    }

                    if ((lastComboMove is WheelingThrust or FangAndClaw) && ActionReady(Drakesbane))
                    {
                        return Drakesbane;
                    }
                }

                return TrueThrust;
            }

            return actionID;
        }
    }

    internal class DRG_AoE_DPS : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.DRG_AoE_DPS;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is DoomSpike or SonicThrust or CoerthanTorment) && IsEnabled(Presets.DRG_AoE_DPS))
            {
                if (CanWeave(actionID, ActionWatching.LastGCD))
                {
                    if (IsEnabled(Presets.DRG_AoE_LanceCharge) && ActionReady(LanceCharge))
                    {
                        return LanceCharge;
                    }

                    if (IsEnabled(Presets.DRG_AoE_BattleLitany) && ActionReady(BattleLitany) && TargetIsBoss()
                        && !HasEffectAny(Buffs.BattleLitany))
                    {
                        return BattleLitany;
                    }

                    if (IsEnabled(Presets.DRG_AoE_Geirskogul) && ActionReady(Geirskogul))
                    {
                        return Geirskogul;
                    }

                    if (IsEnabled(Presets.DRG_AoE_Dragonfire) && ActionReady(DragonfireDive))
                    {
                        return DragonfireDive;
                    }

                    if (IsEnabled(Presets.DRG_AoE_Geirskogul) && ActionReady(Nastrond)
                        && HasEffect(Buffs.NastrondReady))
                    {
                        return Nastrond;
                    }

                    if (IsEnabled(Presets.DRG_AoE_Stardiver) && ActionReady(Stardiver) && Gauge.IsLOTDActive
                        && ActionWatching.GetAttackType(ActionWatching.LastAction) != ActionWatching.ActionAttackType.Ability)
                    {
                        return Stardiver;
                    }

                    if (IsEnabled(Presets.DRG_AoE_Stardiver) && HasEffect(Buffs.StarcrossReady))
                    {
                        return Starcross;
                    }

                    if (IsEnabled(Presets.DRG_AoE_LifeSurge) && ActionReady(LifeSurge)
                        && HasEffect(Buffs.LanceCharge) && !HasEffect(Buffs.LifeSurge)
                        && WasLastWeaponskill(SonicThrust))
                    {
                        return LifeSurge;
                    }

                    if (IsEnabled(Presets.DRG_AoE_Dragonfire) && ActionReady(RiseOfTheDragon) && HasEffect(Buffs.DragonsFlight))
                    {
                        return RiseOfTheDragon;
                    }

                    if (IsEnabled(Presets.DRG_AoE_Wyrmwind) && ActionReady(WyrmwindThrust) && Gauge.FirstmindsFocusCount == 2)
                    {
                        return WyrmwindThrust;
                    }

                    if (IsEnabled(Presets.DRG_AoE_HighJump) && ActionReady(OriginalHook(HighJump)))
                    {
                        return OriginalHook(HighJump);
                    }

                    if (IsEnabled(Presets.DRG_AoE_HighJump) && ActionReady(MirageDive) && HasEffect(Buffs.DiveReady))
                    {
                        return MirageDive;
                    }
                }

                if (ComboTime > 0)
                {
                    if ((lastComboMove is DoomSpike or DraconianFury) && ActionReady(SonicThrust))
                    {
                        return SonicThrust;
                    }

                    if (lastComboMove is SonicThrust && ActionReady(CoerthanTorment))
                    {
                        return CoerthanTorment;
                    }
                }

                return DoomSpike;
            }

            return actionID;
        }
    }
}
