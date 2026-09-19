using Dalamud.Game.ClientState.Conditions;
using ECommons.DalamudServices;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Core;
using UltimateCombo.Data;

namespace UltimateCombo.Combos.General;

internal static class BLU
{
    internal const byte JobID = 36;

    internal const uint
        SongOfTorment = 11386,
        Bristle = 11393,
        BloodDrain = 11395,
        SharpenedKnife = 11400,
        Missile = 11405,
        WhiteWind = 11406,
        FinalSting = 11407,
        SelfDestruct = 11408,
        ToadOil = 11410,
        Offguard = 11411,
        MoonFlute = 11415,
        MightyGuard = 11417,
        RamsVoice = 11419,
        PeculiarLight = 11421,
        FeatherRain = 11426,
        Eruption = 11427,
        MountainBuster = 11428,
        ShockStrike = 11429,
        Electrogenesis = 18298,
        Pomcure = 18303,
        Gobskin = 18304,
        MagicHammer = 18305,
        SonicBoom = 18308,
        Whistle = 18309,
        WhiteKnightsTour = 18310,
        BlackKnightsTour = 18311,
        PerpetualRay = 18314,
        AngelWhisper = 18317,
        Devour = 18320,
        AethericMimicry = 18322,
        Surpanakha = 18323,
        Quasar = 18324,
        JKick = 18325,
        TripleTrident = 23264,
        Tingle = 23265,
        AngelsSnack = 23272,
        RoseOfDestruction = 23275,
        BasicInstinct = 23276,
        Ultravibration = 23277,
        Blaze = 23278,
        MustardBomb = 23279,
        HydroPull = 23282,
        ChocoMeteor = 23284,
        MatraMagic = 23285,
        PeripheralSynthesis = 23286,
        PhantomFlurry = 23288,
        Nightbloom = 23290,
        GoblinPunch = 34563,
        Rehydration = 34566,
        BreathOfMagic = 34567,
        PeatPelt = 34569,
        DeepClean = 34570,
        DimensionalShift = 34573,
        WingedReprobation = 34576,
        MortalFlame = 34579,
        SeaShanty = 34580,
        BeingMortal = 34582;

    internal static class Buffs
    {
        internal const ushort
            MoonFlute = 1718,
            Bristle = 1716,
            WaningNocturne = 1727,
            PhantomFlurry = 2502,
            Tingle = 2492,
            AngelsSnack = 2495,
            Whistle = 2118,
            TankMimicry = 2124,
            DPSMimicry = 2125,
            HealerMimicry = 2126,
            BasicInstinct = 2498,
            ToadOil = 1737,
            MightyGuard = 1719,
            Devour = 2120,
            DeepClean = 3637,
            Gobskin = 2114,
            WingedReprobation = 3640,
            WingedRedemption = 3641;
    }

    internal static class Debuffs
    {
        internal const ushort
            Slow = 9,
            Bind = 13,
            Stun = 142,
            DeepFreeze = 1731,
            Offguard = 1717,
            Bleeding = 1714,
            Malodorous = 1715,
            MustardBomb = 2499,
            Conked = 2115,
            Lightheaded = 2501,
            MortalFlame = 3643,
            BreathOfMagic = 3712,
            PeatPelt = 3636;
    }

    internal static class Maps
    {
        internal const ushort
            Dragonskin = 558,
            Gazelle1 = 712,
            Thief = 725,
            Gazelle2 = 794,
            Zonure1 = 879,
            Zonure2 = 924;

        /*
        TheAquapolis = 558,
        TheLostCanalsOfUznair = 712,
        TheHiddenCanalsOfUznair = 725,
        TheShiftingAltarsOfUznair = 794,
        TheDungeonsOfLyheGhiah = 879,
        TheShiftingOubliettesOfLyheGhiah = 924;
        */
    }

    internal static class Config
    {
        internal static UserInt
            BLU_BloodDrain = new("BLU_BloodDrain", 2000),
            BLU_TankWhiteWind = new("BLU_TankWhiteWind", 50),
            BLU_TreasureWhiteWind = new("BLU_TreasureWhiteWind", 60),
            BLU_TreasureRehydration = new("BLU_TreasureRehydration", 30);

