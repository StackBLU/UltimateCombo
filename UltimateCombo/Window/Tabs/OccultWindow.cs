using UltimateCombo.Core;

namespace UltimateCombo.Window.Tabs;

internal class OccultWindow : ConfigWindow
{
    internal static new void Draw()
    {
        PresetTab.DrawContent(PresetStorage.IsOccult);
    }
}
