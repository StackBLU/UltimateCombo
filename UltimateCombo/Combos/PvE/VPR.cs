using Dalamud.Game.ClientState.JobGauge.Enums;
using Dalamud.Game.ClientState.JobGauge.Types;

using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Core;
using UltimateCombo.Data;

namespace UltimateCombo.Combos.PvE;

internal class VPR
{
    internal const byte JobID = 41;

    internal const uint
        ReavingFangs = 34607,
        ReavingMaw = 34615,
        Vicewinder = 34620,
        HuntersCoil = 34621,
        HuntersDen = 34624,
        HuntersSnap = 39166,
        Vicepit = 34623,
        RattlingCoil = 39189,
        Reawaken = 34626,
        SerpentsIre = 34647,
        SerpentsTail = 35920,
        Slither = 34646,
        SteelFangs = 34606,
        SteelMaw = 34614,
        SwiftskinsCoil = 34622,
        SwiftskinsDen = 34625,
        Twinblood = 35922,
        Twinfang = 35921,
        TwinbloodThresh = 34639,
        TwinfangThresh = 34638,
        UncoiledFury = 34633,
        WrithingSnap = 34632,
        SwiftskinsSting = 34609,
        TwinfangBite = 34636,
        TwinbloodBite = 34637,
        UncoiledTwinfang = 34644,
        UncoiledTwinblood = 34645,
        HindstingStrike = 34612,
        DeathRattle = 34634,
        HuntersSting = 34608,
        HindsbaneFang = 34613,
        FlankstingStrike = 34610,
        FlanksbaneFang = 34611,
        HuntersBite = 34616,
        JaggedMaw = 34618,
        SwiftskinsBite = 34617,
        BloodiedMaw = 34619,
        FirstGeneration = 34627,
        FirstLegacy = 34640,
        SecondGeneration = 34628,
        SecondLegacy = 34641,
        ThirdGeneration = 34629,
        ThirdLegacy = 34642,
        FourthGeneration = 34630,
        FourthLegacy = 34643,
        Ouroboros = 34631,
        LastLash = 34635;

    internal static class Buffs
    {
        internal const ushort
            FellhuntersVenom = 3659,
            FellskinsVenom = 3660,
            FlanksbaneVenom = 3646,
            FlankstungVenom = 3645,
            HindstungVenom = 3647,
            HindsbaneVenom = 3648,
            GrimhuntersVenom = 3649,
            GrimskinsVenom = 3650,
            HuntersVenom = 3657,
            SwiftskinsVenom = 3658,
            HuntersInstinct = 3668,
            Swiftscaled = 3669,
            Reawakened = 3670,
            ReadyToReawaken = 3671,
            PoisedForTwinfang = 3665,
            PoisedForTwinblood = 3666,
            HonedReavers = 3772,
            HonedSteel = 3672;
    }

    internal static VPRGauge Gauge => CustomComboFunctions.GetJobGauge<VPRGauge>();

    internal static class Config
    {

    }

