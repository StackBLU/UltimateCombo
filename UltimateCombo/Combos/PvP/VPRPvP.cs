using UltimateCombo.Combos.General;
using UltimateCombo.Core;

namespace UltimateCombo.Combos.PvP;

internal static class VPRPvP
{
    internal const uint
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

    internal static class Buffs
    {
        internal const ushort
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

    internal static class Debuffs
    {
        internal const ushort
            NoxiousGash = 4099;
    }

    internal class VPRPvP_Combo : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.VPRPvP_Combo;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is SteelFangs or HuntersSting or BarbarousBite or PiercingFangs or SwiftskinsSting or RavenousBite)
                && IsEnabled(Presets.VPRPvP_Combo))
            {
                if (IsEnabled(Presets.VPRPvP_Backlash) && HasEffect(Buffs.SnakesBane))
                {
                    return OriginalHook(SnakeScales);
                }

                if (IsEnabled(Presets.VPRPvP_SerpentsTail) && IsActionEnabled(OriginalHook(SerpentsTail))
                    && IsOffCooldown(OriginalHook(SerpentsTail)))
                {
                    return OriginalHook(SerpentsTail);
                }

                if (!TargetHasEffectAny(AllPvP.Buffs.Guard))
                {
                    if (IsEnabled(Presets.VPRPvP_WorldSwallower) && GetLimitBreakCurrentValue() == GetLimitBreakMaxValue())
                    {
                        return WorldSwallower;
                    }

                    if (CanWeave(actionID))
                    {
                        if (IsEnabled(Presets.VPRPvP_RattlingCoil) && ActionReady(RattlingCoil)
                            && IsOnCooldown(UncoiledFury) && IsOnCooldown(SnakeScales))
                        {
                            return RattlingCoil;
                        }
                    }

                    if (HasEffect(Buffs.Reawakened))
                    {
                        if (WasLastWeaponskill(FourthGeneration))
                        {
                            return OriginalHook(Ouroboros);
                        }

                        return actionID;
                    }

                    if (IsEnabled(Presets.VPRPvP_UncoiledFury) && ActionReady(UncoiledFury))
                    {
                        return UncoiledFury;
                    }

                    if (IsEnabled(Presets.VPRPvP_Bloodcoil) && ActionReady(OriginalHook(Bloodcoil)))
                    {
                        return OriginalHook(Bloodcoil);
                    }
                }
            }

            if ((actionID is SnakeScales or Backlash) && IsEnabled(Presets.VPRPvP_Backlash))
            {
                if (HasEffect(Buffs.SnakesBane))
                {
                    return OriginalHook(SnakeScales);
                }

                if (IsOffCooldown(SnakeScales) || !HasEffect(Buffs.HardnenedScales))
                {
                    return SnakeScales;
                }

                return OriginalHook(11);
            }

            return actionID;
        }
    }
}
