using Dalamud.Game.ClientState.JobGauge.Enums;
using Dalamud.Game.ClientState.JobGauge.Types;
using System.Collections.Generic;
using System.Linq;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Combos.General;
using UltimateCombo.Core;
using UltimateCombo.Data;

namespace UltimateCombo.Combos.PvE;

internal static class AST
{
    internal const byte JobID = 33;

    internal const uint
        Malefic = 3596,
        Malefic2 = 3598,
        EarthlyStar = 7439,
        Malefic3 = 7442,
        Malefic4 = 16555,
        FallMalefic = 25871,
        Gravity = 3615,
        Gravity2 = 25872,
        Oracle = 37029,
        AstralDraw = 37017,
        Play1 = 37019,
        Play2 = 37020,
        Play3 = 37021,
        Arrow = 37024,
        Balance = 37023,
        Bole = 37027,
        Ewer = 37028,
        Spear = 37026,
        Spire = 37025,
        MinorArcana = 37022,
        Divination = 16552,
        Lightspeed = 3606,
        Combust = 3599,
        Combust2 = 3608,
        Combust3 = 16554,
        Benefic1 = 3594,
        Benefic2 = 3610,
        AspectedBenefic = 3595,
        Helios = 3600,
        AspectedHelios = 3601,
        HeliosConjuction = 37030,
        Ascend = 3603,
        EssentialDignity = 3614,
        CelestialOpposition = 16553,
        CelestialIntersection = 16556,
        Horoscope = 16557,
        NeutralSect = 16559,
        Exaltation = 25873,
        Macrocosmos = 25874,
        Synastry = 3612,
        SunSign = 37031;

    internal static class Buffs
    {
        internal const ushort
            AspectedBenefic = 835,
            AspectedHelios = 836,
            HeliosConjunction = 3894,
            Horoscope = 1890,
            HoroscopeHelios = 1891,
            NeutralSect = 1892,
            NeutralSectShield = 1921,
            Divination = 1878,
            LordOfCrownsDrawn = 2054,
            LadyOfCrownsDrawn = 2055,
            ClarifyingDraw = 2713,
            Macrocosmos = 2718,
            BalanceDrawn = 913,
            BoleDrawn = 914,
            ArrowDrawn = 915,
            SpearDrawn = 916,
            EwerDrawn = 917,
            SpireDrawn = 918,
            BalanceBuff = 3887,
            BoleBuff = 3890,
            ArrowBuff = 3888,
            SpearBuff = 3889,
            EwerBuff = 3891,
            SpireBuff = 3892,
            Lightspeed = 841,
            SelfSynastry = 845,
            TargetSynastry = 846,
            Divining = 3893,
            Suntouched = 3895;
    }

    internal static class Debuffs
    {
        internal const ushort
            Combust = 838,
            Combust2 = 843,
            Combust3 = 1881;
    }

    internal static Dictionary<uint, ushort>
        CombustList = new() {
                { Combust,  Debuffs.Combust  },
                { Combust2, Debuffs.Combust2 },
                { Combust3, Debuffs.Combust3 }
        };

    internal static Dictionary<uint, ushort>
        HeliosList = new() {
                { AspectedHelios,  Buffs.AspectedHelios  },
                { HeliosConjuction, Buffs.HeliosConjunction }
        };

    internal static ASTGauge Gauge => CustomComboFunctions.GetJobGauge<ASTGauge>();

    internal static class Config
    {
        internal static UserBool
            AST_ST_DPS_UseDefenseCards = new("AST_ST_DPS_UseDefenseCards"),
            AST_AoE_DPS_UseDefenseCards = new("AST_AoE_DPS_UseDefenseCards");
    }

