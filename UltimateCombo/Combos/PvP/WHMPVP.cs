using UltimateCombo.Combos.General;
using UltimateCombo.Core;
using UltimateCombo.Data;

namespace UltimateCombo.Combos.PvP;

internal static class WHMPvP
{
    internal const uint
        Glare3 = 29223,
        Cure2 = 29224,

        AfflatusMisery = 29226,
        Aquaveil = 29227,
        MiracleOfNature = 29228,

        SeraphStrike = 29229,
        Cure3 = 29225,
        Glare4 = 41499,

        AfflatusPurgation = 29230;

    internal static class Buffs
    {
        internal const ushort
            Aquaveil = 3086,

            Protect = 1415,
            SacredSight = 4326,
            Cure3Ready = 3083,

            Temperance = 2037,
            TemperanceRegen = 2038;
    }

    internal class Debuffs
    {
        internal const ushort
            MiracleOfNature = 3085;
    }

    internal class WHMPvP_Combo : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.WHMPvP_Combo;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (actionID is Glare3 && IsEnabled(Presets.WHMPvP_Combo))
            {
                if (!TargetHasEffectAny(AllPvP.Buffs.Guard))
                {
                    if (IsEnabled(Presets.WHMPvP_AfflatusMisery) && ActionReady(AfflatusMisery))
                    {
                        return AfflatusMisery;
                    }
                }

                if (IsEnabled(Presets.WHMPvP_Aquaveil) && ActionReady(Aquaveil) && CanWeave(actionID, ActionWatching.LastGCD))
                {
                    return Aquaveil;
                }

                if (IsEnabled(Presets.WHMPvP_Cure3) && HasEffect(Buffs.Cure3Ready) && !WasLastAction(SeraphStrike)
                    && (CurrentHP <= MaxHP - 16000 || EffectRemainingTime(Buffs.Cure3Ready) < 5 || HasFriendlyTarget()))
                {
                    return Cure3;
                }

                if (HasEffect(Buffs.SacredSight))
                {
                    return Glare4;
                }
            }

            return actionID;
        }
    }

    internal class WHMPvP_NoWasteMiracleOfNature : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.WHMPvP_NoWasteMiracleOfNature;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (actionID is MiracleOfNature && IsEnabled(Presets.WHMPvP_NoWasteMiracleOfNature))
            {
                if (!TargetHasEffectAny(AllPvP.Buffs.Guard))
                {
                    return MiracleOfNature;
                }

                return OriginalHook(11);
            }

            return actionID;
        }
    }
}
