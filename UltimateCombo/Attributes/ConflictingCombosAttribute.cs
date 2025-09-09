using System;
using UltimateCombo.Combos;

namespace UltimateCombo.Attributes;

[AttributeUsage(AttributeTargets.Field)]
internal sealed class ConflictingCombosAttribute : Attribute
{
    internal ConflictingCombosAttribute(params Presets[] conflictingPresets)
    {
        ConflictingPresets = conflictingPresets;
    }

    internal Presets[] ConflictingPresets { get; }
}
