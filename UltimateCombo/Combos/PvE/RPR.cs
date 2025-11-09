using Dalamud.Game.ClientState.JobGauge.Types;

using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Core;
using UltimateCombo.Data;

namespace UltimateCombo.Combos.PvE;

internal class RPR
{
    internal const byte JobID = 39;

    internal const uint
        Slice = 24373,
        WaxingSlice = 24374,
        InfernalSlice = 24375,
        ShadowOfDeath = 24378,
        SoulSlice = 24380,
        SpinningScythe = 24376,
        NightmareScythe = 24377,
        WhorlOfDeath = 24379,
        SoulScythe = 24381,
        Gibbet = 24382,
        Gallows = 24383,
        Guillotine = 24384,
        UnveiledGibbet = 24390,
        UnveiledGallows = 24391,
        ExecutionersGibbet = 36970,
        ExecutionersGallows = 36971,
        ExecutionersGuillotine = 36972,

        BloodStalk = 24389,
        GrimSwathe = 24392,
        Gluttony = 24393,
        ArcaneCircle = 24405,
        PlentifulHarvest = 24385,
        Enshroud = 24394,
        Communio = 24398,
        LemuresSlice = 24399,
        LemuresScythe = 24400,
        VoidReaping = 24395,
        CrossReaping = 24396,
        GrimReaping = 24397,
        Sacrificium = 36969,
        Perfectio = 36973,
        HellsIngress = 24401,
        HellsEgress = 24402,
        Regress = 24403,
        ArcaneCrest = 24404,
        Harpe = 24386,
        Soulsow = 24387,
        HarvestMoon = 24388;

    internal static class Buffs
    {
        internal const ushort
            SoulReaver = 2587,
            ImmortalSacrifice = 2592,
            ArcaneCircle = 2599,
            EnhancedGibbet = 2588,
            EnhancedGallows = 2589,
            EnhancedVoidReaping = 2590,
            EnhancedCrossReaping = 2591,
            EnhancedHarpe = 2845,
            Enshrouded = 2593,
            Soulsow = 2594,
            Threshold = 2595,
            BloodsownCircle = 2972,
            IdealHost = 3905,
            Oblatio = 3857,
            Executioner = 3858,
            PerfectioParata = 3860;
    }

    internal static class Debuffs
    {
        internal const ushort
            DeathsDesign = 2586;
    }

    private static RPRGauge Gauge => CustomComboFunctions.GetJobGauge<RPRGauge>();

    internal static class Config
    {

    }

