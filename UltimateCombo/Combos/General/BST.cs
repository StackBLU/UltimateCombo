using ECommons.DalamudServices;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using Lumina.Excel.Sheets.Experimental;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Core;
using Buddy = FFXIVClientStructs.FFXIV.Client.Game.UI.Buddy;

#pragma warning disable PendingExcelSchema

namespace UltimateCombo.Combos.General;

internal class BST
{
    internal const byte JobID = 43;

    internal enum InstinctualAffinity : byte
    {
        None = 0,
        Volant = 1,
        Rampant = 2,
        Durant = 3,
        Eldritch = 4,
        Sunstrider = 5,
        Moonstalker = 6,
        WaveringHeart = 7,
    }

    internal enum BeastmasterKinType : byte
    {
        None = 0,
        Beastkin = 1,
        Vilekin = 2,
        Cloudkin = 3,
        Seedkin = 4,
        Wavekin = 5,
        Scalekin = 6,
        Soulkin = 7,
        Ashkin = 8,
    }

    internal enum TrickAffinity
    {
        None,
        Volant,
        Rampant,
        Durant,
        Eldritch,
    }

    [StructLayout(LayoutKind.Explicit, Size = 0x11)]
    internal struct TmpBSTGauge
    {
        [FieldOffset(0x08)] internal byte MasterTP;
        [FieldOffset(0x09)] internal byte PetTP;
        [FieldOffset(0x0B)] internal byte BattleHorn;

        [FieldOffset(0x0C)] internal InstinctualAffinity AffinityStatus;
        [FieldOffset(0x0D)] internal InstinctualAffinity ActiveAffinity;
        [FieldOffset(0x0E)] internal byte Chain;
        [FieldOffset(0x0F)] internal byte Kinship;
        [FieldOffset(0x10)] internal byte Instinct;

        internal BeastmasterKinType KinshipKinType => (BeastmasterKinType) (Kinship >> 4);

        internal byte KinshipBattlehorn => (byte) (Kinship & 0x0F);

        internal int PetInstinct => Instinct & 0x3;

        internal int MasterInstinct => (Instinct & 0xC) >> 2;
    }

    internal static unsafe TmpBSTGauge* GaugePtr => (TmpBSTGauge*) ((nint) JobGaugeManager.StaticAddressPointers.pInstance + 0x08);

    internal static unsafe TmpBSTGauge Gauge => *GaugePtr;

    internal const uint
        SmashAxe = 44879,
        AxebladeBite = 44883,
        Shieldsplitter = 44885,
        Trick = 47093,

        FirstBattlehorn = 44881,
        AvalancheAxe = 44884,
        BeastMode = 44886,
        MistralAxe = 44887,
        SpinningAxe = 44888,
        GaleAxe = 44889,
        TemperedRelease = 44890,
        PartingBlow = 44891,
        SecondBattlehorn = 44892,
        ShieldCharge = 44893,
        ThirdBattlehorn = 44894,
        Borrow = 44895,
        Beastskin = 44896,
        Vileskin = 44897,
        CloudSkim = 44898,
        Seedsower = 44899,
        QuellingWave = 44900,
        Scaleskin = 44901,
        SoulCrush = 44902,
        ScouringAsh = 44903,
        RallyingCheer = 44904,
        Rally = 44905;

    internal static class Buffs
    {
        internal const ushort
            Volant = 4595,
            Rampant = 4596,
            Durant = 4597,
            Eldritch = 4598,
            Sunstrider = 4599,
            Moonstalker = 4600,
            OneWithNature = 4601,

            Beast = 4644,
            Vile = 4645,
            Cloud = 4646,
            Seed = 4647,
            Wave = 4648,
            Scale = 4649,
            Soul = 4650,
            Ash = 4651,

            LingeringVantage = 4614,
            WaveringHeart = 4643,

            Vileskin = 4620,
            Beastskin = 4621,
            SeedsSown = 4622,
            Scaleskin = 4623;
    }

    internal static class Debuffs
    {
        internal const ushort
            PeatPelt = 3636;
    }

    internal static class Config
    {
        internal static UserInt
            BLU_TreasureRehydration = new("BLU_TreasureRehydration", 30);

