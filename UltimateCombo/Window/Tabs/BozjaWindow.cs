using Dalamud.Interface.Utility;
using System.Linq;
using UltimateCombo.Combos;
using UltimateCombo.Core;

namespace UltimateCombo.Window.Tabs;

internal class BozjaWindow : ConfigWindow
{
    internal static new void Draw()
    {
        var i = 1;
        foreach ((Presets preset, Attributes.CustomComboInfoAttribute info) in GroupedPresets["General"].Where(x => PresetStorage.IsBozja(x.Preset)))
        {
            var presetBox = new InfoBox()
            {
                Color = Colors.Grey,
                BorderThickness = 1f,
                CurveRadius = 8f,
                ContentsAction = () =>
                {
                    PresetHandler.DrawPreset(preset, info, ref i);
                }
            };
            presetBox.Draw();
            ImGuiHelpers.ScaledDummy(12.0f);
        }
    }
}
