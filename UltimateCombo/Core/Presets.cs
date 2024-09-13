using Dalamud.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using UltimateCombo.Attributes;
using UltimateCombo.Combos;
using UltimateCombo.Services;

namespace UltimateCombo.Core
{
	internal static class PresetStorage
	{
		private static HashSet<CustomComboPreset>? PvPCombos;
		private static HashSet<CustomComboPreset>? BozjaCombos;
		private static HashSet<CustomComboPreset>? EurekaCombos;
		private static Dictionary<CustomComboPreset, CustomComboPreset[]>? ConflictingCombos;
		private static Dictionary<CustomComboPreset, CustomComboPreset?>? ParentCombos;

		public static void Init()
		{
			PvPCombos = Enum.GetValues<CustomComboPreset>()
				.Where(preset => preset.GetAttribute<PvPCustomComboAttribute>() != default)
				.ToHashSet();

			BozjaCombos = Enum.GetValues<CustomComboPreset>()
				.Where(preset => preset.GetAttribute<BozjaAttribute>() != default)
				.ToHashSet();

			EurekaCombos = Enum.GetValues<CustomComboPreset>()
				.Where(preset => preset.GetAttribute<EurekaAttribute>() != default)
				.ToHashSet();

			ConflictingCombos = Enum.GetValues<CustomComboPreset>()
				.ToDictionary(
					preset => preset,
					preset => preset.GetAttribute<ConflictingCombosAttribute>()?.ConflictingPresets ?? Array.Empty<CustomComboPreset>());

			ParentCombos = Enum.GetValues<CustomComboPreset>()
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
			return PvPCombos.Contains(preset);
		}

		public static bool IsBozja(CustomComboPreset preset)
		{
			return BozjaCombos.Contains(preset);
		}

		public static bool IsEureka(CustomComboPreset preset)
		{
			return EurekaCombos.Contains(preset);
		}

		public static CustomComboPreset? GetParent(CustomComboPreset preset)
		{
			return ParentCombos[preset];
		}

		public static CustomComboPreset[] GetConflicts(CustomComboPreset preset)
		{
			return ConflictingCombos[preset];
		}

		public static List<CustomComboPreset> GetAllConflicts()
		{
			return ConflictingCombos.Keys.ToList();
		}

		public static List<CustomComboPreset[]> GetAllConflictOriginals()
		{
			return ConflictingCombos.Values.ToList();
		}
	}
}