using Dalamud.Interface.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using UltimateCombo.Attributes;
using UltimateCombo.Combos;
using UltimateCombo.Core;

namespace UltimateCombo.Window.Tabs;

internal static class PresetTab
{
    internal static void DrawContent(Func<Presets, bool> filter)
    {
        if (!ConfigWindow.GroupedPresets.TryGetValue("General", out List<(Presets Preset, CustomComboInfoAttribute Info)>? presets))
        {
            return;
        }

        var index = 1;

        foreach ((Presets preset, CustomComboInfoAttribute info) in presets.Where(item => filter(item.Preset)))
        {
            index = DrawPresetBox(preset, info, index);
        }
    }

    internal static int DrawPresetBox(
        Presets preset,
        CustomComboInfoAttribute info,
        int index,
        bool hideEnabledConflicts = false)
    {
        if (hideEnabledConflicts
            && Service.Configuration.HideConflictedCombos
            && PresetStorage.GetConflicts(preset).Any(PresetStorage.IsEnabled))
        {
            if (Service.Configuration.EnabledActions.Remove(preset))
            {
                Service.Configuration.Save();
            }

            return index;
        }

        var currentIndex = index;
        var scale = ImGuiHelpers.GlobalScale;
        var presetBox = new InfoBox
        {
            Color = Colors.Grey,
            BorderThickness = 1f * scale,
            CurveRadius = 8f * scale,
            ContentsOffset = 5f * scale,
            ContentsAction = () => PresetHandler.DrawPreset(preset, info, ref currentIndex)
        };

        presetBox.Draw();
        ImGuiHelpers.ScaledDummy(12f);
        return currentIndex;
    }
}
