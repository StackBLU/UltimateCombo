using UltimateCombo.Combos.General;
using UltimateCombo.Core;

namespace UltimateCombo.Combos.PvP;

internal static class GNBPvP
{
    internal const uint
        KeenEdge = 29098,
        BrutalShell = 29099,
        SolidBarrel = 29100,
        BurstStrike = 29101,
        Hypervelocity = 29107,

        Continuation = 29106,

        GnashingFang = 29102,
        JugularRip = 29108,
        SavageClaw = 29103,
        AbdomenTear = 29109,
        WickedTalon = 29104,
        EyeGouge = 29110,

        FatedCircle = 41511,
        FatedBrand = 41442,

        RoughDivide = 29123,
        BlastingZone = 29128,
        HeartOfCorundum = 41443,

        RelentlessRush = 29130,
        TerminalTrigger = 29469;

    internal static class Buffs
    {
        internal const ushort
            NoMercy = 3042,

            ReadyToBlast = 3041,
            Hypervelocity = 3047,

            ReadyToRip = 2002,
            JugularRip = 3048,

            ReadyToTear = 2003,
            AbdomenTear = 3049,

            ReadyToGouge = 2004,
            EyeGouge = 3050,

            HeartOfCorundum = 4295,
            CatharsisOfCorundum = 4296,

            ReadyToRaze = 4293,
            Nebula = 3051,
            FatedBrand = 4294,

            RelentlessRush = 3052;
    }

    internal static class Debuffs
    {
        internal const ushort
            RelentlessShrapnel = 3053;
    }

    internal class GNBPvP_Combo : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.GNBPvP_Combo;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is KeenEdge or BrutalShell or SolidBarrel or BurstStrike) && IsEnabled(Presets.GNBPvP_Combo))
            {
                if (IsEnabled(Presets.GNBPvP_RelentlessRush) && GetLimitBreakCurrentValue() == GetLimitBreakMaxValue()
                    && InActionRange(RelentlessRush))
                {
                    return RelentlessRush;
                }

                if (!TargetHasEffectAny(AllPvP.Buffs.Guard))
                {
                    if (CanWeave(actionID))
                    {
                        if (IsEnabled(Presets.GNBPvP_Continuation)
                            && (HasEffect(Buffs.ReadyToBlast) || HasEffect(Buffs.ReadyToGouge)
                            || HasEffect(Buffs.ReadyToRaze) || HasEffect(Buffs.ReadyToRip) || HasEffect(Buffs.ReadyToTear)))
                        {
                            return OriginalHook(Continuation);
                        }

                        if (IsEnabled(Presets.GNBPvP_HeartOfCorundum) && ActionReady(HeartOfCorundum))
                        {
                            return HeartOfCorundum;
                        }

                        if (IsEnabled(Presets.GNBPvP_BlastingZone) && ActionReady(BlastingZone)
                            && EnemyPercentHP() <= 50 && HasTarget())
                        {
                            return BlastingZone;
                        }
                    }

                    if (IsEnabled(Presets.GNBPvP_RoughDivide) && ActionReady(RoughDivide) && !HasEffect(Buffs.NoMercy))
                    {
                        return RoughDivide;
                    }

                    if (IsEnabled(Presets.GNBPvP_FatedCircle) && ActionReady(FatedCircle) && InActionRange(FatedCircle))
                    {
                        return FatedCircle;
                    }

                    if (IsEnabled(Presets.GNBPvP_GnashingCombo) && ActionReady(OriginalHook(GnashingFang)))
                    {
                        return OriginalHook(GnashingFang);
                    }
                }
            }

            return actionID;
        }
    }
}
