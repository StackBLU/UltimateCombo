using Dalamud.Interface.Utility;
using System.Linq;
using UltimateCombo.Combos;
using UltimateCombo.Core;
using UltimateCombo.Window.Functions;

namespace UltimateCombo.Window.Tabs
{
    internal class EurekaWindow : ConfigWindow
    {
        internal static new void Draw()
        {
            var i = 1;

            foreach ((CustomComboPreset preset, Attributes.CustomComboInfoAttribute info)
                    in GroupedPresets["General"].Where(x => PresetStorage.IsEureka(x.Preset)))
            {
                InfoBox presetBox = new()
                {
                    Color = Colors.Grey,
                    BorderThickness = 1f,
                    CurveRadius = 8f,
                    ContentsAction = () =>
                    {
                        Presets.DrawPreset(preset, info, ref i);
                    }
                };

                presetBox.Draw();
                ImGuiHelpers.ScaledDummy(12.0f);
            }
        }
    }
}
