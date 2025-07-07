using System;
using System.Collections.Generic;
using System.Linq;

using Dalamud.Utility;

using UltimateCombo.Attributes;
using UltimateCombo.Combos;
using UltimateCombo.Services;

namespace UltimateCombo.Core
{
    internal static class PresetStorage
    {
        private static HashSet<CustomComboPreset>? _pvpCombos;
        private static HashSet<CustomComboPreset>? _bozjaCombos;
        private static HashSet<CustomComboPreset>? _occultCombos;
        private static HashSet<CustomComboPreset>? _eurekaCombos;
        private static HashSet<CustomComboPreset>? _variantCombos;
        private static Dictionary<CustomComboPreset, CustomComboPreset[]>? _conflictingCombos;
        private static Dictionary<CustomComboPreset, CustomComboPreset?>? _parentCombos;

        public static void Init()
        {
            _pvpCombos = [.. Enum.GetValues<CustomComboPreset>().Where(preset => preset.GetAttribute<PvPCustomComboAttribute>() != default)];

            _bozjaCombos = [.. Enum.GetValues<CustomComboPreset>().Where(preset => preset.GetAttribute<BozjaAttribute>() != default)];

            _occultCombos = [.. Enum.GetValues<CustomComboPreset>().Where(preset => preset.GetAttribute<OccultAttribute>() != default)];

            _eurekaCombos = [.. Enum.GetValues<CustomComboPreset>().Where(preset => preset.GetAttribute<EurekaAttribute>() != default)];

            _variantCombos = [.. Enum.GetValues<CustomComboPreset>().Where(preset => preset.GetAttribute<VariantAttribute>() != default)];

            _conflictingCombos = Enum.GetValues<CustomComboPreset>()
                .ToDictionary(
                    preset => preset,
                    preset => preset.GetAttribute<ConflictingCombosAttribute>()?.ConflictingPresets ?? []);

            _parentCombos = Enum.GetValues<CustomComboPreset>()
                .ToDictionary(
                    preset => preset,
                    preset => preset.GetAttribute<ParentComboAttribute>()?.ParentPreset);
        }


        public static bool IsEnabled(CustomComboPreset preset)
        {
            return Service.Configuration.EnabledActions.Contains(preset);
        }

        public static bool IsPvP(CustomComboPreset preset)
        {
            return _pvpCombos?.Contains(preset) ?? false;
        }

        public static bool IsBozja(CustomComboPreset preset)
        {
            return _bozjaCombos?.Contains(preset) ?? false;
        }

        public static bool IsOccult(CustomComboPreset preset)
        {
            return _occultCombos?.Contains(preset) ?? false;
        }

        public static bool IsEureka(CustomComboPreset preset)
        {
            return _eurekaCombos?.Contains(preset) ?? false;
        }

        public static bool IsVariant(CustomComboPreset preset)
        {
            return _variantCombos?.Contains(preset) ?? false;
        }

        public static CustomComboPreset? GetParent(CustomComboPreset preset)
        {
            return _parentCombos?[preset];
        }

        public static CustomComboPreset[] GetConflicts(CustomComboPreset preset)
        {
            return _conflictingCombos?[preset] ?? [];
        }

        public static List<CustomComboPreset> GetAllConflicts()
        {
            return _conflictingCombos?.Keys.ToList() ?? [];
        }

        public static List<CustomComboPreset[]> GetAllConflictOriginals()
        {
            return _conflictingCombos?.Values.ToList() ?? [];
        }
    }
}