        internal static UserBool
            BLU_WingedReprobation = new("BLU_WingedReprobation");
    }

    internal static class TrickActions
    {
        internal const uint
            Cusith_Rake = 44935,
            Squirrel_SomersaultSlash = 44937,
            Lamb_FleeceButt = 44939,
            Pugil_Screwdriver = 44941,
            Opoopo_StoneThrow = 44943,
            Dodo_FowlStench = 44945,
            Coblyn_BestialThunder = 44947,
            Diremite_DeadlyThrust = 44949,
            Megalocrab_DrenchingBlow = 44951,
            Wespe_SharpSting = 44953,
            Vulture_WingCutter = 44955,
            Mandragora_Budbutt = 44957,
            Geshunpest_DarkThunder = 44959,
            Puk_Fireball = 44961,
            Crab_BubbleShower = 44963,
            Mantis_StandingChine = 44965,
            Slime_Digest = 44967,
            Dullahan_IronJustice = 44969,
            Bat_BloodDrain = 44971,
            Flyingtrap_SourSough = 44973,
            Ziz_IceBreath = 44975,
            Sabotender_NaturalNeedles = 44977,
            Golem_BoulderClap = 44979,
            Apkallu_FlyingSardine = 44981,
            Adamantoise_BestialThunderII = 44983,
            Buffalo_Heave = 44985,
            Uragnite_FrostBreath = 44987,
            Worm_SandBreath = 44989,
            Spriggan_Romp = 44991,
            Goobbue_Beatdown = 44993,
            Gigantoad_BestialBlizzardII = 44995,
            Colibri_PeckingFlurry = 44997,
            Coeurl_Blaster = 44999,
            Raptor_FrostBreath = 45001,
            Drake_BurningCyclone = 45003,
            Treant_AcornBomb = 45005,
            Antling_MandibleBite = 45007,
            Chimera_theLionsBreath = 45009,
            Morbol_VineProbe = 45011,
            Ghost_FellGale = 45013,
            Salamander_BrackishRain = 45015,
            Cobra_DrippingFang = 45017,
            Hydra_MainTrap = 45019,
            Damselfly_CursedSphere = 45021,
            Rottinggoobbue_DirtySneeze = 45023,
            Zu_FlyingFrenzy = 45025,
            Icegolem_IceGuillotine = 45027,
            Karlabos_Impale = 45029,
            Rafflesia_BloodyCaress = 45031,
            Behemoth_Thunderbolt = 45033;
    }

    private static readonly List<uint> RampantTricks =
    [
        TrickActions.Cusith_Rake,
        TrickActions.Squirrel_SomersaultSlash,
        TrickActions.Lamb_FleeceButt,
        TrickActions.Opoopo_StoneThrow,
        TrickActions.Diremite_DeadlyThrust,
        TrickActions.Mandragora_Budbutt,
        TrickActions.Puk_Fireball,
        TrickActions.Sabotender_NaturalNeedles,
        TrickActions.Buffalo_Heave,
        TrickActions.Spriggan_Romp,
        TrickActions.Goobbue_Beatdown,
        TrickActions.Drake_BurningCyclone,
        TrickActions.Antling_MandibleBite,
        TrickActions.Chimera_theLionsBreath,
        TrickActions.Morbol_VineProbe,
    ];

    private static readonly List<uint> EldritchTricks =
    [
        TrickActions.Dodo_FowlStench,
        TrickActions.Coblyn_BestialThunder,
        TrickActions.Geshunpest_DarkThunder,
        TrickActions.Slime_Digest,
        TrickActions.Golem_BoulderClap,
        TrickActions.Adamantoise_BestialThunderII,
        TrickActions.Worm_SandBreath,
        TrickActions.Gigantoad_BestialBlizzardII,
        TrickActions.Coeurl_Blaster,
        TrickActions.Treant_AcornBomb,
        TrickActions.Rottinggoobbue_DirtySneeze,
        TrickActions.Rafflesia_BloodyCaress,
        TrickActions.Behemoth_Thunderbolt,
    ];