    internal class VPR_ST_DPS : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.VPR_ST_DPS;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is SteelFangs or ReavingFangs) && IsEnabled(Presets.VPR_ST_DPS))
            {
                if (CanWeave(actionID))
                {
                    if (IsEnabled(Presets.VPR_ST_Uncoiled) && HasEffect(Buffs.PoisedForTwinfang))
                    {
                        return UncoiledTwinfang;
                    }

                    if (IsEnabled(Presets.VPR_ST_Uncoiled) && HasEffect(Buffs.PoisedForTwinblood))
                    {
                        return UncoiledTwinblood;
                    }

                    if (IsEnabled(Presets.VPR_ST_Vicewinder) && HasEffect(Buffs.SwiftskinsVenom))
                    {
                        return TwinbloodBite;
                    }

                    if (IsEnabled(Presets.VPR_ST_Vicewinder) && HasEffect(Buffs.HuntersVenom))
                    {
                        return TwinfangBite;
                    }

                    if (IsEnabled(Presets.VPR_ST_Finishers) && ActionReady(OriginalHook(SerpentsTail))
                        && Gauge.SerpentCombo is SerpentCombo.DeathRattle)
                    {
                        return DeathRattle;
                    }

                    if (IsEnabled(Presets.VPR_ST_Reawaken) && ActionReady(OriginalHook(SerpentsTail)) &&
                        (Gauge.SerpentCombo is SerpentCombo.FirstLegacy ||
                        Gauge.SerpentCombo is SerpentCombo.SecondLegacy ||
                        Gauge.SerpentCombo is SerpentCombo.ThirdLegacy ||
                        Gauge.SerpentCombo is SerpentCombo.FourthLegacy))
                    {
                        return OriginalHook(SerpentsTail);
                    }

                    if (IsEnabled(Presets.VPR_ST_SerpentsIre) && ActionReady(SerpentsIre) && TargetIsBoss())
                    {
                        return SerpentsIre;
                    }
                }

                if (IsEnabled(Presets.VPR_ST_Reawaken) && HasEffect(Buffs.Reawakened))
                {
                    if (Gauge.AnguineTribute is 5 || (Gauge.AnguineTribute is 4 && !LevelChecked(Ouroboros)))
                    {
                        return OriginalHook(SteelMaw);
                    }

                    if (Gauge.AnguineTribute is 4 || (Gauge.AnguineTribute is 3 && !LevelChecked(Ouroboros)))
                    {
                        return OriginalHook(ReavingMaw);
                    }

                    if (Gauge.AnguineTribute is 3 || (Gauge.AnguineTribute is 2 && !LevelChecked(Ouroboros)))
                    {
                        return OriginalHook(HuntersDen);
                    }

                    if (Gauge.AnguineTribute is 2 || (Gauge.AnguineTribute is 1 && !LevelChecked(Ouroboros)))
                    {
                        return OriginalHook(SwiftskinsDen);
                    }

                    if (Gauge.AnguineTribute is 1)
                    {
                        return OriginalHook(Reawaken);
                    }
                }

                if (EffectRemainingTime(Buffs.Swiftscaled) > 10 && EffectRemainingTime(Buffs.HuntersInstinct) > 10
                    && !Gauge.DreadCombo.HasFlag(DreadCombo.Dreadwinder)
                    && !Gauge.DreadCombo.HasFlag(DreadCombo.SwiftskinsCoil)
                    && !Gauge.DreadCombo.HasFlag(DreadCombo.HuntersCoil))
                {
                    if (IsEnabled(Presets.VPR_ST_Reawaken) && ActionReady(Reawaken)
                        && ((TargetWorthDoT() && (GetCooldownRemainingTime(SerpentsIre) > 30 || Gauge.SerpentOffering == 100)) || BossAlmostDead())
                        && (Gauge.SerpentOffering >= 50 || HasEffect(Buffs.ReadyToReawaken)))
                    {
                        return Reawaken;
                    }

                    if (IsEnabled(Presets.VPR_ST_Uncoiled) && ActionReady(UncoiledFury) && Gauge.RattlingCoilStacks > 0
                        && (GetCooldownRemainingTime(SerpentsIre) > 60 || Gauge.RattlingCoilStacks == 3 || BossAlmostDead()))
                    {
                        return UncoiledFury;
                    }
                }

                if (IsEnabled(Presets.VPR_ST_Vicewinder) && Gauge.DreadCombo.HasFlag(DreadCombo.SwiftskinsCoil))
                {
                    return HuntersCoil;
                }

                if (IsEnabled(Presets.VPR_ST_Vicewinder) && Gauge.DreadCombo.HasFlag(DreadCombo.HuntersCoil))
                {
                    return SwiftskinsCoil;
                }

                if (IsEnabled(Presets.VPR_ST_Vicewinder) && Gauge.DreadCombo.HasFlag(DreadCombo.Dreadwinder))
                {
                    if (EffectRemainingTime(Buffs.Swiftscaled) <= EffectRemainingTime(Buffs.HuntersInstinct))
                    {
                        return SwiftskinsCoil;
                    }

                    if (EffectRemainingTime(Buffs.HuntersInstinct) < EffectRemainingTime(Buffs.Swiftscaled))
                    {
                        return HuntersCoil;
                    }
                }

                if (IsEnabled(Presets.VPR_ST_Vicewinder) && ActionReady(Vicewinder)
                    && (WasLastWeaponskill(FlankstingStrike) || WasLastWeaponskill(FlanksbaneFang)
                    || WasLastWeaponskill(HindstingStrike) || WasLastWeaponskill(HindsbaneFang)
                    || (WasLastWeaponskill(SwiftskinsSting) && (ActionWatching.NumberOfGcdsUsed == 2 || Service.Configuration.IgnoreGCDChecks))))
                {
                    return Vicewinder;
                }

                if (IsEnabled(Presets.VPR_ST_UncoiledSnap) && ActionReady(WrithingSnap) && OutOfMeleeRange())
                {
                    if (ActionReady(UncoiledFury) && Gauge.RattlingCoilStacks > 0)
                    {
                        return UncoiledFury;
                    }

                    return WrithingSnap;
                }

                if (ComboTime > 0)
                {
                    if (lastComboMove is SteelFangs or ReavingFangs)
                    {
                        if (HasEffect(Buffs.FlanksbaneVenom) || HasEffect(Buffs.FlankstungVenom))
                        {
                            return OriginalHook(SteelFangs);
                        }

                        return OriginalHook(ReavingFangs);
                    }

                    if (lastComboMove is SwiftskinsSting or HuntersSting)
                    {
                        if (HasEffect(Buffs.FlankstungVenom))
                        {
                            return OriginalHook(SteelFangs);
                        }

                        if (HasEffect(Buffs.FlanksbaneVenom))
                        {
                            return OriginalHook(ReavingFangs);
                        }

                        if (HasEffect(Buffs.HindstungVenom))
                        {
                            return OriginalHook(SteelFangs);
                        }

                        return OriginalHook(ReavingFangs);
                    }
                }

                if (HasEffect(Buffs.HonedReavers))
                {
                    return OriginalHook(ReavingFangs);
                }

                return OriginalHook(SteelFangs);
            }

            return actionID;
        }
    }

    internal class VPR_AoE_DPS : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.VPR_AoE_DPS;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is SteelMaw or ReavingMaw) && IsEnabled(Presets.VPR_AoE_DPS))
            {
                if (CanWeave(actionID))
                {
                    if (IsEnabled(Presets.VPR_AoE_Uncoiled) && HasEffect(Buffs.PoisedForTwinfang))
                    {
                        return UncoiledTwinfang;
                    }

                    if (IsEnabled(Presets.VPR_AoE_Uncoiled) && HasEffect(Buffs.PoisedForTwinblood))
                    {
                        return UncoiledTwinblood;
                    }

                    if (IsEnabled(Presets.VPR_AoE_Vicepit) && HasEffect(Buffs.FellskinsVenom))
                    {
                        return TwinbloodThresh;
                    }

                    if (IsEnabled(Presets.VPR_AoE_Vicepit) && HasEffect(Buffs.FellhuntersVenom))
                    {
                        return TwinfangThresh;
                    }

                    if (IsEnabled(Presets.VPR_AoE_Finishers) && ActionReady(OriginalHook(LastLash))
                        && Gauge.SerpentCombo is SerpentCombo.LastLash)
                    {
                        return LastLash;
                    }

                    if (IsEnabled(Presets.VPR_AoE_Reawaken) && ActionReady(OriginalHook(SerpentsTail)) &&
                        (Gauge.SerpentCombo is SerpentCombo.FirstLegacy ||
                        Gauge.SerpentCombo is SerpentCombo.SecondLegacy ||
                        Gauge.SerpentCombo is SerpentCombo.ThirdLegacy ||
                        Gauge.SerpentCombo is SerpentCombo.FourthLegacy))
                    {
                        return OriginalHook(SerpentsTail);
                    }

                    if (IsEnabled(Presets.VPR_ST_SerpentsIre) && ActionReady(SerpentsIre))
                    {
                        return SerpentsIre;
                    }
                }

                if (IsEnabled(Presets.VPR_AoE_Reawaken) && HasEffect(Buffs.Reawakened))
                {
                    if (Gauge.AnguineTribute is 5 || (Gauge.AnguineTribute is 4 && !LevelChecked(Ouroboros)))
                    {
                        return OriginalHook(SteelMaw);
                    }

                    if (Gauge.AnguineTribute is 4 || (Gauge.AnguineTribute is 3 && !LevelChecked(Ouroboros)))
                    {
                        return OriginalHook(ReavingMaw);
                    }

                    if (Gauge.AnguineTribute is 3 || (Gauge.AnguineTribute is 2 && !LevelChecked(Ouroboros)))
                    {
                        return OriginalHook(HuntersDen);
                    }

                    if (Gauge.AnguineTribute is 2 || (Gauge.AnguineTribute is 1 && !LevelChecked(Ouroboros)))
                    {
                        return OriginalHook(SwiftskinsDen);
                    }

                    if (Gauge.AnguineTribute is 1)
                    {
                        return OriginalHook(Reawaken);
                    }
                }

                if (EffectRemainingTime(Buffs.Swiftscaled) > 10 && EffectRemainingTime(Buffs.HuntersInstinct) > 10 && TargetWorthDoT()
                    && !Gauge.DreadCombo.HasFlag(DreadCombo.PitOfDread)
                    && !Gauge.DreadCombo.HasFlag(DreadCombo.SwiftskinsDen) && !Gauge.DreadCombo.HasFlag(DreadCombo.HuntersDen))
                {
                    if (IsEnabled(Presets.VPR_AoE_Reawaken) && ActionReady(Reawaken)
                        && ((TargetWorthDoT() && (GetCooldownRemainingTime(SerpentsIre) > 30 || Gauge.SerpentOffering == 100)) || BossAlmostDead())
                        && (Gauge.SerpentOffering >= 50 || HasEffect(Buffs.ReadyToReawaken)))
                    {
                        return Reawaken;
                    }

                    if (IsEnabled(Presets.VPR_AoE_Uncoiled) && ActionReady(UncoiledFury) && Gauge.RattlingCoilStacks > 0
                        && (GetCooldownRemainingTime(SerpentsIre) > 60 || Gauge.RattlingCoilStacks == 3 || BossAlmostDead()))
                    {
                        return UncoiledFury;
                    }
                }

                if (IsEnabled(Presets.VPR_AoE_Vicepit) && Gauge.DreadCombo.HasFlag(DreadCombo.SwiftskinsDen))
                {
                    return HuntersDen;
                }

                if (IsEnabled(Presets.VPR_AoE_Vicepit) && Gauge.DreadCombo.HasFlag(DreadCombo.HuntersDen))
                {
                    return SwiftskinsDen;
                }

                if (IsEnabled(Presets.VPR_AoE_Vicepit) && Gauge.DreadCombo.HasFlag(DreadCombo.PitOfDread))
                {
                    if (EffectRemainingTime(Buffs.Swiftscaled) <= EffectRemainingTime(Buffs.HuntersInstinct))
                    {
                        return SwiftskinsDen;
                    }

                    if (EffectRemainingTime(Buffs.HuntersInstinct) < EffectRemainingTime(Buffs.Swiftscaled))
                    {
                        return HuntersDen;
                    }
                }

                if (IsEnabled(Presets.VPR_AoE_Vicepit) && ActionReady(Vicepit)
                    && (WasLastWeaponskill(JaggedMaw) || WasLastWeaponskill(BloodiedMaw)))
                {
                    return Vicepit;
                }

                if (ComboTime > 0)
                {
                    if (lastComboMove is SteelMaw or ReavingMaw)
                    {
                        if (EffectRemainingTime(Buffs.Swiftscaled) > EffectRemainingTime(Buffs.HuntersInstinct))
                        {
                            return OriginalHook(SteelMaw);
                        }

                        return OriginalHook(ReavingMaw);
                    }

                    if (lastComboMove is SwiftskinsBite or HuntersBite)
                    {
                        if (HasEffect(Buffs.GrimhuntersVenom))
                        {
                            return OriginalHook(SteelMaw);
                        }

                        return OriginalHook(ReavingMaw);
                    }
                }

                if (HasEffect(Buffs.HonedReavers))
                {
                    return OriginalHook(ReavingMaw);
                }

                return OriginalHook(SteelMaw);
            }

            return actionID;
        }
    }

    internal class VPR_Vicewinder : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.VPR_Vicewinder;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is Vicewinder or HuntersCoil or SwiftskinsCoil) && IsEnabled(Presets.VPR_Vicewinder))
            {
                if (CanWeave(SwiftskinsCoil))
                {
                    if (HasEffect(Buffs.SwiftskinsVenom))
                    {
                        return TwinbloodBite;
                    }

                    if (HasEffect(Buffs.HuntersVenom))
                    {
                        return TwinfangBite;
                    }
                }

                if (Gauge.DreadCombo.HasFlag(DreadCombo.SwiftskinsCoil))
                {
                    return HuntersCoil;
                }

                if (Gauge.DreadCombo.HasFlag(DreadCombo.HuntersCoil))
                {
                    return SwiftskinsCoil;
                }

                if (Gauge.DreadCombo.HasFlag(DreadCombo.Dreadwinder))
                {
                    if (EffectRemainingTime(Buffs.Swiftscaled) <= EffectRemainingTime(Buffs.HuntersInstinct))
                    {
                        return SwiftskinsCoil;
                    }

                    if (EffectRemainingTime(Buffs.HuntersInstinct) < EffectRemainingTime(Buffs.Swiftscaled))
                    {
                        return HuntersCoil;
                    }
                }
            }

            return actionID;
        }
    }

    internal class VPR_Vicepit : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.VPR_Vicepit;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is Vicepit or HuntersDen or SwiftskinsDen) && IsEnabled(Presets.VPR_Vicepit))
            {
                if (CanWeave(SwiftskinsDen))
                {
                    if (HasEffect(Buffs.FellskinsVenom))
                    {
                        return TwinbloodThresh;
                    }

                    if (HasEffect(Buffs.FellhuntersVenom))
                    {
                        return TwinfangThresh;
                    }
                }

                if (Gauge.DreadCombo.HasFlag(DreadCombo.SwiftskinsDen))
                {
                    return HuntersDen;
                }

                if (Gauge.DreadCombo.HasFlag(DreadCombo.HuntersDen))
                {
                    return SwiftskinsDen;
                }

                if (Gauge.DreadCombo.HasFlag(DreadCombo.PitOfDread))
                {
                    if (EffectRemainingTime(Buffs.Swiftscaled) <= EffectRemainingTime(Buffs.HuntersInstinct))
                    {
                        return SwiftskinsDen;
                    }

                    if (EffectRemainingTime(Buffs.HuntersInstinct) < EffectRemainingTime(Buffs.Swiftscaled))
                    {
                        return HuntersDen;
                    }
                }
            }

            return actionID;
        }
    }

    internal class VPR_Uncoiled : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.VPR_Uncoiled;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (actionID is UncoiledFury && IsEnabled(Presets.VPR_Uncoiled))
            {
                if (CanWeave(UncoiledFury))
                {
                    if (HasEffect(Buffs.PoisedForTwinfang))
                    {
                        return UncoiledTwinfang;
                    }

                    if (HasEffect(Buffs.PoisedForTwinblood))
                    {
                        return UncoiledTwinblood;
                    }
                }
            }

            return actionID;
        }
    }
}
