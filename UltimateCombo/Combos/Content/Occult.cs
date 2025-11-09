using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Combos.General;
using UltimateCombo.Combos.PvE;
using UltimateCombo.Core;
using UltimateCombo.Data;

namespace UltimateCombo.Combos.Content;

internal static class Occult
{
    internal const uint
        OccultResuscitation = 41650,
        OccultTreasureSight = 41651,

        PhantomGuard = 41588,
        Pray = 41589,
        OccultHeal = 41590,
        Pledge = 41591,

        PhantomKick = 41595,
        OccultCounter = 41596,
        Counterstance = 41597,
        OccultChakra = 41598,

        PhantomAim = 41599,

        OccultSlowga = 41621,
        OccultDispel = 41622,
        OccultComet = 41623,
        OccultMageMasher = 41624,
        OccultQuick = 41625,

        OffensiveAria = 41608,
        RomeosBallad = 41609,
        MightyMarch = 41607,
        HerosRime = 41610,

        Predict = 41636,
        PhantomJudgement = 41637,
        Cleansing = 41638,
        Blessing = 41639,
        Starfall = 41640,
        Recuperation = 41641,
        PhantomDoom = 41642,
        PhantomRejuvenation = 41643,
        Invulnerability = 41644,

        PhantomFire = 41626,
        HolyCannon = 41627,
        DarkCannon = 41628,
        ShockCannon = 41629,
        SilverCannon = 41630,

        Mineuchi = 41603,
        Shirahadori = 41604,
        Iainuki = 41605,
        Zeninage = 41606,

        BattleBell = 41611,
        Weather = 41612,
        Sunbath = 41613,
        CloudyCaress = 41614,
        BlessedRain = 41615,
        MistyMirage = 41616,
        HastyMirage = 41617,
        AetherialGain = 41618,
        RingingRespite = 41619,
        Suspend = 41620,

        Sprint = 41646,
        Steal = 41645,
        Vigilance = 41647,
        TrapDetection = 41648,
        PilferWeapon = 41649,

        DeadlyBlow = 41594,
        Rage = 41592;

    internal static class PhantomJobs
    {
        internal const ushort
            Freelancer = 4242,
            Knight = 4358,
            Berserker = 4359,
            Monk = 4360,
            Ranger = 4361,
            Samurai = 4362,
            Bard = 4363,
            Geomancer = 4364,
            TimeMage = 4365,
            Cannoneer = 4366,
            Chemist = 4367,
            Oracle = 4368,
            Thief = 4369;
    }

    internal static class Buffs
    {
        internal const ushort
            Pray = 4232,

            PhantomKick = 4237,
            Counterstance = 4238,

            OffensiveAria = 4247,
            HerosRime = 4249,

            PredictionOfJudgement = 4265,
            PredictionOfCleansing = 4266,
            PredictionOfBlessing = 4267,
            PredictionOfStarfall = 4268,
            Rejuvination = 44274,

            BattleBell = 4251,
            BattlesClangor = 4252,
            RingingRespite = 4257,

            Vigilance = 4277,

            Rage = 4235,
            PentUpRage = 4236,

            OccultQuick = 4260,
            OccultSwift = 4261;
    }

    internal static class Debuffs
    {
        internal const ushort
            Slow = 3493,
            OccultMageMasher = 4259,
            WeaponPilfered = 4279;
    }

    internal static class Config
    {
        internal static readonly UserBool
            Occult_KickNotice = new("Occult_KickNotice");

        internal static readonly UserInt
            Occult_PhantomResuscitation = new("Occult_PhantomResuscitation", 70),
            Occult_Pray = new("Occult_Pray", 75),
            Occult_Heal = new("Occult_Heal", 25),
            Occult_Sunbath = new("Occult_Sunbath", 75);

        internal static readonly UserIntArray
            Occult_Prediction = new("Occult_Prediction"),
            Occult_HolySilverCannon = new("Occult_HolySilverCannon"),
            Occult_DarkShockCannon = new("Occult_DarkShockCannon");
    }

