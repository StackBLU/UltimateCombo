
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Textures.TextureWraps;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using ECommons.ImGuiMethods;
using System;
using System.Collections.Generic;
using System.Numerics;
using UltimateCombo.Attributes;
using UltimateCombo.Combos;

namespace UltimateCombo.Window.Tabs;

internal static class JobTab
{
    internal readonly record struct JobRole(string Name, byte[] JobIDs);

    internal static void Draw(
        JobRole[][] columns,
        Func<Presets, bool> presetFilter,
        ref string openJob,
        Action<string> drawContents)
    {
        if (string.IsNullOrEmpty(openJob))
        {
            DrawJobSelector(columns, presetFilter, ref openJob);
            return;
        }

        if (!ConfigWindow.GroupedPresets.TryGetValue(openJob, out List<(Presets Preset, CustomComboInfoAttribute Info)>? presets) || presets.Count == 0)
        {
            openJob = string.Empty;
            return;
        }

        DrawJobHeader(presets[0].Info.JobID, ref openJob);

        if (string.IsNullOrEmpty(openJob))
        {
            return;
        }

        using ImRaii.ChildDisposable contents = ImRaii.Child("Contents", Vector2.Zero, false);
        drawContents(openJob);
    }

    private static void DrawJobSelector(
        JobRole[][] columns,
        Func<Presets, bool> presetFilter,
        ref string openJob)
    {
        if (columns.Length != 2)
        {
            return;
        }

        var spacing = ImGui.GetStyle().ItemSpacing.X;
        var columnWidth = (ImGui.GetContentRegionAvail().X - spacing) / 2f;

        DrawJobColumn("LeftColumn", columns[0], columnWidth, presetFilter, ref openJob);
        ImGui.SameLine();
        DrawJobColumn("RightColumn", columns[1], columnWidth, presetFilter, ref openJob);
    }

    private static void DrawJobColumn(
        string childID,
        IReadOnlyList<JobRole> roles,
        float width,
        Func<Presets, bool> presetFilter,
        ref string openJob)
    {
        using ImRaii.ChildDisposable child = ImRaii.Child(childID, new Vector2(width, 0), false);

        foreach (JobRole role in roles)
        {
            ImGui.TextColored(Colors.Orange, role.Name);

            foreach (var jobID in role.JobIDs)
            {
                List<(Presets Preset, CustomComboInfoAttribute Info)>? jobPresets = FindJobPresets(jobID, presetFilter);
                if (jobPresets is null)
                {
                    continue;
                }

                DrawJobRow(jobID, jobPresets[0].Info, ref openJob);
            }

            ImGui.Spacing();
        }
    }

    private static List<(Presets Preset, CustomComboInfoAttribute Info)>? FindJobPresets(
        byte jobID,
        Func<Presets, bool> presetFilter)
    {
        if (ConfigWindow.PresetsByJobID.TryGetValue(jobID, out List<(Presets Preset, CustomComboInfoAttribute Info)>? presets)
            && presets.Exists(item => presetFilter(item.Preset)))
        {
            return presets;
        }

        return null;
    }

    private static void DrawJobRow(
        byte jobID,
        CustomComboInfoAttribute info,
        ref string openJob)
    {
        var header = string.IsNullOrEmpty(info.JobShorthand)
            ? info.JobName
            : $"{info.JobName} - {info.JobShorthand}";

        var scale = ImGuiHelpers.GlobalScale;
        IDalamudTextureWrap? icon = Icons.GetJobIcon(jobID);
        Vector2 iconSize = icon is null
            ? Vector2.Zero
            : icon.Size * (scale / 2f);

        var rowHeight = Math.Max(iconSize.Y, ImGui.GetTextLineHeight())
            + (ImGui.GetStyle().FramePadding.Y * 2f);

        Vector2 rowStart = ImGui.GetCursorScreenPos();

        if (ImGui.Selectable($"##Job_{jobID}", openJob == info.JobName, ImGuiSelectableFlags.None, new Vector2(0, rowHeight)))
        {
            openJob = info.JobName;
        }

        Vector2 nextRow = ImGui.GetCursorScreenPos();
        var contentX = rowStart.X + (12f * scale);

        if (icon is not null)
        {
            ImGui.SetCursorScreenPos(new Vector2(contentX, rowStart.Y + ((rowHeight - iconSize.Y) / 2f)));
            ImGui.Image(icon.Handle, iconSize);
        }

        var textX = contentX + (34f * scale);
        var textY = rowStart.Y + ((rowHeight - ImGui.GetTextLineHeight()) / 2f);
        ImGui.SetCursorScreenPos(new Vector2(textX, textY));
        ImGui.TextUnformatted(header);
        ImGui.SetCursorScreenPos(nextRow);
    }

    private static void DrawJobHeader(byte jobID, ref string openJob)
    {
        var scale = ImGuiHelpers.GlobalScale;
        IDalamudTextureWrap? icon = Icons.GetJobIcon(jobID);
        var iconHeight = icon is null ? 0 : icon.Size.Y * scale / 2f;
        var childHeight = Math.Max(24f * scale, iconHeight + (4f * scale));
        var jobName = openJob;

        using ImRaii.ChildDisposable heading = ImRaii.Child(
            "HeadingTab",
            new Vector2(ImGui.GetContentRegionAvail().X, childHeight),
            false);

        if (ImGui.Button("Back", new Vector2(0, 24f * scale)))
        {
            openJob = string.Empty;
            return;
        }

        ImGui.SameLine();
        ImGuiEx.LineCentered(() =>
        {
            if (icon is not null)
            {
                Vector2 iconSize = icon.Size * (scale / 2f);
                ImGui.Image(icon.Handle, iconSize);
                ImGui.SameLine();
            }

            ImGui.TextUnformatted(jobName);
        });
    }
}

