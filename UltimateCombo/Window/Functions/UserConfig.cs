using Dalamud.Interface;
using Dalamud.Interface.Colors;
using Dalamud.Utility;
using ImGuiNET;
using System;
using System.Numerics;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Combos;
using UltimateCombo.Combos.Content;
using UltimateCombo.Combos.PvE;
using UltimateCombo.Combos.PvP;
using UltimateCombo.Core;
using UltimateCombo.Services;

namespace UltimateCombo.Window.Functions
{
    public static class UserConfig
    {
        public static void DrawSliderInt(int minValue, int maxValue, string config, string sliderDescription, float itemWidth = 150, uint sliderIncrement = SliderIncrements.Ones, bool hasAdditionalChoice = false, string additonalChoiceCondition = "")
        {
            ImGui.Indent();
            var output = PluginConfiguration.GetCustomIntValue(config, minValue);
            if (output < minValue)
            {
                output = minValue;
                PluginConfiguration.SetCustomIntValue(config, output);
                Service.Configuration.Save();
            }

            sliderDescription = sliderDescription.Replace("%", "%%");
            var contentRegionMin = ImGui.GetItemRectMax().Y - ImGui.GetItemRectMin().Y;
            var wrapPos = ImGui.GetContentRegionMax().X - 35f;

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
                    var inputChanged = false;
                    Vector2 currentPos = ImGui.GetCursorPos();
                    ImGui.SetCursorPosX(currentPos.X + itemWidth);
                    ImGui.PushTextWrapPos(wrapPos);
                    ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DalamudWhite);
                    ImGui.Text($"{sliderDescription}");
                    Vector2 height = ImGui.GetItemRectSize();
                    var lines = height.Y / ImGui.GetFontSize();
                    Vector2 textLength = ImGui.CalcTextSize(sliderDescription);
                    var newLines = "";
                    for (var i = 1; i < lines; i++)
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
                            var message = "This setting has additional options depending on its value.";

                            if (!string.IsNullOrEmpty(additonalChoiceCondition))
                            {
                                message += $"\nCondition: {additonalChoiceCondition}";
                            }

                            ImGui.TextUnformatted(message);
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
            var output = PluginConfiguration.GetCustomFloatValue(config, minValue);
            if (output < minValue)
            {
                output = minValue;
                PluginConfiguration.SetCustomFloatValue(config, output);
                Service.Configuration.Save();
            }

            sliderDescription = sliderDescription.Replace("%", "%%");
            var contentRegionMin = ImGui.GetItemRectMax().Y - ImGui.GetItemRectMin().Y;
            var wrapPos = ImGui.GetContentRegionMax().X - 35f;


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
                    var inputChanged = false;
                    Vector2 currentPos = ImGui.GetCursorPos();
                    ImGui.SetCursorPosX(currentPos.X + itemWidth);
                    ImGui.PushTextWrapPos(wrapPos);
                    ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DalamudWhite);
                    ImGui.Text($"{sliderDescription}");
                    Vector2 height = ImGui.GetItemRectSize();
                    var lines = height.Y / ImGui.GetFontSize();
                    Vector2 textLength = ImGui.CalcTextSize(sliderDescription);
                    var newLines = "";
                    for (var i = 1; i < lines; i++)
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

                            if (string.IsNullOrEmpty(additonalChoiceCondition))
                            {
                                ImGui.TextUnformatted("This setting has additional options depending on its value.");
                            }

                            else
                            {
                                ImGui.TextUnformatted($"This setting has additional options depending on its value.\nCondition: {additonalChoiceCondition}");
                            }

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
            var output = PluginConfiguration.GetCustomFloatValue(config, minValue);
            if (output < minValue)
            {
                output = minValue;
                PluginConfiguration.SetCustomFloatValue(config, output);
                Service.Configuration.Save();
            }

