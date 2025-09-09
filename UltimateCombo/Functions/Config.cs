using System;
using System.Linq;
using UltimateCombo.Core;
namespace UltimateCombo.ComboHelper.Functions;

internal abstract partial class CustomComboFunctions
{
    internal static int GetOptionValue(string SliderID)
    {
        return PluginConfiguration.GetCustomIntValue(SliderID);
    }
    internal static bool GetOptionBool(string SliderID)
    {
        return PluginConfiguration.GetCustomBoolValue(SliderID);
    }
}

internal class UserData(string v)
{
    internal string Name { get; } = v;
    public static implicit operator string(UserData o)
    {
        return o.Name;
    }
}

internal class UserFloat : UserData
{
    private readonly float _defaultValue;

    internal UserFloat(string v) : this(v, 0.0f) { }
    internal UserFloat(string v, float defaults) : base(v)
    {
        _defaultValue = defaults;
        if (!PluginConfiguration.CustomFloatValues.ContainsKey(Name))
        {
            PluginConfiguration.SetCustomFloatValue(Name, defaults);
            Service.Configuration.Save();
        }
    }

    internal void Reset()
    {
        PluginConfiguration.SetCustomFloatValue(Name, _defaultValue);
        Service.Configuration.Save();
    }

    public static implicit operator float(UserFloat o)
    {
        return PluginConfiguration.GetCustomFloatValue(o.Name);
    }
}

internal class UserInt : UserData
{
    internal readonly int _defaultValue;
    internal UserInt(string v) : this(v, 0) { }
    internal UserInt(string v, int defaults) : base(v)
    {
        _defaultValue = defaults;
        if (!PluginConfiguration.CustomIntValues.ContainsKey(Name))
        {
            PluginConfiguration.SetCustomIntValue(Name, defaults);
            Service.Configuration.Save();
        }
    }
    internal void Reset()
    {
        PluginConfiguration.SetCustomIntValue(Name, _defaultValue);
        Service.Configuration.Save();
    }
    public static implicit operator int(UserInt o)
    {
        return PluginConfiguration.GetCustomIntValue(o.Name);
    }
}

internal class UserBool : UserData
{
    private readonly bool _defaultValue;

    internal UserBool(string v) : this(v, false) { }
    internal UserBool(string v, bool defaults) : base(v)
    {
        _defaultValue = defaults;
        if (!PluginConfiguration.CustomBoolValues.ContainsKey(Name))
        {
            PluginConfiguration.SetCustomBoolValue(Name, defaults);
            Service.Configuration.Save();
        }
    }

    internal void Reset()
    {
        PluginConfiguration.SetCustomBoolValue(Name, _defaultValue);
        Service.Configuration.Save();
    }

    public static implicit operator bool(UserBool o)
    {
        return PluginConfiguration.GetCustomBoolValue(o.Name);
    }
}

internal class UserIntArray(string v) : UserData(v)
{
    internal int Count => PluginConfiguration.GetCustomIntArrayValue(Name).Length;
    internal bool Any(Func<int, bool> func)
    {
        return PluginConfiguration.GetCustomIntArrayValue(Name).Any(func);
    }
    internal void Clear(int maxValues)
    {
        var array = PluginConfiguration.GetCustomIntArrayValue(Name);
        Array.Resize(ref array, maxValues);
        PluginConfiguration.SetCustomIntArrayValue(Name, array);
        Service.Configuration.Save();
    }
}