    internal class RPR_ST_DPS : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.RPR_ST_DPS;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is Slice or WaxingSlice or InfernalSlice) && IsEnabled(Presets.RPR_ST_DPS))
            {
                if (!InCombat() && !HasEffect(Buffs.Soulsow) && ActionReady(Soulsow) && IsEnabled(Presets.RPR_ST_Soulsow))
                {
                    return Soulsow;
                }

                if (!InCombat() && ActionReady(Harpe) && !InMeleeRange())
                {
                    return Harpe;
                }

                if (CanWeave(actionID, ActionWatching.LastGCD) && (ActionWatching.NumberOfGcdsUsed >= 3 || Service.Configuration.IgnoreGCDChecks || LevelIgnoreGCD()))
                {
                    if (IsEnabled(Presets.RPR_ST_Shield) && ActionReady(ArcaneCrest))
                    {
                        return ArcaneCrest;
                    }

                    if (IsEnabled(Presets.RPR_ST_Enshroud) && Gauge.VoidShroud == 2)
                    {
                        return LemuresSlice;
                    }

                    if (IsEnabled(Presets.RPR_ST_Enshroud) && HasEffect(Buffs.Oblatio))
                    {
                        return Sacrificium;
                    }

                    if (IsEnabled(Presets.RPR_ST_Arcane) && ActionReady(ArcaneCircle) && TargetIsBoss()
                        && !HasEffectAny(Buffs.ArcaneCircle))
                    {
                        return ArcaneCircle;
                    }

                    if (IsEnabled(Presets.RPR_ST_Enshroud) && ActionReady(Enshroud)
                        && (Gauge.Shroud >= 50 || HasEffect(Buffs.IdealHost))
                        && !HasEffect(Buffs.Enshrouded) && !HasEffect(Buffs.ImmortalSacrifice))
                    {
                        return Enshroud;
                    }

                    if (IsEnabled(Presets.RPR_ST_Gluttony) && ActionReady(Gluttony) && Gauge.Soul >= 50
                        && !HasEffect(Buffs.SoulReaver) && !HasEffect(Buffs.Executioner) && !HasEffect(Buffs.Enshrouded))
                    {
                        return Gluttony;
                    }

                    if (IsEnabled(Presets.RPR_ST_BloodStalk) && ActionReady(BloodStalk) && Gauge.Soul >= 50
                        && !HasEffect(Buffs.SoulReaver) && !HasEffect(Buffs.Executioner) && !HasEffect(Buffs.Enshrouded))
                    {
                        return OriginalHook(BloodStalk);
                    }
                }

                if (IsEnabled(Presets.RPR_ST_ShadowOfDeath) && ActionReady(ShadowOfDeath)
                    && !HasEffect(Buffs.SoulReaver) && !HasEffect(Buffs.Executioner) && TargetWorthDoT()
                    && (TargetEffectRemainingTime(Debuffs.DeathsDesign) < 3
                    || (GetCooldownRemainingTime(ArcaneCircle) < 10 && TargetEffectRemainingTime(Debuffs.DeathsDesign) < 30)))
                {
                    return ShadowOfDeath;
                }

                if (IsEnabled(Presets.RPR_ST_Soulsow) && HasEffect(Buffs.Soulsow)
                    && (ActionWatching.NumberOfGcdsUsed > 2 || Service.Configuration.IgnoreGCDChecks || LevelIgnoreGCD()))
                {
                    return HarvestMoon;
                }

                if (IsEnabled(Presets.RPR_ST_PlentifulHarvest) && !HasEffect(Buffs.BloodsownCircle)
                    && HasEffect(Buffs.ImmortalSacrifice)
                    && !HasEffect(Buffs.SoulReaver) && !HasEffect(Buffs.Executioner))
                {
                    return PlentifulHarvest;
                }

                if (IsEnabled(Presets.RPR_ST_Enshroud) && Gauge.LemureShroud > 1)
                {
                    if (WasLastWeaponskill(OriginalHook(VoidReaping)))
                    {
                        return OriginalHook(CrossReaping);
                    }

                    return OriginalHook(VoidReaping);
                }

                if (IsEnabled(Presets.RPR_ST_Enshroud) && ActionReady(Communio) && Gauge.LemureShroud == 1)
                {
                    return Communio;
                }

                if (IsEnabled(Presets.RPR_ST_PlentifulHarvest) && HasEffect(Buffs.PerfectioParata))
                {
                    return Perfectio;
                }

                if (IsEnabled(Presets.RPR_ST_GibbetGallows) && (HasEffect(Buffs.SoulReaver) || HasEffect(Buffs.Executioner)))
                {
                    if (HasEffect(Buffs.EnhancedGibbet))
                    {
                        return OriginalHook(Gibbet);
                    }

                    return OriginalHook(Gallows);
                }

                if (IsEnabled(Presets.RPR_ST_SoulSlice) && ActionReady(SoulSlice) && Gauge.Soul <= 40)
                {
                    return SoulSlice;
                }

                if (IsEnabled(Presets.RPR_ST_Harpe) && ActionReady(Harpe) && OutOfMeleeRange()
                    && HasEffect(Buffs.EnhancedHarpe))
                {
                    return Harpe;
                }

                if (ComboTime > 0)
                {
                    if (lastComboMove is WaxingSlice && ActionReady(InfernalSlice))
                    {
                        return InfernalSlice;
                    }

                    if (lastComboMove is Slice && ActionReady(WaxingSlice))
                    {
                        return WaxingSlice;
                    }
                }

                return Slice;
            }

            return actionID;
        }
    }

    internal class RPR_AoE_DPS : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.RPR_AoE_DPS;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is SpinningScythe or NightmareScythe) && IsEnabled(Presets.RPR_AoE_DPS))
            {
                if (IsEnabled(Presets.RPR_AoE_Soulsow) && !InCombat() && !HasEffect(Buffs.Soulsow) && ActionReady(Soulsow))
                {
                    return Soulsow;
                }

                if (CanWeave(actionID, ActionWatching.LastGCD) && InCombat())
                {
                    if (IsEnabled(Presets.RPR_AoE_Shield) && ActionReady(ArcaneCrest))
                    {
                        return ArcaneCrest;
                    }

                    if (IsEnabled(Presets.RPR_AoE_Enshroud) && Gauge.VoidShroud == 2)
                    {
                        return LemuresScythe;
                    }

                    if (IsEnabled(Presets.RPR_AoE_Enshroud) && HasEffect(Buffs.Oblatio))
                    {
                        return Sacrificium;
                    }

                    if (IsEnabled(Presets.RPR_AoE_Arcane) && ActionReady(ArcaneCircle) && TargetIsBoss()
                        && !HasEffectAny(Buffs.ArcaneCircle))
                    {
                        return ArcaneCircle;
                    }

                    if (IsEnabled(Presets.RPR_AoE_Enshroud) && ActionReady(Enshroud)
                        && (Gauge.Shroud >= 50 || HasEffect(Buffs.IdealHost))
                        && !HasEffect(Buffs.Enshrouded) && !HasEffect(Buffs.ImmortalSacrifice))
                    {
                        return Enshroud;
                    }

                    if (IsEnabled(Presets.RPR_AoE_Gluttony) && ActionReady(Gluttony) && Gauge.Soul >= 50
                        && !HasEffect(Buffs.SoulReaver) && !HasEffect(Buffs.Executioner) && !HasEffect(Buffs.Enshrouded))
                    {
                        return Gluttony;
                    }

                    if (IsEnabled(Presets.RPR_AoE_GrimSwathe) && ActionReady(GrimSwathe) && Gauge.Soul >= 50
                        && !HasEffect(Buffs.SoulReaver) && !HasEffect(Buffs.Executioner) && !HasEffect(Buffs.Enshrouded))
                    {
                        return GrimSwathe;
                    }
                }

                if (IsEnabled(Presets.RPR_AoE_WhorlOfDeath) && ActionReady(WhorlOfDeath)
                    && !HasEffect(Buffs.SoulReaver) && !HasEffect(Buffs.Executioner) && TargetWorthDoT()
                    && (TargetEffectRemainingTime(Debuffs.DeathsDesign) < 15
                    || (GetCooldownRemainingTime(ArcaneCircle) < 10 && TargetEffectRemainingTime(Debuffs.DeathsDesign) < 30)))
                {
                    return WhorlOfDeath;
                }

                if (IsEnabled(Presets.RPR_AoE_Soulsow) && HasEffect(Buffs.Soulsow))
                {
                    return HarvestMoon;
                }

                if (IsEnabled(Presets.RPR_AoE_PlentifulHarvest) && !HasEffect(Buffs.BloodsownCircle)
                    && HasEffect(Buffs.ImmortalSacrifice)
                    && !HasEffect(Buffs.SoulReaver) && !HasEffect(Buffs.Executioner))
                {
                    return PlentifulHarvest;
                }

                if (IsEnabled(Presets.RPR_AoE_Enshroud) && Gauge.LemureShroud > 1)
                {
                    return OriginalHook(GrimReaping);
                }

                if (IsEnabled(Presets.RPR_AoE_Enshroud) && ActionReady(Communio) && Gauge.LemureShroud == 1)
                {
                    return Communio;
                }

                if (IsEnabled(Presets.RPR_AoE_PlentifulHarvest) && HasEffect(Buffs.PerfectioParata))
                {
                    return Perfectio;
                }

                if (IsEnabled(Presets.RPR_AoE_Guillotine) && ActionReady(Guillotine)
                    && (HasEffect(Buffs.SoulReaver) || HasEffect(Buffs.Executioner)))
                {
                    return OriginalHook(Guillotine);
                }

                if (IsEnabled(Presets.RPR_AoE_SoulScythe) && ActionReady(SoulScythe) && Gauge.Soul <= 40)
                {
                    return SoulScythe;
                }

                if (ComboTime > 0)
                {
                    if (lastComboMove is SpinningScythe)
                    {
                        return NightmareScythe;
                    }
                }

                return SpinningScythe;
            }

            return actionID;
        }
    }

    internal class RPR_BloodGluttony : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.RPR_BloodGluttony;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is Gluttony or BloodStalk) && IsEnabled(Presets.RPR_BloodGluttony))
            {
                if (Gauge.Soul >= 50 && !HasEffect(Buffs.SoulReaver) && !HasEffect(Buffs.Executioner) && !HasEffect(Buffs.Enshrouded))
                {
                    if (ActionReady(Gluttony))
                    {
                        return Gluttony;
                    }

                    return OriginalHook(BloodStalk);
                }

                return Gluttony;
            }

            return actionID;
        }
    }

    internal class RPR_GibbetGallows : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.RPR_GibbetGallows;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is Gibbet or Gallows) && IsEnabled(Presets.RPR_GibbetGallows))
            {
                if (HasEffect(Buffs.EnhancedGibbet))
                {
                    return OriginalHook(Gibbet);
                }

                return OriginalHook(Gallows);
            }

            return actionID;
        }
    }

    internal class RPR_Regress : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.RPR_Regress;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is HellsIngress or HellsEgress) && IsEnabled(Presets.RPR_Regress))
            {
                if (HasEffect(Buffs.Threshold))
                {
                    return Regress;
                }
            }

            return actionID;
        }
    }
}
