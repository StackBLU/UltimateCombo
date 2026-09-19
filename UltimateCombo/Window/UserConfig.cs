
using Dalamud.Bindings.ImGui;
using Dalamud.Interface;
using Dalamud.Interface.Colors;
using Dalamud.Utility;
using System;
using System.Numerics;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Combos;
using UltimateCombo.Combos.Content;
using UltimateCombo.Combos.General;
using UltimateCombo.Combos.PvE;
using UltimateCombo.Combos.PvP;
using UltimateCombo.Core;

namespace UltimateCombo.Window;

internal static class UserConfig
{
    internal static void DrawSliderInt(int minValue, int maxValue, UserInt userInt, uint sliderIncrement = SliderIncrements.Ones)
    {
        ImGui.Indent();

        var box = new InfoBox
        {
            Color = Colors.White,
            BorderThickness = 1f,
            CurveRadius = 3f,
            AutoResize = true,
            HasMaxWidth = true,
            IsSubBox = true,
            ContentsAction = () =>
            {
                var output = (int) userInt;
                ImGui.SetNextItemWidth(250);
                if (ImGui.SliderInt($"###{userInt.Name}", ref output, minValue, maxValue))
                {
                    var increment = Math.Max(1u, sliderIncrement);
                    output = (int) (Math.Round(output / (double) increment) * increment);
                    output = Math.Clamp(output, minValue, maxValue);
                    PluginConfiguration.SetCustomIntValue(userInt.Name, output);
                }

                if (ImGui.IsItemDeactivatedAfterEdit())
                {
                    Service.Configuration.Save();
                }
            }
        };

        box.Draw();

        ImGui.SameLine();
        if (ImGui.Button($"Reset##{userInt.Name}"))
        {
            userInt.Reset();
        }

        ImGui.Spacing();
        ImGui.Unindent();
    }

