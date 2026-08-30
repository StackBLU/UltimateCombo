using Dalamud.Game.ClientState.Conditions;
using ECommons.DalamudServices;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Core;

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
            WingedReprobation = 3640;
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
            BLU_BloodDrain = new("BLU_BloodDrain", 1500),
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

        protected override uint Invoke(uint actionID, uint lastComboActionID)
        {
            if (actionID is MoonFlute && IsEnabled(Presets.BLU_MoonFluteOpener))
            {
                if (HasEffect(Buffs.PhantomFlurry) && actionID is MoonFlute)
                {
                    return OriginalHook(11);
                }

                if (GetCooldownRemainingTime(PhantomFlurry) > 60 && HasEffect(Buffs.WingedReprobation)
                    && !IsEnabled(Presets.BLU_MoonFluteOpener_DoTOpener) && actionID is MoonFlute)
                {
                    return WingedReprobation;
                }

                if (GetCooldownRemainingTime(PhantomFlurry) > 15 && actionID is MoonFlute)
                {
                    return PhantomFlurry;
                }

                if (!HasEffect(Buffs.MoonFlute) && actionID is MoonFlute)
                {
                    if (!HasEffect(Buffs.Whistle) && actionID is MoonFlute && IsSpellActive(Whistle))
                    {
                        return Whistle;
                    }

                    if (!HasEffect(Buffs.Tingle) && actionID is MoonFlute && IsSpellActive(Tingle))
                    {
                        return Tingle;
                    }

                    if (IsOffCooldown(RoseOfDestruction) && actionID is MoonFlute && IsSpellActive(RoseOfDestruction))
                    {
                        return RoseOfDestruction;
                    }

                    if (IsSpellActive(MoonFlute) && actionID is MoonFlute)
                    {
                        return MoonFlute;
                    }
                }

                if (IsOffCooldown(JKick) && actionID is MoonFlute && IsSpellActive(JKick) && WasLastSpell(MoonFlute))
                {
                    return JKick;
                }

                if (IsOffCooldown(TripleTrident) && actionID is MoonFlute && IsSpellActive(TripleTrident) && WasLastAbility(JKick))
                {
                    return TripleTrident;
                }

                if (IsOffCooldown(Nightbloom) && actionID is MoonFlute && IsSpellActive(Nightbloom) && WasLastSpell(TripleTrident))
                {
                    return Nightbloom;
                }

                if ((IsEnabled(Presets.BLU_MoonFluteOpener_DoTOpener) || IsEnabled(Presets.BLU_MoonFluteOpener_DoubleDoTOpener))
                    && actionID is MoonFlute)
                {
                    if (WasLastAbility(Nightbloom) && actionID is MoonFlute && IsSpellActive(Bristle) && !WasLastSpell(Bristle))
                    {
                        return Bristle;
                    }
                    if (IsOffCooldown(FeatherRain) && actionID is MoonFlute && IsSpellActive(FeatherRain) && WasLastSpell(Bristle))
                    {
                        return FeatherRain;
                    }

                    if (IsOffCooldown(SeaShanty) && actionID is MoonFlute && IsSpellActive(SeaShanty) && WasLastAbility(FeatherRain))
                    {
                        return SeaShanty;
                    }

                    if (!TargetHasEffectAny(Debuffs.BreathOfMagic) && !TargetHasEffectAny(Debuffs.MortalFlame) && actionID is MoonFlute)
                    {
                        if (!WasLastSpell(BreathOfMagic) && !WasLastSpell(MortalFlame) && actionID is MoonFlute)
                        {
                            if (actionID is MoonFlute && IsSpellActive(BreathOfMagic) && WasLastAction(SeaShanty))
                            {
                                return BreathOfMagic;
                            }

                            if (actionID is MoonFlute && IsSpellActive(MortalFlame) && WasLastAction(SeaShanty))
                            {
                                return MortalFlame;
                            }
                        }
                    }

                    if (IsOffCooldown(ShockStrike) && actionID is MoonFlute && IsSpellActive(ShockStrike)
                        && (WasLastSpell(BreathOfMagic) || WasLastSpell(MortalFlame)))
                    {
                        return ShockStrike;
                    }

                    if (WasLastAbility(ShockStrike) && actionID is MoonFlute && IsSpellActive(Bristle) && !WasLastSpell(Bristle))
                    {
                        return Bristle;
                    }

                    if (IsOffCooldown(Common.Swiftcast) && WasLastSpell(Bristle) && actionID is MoonFlute)
                    {
                        return Common.Swiftcast;
                    }

                    if (GetRemainingCharges(Surpanakha) > 0 && actionID is MoonFlute && IsSpellActive(Surpanakha))
                    {
                        return Surpanakha;
                    }

                    if (IsOffCooldown(MatraMagic) && actionID is MoonFlute && IsSpellActive(MatraMagic)
                        && !IsEnabled(Presets.BLU_MoonFluteOpener_DoubleDoTOpener))
                    {
                        return MatraMagic;
                    }

                    if (actionID is MoonFlute && IsSpellActive(MortalFlame) && WasLastAction(Surpanakha))
                    {
                        return MortalFlame;
                    }

                    if (IsOffCooldown(BeingMortal) && actionID is MoonFlute && IsSpellActive(BeingMortal))
                    {
                        return BeingMortal;
                    }

                    if (IsOffCooldown(PhantomFlurry) && actionID is MoonFlute && IsSpellActive(PhantomFlurry))
                    {
                        return PhantomFlurry;
                    }
                }

                if (!IsEnabled(Presets.BLU_MoonFluteOpener_DoTOpener) && !IsEnabled(Presets.BLU_MoonFluteOpener_DoubleDoTOpener)
                    && actionID is MoonFlute)
                {
                    if (IsOffCooldown(WingedReprobation) && actionID is MoonFlute && IsSpellActive(WingedReprobation)
                        && !WasLastSpell(WingedReprobation) && !WasLastAbility(FeatherRain) && !HasEffect(Buffs.WingedReprobation))
                    {
                        return WingedReprobation;
                    }

                    if (IsOffCooldown(FeatherRain) && actionID is MoonFlute && IsSpellActive(FeatherRain) && WasLastSpell(WingedReprobation))
                    {
                        return FeatherRain;
                    }

                    if (IsOffCooldown(SeaShanty) && actionID is MoonFlute && IsSpellActive(SeaShanty) && WasLastAbility(FeatherRain))
                    {
                        return SeaShanty;
                    }

                    if (IsOffCooldown(WingedReprobation) && actionID is MoonFlute && IsSpellActive(WingedReprobation) && WasLastAbility(SeaShanty)
                        && EffectStacks(Buffs.WingedReprobation) == 1)
                    {
                        return WingedReprobation;
                    }

                    if (IsOffCooldown(ShockStrike) && actionID is MoonFlute && IsSpellActive(ShockStrike) && WasLastSpell(WingedReprobation))
                    {
                        return ShockStrike;
                    }

                    if (IsOffCooldown(BeingMortal) && actionID is MoonFlute && IsSpellActive(BeingMortal) && WasLastAbility(ShockStrike))
                    {
                        return BeingMortal;
                    }

                    if (!HasEffect(Buffs.Bristle) && actionID is MoonFlute && IsSpellActive(Bristle) && WasLastAbility(BeingMortal))
                    {
                        return Bristle;
                    }

                    if (IsOffCooldown(Common.Swiftcast) && WasLastSpell(Bristle) && actionID is MoonFlute)
                    {
                        return Common.Swiftcast;
                    }

                    if (GetRemainingCharges(Surpanakha) > 0 && actionID is MoonFlute && IsSpellActive(Surpanakha))
                    {
                        return Surpanakha;
                    }

                    if (IsOffCooldown(MatraMagic) && actionID is MoonFlute && IsSpellActive(MatraMagic))
                    {
                        return MatraMagic;
                    }

                    if (IsOffCooldown(PhantomFlurry) && actionID is MoonFlute && IsSpellActive(PhantomFlurry))
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

        protected override uint Invoke(uint actionID, uint lastComboMove)
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
                if (BLUActionReady(FeatherRain) && GetOptionBool(Config.BLU_FeatherRain))
                {
                    return FeatherRain;
                }

                if (BLUActionReady(Eruption) && GetOptionBool(Config.BLU_Eruption))
                {
                    return Eruption;
                }

                if (BLUActionReady(MountainBuster) && GetOptionBool(Config.BLU_MountainBuster))
                {
                    return MountainBuster;
                }

                if (BLUActionReady(ShockStrike) && GetOptionBool(Config.BLU_ShockStrike))
                {
                    return ShockStrike;
                }

                if (BLUActionReady(Quasar) && GetOptionBool(Config.BLU_Quasar))
                {
                    return Quasar;
                }

                if (BLUActionReady(JKick) && GetOptionBool(Config.BLU_JKick))
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

                //Cooldown checking to show the shortest cooldown remaining

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

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (actionID is TripleTrident && IsEnabled(Presets.BLU_TripleTrident) && IsSpellActive(TripleTrident))
            {
                if (GetCooldownRemainingTime(TripleTrident) > 3 && actionID is TripleTrident)
                {
                    return TripleTrident;
                }

                if (!HasEffect(Buffs.Whistle) && actionID is TripleTrident && IsSpellActive(Whistle))
                {
                    return Whistle;
                }

                if (!HasEffect(Buffs.Tingle) && actionID is TripleTrident && IsSpellActive(Tingle))
                {
                    return Tingle;
                }

                if (HasEffect(Buffs.Whistle) && HasEffect(Buffs.Tingle) && actionID is TripleTrident)
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

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (actionID is FinalSting && IsEnabled(Presets.BLU_Sting) && IsSpellActive(FinalSting))
            {
                if (!HasEffect(Buffs.Whistle) && actionID is FinalSting && IsSpellActive(Whistle))
                {
                    return Whistle;
                }

                if (!TargetHasEffectAny(Debuffs.Offguard) && actionID is FinalSting && IsOffCooldown(Offguard) && IsSpellActive(Offguard))
                {
                    return Offguard;
                }

                if (!HasEffect(Buffs.Tingle) && actionID is FinalSting && IsSpellActive(Tingle))
                {
                    return Tingle;
                }

                if (!HasEffect(Buffs.BasicInstinct) && actionID is FinalSting && IsSpellActive(BasicInstinct)
                    && HasCondition(ConditionFlag.BoundByDuty) && GetPartyMembers().Length == 0)
                {
                    return BasicInstinct;
                }

                if (!HasEffect(Buffs.MoonFlute) && actionID is FinalSting && IsSpellActive(MoonFlute))
                {
                    return MoonFlute;
                }

                if (IsOffCooldown(Common.Swiftcast) && actionID is FinalSting)
                {
                    return Common.Swiftcast;
                }

                if (HasEffect(Buffs.Whistle) && HasEffect(Buffs.Tingle) && HasEffect(Buffs.MoonFlute) && actionID is FinalSting)
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

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (actionID is SelfDestruct && IsEnabled(Presets.BLU_Explode) && IsSpellActive(SelfDestruct))
            {
                if (!HasEffect(Buffs.ToadOil) && actionID is SelfDestruct && IsSpellActive(ToadOil))
                {
                    return ToadOil;
                }

                if (!HasEffect(Buffs.Bristle) && actionID is SelfDestruct && IsSpellActive(Bristle))
                {
                    return Bristle;
                }

                if (!HasEffect(Buffs.MoonFlute) && actionID is SelfDestruct && IsSpellActive(MoonFlute))
                {
                    return MoonFlute;
                }

                if (IsOffCooldown(Common.Swiftcast) && actionID is SelfDestruct)
                {
                    return Common.Swiftcast;
                }

                if (HasEffect(Buffs.ToadOil) && HasEffect(Buffs.Bristle) && HasEffect(Buffs.MoonFlute) && actionID is SelfDestruct)
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

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (actionID is Bristle && IsEnabled(Presets.BLU_DoTs) && !HasEffect(Buffs.MoonFlute))
            {
                if (!HasEffect(Buffs.Bristle) && IsSpellActive(Bristle) && actionID is Bristle)
                {
                    return Bristle;
                }

                if (IsSpellActive(BreathOfMagic) && actionID is Bristle
                    && (!TargetHasEffectAny(Debuffs.BreathOfMagic) || TargetEffectRemainingTimeAny(Debuffs.BreathOfMagic) < 3))
                {
                    return BreathOfMagic;
                }

                if (IsSpellActive(MortalFlame) && !TargetHasEffectAny(Debuffs.MortalFlame) && actionID is Bristle)
                {
                    return MortalFlame;
                }

                if (IsSpellActive(MatraMagic) && IsOffCooldown(MatraMagic) && actionID is Bristle
                    && IsEnabled(Presets.BLU_MoonFluteOpener_DoubleDoTOpener))
                {
                    return MatraMagic;
                }
            }

            return actionID;
        }
    }

    internal class BLU_PeriphBomb : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.BLU_PeriphBomb;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (actionID is PeripheralSynthesis or MustardBomb && IsEnabled(Presets.BLU_PeriphBomb))
            {
                if (IsSpellActive(MustardBomb) && (actionID is PeripheralSynthesis or MustardBomb)
                    && (WasLastSpell(PeripheralSynthesis) || HasEffect(Buffs.Bristle) || TargetHasEffectAny(Debuffs.MustardBomb)
                    || TargetHasEffectAny(Debuffs.Lightheaded)))
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

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (actionID is HydroPull or RamsVoice or Ultravibration && IsEnabled(Presets.BLU_VibeCheck))
            {
                if (!InCombat() && IsOnCooldown(Ultravibration) && (actionID is HydroPull or RamsVoice or Ultravibration))
                {
                    return Ultravibration;
                }

                if (IsSpellActive(HydroPull) && !WasLastSpell(HydroPull) && !WasLastSpell(RamsVoice) && !TargetHasEffectAny(Debuffs.DeepFreeze)
                    && (actionID is HydroPull or RamsVoice or Ultravibration))
                {
                    return HydroPull;
                }

                if (IsSpellActive(RamsVoice) && !TargetHasEffectAny(Debuffs.DeepFreeze) && (WasLastSpell(HydroPull) || !IsSpellActive(HydroPull))
                    && (actionID is HydroPull or RamsVoice or Ultravibration))
                {
                    return RamsVoice;
                }

                if (WasLastSpell(RamsVoice) && IsOffCooldown(Common.Swiftcast) && IsOffCooldown(Ultravibration) && TargetHasEffectAny(Debuffs.DeepFreeze)
                    && (actionID is HydroPull or RamsVoice or Ultravibration))
                {
                    return Common.Swiftcast;
                }

                if (IsSpellActive(Ultravibration) && WasLastSpell(RamsVoice) && TargetHasEffectAny(Debuffs.DeepFreeze)
                    && (actionID is HydroPull or RamsVoice or Ultravibration))
                {
                    return Ultravibration;
                }
            }

            return actionID;
        }
    }

    internal class BLU_BloodDrain : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.BLU_BloodDrain;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (actionID is GoblinPunch or SonicBoom or ChocoMeteor or Blaze && IsEnabled(Presets.BLU_BloodDrain) && !HasEffect(Buffs.PhantomFlurry))
            {
                if (CurrentMP <= GetOptionValue(Config.BLU_BloodDrain) && IsSpellActive(BloodDrain) && (actionID is GoblinPunch or SonicBoom or ChocoMeteor or Blaze))
                {
                    return BloodDrain;
                }
            }

            return actionID;
        }
    }

    internal class BLU_Tanking : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.BLU_Tanking;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (actionID is GoblinPunch && IsEnabled(Presets.BLU_Tanking)
                && HasEffect(Buffs.TankMimicry) && !HasEffect(Buffs.PhantomFlurry) && GetPartyMembers().Length > 0)
            {
                if (!HasEffect(Buffs.MightyGuard) && IsSpellActive(MightyGuard) && actionID is GoblinPunch)
                {
                    return MightyGuard;
                }

                if (IsEnabled(Presets.BLU_Tank_ToadOil) && !HasEffect(Buffs.ToadOil) && IsSpellActive(ToadOil) && !WasLastSpell(ToadOil) && actionID is GoblinPunch)
                {
                    return ToadOil;
                }

                if (IsOffCooldown(Devour) & InActionRange(Devour) && IsSpellActive(Devour) && actionID is GoblinPunch)
                {
                    return Devour;
                }

                if (IsEnabled(Presets.BLU_Tank_Peculiar) && IsOffCooldown(PeculiarLight) & InMeleeRange() && IsSpellActive(PeculiarLight) && actionID is GoblinPunch)
                {
                    return PeculiarLight;
                }

                if (IsSpellActive(WhiteWind) && PlayerHealthPercentageHp() <= GetOptionValue(Config.BLU_TankWhiteWind)
                    && CurrentMP >= GetResourceCost(WhiteWind) && actionID is GoblinPunch)
                {
                    return WhiteWind;
                }

                if (IsEnabled(Presets.BLU_Tank_PeatClean) && !TargetHasEffectAny(Debuffs.PeatPelt) && actionID is GoblinPunch
                    && (!HasEffect(Buffs.DeepClean) || EffectRemainingTime(Buffs.DeepClean) < 2) && !WasLastSpell(PeatPelt) && IsSpellActive(PeatPelt))
                {
                    return PeatPelt;
                }

                if (IsEnabled(Presets.BLU_Tank_PeatClean) && actionID is GoblinPunch && (WasLastSpell(PeatPelt) || TargetHasEffectAny(Debuffs.PeatPelt))
                    && IsSpellActive(DeepClean))
                {
                    return DeepClean;
                }

                if (HasEffect(Buffs.DeepClean) && IsSpellActive(GoblinPunch) && actionID is GoblinPunch)
                {
                    return GoblinPunch;
                }
            }

            return actionID;
        }
    }

    internal class BLU_PhantomEnder : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.BLU_PhantomEnder;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (actionID is GoblinPunch or SonicBoom or ChocoMeteor or Blaze && IsEnabled(Presets.BLU_PhantomEnder))
            {
                if (HasEffect(Buffs.PhantomFlurry) && (actionID is GoblinPunch or SonicBoom or ChocoMeteor or Blaze))
                {
                    if (EffectRemainingTime(Buffs.PhantomFlurry) <= 0.75f && (actionID is GoblinPunch or SonicBoom or ChocoMeteor or Blaze))
                    {
                        return OriginalHook(PhantomFlurry);
                    }

                    return OriginalHook(11);
                }
            }

            return actionID;
        }
    }

    internal class BLU_Treasure_Tank : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.BLU_Treasure_Tank;

        protected override uint Invoke(uint actionID, uint lastComboMove)
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

                    if (IsEnabled(Presets.BLU_Treasure_Tank_AutoSpell)
                        && CurrentTarget == null && !InCombat() && !IsCasting()
                        && IsOffCooldown(ShockStrike) && IsOffCooldown(Gobskin) && IsOffCooldown(Quasar) && IsOffCooldown(SeaShanty)
                        && (!IsSpellActive(RamsVoice) || !IsSpellActive(Missile)
                        || !IsSpellActive(Ultravibration) || !IsSpellActive(HydroPull)))
                    {
                        for (var i = 0; i < 24; i++)
                        {
                            if (!ActionQueued())
                            {
                                if (GetActiveBlueMageActionInSlot(18) == ShockStrike && IsOffCooldown(ShockStrike) && IsSpellActive(ShockStrike))
                                {
                                    AssignBlueMageActionToSlot(18, RamsVoice);
                                }

                                if (GetActiveBlueMageActionInSlot(19) == Quasar && IsOffCooldown(Quasar) && IsSpellActive(Quasar))
                                {
                                    AssignBlueMageActionToSlot(19, Missile);
                                }

                                if (GetActiveBlueMageActionInSlot(20) == Rehydration && IsOffCooldown(Rehydration) && IsSpellActive(Rehydration))
                                {
                                    AssignBlueMageActionToSlot(20, ShockStrike);
                                }

                                if (GetActiveBlueMageActionInSlot(21) == SeaShanty && IsOffCooldown(SeaShanty) && IsSpellActive(SeaShanty))
                                {
                                    AssignBlueMageActionToSlot(21, Ultravibration);
                                }

                                if ((GetActiveBlueMageActionInSlot(22) == PeripheralSynthesis && IsOffCooldown(PeripheralSynthesis) && IsSpellActive(PeripheralSynthesis))
                                    || (GetActiveBlueMageActionInSlot(22) == RamsVoice && IsOffCooldown(RamsVoice) && IsSpellActive(RamsVoice)))
                                {
                                    AssignBlueMageActionToSlot(22, HydroPull);
                                }

                                if ((GetActiveBlueMageActionInSlot(23) == MustardBomb && IsOffCooldown(MustardBomb) && IsSpellActive(MustardBomb))
                                    || (GetActiveBlueMageActionInSlot(23) == Ultravibration && IsOffCooldown(Ultravibration) && IsSpellActive(Ultravibration)))
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

                    if (IsEnabled(Presets.BLU_Treasure_Tank_Rehydration) && IsSpellActive(Rehydration)
                        && PlayerHealthPercentageHp() <= GetOptionValue(Config.BLU_TreasureRehydration) && !WasLastSpell(Devour) && !WasLastSpell(WhiteWind))
                    {
                        if (ActionReady(Common.Swiftcast))
                        {
                            return Common.Swiftcast;
                        }
                    }

                    if (HasEffect(Common.Buffs.Swiftcast))
                    {
                        return Rehydration;
                    }

                    if (IsEnabled(Presets.BLU_Treasure_Tank_Devour) && IsOffCooldown(Devour) && IsSpellActive(Devour))
                    {
                        return Devour;
                    }

                    if (IsEnabled(Presets.BLU_Treasure_Tank_WhiteWind) && IsSpellActive(WhiteWind)
                        && PlayerHealthPercentageHp() <= GetOptionValue(Config.BLU_TreasureWhiteWind)
                        && CurrentMP >= GetResourceCost(WhiteWind))
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

                    if (IsEnabled(Presets.BLU_Treasure_Tank_AutoSpell)
                        && CurrentTarget == null && !InCombat() && !IsCasting() && Svc.DutyState.IsDutyStarted
                        && IsOffCooldown(RamsVoice) && IsOffCooldown(Missile) && IsOffCooldown(Ultravibration) && IsOffCooldown(HydroPull)
                        && (!IsSpellActive(ShockStrike) || !IsSpellActive(Gobskin)
                        || !IsSpellActive(Quasar) || !IsSpellActive(SeaShanty)))
                    {
                        for (var i = 0; i < 24; i++)
                        {
                            if (!ActionQueued())
                            {
                                if (GetActiveBlueMageActionInSlot(18) == RamsVoice && IsOffCooldown(RamsVoice) && IsSpellActive(RamsVoice))
                                {
                                    AssignBlueMageActionToSlot(18, ShockStrike);
                                }

                                if (GetActiveBlueMageActionInSlot(19) == Missile && IsOffCooldown(Missile) && IsSpellActive(Missile))
                                {
                                    AssignBlueMageActionToSlot(19, Quasar);
                                }

                                if (GetActiveBlueMageActionInSlot(20) == ShockStrike && IsOffCooldown(ShockStrike) && IsSpellActive(ShockStrike))
                                {
                                    AssignBlueMageActionToSlot(20, Rehydration);
                                }

                                if (GetActiveBlueMageActionInSlot(21) == Ultravibration && IsOffCooldown(Ultravibration) && IsSpellActive(Ultravibration))
                                {
                                    AssignBlueMageActionToSlot(21, SeaShanty);
                                }

                                if (Svc.ClientState.TerritoryType is not Maps.Zonure1 and not Maps.Zonure2)
                                {
                                    if (GetActiveBlueMageActionInSlot(22) == HydroPull && IsOffCooldown(HydroPull) && IsSpellActive(HydroPull))
                                    {
                                        AssignBlueMageActionToSlot(22, PeripheralSynthesis);
                                    }

                                    if (GetActiveBlueMageActionInSlot(23) == DimensionalShift && IsOffCooldown(DimensionalShift) && IsSpellActive(DimensionalShift))
                                    {
                                        AssignBlueMageActionToSlot(23, MustardBomb);
                                    }
                                }

                                if (Svc.ClientState.TerritoryType is Maps.Zonure1 or Maps.Zonure2)
                                {
                                    if (GetActiveBlueMageActionInSlot(22) == HydroPull && IsOffCooldown(HydroPull) && IsSpellActive(HydroPull))
                                    {
                                        AssignBlueMageActionToSlot(22, RamsVoice);
                                    }

                                    if (GetActiveBlueMageActionInSlot(23) == DimensionalShift && IsOffCooldown(DimensionalShift) && IsSpellActive(DimensionalShift))
                                    {
                                        AssignBlueMageActionToSlot(23, Ultravibration);
                                    }
                                }
                            }
                        }
                    }

                    if (IsEnabled(Presets.BLU_Treasure_Tank_Rehydration) && IsSpellActive(Rehydration)
                        && PlayerHealthPercentageHp() <= GetOptionValue(Config.BLU_TreasureRehydration)
                        && ActionReady(Common.Swiftcast) && !WasLastSpell(WhiteWind) && !WasLastSpell(Devour) && !WasLastSpell(WhiteWind))
                    {
                        return Common.Swiftcast;
                    }

                    if (HasEffect(Common.Buffs.Swiftcast))
                    {
                        return Rehydration;
                    }

                    if (IsEnabled(Presets.BLU_Treasure_Tank_Devour) && IsOffCooldown(Devour) && IsSpellActive(Devour))
                    {
                        return Devour;
                    }

                    if (IsEnabled(Presets.BLU_Treasure_Tank_WhiteWind) && IsSpellActive(WhiteWind)
                        && PlayerHealthPercentageHp() <= GetOptionValue(Config.BLU_TreasureWhiteWind)
                        && CurrentMP >= GetResourceCost(WhiteWind))
                    {
                        return WhiteWind;
                    }

                    if (IsEnabled(Presets.BLU_Treasure_Tank_BreathOfMagic) && TargetIsBoss() && TargetWorthDoT()
                        && !TargetHasEffectAny(Debuffs.BreathOfMagic) && !WasLastSpell(BreathOfMagic))
                    {
                        if (!HasEffect(Buffs.Bristle))
                        {
                            return Bristle;
                        }

                        return BreathOfMagic;
                    }

                    if (IsEnabled(Presets.BLU_Treasure_Tank_MortalFlame) && TargetIsBoss() && TargetWorthDoT()
                        && !TargetHasEffectAny(Debuffs.MortalFlame) && !WasLastSpell(MortalFlame))
                    {
                        if (!HasEffect(Buffs.Bristle))
                        {
                            return Bristle;
                        }

                        return MortalFlame;
                    }

                    if (IsEnabled(Presets.BLU_Treasure_Tank_TripleTrident) && IsSpellActive(TripleTrident)
                        && TargetIsBoss() && TargetWorthDoT()
                        && (IsOffCooldown(TripleTrident) || GetCooldownRemainingTime(TripleTrident) < 5) && !WasLastSpell(TripleTrident))
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