    internal class Occult_Knight : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.Occult_Knight;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.Occult_Knight) && HasEffect(PhantomJobs.Knight) && InCombat() && SafeToUse() && IsComboAction(actionID))
            {
                if (IsEnabled(Presets.Occult_Pray) && DutyActionReady(Pray) && !HasEffect(Buffs.Pray)
                    && PlayerHealthPercentageHp() <= GetOptionValue(Config.Occult_Pray))
                {
                    return Pray;
                }

                if (IsEnabled(Presets.Occult_Heal) && DutyActionReady(OccultHeal)
                    && PlayerHealthPercentageHp() <= GetOptionValue(Config.Occult_Heal)
                    && CurrentMP >= GetResourceCost(OccultHeal))
                {
                    return OccultHeal;
                }
            }

            return actionID;
        }
    }

    internal class Occult_Monk : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.Occult_Monk;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.Occult_Monk) && HasEffect(PhantomJobs.Monk) && InCombat() && SafeToUse() && IsComboAction(actionID))
            {
                if (IsEnabled(Presets.Occult_Chakra) && DutyActionReady(OccultChakra) && PlayerHealthPercentageHp() < 30)
                {
                    return OccultChakra;
                }

                if (IsEnabled(Presets.Occult_PhantomKick) && DutyActionReady(PhantomKick))
                {
                    if (EffectRemainingTime(Buffs.PhantomKick) < 5 && !InActionRange(PhantomKick) && GetOptionBool(Config.Occult_KickNotice))
                    {
                        return PhantomKick;
                    }

                    if (EffectRemainingTime(Buffs.PhantomKick) < 1 && InActionRange(PhantomKick))
                    {
                        return PhantomKick;
                    }
                }

                if (IsEnabled(Presets.Occult_Counter) && DutyActionReady(OccultCounter) && InActionRange(OccultCounter)
                    && CanWeave(actionID, ActionWatching.LastGCD))
                {
                    return OccultCounter;
                }

                if (IsEnabled(Presets.Occult_Counterstance) && DutyActionReady(Counterstance)
                    && IsTargetOfTarget() && EffectRemainingTime(Buffs.Counterstance) < 2
                    && (ActionWatching.NumberOfGcdsUsed >= 3 || Service.Configuration.IgnoreGCDChecks || LevelIgnoreGCD()))
                {
                    return Counterstance;
                }
            }

            return actionID;
        }
    }

    internal class Occult_Thief : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.Occult_Thief;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.Occult_Thief) && HasEffect(PhantomJobs.Thief) && SafeToUse() && IsComboAction(actionID))
            {
                if (IsEnabled(Presets.Occult_Vigilance) && DutyActionReady(Vigilance)
                    && !InCombat() && !HasEffect(Buffs.Vigilance) && HasBattleTarget())
                {
                    return Vigilance;
                }

                if (InCombat() && CanWeave(actionID, ActionWatching.LastGCD))
                {
                    if (IsEnabled(Presets.Occult_PilferWeapon) && DutyActionReady(PilferWeapon)
                        && !TargetHasEffectAny(Debuffs.WeaponPilfered) && InActionRange(PilferWeapon))
                    {
                        return PilferWeapon;
                    }

                    if (IsEnabled(Presets.Occult_Steal) && DutyActionReady(Steal) && InActionRange(Steal) && HasBattleTarget())
                    {
                        return Steal;
                    }
                }
            }

            return actionID;
        }
    }

    internal class Occult_Samurai : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.Occult_Samurai;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.Occult_Samurai) && HasEffect(PhantomJobs.Samurai) && InCombat() && SafeToUse()
                 && IsComboAction(actionID))
            {
                if (IsEnabled(Presets.Occult_Zeninage) && DutyActionReady(Zeninage) && InActionRange(Zeninage)
                    && (ActionWatching.NumberOfGcdsUsed >= 5 || Service.Configuration.IgnoreGCDChecks || LevelIgnoreGCD()))
                {
                    return Zeninage;
                }

                if (IsEnabled(Presets.Occult_Iainuki) && DutyActionReady(Iainuki) && InActionRange(Iainuki) && !IsMoving
                    && (ActionWatching.NumberOfGcdsUsed >= 5 || Service.Configuration.IgnoreGCDChecks || LevelIgnoreGCD()))
                {
                    return Iainuki;
                }
            }

            return actionID;
        }
    }

    internal class Occult_Berserker : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.Occult_Berserker;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.Occult_Berserker) && HasEffect(PhantomJobs.Berserker) && InCombat() && IsComboAction(actionID))
            {
                if (IsEnabled(Presets.Occult_DeadlyBlow) && DutyActionReady(DeadlyBlow) && InActionRange(DeadlyBlow)
                    && HasEffect(Buffs.PentUpRage) && EffectRemainingTime(Buffs.PentUpRage) <= 1)
                {
                    return DeadlyBlow;
                }
            }

            return actionID;
        }
    }

    internal class Occult_Ranger : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.Occult_Thief;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.Occult_Ranger) && HasEffect(PhantomJobs.Ranger) && SafeToUse() && InCombat() && CanWeave(actionID, ActionWatching.LastGCD)
                 && IsComboAction(actionID))
            {
                if (IsEnabled(Presets.Occult_Aim) && DutyActionReady(PhantomAim)
                    && (ActionWatching.NumberOfGcdsUsed >= 2 || Service.Configuration.IgnoreGCDChecks || LevelIgnoreGCD()))
                {
                    return PhantomAim;
                }
            }

            return actionID;
        }
    }

    internal class Occult_TimeMage : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.Occult_TimeMage;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.Occult_TimeMage) && HasEffect(PhantomJobs.TimeMage) && InCombat() && SafeToUse() && IsComboAction(actionID))
            {
                if (IsEnabled(Presets.Occult_Quick) && DutyActionReady(OccultQuick))
                {
                    return OccultQuick;
                }

                if (IsEnabled(Presets.Occult_Comet) && DutyActionReady(OccultComet)
                    && (ActionWatching.NumberOfGcdsUsed >= 5 || Service.Configuration.IgnoreGCDChecks || LevelIgnoreGCD()))
                {
                    if (HasEffect(Common.Buffs.Swiftcast) || HasEffect(RDM.Buffs.Dualcast) || HasEffect(BLM.Buffs.Triplecast) || HasEffect(Buffs.OccultQuick))
                    {
                        return OccultComet;
                    }
                }

                if (IsEnabled(Presets.Occult_MageMasher) && DutyActionReady(OccultMageMasher) && !TargetHasEffectAny(Debuffs.OccultMageMasher)
                    && HasBattleTarget() && CanWeave(actionID, ActionWatching.LastGCD))
                {
                    return OccultMageMasher;
                }
            }

            return actionID;
        }
    }

    internal class Occult_Chemist : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.Occult_Thief;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            return actionID;
        }
    }

    internal class Occult_Geomancer : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.Occult_Geomancer;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.Occult_Geomancer) && HasEffect(PhantomJobs.Geomancer) && SafeToUse() && IsComboAction(actionID))
            {
                if (IsEnabled(Presets.Occult_BattleBell) && DutyActionReady(BattleBell) && CanWeave(actionID, ActionWatching.LastGCD)
                    && (!HasEffectAny(Buffs.BattleBell) || (HasFriendlyTarget() && !TargetHasEffectAny(Buffs.BattleBell))))
                {
                    return BattleBell;
                }

                if (IsEnabled(Presets.Occult_Weather))
                {
                    if (DutyActionReady(Sunbath) && PlayerHealthPercentageHp() <= GetOptionValue(Config.Occult_Sunbath))
                    {
                        return Sunbath;
                    }

                    if (DutyActionReady(CloudyCaress))
                    {
                        return CloudyCaress;
                    }

                    if (DutyActionReady(BlessedRain))
                    {
                        return BlessedRain;
                    }

                    if (DutyActionReady(MistyMirage))
                    {
                        return MistyMirage;
                    }

                    if (DutyActionReady(HastyMirage))
                    {
                        return HastyMirage;
                    }

                    if (DutyActionReady(AetherialGain))
                    {
                        return AetherialGain;
                    }
                }

                if (IsEnabled(Presets.Occult_RingingRespite) && DutyActionReady(RingingRespite) && CanWeave(actionID, ActionWatching.LastGCD)
                    && (!HasEffectAny(Buffs.RingingRespite) || (HasFriendlyTarget() && !TargetHasEffectAny(Buffs.RingingRespite))))
                {
                    return RingingRespite;
                }
            }

            return actionID;
        }
    }

    internal class Occult_Bard : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.Occult_Bard;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.Occult_Bard) && HasEffect(PhantomJobs.Bard) && SafeToUse() && CanWeave(actionID, ActionWatching.LastGCD) && IsComboAction(actionID))
            {
                if (IsEnabled(Presets.Occult_HerosRime) && DutyActionReady(HerosRime)
                    && (ActionWatching.NumberOfGcdsUsed >= 5 || Service.Configuration.IgnoreGCDChecks || LevelIgnoreGCD()))
                {
                    return HerosRime;
                }

                if (IsEnabled(Presets.Occult_OffensiveAria) && DutyActionReady(OffensiveAria)
                    && (!HasEffect(Buffs.HerosRime) || EffectRemainingTime(Buffs.HerosRime) < 2)
                    && (!HasEffect(Buffs.OffensiveAria) || EffectRemainingTime(Buffs.OffensiveAria) < 5))
                {
                    return OffensiveAria;
                }
            }

            return actionID;
        }
    }

    internal class Occult_Oracle : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.Occult_Oracle;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.Occult_Oracle) && HasEffect(PhantomJobs.Oracle) && SafeToUse() && IsComboAction(actionID))
            {
                if (IsEnabled(Presets.Occult_Predict))
                {
                    if (DutyActionReady(PhantomJudgement) && (GetOptionValue(Config.Occult_Prediction) == 1
                        || (GetOptionValue(Config.Occult_Prediction) == 4 && PlayerHealthPercentageHp() <= 90)))
                    {
                        return PhantomJudgement;
                    }

                    if (DutyActionReady(Cleansing) && (GetOptionValue(Config.Occult_Prediction) == 2
                        || (GetOptionValue(Config.Occult_Prediction) == 4 && PlayerHealthPercentageHp() <= 90)))
                    {
                        return Cleansing;
                    }

                    if (DutyActionReady(Blessing) && (GetOptionValue(Config.Occult_Prediction) == 3
                        || (GetOptionValue(Config.Occult_Prediction) == 4 && PlayerHealthPercentageHp() <= 90)))
                    {
                        return Blessing;
                    }

                    if (DutyActionReady(Starfall) && GetOptionValue(Config.Occult_Prediction) == 4 && PlayerHealthPercentageHp() > 90)
                    {
                        return Starfall;
                    }
                }

                if (IsEnabled(Presets.Occult_Predict) && DutyActionReady(Predict) && InCombat()
                    && (!DutyActionReady(PhantomRejuvenation) || (DutyActionReady(PhantomRejuvenation) && EffectRemainingTime(Buffs.Rejuvination) <= 15)))
                {
                    return Predict;
                }

                if (IsEnabled(Presets.Occult_PhantomRejuvination) && DutyActionReady(PhantomRejuvenation) && CanWeave(actionID, ActionWatching.LastGCD)
                    && (GetCooldownRemainingTime(Predict) <= 8 || ActionReady(Predict)))
                {
                    return PhantomRejuvenation;
                }
            }

            return actionID;
        }
    }

    internal class Occult_Cannoneer : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.Occult_Cannoneer;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.Occult_Cannoneer) && HasEffect(PhantomJobs.Cannoneer) && SafeToUse()
                && ((IsEnabled(Presets.Occult_Cannoneer_Utility)
                && (actionID is PhantomFire or HolyCannon or DarkCannon or ShockCannon or SilverCannon))
                || (!IsEnabled(Presets.Occult_Cannoneer_Utility) && InCombat())))
            {
                if (IsEnabled(Presets.Occult_HolySilverCannon))
                {
                    if (DutyActionReady(HolyCannon) && (GetOptionValue(Config.Occult_HolySilverCannon) == 1 || !DutyActionReady(SilverCannon)))
                    {
                        return HolyCannon;
                    }

                    if (DutyActionReady(SilverCannon) && GetOptionValue(Config.Occult_HolySilverCannon) == 2)
                    {
                        return SilverCannon;
                    }
                }

                if (IsEnabled(Presets.Occult_PhantomFire) && DutyActionReady(PhantomFire))
                {
                    return PhantomFire;
                }

                if (IsEnabled(Presets.Occult_DarkShockCannon))
                {
                    if (DutyActionReady(DarkCannon) && (GetOptionValue(Config.Occult_DarkShockCannon) == 1 || !DutyActionReady(ShockCannon)))
                    {
                        return DarkCannon;
                    }

                    if (DutyActionReady(ShockCannon) && GetOptionValue(Config.Occult_DarkShockCannon) == 2)
                    {
                        return ShockCannon;
                    }
                }
            }

            return actionID;
        }
    }

    internal class Occult_Freelancer : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.Occult_Freelancer;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.Occult_Freelancer) && HasEffect(PhantomJobs.Freelancer) && InCombat() && SafeToUse()
                 && IsComboAction(actionID))
            {
                if (IsEnabled(Presets.Occult_PhantomResuscitation) && DutyActionReady(OccultResuscitation)
                    && PlayerHealthPercentageHp() <= GetOptionValue(Config.Occult_PhantomResuscitation))
                {
                    return OccultResuscitation;
                }
            }

            return actionID;
        }
    }
}
