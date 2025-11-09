using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Combos.General;
using UltimateCombo.Core;
using UltimateCombo.Data;

namespace UltimateCombo.Combos.PvP;

internal static class PLDPvP
{
    internal const uint
        FastBlade = 29058,
        RiotBlade = 29059,
        RoyalAuthority = 29060,
        Atonement = 29061,
        Supplication = 41428,
        Sepulchre = 41429,

        HolySpirit = 29062,
        Intervene = 29065,
        Guardian = 29066,
        HolySheltron = 29067,
        ShieldSmite = 41430,
        Imperator = 41431,
        Confiteor = 42194,

        Phalanx = 29069,
        BladeOfFaith = 29071,
        BladeOfTruth = 29072,
        BladeOfValor = 29073;

    internal static class Buffs
    {
        internal const ushort
            AtonementReady = 2015,
            SupplicationReady = 2016,
            SepulchreReady = 2017,

            HolySheltron = 3026,
            SwordOath = 1991,
            ShieldOath = 3188,

            ConfiteorReady = 3028,

            Cover = 1300,
            Covered = 1301,

            HallowedGround = 1302,
            Phalanx = 3210,
            BladeOfFaithReady = 3250;
    }

    internal static class Debuffs
    {
        internal const ushort
            SacredClaim = 3025,
            ShieldSmite = 4283;
    }

    internal static class Config
    {
        internal static UserInt
            PLDPvP_Phalanx = new("PLDPvP_Phalanx", 25);
    }

    internal class PLDPvP_Combo : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.PLDPvP_Combo;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is FastBlade or RiotBlade or RoyalAuthority or Atonement or Supplication or Sepulchre)
                && IsEnabled(Presets.PLDPvP_Combo))
            {
                if (IsEnabled(Presets.PLDPvP_Phalanx) && GetLimitBreakCurrentValue() == GetLimitBreakMaxValue()
                    && (PlayerHealthPercentageHp() <= GetOptionValue(Config.PLDPvP_Phalanx) || WasLastAbility(Guardian)))
                {
                    return Phalanx;
                }

                if (IsEnabled(Presets.PLDPvP_AutoGuard) && GetLimitBreakCurrentValue() != GetLimitBreakMaxValue()
                    && WasLastAbility(Guardian) && ActionReady(AllPvP.Guard) && !HasEffect(Buffs.HallowedGround))
                {
                    return AllPvP.Guard;
                }

                if (IsEnabled(Presets.PLDPvP_ShieldSmite) && ActionReady(ShieldSmite)
                    && InActionRange(ShieldSmite) && TargetHasEffectAny(AllPvP.Buffs.Guard))
                {
                    return ShieldSmite;
                }

                if (!TargetHasEffectAny(AllPvP.Buffs.Guard))
                {
                    if (CanWeave(actionID, ActionWatching.LastGCD))
                    {
                        if (IsEnabled(Presets.PLDPvP_Imperator) && ActionReady(Imperator) && InActionRange(Imperator))
                        {
                            return Imperator;
                        }

                        if (IsEnabled(Presets.PLDPvP_HolySheltron) && ActionReady(HolySheltron))
                        {
                            return HolySheltron;
                        }
                    }

                    if (IsEnabled(Presets.PLDPvP_Blades)
                        && (HasEffect(Buffs.BladeOfFaithReady) || lastComboMove is BladeOfFaith || lastComboMove is BladeOfTruth))
                    {
                        return OriginalHook(Phalanx);
                    }

                    if (IsEnabled(Presets.PLDPvP_Confiteor) && HasEffect(Buffs.ConfiteorReady))
                    {
                        return Confiteor;
                    }

                    if (IsEnabled(Presets.PLDPvP_Intervene) && ActionReady(Intervene))
                    {
                        return Intervene;
                    }

                    if (IsEnabled(Presets.PLDPvP_HolySpirit) && ActionReady(HolySpirit) && (CurrentHP <= MaxHP - 6000 || !InActionRange(FastBlade)))
                    {
                        return HolySpirit;
                    }
                }
            }

            return actionID;
        }
    }
}
