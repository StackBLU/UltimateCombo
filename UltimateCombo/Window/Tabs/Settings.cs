using ImGuiNET;
using System.Numerics;
using UltimateCombo.Services;

namespace UltimateCombo.Window.Tabs
{
	internal class Settings : ConfigWindow
	{
		internal static new void Draw()
		{
			PvEFeatures.HasToOpenJob = true;
			_ = ImGui.BeginChild("main", new Vector2(0, 0), true);

			bool hideChildren = Service.Configuration.HideChildren;
			if (ImGui.Checkbox("Hide nested options", ref hideChildren))
			{
				Service.Configuration.HideChildren = hideChildren;
				Service.Configuration.Save();
			}

			ImGui.NextColumn();
			bool hideConflicting = Service.Configuration.HideConflictedCombos;
			if (ImGui.Checkbox("Hide conflicting combos", ref hideConflicting))
			{
				Service.Configuration.HideConflictedCombos = hideConflicting;
				Service.Configuration.Save();
			}

			ImGui.NextColumn();
			bool ignoreGCDChecks = Service.Configuration.IgnoreGCDChecks;
			if (ImGui.Checkbox("Ignore GCD Checks - Combos will start using abilities immediately", ref ignoreGCDChecks))
			{
				Service.Configuration.IgnoreGCDChecks = ignoreGCDChecks;
				Service.Configuration.Save();
			}

			ImGui.NextColumn();
			bool disableTripleWeaving = Service.Configuration.DisableTripleWeaving;
			if (ImGui.Checkbox("Disable Triple Weaving", ref disableTripleWeaving))
			{
				Service.Configuration.DisableTripleWeaving = disableTripleWeaving;
				Service.Configuration.Save();
			}

			ImGui.EndChild();
		}
	}
}