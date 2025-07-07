using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvP
{
    internal static class RDMPvP
    {
        public const uint
            Jolt3 = 41486,
            GrandImpact = 41487,

            EnchantedRiposte = 41488,
            EnchantedZwerchhau = 41489,
            EnchantedRedoublement = 41490,
            Scorch = 41491,

            Resolution = 41492,

            Embolden = 41494,
            Prefulgence = 41495,

            CorpsACorps = 29699,
            Displacement = 29700,

            Forte = 41496,
            ViceOfThorns = 41493,

            SouthernCross = 41498;

        public static class Buffs
        {
            public const ushort
                Dualcast = 1393,
                Displacement = 3243,

                Embolden = 2282,
                PrefulgenceReady = 4322,

                Forte = 4320,
                ThornedFlourish = 4321,

                EnchantedRiposte = 3234,
                EnchantedZwercchaur = 3235,
                EnchantedRedoublement = 3236;
        }

        internal class Debuffs
        {
            internal const ushort
                Monomachy = 3242,
                Scorch = 4319;
        }

        internal class RDMPvP_Combo : CustomComboClass
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.RDMPvP_Combo;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if ((actionID is Jolt3 or GrandImpact) && IsEnabled(CustomComboPreset.RDMPvP_Combo))
                {
                    if (IsEnabled(CustomComboPreset.RDMPvP_SouthernCross) && GetLimitBreakCurrentValue() == GetLimitBreakMaxValue()
                        && WasLastAbility(ViceOfThorns))
                    {
                        return SouthernCross;
                    }

                    if (!TargetHasEffectAny(PvPCommon.Buffs.Guard))
                    {
                        if (CanWeave(actionID))
                        {
                            if (IsEnabled(CustomComboPreset.RDMPvP_Displacement) && ActionReady(Displacement) && WasLastAction(EnchantedRedoublement))
                            {
                                return Displacement;
                            }

                            if (IsEnabled(CustomComboPreset.RDMPvP_Forte) && ActionReady(Forte) && WasLastAbility(CorpsACorps))
                            {
                                return Forte;
                            }

                            if (IsEnabled(CustomComboPreset.RDMPvP_Embolden) && ActionReady(Embolden))
                            {
                                return Embolden;
                            }

                            if (IsEnabled(CustomComboPreset.RDMPvP_ViceOfThorns) && HasEffect(Buffs.ThornedFlourish))
                            {
                                return ViceOfThorns;
                            }
                        }

                        if (IsEnabled(CustomComboPreset.RDMPvP_Prefulgence) && HasEffect(Buffs.PrefulgenceReady))
                        {
                            return Prefulgence;
                        }

                        if (IsEnabled(CustomComboPreset.RDMPvP_Resolution) && ActionReady(Resolution))
                        {
                            return Resolution;
                        }

                        if (IsEnabled(CustomComboPreset.RDMPvP_Melee) && ActionReady(OriginalHook(EnchantedRiposte)))
                        {
                            if (IsEnabled(CustomComboPreset.RDMPvP_CorpsACorps) && ActionReady(CorpsACorps)
                                && (!TargetHasEffect(Debuffs.Monomachy) || !InActionRange(OriginalHook(EnchantedRiposte))))
                            {
                                return CorpsACorps;
                            }

                            if (InActionRange(OriginalHook(EnchantedRiposte)))
                            {
                                return OriginalHook(EnchantedRiposte);
                            }
                        }
                    }
                }

                return actionID;
            }
        }
    }
}