using UltimateCombo.Combos.General;
using UltimateCombo.Core;
using UltimateCombo.Data;

namespace UltimateCombo.Combos.PvP;

internal static class RPRPvP
{
    internal const uint
        Slice = 29538,
        WaxingSlice = 29539,
        InfernalSlice = 29540,

        HarvestMoon = 29545,

        GrimSwathe = 29547,
        ExecutionersGuillotine = 41456,

        PlentifulHarvest = 29546,

        DeathWarrant = 29549,
        FateSealed = 41457,

        HellsIngress = 29550,
        Regress = 29551,

        ArcaneCrest = 29552,

        TenebraeLemurum = 29553,
        VoidReaping = 29543,
        CrossReaping = 29544,
        LemuresSlice = 29548,
        Communio = 29554,
        Perfectio = 41458;

    internal static class Buffs
    {
        internal const ushort
            ExecutionersGuillotineReady = 4307,
            ImmortalSacrifice = 3204,

            DeathWarrant = 4308,

            Threshold = 2860,
            HellsIngress = 3207,

            CrestOfTimeBorrowed = 2861,

            Enshrouded = 2863,
            RipeForReaping = 2858,
            PerfectioParata = 4309;
    }

    internal class Debuffs
    {
        internal const ushort
            DeathWarrant = 3206;
    }

    internal class RPRPvP_Combo : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.RPRPvP_Combo;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is Slice or WaxingSlice or InfernalSlice) && IsEnabled(Presets.RPRPvP_Combo))
            {
                if (!TargetHasEffectAny(AllPvP.Buffs.Guard))
                {
                    if (CanWeave(actionID, ActionWatching.LastGCD))
                    {
                        if (IsEnabled(Presets.RPRPvP_ArcaneCrest) && ActionReady(ArcaneCrest))
                        {
                            return ArcaneCrest;
                        }

                        if (IsEnabled(Presets.RPRPvP_DeathWarrant) && ActionReady(DeathWarrant) && InActionRange(DeathWarrant))
                        {
                            return DeathWarrant;
                        }

                        if (IsEnabled(Presets.RPRPvP_Enshrouded) && ActionReady(LemuresSlice) && HasEffect(Buffs.Enshrouded))
                        {
                            return LemuresSlice;
                        }

                        if (IsEnabled(Presets.RPRPvP_GrimSwathe) && ActionReady(GrimSwathe) && InActionRange(GrimSwathe)
                            && !HasEffect(Buffs.Enshrouded))
                        {
                            return GrimSwathe;
                        }
                    }

                    if (IsEnabled(Presets.RPRPvP_Enshrouded) && HasEffect(Buffs.Enshrouded) && EffectStacks(Buffs.Enshrouded) == 1)
                    {
                        return Communio;
                    }

                    if (IsEnabled(Presets.RPRPvP_Enshrouded) && HasEffect(Buffs.PerfectioParata))
                    {
                        return Perfectio;
                    }

                    if (IsEnabled(Presets.RPRPvP_Enshrouded) && HasEffect(Buffs.Enshrouded) && InActionRange(VoidReaping))
                    {
                        if (HasEffect(Buffs.RipeForReaping))
                        {
                            return CrossReaping;
                        }

                        return VoidReaping;
                    }

                    if (IsEnabled(Presets.RPRPvP_PlentifulHarvest) && ActionReady(PlentifulHarvest) && EffectStacks(Buffs.ImmortalSacrifice) == 8)
                    {
                        return PlentifulHarvest;
                    }

                    if (IsEnabled(Presets.RPRPvP_HarvestMoon) && ActionReady(HarvestMoon)
                        && (EnemyPercentHP() <= 50 || GetRemainingCharges(HarvestMoon) == GetMaxCharges(HarvestMoon)))
                    {
                        return HarvestMoon;
                    }
                }
            }

            return actionID;
        }
    }
}
