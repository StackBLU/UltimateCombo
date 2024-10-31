using Dalamud.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UltimateCombo.Combos;
using UltimateCombo.Combos.PvE;
using UltimateCombo.Extensions;
using UltimateCombo.Services;

namespace UltimateCombo.Core
{
	[Serializable]
	public class PluginConfiguration : IPluginConfiguration
	{
		public int Version { get; set; } = 5;

		[JsonProperty("EnabledActionsV6")]
		public HashSet<CustomComboPreset> EnabledActions { get; set; } = [];

		public bool EnabledOutputLog { get; set; } = false;

		public bool HideConflictedCombos { get; set; } = false;

		public bool HideChildren { get; set; } = false;

		public bool IgnoreGCDChecks { get; set; } = false;

		public bool DisableTripleWeaving { get; set; } = false;

		public double RangedAttackRange { get; set; } = 7;

		public Vector4 TargetHighlightColor { get; set; } = new() { W = 1, X = 0.5f, Y = 0.5f, Z = 0.5f };

		[JsonProperty("CustomFloatValuesV6")]
		internal static Dictionary<string, float> CustomFloatValues { get; set; } = [];

		public static float GetCustomFloatValue(string config, float defaultMinValue = 0)
		{
			if (!CustomFloatValues.TryGetValue(config, out float configValue))
			{
				SetCustomFloatValue(config, defaultMinValue);
				return defaultMinValue;
			}

			return configValue;
		}

		public static void SetCustomFloatValue(string config, float value)
		{
			CustomFloatValues[config] = value;
		}

		[JsonProperty("CustomIntValuesV6")]
		internal static Dictionary<string, int> CustomIntValues { get; set; } = [];

		public static int GetCustomIntValue(string config, int defaultMinVal = 0)
		{
			if (!CustomIntValues.TryGetValue(config, out int configValue))
			{
				SetCustomIntValue(config, defaultMinVal);
				return defaultMinVal;
			}

			return configValue;
		}

		public static void SetCustomIntValue(string config, int value)
		{
			CustomIntValues[config] = value;
		}

		[JsonProperty("CustomIntArrayValuesV6")]
		internal static Dictionary<string, int[]> CustomIntArrayValues { get; set; } = [];

		public static int[] GetCustomIntArrayValue(string config)
		{
			if (!CustomIntArrayValues.TryGetValue(config, out int[]? configValue))
			{
				SetCustomIntArrayValue(config, []);
				return [];
			}

			return configValue;
		}

		public static void SetCustomIntArrayValue(string config, int[] value)
		{
			CustomIntArrayValues[config] = value;
		}

		[JsonProperty("CustomBoolValuesV6")]
		internal static Dictionary<string, bool> CustomBoolValues { get; set; } = [];

		public static bool GetCustomBoolValue(string config)
		{
			if (!CustomBoolValues.TryGetValue(config, out bool configValue))
			{
				SetCustomBoolValue(config, false);
				return false;
			}

			return configValue;
		}

		public static void SetCustomBoolValue(string config, bool value)
		{
			CustomBoolValues[config] = value;
		}

		[JsonProperty("CustomBoolArrayValuesV6")]
		internal static Dictionary<string, bool[]> CustomBoolArrayValues { get; set; } = [];

		public static bool[] GetCustomBoolArrayValue(string config)
		{
			if (!CustomBoolArrayValues.TryGetValue(config, out bool[]? configValue))
			{
				SetCustomBoolArrayValue(config, Array.Empty<bool>());
				return Array.Empty<bool>();
			}

			return configValue;
		}

		public static void SetCustomBoolArrayValue(string config, bool[] value)
		{
			CustomBoolArrayValues[config] = value;
		}

		public List<uint> ActiveBLUSpells { get; set; } = [];

		public uint[] DancerDanceCompatActionIDs { get; set; } = new uint[]
		{
			DNC.Cascade,
			DNC.Flourish,
			DNC.FanDance1,
			DNC.FanDance2,
		};

		[JsonProperty]
		private static Dictionary<string, bool> ResetFeatureCatalog { get; set; } = [];

		private static bool GetResetValues(string config)
		{
			return ResetFeatureCatalog.TryGetValue(config, out bool value) && value;
		}

		private static void SetResetValues(string config, bool value)
		{
			ResetFeatureCatalog[config] = value;
		}

		public void ResetFeatures(string config, int[] values)
		{
			Service.PluginLog.Debug($"{config} {GetResetValues(config)}");
			if (!GetResetValues(config))
			{
				bool needToResetMessagePrinted = false;

				IEnumerable<int> presets = Enum.GetValues<CustomComboPreset>().Cast<int>();

				foreach (int value in values)
				{
					Service.PluginLog.Debug(value.ToString());
					if (presets.Contains(value))
					{
						CustomComboPreset preset = Enum.GetValues<CustomComboPreset>()
							.Where(preset => (int)preset == value)
							.First();

						if (!PresetStorage.IsEnabled(preset))
						{
							continue;
						}

						if (!needToResetMessagePrinted)
						{
							Service.ChatGui.PrintError($"[UltimateCombo] Some features have been disabled due to an internal configuration update:");
							needToResetMessagePrinted = !needToResetMessagePrinted;
						}

						Attributes.CustomComboInfoAttribute? info = preset.GetComboAttribute();
						Service.ChatGui.PrintError($"[UltimateCombo] - {info.JobName}: {info.FancyName}");
						_ = EnabledActions.Remove(preset);
					}
				}

				if (needToResetMessagePrinted)
				{
					Service.ChatGui.PrintError($"[UltimateCombo] Please re-enable these features to use them again. We apologise for the inconvenience");
				}
			}
			SetResetValues(config, true);
			Save();
		}

		public bool RecommendedSettingsViewed { get; set; } = false;

		public void Save()
		{
			Service.Interface.SavePluginConfig(this);
		}

	}
}