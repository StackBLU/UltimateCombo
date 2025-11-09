using Dalamud.Game.ClientState.JobGauge.Types;
using System.Collections.Generic;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Core;
using UltimateCombo.Data;

namespace UltimateCombo.Combos.PvE;

internal static class WHM
{
    internal const byte JobID = 24;

    internal const uint
        Cure1 = 120,
        Cure2 = 135,
        Cure3 = 131,
        Regen = 137,
        AfflatusSolace = 16531,
        AfflatusRapture = 16534,
        Raise = 125,
        Benediction = 140,
        AfflatusMisery = 16535,
        Medica1 = 124,
        Medica2 = 133,
        Medica3 = 37010,
        Tetragrammaton = 3570,
        DivineBenison = 7432,
        Aquaveil = 25861,
        DivineCaress = 37011,
        Glare1 = 16533,
        Glare3 = 25859,
        Glare4 = 37009,
        Stone1 = 119,
        Stone2 = 127,
        Stone3 = 3568,
        Stone4 = 7431,
        Assize = 3571,
        Holy = 139,
        Holy3 = 25860,
        Aero = 121,
        Aero2 = 132,
        Dia = 16532,
        ThinAir = 7430,
        PresenceOfMind = 136,
        PlenaryIndulgence = 7433;

    internal static class Buffs
    {
        internal const ushort
        Regen = 158,
        Medica2 = 150,
        Medica3 = 3880,
        PresenceOfMind = 157,
        ThinAir = 1217,
        DivineBenison = 1218,
        Aquaveil = 2708,
        SacredSight = 3879,
        DivineGrace = 3881;
    }

    internal static class Debuffs
    {
        internal const ushort
        Aero = 143,
        Aero2 = 144,
        Dia = 1871;
    }

    internal static readonly Dictionary<uint, ushort>
        DiaList = new() {
                { Aero, Debuffs.Aero },
                { Aero2, Debuffs.Aero2 },
                { Dia, Debuffs.Dia }
        };

    private static WHMGauge Gauge => CustomComboFunctions.GetJobGauge<WHMGauge>();

    internal static class Config
    {
        internal static UserBool
            WHM_Raise_ThinAir = new("WHM_Raise_ThinAir");
    }

