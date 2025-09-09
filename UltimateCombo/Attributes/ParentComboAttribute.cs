using System;
using UltimateCombo.Combos;

namespace UltimateCombo.Attributes;

[AttributeUsage(AttributeTargets.Field)]
internal class ParentComboAttribute : Attribute
{
    internal ParentComboAttribute(Presets parentPreset)
    {
        ParentPreset = parentPreset;
    }

    internal Presets ParentPreset { get; }
}
