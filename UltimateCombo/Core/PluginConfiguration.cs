
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

    public bool EnabledOutputLog { get; set; }
    public bool OpenOnLaunch { get; set; }
    public bool HideChildren { get; set; }
    public bool HideConflictedCombos { get; set; }
    public bool ShowInternalNames { get; set; }
    public bool IgnoreGCDChecks { get; set; }
    public bool DisableTripleWeaving { get; set; }
    public double RangedAttackRange { get; set; } = 7;

    [JsonProperty("CustomFloatValuesV6")]
    public Dictionary<string, float> SavedCustomFloatValues { get; set; } = [];

    [JsonProperty("CustomIntValuesV6")]
    public Dictionary<string, int> SavedCustomIntValues { get; set; } = [];

    [JsonProperty("CustomIntArrayValuesV6")]
    public Dictionary<string, int[]> SavedCustomIntArrayValues { get; set; } = [];

    [JsonProperty("CustomBoolValuesV6")]
    public Dictionary<string, bool> SavedCustomBoolValues { get; set; } = [];

    [JsonProperty("CustomBoolArrayValuesV6")]
    public Dictionary<string, bool[]> SavedCustomBoolArrayValues { get; set; } = [];

    [JsonIgnore]
    public static Dictionary<string, float> CustomFloatValues
    {
        get => Service.Configuration.SavedCustomFloatValues;
        set => Service.Configuration.SavedCustomFloatValues = value;
    }

    [JsonIgnore]
    public static Dictionary<string, int> CustomIntValues
    {
        get => Service.Configuration.SavedCustomIntValues;
        set => Service.Configuration.SavedCustomIntValues = value;
    }

    [JsonIgnore]
    public static Dictionary<string, int[]> CustomIntArrayValues
    {
        get => Service.Configuration.SavedCustomIntArrayValues;
        set => Service.Configuration.SavedCustomIntArrayValues = value;
    }

    [JsonIgnore]
    public static Dictionary<string, bool> CustomBoolValues
    {
        get => Service.Configuration.SavedCustomBoolValues;
        set => Service.Configuration.SavedCustomBoolValues = value;
    }

    [JsonIgnore]
    public static Dictionary<string, bool[]> CustomBoolArrayValues
    {
        get => Service.Configuration.SavedCustomBoolArrayValues;
        set => Service.Configuration.SavedCustomBoolArrayValues = value;
    }

    public List<uint> ActiveBLUSpells { get; set; } = [];

    public static float GetCustomFloatValue(string config, float defaultValue = 0)
    {
        if (!CustomFloatValues.TryGetValue(config, out var value))
        {
            value = defaultValue;
            CustomFloatValues[config] = value;
        }

        return value;
    }

    public static void SetCustomFloatValue(string config, float value)
    {
        CustomFloatValues[config] = value;
    }

    public static int GetCustomIntValue(string config, int defaultValue = 0)
    {
        if (!CustomIntValues.TryGetValue(config, out var value))
        {
            value = defaultValue;
            CustomIntValues[config] = value;
        }

        return value;
    }

    public static void SetCustomIntValue(string config, int value)
    {
        CustomIntValues[config] = value;
    }

    public static int[] GetCustomIntArrayValue(string config)
    {
        if (!CustomIntArrayValues.TryGetValue(config, out var value))
        {
            value = [];
            CustomIntArrayValues[config] = value;
        }

        return value;
    }

    public static void SetCustomIntArrayValue(string config, int[] value)
    {
        CustomIntArrayValues[config] = value;
    }

    public static bool GetCustomBoolValue(string config)
    {
        if (!CustomBoolValues.TryGetValue(config, out var value))
        {
            CustomBoolValues[config] = false;
        }

        return value;
    }

    public static void SetCustomBoolValue(string config, bool value)
    {
        CustomBoolValues[config] = value;
    }

    public static bool[] GetCustomBoolArrayValue(string config)
    {
        if (!CustomBoolArrayValues.TryGetValue(config, out var value))
        {
            value = [];
            CustomBoolArrayValues[config] = value;
        }

        return value;
    }

    public static void SetCustomBoolArrayValue(string config, bool[] value)
    {
        CustomBoolArrayValues[config] = value;
    }

    public void Save()
    {
        Service.IconReplacer?.RefreshEnabledCombos();
        Service.Interface.SavePluginConfig(this);
    }
}

