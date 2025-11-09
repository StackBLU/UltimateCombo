using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Textures.TextureWraps;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using ECommons.ImGuiMethods;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UltimateCombo.Combos;
using UltimateCombo.Combos.General;
using UltimateCombo.Combos.PvE;
using UltimateCombo.Core;

namespace UltimateCombo.Window.Tabs;

internal class PvEWindow : ConfigWindow
{
    internal static bool HasToOpenJob = true;
    internal static string OpenJob = string.Empty;
    internal static string? HeaderToOpen;

    private static readonly Dictionary<string, byte[]> JobsByRole = new()
    {
        { "Tanks", new byte[] { PLD.JobID, WAR.JobID, DRK.JobID, GNB.JobID } },
        { "Melee", new byte[] { MNK.JobID, DRG.JobID, NIN.JobID, SAM.JobID, RPR.JobID, VPR.JobID, FSH.JobID, Common.JobID } },
        { "Healers", new byte[] { WHM.JobID, SCH.JobID, AST.JobID, SGE.JobID } },
        { "Physical Ranged", new byte[] { BRD.JobID, MCH.JobID, DNC.JobID } },
        { "Magical Ranged", new byte[] { BLM.JobID, SMN.JobID, RDM.JobID, PCT.JobID, BLU.JobID } }
    };

    private static readonly string[][] ColumnLayout =
    [
        ["Tanks", "Melee"],
        ["Healers", "Physical Ranged", "Magical Ranged"]
    ];

    internal static new void Draw()
    {
        var i = 1;
        var indentwidth = 12f.Scale();
        var indentwidth2 = indentwidth + 42f.Scale();

        if (OpenJob == string.Empty)
        {
            var availableWidth = ImGui.GetContentRegionAvail().X;
            var columnWidth = availableWidth / 2f;

            ImGui.BeginGroup();
            _ = ImGui.BeginChild("LeftColumn", new Vector2(columnWidth, 0), false);

            foreach (var roleName in ColumnLayout[0])
            {
                if (!JobsByRole.ContainsKey(roleName))
                {
                    continue;
                }

                foreach (var jobId in JobsByRole[roleName])
                {
                    List<(Presets Preset, Attributes.CustomComboInfoAttribute Info)>? jobPresets = GroupedPresets.Values
                        .FirstOrDefault(list => list.First().Info.JobID == jobId);

                    if (jobPresets == null)
                    {
                        continue;
                    }

                    var jobName = jobPresets.First().Info.JobName;
                    var abbreviation = jobPresets.First().Info.JobShorthand;
                    var header = string.IsNullOrEmpty(abbreviation) ? jobName : $"{jobName} - {abbreviation}";
                    IDalamudTextureWrap? icon = Icons.GetJobIcon(jobId);
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
                }
            }

            ImGui.EndChild();
            ImGui.EndGroup();

            ImGui.SameLine();

            ImGui.BeginGroup();
            _ = ImGui.BeginChild("RightColumn", new Vector2(columnWidth, 0), false);

            foreach (var roleName in ColumnLayout[1])
            {
                if (!JobsByRole.ContainsKey(roleName))
                {
                    continue;
                }

                foreach (var jobId in JobsByRole[roleName])
                {
                    List<(Presets Preset, Attributes.CustomComboInfoAttribute Info)>? jobPresets = GroupedPresets.Values
                        .FirstOrDefault(list => list.First().Info.JobID == jobId);

                    if (jobPresets == null)
                    {
                        continue;
                    }

                    var jobName = jobPresets.First().Info.JobName;
                    var abbreviation = jobPresets.First().Info.JobShorthand;
                    var header = string.IsNullOrEmpty(abbreviation) ? jobName : $"{jobName} - {abbreviation}";
                    IDalamudTextureWrap? icon = Icons.GetJobIcon(jobId);
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
                }
            }

            ImGui.EndChild();
            ImGui.EndGroup();
        }
        else
        {
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
                List<Presets> conflictsSource = PresetStorage.GetAllConflicts();

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
