using UltimateCombo.Combos.General;
using UltimateCombo.Core;
using UltimateCombo.Data;

namespace UltimateCombo.Combos.PvP;

internal static class MNKPvP
{
    internal const uint
        DragonKick = 29475,
        TwinSnakes = 29476,
        Demolish = 29477,
        LeapingOpo = 41444,
        RisingRaptor = 41445,
        PouncingCoeurl = 41446,
        PhantomRush = 29478,

        RisingPhoenix = 29481,

        FiresReply = 41448,
        WindsReply = 41509,
        RiddleOfEarth = 29482,
        EarthsReply = 29483,

        Thunderclap = 29484,

        Meteodrive = 29485;

    internal static class Buffs
    {
        internal const ushort
            WindResonance = 2007,
            FiresRumination = 4301,
            FireResonance = 3170,
            EarthResonance = 3171,
            Thunderclap = 3173;
    }

    internal static class Debuffs
    {
        internal const ushort
            PressurePoint = 3172;
    }

    internal class MNKPvP_Combo : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.MNKPvP_Combo;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is DragonKick or TwinSnakes or Demolish or LeapingOpo or RisingRaptor or PouncingCoeurl or PhantomRush)
                && IsEnabled(Presets.MNKPvP_Combo))
            {
                if (IsEnabled(Presets.MNKPvP_Meteodrive) && GetLimitBreakCurrentValue() == GetLimitBreakMaxValue()
                    && (TargetHasEffectAny(AllPvP.Buffs.Guard) || (TargetHasEffectAny(Debuffs.PressurePoint) && WasLastWeaponskill(FiresReply))))
                {
                    return Meteodrive;
                }

                if (IsEnabled(Presets.MNKPvP_RiddleOfEarth) && IsActionEnabled(EarthsReply) && EffectRemainingTime(Buffs.EarthResonance) < 1)
                {
                    return EarthsReply;
                }

                if (CanWeave(actionID, ActionWatching.LastGCD))
                {
                    if (IsEnabled(Presets.MNKPvP_RiddleOfEarth) && ActionReady(RiddleOfEarth))
                    {
                        return RiddleOfEarth;
                    }
                }

                if (!TargetHasEffectAny(AllPvP.Buffs.Guard))
                {
                    if (CanWeave(actionID, ActionWatching.LastGCD))
                    {
                        if (IsEnabled(Presets.MNKPvP_RisingPhoenix) && ActionReady(RisingPhoenix) && !WasLastAction(RisingPhoenix)
                            && (WasLastWeaponskill(WindsReply) || WasLastWeaponskill(PouncingCoeurl)))
                        {
                            return RisingPhoenix;
                        }
                    }

                    if (IsEnabled(Presets.MNKPvP_WindsReply) && ActionReady(WindsReply) && WasLastWeaponskill(PhantomRush))
                    {
                        return WindsReply;
                    }

                    if (IsEnabled(Presets.MNKPvP_FiresReply) && (WasLastWeaponskill(WindsReply) || (!InActionRange(DragonKick) && GetRemainingCharges(FiresReply) == 2)))
                    {
                        return FiresReply;
                    }
                }
            }

            return actionID;
        }
    }
}
