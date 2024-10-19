﻿using System;
using System.Collections.Generic;
using UltimateCombo.Data;

namespace UltimateCombo.Attributes
{
	/// <summary> Attribute documenting which skill each preset replace. </summary>
	[AttributeUsage(AttributeTargets.Field)]
	public class ReplaceSkillAttribute : Attribute
	{
		/// <summary> List of each action the feature replaces. Initializes a new instance of the <see cref="ReplaceSkillAttribute"/> class. </summary>
		/// <param name="actionIDs"> List of actions the preset replaces. </param>
		internal ReplaceSkillAttribute(params uint[] actionIDs)
		{
			foreach (uint id in actionIDs)
			{
				if (ActionWatching.ActionSheet.TryGetValue(id, out Lumina.Excel.GeneratedSheets.Action? action) && action != null)
				{
					ActionNames.Add($"{action.Name}");
					ActionIcons.Add(action.Icon);
				}
			}
		}

		internal List<string> ActionNames { get; set; } = [];

		internal List<ushort> ActionIcons { get; set; } = [];
	}
}
