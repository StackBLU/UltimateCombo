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
    private static readonly uint[] JobOrder =
    [
        PLD.JobID, WAR.JobID, DRK.JobID, GNB.JobID,
        WHM.JobID, SCH.JobID, AST.JobID, SGE.JobID,
        MNK.JobID, DRG.JobID, NIN.JobID, SAM.JobID, RPR.JobID, VPR.JobID,
        BST.JobID,
        BRD.JobID, MCH.JobID, DNC.JobID,
        BLM.JobID, SMN.JobID, RDM.JobID, PCT.JobID,
        BLU.JobID, FSH.JobID
    ];

    internal static readonly Dictionary<string, List<(Presets Preset, CustomComboInfoAttribute Info)>> GroupedPresets = GetGroupedPresets();
    internal static readonly Dictionary<byte, List<(Presets Preset, CustomComboInfoAttribute Info)>> PresetsByJobID = GroupedPresets.Values
        .Where(presets => presets.Count > 0)
        .GroupBy(presets => presets[0].Info.JobID)
        .ToDictionary(
            group => group.Key,
            group => group.First());

    internal static readonly Dictionary<Presets, (Presets Preset, CustomComboInfoAttribute Info)[]> PresetChildren = GetPresetChildren();

    internal OpenWindow OpenWindow { get; set; } = OpenWindow.PvE;

    internal ConfigWindow() : base($"UltimateCombo v{P?.GetType().Assembly.GetName().Version}")
    {
        RespectCloseHotkey = true;
        SizeCondition = ImGuiCond.FirstUseEver;
        Size = new Vector2(700, 1000);
        SetMinSize();

        Svc.PluginInterface.UiBuilder.DefaultFontHandle.ImFontChanged += SetMinSize;
    }

    internal static Dictionary<string, List<(Presets Preset, CustomComboInfoAttribute Info)>> GetGroupedPresets()
    {
        return Enum.GetValues<Presets>()
            .Select(preset => (Preset: preset, Info: preset.GetAttribute<CustomComboInfoAttribute>()))
            .Where(item => item.Info != null && PresetStorage.GetParent(item.Preset) == null)
            .Select(item => (item.Preset, Info: item.Info!))
            .OrderBy(item => GetJobOrder(item.Info.JobID))
            .ThenBy(item => GetRoleOrder(item.Info.Role))
            .ThenBy(item => item.Info.ClassJobCategory)
            .ThenBy(item => item.Info.JobName)
            .GroupBy(item => item.Info.JobName)
            .ToDictionary(group => group.Key, group => group.ToList());
    }

    internal static Dictionary<Presets, (Presets Preset, CustomComboInfoAttribute Info)[]> GetPresetChildren()
    {
        Presets[] presets = Enum.GetValues<Presets>();
        Dictionary<Presets, List<Presets>> childCombos = presets.ToDictionary(
            preset => preset,
            _ => new List<Presets>());

        foreach (Presets preset in presets)
        {
            Presets? parent = PresetStorage.GetParent(preset);
            if (parent != null)
            {
                childCombos[parent.Value].Add(preset);
            }
        }

        return childCombos.ToDictionary(
            pair => pair.Key,
            pair => pair.Value
                .Select(preset => (Preset: preset, Info: preset.GetAttribute<CustomComboInfoAttribute>()))
                .Where(item => item.Info != null)
                .Select(item => (item.Preset, Info: item.Info!))
                .ToArray());
    }

    public override void Draw()
    {
        Vector2 region = ImGui.GetContentRegionAvail();
        var leftSideHeight = region.Y;

        using ImRaii.StyleDisposable tableStyle = ImRaii.PushStyle(ImGuiStyleVar.CellPadding, new Vector2(4, 0));
        using ImRaii.TableDisposable table = ImRaii.Table("###MainTable", 2, ImGuiTableFlags.BordersInnerV);
        if (!table)
        {
            return;
        }

        ImGui.TableSetupColumn("##LeftColumn", ImGuiTableColumnFlags.WidthFixed, ImGui.GetWindowWidth() / 6f);
        _ = ImGui.TableNextColumn();
        Vector2 leftRegion = ImGui.GetContentRegionAvail();

        using (ImRaii.StyleDisposable selectableStyle = ImRaii.PushStyle(ImGuiStyleVar.SelectableTextAlign, new Vector2(0.5f, 0.5f)))
        using (ImRaii.ChildDisposable leftChild = ImRaii.Child("###LeftSide", leftRegion with { Y = leftSideHeight }, false))
        {
            DrawNavigationItem("PvE", OpenWindow.PvE);
            DrawNavigationItem("PvP", OpenWindow.PvP);
            DrawNavigationItem("Eureka", OpenWindow.Eureka);
            DrawNavigationItem("Bozja", OpenWindow.Bozja);
            DrawNavigationItem("Occult", OpenWindow.Occult);
            DrawNavigationItem("Variant", OpenWindow.Variant);
            DrawNavigationItem("Settings & Help", OpenWindow.Settings);
            DrawNavigationItem("Debug", OpenWindow.Debug);

            ImGuiEx.LineCentered(() =>
            {
                ImGui.SetCursorPosY(ImGui.GetWindowHeight() - 30f);
                if (ImGui.Button("Close"))
                {
                    Toggle();
                }
            });
        }

        _ = ImGui.TableNextColumn();
        using ImRaii.ChildDisposable rightChild = ImRaii.Child("###RightSide", Vector2.Zero, false);

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
            case OpenWindow.About:
            default:
                break;
        }
    }

    internal void Dispose()
    {
        Svc.PluginInterface.UiBuilder.DefaultFontHandle.ImFontChanged -= SetMinSize;
    }

    private static int GetJobOrder(uint jobId)
    {
        var index = Array.IndexOf(JobOrder, jobId);
        return index >= 0 ? index : int.MaxValue;
    }

    private static int GetRoleOrder(int role)
    {
        return role switch
        {
            1 => 0,
            4 => 1,
            2 => 2,
            3 => 3,
            _ => int.MaxValue
        };
    }

    private void DrawNavigationItem(string label, OpenWindow window)
    {
        if (ImGui.Selectable(label, OpenWindow == window))
        {
            OpenWindow = window;
        }

        ImGui.Spacing();
    }

    private void SetMinSize(IFontHandle? fontHandle = null, ILockedImFont? lockedFont = null)
    {
        SizeConstraints = new()
        {
            MinimumSize = new Vector2(700f.Scale(), 10f.Scale())
        };
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
