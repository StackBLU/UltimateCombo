using Dalamud.Interface.Textures.TextureWraps;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using ECommons.ImGuiMethods;
using Dalamud.Bindings.ImGui;
using System.Linq;
using System.Numerics;
using UltimateCombo.Combos;
using UltimateCombo.Core;
using UltimateCombo.Services;
using UltimateCombo.Window.Functions;

namespace UltimateCombo.Window.Tabs
{
    internal class PvEWindow : ConfigWindow
    {
        internal static bool HasToOpenJob = true;
        internal static string OpenJob = string.Empty;

        internal static new void Draw()
        {
            var i = 1;
            var indentwidth = 12f.Scale();
            var indentwidth2 = indentwidth + 42f.Scale();

            if (OpenJob == string.Empty)
            {
                foreach (var jobName in GroupedPresets.Keys)
                {
                    var abbreviation = GroupedPresets[jobName].First().Info.JobShorthand;
                    string header;

                    if (string.IsNullOrEmpty(abbreviation))
                    {
                        header = jobName;
                    }

                    else
                    {
                        header = $"{jobName} - {abbreviation}";
                    }

                    var id = GroupedPresets[jobName].First().Info.JobID;

                    IDalamudTextureWrap? icon = Icons.GetJobIcon(id);

                    Vector2 selectableSize;

                    if (icon == null)
                    {
                        selectableSize = new Vector2(0);
                    }

                    else
                    {
                        selectableSize = new Vector2(0, (icon.Size.Y / 2f).Scale());
                    }

                    if (ImGui.Selectable($"###{header}", OpenJob == jobName, ImGuiSelectableFlags.None, selectableSize))
                    {
                        OpenJob = jobName;
                    }

                    ImGui.SameLine(indentwidth);

                    if (icon != null)
                    {
                        ImGui.Image(icon.Handle, new Vector2(icon.Size.X.Scale(), icon.Size.Y.Scale()) / 2f);
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
                var id = GroupedPresets[OpenJob].First().Info.JobID;
                IDalamudTextureWrap? icon = Icons.GetJobIcon(id);

                float childHeight;

                if (icon is null)
                {
                    childHeight = 24f.Scale();
                }

                else
                {
                    childHeight = (icon.Size.Y / 2f).Scale() + 4f;
                }

                using (ImRaii.IEndObject headingTab = ImRaii.Child("HeadingTab",
                    new Vector2(ImGui.GetContentRegionAvail().X, childHeight)))
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
                            ImGui.Image(icon.Handle, new Vector2(icon.Size.X.Scale(), icon.Size.Y.Scale()) / 2f);
                            ImGui.SameLine();
                        }
                        ImGuiEx.Text($"{OpenJob}");
                    });
                }

                using ImRaii.IEndObject contents = ImRaii.Child("Contents", new Vector2(0), false);
                try
                {
                    DrawHeadingContents(OpenJob, i);
                }
                catch { }
            }
        }

        internal static void DrawHeadingContents(string jobName, int i)
        {
            foreach ((CustomComboPreset preset,
                Attributes.CustomComboInfoAttribute info) in GroupedPresets[jobName].Where(x => !PresetStorage.IsPvP(x.Preset)
                                                                                             && !PresetStorage.IsBozja(x.Preset)
                                                                                             && !PresetStorage.IsEureka(x.Preset)
                                                                                             && !PresetStorage.IsVariant(x.Preset)
                                                                                             && !PresetStorage.IsOccult(x.Preset)))
            {
                InfoBox presetBox = new()
                {
                    Color = Colors.Grey,
                    BorderThickness = 2f.Scale(),
                    ContentsOffset = 5f.Scale(),
                    ContentsAction = () =>
                    {
                        Presets.DrawPreset(preset, info, ref i);
                    }
                };

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