    internal class AST_ST_DPS : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.AST_ST_DPS;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is Malefic or Malefic2 or Malefic3 or Malefic4 or FallMalefic) && IsEnabled(Presets.AST_ST_DPS))
            {
                if (IsEnabled(Presets.AST_ST_DPS_EarthlyStar) && ActionReady(EarthlyStar) && !InCombat())
                {
                    return EarthlyStar;
                }

                if ((WasLastSpell(AspectedHelios) || WasLastSpell(HeliosConjuction)) && !InCombat())
                {
                    ActionWatching.CombatActions.Clear();
                }

                if (CanWeave(actionID))
                {
                    if (ActionWatching.NumberOfGcdsUsed >= 2 || Service.Configuration.IgnoreGCDChecks)
                    {
                        if (IsEnabled(Presets.AST_ST_DPS_Lightspeed) && ActionReady(Lightspeed) && !HasEffect(Buffs.Lightspeed)
                            && !HasEffect(Common.Buffs.Swiftcast) && GetCooldownRemainingTime(Divination) < 3)
                        {
                            return Lightspeed;
                        }
                    }

                    if (ActionWatching.NumberOfGcdsUsed >= 4 || Service.Configuration.IgnoreGCDChecks)
                    {
                        if (IsEnabled(Presets.AST_ST_DPS_Divination) && TargetIsBoss() && GetCooldownRemainingTime(Divination) < 3 && !HasEffectAny(Buffs.Divination))
                        {
                            if (ActionReady(Divination))
                            {
                                return Divination;
                            }
                        }

                        if (IsEnabled(Presets.AST_ST_DPS_AutoPlay) && ActionReady(OriginalHook(Play1))
                            && (HasEffect(Buffs.Divination) || !LevelChecked(Divination))
                            && (Gauge.DrawnCards.Any(x => x is CardType.Balance) || Gauge.DrawnCards.Any(x => x is CardType.Spear)))
                        {
                            return OriginalHook(Play1);
                        }

                        if (IsEnabled(Presets.AST_ST_DPS_AutoPlay) && ActionReady(OriginalHook(Play2))
                            && Config.AST_ST_DPS_UseDefenseCards
                            && (Gauge.DrawnCards.Any(x => x is CardType.Arrow) || Gauge.DrawnCards.Any(x => x is CardType.Bole)))
                        {
                            return OriginalHook(Play2);
                        }

                        if (IsEnabled(Presets.AST_ST_DPS_AutoPlay) && ActionReady(OriginalHook(Play3))
                            && Config.AST_ST_DPS_UseDefenseCards
                            && (Gauge.DrawnCards.Any(x => x is CardType.Spire) || Gauge.DrawnCards.Any(x => x is CardType.Ewer)))
                        {
                            return OriginalHook(Play3);
                        }

                        if (IsEnabled(Presets.AST_ST_DPS_MinorArcana) && ActionReady(OriginalHook(MinorArcana))
                            && Gauge.DrawnCrownCard.HasFlag(CardType.Lord) && HasEffect(Buffs.Divination))
                        {
                            return OriginalHook(MinorArcana);
                        }

                        if (IsEnabled(Presets.AST_ST_DPS_AutoDraw) && ActionReady(OriginalHook(AstralDraw))
                            && (Gauge.DrawnCards.All(x => x is CardType.None) || (!Gauge.DrawnCards.Any(x => x is CardType.Balance)
                            && !Gauge.DrawnCards.Any(x => x is CardType.Spear) && !Config.AST_ST_DPS_UseDefenseCards)))
                        {
                            if (ActionReady(OriginalHook(MinorArcana))
                                && (Gauge.DrawnCrownCard.HasFlag(CardType.Lord) || Gauge.DrawnCrownCard.HasFlag(CardType.Lady)))
                            {
                                return OriginalHook(MinorArcana);
                            }

                            return OriginalHook(AstralDraw);
                        }

                        if (IsEnabled(Presets.AST_ST_DPS_Divination) && HasEffect(Buffs.Divining))
                        {
                            return Oracle;
                        }

                        if (IsEnabled(Presets.AST_ST_DPS_EarthlyStar) && ActionReady(EarthlyStar))
                        {
                            return EarthlyStar;
                        }

                        if (IsEnabled(Presets.AST_ST_DPS_SunSign) && HasEffect(Buffs.Suntouched))
                        {
                            return SunSign;
                        }
                    }
                }

                if (IsEnabled(Presets.AST_ST_DPS_Combust) && ActionReady(OriginalHook(Combust3))
                    && (ActionWatching.NumberOfGcdsUsed >= 3 || Service.Configuration.IgnoreGCDChecks) && TargetWorthDoT()
                    && (!TargetHasEffect(CombustList[OriginalHook(Combust)])
                    || TargetEffectRemainingTime(CombustList[OriginalHook(Combust)]) <= 3
                    || ActionWatching.NumberOfGcdsUsed == 11))
                {
                    return OriginalHook(Combust);
                }

                return OriginalHook(Malefic);
            }

            return actionID;
        }
    }

    internal class AST_AoE_DPS : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.AST_AoE_DPS;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is Gravity or Gravity2) && IsEnabled(Presets.AST_AoE_DPS))
            {
                if (IsEnabled(Presets.AST_AoE_DPS_EarthlyStar) && ActionReady(EarthlyStar) && !InCombat())
                {
                    return EarthlyStar;
                }

                if (CanWeave(actionID))
                {
                    if (IsEnabled(Presets.AST_AoE_DPS_Lightspeed) && ActionReady(Lightspeed) && !HasEffect(Buffs.Lightspeed)
                        && !HasEffect(Common.Buffs.Swiftcast) && GetCooldownRemainingTime(Divination) < 3)
                    {
                        return Lightspeed;
                    }

                    if (IsEnabled(Presets.AST_AoE_DPS_Divination) && TargetIsBoss() && GetCooldownRemainingTime(Divination) < 3 && !HasEffectAny(Buffs.Divination))
                    {
                        if (ActionReady(Divination))
                        {
                            return Divination;
                        }
                    }

                    if (IsEnabled(Presets.AST_AoE_DPS_AutoPlay) && ActionReady(OriginalHook(Play1))
                        && (HasEffect(Buffs.Divination) || !LevelChecked(Divination))
                        && (Gauge.DrawnCards.Any(x => x is CardType.Balance) || Gauge.DrawnCards.Any(x => x is CardType.Spear)))
                    {
                        return OriginalHook(Play1);
                    }

                    if (IsEnabled(Presets.AST_AoE_DPS_AutoPlay) && ActionReady(OriginalHook(Play2))
                        && Config.AST_AoE_DPS_UseDefenseCards
                        && (Gauge.DrawnCards.Any(x => x is CardType.Arrow) || Gauge.DrawnCards.Any(x => x is CardType.Bole)))
                    {
                        return OriginalHook(Play2);
                    }

                    if (IsEnabled(Presets.AST_AoE_DPS_AutoPlay) && ActionReady(OriginalHook(Play3))
                        && Config.AST_AoE_DPS_UseDefenseCards
                        && (Gauge.DrawnCards.Any(x => x is CardType.Spire) || Gauge.DrawnCards.Any(x => x is CardType.Ewer)))
                    {
                        return OriginalHook(Play3);
                    }

                    if (IsEnabled(Presets.AST_AoE_DPS_MinorArcana) && ActionReady(OriginalHook(MinorArcana))
                        && Gauge.DrawnCrownCard.HasFlag(CardType.Lord) && HasEffect(Buffs.Divination))
                    {
                        return OriginalHook(MinorArcana);
                    }

                    if (IsEnabled(Presets.AST_AoE_DPS_AutoDraw) && ActionReady(OriginalHook(AstralDraw))
                        && (Gauge.DrawnCards.All(x => x is CardType.None) || (!Gauge.DrawnCards.Any(x => x is CardType.Balance)
                        && !Gauge.DrawnCards.Any(x => x is CardType.Spear) && !Config.AST_AoE_DPS_UseDefenseCards)))
                    {
                        if (ActionReady(OriginalHook(MinorArcana))
                            && (Gauge.DrawnCrownCard.HasFlag(CardType.Lord) || Gauge.DrawnCrownCard.HasFlag(CardType.Lady)))
                        {
                            return OriginalHook(MinorArcana);
                        }

                        return OriginalHook(AstralDraw);
                    }

                    if (IsEnabled(Presets.AST_AoE_DPS_Divination) && HasEffect(Buffs.Divining))
                    {
                        return Oracle;
                    }

                    if (IsEnabled(Presets.AST_AoE_DPS_EarthlyStar) && ActionReady(EarthlyStar))
                    {
                        return EarthlyStar;
                    }

                    if (IsEnabled(Presets.AST_AoE_DPS_SunSign) && HasEffect(Buffs.Suntouched))
                    {
                        return SunSign;
                    }
                }

                return OriginalHook(Gravity);
            }

            return actionID;
        }
    }

    internal class AST_ST_Heals : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.AST_ST_Heals;
        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is Benefic1 or Benefic2) && IsEnabled(Presets.AST_ST_Heals))
            {
                if (ActionReady(AspectedBenefic)
                    && ((HasFriendlyTarget() && !TargetHasEffect(Buffs.AspectedBenefic))
                    || (!HasFriendlyTarget() && !HasEffect(Buffs.AspectedBenefic))))
                {
                    return AspectedBenefic;
                }

                if (ActionReady(Benefic2))
                {
                    return Benefic2;
                }

                return Benefic1;
            }

            return actionID;
        }
    }

    internal class AST_AoE_Heals : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.AST_AoE_Heals;
        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is Helios or AspectedHelios or HeliosConjuction) && IsEnabled(Presets.AST_AoE_Heals))
            {
                if (IsEnabled(Presets.AST_AoE_Heals_Horoscope) && ActionReady(Horoscope) && !HasEffect(Buffs.HoroscopeHelios))
                {
                    return Horoscope;
                }

                if (IsEnabled(Presets.AST_AoE_Heals_NeutralSect) && ActionReady(NeutralSect))
                {
                    return NeutralSect;
                }

                if (IsEnabled(Presets.AST_AoE_Heals_SunSign) && HasEffect(Buffs.Suntouched) && HasEffect(Buffs.Suntouched)
                    && !WasLastAction(NeutralSect))
                {
                    return SunSign;
                }

                if (ActionReady(AspectedHelios)
                    && (!HasEffect(HeliosList[OriginalHook(AspectedHelios)])
                    || EffectRemainingTime(HeliosList[OriginalHook(AspectedHelios)]) <= 3
                    || HasEffect(Buffs.NeutralSect)
                    || HasEffect(Buffs.Horoscope)
                    || WasLastAbility(NeutralSect)))
                {
                    return OriginalHook(AspectedHelios);
                }

                if (IsEnabled(Presets.AST_AoE_Heals_Helios))
                {
                    return Helios;
                }
            }

            return actionID;
        }
    }

    internal class AST_Lightspeed_Protection : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.AST_Lightspeed_Protection;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (actionID is Lightspeed && IsEnabled(Presets.AST_Lightspeed_Protection))
            {
                if (HasEffect(Buffs.Lightspeed))
                {
                    return OriginalHook(11);
                }
            }

            return actionID;
        }
    }

    internal class AST_DrawCooldown : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.AST_DrawCooldown;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (actionID is Play1)
            {
                if (!Gauge.DrawnCards.Any(x => x is CardType.Balance) && !Gauge.DrawnCards.Any(x => x is CardType.Spear))
                {
                    return OriginalHook(AstralDraw);
                }

                return actionID;
            }

            if (actionID is Play2)
            {
                if (!Gauge.DrawnCards.Any(x => x is CardType.Arrow) && !Gauge.DrawnCards.Any(x => x is CardType.Bole))
                {
                    return OriginalHook(AstralDraw);
                }

                return actionID;
            }

            if (actionID is Play3)
            {
                if (!Gauge.DrawnCards.Any(x => x is CardType.Spire) && !Gauge.DrawnCards.Any(x => x is CardType.Ewer))
                {
                    return OriginalHook(AstralDraw);
                }

                return actionID;
            }

            return actionID;
        }
    }
}
