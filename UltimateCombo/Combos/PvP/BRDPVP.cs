using UltimateCombo.Combos.General;
using UltimateCombo.Core;

namespace UltimateCombo.Combos.PvP;

internal static class BRDPvP
{
    internal const uint
        PowerfulShot = 29391,
        SilentNocturne = 29395,
        RepellingShot = 29399,
        PitchPerfect = 29392,

        ApexArrow = 29393,
        BlastArrow = 29394,

        HarmonicArrow = 41964,

        TheWardensPaean = 29400,

        FinalFantasia = 29401,
        EncoreOfLight = 41467;

    internal static class Buffs
    {
        internal const ushort
            FrontlinersMarch = 3138,
            FrontlineMarch = 3139,
            BlastArrowReady = 3142,

            Repertoire = 3137,
            TheWardensPaean = 3143,

            EncoreOfLightReady = 4312,
            FinalFantasia = 3144,
            HeroesFantasia = 3145;
    }

    internal class BRDPvP_Combo : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.BRDPvP_Combo;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is PowerfulShot or PitchPerfect) && IsEnabled(Presets.BRDPvP_Combo))
            {
                if (IsEnabled(Presets.BRDPvP_FinalFantasia) && GetLimitBreakCurrentValue() == GetLimitBreakMaxValue()
                    && !IsMoving)
                {
                    return FinalFantasia;
                }

                if (!TargetHasEffectAny(AllPvP.Buffs.Guard))
                {
                    if (CanWeave(actionID))
                    {
                        if (IsEnabled(Presets.BRDPvP_SilentNocturne) && ActionReady(SilentNocturne) && !HasEffect(Buffs.Repertoire))
                        {
                            return SilentNocturne;
                        }

                        if (IsEnabled(Presets.BRDPvP_RepellingShot) && ActionReady(RepellingShot) && InActionRange(RepellingShot)
                            && !HasEffect(Buffs.Repertoire))
                        {
                            return RepellingShot;
                        }
                    }

                    if (IsEnabled(Presets.BRDPvP_HarmonicArrow) && ActionReady(HarmonicArrow)
                        && GetRemainingCharges(HarmonicArrow) == GetMaxCharges(HarmonicArrow))
                    {
                        return HarmonicArrow;
                    }

                    if (IsEnabled(Presets.BRDPvP_ApexArrow) && ActionReady(ApexArrow))
                    {
                        return ApexArrow;
                    }

                    if (IsEnabled(Presets.BRDPvP_BlastArrow) && IsActionEnabled(BlastArrow))
                    {
                        return BlastArrow;
                    }

                    if (IsEnabled(Presets.BRDPvP_EncoreOfLight) && HasEffect(Buffs.EncoreOfLightReady))
                    {
                        return EncoreOfLight;
                    }
                }
            }

            if (actionID is SilentNocturne && IsEnabled(Presets.BRDPvP_SafeNocturne))
            {
                if (!TargetHasEffectAny(AllPvP.Buffs.Guard))
                {
                    return actionID;
                }

                return OriginalHook(11);
            }

            return actionID;
        }
    }
}
