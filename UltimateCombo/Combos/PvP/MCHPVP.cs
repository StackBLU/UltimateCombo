using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvP
{
    internal static class MCHPvP
    {
        public const uint
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

        public static class Buffs
        {
            public const ushort
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

        internal class MCHPvP_Combo : CustomComboClass
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MCHPvP_Combo;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if ((actionID is BlastCharge or BlazingShot) && IsEnabled(CustomComboPreset.MCHPvP_Combo))
                {
                    if (IsEnabled(CustomComboPreset.MCHPvP_Weapons) && ActionReady(OriginalHook(Drill)) && !HasEffect(Buffs.Overheated)
                        && OriginalHook(Drill) == Drill && !WasLastWeaponskill(Drill)
                        && (ActionReady(Analysis) || HasEffect(Buffs.Analysis)))
                    {
                        if (!HasEffect(Buffs.Analysis) && !WasLastAbility(Analysis))
                        {
                            return Analysis;
                        }

                        return Drill;
                    }

                    if (!TargetHasEffectAny(PvPCommon.Buffs.Guard))
                    {
                        if (IsEnabled(CustomComboPreset.MCHPvP_MarksmansSpite) && GetLimitBreakCurrentValue() == GetLimitBreakMaxValue()
                            && (WasLastWeaponskill(AirAnchor) || (TargetHasEffect(Debuffs.Wildfire) && GetDebuffRemainingTime(Debuffs.Wildfire) <= 4)))
                        {
                            return MarksmansSpite;
                        }

                        if (CanWeave(actionID))
                        {
                            if (IsEnabled(CustomComboPreset.MCHPvP_Wildfire) && ActionReady(Wildfire) && HasEffect(Buffs.Overheated)
                                && (WasLastWeaponskill(BlastCharge) || WasLastWeaponskill(FullMetalField)))
                            {
                                return Wildfire;
                            }
                        }

                        if (IsEnabled(CustomComboPreset.MCHPvP_Scattergun) && ActionReady(Scattergun) && !HasEffect(Buffs.Overheated)
                            && (WasLastWeaponskill(BlazingShot) || InMeleeRange()) && InActionRange(Scattergun))
                        {
                            return Scattergun;
                        }

                        if (IsEnabled(CustomComboPreset.MCHPvP_FullMetalField) && ActionReady(FullMetalField))
                        {
                            return FullMetalField;
                        }

                        if (!HasEffect(Buffs.Overheated))
                        {
                            if (IsEnabled(CustomComboPreset.MCHPvP_Weapons) && ActionReady(OriginalHook(Drill))
                                && OriginalHook(Drill) == Bioblaster && !WasLastWeaponskill(Bioblaster) && InActionRange(Bioblaster))
                            {
                                return Bioblaster;
                            }

                            if (IsEnabled(CustomComboPreset.MCHPvP_Weapons) && ActionReady(OriginalHook(Drill))
                                && OriginalHook(Drill) == AirAnchor && !WasLastWeaponskill(AirAnchor)
                                && !TargetHasEffectAny(PvPCommon.Buffs.Resilience)
                                && (ActionReady(Analysis) || HasEffect(Buffs.Analysis)))
                            {
                                if (!HasEffect(Buffs.Analysis) && !WasLastAbility(Analysis))
                                {
                                    return Analysis;
                                }

                                return AirAnchor;
                            }

                            if (IsEnabled(CustomComboPreset.MCHPvP_Weapons) && ActionReady(OriginalHook(Drill))
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
}
