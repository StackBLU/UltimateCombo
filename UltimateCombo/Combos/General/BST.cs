using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Core;

namespace UltimateCombo.Combos.General;

internal class BST
{
    internal const byte JobID = 43;

    internal const uint
        SmashAxe = 44879,
        AxebladeBite = 44883,
        Shieldsplitter = 44885,

        Capture = 44880,
        FirstBattlehorn = 44881,
        Gauge = 44882,
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
            MoonFlute = 1718;
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

    internal class BST_ST_DPS : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.BST_ST_DPS;

        protected override uint Invoke(uint actionID)
        {
            if ((actionID is SmashAxe or AxebladeBite or Shieldsplitter) && IsEnabled(Presets.BST_ST_DPS))
            {
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
}
