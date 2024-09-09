using ImGuiNET;
using StackCombo.Services;
using System.Numerics;

namespace StackCombo.Window.Tabs
{
	internal class Settings : ConfigWindow
	{
		internal static new void Draw()
		{
			PvEFeatures.HasToOpenJob = true;
			_ = ImGui.BeginChild("main", new Vector2(0, 0), true);

			bool hideChildren = Service.Configuration.HideChildren;
			if (ImGui.Checkbox("Hide sub options", ref hideChildren))
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

			ImGui.EndChild();
		}
	}
}