using Dalamud.Game.ClientState.JobGauge.Types;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Combos.General;
using UltimateCombo.Core;
using UltimateCombo.Data;

namespace UltimateCombo.Combos.PvE;

internal class RDM
{
    internal const byte JobID = 35;

    internal const uint
        Verthunder = 7505,
        Veraero = 7507,
        Veraero2 = 16525,
        Veraero3 = 25856,
        Verthunder2 = 16524,
        Verthunder3 = 25855,
        Impact = 16526,
        Redoublement = 7516,
        EnchantedRedoublement = 7529,
        Zwerchhau = 7512,
        EnchantedZwerchhau = 7528,
        Riposte = 7504,
        EnchantedRiposte = 7527,
        Scatter = 7509,
        Verstone = 7511,
        Verfire = 7510,
        Vercure = 7514,
        Jolt = 7503,
        Jolt2 = 7524,
        Jolt3 = 37004,
        Verholy = 7526,
        Verflare = 7525,
        Fleche = 7517,
        ContreSixte = 7519,
        Engagement = 16527,
        Verraise = 7523,
        Scorch = 16530,
        Resolution = 25858,
        Moulinet = 7513,
        EnchantedMoulinet = 7530,
        EnchantedMoulinetDeux = 37002,
        EnchantedMoulinetTrois = 37003,
        Corpsacorps = 7506,
        Displacement = 7515,
        Reprise = 16529,
        ViceOfThorns = 37005,
        GrandImpact = 37006,
        Prefulgence = 37007,
        Acceleration = 7518,
        Manafication = 7521,
        Embolden = 7520,
        MagickBarrier = 25857;

    internal static class Buffs
    {
        internal const ushort
            VerfireReady = 1234,
            VerstoneReady = 1235,
            Dualcast = 1249,
            Chainspell = 2560,
            Acceleration = 1238,
            Embolden = 1239,
            EmboldenOthers = 1297,
            Manafication = 1971,
            MagickBarrier = 2707,
            MagickedSwordPlay = 3875,
            ThornedFlourish = 3876,
            GrandImpactReady = 3877,
            PrefulugenceReady = 3878;
    }

    internal static class Traits
    {
        internal const uint
            EnhancedEmbolden = 620,
            EnhancedManaficationII = 622,
            EnhancedManaficationIII = 622,
            EnhancedAccelerationII = 624;
    }

    internal static class Config
    {

    }

    internal static RDMGauge Gauge => CustomComboFunctions.GetJobGauge<RDMGauge>();

    internal class RDM_ST_DPS : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.RDM_ST_DPS;

