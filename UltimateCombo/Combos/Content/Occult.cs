using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Combos.General;
using UltimateCombo.Combos.PvE;
using UltimateCombo.Core;
using UltimateCombo.Data;

namespace UltimateCombo.Combos.Content;

internal static class Occult
{
    internal const uint
        Resuscitation = 41650,

        PhantomGuard = 41588,
        Pray = 41589,
        Heal = 41590,
        Pledge = 41591,

        PhantomKick = 41595,
        Counter = 41596,
        Counterstance = 41597,
        Chakra = 41598,

        PhantomAim = 41599,

        Slowga = 41621,
        Dispel = 41622,
        Comet = 41623,
        MageMasher = 41624,
        Quick = 41625,

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
        Rage = 41592,

        SunderingSpellblade = 46591,
        MagicShell = 46590,
        HolySpellblade = 46592,
        BlazingSpellblade = 46593,

        Dance = 46598,
        SwordDance = 46599,
        TemptingTango = 46600,
        Jitterbug = 46601,
        MysteryWaltz = 46602,
        Quickstep = 46603,
        SteadfastStance = 46604,
        Mesmerize = 46605,

        Finisher = 46594,
        Defend = 46595,
        LongReach = 46596,
        Bladeblitz = 46597,

        Shuriken = 49062,
        Smoke = 49063,
        LightningScroll = 49064,
        FireScroll = 49065,
        Image = 49066,

        Cure2_WHM = 49067,
        Cure3 = 49068,
        Blink = 49069,
        Raise = 49070,
        Holy = 49071,

        Fire3 = 49072,
        Blizzard3 = 49073,
        Thunder3 = 49074,
        Toad = 49075,
        Flare = 49076,

        Jump = 49077,
        StepForth = 49078,
        Lance = 49079,

        Hellfire = 49080,
        JudgmentBolt = 49081,
        EarthenWall = 49082,
        Thunderstorm = 49083,
        Megaflare = 49084,

        Aero = 49085,
        Missile = 49086,
        AquaBreath = 49087,
        MightyGuard = 49088,
        Aero2 = 49089,
        WhiteWind = 49090,
        Aero3 = 49091,

        Fire2 = 49092,
        Cure2_RDM = 49093,
        Libra = 49094,
        Blizzard2 = 49095,
        Thunder2 = 49096,

        DrainTouch = 49097,
        DeepFreeze = 49098,
        HellWind = 49099,
        ChaosDrive = 49100,
        Doomsday = 49101;

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
            Thief = 4369,
            MysticKnight = 4803,
            Gladiator = 4804,
            Dancer = 4805,
            Ninja = 5328,
            WhiteMage = 5329,
            BlackMage = 5330,
            Dragoon = 5331,
            Summoner = 5332,
            BlueMage = 5333,
            RedMage = 5334,
            Necromancer = 5335;
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

            Quick = 4260,
            Swift = 4261,

            BlazingSpellblade = 4790,

            PoisedToSwordDance = 4794,
            TemptedToTango = 4795,
            Jitterbugged = 4796,
            WillingToWaltz = 4797,
            Quickstep = 4798,

            Smoke = 5327,

