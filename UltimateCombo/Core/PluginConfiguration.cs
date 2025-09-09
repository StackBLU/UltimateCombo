using Dalamud.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UltimateCombo.Combos;

namespace UltimateCombo.Core;

[Serializable]
public class PluginConfiguration : IPluginConfiguration
{
    public int Version { get; set; } = 5;

    [JsonProperty("EnabledActionsV6")]
    public HashSet<Presets> EnabledActions { get; set; } = [];

    public bool EnabledOutputLog { get; set; } = false;
    public bool OpenOnLaunch { get; set; } = false;
    public bool HideChildren { get; set; } = false;
    public bool HideConflictedCombos { get; set; } = false;
    public bool IgnoreGCDChecks { get; set; } = false;
    public bool DisableTripleWeaving { get; set; } = false;
    public double RangedAttackRange { get; set; } = 7;

    [JsonProperty("CustomFloatValuesV6")]
    public static Dictionary<string, float> CustomFloatValues { get; set; } = [];

    public static float GetCustomFloatValue(string config, float defaultMinValue = 0)
    {
        if (!CustomFloatValues.TryGetValue(config, out var configValue))
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
    public static Dictionary<string, int> CustomIntValues { get; set; } = [];

    public static int GetCustomIntValue(string config, int defaultMinVal = 0)
    {
        if (!CustomIntValues.TryGetValue(config, out var configValue))
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
    public static Dictionary<string, int[]> CustomIntArrayValues { get; set; } = [];

    public static int[] GetCustomIntArrayValue(string config)
    {
        if (!CustomIntArrayValues.TryGetValue(config, out var configValue))
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
    public static Dictionary<string, bool> CustomBoolValues { get; set; } = [];

    public static bool GetCustomBoolValue(string config)
    {
        if (!CustomBoolValues.TryGetValue(config, out var configValue))
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
    public static Dictionary<string, bool[]> CustomBoolArrayValues { get; set; } = [];

    public static bool[] GetCustomBoolArrayValue(string config)
    {
        if (!CustomBoolArrayValues.TryGetValue(config, out var configValue))
        {
            SetCustomBoolArrayValue(config, []);
            return [];
        }

        return configValue;
    }

    public static void SetCustomBoolArrayValue(string config, bool[] value)
    {
        CustomBoolArrayValues[config] = value;
    }

    public List<uint> ActiveBLUSpells { get; set; } = [];

    int IPluginConfiguration.Version { get => Version; set => Version = value; }

    public void Save()
    {
        Service.Interface.SavePluginConfig(this);
    }
}
