using Dalamud.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using UltimateCombo.Attributes;
using UltimateCombo.Attributes.Content;
using UltimateCombo.Combos;

namespace UltimateCombo.Core;

internal static class PresetStorage
{
    private static HashSet<Presets>? _pvpCombos;
    private static HashSet<Presets>? _bozjaCombos;
    private static HashSet<Presets>? _occultCombos;
    private static HashSet<Presets>? _eurekaCombos;
    private static HashSet<Presets>? _variantCombos;
    private static Dictionary<Presets, Presets[]>? _conflictingCombos;
    private static Dictionary<Presets, Presets?>? _parentCombos;

    internal static void Init()
    {
        _pvpCombos = [.. Enum.GetValues<Presets>().Where(preset => preset.GetAttribute<PvPAttribute>() != default)];
        _bozjaCombos = [.. Enum.GetValues<Presets>().Where(preset => preset.GetAttribute<BozjaAttribute>() != default)];
        _occultCombos = [.. Enum.GetValues<Presets>().Where(preset => preset.GetAttribute<OccultAttribute>() != default)];
        _eurekaCombos = [.. Enum.GetValues<Presets>().Where(preset => preset.GetAttribute<EurekaAttribute>() != default)];
        _variantCombos = [.. Enum.GetValues<Presets>().Where(preset => preset.GetAttribute<VariantAttribute>() != default)];

        _conflictingCombos = Enum.GetValues<Presets>()
            .ToDictionary(
                preset => preset,
                preset => preset.GetAttribute<ConflictingCombosAttribute>()?.ConflictingPresets ?? []);

        _parentCombos = Enum.GetValues<Presets>()
            .ToDictionary(
                preset => preset,
                preset => preset.GetAttribute<ParentComboAttribute>()?.ParentPreset);
    }

    internal static bool IsEnabled(Presets preset)
    {
        return Service.Configuration.EnabledActions.Contains(preset);
    }

    internal static bool IsPvP(Presets preset)
    {
        return _pvpCombos?.Contains(preset) ?? false;
    }

    internal static bool IsBozja(Presets preset)
    {
        return _bozjaCombos?.Contains(preset) ?? false;
    }

    internal static bool IsOccult(Presets preset)
    {
        return _occultCombos?.Contains(preset) ?? false;
    }

    internal static bool IsEureka(Presets preset)
    {
        return _eurekaCombos?.Contains(preset) ?? false;
    }

    internal static bool IsVariant(Presets preset)
    {
        return _variantCombos?.Contains(preset) ?? false;
    }

    internal static Presets? GetParent(Presets preset)
    {
        return _parentCombos?[preset];
    }

    internal static Presets[] GetConflicts(Presets preset)
    {
        return _conflictingCombos?[preset] ?? [];
    }

    internal static List<Presets> GetAllConflicts()
    {
        return _conflictingCombos?.Keys.ToList() ?? [];
    }

    internal static List<Presets[]> GetAllConflictOriginals()
    {
        return _conflictingCombos?.Values.ToList() ?? [];
    }
}
