using UltimateCombo.Core;

namespace UltimateCombo.Window.Tabs;

internal class VariantWindow : ConfigWindow
{
    internal static new void Draw()
    {
        PresetTab.DrawContent(PresetStorage.IsVariant);
    }
}
