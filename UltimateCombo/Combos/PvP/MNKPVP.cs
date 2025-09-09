using UltimateCombo.Combos.General;
using UltimateCombo.Core;

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

        FlintsReply = 41447,
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
                if (IsEnabled(Presets.MNKPvP_Meteodrive) && GetLimitBreakCurrentValue() == GetLimitBreakMaxValue() && HasEffectAny(AllPvP.Buffs.Guard))
                {
                    return Meteodrive;
                }

                if (CanWeave(actionID))
                {
                    if (IsEnabled(Presets.MNKPvP_RiddleOfEarth) && ActionReady(RiddleOfEarth))
                    {
                        return RiddleOfEarth;
                    }
                }

                if (!TargetHasEffectAny(AllPvP.Buffs.Guard))
                {
                    if (CanWeave(actionID))
                    {
                        if (IsEnabled(Presets.MNKPvP_RisingPhoenix) && ActionReady(RisingPhoenix)
                            && (WasLastWeaponskill(WindsReply) || WasLastWeaponskill(PouncingCoeurl))
                            && !WasLastAction(RisingPhoenix))
                        {
                            return RisingPhoenix;
                        }
                    }

                    if (IsEnabled(Presets.MNKPvP_Thunderclap) && ActionReady(Thunderclap)
                        && !InActionRange(DragonKick) && (HasEffect(Buffs.FireResonance) || WasLastWeaponskill(WindsReply)))
                    {
                        return Thunderclap;
                    }

                    if (!WasLastAbility(RisingPhoenix))
                    {
                        if (IsEnabled(Presets.MNKPvP_RiddleOfEarth) && IsActionEnabled(EarthsReply) && EffectRemainingTime(Buffs.EarthResonance) < 2)
                        {
                            return EarthsReply;
                        }

                        if (IsEnabled(Presets.MNKPvP_FlintsReply) && HasEffect(Buffs.FiresRumination) && !WasLastWeaponskill(WindsReply))
                        {
                            return FiresReply;
                        }

                        if (IsEnabled(Presets.MNKPvP_FlintsReply) && ActionReady(FlintsReply) && !InActionRange(DragonKick))
                        {
                            return FlintsReply;
                        }
                    }

                    if (IsEnabled(Presets.MNKPvP_WindsReply) && ActionReady(WindsReply) && WasLastWeaponskill(PouncingCoeurl))
                    {
                        return WindsReply;
                    }
                }
            }

            return actionID;
        }
    }
}
