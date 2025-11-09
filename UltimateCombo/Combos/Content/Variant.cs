using ECommons.DalamudServices;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Combos.General;
using UltimateCombo.Combos.PvE;
using UltimateCombo.Core;
using UltimateCombo.Data;

namespace UltimateCombo.Combos.Content;

internal static class Variant
{
    internal const uint
        VariantUltimatum = 29730,
        VariantRaise = 29731,
        VariantRaise2 = 29734,

        //Don't use these for logic - they are just for the ReplaceSkill icon
        VariantCure_Image = 29729,
        VariantSpiritDart_Image = 29732,
        VariantRampart_Image = 29733;

    //1069 = The Sil'dihn Subterrane
    //1137 = Mount Rokkon
    //1176 = Aloalo Island

    internal static uint VariantCure => Svc.ClientState.TerritoryType switch
    {
        1069 => 29729,
        1137 or 1176 => 33862,
        _ => 0
    };

    internal static uint VariantSpiritDart => Svc.ClientState.TerritoryType switch
    {
        1069 => 29732,
        1137 or 1176 => 33863,
        _ => 0
    };

    internal static uint VariantRampart => Svc.ClientState.TerritoryType switch
    {
        1069 => 29733,
        1137 or 1176 => 33864,
        _ => 0
    };

    internal static class Buffs
    {
        internal const ushort
            EmnityUp = 3358,
            VulnDown = 3360,
            Rehabilitation = 3367,
            DamageBarrier = 3405;
    }

    internal static class Debuffs
    {
        internal const ushort
            SustainedDamage = 3359;
    }
    internal static class Config
    {
        internal static UserInt
            Variant_Cure = new("All_Variant_Cure", 50);
    }

    internal class Variant_Cure : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.Variant_Cure;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.Variant_Cure) && SafeToUse() && IsComboAction(actionID))
            {
                if (ActionReady(VariantCure) && IsActionEnabled(VariantCure)
                    && PlayerHealthPercentageHp() <= GetOptionValue(Config.Variant_Cure))
                {
                    return VariantCure;
                }
            }

            return actionID;
        }
    }

    internal class Variant_Raise : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.Variant_Raise;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.Variant_Raise) && SafeToUse() && IsComboAction(actionID))
            {
                if (IsActionEnabled(VariantCure)
                    && actionID is WHM.Raise or SCH.Resurrection or AST.Ascend
                        or SGE.Egeiro or SMN.Resurrection or RDM.Verraise)
                {
                    if (ActionReady(Common.Swiftcast))
                    {
                        return Common.Swiftcast;
                    }

                    if (ActionReady(VariantRaise2) && IsActionEnabled(VariantRaise2))
                    {
                        return VariantRaise2;
                    }

                    if (ActionReady(VariantRaise) && IsActionEnabled(VariantRaise))
                    {
                        return VariantRaise;
                    }
                }
            }

            return actionID;
        }
    }

    internal class Variant_Ultimatum : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.Variant_Ultimatum;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.Variant_Ultimatum) && SafeToUse() && IsComboAction(actionID))
            {
                if (ActionReady(VariantUltimatum) && IsActionEnabled(VariantUltimatum) && CanWeave(actionID, ActionWatching.LastGCD))
                {
                    return VariantUltimatum;
                }
            }

            return actionID;
        }
    }

    internal class Variant_SpiritDart : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.Variant_SpiritDart;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.Variant_SpiritDart) && SafeToUse() && IsComboAction(actionID))
            {
                if (ActionReady(VariantSpiritDart) && IsActionEnabled(VariantSpiritDart)
                    && CanWeave(actionID, ActionWatching.LastGCD) && !TargetHasEffectAny(Debuffs.SustainedDamage))
                {
                    return VariantSpiritDart;
                }
            }

            return actionID;
        }
    }

    internal class Variant_Rampart : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.Variant_Rampart;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.Variant_Rampart) && SafeToUse() && IsComboAction(actionID))
            {
                if (ActionReady(VariantRampart) && IsActionEnabled(VariantRampart) && CanWeave(actionID, ActionWatching.LastGCD))
                {
                    return VariantRampart;
                }
            }

            return actionID;
        }
    }
}