        internal static UserBool
            BLU_FeatherRain = new("BLU_FeatherRain"),
            BLU_Eruption = new("BLU_Eruption"),
            BLU_MountainBuster = new("BLU_MountainBuster"),
            BLU_ShockStrike = new("BLU_ShockStrike"),
            BLU_Quasar = new("BLU_Quasar"),
            BLU_JKick = new("BLU_JKick"),
            BLU_RoseOfDestruction = new("BLU_RoseOfDestruction"),
            BLU_WingedReprobation = new("BLU_WingedReprobation");
    }

    internal class BLU_MoonFluteOpener : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.BLU_MoonFluteOpener;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is MoonFlute && IsEnabled(Presets.BLU_MoonFluteOpener))
            {
                if (HasEffect(Buffs.PhantomFlurry))
                {
                    return OriginalHook(11);
                }

                if (GetCooldownRemainingTime(PhantomFlurry) > 15)
                {
                    return PhantomFlurry;
                }

                if (!HasEffect(Buffs.MoonFlute))
                {
                    if (!HasEffect(Buffs.Whistle) && IsSpellActive(Whistle))
                    {
                        return Whistle;
                    }

                    if (!HasEffect(Buffs.Tingle) && IsSpellActive(Tingle))
                    {
                        return Tingle;
                    }

                    if (BLUActionReady(RoseOfDestruction))
                    {
                        return RoseOfDestruction;
                    }

                    if (IsSpellActive(MoonFlute))
                    {
                        return MoonFlute;
                    }
                }

                if (BLUActionReady(JKick) && WasLastAction(MoonFlute))
                {
                    return JKick;
                }

                if (BLUActionReady(TripleTrident) && WasLastAction(JKick))
                {
                    return TripleTrident;
                }

                if (BLUActionReady(Nightbloom) && WasLastAction(TripleTrident))
                {
                    return Nightbloom;
                }

                if (IsEnabled(Presets.BLU_MoonFluteOpener_DoTOpener) || IsEnabled(Presets.BLU_MoonFluteOpener_DoubleDoTOpener))
                {
                    if (WasLastAction(Nightbloom) && IsSpellActive(Bristle) && !WasLastAction(Bristle))
                    {
                        return Bristle;
                    }

                    if (BLUActionReady(FeatherRain) && WasLastAction(Bristle))
                    {
                        return FeatherRain;
                    }

                    if (BLUActionReady(SeaShanty) && WasLastAction(FeatherRain))
                    {
                        return SeaShanty;
                    }

                    if (!TargetHasEffectAny(Debuffs.BreathOfMagic) && !TargetHasEffectAny(Debuffs.MortalFlame)
                        && !WasLastAction(BreathOfMagic) && !WasLastAction(MortalFlame))
                    {
                        if (IsSpellActive(BreathOfMagic) && WasLastAction(SeaShanty))
                        {
                            return BreathOfMagic;
                        }

                        if (IsSpellActive(MortalFlame) && WasLastAction(SeaShanty))
                        {
                            return MortalFlame;
                        }
                    }

                    if (BLUActionReady(ShockStrike) && (WasLastAction(BreathOfMagic) || WasLastAction(MortalFlame)))
                    {
                        return ShockStrike;
                    }

                    if (WasLastAction(ShockStrike) && IsSpellActive(Bristle) && !WasLastAction(Bristle))
                    {
                        return Bristle;
                    }

                    if (ActionReady(Common.Swiftcast) && WasLastAction(Bristle))
                    {
                        return Common.Swiftcast;
                    }

                    if (GetRemainingCharges(Surpanakha) > 0 && IsSpellActive(Surpanakha) && (WasLastAction(Common.Swiftcast) || WasLastAction(Surpanakha)))
                    {
                        return Surpanakha;
                    }

                    if (!IsEnabled(Presets.BLU_MoonFluteOpener_DoubleDoTOpener) && BLUActionReady(MatraMagic) && WasLastAction(Surpanakha))
                    {
                        return MatraMagic;
                    }

                    if (IsSpellActive(MortalFlame) && WasLastAction(Surpanakha))
                    {
                        return MortalFlame;
                    }

                    if (BLUActionReady(BeingMortal) && (WasLastAction(MortalFlame) || WasLastAction(MatraMagic)))
                    {
                        return BeingMortal;
                    }

                    if (BLUActionReady(PhantomFlurry) && WasLastAction(BeingMortal))
                    {
                        return PhantomFlurry;
                    }
                }

                if (!IsEnabled(Presets.BLU_MoonFluteOpener_DoTOpener) && !IsEnabled(Presets.BLU_MoonFluteOpener_DoubleDoTOpener))
                {
                    if (BLUActionReady(WingedReprobation) && !WasLastAction(WingedReprobation) && !WasLastAction(FeatherRain)
                        && IsOffCooldown(FeatherRain) && IsOffCooldown(SeaShanty)
                        && (!HasEffect(Buffs.WingedReprobation) || EffectStacks(Buffs.WingedReprobation) == 2))
                    {
                        return WingedReprobation;
                    }

                    if (BLUActionReady(FeatherRain) && WasLastAction(WingedReprobation))
                    {
                        return FeatherRain;
                    }

                    if (BLUActionReady(SeaShanty) && WasLastAction(FeatherRain))
                    {
                        return SeaShanty;
                    }

                    if (BLUActionReady(WingedReprobation) && WasLastAction(SeaShanty)
                        && IsOffCooldown(ShockStrike) && IsOffCooldown(BeingMortal)
                        && (EffectStacks(Buffs.WingedReprobation) == 1 || EffectStacks(Buffs.WingedReprobation) == 3))
                    {
                        return WingedReprobation;
                    }

                    if (BLUActionReady(ShockStrike) && WasLastAction(WingedReprobation)
                        && (EffectStacks(Buffs.WingedReprobation) == 2 || HasEffect(Buffs.WingedRedemption)))
                    {
                        return ShockStrike;
                    }

                    if (BLUActionReady(BeingMortal) && WasLastAction(ShockStrike)
                        && (EffectStacks(Buffs.WingedReprobation) == 2 || HasEffect(Buffs.WingedRedemption)))
                    {
                        return BeingMortal;
                    }

                    if (BLUActionReady(Bristle) && !HasEffect(Buffs.Bristle) && WasLastAction(BeingMortal))
                    {
                        return Bristle;
                    }

                    if (ActionReady(Common.Swiftcast) && WasLastAction(Bristle))
                    {
                        return Common.Swiftcast;
                    }

                    if (GetRemainingCharges(Surpanakha) > 0 && IsSpellActive(Surpanakha) && (WasLastAction(Common.Swiftcast) || WasLastAction(Surpanakha)))
                    {
                        return Surpanakha;
                    }

                    if (BLUActionReady(MatraMagic) && WasLastAction(Surpanakha))
                    {
                        return MatraMagic;
                    }

                    if (BLUActionReady(PhantomFlurry) && WasLastAction(MatraMagic))
                    {
                        return PhantomFlurry;
                    }
                }
            }

            return actionID;
        }
    }

    internal class BLU_OffFlute : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.BLU_OffFlute;

        private static uint LowerCooldown(uint current, uint candidate)
        {
            if (!IsSpellActive(candidate))
            {
                return current;
            }

            if (current == 0)
            {
                return candidate;
            }

            if (GetCooldownRemainingTime(candidate) < GetCooldownRemainingTime(current))
            {
                return candidate;
            }

            return current;
        }

        protected override uint Invoke(uint actionID)
        {
            if (IsEnabled(Presets.BLU_OffFlute)
                && ((actionID is FeatherRain && GetOptionBool(Config.BLU_FeatherRain))
                 || (actionID is Eruption && GetOptionBool(Config.BLU_Eruption))
                 || (actionID is MountainBuster && GetOptionBool(Config.BLU_MountainBuster))
                 || (actionID is ShockStrike && GetOptionBool(Config.BLU_ShockStrike))
                 || (actionID is Quasar && GetOptionBool(Config.BLU_Quasar))
                 || (actionID is JKick && GetOptionBool(Config.BLU_JKick))
                 || (actionID is RoseOfDestruction && GetOptionBool(Config.BLU_RoseOfDestruction))
                 || (actionID is WingedReprobation && GetOptionBool(Config.BLU_WingedReprobation))))
            {
                if (BLUActionReady(FeatherRain) && GetOptionBool(Config.BLU_FeatherRain) && CanWeave(actionID, ActionWatching.LastGCD))
                {
                    return FeatherRain;
                }

                if (BLUActionReady(Eruption) && GetOptionBool(Config.BLU_Eruption) && CanWeave(actionID, ActionWatching.LastGCD))
                {
                    return Eruption;
                }

                if (BLUActionReady(MountainBuster) && GetOptionBool(Config.BLU_MountainBuster) && CanWeave(actionID, ActionWatching.LastGCD))
                {
                    return MountainBuster;
                }

                if (BLUActionReady(ShockStrike) && GetOptionBool(Config.BLU_ShockStrike) && CanWeave(actionID, ActionWatching.LastGCD))
                {
                    return ShockStrike;
                }

                if (BLUActionReady(Quasar) && GetOptionBool(Config.BLU_Quasar) && CanWeave(actionID, ActionWatching.LastGCD))
                {
                    return Quasar;
                }

                if (BLUActionReady(JKick) && GetOptionBool(Config.BLU_JKick) && CanWeave(actionID, ActionWatching.LastGCD))
                {
                    return JKick;
                }

                if (BLUActionReady(RoseOfDestruction) && GetOptionBool(Config.BLU_RoseOfDestruction))
                {
                    return RoseOfDestruction;
                }

                if (BLUActionReady(WingedReprobation) && GetOptionBool(Config.BLU_WingedReprobation))
                {
                    return WingedReprobation;
                }

                uint lowest = 0;

                if (GetOptionBool(Config.BLU_FeatherRain))
                {
                    lowest = LowerCooldown(lowest, FeatherRain);
                }

                if (GetOptionBool(Config.BLU_Eruption))
                {
                    lowest = LowerCooldown(lowest, Eruption);
                }

                if (GetOptionBool(Config.BLU_MountainBuster))
                {
                    lowest = LowerCooldown(lowest, MountainBuster);
                }

                if (GetOptionBool(Config.BLU_ShockStrike))
                {
                    lowest = LowerCooldown(lowest, ShockStrike);
                }

                if (GetOptionBool(Config.BLU_Quasar))
                {
                    lowest = LowerCooldown(lowest, Quasar);
                }

                if (GetOptionBool(Config.BLU_JKick))
                {
                    lowest = LowerCooldown(lowest, JKick);
                }

                if (GetOptionBool(Config.BLU_RoseOfDestruction))
                {
                    lowest = LowerCooldown(lowest, RoseOfDestruction);
                }

                if (GetOptionBool(Config.BLU_WingedReprobation))
                {
                    lowest = LowerCooldown(lowest, WingedReprobation);
                }

                if (lowest != 0)
                {
                    return lowest;
                }
            }

            return actionID;
        }
    }

    internal class BLU_TripleTrident : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.BLU_TripleTrident;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is TripleTrident && IsEnabled(Presets.BLU_TripleTrident) && IsSpellActive(TripleTrident))
            {
                if (GetCooldownRemainingTime(TripleTrident) > 3)
                {
                    return TripleTrident;
                }

                if (!HasEffect(Buffs.Whistle) && IsSpellActive(Whistle))
                {
                    return Whistle;
                }

                if (!HasEffect(Buffs.Tingle) && IsSpellActive(Tingle))
                {
                    return Tingle;
                }

                if (HasEffect(Buffs.Whistle) && HasEffect(Buffs.Tingle))
                {
                    return TripleTrident;
                }
            }

            return actionID;
        }
    }

    internal class BLU_Sting : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.BLU_Sting;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is FinalSting && IsEnabled(Presets.BLU_Sting) && IsSpellActive(FinalSting))
            {
                if (!HasEffect(Buffs.Whistle) && IsSpellActive(Whistle))
                {
                    return Whistle;
                }

                if (!TargetHasEffectAny(Debuffs.Offguard) && BLUActionReady(Offguard))
                {
                    return Offguard;
                }

                if (!HasEffect(Buffs.Tingle) && IsSpellActive(Tingle))
                {
                    return Tingle;
                }

                if (!HasEffect(Buffs.BasicInstinct) && IsSpellActive(BasicInstinct) && HasCondition(ConditionFlag.BoundByDuty) && GetPartyMembers().Length == 0)
                {
                    return BasicInstinct;
                }

                if (!HasEffect(Buffs.MoonFlute) && IsSpellActive(MoonFlute))
                {
                    return MoonFlute;
                }

                if (ActionReady(Common.Swiftcast))
                {
                    return Common.Swiftcast;
                }

                if (HasEffect(Buffs.Whistle) && HasEffect(Buffs.Tingle) && HasEffect(Buffs.MoonFlute))
                {
                    return FinalSting;
                }
            }

            return actionID;
        }
    }

    internal class BLU_Explode : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.BLU_Explode;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is SelfDestruct && IsEnabled(Presets.BLU_Explode) && IsSpellActive(SelfDestruct))
            {
                if (!HasEffect(Buffs.ToadOil) && IsSpellActive(ToadOil))
                {
                    return ToadOil;
                }

                if (!HasEffect(Buffs.Bristle) && IsSpellActive(Bristle))
                {
                    return Bristle;
                }

                if (!HasEffect(Buffs.MoonFlute) && IsSpellActive(MoonFlute))
                {
                    return MoonFlute;
                }

                if (ActionReady(Common.Swiftcast))
                {
                    return Common.Swiftcast;
                }

                if (HasEffect(Buffs.ToadOil) && HasEffect(Buffs.Bristle) && HasEffect(Buffs.MoonFlute))
                {
                    return SelfDestruct;
                }
            }

            return actionID;
        }
    }

    internal class BLU_DoTs : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.BLU_DoTs;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is Bristle && IsEnabled(Presets.BLU_DoTs) && !HasEffect(Buffs.MoonFlute))
            {
                if (!HasEffect(Buffs.Bristle) && IsSpellActive(Bristle))
                {
                    return Bristle;
                }

                if (IsSpellActive(BreathOfMagic) && (!TargetHasEffectAny(Debuffs.BreathOfMagic) || TargetEffectRemainingTimeAny(Debuffs.BreathOfMagic) < 3))
                {
                    return BreathOfMagic;
                }

                if (IsSpellActive(MortalFlame) && !TargetHasEffectAny(Debuffs.MortalFlame))
                {
                    return MortalFlame;
                }
            }

            return actionID;
        }
    }

    internal class BLU_PeriphBomb : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.BLU_PeriphBomb;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is PeripheralSynthesis or MustardBomb && IsEnabled(Presets.BLU_PeriphBomb))
            {
                if (IsSpellActive(MustardBomb) && (WasLastGCD(PeripheralSynthesis) || HasEffect(Buffs.Bristle)
                    || TargetHasEffectAny(Debuffs.MustardBomb) || TargetHasEffectAny(Debuffs.Lightheaded)))
                {
                    return MustardBomb;
                }

                return PeripheralSynthesis;
            }

            return actionID;
        }
    }

    internal class BLU_VibeCheck : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.BLU_VibeCheck;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is HydroPull or RamsVoice or Ultravibration && IsEnabled(Presets.BLU_VibeCheck))
            {
                if ((BLUActionReady(Ultravibration) && TargetHasEffectAny(Debuffs.DeepFreeze)
                    && (HasEffect(Common.Buffs.Swiftcast) || IsOnCooldown(Common.Swiftcast)))
                    || (!InCombat() && IsOnCooldown(Ultravibration)))
                {
                    return Ultravibration;
                }

                if (WasLastGCD(RamsVoice) && ActionReady(Common.Swiftcast) && TargetHasEffectAny(Debuffs.DeepFreeze))
                {
                    return Common.Swiftcast;
                }

                if (BLUActionReady(RamsVoice) && !TargetHasEffectAny(Debuffs.DeepFreeze) && (WasLastGCD(HydroPull) || !IsSpellActive(HydroPull)))
                {
                    return RamsVoice;
                }

                if (BLUActionReady(HydroPull))
                {
                    return HydroPull;
                }
            }

            return actionID;
        }
    }

    internal class BLU_Basic : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.BLU_Basic;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is GoblinPunch or SonicBoom or ChocoMeteor or Blaze && IsEnabled(Presets.BLU_Basic))
            {
                if (IsEnabled(Presets.BLU_Basic_BloodDrain) && BLUActionReady(BloodDrain)
                    && CurrentMP <= GetOptionValue(Config.BLU_BloodDrain) && !HasEffect(Buffs.PhantomFlurry))
                {
                    return BloodDrain;
                }

                if (IsEnabled(Presets.BLU_Basic_PhantomFlurry) && HasEffect(Buffs.PhantomFlurry))
                {
                    if (EffectRemainingTime(Buffs.PhantomFlurry) <= 0.75f)
                    {
                        return OriginalHook(PhantomFlurry);
                    }

                    return OriginalHook(11);
                }

                if (IsEnabled(Presets.BLU_Basic_FeatherRain) && BLUActionReady(FeatherRain)
                    && GetCooldownRemainingTime(PhantomFlurry) >= GetCooldown(FeatherRain).CooldownTotal && CanWeave(actionID, ActionWatching.LastGCD))
                {
                    return FeatherRain;
                }

                if (IsEnabled(Presets.BLU_Basic_Rose) && BLUActionReady(RoseOfDestruction)
                    && GetCooldownRemainingTime(PhantomFlurry) >= GetCooldown(RoseOfDestruction).CooldownTotal)
                {
                    return RoseOfDestruction;
                }

                if (IsEnabled(Presets.BLU_Basic_WingedReprobation) && BLUActionReady(WingedReprobation)
                    && GetCooldownRemainingTime(PhantomFlurry) >= GetCooldown(WingedReprobation).CooldownTotal && EffectStacks(Buffs.WingedReprobation) >= 2)
                {
                    return WingedReprobation;
                }

                if (IsEnabled(Presets.BLU_Basic_ShockStrike) && BLUActionReady(ShockStrike)
                    && GetCooldownRemainingTime(PhantomFlurry) >= GetCooldown(ShockStrike).CooldownTotal && CanWeave(actionID, ActionWatching.LastGCD))
                {
                    return ShockStrike;
                }

                if (IsEnabled(Presets.BLU_Basic_JKick) && BLUActionReady(JKick)
                    && GetCooldownRemainingTime(PhantomFlurry) >= GetCooldown(JKick).CooldownTotal && CanWeave(actionID, ActionWatching.LastGCD))
                {
                    return JKick;
                }
            }

            return actionID;
        }
    }

    internal class BLU_Tanking : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.BLU_Tanking;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is GoblinPunch && IsEnabled(Presets.BLU_Tanking) && HasEffect(Buffs.TankMimicry)
                && !HasEffect(Buffs.PhantomFlurry) && GetPartyMembers().Length > 0)
            {
                if (!HasEffect(Buffs.MightyGuard) && IsSpellActive(MightyGuard))
                {
                    return MightyGuard;
                }

                if (IsEnabled(Presets.BLU_Tank_ToadOil) && !HasEffect(Buffs.ToadOil) && IsSpellActive(ToadOil) && !WasLastGCD(ToadOil))
                {
                    return ToadOil;
                }

                if (BLUActionReady(Devour) && InActionRange(Devour))
                {
                    return Devour;
                }

                if (IsEnabled(Presets.BLU_Tank_Peculiar) && BLUActionReady(PeculiarLight) && InMeleeRange())
                {
                    return PeculiarLight;
                }

                if (IsSpellActive(WhiteWind) && PlayerHealthPercentageHp() <= GetOptionValue(Config.BLU_TankWhiteWind) && CurrentMP >= GetResourceCost(WhiteWind))
                {
                    return WhiteWind;
                }

                if (IsEnabled(Presets.BLU_Tank_PeatClean) && !TargetHasEffectAny(Debuffs.PeatPelt)
                    && (!HasEffect(Buffs.DeepClean) || EffectRemainingTime(Buffs.DeepClean) < 2) && !WasLastGCD(PeatPelt) && IsSpellActive(PeatPelt))
                {
                    return PeatPelt;
                }

                if (IsEnabled(Presets.BLU_Tank_PeatClean) && (WasLastGCD(PeatPelt) || TargetHasEffectAny(Debuffs.PeatPelt)) && IsSpellActive(DeepClean))
                {
                    return DeepClean;
                }

                if (HasEffect(Buffs.DeepClean) && IsSpellActive(GoblinPunch))
                {
                    return GoblinPunch;
                }
            }

            return actionID;
        }
    }

    internal class BLU_Treasure_Tank : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.BLU_Treasure_Tank;

        protected override uint Invoke(uint actionID)
        {
            var notInMap = Svc.ClientState.TerritoryType is not Maps.Dragonskin and not Maps.Gazelle1 and not Maps.Gazelle2
                and not Maps.Thief and not Maps.Zonure1 and not Maps.Zonure2;

            var inMap = Svc.ClientState.TerritoryType is Maps.Dragonskin or Maps.Gazelle1 or Maps.Gazelle2 or Maps.Thief or Maps.Zonure1 or Maps.Zonure2;

            if (actionID is GoblinPunch && IsEnabled(Presets.BLU_Treasure_Tank) && HasEffect(Buffs.TankMimicry)
                && !HasEffect(Buffs.PhantomFlurry) && GetPartyMembers().Length == 0)
            {
                if (notInMap)
                {
                    if (IsEnabled(Presets.BLU_Treasure_Tank_MightyGuard) && HasEffect(Buffs.MightyGuard))
                    {
                        return MightyGuard;
                    }

                    if (IsEnabled(Presets.BLU_Treasure_Tank_AutoSpell) && CurrentTarget == null && !InCombat() && !IsCasting()
                        && BLUActionReady(ShockStrike) && BLUActionReady(Gobskin) && BLUActionReady(Quasar) && BLUActionReady(SeaShanty)
                        && (!IsSpellActive(RamsVoice) || !IsSpellActive(Missile) || !IsSpellActive(Ultravibration) || !IsSpellActive(HydroPull)))
                    {
                        for (var i = 0; i < 24; i++)
                        {
                            if (!ActionQueued())
                            {
                                if (GetActiveBlueMageActionInSlot(18) == ShockStrike && BLUActionReady(ShockStrike))
                                {
                                    AssignBlueMageActionToSlot(18, RamsVoice);
                                }

                                if (GetActiveBlueMageActionInSlot(19) == Quasar && BLUActionReady(Quasar))
                                {
                                    AssignBlueMageActionToSlot(19, Missile);
                                }

                                if (GetActiveBlueMageActionInSlot(20) == Rehydration && BLUActionReady(Rehydration))
                                {
                                    AssignBlueMageActionToSlot(20, ShockStrike);
                                }

                                if (GetActiveBlueMageActionInSlot(21) == SeaShanty && BLUActionReady(SeaShanty))
                                {
                                    AssignBlueMageActionToSlot(21, Ultravibration);
                                }

                                if ((GetActiveBlueMageActionInSlot(22) == PeripheralSynthesis && BLUActionReady(PeripheralSynthesis))
                                    || (GetActiveBlueMageActionInSlot(22) == RamsVoice && BLUActionReady(RamsVoice)))
                                {
                                    AssignBlueMageActionToSlot(22, HydroPull);
                                }

                                if ((GetActiveBlueMageActionInSlot(23) == MustardBomb && BLUActionReady(MustardBomb))
                                    || (GetActiveBlueMageActionInSlot(23) == Ultravibration && BLUActionReady(Ultravibration)))
                                {
                                    AssignBlueMageActionToSlot(23, DimensionalShift);
                                }
                            }
                        }
                    }

                    if (!IsSpellActive(RamsVoice) || !IsSpellActive(Missile) || !IsSpellActive(Ultravibration) || !IsSpellActive(HydroPull))
                    {
                        return OriginalHook(11);
                    }

                    if (IsEnabled(Presets.BLU_Treasure_Tank_Rehydration) && ActionReady(Common.Swiftcast) && IsSpellActive(Rehydration)
                        && PlayerHealthPercentageHp() <= GetOptionValue(Config.BLU_TreasureRehydration)
                        && !WasLastGCD(WhiteWind) && !WasLastGCD(Devour))
                    {
                        return Common.Swiftcast;
                    }

                    if (HasEffect(Common.Buffs.Swiftcast))
                    {
                        return Rehydration;
                    }

                    if (IsEnabled(Presets.BLU_Treasure_Tank_Devour) && BLUActionReady(Devour))
                    {
                        return Devour;
                    }

                    if (IsEnabled(Presets.BLU_Treasure_Tank_WhiteWind) && IsSpellActive(WhiteWind)
                        && PlayerHealthPercentageHp() <= GetOptionValue(Config.BLU_TreasureWhiteWind) && CurrentMP >= GetResourceCost(WhiteWind))
                    {
                        return WhiteWind;
                    }
                }

                if (inMap)
                {
                    if (IsEnabled(Presets.BLU_Treasure_Tank_BasicInstinct) && !HasEffect(Buffs.BasicInstinct))
                    {
                        return BasicInstinct;
                    }

                    if (IsEnabled(Presets.BLU_Treasure_Tank_MightyGuard) && !HasEffect(Buffs.MightyGuard))
                    {
                        return MightyGuard;
                    }

                    if (IsEnabled(Presets.BLU_Treasure_Tank_AutoSpell) && CurrentTarget == null && !InCombat() && !IsCasting() && Svc.DutyState.IsDutyStarted
                        && BLUActionReady(RamsVoice) && BLUActionReady(Missile) && BLUActionReady(Ultravibration) && BLUActionReady(HydroPull)
                        && (!IsSpellActive(ShockStrike) || !IsSpellActive(Gobskin) || !IsSpellActive(Quasar) || !IsSpellActive(SeaShanty)))
                    {
                        for (var i = 0; i < 24; i++)
                        {
                            if (!ActionQueued())
                            {
                                if (GetActiveBlueMageActionInSlot(18) == RamsVoice && BLUActionReady(RamsVoice))
                                {
                                    AssignBlueMageActionToSlot(18, ShockStrike);
                                }

                                if (GetActiveBlueMageActionInSlot(19) == Missile && BLUActionReady(Missile))
                                {
                                    AssignBlueMageActionToSlot(19, Quasar);
                                }

                                if (GetActiveBlueMageActionInSlot(20) == ShockStrike && BLUActionReady(ShockStrike))
                                {
                                    AssignBlueMageActionToSlot(20, Rehydration);
                                }

                                if (GetActiveBlueMageActionInSlot(21) == Ultravibration && BLUActionReady(Ultravibration))
                                {
                                    AssignBlueMageActionToSlot(21, SeaShanty);
                                }

                                if (Svc.ClientState.TerritoryType is not Maps.Zonure1 and not Maps.Zonure2)
                                {
                                    if (GetActiveBlueMageActionInSlot(22) == HydroPull && BLUActionReady(HydroPull))
                                    {
                                        AssignBlueMageActionToSlot(22, PeripheralSynthesis);
                                    }

                                    if (GetActiveBlueMageActionInSlot(23) == DimensionalShift && BLUActionReady(DimensionalShift))
                                    {
                                        AssignBlueMageActionToSlot(23, MustardBomb);
                                    }
                                }

                                if (Svc.ClientState.TerritoryType is Maps.Zonure1 or Maps.Zonure2)
                                {
                                    if (GetActiveBlueMageActionInSlot(22) == HydroPull && BLUActionReady(HydroPull))
                                    {
                                        AssignBlueMageActionToSlot(22, RamsVoice);
                                    }

                                    if (GetActiveBlueMageActionInSlot(23) == DimensionalShift && BLUActionReady(DimensionalShift))
                                    {
                                        AssignBlueMageActionToSlot(23, Ultravibration);
                                    }
                                }
                            }
                        }
                    }

                    if (IsEnabled(Presets.BLU_Treasure_Tank_Rehydration) && ActionReady(Common.Swiftcast) && IsSpellActive(Rehydration)
                        && PlayerHealthPercentageHp() <= GetOptionValue(Config.BLU_TreasureRehydration)
                        && !WasLastGCD(WhiteWind) && !WasLastGCD(Devour))
                    {
                        return Common.Swiftcast;
                    }

                    if (HasEffect(Common.Buffs.Swiftcast))
                    {
                        return Rehydration;
                    }

                    if (IsEnabled(Presets.BLU_Treasure_Tank_Devour) && BLUActionReady(Devour))
                    {
                        return Devour;
                    }

                    if (IsEnabled(Presets.BLU_Treasure_Tank_WhiteWind) && IsSpellActive(WhiteWind)
                        && PlayerHealthPercentageHp() <= GetOptionValue(Config.BLU_TreasureWhiteWind) && CurrentMP >= GetResourceCost(WhiteWind))
                    {
                        return WhiteWind;
                    }

                    if (IsEnabled(Presets.BLU_Treasure_Tank_BreathOfMagic) && TargetIsBoss() && TargetWorthDoT()
                        && !TargetHasEffectAny(Debuffs.BreathOfMagic) && !WasLastGCD(BreathOfMagic))
                    {
                        if (!HasEffect(Buffs.Bristle))
                        {
                            return Bristle;
                        }

                        return BreathOfMagic;
                    }

                    if (IsEnabled(Presets.BLU_Treasure_Tank_MortalFlame) && TargetIsBoss() && TargetWorthDoT()
                        && !TargetHasEffectAny(Debuffs.MortalFlame) && !WasLastGCD(MortalFlame))
                    {
                        if (!HasEffect(Buffs.Bristle))
                        {
                            return Bristle;
                        }

                        return MortalFlame;
                    }

                    if (IsEnabled(Presets.BLU_Treasure_Tank_TripleTrident) && TargetIsBoss() && TargetWorthDoT()
                        && IsSpellActive(TripleTrident) && IsSpellActive(Whistle) && IsSpellActive(Tingle)
                        && (IsOffCooldown(TripleTrident) || GetCooldownRemainingTime(TripleTrident) < 5) && !WasLastGCD(TripleTrident))
                    {
                        if (!HasEffect(Buffs.Whistle))
                        {
                            return Whistle;
                        }

                        if (!HasEffect(Buffs.Tingle))
                        {
                            return Tingle;
                        }

                        return TripleTrident;
                    }
                }
            }

            return actionID;
        }
    }
}
