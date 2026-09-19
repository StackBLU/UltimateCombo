using System;
using System.Collections.Generic;
using UltimateCombo.Data;

namespace UltimateCombo.Attributes;

[AttributeUsage(AttributeTargets.Field)]
internal sealed class ReplaceSkillAttribute : Attribute
{
    internal ReplaceSkillAttribute(params uint[] actionIDs)
    {
        foreach (var actionID in actionIDs)
        {
            if (ActionWatching.ActionSheet.TryGetValue(actionID, out Lumina.Excel.Sheets.Action action))
            {
                ActionNames.Add(action.Name.ToString());
                ActionIcons.Add(action.Icon);
            }
        }
    }

    internal List<string> ActionNames { get; } = [];
    internal List<ushort> ActionIcons { get; } = [];
}