    internal class WHM_ST_DPS : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.WHM_ST_DPS;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is Stone1 or Stone2 or Stone3 or Stone4 or Glare1 or Glare3) && IsEnabled(Presets.WHM_ST_DPS))
            {
                if ((WasLastSpell(Medica1) || WasLastSpell(Medica2) || WasLastSpell(Medica3)) && !InCombat())
                {
                    ActionWatching.CombatActions.Clear();
                }

                if (CanWeave(actionID, ActionWatching.LastGCD) && (ActionWatching.NumberOfGcdsUsed >= 5 || Service.Configuration.IgnoreGCDChecks || LevelIgnoreGCD()))
                {
                    if (IsEnabled(Presets.WHM_ST_DPS_PresenceOfMind) && ActionReady(PresenceOfMind) && TargetIsBoss())
                    {
                        return PresenceOfMind;
                    }

                    if (IsEnabled(Presets.WHM_ST_DPS_Assize) && ActionReady(Assize))
                    {
                        return Assize;
                    }
                }

                if (IsEnabled(Presets.WHM_ST_DPS_Misery) && ActionReady(AfflatusMisery) && Gauge.BloodLily == 3
                    && (HasEffect(Buffs.PresenceOfMind) || (Gauge.Lily >= 2 && Gauge.LilyTimer >= 15000) || Gauge.Lily == 3))
                {
                    return AfflatusMisery;
                }

                if (IsEnabled(Presets.WHM_ST_DPS_Misery) && ActionReady(AfflatusRapture)
                    && (Gauge.Lily == 3 || (Gauge.Lily == 2 && Gauge.LilyTimer >= 17500)
                    || (IsOnCooldown(PresenceOfMind) && GetCooldownRemainingTime(PresenceOfMind) <= 5 && Gauge.BloodLily != 3)))
                {
                    return AfflatusRapture;
                }

                if (IsEnabled(Presets.WHM_ST_DPS_Dia) && ActionReady(OriginalHook(Dia))
                    && (ActionWatching.NumberOfGcdsUsed >= 1 || Service.Configuration.IgnoreGCDChecks || LevelIgnoreGCD()) && TargetWorthDoT()
                    && (!TargetHasEffect(DiaList[OriginalHook(Dia)]) || TargetEffectRemainingTime(DiaList[OriginalHook(Dia)]) <= 3))
                {
                    return OriginalHook(Dia);
                }

                if (IsEnabled(Presets.WHM_ST_DPS_PresenceOfMind) && EffectStacks(Buffs.SacredSight) > 0 && ActionReady(Glare4))
                {
                    return Glare4;
                }
            }

            return actionID;
        }
    }

    internal class WHM_AoE_DPS : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.WHM_AoE_DPS;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is Holy or Holy3) && IsEnabled(Presets.WHM_AoE_DPS))
            {
                if (CanWeave(actionID, ActionWatching.LastGCD))
                {
                    if (IsEnabled(Presets.WHM_AoE_DPS_PresenceOfMind) && ActionReady(PresenceOfMind))
                    {
                        return PresenceOfMind;
                    }

                    if (IsEnabled(Presets.WHM_AoE_DPS_Assize) && ActionReady(Assize))
                    {
                        return Assize;
                    }
                }

                if (IsEnabled(Presets.WHM_AoE_DPS_Misery) && ActionReady(AfflatusMisery) && Gauge.BloodLily == 3
                    && (HasEffect(Buffs.PresenceOfMind) || (Gauge.Lily >= 2 && Gauge.LilyTimer >= 15000) || Gauge.Lily == 3))
                {
                    return AfflatusMisery;
                }

                if (IsEnabled(Presets.WHM_AoE_DPS_Misery) && ActionReady(AfflatusRapture)
                    && (Gauge.Lily == 3 || (Gauge.Lily == 2 && Gauge.LilyTimer >= 17500)
                    || (!IsOnCooldown(PresenceOfMind) && GetCooldownRemainingTime(PresenceOfMind) <= 5 && Gauge.BloodLily != 3)))
                {
                    return AfflatusRapture;
                }

                if (IsEnabled(Presets.WHM_AoE_DPS_PresenceOfMind) && EffectStacks(Buffs.SacredSight) > 0 && ActionReady(Glare4))
                {
                    return Glare4;
                }
            }

            return actionID;
        }
    }

    internal class WHM_ST_Heals : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.WHM_ST_Heals;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is Cure1 or Cure2) && IsEnabled(Presets.WHM_ST_Heals))
            {
                if (IsEnabled(Presets.WHM_ST_Heals_Tetragrammaton) && ActionReady(Tetragrammaton))
                {
                    return Tetragrammaton;
                }

                if (IsEnabled(Presets.WHM_ST_Heals_Misery) && ActionReady(AfflatusMisery) && Gauge.BloodLily == 3 && HasBattleTarget())
                {
                    return AfflatusMisery;
                }

                if (IsEnabled(Presets.WHM_ST_Heals_Solace) && ActionReady(AfflatusSolace) && Gauge.Lily > 0)
                {
                    return AfflatusSolace;
                }

                if (ActionReady(Regen) && ((HasFriendlyTarget() && !TargetHasEffect(Buffs.Regen)) || (!HasFriendlyTarget() && !HasEffect(Buffs.Regen))))
                {
                    return Regen;
                }

                if (IsEnabled(Presets.WHM_ST_Heals_ThinAir) && ActionReady(ThinAir) && !HasEffect(Buffs.ThinAir) && !WasLastSpell(AfflatusSolace)
                    && !WasLastSpell(AfflatusMisery) && !WasLastSpell(Regen))
                {
                    return ThinAir;
                }

                if (ActionReady(Cure2))
                {
                    return Cure2;
                }

                return Cure1;
            }

            return actionID;
        }
    }

    internal class WHM_AoE_Heals : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.WHM_AoE_Heals;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is Medica1 or Medica2 or Medica3) && IsEnabled(Presets.WHM_AoE_Heals))
            {
                if (IsEnabled(Presets.WHM_AoE_Heals_Plenary) && ActionReady(PlenaryIndulgence))
                {
                    return PlenaryIndulgence;
                }

                if (IsEnabled(Presets.WHM_AoE_Heals_Misery) && ActionReady(AfflatusMisery) && Gauge.BloodLily == 3 && HasBattleTarget())
                {
                    return AfflatusMisery;
                }

                if (IsEnabled(Presets.WHM_AoE_Heals_Rapture) && ActionReady(AfflatusRapture) && Gauge.Lily > 0 && HasBattleTarget())
                {
                    return AfflatusRapture;
                }

                if (IsEnabled(Presets.WHM_AoE_Heals_ThinAir) && ActionReady(ThinAir) && !HasEffect(Buffs.ThinAir)
                    && !WasLastSpell(AfflatusRapture) && !WasLastSpell(AfflatusMisery))
                {
                    return ThinAir;
                }

                if (IsEnabled(Presets.WHM_AoEHeals_Medica2) && !HasEffect(Buffs.Medica2) && !HasEffect(Buffs.Medica3)
                    && !WasLastSpell(Medica2) && !WasLastSpell(Medica3) && (ActionReady(Medica2) || ActionReady(Medica3)))
                {
                    return OriginalHook(Medica3);
                }

                if (IsEnabled(Presets.WHM_AoE_Heals_Cure3) && ActionReady(Cure3))
                {
                    return Cure3;
                }

                return Medica1;
            }

            return actionID;
        }
    }
}
