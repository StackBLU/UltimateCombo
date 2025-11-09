using UltimateCombo.Combos.General;
using UltimateCombo.Core;
using UltimateCombo.Data;

namespace UltimateCombo.Combos.PvP;

internal static class RDMPvP
{
    internal const uint
        Jolt3 = 41486,
        GrandImpact = 41487,

        EnchantedRiposte = 41488,
        EnchantedZwerchhau = 41489,
        EnchantedRedoublement = 41490,
        Scorch = 41491,

        Resolution = 41492,

        Embolden = 41494,
        Prefulgence = 41495,

        CorpsACorps = 29699,
        Displacement = 29700,

        Forte = 41496,
        ViceOfThorns = 41493,

        SouthernCross = 41498;

    internal static class Buffs
    {
        internal const ushort
            Dualcast = 1393,
            Displacement = 3243,

            Embolden = 2282,
            PrefulgenceReady = 4322,

            Forte = 4320,
            ThornedFlourish = 4321,

            EnchantedRiposte = 3234,
            EnchantedZwercchaur = 3235,
            EnchantedRedoublement = 3236;
    }

    internal class Debuffs
    {
        internal const ushort
            Monomachy = 3242,
            Scorch = 4319;
    }

    internal class RDMPvP_Combo : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.RDMPvP_Combo;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is Jolt3 or GrandImpact) && IsEnabled(Presets.RDMPvP_Combo))
            {
                if (IsEnabled(Presets.RDMPvP_SouthernCross) && GetLimitBreakCurrentValue() == GetLimitBreakMaxValue()
                    && WasLastAbility(ViceOfThorns))
                {
                    return SouthernCross;
                }

                if (!TargetHasEffectAny(AllPvP.Buffs.Guard))
                {
                    if (CanWeave(actionID, ActionWatching.LastGCD))
                    {
                        if (IsEnabled(Presets.RDMPvP_Displacement) && ActionReady(Displacement) && WasLastAction(EnchantedRedoublement))
                        {
                            return Displacement;
                        }

                        if (IsEnabled(Presets.RDMPvP_Forte) && ActionReady(Forte) && WasLastAbility(CorpsACorps))
                        {
                            return Forte;
                        }

                        if (IsEnabled(Presets.RDMPvP_Embolden) && ActionReady(Embolden))
                        {
                            return Embolden;
                        }

                        if (IsEnabled(Presets.RDMPvP_ViceOfThorns) && HasEffect(Buffs.ThornedFlourish))
                        {
                            return ViceOfThorns;
                        }
                    }

                    if (IsEnabled(Presets.RDMPvP_Prefulgence) && HasEffect(Buffs.PrefulgenceReady))
                    {
                        return Prefulgence;
                    }

                    if (IsEnabled(Presets.RDMPvP_Resolution) && ActionReady(Resolution))
                    {
                        return Resolution;
                    }
                }

                if (IsEnabled(Presets.RDMPvP_Melee) && ActionReady(OriginalHook(EnchantedRiposte)))
                {
                    if (IsEnabled(Presets.RDMPvP_CorpsACorps) && ActionReady(CorpsACorps)
                        && (!TargetHasEffect(Debuffs.Monomachy) || !InActionRange(OriginalHook(EnchantedRiposte))))
                    {
                        return CorpsACorps;
                    }

                    if (InActionRange(OriginalHook(EnchantedRiposte)))
                    {
                        return OriginalHook(EnchantedRiposte);
                    }
                }
            }

            return actionID;
        }
    }
}
