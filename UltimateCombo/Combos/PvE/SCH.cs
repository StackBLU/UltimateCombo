using Dalamud.Game.ClientState.JobGauge.Types;
using System.Collections.Generic;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Core;
using UltimateCombo.Data;

namespace UltimateCombo.Combos.PvE;

internal static class SCH
{
    internal const byte JobID = 28;

    internal const uint

        Physick = 190,
        Adloquium = 185,
        Succor = 186,
        Lustrate = 189,
        SacredSoil = 188,
        Indomitability = 3583,
        Excogitation = 7434,
        Consolation = 16546,
        Resurrection = 173,
        Concitation = 37013,

        Bio = 17864,
        Bio2 = 17865,
        Biolysis = 16540,
        Ruin = 17869,
        Ruin2 = 17870,
        Broil = 3584,
        Broil2 = 7435,
        Broil3 = 16541,
        Broil4 = 25865,
        EnergyDrain = 167,
        ArtOfWar = 16539,
        ArtOfWar2 = 25866,
        BanefulImpaction = 37012,

        SummonSeraph = 16545,
        SummonEos = 17215,
        WhisperingDawn = 16537,
        FeyIllumination = 16538,
        Dissipation = 3587,
        Aetherpact = 7437,
        DissolveUnion = 7869,
        FeyBlessing = 16543,

        Aetherflow = 166,
        Protraction = 25867,
        Recitation = 16542,
        ChainStratagem = 7436,
        DeploymentTactics = 3585;

    internal static class Buffs
    {
        internal const ushort
            Galvanize = 297,
            SacredSoil = 299,
            Recitation = 1896,
            Dissipation = 791,
            ImpactImminent = 3882,
            Seraphism = 3884;
    }

    internal static class Debuffs
    {
        internal const ushort
            Bio1 = 179,
            Bio2 = 189,
            Biolysis = 1895,
            ChainStratagem = 1221;
    }

    internal static readonly Dictionary<uint, ushort>
        BioList = new() {
                { Bio, Debuffs.Bio1 },
                { Bio2, Debuffs.Bio2 },
                { Biolysis, Debuffs.Biolysis }
        };

    private static SCHGauge Gauge => CustomComboFunctions.GetJobGauge<SCHGauge>();

    internal static class Config
    {

    }

