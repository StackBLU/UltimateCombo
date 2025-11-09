using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Textures.TextureWraps;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using ECommons.ImGuiMethods;
using System.Linq;
using System.Numerics;
using UltimateCombo.Combos;
using UltimateCombo.Core;

namespace UltimateCombo.Window.Tabs;

internal class PvEWindow : ConfigWindow
{
    internal static bool HasToOpenJob = true;
    internal static string OpenJob = string.Empty;
    internal static string? HeaderToOpen;

    internal static new void Draw()
    {
        var i = 1;
        var indentwidth = 12f.Scale();
        var indentwidth2 = indentwidth + 42f.Scale();

        if (OpenJob == string.Empty)
        {
            // start two-column layout
            ImGui.Columns(2, "preset_columns", false);

            foreach (var jobName in GroupedPresets.Keys)
            {
                var abbreviation = GroupedPresets[jobName].First().Info.JobShorthand;
                var header = string.IsNullOrEmpty(abbreviation) ? jobName : $"{jobName} - {abbreviation}";
                var id = GroupedPresets[jobName].First().Info.JobID;
                IDalamudTextureWrap? icon = Icons.GetJobIcon(id);
                Vector2 selectableSize = icon == null ? new Vector2(0) : new Vector2(0, (icon.Size.Y / 2f).Scale());

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

                // move to next column after drawing this item
                ImGui.NextColumn();
            }

            // restore single column mode
            ImGui.Columns(1);
        }
        else
        {
            // ensure column state is reset when opening a job
            ImGui.Columns(1);

            var id = GroupedPresets[OpenJob].First().Info.JobID;
            IDalamudTextureWrap? icon = Icons.GetJobIcon(id);
            var childHeight = icon is null ? 24f.Scale() : (icon.Size.Y / 2f).Scale() + 4f;

            using (ImRaii.IEndObject headingTab = ImRaii.Child("HeadingTab", new Vector2(ImGui.GetContentRegionAvail().X, childHeight)))
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
        foreach ((Presets preset, Attributes.CustomComboInfoAttribute info) in GroupedPresets[jobName].Where(x => !PresetStorage.IsPvP(x.Preset)
                                                                         && !PresetStorage.IsBozja(x.Preset)
                                                                         && !PresetStorage.IsEureka(x.Preset)
                                                                         && !PresetStorage.IsVariant(x.Preset)
                                                                         && !PresetStorage.IsOccult(x.Preset)))
        {
            var presetBox = new InfoBox()
            {
                Color = Colors.Grey,
                BorderThickness = 2f.Scale(),
                ContentsOffset = 5f.Scale(),
                ContentsAction = () =>
                {
                    PresetHandler.DrawPreset(preset, info, ref i);
                }
            };

            if (Service.Configuration.HideConflictedCombos)
            {
                Presets[] conflictOriginals = PresetStorage.GetConflicts(preset);
                System.Collections.Generic.List<Presets> conflictsSource = PresetStorage.GetAllConflicts();

                if (!conflictsSource.Any(x => x == preset) || conflictOriginals.Length == 0)
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
}
