using System;
using System.Collections.Generic;
using UltimateCombo.Data;

namespace UltimateCombo.Attributes;

[AttributeUsage(AttributeTargets.Field)]
internal class ReplaceSkillAttribute : Attribute
{
    internal ReplaceSkillAttribute(params uint[] actionIDs)
    {
        foreach (var id in actionIDs)
        {
            if (ActionWatching.ActionSheet.TryGetValue(id, out Lumina.Excel.Sheets.Action action))
            {
                ActionNames.Add($"{action.Name}");
                ActionIcons.Add(action.Icon);
            }
        }
    }

    internal List<string> ActionNames { get; set; } = [];
    internal List<ushort> ActionIcons { get; set; } = [];
}
