using System;
using System.Collections.Generic;
using UltimateCombo.Core;

namespace UltimateCombo.Attributes;

[AttributeUsage(AttributeTargets.Field)]
internal sealed class BlueInactiveAttribute : Attribute
{
    internal BlueInactiveAttribute(params uint[] actionIDs)
    {
        if (Service.Configuration is null)
        {
            return;
        }

        NoneSet = true;
        foreach (var id in actionIDs)
        {
            if (Service.Configuration.ActiveBLUSpells.Contains(id))
            {
                NoneSet = false;
                continue;
            }
            Actions.Add(id);
        }
    }

    internal List<uint> Actions { get; set; } = [];
    internal bool NoneSet { get; set; } = false;
}
