using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Colors;
using Dalamud.Utility;
using ECommons.DalamudServices;
using ECommons.ImGuiMethods;
using System.Linq;
using System.Numerics;
using System.Text;
using UltimateCombo.Attributes;
using UltimateCombo.Combos;
using UltimateCombo.Core;
using UltimateCombo.Data;
using UltimateCombo.Extensions;
using UltimateCombo.Services;

namespace UltimateCombo.Window.Functions
{
	internal class Presets : ConfigWindow
	{
		internal static unsafe void DrawPreset(CustomComboPreset preset, CustomComboInfoAttribute info, ref int i)
		{
			var enabled = PresetStorage.IsEnabled(preset);
			var secret = PresetStorage.IsPvP(preset);
			CustomComboPreset[] conflicts = PresetStorage.GetConflicts(preset);
			CustomComboPreset? parent = PresetStorage.GetParent(preset);
			BlueInactiveAttribute? blueAttr = preset.GetAttribute<BlueInactiveAttribute>();

			if (ImGui.Checkbox($"{info.FancyName}###{info.FancyName}{i}", ref enabled))
			{
				if (enabled)
				{
					EnableParentPresets(preset);
					_ = Service.Configuration.EnabledActions.Add(preset);
					foreach (CustomComboPreset conflict in conflicts)
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

			DrawReplaceAttribute(preset);

			if (conflicts.Length > 0)
			{
				ImGui.TextColored(ImGuiColors.DalamudRed, "Conflicts with:");
				StringBuilder conflictBuilder = new();
				ImGui.Indent();
				foreach (CustomComboPreset conflict in conflicts)
				{
					CustomComboInfoAttribute? comboInfo = conflict.GetAttribute<CustomComboInfoAttribute>();
					_ = conflictBuilder.Insert(0, $"{comboInfo?.FancyName}");
					CustomComboPreset par2 = conflict;

					while (PresetStorage.GetParent(par2) != null)
					{
						CustomComboPreset? subpar = PresetStorage.GetParent(par2);
						_ = conflictBuilder.Insert(0, $"{subpar?.GetAttribute<CustomComboInfoAttribute>()?.FancyName} -> ");
						par2 = subpar!.Value;

					}

					if (!string.IsNullOrEmpty(comboInfo?.JobShorthand))
					{
						_ = conflictBuilder.Insert(0, $"[{comboInfo.JobShorthand}] ");
					}

					Vector4 color;

					if (ComboHelper.Functions.CustomComboFunctions.IsEnabled(conflict))
					{
						color = GradientColor.Get(ImGuiColors.DalamudRed, ImGuiColors.HealerGreen, 1500);
					}
					else
					{
						color = GradientColor.Get(ImGuiColors.DalamudRed, ImGuiColors.DalamudRed, 1500);
					}

					ImGuiEx.Text(color, $"- {conflictBuilder}");
					_ = conflictBuilder.Clear();
				}
				ImGui.Unindent();
				ImGui.Spacing();
			}

			if (blueAttr != null)
			{
				if (blueAttr.Actions.Count > 0)
				{
					Vector4 textColor;
					if (blueAttr.NoneSet)
					{
						textColor = ImGuiColors.DPSRed;
					}

					else
					{
						textColor = ImGuiColors.DalamudOrange;
					}
					ImGui.PushStyleColor(ImGuiCol.Text, textColor);

					string displayText;

					if (blueAttr.NoneSet)
					{
						displayText = "No Required Spells Active:";
					}

					else
					{
						displayText = "Missing active spells:";
					}

					ImGui.Text($"{displayText} {string.Join(", ", blueAttr.Actions.Select(x => ActionWatching.GetBLUIndex(x) + ActionWatching.GetActionName(x)))}");
					ImGui.PopStyleColor();
				}

				else
				{
					ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.HealerGreen);
					ImGui.Text($"All required spells active!");
					ImGui.PopStyleColor();
				}
			}

			UserConfigItems.Draw(preset, enabled);

			i++;

			var hideChildren = Service.Configuration.HideChildren;
			(CustomComboPreset Preset, CustomComboInfoAttribute Info)[] children = PresetChildren[preset];

			if (children.Length > 0)
			{
				if (enabled || !hideChildren)
				{
					ImGui.Indent();
					ImGui.Indent();

					foreach ((CustomComboPreset childPreset, CustomComboInfoAttribute childInfo) in children)
					{
						if (Service.Configuration.HideConflictedCombos)
						{
							CustomComboPreset[] conflictOriginals = PresetStorage.GetConflicts(childPreset);
							System.Collections.Generic.List<CustomComboPreset> conflictsSource = PresetStorage.GetAllConflicts();

							if (!conflictsSource.Where(x => x == childPreset || x == preset).Any() || conflictOriginals.Length == 0)
							{
								DrawPreset(childPreset, childInfo, ref i);
								continue;
							}

							if (conflictOriginals.Any(PresetStorage.IsEnabled))
							{
								_ = Service.Configuration.EnabledActions.Remove(childPreset);
								Service.Configuration.Save();
							}

							else
							{
								DrawPreset(childPreset, childInfo, ref i);
								continue;
							}
						}

						else
						{
							DrawPreset(childPreset, childInfo, ref i);
						}
					}

					ImGui.Unindent();
					ImGui.Unindent();
				}

				else
				{
					i += AllChildren(PresetChildren[preset]);
				}
			}
		}

		private static void DrawReplaceAttribute(CustomComboPreset preset)
		{
			ReplaceSkillAttribute? att = preset.GetReplaceAttribute();
			if (att != null)
			{
				ImGui.Spacing();
				for (var i = 0; i < att.ActionIcons.Count; i++)
				{
					Dalamud.Interface.Textures.TextureWraps.IDalamudTextureWrap img = Svc.Texture.GetFromGameIcon(new(att.ActionIcons[i])).GetWrapOrEmpty();
					ImGui.Image(img.Handle, img.Size / 2f * ImGui.GetIO().FontGlobalScale);
					ImGui.SameLine();
					if (i == 8)
					{
						ImGui.Spacing();
					}

					if (ImGui.IsItemHovered())
					{
						ImGui.BeginTooltip();
						ImGui.Text(att.ActionNames[i]);
						ImGui.EndTooltip();
					}
				}
				ImGui.Spacing();
				ImGui.Spacing();
			}
		}

		internal static int AllChildren((CustomComboPreset Preset, CustomComboInfoAttribute Info)[] children)
		{
			var output = 0;

			foreach ((CustomComboPreset Preset, CustomComboInfoAttribute Info) in children)
			{
				output++;
				output += AllChildren(PresetChildren[Preset]);
			}

			return output;
		}

		private static void EnableParentPresets(CustomComboPreset preset)
		{
			CustomComboPreset? parentMaybe = PresetStorage.GetParent(preset);

			while (parentMaybe != null)
			{
				CustomComboPreset parent = parentMaybe.Value;

				if (!Service.Configuration.EnabledActions.Contains(parent))
				{
					_ = Service.Configuration.EnabledActions.Add(parent);
					foreach (CustomComboPreset conflict in PresetStorage.GetConflicts(parent))
					{
						_ = Service.Configuration.EnabledActions.Remove(conflict);
					}
				}

				parentMaybe = PresetStorage.GetParent(parent);
			}
		}
	}
}
