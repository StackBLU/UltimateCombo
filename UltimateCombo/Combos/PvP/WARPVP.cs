using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Combos.General;
using UltimateCombo.Core;
using UltimateCombo.Data;

namespace UltimateCombo.Combos.PvP;

internal static class WARPvP
{
    internal const uint
        HeavySwing = 29074,
        Maim = 29075,
        StormsPath = 29076,
        FellCleave = 29078,

        Onslaught = 29079,
        Orogeny = 29080,

        Blota = 29081,
        InnerChaos = 29077,

        Bloodwhetting = 29082,
        ChaoticCyclone = 29736,

        PrimalRend = 29084,
        PrimalRuination = 41432,

        PrimalScream = 29083,
        PrimalWrath = 41433;

    internal static class Buffs
    {
        internal const ushort
            Bloodwhetting = 3030,
            StemTheTide = 3031,

            ChaoticCycloneReady = 1992,
            InnerChaosReady = 4284,
            PrimalRuinationReady = 4285,

            InnerRelease = 1303,
            ThrillOfBattle = 3185,
            Wrathful = 4286,
            BurgeoningFury = 4287;
    }
    internal static class Debuffs
    {
        internal const ushort
            Onslaught = 3029,
            Orogeny = 3256,
            Unguarded = 3021;
    }

    internal static class Config
    {
        internal static UserInt
            WARPvP_Bloodwhetting = new("WARPvP_Bloodwhetting", 75);
    }

    internal class WARPvP_Combo : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.WARPvP_Combo;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is HeavySwing or Maim or StormsPath or FellCleave)
                && IsEnabled(Presets.WARPvP_Combo))
            {
                if (IsEnabled(Presets.WARPvP_PrimalScream) && GetLimitBreakCurrentValue() == GetLimitBreakMaxValue())
                {
                    return PrimalScream;
                }

                if (!TargetHasEffectAny(AllPvP.Buffs.Guard))
                {
                    if (CanWeave(actionID, ActionWatching.LastGCD))
                    {
                        if (IsEnabled(Presets.WARPvP_Bloodwhetting) && ActionReady(Bloodwhetting)
                            && PlayerHealthPercentageHp() <= GetOptionValue(Config.WARPvP_Bloodwhetting))
                        {
                            return Bloodwhetting;
                        }

                        if (IsEnabled(Presets.WARPvP_Onslaught) && ActionReady(Onslaught))
                        {
                            return Onslaught;
                        }

                        if (IsEnabled(Presets.WARPvP_Orogeny) && ActionReady(Orogeny)
                            && InActionRange(Orogeny))
                        {
                            return Orogeny;
                        }
                    }

                    if (IsEnabled(Presets.WARPvP_PrimalRend) && ActionReady(PrimalRend))
                    {
                        return PrimalRend;
                    }

                    if (IsEnabled(Presets.WARPvP_Blota) && ActionReady(Blota) && !InActionRange(HeavySwing))
                    {
                        return Blota;
                    }

                    if (IsEnabled(Presets.WARPvP_PrimalRuination) && HasEffect(Buffs.PrimalRuinationReady))
                    {
                        return PrimalRuination;
                    }

                    if (IsEnabled(Presets.WARPvP_ChaoticCyclone) && HasEffect(Buffs.ChaoticCycloneReady))
                    {
                        return ChaoticCyclone;
                    }

                    if (IsEnabled(Presets.WARPvP_InnerChaos) && HasEffect(Buffs.InnerChaosReady))
                    {
                        return InnerChaos;
                    }

                    if (IsEnabled(Presets.WARPvP_PrimalWrath) && HasEffect(Buffs.Wrathful))
                    {
                        return PrimalWrath;
                    }
                }
            }

            return actionID;
        }
    }
}
