using UltimateCombo.Combos.General;
using UltimateCombo.Core;

namespace UltimateCombo.Combos.PvP;

internal static class BLMPvP
{
    internal const uint
        Fire = 29649,
        Fire3 = 30896,
        Fire4 = 29650,
        HighFire2 = 41473,
        Flare = 29651,

        Blizzard = 29653,
        Blizzard3 = 30897,
        Blizzard4 = 29654,
        HighBlizzard2 = 41474,
        Freeze = 29655,

        Paradox = 29663,
        Burst = 29657,
        Xenoglossy = 29658,
        Lethargy = 41510,
        AetherialManipulation = 29660,

        ElementalWave = 41475,
        WreathOfFire = 41476,
        WreathOfIce = 41478,

        SoulResonance = 29662,
        FlareStar = 41480,
        FrostStar = 41481;

    internal static class Buffs
    {
        internal const ushort
            Paradox = 3223,

            AstralFire1 = 3212,
            AstralFire2 = 3213,
            AstralFire3 = 3381,

            UmbralIce1 = 3214,
            UmbralIce2 = 3215,
            UmbralIce3 = 3382,

            Burst = 3221,

            WreathOfIce = 4316,
            WreathOfFice = 4315,

            SoulResonance = 3222,
            ElementalStar = 4317;
    }

    internal class Debuffs
    {
        internal const ushort
            Lethargy = 4333;
    }

    internal class BLMPvP_Combo : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.BLMPvP_Combo;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is Fire or Fire3 or Fire4 or HighFire2 or Flare or Blizzard or Blizzard3 or Blizzard4 or HighBlizzard2 or Freeze)
                && IsEnabled(Presets.BLMPvP_Combo))
            {
                if (IsEnabled(Presets.BLMPvP_SoulResonance) && GetLimitBreakCurrentValue() == GetLimitBreakMaxValue()
                    && PlayerHealthPercentageHp() > 25)
                {
                    return SoulResonance;
                }

                if (!TargetHasEffectAny(AllPvP.Buffs.Guard))
                {
                    if (CanWeave(actionID))
                    {
                        if (IsEnabled(Presets.BLMPvP_Lethargy) && ActionReady(Lethargy))
                        {
                            return Lethargy;
                        }
                    }

                    if (IsEnabled(Presets.BLMPvP_Burst) && ActionReady(Burst) && (InActionRange(Burst) || WasLastAbility(AetherialManipulation)))
                    {
                        return Burst;
                    }

                    if (IsEnabled(Presets.BLMPvP_Xenoglossy) && ActionReady(Xenoglossy)
                        && (GetRemainingCharges(Xenoglossy) == GetMaxCharges(Xenoglossy) || PlayerHealthPercentageHp() <= 50))
                    {
                        return Xenoglossy;
                    }

                    if (IsEnabled(Presets.BLMPvP_Paradox) && HasEffect(Buffs.Paradox) && !HasEffect(Buffs.SoulResonance))
                    {
                        return Paradox;
                    }
                }

                if (IsMoving || (HasEffect(Buffs.UmbralIce3) && !HasEffect(Buffs.SoulResonance)))
                {
                    return OriginalHook(Blizzard);
                }
            }

            return actionID;
        }
    }
}
