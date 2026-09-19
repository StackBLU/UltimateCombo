
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Colors;
using Dalamud.Utility;
using ECommons.DalamudServices;
using ECommons.ImGuiMethods;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UltimateCombo.Attributes;
using UltimateCombo.Combos;
using UltimateCombo.Core;
using UltimateCombo.Data;

namespace UltimateCombo.Window;

internal class PresetHandler : ConfigWindow
{
    private static readonly Dictionary<Presets, string> PresetPathCache = [];

    internal static void DrawPreset(Presets preset, CustomComboInfoAttribute info, ref int i)
    {
        var enabled = PresetStorage.IsEnabled(preset);
        Presets[] conflicts = PresetStorage.GetConflicts(preset);
        BlueInactiveAttribute? blueAttr = preset.GetAttribute<BlueInactiveAttribute>();
        var internalName = preset.ToString();

        if (ImGui.Checkbox($"{info.FancyName}###{internalName}", ref enabled))
        {
            if (enabled)
            {
                EnableParentPresets(preset);
                _ = Service.Configuration.EnabledActions.Add(preset);

                foreach (Presets conflict in conflicts)
                {
                    _ = Service.Configuration.EnabledActions.Remove(conflict);
                }
            }
            else
            {
                _ = Service.Configuration.EnabledActions.Remove(preset);
            }

            Service.Configuration.Save();
        }

        if (Service.Configuration.ShowInternalNames)
        {
            ImGui.SameLine();
            ImGui.PushStyleColor(ImGuiCol.Text, new Vector4(0.45f, 0.45f, 0.45f, 1f));
            ImGui.TextUnformatted($"- {internalName}");
            ImGui.PopStyleColor();
        }

        DrawReplaceAttribute(preset);

        if (conflicts.Length > 0)
        {
            ImGui.TextColored(ImGuiColors.DalamudRed, "Conflicts with:");
            ImGui.Indent();

            foreach (Presets conflict in conflicts)
            {
                Vector4 color = PresetStorage.IsEnabled(conflict)
                    ? GradientColor.Get(ImGuiColors.DalamudRed, ImGuiColors.HealerGreen, 1500)
                    : ImGuiColors.DalamudRed;

                ImGuiEx.Text(color, $"- {GetPresetPath(conflict)}");
            }

            ImGui.Unindent();
            ImGui.Spacing();
        }

        if (blueAttr != null)
        {
            List<uint> missingActions = blueAttr.Actions;

            if (missingActions.Count > 0)
            {
                Vector4 textColor = blueAttr.NoneSet ? ImGuiColors.DPSRed : ImGuiColors.DalamudOrange;
                var displayText = blueAttr.NoneSet ? "No required spells active:" : "Missing active spells:";
                var actions = string.Join(", ", missingActions.Select(action => ActionWatching.GetBLUIndex(action) + ActionWatching.GetActionName(action)));

                ImGui.PushStyleColor(ImGuiCol.Text, textColor);
                ImGui.TextUnformatted($"{displayText} {actions}");
                ImGui.PopStyleColor();
            }
            else
            {
                ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.HealerGreen);
                ImGui.TextUnformatted("All required spells active!");
                ImGui.PopStyleColor();
            }
        }

        UserConfigItems.Draw(preset, enabled);
        i++;

        (Presets Preset, CustomComboInfoAttribute Info)[] children = PresetChildren[preset];
        if (children.Length == 0)
        {
            return;
        }

        if (!enabled && Service.Configuration.HideChildren)
        {
            i += AllChildren(children);
            return;
        }

        ImGui.Indent();
        ImGui.Indent();

        foreach ((Presets childPreset, CustomComboInfoAttribute childInfo) in children)
        {
            Presets[] childConflicts = PresetStorage.GetConflicts(childPreset);
            var hideForConflict = Service.Configuration.HideConflictedCombos && childConflicts.Any(PresetStorage.IsEnabled);

            if (hideForConflict)
            {
                if (Service.Configuration.EnabledActions.Remove(childPreset))
                {
                    Service.Configuration.Save();
                }

                continue;
            }

            DrawPreset(childPreset, childInfo, ref i);
        }

        ImGui.Unindent();
        ImGui.Unindent();
    }

    private static string GetPresetPath(Presets preset)
    {
        if (PresetPathCache.TryGetValue(preset, out string? path))
        {
            return path;
        }

        List<string> names = [];
        CustomComboInfoAttribute? comboInfo = preset.GetAttribute<CustomComboInfoAttribute>();
        Presets? current = preset;

        while (current != null)
        {
            CustomComboInfoAttribute? currentInfo = current.Value.GetAttribute<CustomComboInfoAttribute>();
            names.Insert(0, currentInfo?.FancyName ?? current.Value.ToString());
            current = PresetStorage.GetParent(current.Value);
        }

        var jobPrefix = string.IsNullOrEmpty(comboInfo?.JobShorthand) ? "" : $"[{comboInfo.JobShorthand}] ";
        path = jobPrefix + string.Join(" -> ", names);
        PresetPathCache[preset] = path;
        return path;
    }

    private static void DrawReplaceAttribute(Presets preset)
    {
        ReplaceSkillAttribute? attribute = preset.GetReplaceAttribute();
        if (attribute == null)
        {
            return;
        }

        ImGui.Spacing();

        for (var i = 0; i < attribute.ActionIcons.Count; i++)
        {
            Dalamud.Interface.Textures.TextureWraps.IDalamudTextureWrap image = Svc.Texture
                .GetFromGameIcon(new(attribute.ActionIcons[i]))
                .GetWrapOrEmpty();

            ImGui.Image(image.Handle, image.Size / 2f * ImGui.GetIO().FontGlobalScale);

            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                ImGui.TextUnformatted(attribute.ActionNames[i]);
                ImGui.EndTooltip();
            }

            if (i == 8)
            {
                ImGui.NewLine();
            }
            else
            {
                ImGui.SameLine();
            }
        }

        ImGui.Spacing();
        ImGui.Spacing();
    }

    internal static int AllChildren((Presets Preset, CustomComboInfoAttribute Info)[] children)
    {
        var output = 0;

        foreach ((Presets preset, _) in children)
        {
            output++;
            output += AllChildren(PresetChildren[preset]);
        }

        return output;
    }

    private static void EnableParentPresets(Presets preset)
    {
        Presets? parentMaybe = PresetStorage.GetParent(preset);

        while (parentMaybe != null)
        {
            Presets parent = parentMaybe.Value;

            if (Service.Configuration.EnabledActions.Add(parent))
            {
                foreach (Presets conflict in PresetStorage.GetConflicts(parent))
                {
                    _ = Service.Configuration.EnabledActions.Remove(conflict);
                }
            }

            parentMaybe = PresetStorage.GetParent(parent);
        }
    }
}

internal static class PresetExtensions
{
    private static readonly Dictionary<Presets, ReplaceSkillAttribute?> ReplaceSkillCache = [];

    internal static ReplaceSkillAttribute? GetReplaceAttribute(this Presets preset)
    {
        if (!ReplaceSkillCache.TryGetValue(preset, out ReplaceSkillAttribute? attribute))
        {
            attribute = preset.GetAttribute<ReplaceSkillAttribute>();
            ReplaceSkillCache[preset] = attribute;
        }

        return attribute;
    }
}

