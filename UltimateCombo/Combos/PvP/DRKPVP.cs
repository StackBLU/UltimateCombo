using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Combos.General;
using UltimateCombo.Core;

namespace UltimateCombo.Combos.PvP;

internal static class DRKPvP
{
    internal const uint
        HardSlash = 29085,
        SyphonStrike = 29086,
        Souleater = 29087,

        Shadowbringer = 29091,
        ScarletDelirium = 41434,
        Comeuppance = 41435,
        Torcleaver = 41436,

        Impalement = 41438,
        Plunge = 29092,
        TheBlackestNight = 29093,
        SaltedEarth = 29094,
        SaltAndDarkness = 29095,

        Eventide = 29097,
        Disesteem = 41437;

    internal static class Buffs
    {
        internal const ushort
            Blackblood = 3033,
            ComeuppanceReady = 4288,
            TorcleaverReady = 4289,

            BlackestNight = 1308,
            DarkArts = 3034,

            SaltedEarth = 3036,
            SaltedEarthDefense = 3037,

            UndeadRedemption = 3039,
            Scorn = 4290;
    }

    internal static class Debuffs
    {
        internal const ushort
            SoleSurvivor = 1306,
            SaltsBane = 3038;
    }

    internal static class Config
    {
        internal static UserInt
            DRKPvP_Shadowbringer = new("DRKPvP_Shadowbringer", 50),
            DRKPvP_Impalement = new("DRKPvP_Impalement", 75);
    }

    internal class DRKPvP_Combo : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.DRKPvP_Combo;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is HardSlash or SyphonStrike or Souleater or ScarletDelirium or Comeuppance or Torcleaver)
                && IsEnabled(Presets.DRKPvP_Combo))
            {
                if (IsEnabled(Presets.DRKPvP_Eventide) && GetLimitBreakCurrentValue() == GetLimitBreakMaxValue())
                {
                    if (PlayerHealthPercentageHp() < 50 && CurrentMP >= GetResourceCost(AllPvP.Recuperate))
                    {
                        return AllPvP.Recuperate;
                    }

                    return Eventide;
                }

                if (IsEnabled(Presets.DRKPvP_SaltAndDarkness) && (IsActionEnabled(SaltAndDarkness) || WasLastAbility(SaltedEarth)))
                {
                    return SaltAndDarkness;
                }

                if (IsEnabled(Presets.DRKPvP_SaltedEarth) && ActionReady(SaltedEarth)
                    && WasLastAbility(Plunge))
                {
                    return SaltedEarth;
                }

                if (!TargetHasEffectAny(AllPvP.Buffs.Guard))
                {
                    if (CanWeave(actionID))
                    {
                        if (IsEnabled(Presets.DRKPvP_BlackestNight) && ActionReady(TheBlackestNight)
                            && !HasEffect(Buffs.BlackestNight) && !HasEffect(Buffs.DarkArts) && !WasLastAction(TheBlackestNight))
                        {
                            return TheBlackestNight;
                        }

                        if (IsEnabled(Presets.DRKPvP_Shadowbringer) && ActionReady(Shadowbringer)
                            && PlayerHealthPercentageHp() > GetOptionValue(Config.DRKPvP_Shadowbringer) && !WasLastAction(Shadowbringer))
                        {
                            return Shadowbringer;
                        }
                    }

                    if (IsEnabled(Presets.DRKPvP_Plunge) && ActionReady(Plunge))
                    {
                        return Plunge;
                    }

                    if (IsEnabled(Presets.DRKPvP_Disesteem) && HasEffect(Buffs.Scorn))
                    {
                        return Disesteem;
                    }

                    if (IsEnabled(Presets.DRKPvP_Impalement) && ActionReady(Impalement)
                        && PlayerHealthPercentageHp() <= GetOptionValue(Config.DRKPvP_Impalement))
                    {
                        return Impalement;
                    }
                }
            }

            return actionID;
        }
    }
}
