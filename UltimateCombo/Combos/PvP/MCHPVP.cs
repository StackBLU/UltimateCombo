using UltimateCombo.Combos.General;
using UltimateCombo.Core;

namespace UltimateCombo.Combos.PvP;

internal static class MCHPvP
{
    internal const uint
        BlastCharge = 29402,
        FullMetalField = 41469,
        BlazingShot = 41468,
        Wildfire = 29409,
        Detonator = 41470,

        Scattergun = 29404,

        Analysis = 29414,
        Drill = 29405,
        Bioblaster = 29406,
        AirAnchor = 29407,
        ChainSaw = 29408,

        BishopAutoturret = 29412,
        AetherMortar = 29413,

        MarksmansSpite = 29415;

    internal static class Buffs
    {
        internal const ushort
            Heat = 3148,
            Overheated = 3149,

            Analysis = 3158,
            DrillPrimed = 3150,
            BioblasterPrimed = 3151,
            AirAnchorPrimed = 3152,
            ChainSawPrimed = 3153,

            Wildfire = 2018,

            BishopActive = 3155,
            AetherMortar = 3156;
    }

    internal class Debuffs
    {
        internal const ushort
            Bioblaster = 2019,
            ChainSaw = 3154,
            Wildfire = 1323;
    }

    internal class MCHPvP_Combo : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.MCHPvP_Combo;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is BlastCharge or BlazingShot) && IsEnabled(Presets.MCHPvP_Combo))
            {
                if (IsEnabled(Presets.MCHPvP_Weapons) && ActionReady(OriginalHook(Drill)) && !HasEffect(Buffs.Overheated)
                    && OriginalHook(Drill) == Drill && !WasLastWeaponskill(Drill)
                    && (ActionReady(Analysis) || HasEffect(Buffs.Analysis) || WasLastAction(Analysis)))
                {
                    if (!HasEffect(Buffs.Analysis) && !WasLastAction(Analysis))
                    {
                        return Analysis;
                    }

                    return Drill;
                }

                if (!TargetHasEffectAny(AllPvP.Buffs.Guard))
                {
                    if (IsEnabled(Presets.MCHPvP_MarksmansSpite) && GetLimitBreakCurrentValue() == GetLimitBreakMaxValue()
                        && CurrentHP > MaxMP / 3
                        && (WasLastAction(AirAnchor) || WasLastAction(Drill)
                        || (HasEffect(Debuffs.Wildfire) && EffectRemainingTime(Debuffs.Wildfire) <= 4)))
                    {
                        return MarksmansSpite;
                    }

                    if (CanWeave(actionID))
                    {
                        if (IsEnabled(Presets.MCHPvP_Wildfire) && ActionReady(Wildfire) && HasEffect(Buffs.Overheated)
                            && (WasLastWeaponskill(BlastCharge) || WasLastWeaponskill(FullMetalField)))
                        {
                            return Wildfire;
                        }
                    }

                    if (IsEnabled(Presets.MCHPvP_Scattergun) && ActionReady(Scattergun) && !HasEffect(Buffs.Overheated)
                        && (WasLastWeaponskill(BlazingShot) || InMeleeRange()) && InActionRange(Scattergun))
                    {
                        return Scattergun;
                    }

                    if (IsEnabled(Presets.MCHPvP_FullMetalField) && ActionReady(FullMetalField))
                    {
                        return FullMetalField;
                    }

                    if (!HasEffect(Buffs.Overheated))
                    {
                        if (IsEnabled(Presets.MCHPvP_Weapons) && ActionReady(OriginalHook(Drill))
                            && OriginalHook(Drill) == Bioblaster && !WasLastWeaponskill(Bioblaster) && InActionRange(Bioblaster))
                        {
                            return Bioblaster;
                        }

                        if (IsEnabled(Presets.MCHPvP_Weapons) && ActionReady(OriginalHook(Drill))
                            && OriginalHook(Drill) == AirAnchor && !WasLastWeaponskill(AirAnchor)
                            && !TargetHasEffectAny(AllPvP.Buffs.Resilience)
                            && (ActionReady(Analysis) || HasEffect(Buffs.Analysis) || WasLastAction(Analysis)))
                        {
                            if (!HasEffect(Buffs.Analysis) && !WasLastAction(Analysis))
                            {
                                return Analysis;
                            }

                            return AirAnchor;
                        }

                        if (IsEnabled(Presets.MCHPvP_Weapons) && ActionReady(OriginalHook(Drill))
                            && OriginalHook(Drill) == ChainSaw && !WasLastWeaponskill(ChainSaw))
                        {
                            return ChainSaw;
                        }
                    }
                }
            }

            return actionID;
        }
    }
}
