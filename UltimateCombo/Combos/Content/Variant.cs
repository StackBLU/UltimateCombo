using ECommons.DalamudServices;
using ECommons.Logging;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Combos.General;
using UltimateCombo.Combos.PvE;
using UltimateCombo.Core;
using UltimateCombo.Data;

namespace UltimateCombo.Combos.Content;

internal static class Variant
{
    internal const uint
    //Purely for image purposes in the plugin
        VariantCure_Image = 46939,
        VariantUltimatum_Image = 29730,
        VariantRaise_Image = 29731,
        VariantRaise2_Image = 29734,
        VariantSpiritDart_Image = 46940,
        VariantRampart_Image = 46941,
        VariantEagleEyeShot_Image = 46942;

    //1069 = The Sil'dihn Subterrane
    //1137 = Mount Rokkon
    //1176 = Aloalo Island
    //1315 = Merchant's Tale

    //1075 = Another Sil'dihn Subterrane
    //1155 = Another Mount Rokkon
    //1179 = Another Aloalo Island
    //1316 = Another Merchant's Tale

    //1076 = Another Sil'dihn Subterrane (Savage)
    //1156 = Another Mount Rokkon (Savage)
    //1180 = Another Aloalo Island (Savage)
    //1317 = Another Merchant's Tale (Savage)

    internal static uint VariantCure => Svc.ClientState.TerritoryType switch
    {
        1069 => 29729,
        1137 or 1176 => 33862,
        1315 => 46939,
        _ => 0
    };

    internal static uint VariantUltimatum => Svc.ClientState.TerritoryType switch
    {
        1069 or 1137 or 1176 or 1315 => 29730,
        _ => 0
    };

    internal static uint VariantRaise => Svc.ClientState.TerritoryType switch
    {
        1069 or 1137 or 1176 or 1315 => 29731,
        1075 or 1076 or 1155 or 1156 or 1179 or 1180 or 1316 or 1317 => 29734,
        _ => 0
    };

    internal static uint VariantSpiritDart => Svc.ClientState.TerritoryType switch
    {
        1069 => 29732,
        1137 or 1176 => 33863,
        1315 => 46940,
        _ => 0
    };

    internal static uint VariantRampart => Svc.ClientState.TerritoryType switch
    {
        1069 => 29733,
        1137 or 1176 => 33864,
        1315 => 46941,
        _ => 0
    };

    internal static uint VariantEagleEyeShot => Svc.ClientState.TerritoryType switch
    {
        1069 or 1137 or 1176 or 1315 => 46942,
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
                if (ActionReady(VariantCure) && IsActionEnabled(VariantCure) && PlayerHealthPercentageHp() <= GetOptionValue(Config.Variant_Cure))
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
            PluginLog.Debug(Svc.ClientState.TerritoryType + " ");

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

    internal class Variant_EagleEyeShot : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.Variant_EagleEyeShot;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.Variant_EagleEyeShot) && SafeToUse() && IsComboAction(actionID))
            {
                if (ActionReady(VariantEagleEyeShot) && IsActionEnabled(VariantEagleEyeShot) && CanWeave(actionID, ActionWatching.LastGCD))
                {
                    return VariantEagleEyeShot;
                }
            }

            return actionID;
        }
    }
}
