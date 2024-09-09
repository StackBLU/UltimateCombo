﻿using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Interface;
using Dalamud.Interface.Colors;
using Dalamud.Utility;
using ImGuiNET;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Combos;
using UltimateCombo.Combos.PvE;
using UltimateCombo.Combos.PvP;
using UltimateCombo.Core;
using UltimateCombo.Services;
using System;
using System.Numerics;

namespace UltimateCombo.Window.Functions
{
	public static class UserConfig
	{
		public static void DrawSliderInt(int minValue, int maxValue, string config, string sliderDescription, float itemWidth = 150, uint sliderIncrement = SliderIncrements.Ones, bool hasAdditionalChoice = false, string additonalChoiceCondition = "")
		{
			ImGui.Indent();
			int output = PluginConfiguration.GetCustomIntValue(config, minValue);
			if (output < minValue)
			{
				output = minValue;
				PluginConfiguration.SetCustomIntValue(config, output);
				Service.Configuration.Save();
			}

			sliderDescription = sliderDescription.Replace("%", "%%");
			float contentRegionMin = ImGui.GetItemRectMax().Y - ImGui.GetItemRectMin().Y;
			float wrapPos = ImGui.GetContentRegionMax().X - 35f;

			InfoBox box = new()
			{
				Color = Colors.White,
				BorderThickness = 1f,
				CurveRadius = 3f,
				AutoResize = true,
				HasMaxWidth = true,
				IsSubBox = true,
				ContentsAction = () =>
				{
					bool inputChanged = false;
					Vector2 currentPos = ImGui.GetCursorPos();
					ImGui.SetCursorPosX(currentPos.X + itemWidth);
					ImGui.PushTextWrapPos(wrapPos);
					ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DalamudWhite);
					ImGui.Text($"{sliderDescription}");
					Vector2 height = ImGui.GetItemRectSize();
					float lines = height.Y / ImGui.GetFontSize();
					Vector2 textLength = ImGui.CalcTextSize(sliderDescription);
					string newLines = "";
					for (int i = 1; i < lines; i++)
					{
						if (i % 2 == 0)
						{
							newLines += "\n";
						}
						else
						{
							newLines += "\n\n";
						}

					}

					if (hasAdditionalChoice)
					{
						ImGui.SameLine();
						ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.HealerGreen);
						ImGui.PushFont(UiBuilder.IconFont);
						ImGui.Dummy(new Vector2(5, 0));
						ImGui.SameLine();
						ImGui.TextWrapped($"{FontAwesomeIcon.Search.ToIconString()}");
						ImGui.PopFont();
						ImGui.PopStyleColor();

						if (ImGui.IsItemHovered())
						{
							ImGui.BeginTooltip();
							ImGui.TextUnformatted($"This setting has additional options depending on its value.{(string.IsNullOrEmpty(additonalChoiceCondition) ? "" : $"\nCondition: {additonalChoiceCondition}")}");
							ImGui.EndTooltip();
						}
					}

					ImGui.PopStyleColor();
					ImGui.PopTextWrapPos();
					ImGui.SameLine();
					ImGui.SetCursorPosX(currentPos.X);
					ImGui.PushItemWidth(itemWidth);
					inputChanged |= ImGui.SliderInt($"{newLines}###{config}", ref output, minValue, maxValue);

					if (inputChanged)
					{
						if (output % sliderIncrement != 0)
						{
							output = output.RoundOff(sliderIncrement);
							if (output < minValue)
							{
								output = minValue;
							}

							if (output > maxValue)
							{
								output = maxValue;
							}
						}

						PluginConfiguration.SetCustomIntValue(config, output);
						Service.Configuration.Save();
					}
				}
			};

