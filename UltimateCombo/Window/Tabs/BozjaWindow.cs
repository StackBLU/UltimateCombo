using UltimateCombo.Core;

namespace UltimateCombo.Window.Tabs;

internal class BozjaWindow : ConfigWindow
{
    internal static new void Draw()
    {
        PresetTab.DrawContent(PresetStorage.IsBozja);
    }
}