            DrainTouch = 5326;
    }

    internal static class Debuffs
    {
        internal const ushort
            Slow = 3493,
            MageMasher = 4259,
            WeaponPilfered = 4279,

            BlazingBane = 4791,

            FireWeakness = 5322,
            IceWeakness = 5323,
            LightningWeakness = 5324,
            WindWeakness = 5325;

        internal static readonly ushort[] ElementalWeaknesses =
        [
            FireWeakness,
            IceWeakness,
            LightningWeakness,
            WindWeakness
        ];
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
            Occult_DarkShockCannon = new("Occult_DarkShockCannon"),
            Occult_Spell2 = new("Occult_Spell2"),
            Occult_Spell3 = new("Occult_Spell3"),
            Occult_SMNSpell = new("Occult_SMNSpell"),
            Occult_NECSpell = new("Occult_NECSpell");
    }

    internal class Occult_Freelancer : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.Occult_Freelancer;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.Occult_Freelancer) && HasEffect(PhantomJobs.Freelancer) && InCombat() && SafeToUse() && IsComboAction(actionID))
            {
                if (IsEnabled(Presets.Occult_PhantomResuscitation) && DutyActionReady(Resuscitation)
                    && PlayerHealthPercentageHp() <= GetOptionValue(Config.Occult_PhantomResuscitation))
                {
                    return Resuscitation;
                }
            }

            return actionID;
        }
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

                if (IsEnabled(Presets.Occult_Heal) && DutyActionReady(Heal) && PlayerHealthPercentageHp() <= GetOptionValue(Config.Occult_Heal)
                    && CurrentMP >= GetResourceCost(Heal))
                {
                    return Heal;
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
                    && HasEffect(Buffs.PentUpRage) && EffectRemainingTime(Buffs.PentUpRage) <= 3)
                {
                    return DeadlyBlow;
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
                if (IsEnabled(Presets.Occult_Chakra) && DutyActionReady(Chakra) && PlayerHealthPercentageHp() < 30)
                {
                    return Chakra;
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

                if (IsEnabled(Presets.Occult_Counter) && DutyActionReady(Counter) && InActionRange(Counter) && CanWeave(actionID, ActionWatching.LastGCD))
                {
                    return Counter;
                }

                if (IsEnabled(Presets.Occult_Counterstance) && DutyActionReady(Counterstance) && IsTargetOfTarget() && !HasEffect(Buffs.Counterstance))
                {
                    return Counterstance;
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
            if (IsEnabled(Presets.Occult_Ranger) && HasEffect(PhantomJobs.Ranger) && SafeToUse() && InCombat()
                && CanWeave(actionID, ActionWatching.LastGCD) && IsComboAction(actionID))
            {
                if (IsEnabled(Presets.Occult_Aim) && DutyActionReady(PhantomAim) && GCDCheck(2))
                {
                    return PhantomAim;
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
            if (IsEnabled(Presets.Occult_Samurai) && HasEffect(PhantomJobs.Samurai) && InCombat() && SafeToUse() && IsComboAction(actionID))
            {
                if (IsEnabled(Presets.Occult_Zeninage) && DutyActionReady(Zeninage) && InActionRange(Zeninage) && GCDCheck(5))
                {
                    return Zeninage;
                }

                if (IsEnabled(Presets.Occult_Iainuki) && DutyActionReady(Iainuki) && InActionRange(Iainuki) && !IsMoving && GCDCheck(5))
                {
                    return Iainuki;
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
                if (IsEnabled(Presets.Occult_HerosRime) && DutyActionReady(HerosRime) && GCDCheck(3))
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

    internal class Occult_TimeMage : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.Occult_TimeMage;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.Occult_TimeMage) && HasEffect(PhantomJobs.TimeMage) && InCombat() && SafeToUse() && IsComboAction(actionID))
            {
                if (IsEnabled(Presets.Occult_Quick) && DutyActionReady(Quick) && !IsMoving)
                {
                    return Quick;
                }

                if (IsEnabled(Presets.Occult_Comet) && DutyActionReady(Comet) && GCDCheck(5))
                {
                    if (HasEffect(Common.Buffs.Swiftcast) || HasEffect(RDM.Buffs.Dualcast) || HasEffect(BLM.Buffs.Triplecast) || HasEffect(Buffs.Quick))
                    {
                        return Comet;
                    }
                }

                if (IsEnabled(Presets.Occult_MageMasher) && DutyActionReady(MageMasher) && !TargetHasEffectAny(Debuffs.MageMasher)
                    && HasBattleTarget() && CanWeave(actionID, ActionWatching.LastGCD))
                {
                    return MageMasher;
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
                    && (GetCooldownRemainingTime(Predict) <= 8 || ActionReady(Predict)
                    || (ActionReady(PhantomRejuvenation) && GetOptionValue(Config.Occult_Prediction) != 4)))
                {
                    return PhantomRejuvenation;
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
            if (IsEnabled(Presets.Occult_Thief) && HasEffect(PhantomJobs.Thief) && SafeToUse() && (IsComboAction(actionID) || IsTankProjectile(actionID)))
            {
                if (IsEnabled(Presets.Occult_Vigilance) && DutyActionReady(Vigilance) && !InCombat() && !HasEffect(Buffs.Vigilance))
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

    internal class Occult_MysticKnight : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.Occult_MysticKnight;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.Occult_MysticKnight) && HasEffect(PhantomJobs.MysticKnight) && InCombat() && SafeToUse() && IsComboAction(actionID))
            {
                if (IsEnabled(Presets.Occult_MagicShell) && DutyActionReady(MagicShell))
                {
                    return MagicShell;
                }

                if (IsEnabled(Presets.Occult_HolySpellblade) && DutyActionReady(HolySpellblade) && InActionRange(HolySpellblade)
                    && TargetEffectRemainingTimeAny(Debuffs.BlazingBane) >= 15 && GCDCheck(5))
                {
                    return HolySpellblade;
                }

                if (IsEnabled(Presets.Occult_BlazingSpellblade) && DutyActionReady(BlazingSpellblade) && InActionRange(BlazingSpellblade) && GCDCheck(5)
                    && (!HasEffect(Buffs.BlazingSpellblade) || !TargetHasEffectAny(Debuffs.BlazingBane) || TargetEffectRemainingTimeAny(Debuffs.BlazingBane) <= 15))
                {
                    return BlazingSpellblade;
                }
            }

            return actionID;
        }
    }

    internal class Occult_Gladiator : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.Occult_Gladiator;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.Occult_Gladiator) && HasEffect(PhantomJobs.Gladiator) && InCombat() && SafeToUse() && IsComboAction(actionID))
            {
                if (IsEnabled(Presets.Occult_Finisher) && DutyActionReady(Finisher) && InActionRange(Finisher) && GCDCheck(5))
                {
                    return Finisher;
                }

                if (IsEnabled(Presets.Occult_LongReach) && DutyActionReady(LongReach) && InActionRange(LongReach) && GCDCheck(5))
                {
                    return LongReach;
                }

                if (IsEnabled(Presets.Occult_Bladeblitz) && DutyActionReady(Bladeblitz) && InActionRange(Bladeblitz) && GCDCheck(5))
                {
                    return Bladeblitz;
                }
            }

            return actionID;
        }
    }

    internal class Occult_Dancer : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.Occult_Dancer;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.Occult_Dancer) && HasEffect(PhantomJobs.Dancer) && InCombat() && SafeToUse() && IsComboAction(actionID))
            {
                if (IsEnabled(Presets.Occult_Dance) && GCDCheck(5))
                {
                    if (HasEffect(Buffs.PoisedToSwordDance))
                    {
                        return SwordDance;
                    }

                    if (HasEffect(Buffs.TemptedToTango))
                    {
                        return TemptingTango;
                    }

                    if (HasEffect(Buffs.Jitterbugged))
                    {
                        return Jitterbug;
                    }

                    if (HasEffect(Buffs.WillingToWaltz))
                    {
                        return MysteryWaltz;
                    }

                    if (DutyActionReady(Dance))
                    {
                        return Dance;
                    }
                }

                if (IsEnabled(Presets.Occult_Quickstep) && DutyActionReady(Quickstep) && !HasEffect(Buffs.Quickstep))
                {
                    return Quickstep;
                }

                if (IsEnabled(Presets.Occult_Mesmerize) && DutyActionReady(Mesmerize))
                {
                    return Mesmerize;
                }
            }

            return actionID;
        }
    }

    internal class Occult_Ninja : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.Occult_Ninja;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.Occult_Ninja) && HasEffect(PhantomJobs.Ninja) && InCombat() && SafeToUse()
                && CanWeave(actionID, ActionWatching.LastGCD) && IsComboAction(actionID))
            {
                if (IsEnabled(Presets.Occult_Smoke) && DutyActionReady(Smoke) && !HasEffect(Buffs.Smoke))
                {
                    return Smoke;
                }

                if (IsEnabled(Presets.Occult_Image) && DutyActionReady(Image))
                {
                    return Image;
                }

                if (IsEnabled(Presets.Occult_Shuriken) && DutyActionReady(Shuriken) && GCDCheck(5))
                {
                    return Shuriken;
                }

                if (IsEnabled(Presets.Occult_LightningScroll) && DutyActionReady(LightningScroll) && GCDCheck(5))
                {
                    return LightningScroll;
                }

                if (IsEnabled(Presets.Occult_FireScroll) && DutyActionReady(FireScroll) && GCDCheck(5))
                {
                    return FireScroll;
                }
            }

            return actionID;
        }
    }

    internal class Occult_WhiteMage : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.Occult_WhiteMage;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.Occult_WhiteMage) && HasEffect(PhantomJobs.WhiteMage) && InCombat() && SafeToUse() && IsComboAction(actionID))
            {
                if (IsEnabled(Presets.Occult_Holy) && DutyActionReady(Holy) && GCDCheck(5) && !IsMoving)
                {
                    return Holy;
                }
            }

            return actionID;
        }
    }

    internal class Occult_BlackMage : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.Occult_BlackMage;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.Occult_BlackMage) && HasEffect(PhantomJobs.BlackMage) && InCombat() && SafeToUse() && IsComboAction(actionID))
            {
                if (IsEnabled(Presets.Occult_Spell3) && DutyActionReady(Fire3) && GCDCheck(5) && !IsMoving)
                {
                    return CurrentElementalWeakness() switch
                    {
                        Debuffs.FireWeakness => Fire3,
                        Debuffs.IceWeakness => Blizzard3,
                        Debuffs.LightningWeakness => Thunder3,
                        _ => GetOptionValue(Config.Occult_SMNSpell) switch
                        {
                            1 => Fire3,
                            2 => Blizzard3,
                            3 => Thunder3,
                            _ => 1
                        }
                    };
                }

                if (IsEnabled(Presets.Occult_Flare) && DutyActionReady(Flare) && GCDCheck(5) && !IsMoving)
                {
                    return Flare;
                }
            }

            return actionID;
        }
    }

    internal class Occult_Dragoon : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.Occult_Dragoon;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.Occult_Dragoon) && HasEffect(PhantomJobs.Dragoon) && InCombat() && SafeToUse() && IsComboAction(actionID))
            {
                if (IsEnabled(Presets.Occult_Jump) && DutyActionReady(Jump) && GCDCheck(5) && !IsMoving)
                {
                    return Jump;
                }

                if (IsEnabled(Presets.Occult_Lance) && DutyActionReady(Lance) && CanWeave(actionID, ActionWatching.LastGCD) && GCDCheck(5))
                {
                    return Lance;
                }
            }

            return actionID;
        }
    }

    internal class Occult_Summoner : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.Occult_Summoner;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.Occult_Summoner) && HasEffect(PhantomJobs.Summoner) && InCombat() && SafeToUse() && IsComboAction(actionID))
            {
                if (IsEnabled(Presets.Occult_SMNSpell) && DutyActionReady(Hellfire) && GCDCheck(3) && !IsMoving)
                {
                    return CurrentElementalWeakness() switch
                    {
                        Debuffs.FireWeakness => Hellfire,
                        Debuffs.LightningWeakness => JudgmentBolt,
                        Debuffs.WindWeakness => Thunderstorm,
                        _ => GetOptionValue(Config.Occult_SMNSpell) switch
                        {
                            1 => Hellfire,
                            2 => JudgmentBolt,
                            3 => Thunderstorm,
                            _ => 1
                        }
                    };
                }

                if (IsEnabled(Presets.Occult_Megaflare) && DutyActionReady(Megaflare) && GCDCheck(3) && !IsMoving)
                {
                    return Megaflare;
                }
            }

            return actionID;
        }
    }

    internal class Occult_BlueMage : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.Occult_BlueMage;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.Occult_BlueMage) && HasEffect(PhantomJobs.BlueMage) && InCombat() && SafeToUse() && IsComboAction(actionID))
            {
                if (IsEnabled(Presets.Occult_Aero) && DutyActionReady(Aero) && GCDCheck(5) && !IsMoving)
                {
                    return Aero;
                }

                if (IsEnabled(Presets.Occult_Aero) && DutyActionReady(Aero2) && GCDCheck(5) && !IsMoving)
                {
                    return Aero2;
                }

                if (IsEnabled(Presets.Occult_Aero) && DutyActionReady(Aero3) && GCDCheck(5) && !IsMoving)
                {
                    return Aero3;
                }

                if (IsEnabled(Presets.Occult_AquaBreath) && DutyActionReady(AquaBreath) && GCDCheck(5) && !IsMoving)
                {
                    return AquaBreath;
                }
            }

            return actionID;
        }
    }

    internal class Occult_RedMage : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.Occult_RedMage;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.Occult_RedMage) && HasEffect(PhantomJobs.RedMage) && InCombat() && SafeToUse() && IsComboAction(actionID))
            {
                if (IsEnabled(Presets.Occult_Libra) && DutyActionReady(Libra) && CanWeave(actionID, ActionWatching.LastGCD)
                    && !TargetHasEffectAny(Debuffs.ElementalWeaknesses))
                {
                    return Libra;
                }

                if (IsEnabled(Presets.Occult_Spell2) && DutyActionReady(Fire2) && GCDCheck(5) && !IsMoving)
                {
                    return CurrentElementalWeakness() switch
                    {
                        Debuffs.FireWeakness => Fire2,
                        Debuffs.IceWeakness => Blizzard2,
                        Debuffs.LightningWeakness => Thunder2,
                        _ => GetOptionValue(Config.Occult_Spell2) switch
                        {
                            1 => Fire2,
                            2 => Blizzard2,
                            3 => Thunder2,
                            _ => 1
                        }
                    };
                }
            }

            return actionID;
        }
    }

    internal class Occult_Necromancer : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.Occult_Necromancer;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.Occult_Necromancer) && HasEffect(PhantomJobs.Necromancer) && InCombat() && SafeToUse() && IsComboAction(actionID))
            {
                if (IsEnabled(Presets.Occult_DrainTouch) && DutyActionReady(DrainTouch) && CanWeave(actionID, ActionWatching.LastGCD) && GCDCheck(5))
                {
                    return DrainTouch;
                }

                if (IsEnabled(Presets.Occult_Doomsday) && DutyActionReady(Doomsday) && HasEffect(Buffs.DrainTouch) && GCDCheck(5) && !IsMoving)
                {
                    return Doomsday;
                }

                if (IsEnabled(Presets.Occult_NECSpell) && DutyActionReady(DeepFreeze) && HasEffect(Buffs.DrainTouch) && GCDCheck(5) && !IsMoving)
                {
                    return CurrentElementalWeakness() switch
                    {
                        Debuffs.IceWeakness => DeepFreeze,
                        Debuffs.LightningWeakness => ChaosDrive,
                        Debuffs.WindWeakness => HellWind,
                        _ => GetOptionValue(Config.Occult_Spell2) switch
                        {
                            1 => DeepFreeze,
                            2 => ChaosDrive,
                            3 => HellWind,
                            _ => 1
                        }
                    };
                }
            }

            return actionID;
        }
    }
}