    internal static void DrawRadioButton(string config, string checkBoxName, string checkboxDescription, int outputValue, float itemWidth = 150, Vector4 descriptionColor = default)
    {
        ImGui.Indent();
        descriptionColor = descriptionColor == Vector4.Zero ? ImGuiColors.DalamudYellow : descriptionColor;

        var output = PluginConfiguration.GetCustomIntValue(config, outputValue);
        var enabled = output == outputValue;

        ImGui.SetNextItemWidth(itemWidth);
        ImGui.SameLine();
        ImGui.Dummy(new Vector2(21, 0));
        ImGui.SameLine();

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

    internal static void DrawAdditionalBoolChoice(string config, string checkBoxName, string checkboxDescription, float itemWidth = 150, bool isConditionalChoice = false)
    {
        var output = PluginConfiguration.GetCustomBoolValue(config);
        ImGui.SetNextItemWidth(itemWidth);

        if (isConditionalChoice)
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
        }

        ImGui.Indent();

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
}

internal static class SliderIncrements
{
    internal const uint
        Ones = 1,
        Fives = 5,
        Tens = 10,
        Hundreds = 100,
        Thousands = 1000;
}

internal static class UserConfigItems
{
    internal static void Draw(Presets preset, bool enabled)
    {
        if (!enabled)
        {
            return;
        }

        #region All

        #region Role Actions

        if (preset is Presets.All_SecondWind)
        {
            UserConfig.DrawSliderInt(0, 100, Common.Config.All_SecondWind, SliderIncrements.Fives);
        }

        if (preset is Presets.All_Bloodbath)
        {
            UserConfig.DrawSliderInt(0, 100, Common.Config.All_Bloodbath, SliderIncrements.Fives);
        }

        if (preset is Presets.All_Healer_Lucid)
        {
            UserConfig.DrawSliderInt(0, 10000, Common.Config.All_Healer_Lucid, SliderIncrements.Hundreds);
        }

        if (preset is Presets.All_Mage_Lucid)
        {
            UserConfig.DrawSliderInt(0, 10000, Common.Config.All_Mage_Lucid, SliderIncrements.Hundreds);
        }

        if (preset is Presets.All_BLU_Lucid)
        {
            UserConfig.DrawSliderInt(0, 10000, Common.Config.All_BLU_Lucid, SliderIncrements.Hundreds);
        }

        #endregion

        #region Chocobo Actions

        if (preset is Presets.All_Choco)
        {
            UserConfig.DrawAdditionalBoolChoice(Common.Config.All_ChocoAuto, "Automatically command Chocobo?", "");
        }

        if (preset is Presets.All_Choco && !Common.Config.All_ChocoAuto)
        {
            UserConfig.DrawRadioButton(Common.Config.All_ChocoMode, "Attack Mode", "", 1);
            UserConfig.DrawRadioButton(Common.Config.All_ChocoMode, "Heal Mode", "", 2);
            UserConfig.DrawRadioButton(Common.Config.All_ChocoMode, "Tank Mode", "", 3);
        }

        if (preset is Presets.All_Choco && Common.Config.All_ChocoAuto)
        {
            UserConfig.DrawSliderInt(0, 100, Common.Config.All_ChocoHP, SliderIncrements.Fives);
        }

        #endregion

        #region Variant Actions

        if (preset is Presets.Variant_Cure)
        {
            UserConfig.DrawSliderInt(0, 100, Variant.Config.Variant_Cure, SliderIncrements.Fives);
        }

        #endregion

        #region Occult Actions

        if (preset is Presets.Occult_PhantomResuscitation)
        {
            UserConfig.DrawSliderInt(0, 100, Occult.Config.Occult_PhantomResuscitation, SliderIncrements.Fives);
        }

        if (preset is Presets.Occult_Pray)
        {
            UserConfig.DrawSliderInt(0, 100, Occult.Config.Occult_Pray, SliderIncrements.Fives);
        }

        if (preset is Presets.Occult_Heal)
        {
            UserConfig.DrawSliderInt(0, 100, Occult.Config.Occult_Heal, SliderIncrements.Fives);
        }

        if (preset is Presets.Occult_Predict)
        {
            ImGui.Spacing();
            UserConfig.DrawRadioButton(Occult.Config.Occult_Prediction, "Phantom Judgement", "", 1, 150);
            UserConfig.DrawRadioButton(Occult.Config.Occult_Prediction, "Cleansing", "", 2, 150);
            UserConfig.DrawRadioButton(Occult.Config.Occult_Prediction, "Blessing", "", 3, 150);
            UserConfig.DrawRadioButton(Occult.Config.Occult_Prediction, "Starfall", "", 4, 150);
        }

        if (preset is Presets.Occult_HolySilverCannon)
        {
            ImGui.Spacing();
            UserConfig.DrawRadioButton(Occult.Config.Occult_HolySilverCannon, "Holy Cannon", "", 1, 150);
            UserConfig.DrawRadioButton(Occult.Config.Occult_HolySilverCannon, "Silver Cannon", "", 2, 150);
        }

        if (preset is Presets.Occult_DarkShockCannon)
        {
            ImGui.Spacing();
            UserConfig.DrawRadioButton(Occult.Config.Occult_DarkShockCannon, "Dark Cannon", "", 1, 150);
            UserConfig.DrawRadioButton(Occult.Config.Occult_DarkShockCannon, "Shock Cannon", "", 2, 150);
        }

        if (preset is Presets.Occult_Weather)
        {
            UserConfig.DrawSliderInt(0, 100, Occult.Config.Occult_Sunbath, SliderIncrements.Fives);
        }

        if (preset is Presets.Occult_PhantomKick)
        {
            UserConfig.DrawAdditionalBoolChoice(Occult.Config.Occult_KickNotice, "Show Phantom Kick if out of range and buff" +
                "\nhas fewer than 5 seconds left", "");
        }

        if (preset is Presets.Occult_Spell2)
        {
            ImGui.Spacing();
            UserConfig.DrawRadioButton(Occult.Config.Occult_Spell2, "Fire II", "", 1, 150);
            UserConfig.DrawRadioButton(Occult.Config.Occult_Spell2, "Blizzard II", "", 2, 150);
            UserConfig.DrawRadioButton(Occult.Config.Occult_Spell2, "Thunder II", "", 3, 150);
        }

        if (preset is Presets.Occult_Spell3)
        {
            ImGui.Spacing();
            UserConfig.DrawRadioButton(Occult.Config.Occult_Spell3, "Fire III", "", 1, 150);
            UserConfig.DrawRadioButton(Occult.Config.Occult_Spell3, "Blizzard III", "", 2, 150);
            UserConfig.DrawRadioButton(Occult.Config.Occult_Spell3, "Thunder III", "", 3, 150);
        }

        if (preset is Presets.Occult_SMNSpell)
        {
            ImGui.Spacing();
            UserConfig.DrawRadioButton(Occult.Config.Occult_SMNSpell, "Hellfire", "", 1, 150);
            UserConfig.DrawRadioButton(Occult.Config.Occult_SMNSpell, "Judgement Bolt", "", 2, 150);
            UserConfig.DrawRadioButton(Occult.Config.Occult_SMNSpell, "Thunderstorm (Wind)", "", 3, 150);
        }

        if (preset is Presets.Occult_NECSpell)
        {
            ImGui.Spacing();
            UserConfig.DrawRadioButton(Occult.Config.Occult_NECSpell, "Deep Freeze", "", 1, 150);
            UserConfig.DrawRadioButton(Occult.Config.Occult_NECSpell, "Hell Wind", "", 2, 150);
            UserConfig.DrawRadioButton(Occult.Config.Occult_NECSpell, "Chaos Drive (Lightning)", "", 3, 150);
        }

        #endregion

        #endregion

        #region PvP Global

        if (preset == Presets.PvP_Recuperate)
        {
            UserConfig.DrawSliderInt(1, 100, AllPvP.Config.Recuperate);
        }

        if (preset == Presets.PvP_Guard)
        {
            UserConfig.DrawSliderInt(1, 100, AllPvP.Config.Guard);
        }

        if (preset == Presets.PvP_Purify)
        {
            UserConfig.DrawAdditionalBoolChoice(AllPvP.Config.Purify_Stun, "Stun", "");
            UserConfig.DrawAdditionalBoolChoice(AllPvP.Config.Purify_Heavy, "Heavy", "");
            UserConfig.DrawAdditionalBoolChoice(AllPvP.Config.Purify_Bind, "Bind", "");
            UserConfig.DrawAdditionalBoolChoice(AllPvP.Config.Purify_Silence, "Silence", "");
            UserConfig.DrawAdditionalBoolChoice(AllPvP.Config.Purify_DeepFreeze, "Deep Freeze", "");
            UserConfig.DrawAdditionalBoolChoice(AllPvP.Config.Purify_MiracleOfNature, "Miracle of Nature", "");
        }

        if (preset == Presets.TankPvP_Rampart)
        {
            UserConfig.DrawSliderInt(1, 100, AllPvP.Config.TankPvP_Rampart);
        }

        if (preset == Presets.MeleePvP_Bloodbath)
        {
            UserConfig.DrawSliderInt(1, 100, AllPvP.Config.MeleePvP_Bloodbath);
        }

        #endregion

        #region Tanks

        #region Paladin

        if (preset == Presets.PLD_ST_Sheltron)
        {
            UserConfig.DrawSliderInt(50, 100, PLD.Config.PLD_ST_Sheltron, SliderIncrements.Fives);
        }

        if (preset == Presets.PLD_ST_Intervention)
        {
            UserConfig.DrawSliderInt(50, 100, PLD.Config.PLD_ST_Intervention, SliderIncrements.Fives);
        }

        if (preset == Presets.PLD_AoE_Sheltron)
        {
            UserConfig.DrawSliderInt(50, 100, PLD.Config.PLD_AoE_Sheltron, SliderIncrements.Fives);
        }

        if (preset == Presets.PLD_AoE_Intervention)
        {
            UserConfig.DrawSliderInt(50, 100, PLD.Config.PLD_AoE_Intervention, SliderIncrements.Fives);
        }

        if (preset == Presets.PLD_ST_Intervene)
        {
            UserConfig.DrawAdditionalBoolChoice(PLD.Config.PLD_ST_InterveneSave, "Save one charge", "");
        }

        if (preset == Presets.PLD_AoE_Intervene)
        {
            UserConfig.DrawAdditionalBoolChoice(PLD.Config.PLD_AoE_InterveneSave, "Save one charge", "");
        }

        if (preset == Presets.PLD_ST_Invuln)
        {
            UserConfig.DrawSliderInt(0, 100, PLD.Config.PLD_ST_Invuln, SliderIncrements.Ones);
        }

        if (preset == Presets.PLD_AoE_Invuln)
        {
            UserConfig.DrawSliderInt(0, 100, PLD.Config.PLD_AoE_Invuln, SliderIncrements.Ones);
        }

        #region PvP

        if (preset is Presets.PLDPvP_Phalanx)
        {
            UserConfig.DrawSliderInt(0, 100, PLDPvP.Config.PLDPvP_Phalanx, SliderIncrements.Fives);
        }

        #endregion

        #endregion

        #region Warrior

        if (preset == Presets.WAR_ST_StormsEye)
        {
            UserConfig.DrawSliderInt(0, 30, WAR.Config.WAR_SurgingRefresh, SliderIncrements.Ones);
        }

        if (preset == Presets.WAR_ST_FellCleave)
        {
            UserConfig.DrawSliderInt(50, 100, WAR.Config.WAR_FellCleaveGauge, SliderIncrements.Fives);
        }

        if (preset == Presets.WAR_AoE_Decimate)
        {
            UserConfig.DrawSliderInt(50, 100, WAR.Config.WAR_DecimateGauge, SliderIncrements.Fives);
        }

        if (preset == Presets.WAR_ST_Onslaught)
        {
            UserConfig.DrawAdditionalBoolChoice(WAR.Config.WAR_ST_OnslaughtSave, "Save one charge", "");
        }

        if (preset == Presets.WAR_AoE_Onslaught)
        {
            UserConfig.DrawAdditionalBoolChoice(WAR.Config.WAR_AoE_OnslaughtSave, "Save one charge", "");
        }

        if (preset == Presets.WAR_ST_Invuln)
        {
            UserConfig.DrawSliderInt(0, 100, WAR.Config.WAR_ST_Invuln, SliderIncrements.Ones);
        }

        if (preset == Presets.WAR_AoE_Invuln)
        {
            UserConfig.DrawSliderInt(0, 100, WAR.Config.WAR_AoE_Invuln, SliderIncrements.Ones);
        }

        #region PvP

        if (preset is Presets.WARPvP_Bloodwhetting)
        {
            UserConfig.DrawSliderInt(0, 100, WARPvP.Config.WARPvP_Bloodwhetting, SliderIncrements.Fives);
        }

        #endregion

        #endregion

        #region Dark Knight

        if (preset == Presets.DRK_ST_Edge)
        {
            UserConfig.DrawSliderInt(0, 10000, DRK.Config.DRK_ST_ManaSaver, SliderIncrements.Thousands);
        }

        if (preset == Presets.DRK_AoE_Flood)
        {
            UserConfig.DrawSliderInt(0, 10000, DRK.Config.DRK_AoE_ManaSaver, SliderIncrements.Thousands);
        }

        if (preset == Presets.DRK_ST_Bloodspiller)
        {
            UserConfig.DrawSliderInt(50, 100, DRK.Config.DRK_BloodspillerGauge, SliderIncrements.Fives);
        }

        if (preset == Presets.DRK_AoE_Quietus)
        {
            UserConfig.DrawSliderInt(50, 100, DRK.Config.DRK_QuietusGauge, SliderIncrements.Fives);
        }

        if (preset == Presets.DRK_AoE_Abyssal)
        {
            UserConfig.DrawSliderInt(0, 100, DRK.Config.DRK_AoE_Abyssal, SliderIncrements.Ones);
        }

        if (preset == Presets.DRK_ST_Invuln)
        {
            UserConfig.DrawSliderInt(0, 100, DRK.Config.DRK_ST_Invuln, SliderIncrements.Ones);
        }

        if (preset == Presets.DRK_AoE_Invuln)
        {
            UserConfig.DrawSliderInt(0, 100, DRK.Config.DRK_AoE_Invuln, SliderIncrements.Ones);
        }

        #region PvP

        if (preset is Presets.DRKPvP_Shadowbringer)
        {
            UserConfig.DrawSliderInt(0, 100, DRKPvP.Config.DRKPvP_Shadowbringer, SliderIncrements.Fives);
        }

        if (preset is Presets.DRKPvP_Impalement)
        {
            UserConfig.DrawSliderInt(0, 100, DRKPvP.Config.DRKPvP_Impalement, SliderIncrements.Fives);
        }

        #endregion

        #endregion

        #region Gunbreaker

        if (preset == Presets.GNB_ST_Invuln)
        {
            UserConfig.DrawSliderInt(0, 100, GNB.Config.GNB_ST_Invuln, SliderIncrements.Ones);
        }

        if (preset == Presets.GNB_AoE_Invuln)
        {
            UserConfig.DrawSliderInt(0, 100, GNB.Config.GNB_AoE_Invuln, SliderIncrements.Ones);
        }

        #endregion

        #endregion

        #region Healers

        #region White Mage



        #region PvP



        #endregion

        #endregion

        #region Scholar



        #endregion

        #region Astrologian

        if (preset is Presets.AST_ST_DPS_AutoDraw)
        {
            UserConfig.DrawAdditionalBoolChoice(AST.Config.AST_ST_DPS_UseDefenseCards, "Use Arrow/Spire/Bole/Ewer", "");
        }

        if (preset is Presets.AST_AoE_DPS_AutoDraw)
        {
            UserConfig.DrawAdditionalBoolChoice(AST.Config.AST_AoE_DPS_UseDefenseCards, "Use Arrow/Spire/Bole/Ewer", "");
        }

        #region PvP

        if (preset is Presets.ASTPvP_AspectedBenefic)
        {
            UserConfig.DrawSliderInt(0, 100, ASTPvP.Config.ASTPvP_AspectedBenefic, SliderIncrements.Fives);
        }

        #endregion

        #endregion

        #region Sage

        if (preset is Presets.SGE_ST_DPS_Rhizo)
        {
            UserConfig.DrawSliderInt(0, 1, SGE.Config.SGE_ST_DPS_Rhizo, SliderIncrements.Ones);
        }

        if (preset is Presets.SGE_ST_DPS_AddersgallProtect)
        {
            UserConfig.DrawSliderInt(1, 3, SGE.Config.SGE_ST_DPS_AddersgallProtect, SliderIncrements.Ones);
        }

        if (preset is Presets.SGE_AoE_DPS_Rhizo)
        {
            UserConfig.DrawSliderInt(0, 1, SGE.Config.SGE_AoE_DPS_Rhizo, SliderIncrements.Ones);
        }

        if (preset is Presets.SGE_AoE_DPS_AddersgallProtect)
        {
            UserConfig.DrawSliderInt(1, 3, SGE.Config.SGE_AoE_DPS_AddersgallProtect, SliderIncrements.Ones);
        }

        #endregion

        #endregion

        #region Melee DPS

        #region Monk



        #endregion

        #region Dragoon



        #endregion

        #region Ninja

        #region PvP



        #endregion

        #endregion

        #region Samurai

        if (preset is Presets.SAM_ST_Shinten)
        {
            UserConfig.DrawAdditionalBoolChoice(SAM.Config.SAM_ST_SaveKenkiDash, "Save 10 Kenki for Gyoten", "");
        }

        if (preset is Presets.SAM_AoE_Kyuten)
        {
            UserConfig.DrawAdditionalBoolChoice(SAM.Config.SAM_AoE_SaveKenkiDash, "Save 10 Kenki for Gyoten", "");
        }

        #endregion

        #region Reaper



        #endregion

        #region Viper



        #endregion

        #region Beastmaster



        #endregion

        #endregion

        #region Physical Ranged DPS

        #region Bard



        #endregion

        #region Machinist

        if (preset is Presets.MCH_ST_Hypercharge)
        {
            UserConfig.DrawSliderInt(0, 100, MCH.Config.MCH_ST_Hypercharge, SliderIncrements.Fives);
        }

        if (preset is Presets.MCH_ST_Queen)
        {
            UserConfig.DrawSliderInt(0, 100, MCH.Config.MCH_ST_Queen, SliderIncrements.Fives);
        }

        if (preset is Presets.MCH_AoE_Hypercharge)
        {
            UserConfig.DrawSliderInt(0, 100, MCH.Config.MCH_AoE_Hypercharge, SliderIncrements.Fives);
        }

        #endregion

        #region Dancer



        #endregion

        #endregion

        #region Magical Ranged DPS

        #region Black Mage



        #endregion

        #region Summoner

        if (preset is Presets.SMN_ST_Astral)
        {
            UserConfig.DrawAdditionalBoolChoice(SMN.Config.SMN_ST_Astral_Swift, "Use Swiftcast on Slipstream", "");
            UserConfig.DrawAdditionalBoolChoice(SMN.Config.SMN_ST_Astral_Garuda, "Don't use for Garuda/Slipstream", "");
            UserConfig.DrawAdditionalBoolChoice(SMN.Config.SMN_ST_Astral_Ifrit, "Don't use for Ifrit/Crimson Strike", "");
        }

        if (preset is Presets.SMN_AoE_Astral)
        {
            UserConfig.DrawAdditionalBoolChoice(SMN.Config.SMN_AoE_Astral_Swift, "Use Swiftcast on Slipstream", "");
            UserConfig.DrawAdditionalBoolChoice(SMN.Config.SMN_AoE_Astral_Garuda, "Don't use for Garuda/Slipstream", "");
            UserConfig.DrawAdditionalBoolChoice(SMN.Config.SMN_AoE_Astral_Ifrit, "Don't use for Ifrit/Crimson Strike", "");
        }

        #endregion

        #region Red Mage



        #endregion

        #region Pictomancer

        if (preset == Presets.PCTPvP_AutoPalette)
        {
            UserConfig.DrawSliderInt(0, 100, PCTPvP.Config.PCTPvP_AutoPalette, SliderIncrements.Fives);
        }

        #endregion

        #region Blue Mage

        if (preset is Presets.BLU_Basic_BloodDrain)
        {
            UserConfig.DrawSliderInt(1000, 10000, BLU.Config.BLU_BloodDrain, SliderIncrements.Hundreds);
        }

        if (preset is Presets.BLU_Tank_WhiteWind)
        {
            UserConfig.DrawSliderInt(0, 100, BLU.Config.BLU_TankWhiteWind, SliderIncrements.Fives);
        }

        if (preset is Presets.BLU_Treasure_Tank_WhiteWind)
        {
            UserConfig.DrawSliderInt(0, 100, BLU.Config.BLU_TreasureWhiteWind, SliderIncrements.Fives);
        }

        if (preset is Presets.BLU_Treasure_Tank_Rehydration)
        {
            UserConfig.DrawSliderInt(0, 100, BLU.Config.BLU_TreasureRehydration, SliderIncrements.Fives);
        }

        if (preset is Presets.BLU_OffFlute)
        {
            UserConfig.DrawAdditionalBoolChoice(BLU.Config.BLU_FeatherRain, "Feather Rain", "");
            UserConfig.DrawAdditionalBoolChoice(BLU.Config.BLU_Eruption, "Eruption", "");
            UserConfig.DrawAdditionalBoolChoice(BLU.Config.BLU_MountainBuster, "Mountain Buster", "");
            UserConfig.DrawAdditionalBoolChoice(BLU.Config.BLU_ShockStrike, "Shock Strike", "");
            UserConfig.DrawAdditionalBoolChoice(BLU.Config.BLU_Quasar, "Quasar", "");
            UserConfig.DrawAdditionalBoolChoice(BLU.Config.BLU_JKick, "J Kick", "");
            UserConfig.DrawAdditionalBoolChoice(BLU.Config.BLU_RoseOfDestruction, "Rose of Destruction", "");
            UserConfig.DrawAdditionalBoolChoice(BLU.Config.BLU_WingedReprobation, "Winged Reprobation", "");
        }

        #endregion

        #endregion
    }
}

