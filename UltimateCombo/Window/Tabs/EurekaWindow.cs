using UltimateCombo.Core;

namespace UltimateCombo.Window.Tabs;

internal class EurekaWindow : ConfigWindow
{
    internal static new void Draw()
    {
        PresetTab.DrawContent(PresetStorage.IsEureka);
    }
}