    private static readonly List<uint> DurantTricks =
    [
        TrickActions.Pugil_Screwdriver,
        TrickActions.Megalocrab_DrenchingBlow,
        TrickActions.Crab_BubbleShower,
        TrickActions.Mantis_StandingChine,
        TrickActions.Dullahan_IronJustice,
        TrickActions.Ziz_IceBreath,
        TrickActions.Apkallu_FlyingSardine,
        TrickActions.Uragnite_FrostBreath,
        TrickActions.Raptor_FrostBreath,
        TrickActions.Salamander_BrackishRain,
        TrickActions.Cobra_DrippingFang,
        TrickActions.Hydra_MainTrap,
        TrickActions.Icegolem_IceGuillotine,
        TrickActions.Karlabos_Impale,
    ];

    private static readonly List<uint> VolantTricks =
    [
        TrickActions.Wespe_SharpSting,
        TrickActions.Vulture_WingCutter,
        TrickActions.Bat_BloodDrain,
        TrickActions.Flyingtrap_SourSough,
        TrickActions.Colibri_PeckingFlurry,
        TrickActions.Ghost_FellGale,
        TrickActions.Damselfly_CursedSphere,
        TrickActions.Zu_FlyingFrenzy,
    ];

    internal static unsafe Buddy.BuddyMember? CurrentPet => *UIState.Instance()->Buddy.PetInfo.Pet;

    internal static bool CurrentPetIsBMPet => CurrentPet?.DataId > 0 && Svc.Data.GetExcelSheet<XBMPet>().Any(x => x.Pet.RowId == CurrentPet?.DataId);

    internal static Pet? CurrentPetSheet => CurrentPetIsBMPet ? Svc.Data.GetExcelSheet<Pet>().GetRow(CurrentPet?.DataId ?? 0) : null;

    internal static uint CurrentPetTrickAction => CurrentPetSheet?.Abilities[0].RowId ?? 0;

    private static bool TrickIsDurant => DurantTricks.Any(x => x == CurrentPetTrickAction);

    private static bool TrickIsEldritch => EldritchTricks.Any(x => x == CurrentPetTrickAction);

    private static bool TrickIsVolant => VolantTricks.Any(x => x == CurrentPetTrickAction);

    private static bool TrickIsRampant => RampantTricks.Any(x => x == CurrentPetTrickAction);

    internal enum TrickTypes
    {
        None,
        Durant,
        Eldritch,
        Volant,
        Rampant,
    }

    internal static TrickTypes TrickType =>
        TrickIsDurant ? TrickTypes.Durant :
        TrickIsEldritch ? TrickTypes.Eldritch :
        TrickIsVolant ? TrickTypes.Volant :
        TrickIsRampant ? TrickTypes.Rampant :
        TrickTypes.None;

    //internal static BSTGauge Gauge => CustomComboFunctions.GetJobGauge<BSTGauge>();

    internal class BST_Basic : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.BST_DPS;

