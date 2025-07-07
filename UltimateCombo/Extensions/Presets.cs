using System.Collections.Generic;

using Dalamud.Utility;

using UltimateCombo.Attributes;
using UltimateCombo.Combos;

namespace UltimateCombo.Extensions
{
    internal static class PresetExtensions
    {
        internal static Dictionary<CustomComboPreset, ReplaceSkillAttribute> replaceSkillCache = [];
        internal static Dictionary<CustomComboPreset, CustomComboInfoAttribute> comboInfoCache = [];
        internal static Dictionary<CustomComboPreset, HoverInfoAttribute> hoverInfoCache = [];

        ///<summary> Retrieves the <see cref="ReplaceSkillAttribute"/> for the preset if it exists.</summary>
        internal static ReplaceSkillAttribute? GetReplaceAttribute(this CustomComboPreset preset)
        {
            if (replaceSkillCache.TryGetValue(preset, out ReplaceSkillAttribute? replaceSkillAttribute))
            {
                return replaceSkillAttribute;
            }

            ReplaceSkillAttribute? att = preset.GetAttribute<ReplaceSkillAttribute>();
            return att != null && replaceSkillCache.TryAdd(preset, att) ? replaceSkillCache[preset] : null;
        }

        ///<summary> Retrieves the <see cref="CustomComboInfoAttribute"/> for the preset if it exists.</summary>
        internal static CustomComboInfoAttribute? GetComboAttribute(this CustomComboPreset preset)
        {
            if (comboInfoCache.TryGetValue(preset, out CustomComboInfoAttribute? customComboInfoAttribute))
            {
                return customComboInfoAttribute;
            }

            CustomComboInfoAttribute? att = preset.GetAttribute<CustomComboInfoAttribute>();
            return att != null && comboInfoCache.TryAdd(preset, att) ? comboInfoCache[preset] : null;

        }

        ///<summary> Retrieves the <see cref="HoverInfoAttribute"/> for the preset if it exists.</summary>
        internal static HoverInfoAttribute? GetHoverAttribute(this CustomComboPreset preset)
        {
            if (hoverInfoCache.TryGetValue(preset, out HoverInfoAttribute? hoverInfoAttribute))
            {
                return hoverInfoAttribute;
            }

            HoverInfoAttribute? att = preset.GetAttribute<HoverInfoAttribute>();
            return att != null && hoverInfoCache.TryAdd(preset, att) ? hoverInfoCache[preset] : null;

        }
    }
}