    internal class SCH_ST_DPS : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.SCH_ST_DPS;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is Ruin or Broil or Broil2 or Broil3 or Broil4) && IsEnabled(Presets.SCH_ST_DPS))
            {
                if ((WasLastSpell(Succor) || WasLastSpell(Concitation) || WasLastSpell(Adloquium)
                    || WasLastSpell(SummonEos)) && !InCombat())
                {
                    ActionWatching.CombatActions.Clear();
                }

                if (IsEnabled(Presets.SCH_ST_DPS_Aetherflow) && ActionReady(Aetherflow) && Gauge.Aetherflow == 0 && CurrentMP < 2000)
                {
                    return Aetherflow;
                }

                if (ActionWatching.NumberOfGcdsUsed >= 4 || Service.Configuration.IgnoreGCDChecks)
                {
                    if (IsEnabled(Presets.SCH_ST_DPS_ChainStrat) && ActionReady(ChainStratagem) && TargetIsBoss()
                        && !TargetHasEffectAny(Debuffs.ChainStratagem))
                    {
                        return ChainStratagem;
                    }
                }

                if (CanWeave(actionID))
                {
                    if (ActionWatching.NumberOfGcdsUsed >= 4 || Service.Configuration.IgnoreGCDChecks)
                    {
                        if (IsEnabled(Presets.SCH_ST_DPS_Seraph) && ActionReady(OriginalHook(SummonSeraph)) && Gauge.SeraphTimer > 0
                        && Gauge.SeraphTimer < 5000)
                        {
                            return OriginalHook(SummonSeraph);
                        }

                        if (IsEnabled(Presets.SCH_ST_DPS_ChainStrat) && HasEffect(Buffs.ImpactImminent)
                            && TargetEffectRemainingTime(Debuffs.ChainStratagem) is < 15 and > 1)
                        {
                            return BanefulImpaction;
                        }

                        if (IsEnabled(Presets.SCH_ST_DPS_EnergyDrain) && ActionReady(EnergyDrain) && Gauge.Aetherflow > 0
                            && (GetCooldownRemainingTime(Aetherflow) <= (Gauge.Aetherflow * GetCooldown(actionID).CooldownTotal) + 0.5
                            || TargetHasEffectAny(Debuffs.ChainStratagem)))
                        {
                            return EnergyDrain;
                        }

                        if (IsEnabled(Presets.SCH_ST_DPS_Aetherflow) && ActionReady(Aetherflow) && Gauge.Aetherflow == 0)
                        {
                            return Aetherflow;
                        }
                    }

                    if (ActionWatching.NumberOfGcdsUsed >= 2 || Service.Configuration.IgnoreGCDChecks)
                    {
                        if (IsEnabled(Presets.SCH_ST_DPS_Dissipation) && ActionReady(Dissipation)
                            && !HasEffect(Buffs.Seraphism) && Gauge.SeraphTimer == 0 && Gauge.Aetherflow == 0)
                        {
                            return Dissipation;
                        }
                    }
                }

                if (IsEnabled(Presets.SCH_ST_DPS_Fairy) && !HasPetPresent() && !HasEffect(Buffs.Dissipation))
                {
                    return SummonEos;
                }

                if (IsEnabled(Presets.SCH_ST_DPS_Bio) && ActionReady(OriginalHook(Biolysis))
                    && (ActionWatching.NumberOfGcdsUsed >= 1 || Service.Configuration.IgnoreGCDChecks) && TargetWorthDoT()
                    && (!TargetHasEffect(BioList[OriginalHook(Biolysis)]) || TargetEffectRemainingTime(BioList[OriginalHook(Biolysis)]) <= 3
                        || ActionWatching.NumberOfGcdsUsed == 11))
                {
                    return OriginalHook(Biolysis);
                }

                if (IsEnabled(Presets.SCH_ST_DPS_Ruin2Movement) && ActionReady(Ruin2) && IsMoving)
                {
                    return OriginalHook(Ruin2);
                }

                return OriginalHook(Ruin);
            }

            return actionID;
        }
    }

    internal class SCH_AoE_DPS : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.SCH_AoE_DPS;
        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is ArtOfWar or ArtOfWar2) && IsEnabled(Presets.SCH_AoE_DPS))
            {
                if (IsEnabled(Presets.SCH_ST_DPS_Aetherflow) && ActionReady(Aetherflow) && Gauge.Aetherflow == 0 && CurrentMP < 2000)
                {
                    return Aetherflow;
                }

                if (CanWeave(actionID))
                {
                    if (IsEnabled(Presets.SCH_AoE_Aetherflow) && ActionReady(Aetherflow) && Gauge.Aetherflow == 0)
                    {
                        return Aetherflow;
                    }

                    if (IsEnabled(Presets.SCH_AoE_DPS_ChainStrat))
                    {
                        if (ActionReady(ChainStratagem) && TargetIsBoss() && !TargetHasEffectAny(Debuffs.ChainStratagem))
                        {
                            return ChainStratagem;
                        }

                        if (HasEffect(Buffs.ImpactImminent) && TargetEffectRemainingTime(Debuffs.ChainStratagem) is < 15 and > 1)
                        {
                            return BanefulImpaction;
                        }
                    }

                    if (IsEnabled(Presets.SCH_AoE_DPS_EnergyDrain) && ActionReady(EnergyDrain) && Gauge.Aetherflow > 0
                        && (GetCooldownRemainingTime(Aetherflow) <= (Gauge.Aetherflow * GetCooldown(actionID).CooldownTotal) + 0.5
                        || TargetHasEffectAny(Debuffs.ChainStratagem)))
                    {
                        return EnergyDrain;
                    }

                    if (IsEnabled(Presets.SCH_AoE_DPS_Dissipation) && ActionReady(Dissipation)
                        && !HasEffect(Buffs.Seraphism) && Gauge.SeraphTimer == 0
                        && Gauge.Aetherflow == 0 && IsOnCooldown(Aetherflow) && TargetEffectRemainingTimeAny(Debuffs.ChainStratagem) >= 5)
                    {
                        return Dissipation;
                    }
                }

                if (IsEnabled(Presets.SCH_AoE_DPS_Fairy) && !HasPetPresent() && !HasEffect(Buffs.Dissipation))
                {
                    return SummonEos;
                }

                return OriginalHook(ArtOfWar);
            }

            return actionID;
        }
    }

    internal class SCH_ST_Heals : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.SCH_ST_Heals;
        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is Physick or Adloquium) && IsEnabled(Presets.SCH_ST_Heals))
            {
                if (ActionReady(Adloquium))
                {
                    return Adloquium;
                }

                return Physick;
            }

            return actionID;
        }
    }

    internal class SCH_Lustrate : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.SCH_Lustrate;
        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is Lustrate or Excogitation) && IsEnabled(Presets.SCH_Lustrate))
            {
                if (ActionReady(Excogitation))
                {
                    return Excogitation;
                }

                if (ActionReady(Lustrate) && Gauge.Aetherflow > 0)
                {
                    return Lustrate;
                }
            }

            return actionID;
        }
    }

    internal class SCH_DissipationDrain : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.SCH_DissipationDrain;
        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is Aetherflow or Dissipation or EnergyDrain) && IsEnabled(Presets.SCH_DissipationDrain))
            {
                if (ActionReady(EnergyDrain) && Gauge.Aetherflow > 0)
                {
                    return EnergyDrain;
                }

                if (ActionReady(Aetherflow))
                {
                    return Aetherflow;
                }

                if ((ActionReady(Dissipation) && !HasEffect(Buffs.Seraphism))
                    || (LevelChecked(Dissipation) && GetCooldownRemainingTime(Dissipation) < GetCooldownRemainingTime(Aetherflow)))
                {
                    return Dissipation;
                }

                return Aetherflow;
            }

            return actionID;
        }
    }

    internal class SCH_SeraphBlessing : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.SCH_SeraphBlessing;
        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is FeyBlessing or SummonSeraph or Consolation) && IsEnabled(Presets.SCH_SeraphBlessing))
            {
                if (ActionReady(Consolation) && Gauge.SeraphTimer > 0)
                {
                    return Consolation;
                }

                if (ActionReady(SummonSeraph))
                {
                    return SummonSeraph;
                }

                if (ActionReady(FeyBlessing) && Gauge.SeraphTimer == 0)
                {
                    return FeyBlessing;
                }
            }

            return actionID;
        }
    }

    internal class SCH_ProRecitation : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.SCH_ProRecitation;
        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is Protraction or Recitation) && IsEnabled(Presets.SCH_ProRecitation))
            {
                if (ActionReady(Recitation))
                {
                    return Recitation;
                }

                if (ActionReady(Protraction))
                {
                    return Protraction;
                }
            }

            return actionID;
        }
    }

    internal class SCH_NoDissipate : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.SCH_NoDissipate;
        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (actionID is Dissipation && IsEnabled(Presets.SCH_NoDissipate))
            {
                if (HasEffect(Buffs.Seraphism))
                {
                    return OriginalHook(11);
                }
            }

            return actionID;
        }
    }

    internal class SCH_SeraphNoWaste : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.SCH_SeraphNoWaste;
        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (actionID is SummonSeraph && IsEnabled(Presets.SCH_SeraphNoWaste))
            {
                if (WasLastAction(WhisperingDawn) || WasLastAction(FeyIllumination) || WasLastAction(FeyBlessing))
                {
                    return OriginalHook(11);
                }
            }

            return actionID;
        }
    }
}
