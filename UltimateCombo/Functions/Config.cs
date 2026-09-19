using System;
using System.Linq;
using UltimateCombo.Core;

namespace UltimateCombo.ComboHelper.Functions;

internal abstract partial class CustomComboFunctions
{
    internal static int GetOptionValue(string config)
    {
        return PluginConfiguration.GetCustomIntValue(config);
    }

    internal static bool GetOptionBool(string config)
    {
        return PluginConfiguration.GetCustomBoolValue(config);
    }
}

internal abstract class UserData
{
    protected UserData(string name)
    {
        Name = name;
    }

    internal string Name { get; }

    public static implicit operator string(UserData value) => value.Name;
}

internal sealed class UserFloat : UserData
{
    private readonly float _defaultValue;

    internal UserFloat(string name) : this(name, 0f)
    {
    }

    internal UserFloat(string name, float defaultValue) : base(name)
    {
        _defaultValue = defaultValue;

        if (!PluginConfiguration.CustomFloatValues.ContainsKey(Name))
        {
            PluginConfiguration.SetCustomFloatValue(Name, defaultValue);
        }
    }

    internal void Reset()
    {
        PluginConfiguration.SetCustomFloatValue(Name, _defaultValue);
        Service.Configuration.Save();
    }

    public static implicit operator float(UserFloat value) => PluginConfiguration.GetCustomFloatValue(value.Name, value._defaultValue);
}

internal sealed class UserInt : UserData
{
    private readonly int _defaultValue;

    internal UserInt(string name) : this(name, 0)
    {
    }

    internal UserInt(string name, int defaultValue) : base(name)
    {
        _defaultValue = defaultValue;

        if (!PluginConfiguration.CustomIntValues.ContainsKey(Name))
        {
            PluginConfiguration.SetCustomIntValue(Name, defaultValue);
        }
    }

    internal void Reset()
    {
        PluginConfiguration.SetCustomIntValue(Name, _defaultValue);
        Service.Configuration.Save();
    }

    public static implicit operator int(UserInt value) => PluginConfiguration.GetCustomIntValue(value.Name, value._defaultValue);
}

internal sealed class UserBool : UserData
{
    private readonly bool _defaultValue;

    internal UserBool(string name) : this(name, false)
    {
    }

    internal UserBool(string name, bool defaultValue) : base(name)
    {
        _defaultValue = defaultValue;

        if (!PluginConfiguration.CustomBoolValues.ContainsKey(Name))
        {
            PluginConfiguration.SetCustomBoolValue(Name, defaultValue);
        }
    }

    internal void Reset()
    {
        PluginConfiguration.SetCustomBoolValue(Name, _defaultValue);
        Service.Configuration.Save();
    }

    public static implicit operator bool(UserBool value) => PluginConfiguration.GetCustomBoolValue(value.Name);
}

internal sealed class UserIntArray : UserData
{
    internal UserIntArray(string name) : base(name)
    {
    }

    internal int Count => PluginConfiguration.GetCustomIntArrayValue(Name).Length;

    internal bool Any(Func<int, bool> predicate)
    {
        return PluginConfiguration.GetCustomIntArrayValue(Name).Any(predicate);
    }

    internal void Clear(int length)
    {
        PluginConfiguration.SetCustomIntArrayValue(Name, new int[Math.Max(0, length)]);
        Service.Configuration.Save();
    }
}
