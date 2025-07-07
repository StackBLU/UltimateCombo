using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvP
{
    internal static class DRGPvP
    {
        public const uint
            RaidenThrust = 29486,
            FangAndClaw = 29487,
            WheelingThrust = 29488,
            Drakesbane = 41449,

            ChaoticSpring = 29490,

            Geirskogul = 29491,
            Nastrond = 29492,
            Starcross = 41450,

            HighJump = 29493,
            HorridRoar = 29496,
            HeavensThrust = 29489,

            ElusiveJump = 29494,
            WyrmwindThrust = 29495,

            SkyHigh = 29497,
            SkyShatter = 29499;

        public static class Buffs
        {
            public const ushort
                LifeOfTheDragon = 3177,
                NastrondReady = 4404,
                StarcrossReady = 4302,

                Heavensent = 3176,

                ElusiveJump = 3209,
                FirstmindsFocus = 3178,

                SkyHigh = 3180,
                SkyShatter = 3183;
        }

        public static class Debuffs
        {
            public const ushort
                HorridRoad = 3179;
        }

        internal class DRGPvP_Combo : CustomComboClass
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DRGPvP_Combo;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if ((actionID is RaidenThrust or FangAndClaw or WheelingThrust or Drakesbane or HeavensThrust)
                    && IsEnabled(CustomComboPreset.DRGPvP_Combo))
                {
                    if (IsEnabled(CustomComboPreset.DRGPvP_ChaoticSpring) && ActionReady(ChaoticSpring)
                        && LocalPlayer?.CurrentHp <= LocalPlayer?.MaxHp - 12000 && InActionRange(ChaoticSpring))
                    {
                        return ChaoticSpring;
                    }

                    if (!TargetHasEffectAny(PvPCommon.Buffs.Guard))
                    {
                        if (CanWeave(actionID))
                        {
                            if (IsEnabled(CustomComboPreset.DRGPvP_HorridRoar) && ActionReady(HorridRoar) && WasLastAbility(HighJump))
                            {
                                return HorridRoar;
                            }
                        }

                        if (IsEnabled(CustomComboPreset.DRGPvP_Nastrond) && HasEffect(Buffs.NastrondReady) && GetBuffRemainingTime(Buffs.NastrondReady) <= 1)
                        {
                            return Nastrond;
                        }

                        if (IsEnabled(CustomComboPreset.DRGPvP_WyrmwindThrust) && HasEffect(Buffs.FirstmindsFocus)
                            && HasTarget() && GetTargetDistance() > 15)
                        {
                            return WyrmwindThrust;
                        }
                    }
                }

                return actionID;
            }
        }
    }
}