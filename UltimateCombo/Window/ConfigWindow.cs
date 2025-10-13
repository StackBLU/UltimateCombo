using Dalamud.Bindings.ImGui;
using Dalamud.Interface.ManagedFontAtlas;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Utility;
using ECommons.DalamudServices;
using ECommons.ImGuiMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UltimateCombo.Attributes;
using UltimateCombo.Combos;
using UltimateCombo.Combos.General;
using UltimateCombo.Combos.PvE;
using UltimateCombo.Core;
using UltimateCombo.Window.Tabs;

namespace UltimateCombo.Window;

internal class ConfigWindow : Dalamud.Interface.Windowing.Window
{
    internal static readonly Dictionary<string, List<(Presets Preset, CustomComboInfoAttribute Info)>> GroupedPresets = GetGroupedPresets();
    internal static readonly Dictionary<Presets, (Presets Preset, CustomComboInfoAttribute Info)[]> PresetChildren = GetPresetChildren();

    internal static Dictionary<string, List<(Presets Preset, CustomComboInfoAttribute Info)>> GetGroupedPresets()
    {
        return Enum
            .GetValues<Presets>()
            .Select(preset => (Preset: preset, Info: preset.GetAttribute<CustomComboInfoAttribute>()))
            .Where(tpl => tpl.Info != null && PresetStorage.GetParent(tpl.Preset) == null)
            .OrderByDescending(tpl => tpl.Info!.JobID == 0)
            .ThenByDescending(tpl => tpl.Info!.JobID == PLD.JobID)
            .ThenByDescending(tpl => tpl.Info!.JobID == WAR.JobID)
            .ThenByDescending(tpl => tpl.Info!.JobID == DRK.JobID)
            .ThenByDescending(tpl => tpl.Info!.JobID == GNB.JobID)
            .ThenByDescending(tpl => tpl.Info!.JobID == WHM.JobID)
            .ThenByDescending(tpl => tpl.Info!.JobID == SCH.JobID)
            .ThenByDescending(tpl => tpl.Info!.JobID == AST.JobID)
            .ThenByDescending(tpl => tpl.Info!.JobID == SGE.JobID)
            .ThenByDescending(tpl => tpl.Info!.JobID == MNK.JobID)
            .ThenByDescending(tpl => tpl.Info!.JobID == DRG.JobID)
            .ThenByDescending(tpl => tpl.Info!.JobID == NIN.JobID)
            .ThenByDescending(tpl => tpl.Info!.JobID == SAM.JobID)
            .ThenByDescending(tpl => tpl.Info!.JobID == RPR.JobID)
            .ThenByDescending(tpl => tpl.Info!.JobID == VPR.JobID)
            .ThenByDescending(tpl => tpl.Info!.JobID == BRD.JobID)
            .ThenByDescending(tpl => tpl.Info!.JobID == MCH.JobID)
            .ThenByDescending(tpl => tpl.Info!.JobID == DNC.JobID)
            .ThenByDescending(tpl => tpl.Info!.JobID == BLM.JobID)
            .ThenByDescending(tpl => tpl.Info!.JobID == SMN.JobID)
            .ThenByDescending(tpl => tpl.Info!.JobID == RDM.JobID)
            .ThenByDescending(tpl => tpl.Info!.JobID == PCT.JobID)
            .ThenByDescending(tpl => tpl.Info!.JobID == BLU.JobID)
            .ThenByDescending(tpl => tpl.Info!.JobID == FSH.JobID)
            .ThenByDescending(tpl => tpl.Info!.Role == 1)
            .ThenByDescending(tpl => tpl.Info!.Role == 4)
            .ThenByDescending(tpl => tpl.Info!.Role == 2)
            .ThenByDescending(tpl => tpl.Info!.Role == 3)
            .ThenBy(tpl => tpl.Info!.ClassJobCategory)
            .ThenBy(tpl => tpl.Info!.JobName)
            .ThenBy(tpl => tpl.Info!.Order)
            .GroupBy(tpl => tpl.Info!.JobName)
            .ToDictionary(
                tpl => tpl.Key,
                tpl => tpl.ToList())!;
    }

