using System;
using System.Collections.Generic;
using System.Linq;
using UltimateCombo.Core;

namespace UltimateCombo.Attributes;

[AttributeUsage(AttributeTargets.Field)]
internal sealed class BlueInactiveAttribute : Attribute
{
    private readonly uint[] _actionIDs;

    internal BlueInactiveAttribute(params uint[] actionIDs)
    {
        _actionIDs = actionIDs;
    }

    internal List<uint> Actions
    {
        get
        {
            if (Service.Configuration == null)
            {
                return [];
            }

            return [.. _actionIDs.Where(actionID => !Service.Configuration.ActiveBLUSpells.Contains(actionID))];
        }
    }

    internal bool NoneSet => Service.Configuration != null
                && !_actionIDs.Any(Service.Configuration.ActiveBLUSpells.Contains);
}
