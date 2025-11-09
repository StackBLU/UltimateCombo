using Dalamud.Game.ClientState.JobGauge.Types;
using System.Collections.Generic;
using System.Linq;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Combos.General;
using UltimateCombo.Core;
using UltimateCombo.Data;

namespace UltimateCombo.Combos.PvE;

internal static class SGE
{
    internal const byte JobID = 40;

    internal const uint
        Diagnosis = 24284,
        Prognosis = 24286,
        Physis = 24288,
        Druochole = 24296,
        Kerachole = 24298,
        Ixochole = 24299,
        Pepsis = 24301,
        Physis2 = 24302,
        Taurochole = 24303,
        Haima = 24305,
        Panhaima = 24311,
        Holos = 24310,
        EukrasianDiagnosis = 24291,
        EukrasianPrognosis1 = 24292,
        EukrasianPrognosis2 = 37034,
        Egeiro = 24287,

        Dosis1 = 24283,
        Dosis2 = 24306,
        Dosis3 = 24312,
        EukrasianDosis1 = 24293,
        EukrasianDosis2 = 24308,
        EukrasianDosis3 = 24314,
        Phlegma = 24289,
        Phlegma2 = 24307,
        Phlegma3 = 24313,
        Dyskrasia1 = 24297,
        Dyskrasia2 = 24315,
        Toxikon = 24304,
        Pneuma = 24318,
        EukrasianDyskrasia = 37032,
        Psyche = 37033,

        Soteria = 24294,
        Zoe = 24300,
        Krasis = 24317,
        Philosophia = 37035,

        Kardia = 24285,
        Eukrasia = 24290,
        Rhizomata = 24309;

    internal static class Buffs
    {
        internal const ushort
            Kardia = 2604,
            Kardion = 2605,
            Eukrasia = 2606,
            EukrasianDiagnosis = 2607,
            EukrasianPrognosis = 2609,
            Panhaima = 2613,
            Kerachole = 2618,
            Eudaimonia = 3899;
    }

    internal static class Debuffs
    {
        internal const ushort
            EukrasianDosis1 = 2614,
            EukrasianDosis2 = 2615,
            EukrasianDosis3 = 2616,
            EukrasianDyskrasia = 3897;
    }

    internal static readonly Dictionary<uint, ushort>
        DosisList = new()
        {
                { Dosis1, Debuffs.EukrasianDosis1 },
                { Dosis2, Debuffs.EukrasianDosis2 },
                { Dosis3, Debuffs.EukrasianDosis3 },
                { EukrasianDosis1, Debuffs.EukrasianDosis1 },
                { EukrasianDosis2, Debuffs.EukrasianDosis2 },
                { EukrasianDosis3, Debuffs.EukrasianDosis3 }
        };

    internal static SGEGauge Gauge => CustomComboFunctions.GetJobGauge<SGEGauge>();

    internal static class Config
    {
        internal static UserInt
            SGE_ST_DPS_Rhizo = new("SGE_ST_DPS_Rhizo", 0),
            SGE_ST_DPS_AddersgallProtect = new("SGE_ST_DPS_AddersgallProtect", 3),
            SGE_AoE_DPS_Rhizo = new("SGE_AoE_DPS_Rhizo", 0),
            SGE_AoE_DPS_AddersgallProtect = new("SGE_AoE_DPS_AddersgallProtect", 3);
    }