        protected override uint Invoke(uint actionID)
        {
            if ((actionID is Jolt or Jolt2 or Jolt3 or Verthunder or Verthunder3 or Veraero or Veraero3 or Verfire or Verstone)
                && IsEnabled(Presets.RDM_ST_DPS))
            {
                if (ActionReady(OriginalHook(Verthunder3)) && !InCombat() && !HasEffect(Buffs.MagickedSwordPlay) && Gauge.ManaStacks == 0
                    && !Service.Configuration.IgnoreGCDChecks)
                {
                    return OriginalHook(Verthunder3);
                }

                if (CanWeave(actionID, ActionWatching.LastGCD)
                    && (!WasLastGCD(EnchantedRiposte)
                    || (WasLastGCD(EnchantedRiposte) && ActionWatching.GetAttackType(ActionWatching.LastAction) != ActionWatching.ActionAttackType.Ability))
                    && (!WasLastGCD(EnchantedZwerchhau)
                    || (WasLastGCD(EnchantedZwerchhau) && ActionWatching.GetAttackType(ActionWatching.LastAction) != ActionWatching.ActionAttackType.Ability))
                    && (!WasLastGCD(EnchantedRedoublement)
                    || (WasLastGCD(EnchantedRedoublement) && ActionWatching.GetAttackType(ActionWatching.LastAction) != ActionWatching.ActionAttackType.Ability)))
                {
                    if (ActionWatching.NumberOfGcdsUsed >= 2 || Service.Configuration.IgnoreGCDChecks || LevelIgnoreGCD())
                    {
                        if (IsEnabled(Presets.RDM_ST_Swift) && ActionReady(Common.Swiftcast) && CanLateWeave(actionID, ActionWatching.LastGCD)
                            && !HasEffect(Buffs.Acceleration) && (Gauge.WhiteMana < 50 || Gauge.BlackMana < 50 || !InActionRange(EnchantedRiposte))
                            && Gauge.ManaStacks == 0 && !WasLastGCD(Verflare) && !WasLastGCD(Verholy) && !WasLastGCD(Scorch)
                            && !WasLastGCD(EnchantedRiposte) && !WasLastGCD(EnchantedZwerchhau) && !WasLastGCD(EnchantedRedoublement))
                        {
                            return Common.Swiftcast;
                        }
                    }

                    if (ActionWatching.NumberOfGcdsUsed >= 3 || Service.Configuration.IgnoreGCDChecks || LevelIgnoreGCD())
                    {
                        if (IsEnabled(Presets.RDM_ST_Fleche) && ActionReady(Fleche))
                        {
                            return Fleche;
                        }

                        if (IsEnabled(Presets.RDM_ST_Accel) && ActionReady(Acceleration)
                            && !HasEffect(Buffs.Acceleration) && !HasEffect(Common.Buffs.Swiftcast) && !HasEffect(Buffs.GrandImpactReady) && Gauge.ManaStacks == 0
                            && (Gauge.WhiteMana < 50 || Gauge.BlackMana < 50 || !InActionRange(EnchantedRiposte))
                            && (HasEffect(Buffs.Embolden) || GetRemainingCharges(Acceleration) == GetMaxCharges(Acceleration)
                            || (GetRemainingCharges(Acceleration) == 1 && GetCooldownChargeRemainingTime(Acceleration) < 10))
                            && !WasLastGCD(Verflare) && !WasLastGCD(Verholy) && !WasLastGCD(Scorch)
                            && !WasLastGCD(EnchantedRiposte) && !WasLastGCD(EnchantedZwerchhau) && !WasLastGCD(EnchantedRedoublement))
                        {
                            return Acceleration;
                        }
                    }

                    if (ActionWatching.NumberOfGcdsUsed >= 4 || Service.Configuration.IgnoreGCDChecks || LevelIgnoreGCD())
                    {
                        if (IsEnabled(Presets.RDM_ST_Embolden) && ActionReady(Embolden) && TargetIsBoss() && !HasEffectAny(Buffs.Embolden))
                        {
                            return Embolden;
                        }

                        if (IsEnabled(Presets.RDM_ST_Manafication) && ActionReady(Manafication)
                            && (HasEffect(Buffs.Embolden) || GetCooldownRemainingTime(Embolden) > 90)
                            && !WasLastGCD(Verholy) && !WasLastGCD(Verflare) && !WasLastGCD(Scorch)
                            && Gauge.ManaStacks == 0)
                        {
                            return Manafication;
                        }

                        if (IsEnabled(Presets.RDM_ST_Contre) && ActionReady(ContreSixte))
                        {
                            return ContreSixte;
                        }

                        if (IsEnabled(Presets.RDM_ST_Engagement) && ActionReady(Engagement) && InActionRange(Engagement))
                        {
                            return Engagement;
                        }

                        if (IsEnabled(Presets.RDM_ST_Corps) && ActionReady(Corpsacorps) && InMeleeRangeNoMovement())
                        {
                            return Corpsacorps;
                        }

                        if (IsEnabled(Presets.RDM_ST_Embolden) && HasEffect(Buffs.ThornedFlourish))
                        {
                            return ViceOfThorns;
                        }

                        if (IsEnabled(Presets.RDM_ST_Manafication) && HasEffect(Buffs.PrefulugenceReady))
                        {
                            return Prefulgence;
                        }
                    }
                }

                if (IsEnabled(Presets.RDM_ST_Accel) && HasEffect(Buffs.GrandImpactReady)
                    && (HasEffect(Buffs.Embolden) || EffectRemainingTime(Buffs.GrandImpactReady) < 5) && SafeToUse())
                {
                    return GrandImpact;
                }

                if (ActionReady(Resolution))
                {
                    return Resolution;
                }

                if (ActionReady(Scorch))
                {
                    return Scorch;
                }

                if (ActionReady(Verholy) && Gauge.BlackMana >= Gauge.WhiteMana)
                {
                    return Verholy;
                }

                if (ActionReady(Verflare) && Gauge.WhiteMana >= Gauge.BlackMana)
                {
                    return Verflare;
                }

                if (IsEnabled(Presets.RDM_ST_Swords) && !HasEffect(Buffs.Dualcast)
                    && ((Gauge.BlackMana >= 50 && Gauge.WhiteMana >= 50) || HasEffect(Buffs.MagickedSwordPlay)
                    || Gauge.ManaStacks == 1 || Gauge.ManaStacks == 2
                    || (!LevelChecked(Verflare) && (WasLastGCD(EnchantedRiposte) || WasLastGCD(EnchantedZwerchhau)))))
                {
                    if (LevelChecked(Redoublement) && (Gauge.ManaStacks == 2
                        || (!LevelChecked(Verflare) && WasLastGCD(EnchantedZwerchhau)))
                        && ((Gauge.WhiteMana >= 15 && Gauge.BlackMana >= 15) || HasEffect(Buffs.MagickedSwordPlay)))
                    {
                        if (IsEnabled(Presets.RDM_ST_Corps) && ActionReady(Corpsacorps) && !InActionRange(EnchantedRedoublement))
                        {
                            return Corpsacorps;
                        }

                        return OriginalHook(EnchantedRedoublement);
                    }

                    if (LevelChecked(Zwerchhau) && (Gauge.ManaStacks == 1
                        || (!LevelChecked(Verflare) && WasLastGCD(EnchantedRiposte)))
                        && ((Gauge.WhiteMana >= 15 && Gauge.BlackMana >= 15) || HasEffect(Buffs.MagickedSwordPlay)))
                    {
                        if (IsEnabled(Presets.RDM_ST_Corps) && ActionReady(Corpsacorps) && !InActionRange(EnchantedZwerchhau))
                        {
                            return Corpsacorps;
                        }

                        return OriginalHook(EnchantedZwerchhau);
                    }

                    if ((Gauge.ManaStacks == 0 || !LevelChecked(Verflare))
                        && (GetCooldownRemainingTime(Manafication) > 15 || !LevelChecked(Manafication) || Gauge.WhiteMana == 100 || Gauge.BlackMana == 100)
                        && ((LevelChecked(Verflare) && Gauge.WhiteMana >= 20 && Gauge.BlackMana >= 20)
                        || (!LevelChecked(Verflare) && Gauge.WhiteMana >= 50 && Gauge.BlackMana >= 50)
                        || HasEffect(Buffs.MagickedSwordPlay)))
                    {
                        if (IsEnabled(Presets.RDM_ST_Corps) && ActionReady(Corpsacorps) && !InActionRange(EnchantedRiposte))
                        {
                            return Corpsacorps;
                        }

                        if (InActionRange(EnchantedRiposte))
                        {
                            return OriginalHook(EnchantedRiposte);
                        }
                    }
                }

                if (HasEffect(Buffs.Dualcast) || HasEffect(Buffs.Acceleration) || HasEffect(Common.Buffs.Swiftcast))
                {
                    if (Gauge.BlackMana >= Gauge.WhiteMana)
                    {
                        return OriginalHook(Veraero3);
                    }

                    if (Gauge.WhiteMana >= Gauge.BlackMana)
                    {
                        return OriginalHook(Verthunder3);
                    }
                }

                if (HasEffect(Buffs.VerstoneReady))
                {
                    return Verstone;
                }

                if (HasEffect(Buffs.VerfireReady))
                {
                    return Verfire;
                }

                return OriginalHook(Jolt3);
            }

            return actionID;
        }
    }

    internal class RDM_AoE_DPS : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.RDM_AoE_DPS;

        protected override uint Invoke(uint actionID)
        {
            if ((actionID is Scatter or Impact or Verthunder2 or Veraero2) && IsEnabled(Presets.RDM_AoE_DPS))
            {
                if (CanWeave(actionID, ActionWatching.LastGCD)
                    && (!WasLastGCD(EnchantedMoulinet)
                    || (WasLastGCD(EnchantedMoulinet) && ActionWatching.GetAttackType(ActionWatching.LastAction) != ActionWatching.ActionAttackType.Ability))
                    && (!WasLastGCD(EnchantedMoulinetDeux)
                    || (WasLastGCD(EnchantedMoulinetDeux) && ActionWatching.GetAttackType(ActionWatching.LastAction) != ActionWatching.ActionAttackType.Ability))
                    && (!WasLastGCD(EnchantedMoulinetTrois)
                    || (WasLastGCD(EnchantedMoulinetTrois) && ActionWatching.GetAttackType(ActionWatching.LastAction) != ActionWatching.ActionAttackType.Ability)))
                {
                    if (IsEnabled(Presets.RDM_AoE_Embolden) && ActionReady(Embolden) && TargetIsBoss() && !HasEffectAny(Buffs.Embolden))
                    {
                        return Embolden;
                    }

                    if (IsEnabled(Presets.RDM_AoE_Manafication) && ActionReady(Manafication)
                        && (HasEffect(Buffs.Embolden) || GetCooldownRemainingTime(Embolden) > 90)
                        && !WasLastGCD(Verholy) && !WasLastGCD(Verflare) && !WasLastGCD(Scorch)
                        && Gauge.ManaStacks == 0)
                    {
                        return Manafication;
                    }

                    if (IsEnabled(Presets.RDM_AoE_Swift) && ActionReady(Common.Swiftcast) && CanLateWeave(actionID, ActionWatching.LastGCD)
                        && !HasEffect(Buffs.Acceleration) && (Gauge.WhiteMana < 50 || Gauge.BlackMana < 50 || !InActionRange(EnchantedRiposte))
                        && Gauge.ManaStacks == 0 && !WasLastGCD(Verflare) && !WasLastGCD(Verholy) && !WasLastGCD(Scorch)
                        && !WasLastGCD(EnchantedMoulinet) && !WasLastGCD(EnchantedMoulinetDeux) && !WasLastGCD(EnchantedMoulinetTrois))
                    {
                        return Common.Swiftcast;
                    }

                    if (IsEnabled(Presets.RDM_AoE_Fleche) && ActionReady(Fleche))
                    {
                        return Fleche;
                    }

                    if (IsEnabled(Presets.RDM_AoE_Accel) && ActionReady(Acceleration)
                        && !HasEffect(Buffs.Acceleration) && !HasEffect(Common.Buffs.Swiftcast) && !HasEffect(Buffs.GrandImpactReady) && Gauge.ManaStacks == 0
                        && (Gauge.WhiteMana < 50 || Gauge.BlackMana < 50 || !InActionRange(EnchantedRiposte))
                        && (HasEffect(Buffs.Embolden) || GetRemainingCharges(Acceleration) == GetMaxCharges(Acceleration)
                        || (GetRemainingCharges(Acceleration) == 1 && GetCooldownChargeRemainingTime(Acceleration) < 10))
                        && !WasLastGCD(Verflare) && !WasLastGCD(Verholy) && !WasLastGCD(Scorch)
                        && !WasLastGCD(EnchantedMoulinet) && !WasLastGCD(EnchantedMoulinetDeux) && !WasLastGCD(EnchantedMoulinetTrois))
                    {
                        return Acceleration;
                    }

                    if (IsEnabled(Presets.RDM_AoE_Embolden) && HasEffect(Buffs.ThornedFlourish))
                    {
                        return ViceOfThorns;
                    }

                    if (IsEnabled(Presets.RDM_AoE_Manafication) && HasEffect(Buffs.PrefulugenceReady))
                    {
                        return Prefulgence;
                    }

                    if (IsEnabled(Presets.RDM_AoE_Contre) && ActionReady(ContreSixte))
                    {
                        return ContreSixte;
                    }
                }

                if (ActionReady(Resolution))
                {
                    return Resolution;
                }

                if (ActionReady(Scorch))
                {
                    return Scorch;
                }

                if (IsEnabled(Presets.RDM_AoE_Accel) && HasEffect(Buffs.GrandImpactReady) && SafeToUse())
                {
                    return GrandImpact;
                }

                if (ActionReady(Verholy) && Gauge.BlackMana >= Gauge.WhiteMana)
                {
                    return Verholy;
                }

                if (ActionReady(Verflare) && Gauge.WhiteMana >= Gauge.BlackMana)
                {
                    return Verflare;
                }

                if (IsEnabled(Presets.RDM_AoE_Swords) && !HasEffect(Buffs.Dualcast) && LevelChecked(Moulinet) && ((Gauge.BlackMana >= 50 && Gauge.WhiteMana >= 50)
                    || HasEffect(Buffs.MagickedSwordPlay) || Gauge.ManaStacks == 1 || Gauge.ManaStacks == 2
                    || (!LevelChecked(Verflare) && (WasLastGCD(EnchantedMoulinet) || WasLastGCD(EnchantedMoulinetDeux)))))
                {
                    if (WasLastGCD(OriginalHook(EnchantedMoulinetDeux)) && (Gauge.ManaStacks == 2 || !LevelChecked(Verflare))
                        && ((Gauge.WhiteMana >= 15 && Gauge.BlackMana >= 15) || HasEffect(Buffs.MagickedSwordPlay)))
                    {
                        if (IsEnabled(Presets.RDM_AoE_Corps) && ActionReady(Corpsacorps) && !InActionRange(EnchantedMoulinetTrois))
                        {
                            return Corpsacorps;
                        }

                        return OriginalHook(EnchantedMoulinetTrois);
                    }

                    if (WasLastGCD(OriginalHook(EnchantedMoulinet)) && (Gauge.ManaStacks == 1 || !LevelChecked(Verflare))
                        && ((Gauge.WhiteMana >= 15 && Gauge.BlackMana >= 15) || HasEffect(Buffs.MagickedSwordPlay)))
                    {
                        if (IsEnabled(Presets.RDM_AoE_Corps) && ActionReady(Corpsacorps) && !InActionRange(EnchantedMoulinetDeux))
                        {
                            return Corpsacorps;
                        }

                        return OriginalHook(EnchantedMoulinetDeux);
                    }

                    if ((Gauge.ManaStacks == 0 || !LevelChecked(Verflare))
                        && (GetCooldownRemainingTime(Manafication) > 15 || !LevelChecked(Manafication) || Gauge.WhiteMana == 100 || Gauge.BlackMana == 100)
                        && ((LevelChecked(Verflare) && Gauge.WhiteMana >= 20 && Gauge.BlackMana >= 20)
                        || (!LevelChecked(Verflare) && Gauge.WhiteMana >= 50 && Gauge.BlackMana >= 50)
                        || HasEffect(Buffs.MagickedSwordPlay)))
                    {
                        if (IsEnabled(Presets.RDM_AoE_Corps) && ActionReady(Corpsacorps) && !InActionRange(EnchantedMoulinet))
                        {
                            return Corpsacorps;
                        }

                        if (InActionRange(EnchantedMoulinet))
                        {
                            return OriginalHook(EnchantedMoulinet);
                        }
                    }
                }

                if (HasEffect(Buffs.Dualcast) || HasEffect(Buffs.Acceleration) || HasEffect(Common.Buffs.Swiftcast))
                {
                    return OriginalHook(Impact);
                }

                if (Gauge.BlackMana >= Gauge.WhiteMana)
                {
                    return Veraero2;
                }

                if (Gauge.WhiteMana >= Gauge.BlackMana)
                {
                    return Verthunder2;
                }
            }

            return actionID;
        }
    }

    internal class RDM_ST_Melee : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.RDM_ST_Melee;

        protected override uint Invoke(uint actionID)
        {
            if ((actionID is Riposte or Zwerchhau or Redoublement) && IsEnabled(Presets.RDM_ST_Melee))
            {
                if (ActionReady(Resolution))
                {
                    return Resolution;
                }

                if (ActionReady(Scorch))
                {
                    return Scorch;
                }

                if (ActionReady(Verholy) && Gauge.BlackMana >= Gauge.WhiteMana)
                {
                    return Verholy;
                }

                if (ActionReady(Verflare) && Gauge.WhiteMana >= Gauge.BlackMana)
                {
                    return Verflare;
                }

                if (!HasEffect(Buffs.Dualcast) && ((Gauge.BlackMana >= 50 && Gauge.WhiteMana >= 50) || HasEffect(Buffs.MagickedSwordPlay)
                    || Gauge.ManaStacks == 1 || Gauge.ManaStacks == 2
                    || (!LevelChecked(Verflare) && (WasLastGCD(EnchantedRiposte) || WasLastGCD(EnchantedZwerchhau)))))
                {
                    if (LevelChecked(Redoublement) && (Gauge.ManaStacks == 2
                        || (!LevelChecked(Verflare) && WasLastGCD(EnchantedZwerchhau)))
                        && ((Gauge.WhiteMana >= 15 && Gauge.BlackMana >= 15) || HasEffect(Buffs.MagickedSwordPlay)))
                    {
                        return OriginalHook(EnchantedRedoublement);
                    }

                    if (LevelChecked(Zwerchhau) && (Gauge.ManaStacks == 1
                        || (!LevelChecked(Verflare) && WasLastGCD(EnchantedRiposte)))
                        && ((Gauge.WhiteMana >= 15 && Gauge.BlackMana >= 15) || HasEffect(Buffs.MagickedSwordPlay)))
                    {
                        return OriginalHook(EnchantedZwerchhau);
                    }

                    if ((Gauge.ManaStacks == 0 || !LevelChecked(Verflare))
                        && (GetCooldownRemainingTime(Manafication) > 15 || !LevelChecked(Manafication) || Gauge.WhiteMana == 100 || Gauge.BlackMana == 100)
                        && ((LevelChecked(Verflare) && Gauge.WhiteMana >= 20 && Gauge.BlackMana >= 20)
                        || (!LevelChecked(Verflare) && Gauge.WhiteMana >= 50 && Gauge.BlackMana >= 50)
                        || HasEffect(Buffs.MagickedSwordPlay)))
                    {
                        if (InActionRange(EnchantedRiposte))
                        {
                            return OriginalHook(EnchantedRiposte);
                        }
                    }
                }

                return OriginalHook(13);
            }

            return actionID;
        }
    }

    internal class RDM_AoE_Melee : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.RDM_AoE_Melee;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is Moulinet && IsEnabled(Presets.RDM_AoE_Melee))
            {
                if (ActionReady(Resolution) && WasLastGCD(Scorch))
                {
                    return Resolution;
                }

                if (ActionReady(Scorch) && (WasLastGCD(Verholy) || WasLastGCD(Verflare)))
                {
                    return Scorch;
                }

                if (ActionReady(Verholy) && Gauge.BlackMana >= Gauge.WhiteMana)
                {
                    return Verholy;
                }

                if (ActionReady(Verflare) && Gauge.WhiteMana >= Gauge.BlackMana)
                {
                    return Verflare;
                }

                if (!HasEffect(Buffs.Dualcast) && LevelChecked(Moulinet) && ((Gauge.BlackMana >= 50 && Gauge.WhiteMana >= 50)
                    || HasEffect(Buffs.MagickedSwordPlay) || Gauge.ManaStacks == 1 || Gauge.ManaStacks == 2
                    || (!LevelChecked(Verflare) && (WasLastGCD(EnchantedMoulinet) || WasLastGCD(EnchantedMoulinetDeux)))))
                {
                    if (WasLastGCD(EnchantedMoulinetDeux) && (Gauge.ManaStacks == 2 || !LevelChecked(Verflare))
                        && ((Gauge.WhiteMana >= 15 && Gauge.BlackMana >= 15) || HasEffect(Buffs.MagickedSwordPlay)))
                    {
                        return OriginalHook(EnchantedMoulinetTrois);
                    }

                    if (WasLastGCD(EnchantedMoulinet) && (Gauge.ManaStacks == 1 || !LevelChecked(Verflare))
                        && ((Gauge.WhiteMana >= 15 && Gauge.BlackMana >= 15) || HasEffect(Buffs.MagickedSwordPlay)))
                    {
                        return OriginalHook(EnchantedMoulinetDeux);
                    }

                    if ((Gauge.ManaStacks == 0 || !LevelChecked(Verflare))
                        && (GetCooldownRemainingTime(Manafication) > 15 || !LevelChecked(Manafication) || Gauge.WhiteMana == 100 || Gauge.BlackMana == 100)
                        && ((LevelChecked(Verflare) && Gauge.WhiteMana >= 20 && Gauge.BlackMana >= 20)
                        || (!LevelChecked(Verflare) && Gauge.WhiteMana >= 50 && Gauge.BlackMana >= 50)
                        || HasEffect(Buffs.MagickedSwordPlay)))
                    {
                        return OriginalHook(EnchantedMoulinet);
                    }
                }

                return OriginalHook(13);
            }

            return actionID;
        }
    }
}
