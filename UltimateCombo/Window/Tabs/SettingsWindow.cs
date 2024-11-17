using ImGuiNET;
using System.Numerics;
using UltimateCombo.Services;

namespace UltimateCombo.Window.Tabs
{
	internal class SettingsWindow : ConfigWindow
	{
		internal static new void Draw()
		{
			PvEWindow.HasToOpenJob = true;
			_ = ImGui.BeginChild("main", new Vector2(0, 0), true);

			bool openOnLaunch = Service.Configuration.OpenOnLaunch;
			if (ImGui.Checkbox("Open on launch", ref openOnLaunch))
			{
				Service.Configuration.OpenOnLaunch = openOnLaunch;
				Service.Configuration.Save();
			}

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

			ImGui.NextColumn();
			double rangedAttackRange = Service.Configuration.RangedAttackRange;
			ImGui.SetNextItemWidth(150);
			if (ImGui.InputDouble("Minimum range away from target before using ranged attacks on melee classes", ref rangedAttackRange, 1))
			{
				Service.Configuration.RangedAttackRange = rangedAttackRange;
				Service.Configuration.Save();
			}

			ImGui.EndChild();
		}
	}
}