    internal class SGE_ST_DPS : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.SGE_ST_DPS;
        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is Dosis1 or Dosis2 or Dosis3 or EukrasianDosis1 or EukrasianDosis2 or EukrasianDosis3) && IsEnabled(Presets.SGE_ST_DPS))
            {
                if ((WasLastSpell(EukrasianPrognosis1) || WasLastSpell(EukrasianPrognosis2)) && !InCombat())
                {
                    ActionWatching.CombatActions.Clear();
                }

                if (CanWeave(actionID, ActionWatching.LastGCD) && (ActionWatching.NumberOfGcdsUsed >= 2 || Service.Configuration.IgnoreGCDChecks || LevelIgnoreGCD())
                    && !WasLastSpell(Eukrasia) && !WasLastSpell(EukrasianPrognosis1) && !WasLastSpell(EukrasianPrognosis2)
                    && !WasLastSpell(EukrasianDosis1) && !WasLastSpell(EukrasianDosis2) && !WasLastSpell(EukrasianDosis2)
                    && !WasLastSpell(EukrasianDyskrasia))
                {
                    if (IsEnabled(Presets.SGE_ST_DPS_Kardia) && ActionReady(Kardia) && InCombat() && TargetIsBoss()
                       && (!HasEffect(Buffs.Kardia)
                       || (HasEffect(Buffs.Kardion) && !IsTargetOfTarget() && GetPartyMembers().Any(x => x.GameObject == TargetOfTarget))
                       || (!TargetOfTargetHasEffect(Buffs.Kardion) && GetPartyMembers().Any(x => x.GameObject == TargetOfTarget))))
                    {
                        return Kardia;
                    }

                    if (IsEnabled(Presets.SGE_ST_DPS_Psyche) && ActionReady(Psyche)
                        && (ActionWatching.NumberOfGcdsUsed >= 6 || Service.Configuration.IgnoreGCDChecks || LevelIgnoreGCD()))
                    {
                        return Psyche;
                    }

                    if (IsEnabled(Presets.SGE_ST_DPS_Rhizo) && ActionReady(Rhizomata) && Gauge.Addersgall <= GetOptionValue(Config.SGE_ST_DPS_Rhizo))
                    {
                        return Rhizomata;
                    }

                    if (IsEnabled(Presets.SGE_ST_DPS_Soteria) && ActionReady(Soteria))
                    {
                        return Soteria;
                    }

                    if (IsEnabled(Presets.SGE_ST_DPS_AddersgallProtect) && ActionReady(Druochole)
                        && (Gauge.Addersgall >= GetOptionValue(Config.SGE_ST_DPS_AddersgallProtect)
                        || (Gauge.Addersgall == GetOptionValue(Config.SGE_ST_DPS_AddersgallProtect) - 1 && Gauge.AddersgallTimer >= 17500)))
                    {
                        return Druochole;
                    }
                }

                if (IsEnabled(Presets.SGE_ST_DPS_EDosis) && ActionReady(OriginalHook(Dosis1)) && HasBattleTarget() && TargetWorthDoT()
                   && (ActionWatching.NumberOfGcdsUsed >= 3 || Service.Configuration.IgnoreGCDChecks || LevelIgnoreGCD())
                   && (!TargetHasEffect(DosisList[OriginalHook(Dosis1)]) || TargetEffectRemainingTime(DosisList[OriginalHook(Dosis1)]) <= 3))
                {
                    if (ActionReady(Eukrasia) && !HasEffect(Buffs.Eukrasia))
                    {
                        return Eukrasia;
                    }

                    return OriginalHook(Dosis1);
                }

                if (IsEnabled(Presets.SGE_ST_DPS_Phlegma) && InActionRange(OriginalHook(Phlegma)) && ActionReady(OriginalHook(Phlegma))
                    && (ActionReady(Psyche) || !LevelChecked(Psyche) || WasLastSpell(OriginalHook(Phlegma)) || GetCooldownRemainingTime(Psyche) > 50)
                    && (ActionWatching.NumberOfGcdsUsed >= 5 || Service.Configuration.IgnoreGCDChecks || LevelIgnoreGCD()))
                {
                    return OriginalHook(Phlegma);
                }

                if (IsEnabled(Presets.SGE_ST_DPS_Toxikon) && IsMoving && ActionReady(Toxikon)
                     && !HasEffect(Buffs.Eukrasia) && Gauge.Addersting > 0 && !HasEffect(Common.Buffs.Swiftcast))
                {
                    return OriginalHook(Toxikon);
                }

                return OriginalHook(Dosis1);

            }

            return actionID;
        }
    }

    internal class SGE_AoE_DPS : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.SGE_AoE_DPS;
        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is Dyskrasia1 or Dyskrasia2 or EukrasianDyskrasia) && IsEnabled(Presets.SGE_AoE_DPS))
            {
                if (CanWeave(actionID, ActionWatching.LastGCD)
                    && !WasLastSpell(Eukrasia) && !WasLastSpell(EukrasianPrognosis1) && !WasLastSpell(EukrasianPrognosis2)
                    && !WasLastSpell(EukrasianDosis1) && !WasLastSpell(EukrasianDosis2) && !WasLastSpell(EukrasianDosis2)
                    && !WasLastSpell(EukrasianDyskrasia))
                {
                    if (IsEnabled(Presets.SGE_AoE_DPS_Kardia) && ActionReady(Kardia) && InCombat() && TargetIsBoss()
                       && (ActionWatching.NumberOfGcdsUsed >= 2 || Service.Configuration.IgnoreGCDChecks || LevelIgnoreGCD())
                       && (!HasEffect(Buffs.Kardia)
                       || (HasEffect(Buffs.Kardion) && !IsTargetOfTarget() && GetPartyMembers().Any(x => x.GameObject == TargetOfTarget))
                       || (!TargetOfTargetHasEffect(Buffs.Kardion) && GetPartyMembers().Any(x => x.GameObject == TargetOfTarget))))
                    {
                        return Kardia;
                    }

                    if (IsEnabled(Presets.SGE_AoE_DPS_Psyche) && ActionReady(Psyche) && CanWeave(actionID, ActionWatching.LastGCD))
                    {
                        return Psyche;
                    }

                    if (IsEnabled(Presets.SGE_AoE_DPS_Rhizo) && ActionReady(Rhizomata)
                        && Gauge.Addersgall <= GetOptionValue(Config.SGE_AoE_DPS_Rhizo))
                    {
                        return Rhizomata;
                    }

                    if (IsEnabled(Presets.SGE_AoE_DPS_Soteria) && ActionReady(Soteria))
                    {
                        return Soteria;
                    }

                    if (IsEnabled(Presets.SGE_AoE_DPS_AddersgallProtect) && ActionReady(Druochole)
                        && (Gauge.Addersgall >= GetOptionValue(Config.SGE_AoE_DPS_AddersgallProtect)
                        || (Gauge.Addersgall == GetOptionValue(Config.SGE_AoE_DPS_AddersgallProtect) - 1 && Gauge.AddersgallTimer >= 17500)))
                    {
                        return Druochole;
                    }
                }

                if (IsEnabled(Presets.SGE_AoE_DPS_EDyskrasia) && ActionReady(OriginalHook(Dyskrasia1))
                    && HasBattleTarget() && LevelChecked(EukrasianDyskrasia) && !WasLastSpell(EukrasianDyskrasia)
                    && (!TargetHasEffect(DosisList[OriginalHook(Dosis1)]) || TargetEffectRemainingTime(DosisList[OriginalHook(Dosis1)]) <= 3)
                    && (!TargetHasEffect(Debuffs.EukrasianDyskrasia) || TargetEffectRemainingTime(Debuffs.EukrasianDyskrasia) <= 3))
                {
                    if (ActionReady(Eukrasia) && !HasEffect(Buffs.Eukrasia))
                    {
                        return Eukrasia;
                    }

                    return OriginalHook(Dyskrasia1);
                }

                if (IsEnabled(Presets.SGE_AoE_DPS_Phlegma) && InActionRange(OriginalHook(Phlegma)) && ActionReady(OriginalHook(Phlegma))
                    && (ActionReady(Psyche) || !LevelChecked(Psyche) || WasLastSpell(OriginalHook(Phlegma)) || GetCooldownRemainingTime(Psyche) > 50))
                {
                    return OriginalHook(Phlegma);
                }

                return OriginalHook(Dyskrasia1);

            }

            return actionID;
        }
    }

    internal class SGE_ST_Heals : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.SGE_ST_Heals;
        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is Diagnosis or EukrasianDiagnosis) && IsEnabled(Presets.SGE_ST_Heals))
            {
                if (IsEnabled(Presets.SGE_ST_Heals_Krasis) && ActionReady(Krasis))
                {
                    return Krasis;
                }

                if (!HasEffect(Buffs.Eukrasia))
                {
                    if (IsEnabled(Presets.SGE_ST_Heals_Haima) && ActionReady(Haima))
                    {
                        return Haima;
                    }
                }
            }

            return actionID;
        }
    }

    internal class SGE_AoE_Heals : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.SGE_AoE_Heals;
        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is Prognosis or EukrasianPrognosis1 or EukrasianPrognosis2) && IsEnabled(Presets.SGE_AoE_Heals))
            {
                if (IsEnabled(Presets.SGE_AoE_Heals_Pepsis) && ActionReady(Pepsis)
                    && HasEffect(Buffs.EukrasianPrognosis) && !WasLastAction(EukrasianPrognosis1) && !WasLastAction(EukrasianPrognosis2))
                {
                    return Pepsis;
                }
            }

            return actionID;
        }
    }
}
