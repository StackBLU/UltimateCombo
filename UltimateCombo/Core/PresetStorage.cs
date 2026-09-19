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
    private static HashSet<Presets> _pvpCombos = [];
    private static HashSet<Presets> _bozjaCombos = [];
    private static HashSet<Presets> _occultCombos = [];
    private static HashSet<Presets> _eurekaCombos = [];
    private static HashSet<Presets> _variantCombos = [];
    private static Dictionary<Presets, Presets[]> _conflictingCombos = [];
    private static Dictionary<Presets, Presets?> _parentCombos = [];

    internal static void Init()
    {
        Presets[] presets = Enum.GetValues<Presets>();

        _pvpCombos = [.. presets.Where(preset => preset.GetAttribute<PvPAttribute>() != null)];
        _bozjaCombos = [.. presets.Where(preset => preset.GetAttribute<BozjaAttribute>() != null)];
        _occultCombos = [.. presets.Where(preset => preset.GetAttribute<OccultAttribute>() != null)];
        _eurekaCombos = [.. presets.Where(preset => preset.GetAttribute<EurekaAttribute>() != null)];
        _variantCombos = [.. presets.Where(preset => preset.GetAttribute<VariantAttribute>() != null)];

        _conflictingCombos = presets.ToDictionary(
            preset => preset,
            preset => preset.GetAttribute<ConflictingCombosAttribute>()?.ConflictingPresets ?? []);

        _parentCombos = presets.ToDictionary(
            preset => preset,
            preset => preset.GetAttribute<ParentComboAttribute>()?.ParentPreset);
    }

    internal static bool IsEnabled(Presets preset)
    {
        return Service.Configuration.EnabledActions.Contains(preset);
    }

    internal static bool IsPvP(Presets preset)
    {
        return _pvpCombos.Contains(preset);
    }

    internal static bool IsBozja(Presets preset)
    {
        return _bozjaCombos.Contains(preset);
    }

    internal static bool IsOccult(Presets preset)
    {
        return _occultCombos.Contains(preset);
    }

    internal static bool IsEureka(Presets preset)
    {
        return _eurekaCombos.Contains(preset);
    }

    internal static bool IsVariant(Presets preset)
    {
        return _variantCombos.Contains(preset);
    }

    internal static Presets? GetParent(Presets preset)
    {
        return _parentCombos.TryGetValue(preset, out Presets? parent) ? parent : null;
    }

    internal static Presets[] GetConflicts(Presets preset)
    {
        return _conflictingCombos.TryGetValue(preset, out Presets[]? conflicts) ? conflicts : [];
    }

    internal static List<Presets> GetAllConflicts()
    {
        return _conflictingCombos
            .Where(pair => pair.Value.Length > 0)
            .Select(pair => pair.Key)
            .ToList();
    }

    internal static List<Presets[]> GetAllConflictOriginals()
    {
        return _conflictingCombos.Values
            .Where(conflicts => conflicts.Length > 0)
            .ToList();
    }
}
