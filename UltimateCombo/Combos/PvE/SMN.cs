using Dalamud.Game.ClientState.JobGauge.Types;

using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Combos.Content;
using UltimateCombo.Combos.General;
using UltimateCombo.Core;
using UltimateCombo.Data;

namespace UltimateCombo.Combos.PvE;

internal class SMN
{
    internal const byte JobID = 27;

    internal const uint
        SummonRuby = 25802,
        SummonTopaz = 25803,
        SummonEmerald = 25804,
        SummonIfrit = 25805,
        SummonTitan = 25806,
        SummonGaruda = 25807,
        SummonIfrit2 = 25838,
        SummonTitan2 = 25839,
        SummonGaruda2 = 25840,
        SummonCarbuncle = 25798,
        Gemshine = 25883,
        PreciousBrilliance = 25884,
        DreadwyrmTrance = 3581,
        RubyRuin1 = 25808,
        RubyRuin2 = 25811,
        RubyRuin3 = 25817,
        TopazRuin1 = 25809,
        TopazRuin2 = 25812,
        TopazRuin3 = 25818,
        EmeraldRuin1 = 25810,
        EmeraldRuin2 = 25813,
        EmeraldRuin3 = 25819,
        Outburst = 16511,
        RubyOutburst = 25814,
        TopazOutburst = 25815,
        EmeraldOutburst = 25816,
        RubyRite = 25823,
        TopazRite = 25824,
        EmeraldRite = 25825,
        RubyCatastrophe = 25832,
        TopazCatastrophe = 25833,
        EmeraldCatastrophe = 25834,
        CrimsonCyclone = 25835,
        CrimsonStrike = 25885,
        MountainBuster = 25836,
        Slipstream = 25837,
        SummonBahamut = 7427,
        SummonPhoenix = 25831,
        SummonSolarBahamut = 36992,
        AstralImpulse = 25820,
        AstralFlare = 25821,
        Deathflare = 3582,
        EnkindleBahamut = 7429,
        FountainOfFire = 16514,
        BrandOfPurgatory = 16515,
        Rekindle = 25830,
        EnkindlePhoenix = 16516,
        UmbralImpulse = 36994,
        UmbralFlare = 36995,
        Sunflare = 36996,
        EnkindleSolarBahamut = 36998,
        LuxSolaris = 36997,
        AstralFlow = 25822,
        Ruin = 163,
        Ruin2 = 172,
        Ruin3 = 3579,
        Ruin4 = 7426,
        Tridisaster = 25826,
        RubyDisaster = 25827,
        TopazDisaster = 25828,
        EmeraldDisaster = 25829,
        EnergyDrain = 16508,
        Fester = 181,
        EnergySiphon = 16510,
        Painflare = 3578,
        Necrotize = 36990,
        SearingFlash = 36991,
        Resurrection = 173,
        RadiantAegis = 25799,
        Aethercharge = 25800,
        SearingLight = 25801;


    internal static class Buffs
    {
        internal const ushort
            FurtherRuin = 2701,
            GarudasFavor = 2725,
            TitansFavor = 2853,
            IfritsFavor = 2724,
            EverlastingFlight = 16517,
            SearingLight = 2703,
            RubysGlimmer = 3873,
            RefulgentLux = 3874,
            CrimsonStrikeReady = 4403;
    }

    private static SMNGauge Gauge => CustomComboFunctions.GetJobGauge<SMNGauge>();

    internal static class Config
    {
        internal static UserBool
            SMN_ST_Astral_Swift = new("SMN_ST_Astral_Swift"),
            SMN_ST_Astral_Garuda = new("SMN_ST_Astral_Garuda"),
            SMN_ST_Astral_Ifrit = new("SMN_ST_Astral_Ifrit"),

            SMN_AoE_Astral_Swift = new("SMN_AoE_Astral_Swift"),
            SMN_AoE_Astral_Garuda = new("SMN_AoE_Astral_Garuda"),
            SMN_AoE_Astral_Ifrit = new("SMN_AoE_Astral_Ifrit");
    }