            sliderDescription = sliderDescription.Replace("%", "%%");
            var contentRegionMin = ImGui.GetItemRectMax().Y - ImGui.GetItemRectMin().Y;
            var wrapPos = ImGui.GetContentRegionMax().X - 35f;


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
                    var inputChanged = false;
                    Vector2 currentPos = ImGui.GetCursorPos();
                    ImGui.SetCursorPosX(currentPos.X + itemWidth);
                    ImGui.PushTextWrapPos(wrapPos);
                    ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DalamudWhite);
                    ImGui.Text($"{sliderDescription}");
                    Vector2 height = ImGui.GetItemRectSize();
                    var lines = height.Y / ImGui.GetFontSize();
                    Vector2 textLength = ImGui.CalcTextSize(sliderDescription);
                    var newLines = "";
                    for (var i = 1; i < lines; i++)
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
                            var message = "This setting has additional options depending on its value.";

                            if (!string.IsNullOrEmpty(additonalChoiceCondition))
                            {
                                message += $"\nCondition: {additonalChoiceCondition}";
                            }

                            ImGui.TextUnformatted(message);
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

            var output = PluginConfiguration.GetCustomIntValue(config, outputValue);
            ImGui.PushItemWidth(itemWidth);
            ImGui.SameLine();
            ImGui.Dummy(new Vector2(21, 0));
            ImGui.SameLine();
            var enabled = output == outputValue;

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

            var output = PluginConfiguration.GetCustomIntValue(config);
            ImGui.SameLine();
            ImGui.PushItemWidth(itemWidth);
            var enabled = output == outputValue;

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
            var output = PluginConfiguration.GetCustomBoolValue(config);
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

            var values = PluginConfiguration.GetCustomBoolArrayValue(config);

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
            var totalChoices = nameAndDesc.GetLength(0);
            if (totalChoices > 0)
            {
                ImGui.Indent();
                if (descriptionColor == new Vector4())
                {
                    descriptionColor = ImGuiColors.DalamudWhite;
                }
                var values = PluginConfiguration.GetCustomBoolArrayValue(config);

                if (values.Length == 0 || values.Length != totalChoices)
                {
                    Array.Resize(ref values, totalChoices);
                    PluginConfiguration.SetCustomBoolArrayValue(config, values);
                    Service.Configuration.Save();
                }

                _ = ImGui.BeginTable($"Grid###{config}", columns);
                ImGui.TableNextRow();
                for (var idx = 0; idx < totalChoices; idx++)
                {
                    _ = ImGui.TableNextColumn();
                    var checkBoxName = nameAndDesc[idx, 0];
                    var checkboxDescription = nameAndDesc[idx, 1];

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

        public static void DrawRoleGridMultiChoice(string config)
        {
            var values = PluginConfiguration.GetCustomBoolArrayValue(config);

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
            var value = PluginConfiguration.GetCustomIntValue(config);
            var values = new bool[20];

            for (var i = 0; i <= 4; i++)
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
            var values = PluginConfiguration.GetCustomBoolArrayValue(config);

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
            var value = PluginConfiguration.GetCustomIntValue(config);
            var values = new bool[20];

            for (var i = 0; i <= 19; i++)
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
                for (var i = 1; i <= maxValues; i++)
                {
                    config[i - 1] = i;
                }
            }

            var curVal = config[currentItem];
            var oldVal = config[currentItem];

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
                        for (var i = 0; i < maxValues; i++)
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
            var sliderAsDouble = Convert.ToDouble(sliderIncrement);
            return ((int) Math.Round(i / sliderAsDouble)) * (int) sliderIncrement;
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

    public static class UserConfigItems
    {
        internal static void Draw(CustomComboPreset preset, bool enabled)
        {
            if (!enabled)
            {
                return;
            }

            #region All

            #region Role Actions

            if (preset is CustomComboPreset.All_SecondWind && enabled)
            {
                UserConfig.DrawSliderInt(0, 100, All.Config.All_SecondWind, "HP % Threshold", 150, SliderIncrements.Fives);
            }

            if (preset is CustomComboPreset.All_Bloodbath && enabled)
            {
                UserConfig.DrawSliderInt(0, 100, All.Config.All_Bloodbath, "HP % Threshold", 150, SliderIncrements.Fives);
            }

            if (preset is CustomComboPreset.All_Healer_Lucid && enabled)
            {
                UserConfig.DrawSliderInt(0, 10000, All.Config.All_Healer_Lucid, "MP Threshold", 150, SliderIncrements.Hundreds);
            }

            if (preset is CustomComboPreset.All_Mage_Lucid && enabled)
            {
                UserConfig.DrawSliderInt(0, 10000, All.Config.All_Mage_Lucid, "MP Threshold", 150, SliderIncrements.Hundreds);
            }

            if (preset is CustomComboPreset.All_BLU_Lucid && enabled)
            {
                UserConfig.DrawSliderInt(0, 10000, All.Config.All_BLU_Lucid, "MP Threshold", 150, SliderIncrements.Hundreds);
            }

            #endregion

            #region Chocobo Actions

            if (preset is CustomComboPreset.All_Choco && enabled)
            {
                UserConfig.DrawAdditionalBoolChoice(All.Config.All_ChocoAuto, "Automatically command Chocobo?", "");
            }

            if (preset is CustomComboPreset.All_Choco && enabled && !All.Config.All_ChocoAuto)
            {
                UserConfig.DrawRadioButton(All.Config.All_ChocoMode, "Attack Mode", "", 1);
                UserConfig.DrawRadioButton(All.Config.All_ChocoMode, "Heal Mode", "", 2);
                UserConfig.DrawRadioButton(All.Config.All_ChocoMode, "Tank Mode", "", 3);
            }

            if (preset is CustomComboPreset.All_Choco && enabled && All.Config.All_ChocoAuto)
            {
                UserConfig.DrawSliderInt(0, 100, All.Config.All_ChocoHP, "HP % Threshold", 150, SliderIncrements.Fives);
            }

            #endregion

            #region Variant Actions

            if (preset is CustomComboPreset.Variant_Cure && enabled)
            {
                UserConfig.DrawSliderInt(0, 100, Variant.Config.Variant_Cure, "HP % Threshold", 150, SliderIncrements.Fives);
            }

            #endregion

            #region Occult Actions

            if (preset is CustomComboPreset.Occult_PhantomResuscitation && enabled)
            {
                UserConfig.DrawSliderInt(0, 100, Occult.Config.Occult_PhantomResuscitation, "HP % Threshold", 150, SliderIncrements.Fives);
            }

            if (preset is CustomComboPreset.Occult_Pray && enabled)
            {
                UserConfig.DrawSliderInt(0, 100, Occult.Config.Occult_Pray, "HP % Threshold", 150, SliderIncrements.Fives);
            }

            if (preset is CustomComboPreset.Occult_Heal && enabled)
            {
                UserConfig.DrawSliderInt(0, 100, Occult.Config.Occult_Heal, "HP % Threshold", 150, SliderIncrements.Fives);
            }

            if (preset is CustomComboPreset.Occult_Predict && enabled)
            {
                ImGui.Spacing();
                UserConfig.DrawRadioButton(Occult.Config.Occult_Prediction, "Phantom Judgement", "", 1, 150);
                UserConfig.DrawRadioButton(Occult.Config.Occult_Prediction, "Cleansing", "", 2, 150);
                UserConfig.DrawRadioButton(Occult.Config.Occult_Prediction, "Blessing", "", 3, 150);
                UserConfig.DrawRadioButton(Occult.Config.Occult_Prediction, "Starfall", "", 4, 150);
            }

            if (preset is CustomComboPreset.Occult_HolySilverCannon && enabled)
            {
                ImGui.Spacing();
                UserConfig.DrawRadioButton(Occult.Config.Occult_HolySilverCannon, "Holy Cannon", "", 1, 150);
                UserConfig.DrawRadioButton(Occult.Config.Occult_HolySilverCannon, "Silver Cannon", "", 2, 150);
            }

            if (preset is CustomComboPreset.Occult_DarkShockCannon && enabled)
            {
                ImGui.Spacing();
                UserConfig.DrawRadioButton(Occult.Config.Occult_DarkShockCannon, "Dark Cannon", "", 1, 150);
                UserConfig.DrawRadioButton(Occult.Config.Occult_DarkShockCannon, "Shock Cannon", "", 2, 150);
            }

            if (preset is CustomComboPreset.Occult_Weather && enabled)
            {
                UserConfig.DrawSliderInt(0, 100, Occult.Config.Occult_Sunbath, "Sunbath HP % Threshold", 150, SliderIncrements.Fives);
            }

            if (preset is CustomComboPreset.Occult_PhantomKick && enabled)
            {
                UserConfig.DrawAdditionalBoolChoice(Occult.Config.Occult_KickNotice, "Show Phantom Kick if out of range and buff" +
                    "\nhas fewer than 5 seconds left", "");
            }

            #endregion

            #endregion

            #region PvP Global

            if (preset == CustomComboPreset.PvP_Recuperate && enabled)
            {
                UserConfig.DrawSliderInt(1, 100, PvPCommon.Config.Recuperate, "HP % Threshold");
            }

            if (preset == CustomComboPreset.PvP_Guard && enabled)
            {
                UserConfig.DrawSliderInt(1, 100, PvPCommon.Config.Guard, "HP % Threshold");
            }

            if (preset == CustomComboPreset.PvP_Purify && enabled)
            {
                UserConfig.DrawAdditionalBoolChoice(PvPCommon.Config.Purify_Stun, "Stun", "");
                UserConfig.DrawAdditionalBoolChoice(PvPCommon.Config.Purify_DeepFreeze, "Deep Freeze", "");
                UserConfig.DrawAdditionalBoolChoice(PvPCommon.Config.Purify_HalfAsleep, "Half Asleep", "");
                UserConfig.DrawAdditionalBoolChoice(PvPCommon.Config.Purify_Sleep, "Sleep", "");
                UserConfig.DrawAdditionalBoolChoice(PvPCommon.Config.Purify_Bind, "Bind", "");
                UserConfig.DrawAdditionalBoolChoice(PvPCommon.Config.Purify_Heavy, "Heavy", "");
                UserConfig.DrawAdditionalBoolChoice(PvPCommon.Config.Purify_Silence, "Silence", "");
            }

            if (preset == CustomComboPreset.TankPvP_Rampart && enabled)
            {
                UserConfig.DrawSliderInt(1, 100, PvPCommon.Config.TankPvP_Rampart, "HP % Threshold");
            }

            if (preset == CustomComboPreset.MeleePvP_Bloodbath && enabled)
            {
                UserConfig.DrawSliderInt(1, 100, PvPCommon.Config.MeleePvP_Bloodbath, "HP % Threshold");
            }

            #endregion

            #region Tanks

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

            #region PvP

            if (preset is CustomComboPreset.PLDPvP_Phalanx && enabled)
            {
                UserConfig.DrawSliderInt(0, 100, PLDPvP.Config.PLDPvP_Phalanx, "Triggers when you have less than or equal to this % of HP, or after using Guardian", 150, SliderIncrements.Fives);
            }

            #endregion

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

            #region PvP

            if (preset is CustomComboPreset.WARPvP_Bloodwhetting && enabled)
            {
                UserConfig.DrawSliderInt(0, 100, WARPvP.Config.WARPvP_Bloodwhetting, "Triggers when you have less than or equal to this % of HP", 150, SliderIncrements.Fives);
            }

            #endregion

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

            #region PvP

            if (preset is CustomComboPreset.DRKPvP_Shadowbringer && enabled)
            {
                UserConfig.DrawSliderInt(0, 100, DRKPvP.Config.DRKPvP_Shadowbringer, "Triggers when you have more than this % of HP", 150, SliderIncrements.Fives);
            }

            if (preset is CustomComboPreset.DRKPvP_Impalement && enabled)
            {
                UserConfig.DrawSliderInt(0, 100, DRKPvP.Config.DRKPvP_Impalement, "Triggers when you have less than or equal to this % of HP", 150, SliderIncrements.Fives);
            }

            #endregion

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

            #endregion

            #region Healers

            #region WHITE MAGE



            #region PvP



            #endregion

            #endregion

            #region SCHOLAR



            #endregion

            #region ASTROLOGIAN

            if (preset is CustomComboPreset.AST_ST_DPS_AutoDraw && enabled)
            {
                UserConfig.DrawAdditionalBoolChoice(AST.Config.AST_ST_DPS_UseDefenseCards, "Use Arrow/Spire/Bole/Ewer", "");
            }

            if (preset is CustomComboPreset.AST_AoE_DPS_AutoDraw && enabled)
            {
                UserConfig.DrawAdditionalBoolChoice(AST.Config.AST_AoE_DPS_UseDefenseCards, "Use Arrow/Spire/Bole/Ewer", "");
            }

            #region PvP

            if (preset is CustomComboPreset.ASTPvP_AspectedBenefic && enabled)
            {
                UserConfig.DrawSliderInt(0, 100, ASTPvP.Config.ASTPvP_AspectedBenefic, "Triggers when you have less than this % of HP", 150, SliderIncrements.Fives);
            }

            #endregion

            #endregion

            #region SAGE

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

            #endregion

            #region Melee DPS

            #region MONK



            #endregion

            #region DRAGOON



            #endregion

            #region NINJA

            #region PvP



            #endregion

            #endregion

            #region SAMURAI

            if (preset is CustomComboPreset.SAM_ST_Shinten && enabled)
            {
                UserConfig.DrawAdditionalBoolChoice(SAM.Config.SAM_ST_SaveKenkiDash, "Save 10 Kenki for Gyoten", "");
            }

            if (preset is CustomComboPreset.SAM_AoE_Kyuten && enabled)
            {
                UserConfig.DrawAdditionalBoolChoice(SAM.Config.SAM_AoE_SaveKenkiDash, "Save 10 Kenki for Gyoten", "");
            }

            #endregion

            #region REAPER



            #endregion

            #region VIPER



            #endregion

            #endregion

            #region Physical Ranged DPS

            #region BARD



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

            #region DANCER



            #endregion

            #endregion

            #region Magical Ranged DPS

            #region BLACK MAGE



            #endregion

            #region SUMMONER

            if (preset is CustomComboPreset.SMN_ST_Astral && enabled)
            {
                UserConfig.DrawAdditionalBoolChoice(SMN.Config.SMN_ST_Astral_Swift, "Use Swiftcast on Slipstream", "");
            }

            if (preset is CustomComboPreset.SMN_AoE_Astral && enabled)
            {
                UserConfig.DrawAdditionalBoolChoice(SMN.Config.SMN_AoE_Astral_Swift, "Use Swiftcast on Slipstream", "");
            }

            #endregion

            #region RED MAGE



            #endregion

            #region PICTOMANCER

            if (preset == CustomComboPreset.PCTPvP_AutoPalette && enabled)
            {
                UserConfig.DrawSliderInt(0, 100, PCTPvP.Config.PCTPvP_AutoPalette, "Triggers Subtractive Palette when above this % HP", 150, SliderIncrements.Fives);
            }

            #endregion

            #region BLUE MAGE

            if (preset is CustomComboPreset.BLU_ManaGain && enabled)
            {
                UserConfig.DrawSliderInt(1000, 10000, BLU.Config.BLU_ManaGain, "Blood Drain MP Threshold", 150, SliderIncrements.Hundreds);
            }

            if (preset is CustomComboPreset.BLU_Tank_WhiteWind && enabled)
            {
                UserConfig.DrawSliderInt(0, 100, BLU.Config.BLU_TankWhiteWind, "White Wind HP % Threshold", 150, SliderIncrements.Fives);
            }

            if (preset is CustomComboPreset.BLU_Treasure_Healer_Pomcure && enabled)
            {
                UserConfig.DrawSliderInt(0, 100, BLU.Config.BLU_TreasurePomcure, "Pomcure HP % Threshold", 150, SliderIncrements.Fives);
            }

            if (preset is CustomComboPreset.BLU_Treasure_Healer_Gobskin && enabled)
            {
                UserConfig.DrawSliderInt(0, 100, BLU.Config.BLU_TreasureGobskin, "Shield % Threshold", 150, SliderIncrements.Fives);
            }

            if (preset is CustomComboPreset.BLU_Treasure_Tank_WhiteWind && enabled)
            {
                UserConfig.DrawSliderInt(0, 100, BLU.Config.BLU_TreasureWhiteWind, "White Wind HP % Threshold", 150, SliderIncrements.Fives);
            }

            if (preset is CustomComboPreset.BLU_Treasure_Tank_Rehydration && enabled)
            {
                UserConfig.DrawSliderInt(0, 100, BLU.Config.BLU_TreasureRehydration, "Rehydration HP % Threshold", 150, SliderIncrements.Fives);
            }

            #endregion

            #endregion
        }
    }
}
