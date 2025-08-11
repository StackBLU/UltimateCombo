using Dalamud.Utility;
using System.Collections.Generic;
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
			if (att != null && replaceSkillCache.TryAdd(preset, att))
			{
				return replaceSkillCache[preset];
			}

			return null;
		}

		///<summary> Retrieves the <see cref="CustomComboInfoAttribute"/> for the preset if it exists.</summary>
		internal static CustomComboInfoAttribute? GetComboAttribute(this CustomComboPreset preset)
		{
			if (comboInfoCache.TryGetValue(preset, out CustomComboInfoAttribute? customComboInfoAttribute))
			{
				return customComboInfoAttribute;
			}

			CustomComboInfoAttribute? att = preset.GetAttribute<CustomComboInfoAttribute>();
			if (att != null && comboInfoCache.TryAdd(preset, att))
			{
				return comboInfoCache[preset];
			}

			return null;
		}

		///<summary> Retrieves the <see cref="HoverInfoAttribute"/> for the preset if it exists.</summary>
		internal static HoverInfoAttribute? GetHoverAttribute(this CustomComboPreset preset)
		{
			if (hoverInfoCache.TryGetValue(preset, out HoverInfoAttribute? hoverInfoAttribute))
			{
				return hoverInfoAttribute;
			}

			HoverInfoAttribute? att = preset.GetAttribute<HoverInfoAttribute>();
			if (att != null && hoverInfoCache.TryAdd(preset, att))
			{
				return hoverInfoCache[preset];
			}

			return null;
		}
	}
}
