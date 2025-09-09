using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Combos.General;
using UltimateCombo.Core;

namespace UltimateCombo.Combos.PvP;

internal static class ASTPvP
{
    internal const uint
        FallMalefic = 29242,
        AspectedBenefic = 29243,
        Gravity2 = 29244,
        DoubleCast = 29245,

        MinorArcana = 41503,
        LadyOfCrowns = 41504,
        LordOfCrowns = 41505,

        Macrocosmos = 29253,
        Microcosmos = 29254,

        Epicycle = 41506,
        Retrograde = 41507,

        CelestialRiver = 29255,
        Oracle = 41508;

    internal static class Buffs
    {
        internal const ushort
            DiurnalBenefic = 3099,
            NocturnalBenefic = 3100,

            LadyOfCrownsCard = 4328,
            LadyOfCrownsBuff = 1452,
            LordOfCrownsCard = 4329,

            Macrocosmos = 3104,

            RetrogradeReady = 4331,
            Epicycle = 4330,

            CelestialRiver = 3105,
            Divining = 4332;
    }

    internal class Debuffs
    {
        internal const ushort
            LordOfCrownsDebuff = 1451,
            CelestialTide = 3106;
    }

    internal static class Config
    {
        internal static UserInt
            ASTPvP_AspectedBenefic = new("ASTPvP_AspectedBenefic", 75);
    }

    internal class ASTPvP_Combo : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.ASTPvP_Combo;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (actionID is FallMalefic && IsEnabled(Presets.ASTPvP_Combo))
            {
                if (!TargetHasEffectAny(AllPvP.Buffs.Guard))
                {
                    if (CanWeave(actionID))
                    {
                        if (IsEnabled(Presets.ASTPvP_MinorArcana) && ActionReady(MinorArcana))
                        {
                            return MinorArcana;
                        }
                    }

                    if (IsEnabled(Presets.ASTPvP_AspectedBenefic) && ActionReady(AspectedBenefic)
                        && PlayerHealthPercentageHp() <= GetOptionValue(Config.ASTPvP_AspectedBenefic))
                    {
                        return AspectedBenefic;
                    }
                }
            }

            return actionID;
        }
    }
}
