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
            BLU_ManaGain = new("BLU_ManaGain", 1500),
            BLU_TankWhiteWind = new("BLU_TankWhiteWind", 50),
            BLU_TreasurePomcure = new("BLU_TreasurePomcure", 75),
            BLU_TreasureGobskin = new("BLU_TreasureGobskin", 5),
            BLU_TreasureWhiteWind = new("BLU_TreasureWhiteWind", 60),
            BLU_TreasureRehydration = new("BLU_TreasureRehydration", 30);
    }

    internal class BLU_MoonFluteOpener : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.BLU_MoonFluteOpener;

        protected override uint Invoke(uint actionID, uint lastComboActionID)
        {
            if (actionID is MoonFlute && IsEnabled(Presets.BLU_MoonFluteOpener))
            {
                if (HasEffect(Buffs.PhantomFlurry))
                {
                    return OriginalHook(11);
                }

                if (GetCooldownRemainingTime(PhantomFlurry) > 60
                    && HasEffect(Buffs.WingedReprobation)
                    && !IsEnabled(Presets.BLU_MoonFluteOpener_DoTOpener))
                {
                    return WingedReprobation;
                }

                if (GetCooldownRemainingTime(PhantomFlurry) > 20)
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
                    if (IsSpellActive(MoonFlute))
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

                if (IsEnabled(Presets.BLU_MoonFluteOpener_DoTOpener) && actionID is MoonFlute)
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
                            if (IsSpellActive(BreathOfMagic) && actionID is MoonFlute && IsSpellActive(BreathOfMagic) && WasLastAbility(SeaShanty))
                            {
                                return BreathOfMagic;
                            }

                            if (IsSpellActive(MortalFlame) && actionID is MoonFlute && IsSpellActive(MortalFlame) && WasLastAbility(SeaShanty))
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

                    if (IsOffCooldown(MatraMagic) && actionID is MoonFlute && IsSpellActive(MatraMagic))
                    {
                        return MatraMagic;
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

                if (!IsEnabled(Presets.BLU_MoonFluteOpener_DoTOpener) && actionID is MoonFlute)
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

                if (!TargetHasEffectAny(Debuffs.Offguard) && IsOffCooldown(Offguard)
                    && actionID is FinalSting && IsSpellActive(Offguard))
                {
                    return Offguard;
                }

                if (!HasEffect(Buffs.Tingle) && actionID is FinalSting && IsSpellActive(Tingle))
                {
                    return Tingle;
                }

                if (!HasEffect(Buffs.BasicInstinct) && IsSpellActive(BasicInstinct)
                    && actionID is FinalSting && HasCondition(ConditionFlag.BoundByDuty) && GetPartyMembers().Length == 0)
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

                if (IsSpellActive(SongOfTorment) && actionID is Bristle
                    && (!TargetHasEffectAny(Debuffs.Bleeding) || TargetEffectRemainingTimeAny(Debuffs.Bleeding) < 3))
                {
                    return SongOfTorment;
                }
            }

            return actionID;
        }
    }

    internal class BLU_Periph : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.BLU_Periph;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (actionID is PeripheralSynthesis or MustardBomb && IsEnabled(Presets.BLU_Periph))
            {
                if (IsSpellActive(MustardBomb)
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

    internal class BLU_Ultravibration : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.BLU_Ultravibration;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (actionID is HydroPull or RamsVoice or Ultravibration && IsEnabled(Presets.BLU_Ultravibration))
            {
                if (!InCombat())
                {
                    return Ultravibration;
                }

                if (IsSpellActive(HydroPull) && !WasLastSpell(HydroPull) && !WasLastSpell(RamsVoice) && !TargetHasEffectAny(Debuffs.DeepFreeze))
                {
                    return HydroPull;
                }

                if (IsSpellActive(RamsVoice) && !TargetHasEffectAny(Debuffs.DeepFreeze) && (WasLastSpell(HydroPull) || !IsSpellActive(HydroPull)))
                {
                    return RamsVoice;
                }

                if (WasLastSpell(RamsVoice) && IsOffCooldown(Common.Swiftcast) && IsOffCooldown(Ultravibration))
                {
                    return Common.Swiftcast;
                }

                if (IsSpellActive(Ultravibration) && WasLastSpell(RamsVoice))
                {
                    return Ultravibration;
                }
            }

            return actionID;
        }
    }

    internal class BLU_ManaGain : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.BLU_ManaGain;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (actionID is GoblinPunch or SonicBoom or ChocoMeteor or Blaze && IsEnabled(Presets.BLU_ManaGain)
                && !HasEffect(Buffs.PhantomFlurry))
            {
                if (CurrentMP <= GetOptionValue(Config.BLU_ManaGain) && IsSpellActive(BloodDrain))
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
                if (!HasEffect(Buffs.MightyGuard) && IsSpellActive(MightyGuard))
                {
                    return MightyGuard;
                }

                if (IsEnabled(Presets.BLU_Tank_ToadOil) && !HasEffect(Buffs.ToadOil) && IsSpellActive(ToadOil) && !WasLastSpell(ToadOil))
                {
                    return ToadOil;
                }

                if (IsOffCooldown(Devour) & InActionRange(Devour) && IsSpellActive(Devour))
                {
                    return Devour;
                }

                if (IsEnabled(Presets.BLU_Tank_Peculiar) && IsOffCooldown(PeculiarLight) & InMeleeRange() && IsSpellActive(PeculiarLight))
                {
                    return PeculiarLight;
                }

                if (IsSpellActive(WhiteWind) && PlayerHealthPercentageHp() <= GetOptionValue(Config.BLU_TankWhiteWind) && CurrentMP >= GetResourceCost(WhiteWind))
                {
                    return WhiteWind;
                }

                if (IsEnabled(Presets.BLU_Tank_PeatClean) && !TargetHasEffectAny(Debuffs.PeatPelt)
                    && (!HasEffect(Buffs.DeepClean) || EffectRemainingTime(Buffs.DeepClean) < 2) && !WasLastSpell(PeatPelt) && IsSpellActive(PeatPelt))
                {
                    return PeatPelt;
                }

                if (IsEnabled(Presets.BLU_Tank_PeatClean) && (WasLastSpell(PeatPelt) || TargetHasEffectAny(Debuffs.PeatPelt)) && IsSpellActive(DeepClean))
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

    internal class BLU_PhantomEnder : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.BLU_PhantomEnder;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (actionID is GoblinPunch or SonicBoom or ChocoMeteor or Blaze && IsEnabled(Presets.BLU_PhantomEnder))
            {
                if (HasEffect(Buffs.PhantomFlurry))
                {
                    if (EffectRemainingTime(Buffs.PhantomFlurry) <= 0.75f)
                    {
                        return OriginalHook(PhantomFlurry);
                    }

                    return OriginalHook(11);
                }
            }

            return actionID;
        }
    }

    internal class BLU_Treasure_Healer : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.BLU_Treasure_Healer;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            var notInMap = Svc.ClientState.TerritoryType is not Maps.Dragonskin and not Maps.Gazelle1
                and not Maps.Gazelle2 and not Maps.Thief and not Maps.Zonure1 and not Maps.Zonure2;

            var inMap = Svc.ClientState.TerritoryType is Maps.Dragonskin or Maps.Gazelle1
                or Maps.Gazelle2 or Maps.Thief or Maps.Zonure1 or Maps.Zonure2;

            if (actionID is GoblinPunch && IsEnabled(Presets.BLU_Treasure_Healer) && HasEffect(Buffs.HealerMimicry)
                && !HasEffect(Buffs.PhantomFlurry) && GetPartyMembers().Length == 0)
            {
                if (notInMap)
                {
                    if (IsEnabled(Presets.BLU_Treasure_Healer_MightyGuard) && HasEffect(Buffs.MightyGuard))
                    {
                        return MightyGuard;
                    }

                    if (IsEnabled(Presets.BLU_Treasure_Healer_AutoSpell)
                        && CurrentTarget == null && !InCombat() && !IsCasting()
                        && IsOffCooldown(ShockStrike) && IsOffCooldown(Gobskin) && IsOffCooldown(Quasar) && IsOffCooldown(SeaShanty)
                        && (!IsSpellActive(RamsVoice) || !IsSpellActive(Missile)
                        || !IsSpellActive(Ultravibration) || !IsSpellActive(HydroPull)))
                    {
                        for (var i = 0; i < 24; i++)
                        {
                            if (!ActionQueued())
                            {
                                if (GetActiveBlueMageActionInSlot(20) == ShockStrike)
                                {
                                    if (IsOffCooldown(ShockStrike) && IsSpellActive(ShockStrike))
                                    {
                                        AssignBlueMageActionToSlot(20, RamsVoice);
                                    }
                                }

                                if (GetActiveBlueMageActionInSlot(21) == Gobskin)
                                {
                                    if (IsOffCooldown(Gobskin) && IsSpellActive(Gobskin))
                                    {
                                        AssignBlueMageActionToSlot(21, Missile);
                                    }
                                }

                                if (GetActiveBlueMageActionInSlot(22) == Quasar)
                                {
                                    if (IsOffCooldown(Quasar) && IsSpellActive(Quasar))
                                    {
                                        AssignBlueMageActionToSlot(22, Ultravibration);
                                    }
                                }

                                if (GetActiveBlueMageActionInSlot(23) == SeaShanty)
                                {
                                    if (IsOffCooldown(SeaShanty) && IsSpellActive(SeaShanty))
                                    {
                                        AssignBlueMageActionToSlot(23, HydroPull);
                                    }
                                }
                            }
                        }
                    }

                    if (!IsSpellActive(RamsVoice) || !IsSpellActive(Missile) || !IsSpellActive(Ultravibration) || !IsSpellActive(HydroPull))
                    {
                        return OriginalHook(11);
                    }

                    if (IsEnabled(Presets.BLU_Treasure_Healer_AngelsSnack)
                        && PlayerHealthPercentageHp() <= GetOptionValue(Config.BLU_TreasurePomcure) && IsOffCooldown(AngelsSnack)
                        && IsSpellActive(AngelsSnack))
                    {
                        if (ActionReady(Common.Swiftcast))
                        {
                            return Common.Swiftcast;
                        }

                        return AngelsSnack;
                    }
                }

                if (inMap)
                {
                    if (IsEnabled(Presets.BLU_Treasure_Healer_BasicInstinct) && !HasEffect(Buffs.BasicInstinct))
                    {
                        return BasicInstinct;
                    }

                    if (IsEnabled(Presets.BLU_Treasure_Healer_MightyGuard) && !HasEffect(Buffs.MightyGuard))
                    {
                        return MightyGuard;
                    }

                    if (IsEnabled(Presets.BLU_Treasure_Healer_AutoSpell)
                        && CurrentTarget == null && !InCombat() && !IsCasting() && Svc.DutyState.IsDutyStarted
                        && IsOffCooldown(RamsVoice) && IsOffCooldown(Missile) && IsOffCooldown(Ultravibration) && IsOffCooldown(HydroPull)
                        && (!IsSpellActive(ShockStrike) || !IsSpellActive(Gobskin)
                        || !IsSpellActive(Quasar) || !IsSpellActive(SeaShanty)))
                    {
                        for (var i = 0; i < 24; i++)
                        {
                            if (!ActionQueued())
                            {
                                if (GetActiveBlueMageActionInSlot(20) == RamsVoice)
                                {
                                    if (IsOffCooldown(RamsVoice) && IsSpellActive(RamsVoice))
                                    {
                                        AssignBlueMageActionToSlot(20, ShockStrike);
                                    }
                                }


                                if (GetActiveBlueMageActionInSlot(21) == Missile)
                                {
                                    if (IsOffCooldown(Missile) && IsSpellActive(Missile))
                                    {
                                        AssignBlueMageActionToSlot(21, Gobskin);
                                    }
                                }

                                if (GetActiveBlueMageActionInSlot(22) == Ultravibration)
                                {
                                    if (IsOffCooldown(Ultravibration) && IsSpellActive(Ultravibration))
                                    {
                                        AssignBlueMageActionToSlot(22, Quasar);
                                    }
                                }

                                if (GetActiveBlueMageActionInSlot(23) == HydroPull)
                                {
                                    if (IsOffCooldown(HydroPull) && IsSpellActive(HydroPull))
                                    {
                                        AssignBlueMageActionToSlot(23, SeaShanty);
                                    }
                                }
                            }
                        }
                    }

                    if (IsEnabled(Presets.BLU_Treasure_Healer_AngelsSnack)
                        && PlayerHealthPercentageHp() <= GetOptionValue(Config.BLU_TreasurePomcure)
                        && ActionReady(AngelsSnack) && IsSpellActive(AngelsSnack))
                    {
                        if (PlayerHealthPercentageHp() < 50 && ActionReady(Common.Swiftcast))
                        {
                            return Common.Swiftcast;
                        }

                        return AngelsSnack;
                    }

                    if (IsEnabled(Presets.BLU_Treasure_Healer_Pomcure)
                        && PlayerHealthPercentageHp() <= GetOptionValue(Config.BLU_TreasurePomcure)
                        && IsOnCooldown(AngelsSnack) && IsSpellActive(Pomcure)
                        && !HasEffect(Buffs.AngelsSnack) && !WasLastSpell(AngelsSnack))
                    {
                        if (PlayerHealthPercentageHp() < 50 && ActionReady(Common.Swiftcast) && !WasLastSpell(Pomcure))
                        {
                            return Common.Swiftcast;
                        }

                        return Pomcure;
                    }

                    if (IsEnabled(Presets.BLU_Treasure_Healer_Gobskin)
                        && ShieldPercentage <= GetOptionValue(Config.BLU_TreasureGobskin) && IsSpellActive(Gobskin))
                    {
                        return Gobskin;
                    }

                    if (IsEnabled(Presets.BLU_Treasure_Healer_BreathOfMagic) && TargetIsBoss()
                        && !TargetHasEffectAny(Debuffs.BreathOfMagic) && !WasLastSpell(BreathOfMagic)
                        && TargetWorthDoT())
                    {
                        if (!HasEffect(Buffs.Bristle))
                        {
                            return Bristle;
                        }

                        return BreathOfMagic;
                    }

                    if (IsEnabled(Presets.BLU_Treasure_Healer_MortalFlame) && TargetIsBoss()
                        && !TargetHasEffectAny(Debuffs.MortalFlame) && !WasLastSpell(MortalFlame)
                        && TargetWorthDoT())
                    {
                        if (!HasEffect(Buffs.Bristle))
                        {
                            return Bristle;
                        }

                        return MortalFlame;
                    }

                    if (IsEnabled(Presets.BLU_Treasure_Healer_TripleTrident) && IsSpellActive(TripleTrident)
                        && TargetIsBoss()
                        && (IsOffCooldown(TripleTrident) || GetCooldownRemainingTime(TripleTrident) < 5) && !WasLastSpell(TripleTrident)
                        && TargetWorthDoT())
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

    internal class BLU_Treasure_Tank : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.BLU_Treasure_Tank;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            var notInMap = Svc.ClientState.TerritoryType is not Maps.Dragonskin and not Maps.Gazelle1
                and not Maps.Gazelle2 and not Maps.Thief and not Maps.Zonure1 and not Maps.Zonure2;

            var inMap = Svc.ClientState.TerritoryType is Maps.Dragonskin or Maps.Gazelle1
                or Maps.Gazelle2 or Maps.Thief or Maps.Zonure1 or Maps.Zonure2;

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
                                if (GetActiveBlueMageActionInSlot(20) == ShockStrike)
                                {
                                    if (IsOffCooldown(ShockStrike) && IsSpellActive(ShockStrike))
                                    {
                                        AssignBlueMageActionToSlot(20, RamsVoice);
                                    }
                                }

                                if (GetActiveBlueMageActionInSlot(21) == Quasar)
                                {
                                    if (IsOffCooldown(Quasar) && IsSpellActive(Quasar))
                                    {
                                        AssignBlueMageActionToSlot(21, Missile);
                                    }
                                }

                                if (GetActiveBlueMageActionInSlot(22) == Rehydration)
                                {
                                    if (IsOffCooldown(Rehydration) && IsSpellActive(Rehydration))
                                    {
                                        AssignBlueMageActionToSlot(22, Ultravibration);
                                    }
                                }

                                if (GetActiveBlueMageActionInSlot(23) == SeaShanty)
                                {
                                    if (IsOffCooldown(SeaShanty) && IsSpellActive(SeaShanty))
                                    {
                                        AssignBlueMageActionToSlot(23, HydroPull);
                                    }
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
                                if (GetActiveBlueMageActionInSlot(20) == RamsVoice)
                                {
                                    if (IsOffCooldown(RamsVoice) && IsSpellActive(RamsVoice))
                                    {
                                        AssignBlueMageActionToSlot(20, ShockStrike);
                                    }
                                }


                                if (GetActiveBlueMageActionInSlot(21) == Missile)
                                {
                                    if (IsOffCooldown(Missile) && IsSpellActive(Missile))
                                    {
                                        AssignBlueMageActionToSlot(21, Quasar);
                                    }
                                }

                                if (GetActiveBlueMageActionInSlot(22) == Ultravibration)
                                {
                                    if (IsOffCooldown(Ultravibration) && IsSpellActive(Ultravibration))
                                    {
                                        AssignBlueMageActionToSlot(22, Rehydration);
                                    }
                                }

                                if (GetActiveBlueMageActionInSlot(23) == HydroPull)
                                {
                                    if (IsOffCooldown(HydroPull) && IsSpellActive(HydroPull))
                                    {
                                        AssignBlueMageActionToSlot(23, SeaShanty);
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

                    if (IsEnabled(Presets.BLU_Treasure_Tank_BreathOfMagic) && TargetIsBoss()
                        && !TargetHasEffectAny(Debuffs.BreathOfMagic) && !WasLastSpell(BreathOfMagic)
                        && TargetWorthDoT())
                    {
                        if (!HasEffect(Buffs.Bristle))
                        {
                            return Bristle;
                        }

                        return BreathOfMagic;
                    }

                    if (IsEnabled(Presets.BLU_Treasure_Tank_MortalFlame) && TargetIsBoss()
                        && !TargetHasEffectAny(Debuffs.MortalFlame) && !WasLastSpell(MortalFlame)
                        && TargetWorthDoT())
                    {
                        if (!HasEffect(Buffs.Bristle))
                        {
                            return Bristle;
                        }

                        return MortalFlame;
                    }

                    if (IsEnabled(Presets.BLU_Treasure_Tank_TripleTrident) && IsSpellActive(TripleTrident)
                        && TargetIsBoss()
                        && (IsOffCooldown(TripleTrident) || GetCooldownRemainingTime(TripleTrident) < 5) && !WasLastSpell(TripleTrident)
                        && TargetWorthDoT())
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
