using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvP
{
    internal static class NINPvP
    {
        public const uint
            SpinningEdge = 29500,
            GustSlash = 29501,
            AeolianEdge = 29502,

            FumaShuriken = 29505,
            Bunshin = 29511,
            Dokumori = 41451,
            ZeshoMeppo = 41452,

            Shukuchi = 29513,
            Assassinate = 29503,

            ThreeMudra = 29507,
            ForkedRaiju = 29510,
            FleetingRaiju = 29707,
            HyoshoRanryu = 29506,
            GokaMekkyaku = 29504,
            Meisui = 29508,
            Huton = 29512,
            Doton = 29514,

            SeitonTenchu = 29515,

            //Unused
            HollowNozuchi = 29559;

        public static class Buffs
        {
            public const ushort
                ThreeMudra = 1317,
                FleetingRaijuReady = 3211,
                Meisui = 3189,
                Huton = 3186,

                ZeshoMeppoReady = 4305,

                Hidden = 1316,

                Bunshin = 2010,
                ShadeShift = 2011,

                UnsealedSeitonAnimation = 3190,
                UnsealedSeitonTenchu = 3192;
        }

        internal class Debuffs
        {
            internal const ushort
                Dokumori = 4303,
                GokaMekkyaku = 3184,
                DotonCorruption = 3079,

                SealedForkedRaiju = 3195,
                SealedHyoshoRanryu = 3194,
                SealedGokaMekkyaku = 3193,
                SealedDoton = 3197,
                SealedMeisui = 3198,
                SealedHuton = 3196,

                DeathLink = 3191;
        }

        internal class NINPvP_Combo : CustomComboClass
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.NINPvP_Combo;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if ((actionID is SpinningEdge or GustSlash or AeolianEdge or ZeshoMeppo or ForkedRaiju or FleetingRaiju or Assassinate)
                    && IsEnabled(CustomComboPreset.NINPvP_Combo))
                {
                    if (IsEnabled(CustomComboPreset.NINPvP_SeitonTenchu) && HasTarget()
                        && (GetLimitBreakCurrentValue() == GetLimitBreakMaxValue() || HasEffect(Buffs.UnsealedSeitonTenchu))
                        && (GetTargetHPPercent() <= 49
                        || (HasEffect(Buffs.UnsealedSeitonTenchu) && GetBuffRemainingTime(Buffs.UnsealedSeitonTenchu) <= 1)))
                    {
                        return OriginalHook(SeitonTenchu);
                    }

                    if (!TargetHasEffectAny(PvPCommon.Buffs.Guard) && !HasEffect(Buffs.Hidden))
                    {
                        if (CanWeave(actionID))
                        {
                            if (IsEnabled(CustomComboPreset.NINPvP_Dokumori) && ActionReady(Dokumori) && InActionRange(Dokumori))
                            {
                                return Dokumori;
                            }

                            if (IsEnabled(CustomComboPreset.NINPvP_Bunshin) && ActionReady(Bunshin))
                            {
                                return Bunshin;
                            }
                        }

                        if (IsEnabled(CustomComboPreset.NINPvP_ThreeMudra) && ActionReady(ThreeMudra) && !HasEffect(Buffs.ThreeMudra)
                            && (CanWeave(actionID) || !InActionRange(SpinningEdge)))
                        {
                            return ThreeMudra;
                        }

                        if (HasEffect(Buffs.ThreeMudra))
                        {
                            if (IsEnabled(CustomComboPreset.NINPvP_GokaMekkyaku) && !HasEffect(Debuffs.SealedGokaMekkyaku)
                                && !TargetHasEffect(Debuffs.GokaMekkyaku))
                            {
                                return GokaMekkyaku;
                            }

                            if (IsEnabled(CustomComboPreset.NINPvP_HyoshoRanryu) && !HasEffect(Debuffs.SealedHyoshoRanryu)
                                && GetTargetHPPercent() >= 60 && HasTarget())
                            {
                                return HyoshoRanryu;
                            }

                            if (IsEnabled(CustomComboPreset.NINPvP_Raiju) && !HasEffect(Debuffs.SealedForkedRaiju)
                                && !TargetHasEffectAny(PvPCommon.Buffs.Resilience))
                            {
                                return ForkedRaiju;
                            }
                        }

                        if (IsEnabled(CustomComboPreset.NINPvP_Shuriken) && ActionReady(FumaShuriken)
                            && (!HasEffect(Buffs.Hidden) || (HasEffect(Buffs.Hidden) && !InActionRange(SpinningEdge)))
                            && (!InActionRange(SpinningEdge) || GetRemainingCharges(FumaShuriken) == GetMaxCharges(FumaShuriken)))
                        {
                            return FumaShuriken;
                        }
                    }
                }

                return actionID;
            }
        }
    }
}