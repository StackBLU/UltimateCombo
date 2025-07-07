using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvP
{
    internal static class VPRPvP
    {
        public const uint
            SteelFangs = 39157,
            HuntersSting = 39159,
            BarbarousBite = 39161,
            PiercingFangs = 39158,
            SwiftskinsSting = 39160,
            RavenousBite = 39163,

            SerpentsTail = 39183,
            DeathRattle = 39174,
            TwinfangBite = 39175,
            TwinbloodBite = 39176,
            UncoiledTwinfang = 39177,
            UncoiledTwinblood = 39178,

            Bloodcoil = 39166,
            SanguineFeast = 39167,

            UncoiledFury = 39168,
            RattlingCoil = 39189,

            SnakeScales = 39185,
            Backlash = 39186,

            Slither = 39184,

            FirstGeneration = 39169,
            FirstLegacy = 39179,
            SecondGeneration = 39170,
            SecondLegacy = 39180,
            ThirdGeneration = 39171,
            ThirdLegacy = 39181,
            FourthGeneration = 39172,
            FourthLegacy = 39182,
            Ouroboros = 39173,

            WorldSwallower = 39190;

        public static class Buffs
        {
            public const ushort
                Slither = 4095,

                DeathRattleReady = 4085,
                TwinfangBiteReady = 4086,
                TwinbloodBiteReady = 4087,
                UncoiledTwinfangReady = 4088,
                UncoiledTwinbloodReady = 4089,
                FirstGenerationReady = 4090,
                SecondGenerationReady = 4091,
                ThirdGenerationReady = 4092,
                FourthGenerationReady = 4093,

                HardnenedScales = 4096,
                ArmoredScales = 4097,
                SnakesBane = 4098,

                Reawakened = 4094;
        }

        public static class Debuffs
        {
            public const ushort
                NoxiousGash = 4099;
        }

        internal class VPRPvP_Combo : CustomComboClass
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.VPRPvP_Combo;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if ((actionID is SteelFangs or HuntersSting or BarbarousBite or PiercingFangs or SwiftskinsSting or RavenousBite)
                    && IsEnabled(CustomComboPreset.VPRPvP_Combo))
                {
                    if (IsEnabled(CustomComboPreset.VPRPvP_Backlash) && HasEffect(Buffs.SnakesBane))
                    {
                        return OriginalHook(SnakeScales);
                    }

                    if (IsEnabled(CustomComboPreset.VPRPvP_SerpentsTail) && IsEnabled(OriginalHook(SerpentsTail))
                        && IsOffCooldown(OriginalHook(SerpentsTail)))
                    {
                        return OriginalHook(SerpentsTail);
                    }

                    if (!TargetHasEffectAny(PvPCommon.Buffs.Guard))
                    {
                        if (IsEnabled(CustomComboPreset.VPRPvP_WorldSwallower) && GetLimitBreakCurrentValue() == GetLimitBreakMaxValue())
                        {
                            return WorldSwallower;
                        }

                        if (CanWeave(actionID))
                        {
                            if (IsEnabled(CustomComboPreset.VPRPvP_RattlingCoil) && ActionReady(RattlingCoil)
                                && IsOnCooldown(UncoiledFury) && IsOnCooldown(SnakeScales))
                            {
                                return RattlingCoil;
                            }
                        }

                        if (HasEffect(Buffs.Reawakened))
                        {
                            return WasLastWeaponskill(FourthGeneration) ? OriginalHook(Ouroboros) : actionID;
                        }

                        if (IsEnabled(CustomComboPreset.VPRPvP_UncoiledFury) && ActionReady(UncoiledFury))
                        {
                            return UncoiledFury;
                        }

                        if (IsEnabled(CustomComboPreset.VPRPvP_Bloodcoil) && ActionReady(OriginalHook(Bloodcoil)))
                        {
                            return OriginalHook(Bloodcoil);
                        }
                    }
                }

                return (actionID is SnakeScales or Backlash) && IsEnabled(CustomComboPreset.VPRPvP_Backlash)
                    ? HasEffect(Buffs.SnakesBane)
                        ? OriginalHook(SnakeScales)
                        : IsOffCooldown(SnakeScales) || !HasEffect(Buffs.HardnenedScales) ? SnakeScales : OriginalHook(11)
                    : actionID;
            }
        }
    }
}