			box.Draw();
			ImGui.Spacing();
			ImGui.Unindent();
		}

		public static void DrawSliderFloat(float minValue, float maxValue, string config, string sliderDescription, float itemWidth = 150, bool hasAdditionalChoice = false, string additonalChoiceCondition = "")
		{
			float output = PluginConfiguration.GetCustomFloatValue(config, minValue);
			if (output < minValue)
			{
				output = minValue;
				PluginConfiguration.SetCustomFloatValue(config, output);
				Service.Configuration.Save();
			}

			sliderDescription = sliderDescription.Replace("%", "%%");
			float contentRegionMin = ImGui.GetItemRectMax().Y - ImGui.GetItemRectMin().Y;
			float wrapPos = ImGui.GetContentRegionMax().X - 35f;


			InfoBox box = new()
			{
				Color = Colors.White,
				BorderThickness = 1f,
				CurveRadius = 3f,
				AutoResize = true,
				HasMaxWidth = true,
				IsSubBox = true,
				ContentsAction = () =>
				{
					bool inputChanged = false;
					Vector2 currentPos = ImGui.GetCursorPos();
					ImGui.SetCursorPosX(currentPos.X + itemWidth);
					ImGui.PushTextWrapPos(wrapPos);
					ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DalamudWhite);
					ImGui.Text($"{sliderDescription}");
					Vector2 height = ImGui.GetItemRectSize();
					float lines = height.Y / ImGui.GetFontSize();
					Vector2 textLength = ImGui.CalcTextSize(sliderDescription);
					string newLines = "";
					for (int i = 1; i < lines; i++)
					{
						if (i % 2 == 0)
						{
							newLines += "\n";
						}
						else
						{
							newLines += "\n\n";
						}

					}

					if (hasAdditionalChoice)
					{
						ImGui.SameLine();
						ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.HealerGreen);
						ImGui.PushFont(UiBuilder.IconFont);
						ImGui.Dummy(new Vector2(5, 0));
						ImGui.SameLine();
						ImGui.TextWrapped($"{FontAwesomeIcon.Search.ToIconString()}");
						ImGui.PopFont();
						ImGui.PopStyleColor();

						if (ImGui.IsItemHovered())
						{
							ImGui.BeginTooltip();
							ImGui.TextUnformatted($"This setting has additional options depending on its value.{(string.IsNullOrEmpty(additonalChoiceCondition) ? "" : $"\nCondition: {additonalChoiceCondition}")}");
							ImGui.EndTooltip();
						}
					}

					ImGui.PopStyleColor();
					ImGui.PopTextWrapPos();
					ImGui.SameLine();
					ImGui.SetCursorPosX(currentPos.X);
					ImGui.PushItemWidth(itemWidth);
					inputChanged |= ImGui.SliderFloat($"{newLines}###{config}", ref output, minValue, maxValue);

					if (inputChanged)
					{
						PluginConfiguration.SetCustomFloatValue(config, output);
						Service.Configuration.Save();
					}
				}
			};

			box.Draw();
			ImGui.Spacing();
		}

		public static void DrawRoundedSliderFloat(float minValue, float maxValue, string config, string sliderDescription, float itemWidth = 150, bool hasAdditionalChoice = false, string additonalChoiceCondition = "", int digits = 1)
		{
			float output = PluginConfiguration.GetCustomFloatValue(config, minValue);
			if (output < minValue)
			{
				output = minValue;
				PluginConfiguration.SetCustomFloatValue(config, output);
				Service.Configuration.Save();
			}

			sliderDescription = sliderDescription.Replace("%", "%%");
			float contentRegionMin = ImGui.GetItemRectMax().Y - ImGui.GetItemRectMin().Y;
			float wrapPos = ImGui.GetContentRegionMax().X - 35f;


			InfoBox box = new()
			{
				Color = Colors.White,
				BorderThickness = 1f,
				CurveRadius = 3f,
				AutoResize = true,
				HasMaxWidth = true,
				IsSubBox = true,
				ContentsAction = () =>
				{
					bool inputChanged = false;
					Vector2 currentPos = ImGui.GetCursorPos();
					ImGui.SetCursorPosX(currentPos.X + itemWidth);
					ImGui.PushTextWrapPos(wrapPos);
					ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DalamudWhite);
					ImGui.Text($"{sliderDescription}");
					Vector2 height = ImGui.GetItemRectSize();
					float lines = height.Y / ImGui.GetFontSize();
					Vector2 textLength = ImGui.CalcTextSize(sliderDescription);
					string newLines = "";
					for (int i = 1; i < lines; i++)
					{
						if (i % 2 == 0)
						{
							newLines += "\n";
						}
						else
						{
							newLines += "\n\n";
						}

					}

					if (hasAdditionalChoice)
					{
						ImGui.SameLine();
						ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.HealerGreen);
						ImGui.PushFont(UiBuilder.IconFont);
						ImGui.Dummy(new Vector2(5, 0));
						ImGui.SameLine();
						ImGui.TextWrapped($"{FontAwesomeIcon.Search.ToIconString()}");
						ImGui.PopFont();
						ImGui.PopStyleColor();

						if (ImGui.IsItemHovered())
						{
							ImGui.BeginTooltip();
							ImGui.TextUnformatted($"This setting has additional options depending on its value.{(string.IsNullOrEmpty(additonalChoiceCondition) ? "" : $"\nCondition: {additonalChoiceCondition}")}");
							ImGui.EndTooltip();
						}
					}

					ImGui.PopStyleColor();
					ImGui.PopTextWrapPos();
					ImGui.SameLine();
					ImGui.SetCursorPosX(currentPos.X);
					ImGui.PushItemWidth(itemWidth);
					inputChanged |= ImGui.SliderFloat($"{newLines}###{config}", ref output, minValue, maxValue, $"%.{digits}f");

					if (inputChanged)
					{
						PluginConfiguration.SetCustomFloatValue(config, output);
						Service.Configuration.Save();
					}
				}
			};

			box.Draw();
			ImGui.Spacing();
		}

		public static void DrawRadioButton(string config, string checkBoxName, string checkboxDescription, int outputValue, float itemWidth = 150, Vector4 descriptionColor = new Vector4())
		{
			ImGui.Indent();
			if (descriptionColor == new Vector4())
			{
				descriptionColor = ImGuiColors.DalamudYellow;
			}

			int output = PluginConfiguration.GetCustomIntValue(config, outputValue);
			ImGui.PushItemWidth(itemWidth);
			ImGui.SameLine();
			ImGui.Dummy(new Vector2(21, 0));
			ImGui.SameLine();
			bool enabled = output == outputValue;

			if (ImGui.RadioButton($"{checkBoxName}###{config}{outputValue}", enabled))
			{
				PluginConfiguration.SetCustomIntValue(config, outputValue);
				Service.Configuration.Save();
			}

			if (!checkboxDescription.IsNullOrEmpty())
			{
				ImGui.PushStyleColor(ImGuiCol.Text, descriptionColor);
				ImGui.TextWrapped(checkboxDescription);
				ImGui.PopStyleColor();
			}

			ImGui.Unindent();
			ImGui.Spacing();
		}

		public static void DrawHorizontalRadioButton(string config, string checkBoxName, string checkboxDescription, int outputValue, float itemWidth = 150, Vector4 descriptionColor = new Vector4())
		{
			if (descriptionColor == new Vector4())
			{
				descriptionColor = ImGuiColors.DalamudYellow;
			}

			int output = PluginConfiguration.GetCustomIntValue(config);
			ImGui.SameLine();
			ImGui.PushItemWidth(itemWidth);
			bool enabled = output == outputValue;

			ImGui.PushStyleColor(ImGuiCol.Text, descriptionColor);
			if (ImGui.RadioButton($"{checkBoxName}###{config}{outputValue}", enabled))
			{
				PluginConfiguration.SetCustomIntValue(config, outputValue);
				Service.Configuration.Save();
			}

			if (!checkboxDescription.IsNullOrEmpty() && ImGui.IsItemHovered())
			{
				ImGui.BeginTooltip();
				ImGui.TextUnformatted(checkboxDescription);
				ImGui.EndTooltip();
			}
			ImGui.PopStyleColor();

			ImGui.SameLine();
			ImGui.Dummy(new Vector2(16f, 0));
		}

		public static void DrawAdditionalBoolChoice(string config, string checkBoxName, string checkboxDescription, float itemWidth = 150, bool isConditionalChoice = false)
		{
			bool output = PluginConfiguration.GetCustomBoolValue(config);
			ImGui.PushItemWidth(itemWidth);
			if (!isConditionalChoice)
			{
				ImGui.Indent();
			}
			else
			{
				ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.HealerGreen);
				ImGui.PushFont(UiBuilder.IconFont);
				ImGui.AlignTextToFramePadding();
				ImGui.TextWrapped($"{FontAwesomeIcon.Plus.ToIconString()}");
				ImGui.PopFont();
				ImGui.PopStyleColor();
				ImGui.SameLine();
				ImGui.Dummy(new Vector2(3));
				ImGui.SameLine();
				if (isConditionalChoice)
				{
					ImGui.Indent();
				}
			}
			if (ImGui.Checkbox($"{checkBoxName}###{config}", ref output))
			{
				PluginConfiguration.SetCustomBoolValue(config, output);
				Service.Configuration.Save();
			}

			if (!checkboxDescription.IsNullOrEmpty())
			{
				ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DalamudGrey);
				ImGui.TextWrapped(checkboxDescription);
				ImGui.PopStyleColor();
			}

			ImGui.Unindent();
			ImGui.Spacing();
		}

		public static void DrawHorizontalMultiChoice(string config, string checkBoxName, string checkboxDescription, int totalChoices, int choice, Vector4 descriptionColor = new Vector4())
		{
			ImGui.Indent();
			if (descriptionColor == new Vector4())
			{
				descriptionColor = ImGuiColors.DalamudWhite;
			}

			bool[]? values = PluginConfiguration.GetCustomBoolArrayValue(config);

			if (values.Length == 0 || values.Length != totalChoices)
			{
				Array.Resize(ref values, totalChoices);
				PluginConfiguration.SetCustomBoolArrayValue(config, values);
				Service.Configuration.Save();
			}

			ImGui.PushStyleColor(ImGuiCol.Text, descriptionColor);
			if (choice > 0)
			{
				ImGui.SameLine();
				ImGui.Dummy(new Vector2(12f, 0));
				ImGui.SameLine();
			}

			if (ImGui.Checkbox($"{checkBoxName}###{config}{choice}", ref values[choice]))
			{
				PluginConfiguration.SetCustomBoolArrayValue(config, values);
				Service.Configuration.Save();
			}
			if (!checkboxDescription.IsNullOrEmpty() && ImGui.IsItemHovered())
			{
				ImGui.BeginTooltip();
				ImGui.TextUnformatted(checkboxDescription);
				ImGui.EndTooltip();
			}


			ImGui.PopStyleColor();
			ImGui.Unindent();
		}

		public static void DrawGridMultiChoice(string config, byte columns, string[,] nameAndDesc, Vector4 descriptionColor = new Vector4())
		{
			int totalChoices = nameAndDesc.GetLength(0);
			if (totalChoices > 0)
			{
				ImGui.Indent();
				if (descriptionColor == new Vector4())
				{
					descriptionColor = ImGuiColors.DalamudWhite;
				}
				bool[]? values = PluginConfiguration.GetCustomBoolArrayValue(config);

				if (values.Length == 0 || values.Length != totalChoices)
				{
					Array.Resize(ref values, totalChoices);
					PluginConfiguration.SetCustomBoolArrayValue(config, values);
					Service.Configuration.Save();
				}

				_ = ImGui.BeginTable($"Grid###{config}", columns);
				ImGui.TableNextRow();
				for (int idx = 0; idx < totalChoices; idx++)
				{
					_ = ImGui.TableNextColumn();
					string checkBoxName = nameAndDesc[idx, 0];
					string checkboxDescription = nameAndDesc[idx, 1];

					ImGui.PushStyleColor(ImGuiCol.Text, descriptionColor);
					if (ImGui.Checkbox($"{checkBoxName}###{config}{idx}", ref values[idx]))
					{
						PluginConfiguration.SetCustomBoolArrayValue(config, values);
						Service.Configuration.Save();
					}

					if (!checkboxDescription.IsNullOrEmpty() && ImGui.IsItemHovered())
					{
						ImGui.BeginTooltip();
						ImGui.TextUnformatted(checkboxDescription);
						ImGui.EndTooltip();
					}

					ImGui.PopStyleColor();

					if (((idx + 1) % columns) == 0)
					{
						ImGui.TableNextRow();
					}
				}
				ImGui.EndTable();
				ImGui.Unindent();
			}
		}

		public static void DrawPvPStatusMultiChoice(string config)
		{
			bool[]? values = PluginConfiguration.GetCustomBoolArrayValue(config);

			ImGui.Columns(4, $"{config}", false);

			if (values.Length == 0)
			{
				Array.Resize(ref values, 7);
			}

			ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.ParsedPink);

			if (ImGui.Checkbox($"Stun###{config}0", ref values[0]))
			{
				PluginConfiguration.SetCustomBoolArrayValue(config, values);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();

			if (ImGui.Checkbox($"Deep Freeze###{config}1", ref values[1]))
			{
				PluginConfiguration.SetCustomBoolArrayValue(config, values);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();

			if (ImGui.Checkbox($"Half Asleep###{config}2", ref values[2]))
			{
				PluginConfiguration.SetCustomBoolArrayValue(config, values);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();

			if (ImGui.Checkbox($"Sleep###{config}3", ref values[3]))
			{
				PluginConfiguration.SetCustomBoolArrayValue(config, values);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();

			if (ImGui.Checkbox($"Bind###{config}4", ref values[4]))
			{
				PluginConfiguration.SetCustomBoolArrayValue(config, values);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();

			if (ImGui.Checkbox($"Heavy###{config}5", ref values[5]))
			{
				PluginConfiguration.SetCustomBoolArrayValue(config, values);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();

			if (ImGui.Checkbox($"Silence###{config}6", ref values[6]))
			{
				PluginConfiguration.SetCustomBoolArrayValue(config, values);
				Service.Configuration.Save();
			}

			ImGui.Columns(1);
			ImGui.PopStyleColor();
			ImGui.Spacing();
		}

		public static void DrawRoleGridMultiChoice(string config)
		{
			bool[]? values = PluginConfiguration.GetCustomBoolArrayValue(config);

			ImGui.Columns(5, $"{config}", false);

			if (values.Length == 0)
			{
				Array.Resize(ref values, 5);
			}

			ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.TankBlue);

			if (ImGui.Checkbox($"Tanks###{config}0", ref values[0]))
			{
				PluginConfiguration.SetCustomBoolArrayValue(config, values);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();
			ImGui.PopStyleColor();
			ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.HealerGreen);

			if (ImGui.Checkbox($"Healers###{config}1", ref values[1]))
			{
				PluginConfiguration.SetCustomBoolArrayValue(config, values);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();
			ImGui.PopStyleColor();
			ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DPSRed);

			if (ImGui.Checkbox($"Melee###{config}2", ref values[2]))
			{
				PluginConfiguration.SetCustomBoolArrayValue(config, values);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();
			ImGui.PopStyleColor();
			ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DalamudYellow);

			if (ImGui.Checkbox($"Ranged###{config}3", ref values[3]))
			{
				PluginConfiguration.SetCustomBoolArrayValue(config, values);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();
			ImGui.PopStyleColor();
			ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.ParsedPurple);

			if (ImGui.Checkbox($"Casters###{config}4", ref values[4]))
			{
				PluginConfiguration.SetCustomBoolArrayValue(config, values);
				Service.Configuration.Save();
			}

			ImGui.Columns(1);
			ImGui.PopStyleColor();
			ImGui.Spacing();
		}

		public static void DrawRoleGridSingleChoice(string config)
		{
			int value = PluginConfiguration.GetCustomIntValue(config);
			bool[] values = new bool[20];

			for (int i = 0; i <= 4; i++)
			{
				values[i] = value == i;
			}

			ImGui.Columns(5, $"{config}", false);

			if (values.Length == 0)
			{
				Array.Resize(ref values, 5);
			}

			ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.TankBlue);

			if (ImGui.Checkbox($"Tanks###{config}0", ref values[0]))
			{
				PluginConfiguration.SetCustomIntValue(config, 0);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();
			ImGui.PopStyleColor();
			ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.HealerGreen);

			if (ImGui.Checkbox($"Healers###{config}1", ref values[1]))
			{
				PluginConfiguration.SetCustomIntValue(config, 1);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();
			ImGui.PopStyleColor();
			ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DPSRed);

			if (ImGui.Checkbox($"Melee###{config}2", ref values[2]))
			{
				PluginConfiguration.SetCustomIntValue(config, 2);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();
			ImGui.PopStyleColor();
			ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DalamudYellow);

			if (ImGui.Checkbox($"Ranged###{config}3", ref values[3]))
			{
				PluginConfiguration.SetCustomIntValue(config, 3);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();
			ImGui.PopStyleColor();
			ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.ParsedPurple);

			if (ImGui.Checkbox($"Casters###{config}4", ref values[4]))
			{
				PluginConfiguration.SetCustomIntValue(config, 4);
				Service.Configuration.Save();
			}

			ImGui.Columns(1);
			ImGui.PopStyleColor();
			ImGui.Spacing();
		}

		public static void DrawJobGridMultiChoice(string config)
		{
			bool[]? values = PluginConfiguration.GetCustomBoolArrayValue(config);

			ImGui.Columns(5, $"{config}", false);

			if (values.Length == 0)
			{
				Array.Resize(ref values, 20);
			}

			ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.TankBlue);

			if (ImGui.Checkbox($"Paladin###{config}0", ref values[0]))
			{
				PluginConfiguration.SetCustomBoolArrayValue(config, values);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();

			if (ImGui.Checkbox($"Warrior###{config}1", ref values[1]))
			{
				PluginConfiguration.SetCustomBoolArrayValue(config, values);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();

			if (ImGui.Checkbox($"Dark Knight###{config}2", ref values[2]))
			{
				PluginConfiguration.SetCustomBoolArrayValue(config, values);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();

			if (ImGui.Checkbox($"Gunbreaker###{config}3", ref values[3]))
			{
				PluginConfiguration.SetCustomBoolArrayValue(config, values);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();
			ImGui.NextColumn();

			ImGui.PopStyleColor();
			ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.HealerGreen);

			if (ImGui.Checkbox($"White Mage###{config}", ref values[4]))
			{
				PluginConfiguration.SetCustomBoolArrayValue(config, values);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();

			if (ImGui.Checkbox($"Scholar###{config}5", ref values[5]))
			{
				PluginConfiguration.SetCustomBoolArrayValue(config, values);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();

			if (ImGui.Checkbox($"Astrologian###{config}6", ref values[6]))
			{
				PluginConfiguration.SetCustomBoolArrayValue(config, values);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();

			if (ImGui.Checkbox($"Sage###{config}7", ref values[7]))
			{
				PluginConfiguration.SetCustomBoolArrayValue(config, values);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();
			ImGui.NextColumn();

			ImGui.PopStyleColor();
			ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DPSRed);

			if (ImGui.Checkbox($"Monk###{config}8", ref values[8]))
			{
				PluginConfiguration.SetCustomBoolArrayValue(config, values);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();

			if (ImGui.Checkbox($"Dragoon###{config}9", ref values[9]))
			{
				PluginConfiguration.SetCustomBoolArrayValue(config, values);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();

			if (ImGui.Checkbox($"Ninja###{config}10", ref values[10]))
			{
				PluginConfiguration.SetCustomBoolArrayValue(config, values);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();

			if (ImGui.Checkbox($"Samurai###{config}11", ref values[11]))
			{
				PluginConfiguration.SetCustomBoolArrayValue(config, values);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();

			if (ImGui.Checkbox($"Reaper###{config}12", ref values[12]))
			{
				PluginConfiguration.SetCustomBoolArrayValue(config, values);
				Service.Configuration.Save();
			}

			ImGui.PopStyleColor();
			ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DalamudYellow);
			ImGui.NextColumn();

			if (ImGui.Checkbox($"Bard###{config}13", ref values[13]))
			{
				PluginConfiguration.SetCustomBoolArrayValue(config, values);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();

			if (ImGui.Checkbox($"Machinist###{config}14", ref values[14]))
			{
				PluginConfiguration.SetCustomBoolArrayValue(config, values);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();

			if (ImGui.Checkbox($"Dancer###{config}15", ref values[15]))
			{
				PluginConfiguration.SetCustomBoolArrayValue(config, values);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();
			ImGui.NextColumn();
			ImGui.NextColumn();

			ImGui.PopStyleColor();
			ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.ParsedPurple);

			if (ImGui.Checkbox($"Black Mage###{config}16", ref values[16]))
			{
				PluginConfiguration.SetCustomBoolArrayValue(config, values);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();

			if (ImGui.Checkbox($"Summoner###{config}17", ref values[17]))
			{
				PluginConfiguration.SetCustomBoolArrayValue(config, values);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();

			if (ImGui.Checkbox($"Red Mage###{config}18", ref values[18]))
			{
				PluginConfiguration.SetCustomBoolArrayValue(config, values);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();

			if (ImGui.Checkbox($"Blue Mage###{config}19", ref values[19]))
			{
				PluginConfiguration.SetCustomBoolArrayValue(config, values);
				Service.Configuration.Save();
			}

			ImGui.PopStyleColor();
			ImGui.NextColumn();
			ImGui.Columns(1);
			ImGui.Spacing();
		}

		public static void DrawJobGridSingleChoice(string config)
		{
			int value = PluginConfiguration.GetCustomIntValue(config);
			bool[] values = new bool[20];

			for (int i = 0; i <= 19; i++)
			{
				values[i] = value == i;
			}

			ImGui.Columns(5, null, false);
			ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.TankBlue);

			if (ImGui.Checkbox($"Paladin###{config}0", ref values[0]))
			{
				PluginConfiguration.SetCustomIntValue(config, 0);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();

			if (ImGui.Checkbox($"Warrior###{config}1", ref values[1]))
			{
				PluginConfiguration.SetCustomIntValue(config, 1);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();

			if (ImGui.Checkbox($"Dark Knight###{config}2", ref values[2]))
			{
				PluginConfiguration.SetCustomIntValue(config, 2);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();

			if (ImGui.Checkbox($"Gunbreaker###{config}3", ref values[3]))
			{
				PluginConfiguration.SetCustomIntValue(config, 3);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();
			ImGui.NextColumn();

			ImGui.PopStyleColor();
			ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.HealerGreen);

			if (ImGui.Checkbox($"White Mage###{config}4", ref values[4]))
			{
				PluginConfiguration.SetCustomIntValue(config, 4);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();

			if (ImGui.Checkbox($"Scholar###{config}5", ref values[5]))
			{
				PluginConfiguration.SetCustomIntValue(config, 5);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();

			if (ImGui.Checkbox($"Astrologian###{config}6", ref values[6]))
			{
				PluginConfiguration.SetCustomIntValue(config, 6);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();

			if (ImGui.Checkbox($"Sage###{config}7", ref values[7]))
			{
				PluginConfiguration.SetCustomIntValue(config, 7);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();
			ImGui.NextColumn();

			ImGui.PopStyleColor();
			ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DPSRed);

			if (ImGui.Checkbox($"Monk###{config}8", ref values[8]))
			{
				PluginConfiguration.SetCustomIntValue(config, 8);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();

			if (ImGui.Checkbox($"Dragoon###{config}9", ref values[9]))
			{
				PluginConfiguration.SetCustomIntValue(config, 9);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();

			if (ImGui.Checkbox($"Ninja###{config}10", ref values[10]))
			{
				PluginConfiguration.SetCustomIntValue(config, 10);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();

			if (ImGui.Checkbox($"Samurai###{config}11", ref values[11]))
			{
				PluginConfiguration.SetCustomIntValue(config, 11);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();

			if (ImGui.Checkbox($"Reaper###{config}12", ref values[12]))
			{
				PluginConfiguration.SetCustomIntValue(config, 12);
				Service.Configuration.Save();
			}

			ImGui.PopStyleColor();
			ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DalamudYellow);
			ImGui.NextColumn();

			if (ImGui.Checkbox($"Bard###{config}13", ref values[13]))
			{
				PluginConfiguration.SetCustomIntValue(config, 13);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();

			if (ImGui.Checkbox($"Machinist###{config}14", ref values[14]))
			{
				PluginConfiguration.SetCustomIntValue(config, 14);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();

			if (ImGui.Checkbox($"Dancer###{config}15", ref values[15]))
			{
				PluginConfiguration.SetCustomIntValue(config, 15);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();
			ImGui.NextColumn();
			ImGui.NextColumn();

			ImGui.PopStyleColor();
			ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.ParsedPurple);

			if (ImGui.Checkbox($"Black Mage###{config}16", ref values[16]))
			{
				PluginConfiguration.SetCustomIntValue(config, 16);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();

			if (ImGui.Checkbox($"Summoner###{config}17", ref values[17]))
			{
				PluginConfiguration.SetCustomIntValue(config, 17);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();

			if (ImGui.Checkbox($"Red Mage###{config}18", ref values[18]))
			{
				PluginConfiguration.SetCustomIntValue(config, 18);
				Service.Configuration.Save();
			}

			ImGui.NextColumn();

			if (ImGui.Checkbox($"Blue Mage###{config}19", ref values[19]))
			{
				PluginConfiguration.SetCustomIntValue(config, 19);
				Service.Configuration.Save();
			}

			ImGui.PopStyleColor();
			ImGui.NextColumn();
			ImGui.Columns(1);
			ImGui.Spacing();
		}

		internal static void DrawPriorityInput(UserIntArray config, int maxValues, int currentItem, string customLabel = "")
		{
			if (config.Count != maxValues || config.Any(x => x == 0))
			{
				config.Clear(maxValues);
				for (int i = 1; i <= maxValues; i++)
				{
					config[i - 1] = i;
				}
			}

			int curVal = config[currentItem];
			int oldVal = config[currentItem];

			InfoBox box = new()
			{
				Color = Colors.Blue,
				BorderThickness = 1f,
				CurveRadius = 3f,
				AutoResize = true,
				HasMaxWidth = true,
				IsSubBox = true,
				ContentsAction = () =>
				{
					if (customLabel.IsNullOrEmpty())
					{
						ImGui.TextUnformatted($"Priority: ");
					}
					else
					{
						ImGui.TextUnformatted(customLabel);
					}
					ImGui.SameLine();
					ImGui.PushItemWidth(100f);

					if (ImGui.InputInt($"###Priority{config.Name}{currentItem}", ref curVal))
					{
						for (int i = 0; i < maxValues; i++)
						{
							if (i == currentItem)
							{
								continue;
							}

							if (config[i] == curVal)
							{
								config[i] = oldVal;
								config[currentItem] = curVal;
								break;
							}
						}
					}
				}
			};

			ImGui.Indent();
			box.Draw();
			if (ImGui.IsItemHovered())
			{
				ImGui.BeginTooltip();
				ImGui.Text("Smaller Number = Higher Priority");
				ImGui.EndTooltip();
			}
			ImGui.Unindent();
			ImGui.Spacing();
		}

		public static int RoundOff(this int i, uint sliderIncrement)
		{
			double sliderAsDouble = Convert.ToDouble(sliderIncrement);
			return ((int)Math.Round(i / sliderAsDouble)) * (int)sliderIncrement;
		}
	}

	public static class UserConfigItems
	{
		internal static void Draw(CustomComboPreset preset, bool enabled)
		{
			if (!enabled)
			{
				return;
			}

			#region ASTROLOGIAN

			if (preset is CustomComboPreset.AST_ST_DPS_Lucid && enabled)
			{
				UserConfig.DrawSliderInt(1000, 10000, AST.Config.AST_ST_DPS_Lucid, "MP Threshold", 150, SliderIncrements.Hundreds);
			}

			if (preset is CustomComboPreset.AST_AoE_DPS_Lucid && enabled)
			{
				UserConfig.DrawSliderInt(1000, 10000, AST.Config.AST_AoE_DPS_Lucid, "MP Threshold", 150, SliderIncrements.Hundreds);
			}

			if (preset is CustomComboPreset.AST_ST_DPS_AutoDraw && enabled)
			{
				UserConfig.DrawAdditionalBoolChoice(AST.Config.AST_ST_DPS_OverwriteCards, "Overwrite Non-DPS Cards", "");
			}

			if (preset is CustomComboPreset.AST_AoE_DPS_AutoDraw && enabled)
			{
				UserConfig.DrawAdditionalBoolChoice(AST.Config.AST_AoE_DPS_OverwriteCards, "Overwrite Non-DPS Cards", "");
			}

			#endregion

			#region BLACK MAGE



			#endregion

			#region BLUE MAGE

			if (preset is CustomComboPreset.BLU_ManaGain && enabled)
			{
				UserConfig.DrawSliderInt(1000, 10000, BLU.Config.BLU_Lucid, "Lucid Dreaming MP Threshold", 150, SliderIncrements.Hundreds);
			}

			if (preset is CustomComboPreset.BLU_ManaGain && enabled)
			{
				UserConfig.DrawSliderInt(1000, 10000, BLU.Config.BLU_ManaGain, "Blood Drain MP Threshold", 150, SliderIncrements.Hundreds);
			}

			if (preset is CustomComboPreset.BLU_Treasure_Pomcure && enabled)
			{
				UserConfig.DrawSliderInt(1, 100, BLU.Config.BLU_TreasurePomcure, "Pomcure HP Threshold", 150, SliderIncrements.Fives);
			}

			if (preset is CustomComboPreset.BLU_Treasure_Gobskin && enabled)
			{
				UserConfig.DrawSliderInt(0, 100, BLU.Config.BLU_TreasureGobskin, "Shield Percentage Threshold", 150, SliderIncrements.Fives);
			}

			#endregion

			#region BARD



			#endregion

			#region DANCER



			#endregion

			#region DARK KNIGHT

			if (preset == CustomComboPreset.DRK_ST_Edge && enabled)
			{
				UserConfig.DrawSliderInt(0, 10000, DRK.Config.DRK_ST_ManaSaver, "", 150, SliderIncrements.Thousands);
			}

			if (preset == CustomComboPreset.DRK_AoE_Flood && enabled)
			{
				UserConfig.DrawSliderInt(0, 10000, DRK.Config.DRK_AoE_ManaSaver, "", 150, SliderIncrements.Thousands);
			}

			if (preset == CustomComboPreset.DRK_ST_Bloodspiller && enabled)
			{
				UserConfig.DrawSliderInt(50, 100, DRK.Config.DRK_BloodspillerGauge, "", 150, SliderIncrements.Fives);
			}

			if (preset == CustomComboPreset.DRK_AoE_Quietus && enabled)
			{
				UserConfig.DrawSliderInt(50, 100, DRK.Config.DRK_QuietusGauge, "", 150, SliderIncrements.Fives);
			}

			if (preset == CustomComboPreset.DRK_AoE_Abyssal && enabled)
			{
				UserConfig.DrawSliderInt(0, 100, DRK.Config.DRK_AoE_Abyssal, "", 150, SliderIncrements.Ones);
			}

			if (preset == CustomComboPreset.DRK_ST_Invuln && enabled)
			{
				UserConfig.DrawSliderInt(0, 100, DRK.Config.DRK_ST_Invuln, "", 150, SliderIncrements.Ones);
			}

			if (preset == CustomComboPreset.DRK_AoE_Invuln && enabled)
			{
				UserConfig.DrawSliderInt(0, 100, DRK.Config.DRK_AoE_Invuln, "", 150, SliderIncrements.Ones);
			}

			#endregion

			#region DRAGOON



			#endregion

			#region GUNBREAKER

			if (preset == CustomComboPreset.GNB_ST_Invuln && enabled)
			{
				UserConfig.DrawSliderInt(0, 100, GNB.Config.GNB_ST_Invuln, "", 150, SliderIncrements.Ones);
			}

			if (preset == CustomComboPreset.GNB_AoE_Invuln && enabled)
			{
				UserConfig.DrawSliderInt(0, 100, GNB.Config.GNB_AoE_Invuln, "", 150, SliderIncrements.Ones);
			}

			#endregion

			#region MACHINIST

			if (preset is CustomComboPreset.MCH_ST_Hypercharge && enabled)
			{
				UserConfig.DrawSliderInt(0, 100, MCH.Config.MCH_ST_Hypercharge, "Heat Threshold", 150, SliderIncrements.Fives);
			}

			if (preset is CustomComboPreset.MCH_ST_Queen && enabled)
			{
				UserConfig.DrawSliderInt(0, 100, MCH.Config.MCH_ST_Queen, "Battery Threshold", 150, SliderIncrements.Fives);
			}

			if (preset is CustomComboPreset.MCH_AoE_Hypercharge && enabled)
			{
				UserConfig.DrawSliderInt(0, 100, MCH.Config.MCH_AoE_Hypercharge, "Heat Threshold", 150, SliderIncrements.Fives);
			}

			#endregion

			#region MONK



			#endregion

			#region NINJA

			#region PvP

			if (preset is CustomComboPreset.NINPvP_Huton && enabled)
			{
				UserConfig.DrawSliderInt(0, 100, NINPvP.Config.NINPvP_Huton, "Triggers when you have more than this % of HP", 150, SliderIncrements.Fives);
			}

			if (preset is CustomComboPreset.NINPvP_Meisui && enabled)
			{
				UserConfig.DrawSliderInt(0, 100, NINPvP.Config.NINPvP_Meisui, "Triggers when you have less than this % of HP", 150, SliderIncrements.Fives);
			}

			#endregion

			#endregion

			#region PICTOMANCER

			if (preset is CustomComboPreset.PCT_ST_Lucid && enabled)
			{
				UserConfig.DrawSliderInt(1000, 10000, PCT.Config.PCT_ST_Lucid, "MP Threshold", 150, SliderIncrements.Hundreds);
			}

			if (preset is CustomComboPreset.PCT_AoE_Lucid && enabled)
			{
				UserConfig.DrawSliderInt(1000, 10000, PCT.Config.PCT_AoE_Lucid, "MP Threshold", 150, SliderIncrements.Hundreds);
			}

			#endregion

			#region PALADIN

			if (preset == CustomComboPreset.PLD_ST_Sheltron && enabled)
			{
				UserConfig.DrawSliderInt(50, 100, PLD.Config.PLD_ST_Sheltron, "Oath Gauge", 150, SliderIncrements.Fives);
			}

			if (preset == CustomComboPreset.PLD_ST_Intervention && enabled)
			{
				UserConfig.DrawSliderInt(50, 100, PLD.Config.PLD_ST_Intervention, "Oath Gauge", 150, SliderIncrements.Fives);
			}

			if (preset == CustomComboPreset.PLD_AoE_Sheltron && enabled)
			{
				UserConfig.DrawSliderInt(50, 100, PLD.Config.PLD_AoE_Sheltron, "Oath Gauge", 150, SliderIncrements.Fives);
			}

			if (preset == CustomComboPreset.PLD_AoE_Intervention && enabled)
			{
				UserConfig.DrawSliderInt(50, 100, PLD.Config.PLD_AoE_Intervention, "Oath Gauge", 150, SliderIncrements.Fives);
			}

			if (preset == CustomComboPreset.PLD_ST_Invuln && enabled)
			{
				UserConfig.DrawSliderInt(0, 100, PLD.Config.PLD_ST_Invuln, "", 150, SliderIncrements.Ones);
			}

			if (preset == CustomComboPreset.PLD_AoE_Invuln && enabled)
			{
				UserConfig.DrawSliderInt(0, 100, PLD.Config.PLD_AoE_Invuln, "", 150, SliderIncrements.Ones);
			}

			#endregion

			#region REAPER



			#endregion

			#region RED MAGE

			if (preset is CustomComboPreset.RDM_ST_Lucid && enabled)
			{
				UserConfig.DrawSliderInt(1000, 10000, RDM.Config.RDM_ST_Lucid, "MP Threshold", 150, SliderIncrements.Hundreds);
			}

			if (preset is CustomComboPreset.RDM_AoE_Lucid && enabled)
			{
				UserConfig.DrawSliderInt(1000, 10000, RDM.Config.RDM_AoE_Lucid, "MP Threshold", 150, SliderIncrements.Hundreds);
			}

			#endregion

			#region SAGE

			if (preset is CustomComboPreset.SGE_ST_DPS_Lucid && enabled)
			{
				UserConfig.DrawSliderInt(1000, 10000, SGE.Config.SGE_ST_DPS_Lucid, "MP Threshold", 150, SliderIncrements.Hundreds);
			}

			if (preset is CustomComboPreset.SGE_AoE_DPS_Lucid && enabled)
			{
				UserConfig.DrawSliderInt(1000, 10000, SGE.Config.SGE_AoE_DPS_Lucid, "MP Threshold", 150, SliderIncrements.Hundreds);
			}

			if (preset is CustomComboPreset.SGE_ST_DPS_Rhizo && enabled)
			{
				UserConfig.DrawSliderInt(0, 1, SGE.Config.SGE_ST_DPS_Rhizo, "Addersgall Threshold", 150, SliderIncrements.Ones);
			}

			if (preset is CustomComboPreset.SGE_ST_DPS_AddersgallProtect && enabled)
			{
				UserConfig.DrawSliderInt(1, 3, SGE.Config.SGE_ST_DPS_AddersgallProtect, "Addersgall Threshold", 150, SliderIncrements.Ones);
			}

			if (preset is CustomComboPreset.SGE_AoE_DPS_Rhizo && enabled)
			{
				UserConfig.DrawSliderInt(0, 1, SGE.Config.SGE_AoE_DPS_Rhizo, "Addersgall Threshold", 150, SliderIncrements.Ones);
			}

			if (preset is CustomComboPreset.SGE_AoE_DPS_AddersgallProtect && enabled)
			{
				UserConfig.DrawSliderInt(1, 3, SGE.Config.SGE_AoE_DPS_AddersgallProtect, "Addersgall Threshold", 150, SliderIncrements.Ones);
			}

			#endregion

			#region SAMURAI



			#endregion

			#region SCHOLAR

			if (preset is CustomComboPreset.SCH_ST_DPS_Lucid && enabled)
			{
				UserConfig.DrawSliderInt(1000, 10000, SCH.Config.SCH_ST_DPS_Lucid, "MP Threshold", 150, SliderIncrements.Hundreds);
			}

			if (preset is CustomComboPreset.SCH_AoE_DPS_Lucid && enabled)
			{
				UserConfig.DrawSliderInt(1000, 10000, SCH.Config.SCH_AoE_DPS_Lucid, "MP Threshold", 150, SliderIncrements.Hundreds);
			}

			#endregion

			#region SUMMONER

			if (preset == CustomComboPreset.SMN_ST_Lucid && enabled)
			{
				UserConfig.DrawSliderInt(1000, 10000, SMN.Config.SMN_ST_Lucid, "MP Threshold", 150, SliderIncrements.Hundreds);
			}

			if (preset == CustomComboPreset.SMN_AoE_Lucid && enabled)
			{
				UserConfig.DrawSliderInt(1000, 10000, SMN.Config.SMN_AoE_Lucid, "MP Threshold", 150, SliderIncrements.Hundreds);
			}

			#endregion

			#region VIPER



			#endregion

			#region WARRIOR

			if (preset == CustomComboPreset.WAR_ST_StormsEye && enabled)
			{
				UserConfig.DrawSliderInt(0, 30, WAR.Config.WAR_SurgingRefresh, "Seconds remaining before refreshing Surging Tempest", 150, SliderIncrements.Ones);
			}

			if (preset == CustomComboPreset.WAR_ST_FellCleave && enabled)
			{
				UserConfig.DrawSliderInt(50, 100, WAR.Config.WAR_FellCleaveGauge, "Minimum gauge to spend", 150, SliderIncrements.Fives);
			}

			if (preset == CustomComboPreset.WAR_AoE_Decimate && enabled)
			{
				UserConfig.DrawSliderInt(50, 100, WAR.Config.WAR_DecimateGauge, "Minimum gauge to spend", 150, SliderIncrements.Fives);
			}

			if (preset == CustomComboPreset.WAR_ST_Invuln && enabled)
			{
				UserConfig.DrawSliderInt(0, 100, WAR.Config.WAR_ST_Invuln, "", 150, SliderIncrements.Ones);
			}

			if (preset == CustomComboPreset.WAR_AoE_Invuln && enabled)
			{
				UserConfig.DrawSliderInt(0, 100, WAR.Config.WAR_AoE_Invuln, "", 150, SliderIncrements.Ones);
			}

			#endregion

			#region WHITE MAGE

			if (preset == CustomComboPreset.WHM_ST_DPS_Lucid && enabled)
			{
				UserConfig.DrawSliderInt(1000, 10000, WHM.Config.WHM_ST_DPS_Lucid, "MP Threshold", 150, SliderIncrements.Hundreds);
			}

			if (preset == CustomComboPreset.WHM_AoE_DPS_Lucid && enabled)
			{
				UserConfig.DrawSliderInt(1000, 10000, WHM.Config.WHM_AoE_DPS_Lucid, "MP Threshold", 150, SliderIncrements.Hundreds);
			}

			if (preset == CustomComboPreset.WHM_Raise && enabled)
			{
				UserConfig.DrawAdditionalBoolChoice(WHM.Config.WHM_Raise_ThinAir, "Use Thin Air", "");
			}

			#region PvP

			if (preset is CustomComboPreset.WHMPvP_Polymorph && enabled)
			{
				UserConfig.DrawSliderInt(0, 100, WHMPvP.Config.WHMPvP_Polymorph, "Target % HP", 150, SliderIncrements.Fives);
			}

			if (preset is CustomComboPreset.WHMPvP_Cure3 && enabled)
			{
				UserConfig.DrawSliderInt(0, 100, WHMPvP.Config.WHMPvP_Cure3, "HP % Threshold", 150, SliderIncrements.Fives);
			}

			#endregion

			#endregion

			#region PvP Global

			IPlayerCharacter? pc = Service.ClientState.LocalPlayer;

			if (preset == CustomComboPreset.PvP_EmergencyHeals && enabled)
			{
				if (pc != null)
				{
					uint maxHP = Service.ClientState.LocalPlayer?.MaxHp <= 15000 ? 0 : Service.ClientState.LocalPlayer.MaxHp - 15000;

					if (maxHP > 0)
					{
						int setting = PluginConfiguration.GetCustomIntValue(PvPCommon.Config.EmergencyHealThreshold);
						_ = (float)maxHP / 100 * setting;

						UserConfig.DrawSliderInt(1, 100, PvPCommon.Config.EmergencyHealThreshold, "");
					}

					else
					{
						UserConfig.DrawSliderInt(1, 100, PvPCommon.Config.EmergencyHealThreshold, "");
					}
				}

				else
				{
					UserConfig.DrawSliderInt(1, 100, PvPCommon.Config.EmergencyHealThreshold, "");
				}
			}

			if (preset == CustomComboPreset.PvP_EmergencyGuard && enabled)
			{
				UserConfig.DrawSliderInt(1, 100, PvPCommon.Config.EmergencyGuardThreshold, "");
			}

			if (preset == CustomComboPreset.PvP_QuickPurify && enabled)
			{
				UserConfig.DrawPvPStatusMultiChoice(PvPCommon.Config.QuickPurifyStatuses);
			}

			#endregion
		}
	}

	public static class SliderIncrements
	{
		public const uint
			Ones = 1,
			Fives = 5,
			Tens = 10,
			Hundreds = 100,
			Thousands = 1000;
	}
}