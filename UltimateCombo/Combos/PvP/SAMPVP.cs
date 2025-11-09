using UltimateCombo.Combos.General;
using UltimateCombo.Core;
using UltimateCombo.Data;

namespace UltimateCombo.Combos.PvP;

internal static class SAMPvP
{
    internal const uint
        Yukikaze = 29523,
        Gekko = 29524,
        Kasha = 29525,

        OgiNamikiri = 29530,
        KaeshiNamikiri = 29531,

        Mineuchi = 29535,

        Soten = 29532,
        Hyosetsu = 29526,
        Mangetsu = 29527,
        Oka = 29528,

        Chiten = 29533,
        Zanshin = 41577,

        MeikyoShisui = 29536,
        TendoSetsugekka = 41454,
        TendoKaeshiSetsugekka = 41455,

        Zantetsuken = 29537;

    internal static class Buffs
    {
        internal const ushort
            Kaiten = 3201,

            Chiten = 1240,
            ZanshinReady = 1318,

            MeikyoShisui = 1320,
            TendoSetsugekkaReady = 3203,

            OgiNamikiri = 3199,
            KaeshiNamikiri = 3200;
    }

    internal class Debuffs
    {
        internal const ushort
            Debana = 4306,
            Kuzushi = 3202;
    }

    internal class SAMPvP_Combo : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.SAMPvP_Combo;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is Yukikaze or Gekko or Kasha or Hyosetsu or Mangetsu or Oka)
                && IsEnabled(Presets.SAMPvP_Combo))
            {
                if (IsEnabled(Presets.SAMPvP_Zantetsuken) && GetLimitBreakCurrentValue() == GetLimitBreakMaxValue()
                    && TargetHasEffect(Debuffs.Kuzushi))
                {
                    return Zantetsuken;
                }

                if (!TargetHasEffectAny(AllPvP.Buffs.Guard))
                {
                    if (CanWeave(actionID, ActionWatching.LastGCD))
                    {
                        if (IsEnabled(Presets.SAMPvP_Chiten) && ActionReady(Chiten)
                            && (GetLimitBreakCurrentValue() <= GetLimitBreakMaxValue() * 0.75 || GetLimitBreakCurrentValue() == GetLimitBreakMaxValue()))
                        {
                            return Chiten;
                        }

                        if (IsEnabled(Presets.SAMPvP_Chiten) && HasEffect(Buffs.ZanshinReady)
                            && (CurrentHP <= MaxHP - 12000 || EffectRemainingTime(Buffs.ZanshinReady) <= 2))
                        {
                            return Zanshin;
                        }

                        if (IsEnabled(Presets.SAMPvP_Soten) && ActionReady(Soten)
                            && !HasEffect(Buffs.Kaiten) && InActionRange(Soten))
                        {
                            return Soten;
                        }

                        if (IsEnabled(Presets.SAMPvP_MeikyoShisui) && ActionReady(MeikyoShisui))
                        {
                            return MeikyoShisui;
                        }

                        if (IsEnabled(Presets.SAMPvP_Mineuchi) && ActionReady(Mineuchi)
                            && InActionRange(Mineuchi) && !TargetHasEffectAny(AllPvP.Buffs.Resilience))
                        {
                            return Mineuchi;
                        }
                    }

                    if (IsEnabled(Presets.SAMPvP_Soten) && ActionReady(Soten) && !HasEffect(Buffs.Kaiten) && !InActionRange(OgiNamikiri))
                    {
                        return Soten;
                    }

                    if (IsEnabled(Presets.SAMPvP_MeikyoShisui) && WasLastWeaponskill(TendoSetsugekka) && InActionRange(TendoSetsugekka))
                    {
                        return TendoKaeshiSetsugekka;
                    }

                    if (IsEnabled(Presets.SAMPvP_MeikyoShisui) && HasEffect(Buffs.TendoSetsugekkaReady) && InActionRange(TendoSetsugekka))
                    {
                        return TendoSetsugekka;
                    }

                    if (IsEnabled(Presets.SAMPvP_Namikiri) && WasLastWeaponskill(OgiNamikiri) && InActionRange(OgiNamikiri))
                    {
                        return KaeshiNamikiri;
                    }

                    if (IsEnabled(Presets.SAMPvP_Namikiri) && ActionReady(OgiNamikiri) && InActionRange(OgiNamikiri))
                    {
                        return OgiNamikiri;
                    }
                }
            }

            return actionID;
        }
    }
}
