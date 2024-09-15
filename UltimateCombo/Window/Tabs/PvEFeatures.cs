using Dalamud.Interface.Textures.TextureWraps;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using ECommons.ImGuiMethods;
using ImGuiNET;
using System.Linq;
using System.Numerics;
using UltimateCombo.Combos;
using UltimateCombo.Core;
using UltimateCombo.Services;
using UltimateCombo.Window.Functions;

namespace UltimateCombo.Window.Tabs
{
	internal class PvEFeatures : ConfigWindow
	{
		internal static bool HasToOpenJob = true;
		internal static string OpenJob = string.Empty;

		internal static new void Draw()
		{
			int i = 1;
			float indentwidth = 12f.Scale();
			float indentwidth2 = indentwidth + 42f.Scale();

			if (OpenJob == string.Empty)
			{
				foreach (string? jobName in groupedPresets.Keys)
				{
					string abbreviation = groupedPresets[jobName].First().Info.JobShorthand;
					string header = string.IsNullOrEmpty(abbreviation) ? jobName : $"{jobName} - {abbreviation}";
					byte id = groupedPresets[jobName].First().Info.JobID;
					IDalamudTextureWrap? icon = Icons.GetJobIcon(id);

					if (ImGui.Selectable($"###{header}", OpenJob == jobName, ImGuiSelectableFlags.None,
						icon == null ? new Vector2(0) : new Vector2(0, (icon.Size.Y / 2f).Scale())))
					{
						OpenJob = jobName;
					}

					ImGui.SameLine(indentwidth);

					if (icon != null)
					{
						ImGui.Image(icon.ImGuiHandle, new Vector2(icon.Size.X.Scale(), icon.Size.Y.Scale()) / 2f);
						ImGui.SameLine(indentwidth2);
						ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0, 0, 0, 0));
						ImGui.PushStyleVar(ImGuiStyleVar.ButtonTextAlign, new Vector2(0, 0.5f));
						_ = ImGui.Button($"{header}", new Vector2(0, icon.Size.Y.Scale()) / 2f);
						ImGui.PopStyleColor();
						ImGui.PopStyleVar();
					}
				}
			}
			else
			{
				byte id = groupedPresets[OpenJob].First().Info.JobID;
				IDalamudTextureWrap? icon = Icons.GetJobIcon(id);

				using (ImRaii.IEndObject headingTab = ImRaii.Child("HeadingTab",
					new Vector2(ImGui.GetContentRegionAvail().X, icon is null ? 24f.Scale() : (icon.Size.Y / 2f).Scale() + 4f)))
				{
					if (ImGui.Button("Back", new Vector2(0, 24f.Scale())))
					{
						OpenJob = "";
						return;
					}
					ImGui.SameLine();
					ImGuiEx.LineCentered(() =>
					{
						if (icon != null)
						{
							ImGui.Image(icon.ImGuiHandle, new Vector2(icon.Size.X.Scale(), icon.Size.Y.Scale()) / 2f);
							ImGui.SameLine();
						}
						ImGuiEx.Text($"{OpenJob}");
					});

				}

				using ImRaii.IEndObject contents = ImRaii.Child("Contents", new Vector2(0), false);
				try
				{
					if (ImGui.BeginTabBar($"subTab{OpenJob}", ImGuiTabBarFlags.Reorderable | ImGuiTabBarFlags.AutoSelectNewTabs))
					{
						if (ImGui.BeginTabItem("Normal"))
						{
							DrawHeadingContents(OpenJob, i);
							ImGui.EndTabItem();
						}

						if (groupedPresets[OpenJob].Any(x => PresetStorage.IsBozja(x.Preset)))
						{
							if (ImGui.BeginTabItem("Bozja"))
							{
								DrawBozjaContents(OpenJob);
								ImGui.EndTabItem();
							}
						}

						if (groupedPresets[OpenJob].Any(x => PresetStorage.IsEureka(x.Preset)))
						{
							if (ImGui.BeginTabItem("Eureka"))
							{
								DrawEurekaContents(OpenJob);
								ImGui.EndTabItem();
							}
						}

						ImGui.EndTabBar();
					}
				}
				catch { }
			}
		}

		private static void DrawBozjaContents(string jobName)
		{
			foreach ((CustomComboPreset preset, Attributes.CustomComboInfoAttribute info) in groupedPresets[jobName].Where(x => PresetStorage.IsBozja(x.Preset)))
			{
				int i = -1;
				InfoBox presetBox = new() { Color = Colors.Grey, BorderThickness = 1f, CurveRadius = 8f, ContentsAction = () => { Presets.DrawPreset(preset, info, ref i); } };
				presetBox.Draw();
				ImGuiHelpers.ScaledDummy(12.0f);
			}
		}

		private static void DrawEurekaContents(string jobName)
		{
			foreach ((CustomComboPreset preset, Attributes.CustomComboInfoAttribute info) in groupedPresets[jobName].Where(x => PresetStorage.IsEureka(x.Preset)))
			{
				int i = -1;
				InfoBox presetBox = new() { Color = Colors.Grey, BorderThickness = 1f, CurveRadius = 8f, ContentsAction = () => { Presets.DrawPreset(preset, info, ref i); } };
				presetBox.Draw();
				ImGuiHelpers.ScaledDummy(12.0f);
			}
		}

		internal static void DrawHeadingContents(string jobName, int i)
		{
			foreach ((CustomComboPreset preset,
				Attributes.CustomComboInfoAttribute info) in groupedPresets[jobName].Where(x => !PresetStorage.IsPvP(x.Preset)
																							 && !PresetStorage.IsBozja(x.Preset) &&
																								!PresetStorage.IsEureka(x.Preset)))
			{
				InfoBox presetBox = new() { Color = Colors.Grey, BorderThickness = 2f.Scale(), ContentsOffset = 5f.Scale(), ContentsAction = () => { Presets.DrawPreset(preset, info, ref i); } };

				if (Service.Configuration.HideConflictedCombos)
				{
					CustomComboPreset[] conflictOriginals = PresetStorage.GetConflicts(preset); // Presets that are contained within a ConflictedAttribute
					System.Collections.Generic.List<CustomComboPreset> conflictsSource = PresetStorage.GetAllConflicts();      // Presets with the ConflictedAttribute

					if (!conflictsSource.Where(x => x == preset).Any() || conflictOriginals.Length == 0)
					{
						presetBox.Draw();
						ImGuiHelpers.ScaledDummy(12.0f);
						continue;
					}

					if (conflictOriginals.Any(PresetStorage.IsEnabled))
					{
						_ = Service.Configuration.EnabledActions.Remove(preset);
						Service.Configuration.Save();
					}

					else
					{
						presetBox.Draw();

						continue;
					}
				}

				else
				{
					presetBox.Draw();
					ImGuiHelpers.ScaledDummy(12.0f);
				}
			}
		}

		internal static string? HeaderToOpen;
	}
}