    internal class SMN_ST_DPS : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.SMN_ST_DPS;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is Ruin or Ruin2 or Ruin3) && IsEnabled(Presets.SMN_ST_DPS))
            {
                if (WasLastSpell(SummonCarbuncle) && !InCombat())
                {
                    ActionWatching.CombatActions.Clear();
                }

                if (CanWeave(actionID) && (ActionWatching.NumberOfGcdsUsed >= 4 || Service.Configuration.IgnoreGCDChecks))
                {
                    if (IsEnabled(Presets.SMN_ST_SearingLight) && HasEffect(Buffs.RubysGlimmer))
                    {
                        return SearingFlash;
                    }

                    if (IsEnabled(Presets.SMN_ST_SearingLight) && ActionReady(SearingLight) && TargetIsBoss() && !HasEffectAny(Buffs.SearingLight))
                    {
                        return SearingLight;
                    }

                    if (IsEnabled(Presets.SMN_ST_EnergyDrain) && (ActionReady(EnergyDrain) || Gauge.AetherflowStacks > 0))
                    {
                        if (Gauge.AetherflowStacks > 0 && (HasEffect(Buffs.SearingLight) || !LevelChecked(SearingLight)))
                        {
                            return OriginalHook(Necrotize);
                        }

                        if (ActionReady(EnergyDrain) && Gauge.AetherflowStacks == 0)
                        {
                            return EnergyDrain;
                        }
                    }

                    if (IsEnabled(Presets.SMN_ST_Astral) && ActionReady(OriginalHook(AstralFlow)) && IsEnabled(Presets.SMN_ST_Astral)
                        && (WasLastSpell(AstralImpulse) || WasLastSpell(FountainOfFire) || WasLastSpell(UmbralImpulse)
                        || HasEffect(Buffs.TitansFavor)))
                    {
                        return OriginalHook(AstralFlow);
                    }

                    if (IsEnabled(Presets.SMN_ST_Enkindle) && ActionReady(OriginalHook(EnkindleBahamut))
                        && Gauge.AttunementTimerRemaining == 0 && Gauge.Attunement == 0
                        && (WasLastSpell(AstralImpulse) || WasLastSpell(FountainOfFire) || WasLastSpell(UmbralImpulse)))
                    {
                        return OriginalHook(EnkindleBahamut);
                    }

                    if (IsEnabled(Presets.SMN_ST_Lux) && HasEffect(Buffs.RefulgentLux) && EffectRemainingTime(Buffs.RefulgentLux) < 3)
                    {
                        return OriginalHook(LuxSolaris);
                    }
                }

                if (HasEffect(Buffs.GarudasFavor) && IsEnabled(Presets.SMN_ST_Astral) && !GetOptionBool(Config.SMN_ST_Astral_Garuda))
                {
                    if (ActionReady(Common.Swiftcast) && GetOptionBool(Config.SMN_ST_Astral_Swift) && !HasEffect(Occult.Buffs.OccultQuick))
                    {
                        return Common.Swiftcast;
                    }

                    return OriginalHook(AstralFlow);
                }

                if (lastComboMove is CrimsonCyclone && IsEnabled(Presets.SMN_ST_Astral) && !GetOptionBool(Config.SMN_ST_Astral_Ifrit))
                {
                    return OriginalHook(AstralFlow);
                }

                if (HasEffect(Buffs.IfritsFavor) && ActionReady(CrimsonCyclone) && IsEnabled(Presets.SMN_ST_Astral) && !GetOptionBool(Config.SMN_ST_Astral_Ifrit)
                    && (HasEffect(Buffs.IfritsFavor) || HasEffect(Buffs.CrimsonStrikeReady)))
                {
                    return OriginalHook(AstralFlow);
                }

                if (Gauge.AttunementTimerRemaining > 0 && Gauge.Attunement > 0)
                {
                    return OriginalHook(Gemshine);
                }

                if (IsEnabled(Presets.SMN_ST_SummonElements) && Gauge.AttunementTimerRemaining == 0
                    && Gauge.SummonTimerRemaining == 0 && !IsOffCooldown(OriginalHook(SummonBahamut)))
                {
                    if (ActionReady(OriginalHook(SummonGaruda)) && Gauge.IsGarudaReady)
                    {
                        return OriginalHook(SummonGaruda);
                    }

                    if (ActionReady(OriginalHook(SummonTitan)) && Gauge.IsTitanReady)
                    {
                        return OriginalHook(SummonTitan);
                    }

                    if (ActionReady(OriginalHook(SummonIfrit)) && Gauge.IsIfritReady)
                    {
                        return OriginalHook(SummonIfrit);
                    }
                }

                if (IsEnabled(Presets.SMN_ST_Ruin4) && HasEffect(Buffs.FurtherRuin))
                {
                    return Ruin4;
                }

                if (IsEnabled(Presets.SMN_ST_Carbuncle) && !HasPetPresent() && ActionWatching.NumberOfGcdsUsed == 0)
                {
                    return SummonCarbuncle;
                }

                if (IsEnabled(Presets.SMN_ST_SummonBahaPhoenix) && ActionReady(OriginalHook(SummonBahamut)))
                {
                    return OriginalHook(SummonBahamut);
                }

                if (ActionReady(OriginalHook(Ruin3)))
                {
                    return OriginalHook(Ruin3);
                }
            }

            return actionID;
        }
    }

    internal class SMN_AoE_DPS : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.SMN_AoE_DPS;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is Outburst or Tridisaster) && IsEnabled(Presets.SMN_AoE_DPS))
            {
                if (WasLastSpell(SummonCarbuncle) && !InCombat())
                {
                    ActionWatching.CombatActions.Clear();
                }

                if (CanWeave(actionID))
                {
                    if (IsEnabled(Presets.SMN_AoE_SearingLight) && HasEffect(Buffs.RubysGlimmer))
                    {
                        return SearingFlash;
                    }

                    if (IsEnabled(Presets.SMN_AoE_SearingLight) && ActionReady(SearingLight) && TargetIsBoss()
                        && !HasEffectAny(Buffs.SearingLight))
                    {
                        return SearingLight;
                    }

                    if (IsEnabled(Presets.SMN_AoE_EnergySiphon)
                        && (ActionReady(EnergySiphon) || Gauge.AetherflowStacks > 0))
                    {
                        if (Gauge.AetherflowStacks > 0 && (HasEffect(Buffs.SearingLight) || !LevelChecked(SearingLight)))
                        {
                            return OriginalHook(Painflare);
                        }

                        if (ActionReady(EnergySiphon) && Gauge.AetherflowStacks == 0)
                        {
                            return EnergySiphon;
                        }
                    }

                    if (IsEnabled(Presets.SMN_AoE_Astral) && ActionReady(OriginalHook(AstralFlow)) && IsEnabled(Presets.SMN_ST_Astral)
                        && (WasLastSpell(AstralFlare) || WasLastSpell(BrandOfPurgatory) || WasLastSpell(UmbralFlare)
                        || HasEffect(Buffs.TitansFavor)))
                    {
                        return OriginalHook(AstralFlow);
                    }

                    if (IsEnabled(Presets.SMN_AoE_Enkindle) && ActionReady(OriginalHook(EnkindleBahamut))
                        && Gauge.AttunementTimerRemaining == 0 && Gauge.Attunement == 0
                        && (WasLastSpell(AstralFlare) || WasLastSpell(BrandOfPurgatory) || WasLastSpell(UmbralFlare)))
                    {
                        return OriginalHook(EnkindleBahamut);
                    }

                    if (IsEnabled(Presets.SMN_ST_Lux) && HasEffect(Buffs.RefulgentLux) && EffectRemainingTime(Buffs.RefulgentLux) < 3)
                    {
                        return OriginalHook(LuxSolaris);
                    }
                }

                if (HasEffect(Buffs.GarudasFavor) && IsEnabled(Presets.SMN_ST_Astral) && !GetOptionBool(Config.SMN_AoE_Astral_Garuda))
                {
                    if (ActionReady(Common.Swiftcast) && GetOptionBool(Config.SMN_AoE_Astral_Swift) && !HasEffect(Occult.Buffs.OccultQuick))
                    {
                        return Common.Swiftcast;
                    }

                    return OriginalHook(AstralFlow);
                }

                if (lastComboMove is CrimsonCyclone && IsEnabled(Presets.SMN_ST_Astral) && !GetOptionBool(Config.SMN_AoE_Astral_Ifrit))
                {
                    return OriginalHook(AstralFlow);
                }

                if (HasEffect(Buffs.IfritsFavor) && ActionReady(CrimsonCyclone) && IsEnabled(Presets.SMN_ST_Astral) && !GetOptionBool(Config.SMN_AoE_Astral_Ifrit)
                    && (HasEffect(Buffs.IfritsFavor) || HasEffect(Buffs.CrimsonStrikeReady)))
                {
                    return OriginalHook(AstralFlow);
                }

                if (Gauge.AttunementTimerRemaining > 0 && Gauge.Attunement > 0)
                {
                    return OriginalHook(PreciousBrilliance);
                }

                if (IsEnabled(Presets.SMN_AoE_SummonElements) && Gauge.AttunementTimerRemaining == 0
                    && Gauge.SummonTimerRemaining == 0 && !IsOffCooldown(OriginalHook(SummonBahamut)))
                {
                    if (ActionReady(OriginalHook(SummonGaruda)) && Gauge.IsGarudaReady)
                    {
                        return OriginalHook(SummonGaruda);
                    }

                    if (ActionReady(OriginalHook(SummonTitan)) && Gauge.IsTitanReady)
                    {
                        return OriginalHook(SummonTitan);
                    }

                    if (ActionReady(OriginalHook(SummonIfrit)) && Gauge.IsIfritReady)
                    {
                        return OriginalHook(SummonIfrit);
                    }
                }

                if (IsEnabled(Presets.SMN_AoE_Ruin4) && HasEffect(Buffs.FurtherRuin))
                {
                    return Ruin4;
                }

                if (IsEnabled(Presets.SMN_AoE_Carbuncle) && !HasPetPresent() && ActionWatching.NumberOfGcdsUsed == 0)
                {
                    return SummonCarbuncle;
                }

                if (IsEnabled(Presets.SMN_AoE_SummonBahaPhoenix) && ActionReady(OriginalHook(SummonBahamut)))
                {
                    return OriginalHook(SummonBahamut);
                }

                if (ActionReady(OriginalHook(Tridisaster)))
                {
                    return OriginalHook(Tridisaster);
                }
            }

            return actionID;
        }
    }

    internal class SMN_EnergyDrain : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.SMN_EnergyDrain;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is EnergyDrain or Fester or Necrotize) && IsEnabled(Presets.SMN_EnergyDrain))
            {
                if (Gauge.AetherflowStacks > 0)
                {
                    return OriginalHook(Necrotize);
                }

                return EnergyDrain;
            }

            return actionID;
        }
    }

    internal class SMN_EnergySiphon : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.SMN_EnergySiphon;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is EnergySiphon or Painflare) && IsEnabled(Presets.SMN_EnergySiphon))
            {
                if (Gauge.AetherflowStacks > 0)
                {
                    return Painflare;
                }

                return EnergySiphon;
            }

            return actionID;
        }
    }

    internal class SMN_Enkindle : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.SMN_Enkindle;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is SummonBahamut or SummonPhoenix or SummonSolarBahamut) && IsEnabled(Presets.SMN_Enkindle))
            {
                if (CanWeave(actionID) && ActionReady(OriginalHook(EnkindleBahamut)))
                {
                    return OriginalHook(EnkindleBahamut);
                }

                return SummonBahamut;
            }

            return actionID;
        }
    }
}
