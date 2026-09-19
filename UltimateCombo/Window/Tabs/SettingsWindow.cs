
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using System;
using System.Numerics;
using UltimateCombo.Core;

namespace UltimateCombo.Window.Tabs;

internal class SettingsWindow : ConfigWindow
{
    internal static new void Draw()
    {
        PvEWindow.HasToOpenJob = true;

        using ImRaii.ChildDisposable main = ImRaii.Child("SettingsMain", Vector2.Zero, true);

        DrawQuickGuide();
        DrawControllerGuide();
        DrawSettings();
    }

    private static void DrawQuickGuide()
    {
        ImGui.TextUnformatted("Quick Guide");
        ImGui.Separator();
        ImGui.TextWrapped(
            "If an option is enabled, it will appear in the rotation when its conditions are met. "
            + "Some actions require particular buffs, timings, HP values, or other conditions, so an action may not appear immediately.\n\n"
            + "Each combo's trigger actions are shown underneath its toggle. Only those actions will trigger the combo.");
        ImGui.Spacing();
    }

    private static void DrawControllerGuide()
    {
        ImGui.TextUnformatted("Controller Controls (PlayStation / Xbox)");
        ImGui.Separator();
        ImGui.TextUnformatted(
            "L1 + L3 - Dalamud interface\n"
            + "D-pad - Navigate window\n"
            + "X / A - Select\n"
            + "Circle / B - Back\n"
            + "Square / X (Hold) - Select window\n"
            + "    L1 / R1 - Up / Down\n"
            + "    D-pad - Resize window\n"
            + "    Left stick - Move window");
        ImGui.Spacing();
    }

    private static void DrawSettings()
    {
        ImGui.TextUnformatted("Settings");
        ImGui.Separator();

        var openOnLaunch = Service.Configuration.OpenOnLaunch;
        if (ImGui.Checkbox("Open on launch", ref openOnLaunch))
        {
            Service.Configuration.OpenOnLaunch = openOnLaunch;
            Service.Configuration.Save();
        }

        var hideChildren = Service.Configuration.HideChildren;
        if (ImGui.Checkbox("Hide nested options", ref hideChildren))
        {
            Service.Configuration.HideChildren = hideChildren;
            Service.Configuration.Save();
        }

        var hideConflicting = Service.Configuration.HideConflictedCombos;
        if (ImGui.Checkbox("Hide conflicting combos", ref hideConflicting))
        {
            Service.Configuration.HideConflictedCombos = hideConflicting;
            Service.Configuration.Save();
        }

        var showInternalNames = Service.Configuration.ShowInternalNames;
        if (ImGui.Checkbox("Show internal names", ref showInternalNames))
        {
            Service.Configuration.ShowInternalNames = showInternalNames;
            Service.Configuration.Save();
        }

        var ignoreGCDChecks = Service.Configuration.IgnoreGCDChecks;
        if (ImGui.Checkbox("Ignore GCD checks", ref ignoreGCDChecks))
        {
            Service.Configuration.IgnoreGCDChecks = ignoreGCDChecks;
            Service.Configuration.Save();
            Service.DtrBarEntry.Text = $"GCD Counting  {(ignoreGCDChecks ? "X" : "✓")}";
        }

        if (ImGui.IsItemHovered())
        {
            ImGui.SetTooltip("Combos will begin using abilities immediately.");
        }

        var disableTripleWeaving = Service.Configuration.DisableTripleWeaving;
        if (ImGui.Checkbox("Disable triple weaving", ref disableTripleWeaving))
        {
            Service.Configuration.DisableTripleWeaving = disableTripleWeaving;
            Service.Configuration.Save();
        }

        ImGui.Spacing();
        ImGui.TextWrapped("Minimum distance from the target before melee jobs use ranged attacks. Default: 7. Minimum: 3.");

        var rangedAttackRange = Service.Configuration.RangedAttackRange;
        ImGui.SetNextItemWidth(150f * ImGuiHelpers.GlobalScale);

        if (ImGui.InputDouble("##RangedAttackRange", ref rangedAttackRange, 1))
        {
            Service.Configuration.RangedAttackRange = Math.Max(3, rangedAttackRange);
        }

        if (ImGui.IsItemDeactivatedAfterEdit())
        {
            Service.Configuration.Save();
        }
    }
}