        protected override uint Invoke(uint actionID)
        {
            if ((actionID is SmashAxe or AxebladeBite or Shieldsplitter) && IsEnabled(Presets.BST_DPS))
            {
                if (CanWeave(actionID, SmashAxe))
                {
                    if (IsEnabled(Presets.BST_Rally) && ActionReady(Rally) && Gauge.MasterInstinct == 3 && Gauge.MasterTP < 100 && Gauge.PetInstinct == 1
                        && (HasEffect(Buffs.Sunstrider) || HasEffect(Buffs.Moonstalker))
                        && (EffectRemainingTime(Buffs.Sunstrider) > 2 || EffectRemainingTime(Buffs.Moonstalker) > 2))
                    {
                        return Rally;
                    }

                    if (IsEnabled(Presets.BST_Cheer) && ActionReady(RallyingCheer) && Gauge.PetInstinct == 3 && Gauge.PetTP < 100 && Gauge.MasterInstinct == 2
                        && (HasEffect(Buffs.Rampant) || HasEffect(Buffs.Durant) || HasEffect(Buffs.Eldritch) || HasEffect(Buffs.Volant)))
                    {
                        return RallyingCheer;
                    }

                    if (IsEnabled(Presets.BST_Combo) && Gauge.BattleHorn != 0 && !WasLastAction(PartingBlow))
                    {
                        //Infinitive
                        if (HasEffect(Buffs.Sunstrider) && Gauge.MasterTP == 250)
                        {
                            return OriginalHook(GaleAxe);
                        }

                        if (HasEffect(Buffs.Moonstalker) && Gauge.MasterTP == 250)
                        {
                            return OriginalHook(SpinningAxe);
                        }

                        //Intentional
                        if (Gauge.MasterTP >= 100 && HasEffect(Buffs.Rampant) && (Gauge.MasterInstinct != 2 || Gauge.PetInstinct != 0))
                        {
                            return OriginalHook(MistralAxe);
                        }

                        if (Gauge.MasterTP >= 100 && HasEffect(Buffs.Durant) && (Gauge.MasterInstinct != 2 || Gauge.PetInstinct != 0))
                        {
                            return OriginalHook(SpinningAxe);
                        }

                        if (Gauge.MasterTP >= 100 && HasEffect(Buffs.Eldritch) && (Gauge.MasterInstinct != 2 || Gauge.PetInstinct != 0))
                        {
                            return OriginalHook(GaleAxe);
                        }

                        if (Gauge.MasterTP >= 100 && HasEffect(Buffs.Volant) && (Gauge.MasterInstinct != 2 || Gauge.PetInstinct != 0))
                        {
                            return OriginalHook(AvalancheAxe);
                        }

                        if (Gauge.PetTP >= 100 && (HasEffect(Buffs.Rampant) || HasEffect(Buffs.Durant) || HasEffect(Buffs.Eldritch) || HasEffect(Buffs.Volant)))
                        {
                            return OriginalHook(Trick);
                        }

                        //Starter
                        if (Gauge.MasterTP >= 100 && Gauge.PetTP >= 100 && Gauge.MasterInstinct >= Gauge.PetInstinct && LevelChecked(RallyingCheer))
                        {
                            if (TrickType is TrickTypes.Rampant)
                            {
                                return OriginalHook(GaleAxe);
                            }

                            if (TrickType is TrickTypes.Durant)
                            {
                                return OriginalHook(AvalancheAxe);
                            }

                            if (TrickType is TrickTypes.Eldritch)
                            {
                                return OriginalHook(MistralAxe);
                            }

                            if (TrickType is TrickTypes.Volant)
                            {
                                return OriginalHook(SpinningAxe);
                            }
                        }

                        if (Gauge.MasterTP >= 100 && Gauge.PetTP >= 100 && (Gauge.PetInstinct >= Gauge.MasterInstinct || !LevelChecked(RallyingCheer)))
                        {
                            return OriginalHook(Trick);
                        }
                    }
                }

                if (ComboAction is SmashAxe && ActionReady(AxebladeBite))
                {
                    return AxebladeBite;
                }

                if (ComboAction is AxebladeBite && ActionReady(Shieldsplitter))
                {
                    return Shieldsplitter;
                }

                return SmashAxe;
            }

            return actionID;
        }
    }

    internal class BST_Horn : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.BST_Horn;

        protected override uint Invoke(uint actionID)
        {
            if (IsEnabled(Presets.BST_Horn))
            {
                if (actionID is FirstBattlehorn)
                {
                    if (HasEffect(Buffs.OneWithNature) && Gauge.BattleHorn != 0)
                    {
                        return OriginalHook(TemperedRelease);
                    }

                    if (Gauge.BattleHorn == 1)
                    {
                        return PartingBlow;
                    }
                }

                if (actionID is SecondBattlehorn)
                {
                    if (HasEffect(Buffs.OneWithNature) && Gauge.BattleHorn != 0)
                    {
                        return OriginalHook(TemperedRelease);
                    }

                    if (Gauge.BattleHorn == 2)
                    {
                        return PartingBlow;
                    }
                }

                if (actionID is ThirdBattlehorn)
                {
                    if (HasEffect(Buffs.OneWithNature) && Gauge.BattleHorn != 0)
                    {
                        return OriginalHook(TemperedRelease);
                    }

                    if (Gauge.BattleHorn == 3)
                    {
                        return PartingBlow;
                    }
                }
            }

            return actionID;
        }
    }
}

#pragma warning restore PendingExcelSchema