    internal static Dictionary<Presets, (Presets Preset, CustomComboInfoAttribute Info)[]> GetPresetChildren()
    {
        Dictionary<Presets, List<Presets>> childCombos = Enum.GetValues<Presets>().ToDictionary(
            tpl => tpl,
            tpl => new List<Presets>());

        foreach (Presets preset in Enum.GetValues<Presets>())
        {
            Presets? parent = preset.GetAttribute<ParentComboAttribute>()?.ParentPreset;
            if (parent != null)
            {
                childCombos[parent.Value].Add(preset);
            }
        }

        return childCombos.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value
                .Select(preset => (Preset: preset, Info: preset.GetAttribute<CustomComboInfoAttribute>()))
                .Where(tpl => tpl.Info != null)
                .OrderBy(tpl => tpl.Info!.Order).ToArray())!;
    }

    internal OpenWindow OpenWindow { get; set; } = OpenWindow.PvE;

    internal ConfigWindow() : base($"UltimateCombo v{P?.GetType().Assembly.GetName().Version}")
    {
        RespectCloseHotkey = true;

        SizeCondition = ImGuiCond.FirstUseEver;
        Size = new Vector2(700, 1000);
        SetMinSize();

        Svc.PluginInterface.UiBuilder.DefaultFontHandle.ImFontChanged += SetMinSize;
    }

    private void SetMinSize(IFontHandle? fontHandle = null, ILockedImFont? lockedFont = null)
    {
        SizeConstraints = new()
        {
            MinimumSize = new Vector2(700f.Scale(), 10f.Scale())
        };
    }

    public override void Draw()
    {
        Vector2 region = ImGui.GetContentRegionAvail();
        var topLeftSideHeight = region.Y;

        using ImRaii.Style style = ImRaii.PushStyle(ImGuiStyleVar.CellPadding, new Vector2(4, 0));
        using ImRaii.IEndObject table = ImRaii.Table("###MainTable", 2, ImGuiTableFlags.BordersInnerV);
        if (!table)
        {
            return;
        }

        ImGui.TableSetupColumn("##LeftColumn", ImGuiTableColumnFlags.WidthFixed, ImGui.GetWindowWidth() / 6);
        _ = ImGui.TableNextColumn();
        Vector2 regionSize = ImGui.GetContentRegionAvail();
        ImGui.PushStyleVar(ImGuiStyleVar.SelectableTextAlign, new Vector2(0.5f, 0.5f));

        using (ImRaii.IEndObject leftChild = ImRaii.Child($"###LeftSide", regionSize with { Y = topLeftSideHeight }, false))
        {
            if (ImGui.Selectable("PvE", OpenWindow == OpenWindow.PvE))
            {
                OpenWindow = OpenWindow.PvE;
            }

            ImGui.Spacing();

            if (ImGui.Selectable("PvP", OpenWindow == OpenWindow.PvP))
            {
                OpenWindow = OpenWindow.PvP;
            }

            ImGui.Spacing();

            if (ImGui.Selectable("Eureka", OpenWindow == OpenWindow.Eureka))
            {
                OpenWindow = OpenWindow.Eureka;
            }

            ImGui.Spacing();

            if (ImGui.Selectable("Bozja", OpenWindow == OpenWindow.Bozja))
            {
                OpenWindow = OpenWindow.Bozja;
            }

            ImGui.Spacing();

            if (ImGui.Selectable("Occult", OpenWindow == OpenWindow.Occult))
            {
                OpenWindow = OpenWindow.Occult;
            }

            ImGui.Spacing();

            if (ImGui.Selectable("Variant", OpenWindow == OpenWindow.Variant))
            {
                OpenWindow = OpenWindow.Variant;
            }

            ImGui.Spacing();

            if (ImGui.Selectable("Settings & Help", OpenWindow == OpenWindow.Settings))
            {
                OpenWindow = OpenWindow.Settings;
            }

            ImGui.Spacing();

            if (ImGui.Selectable("Debug", OpenWindow == OpenWindow.Debug))
            {
                OpenWindow = OpenWindow.Debug;
            }

            ImGuiEx.LineCentered(() =>
            {
                ImGui.SetCursorPosY(ImGui.GetWindowHeight() - 30);
                if (ImGui.Button("Close"))
                {
                    Toggle();
                }
            });
        }

        ImGui.PopStyleVar();
        _ = ImGui.TableNextColumn();
        using ImRaii.IEndObject rightChild = ImRaii.Child($"###RightSide", Vector2.Zero, false);
        switch (OpenWindow)
        {
            case OpenWindow.PvE:
                PvEWindow.Draw();
                break;
            case OpenWindow.PvP:
                PvPWindow.Draw();
                break;
            case OpenWindow.Eureka:
                EurekaWindow.Draw();
                break;
            case OpenWindow.Bozja:
                BozjaWindow.Draw();
                break;
            case OpenWindow.Occult:
                OccultWindow.Draw();
                break;
            case OpenWindow.Variant:
                VariantWindow.Draw();
                break;
            case OpenWindow.Settings:
                SettingsWindow.Draw();
                break;
            case OpenWindow.Debug:
                DebugWindow.Draw();
                break;
            case OpenWindow.None:
                break;
            case OpenWindow.About:
                break;
            default:
                break;
        }
    }

    internal void Dispose()
    {
        Svc.PluginInterface.UiBuilder.DefaultFontHandle.ImFontChanged -= SetMinSize;
    }
}

internal enum OpenWindow
{
    None = 0,
    PvE = 1,
    PvP = 2,
    Eureka = 3,
    Bozja = 4,
    Occult = 5,
    Variant = 6,
    Settings = 7,
    About = 8,
    Debug = 9
}
