using FFXIVClientStructs.FFXIV.Client.UI.Agent;

namespace UltimateCombo.ComboHelper.Functions;

internal abstract partial class CustomComboFunctions
{
    internal static unsafe bool IsMoving
    {
        get
        {
            AgentMap* map = AgentMap.Instance();
            return map != null && map->IsPlayerMoving;
        }
    }
}
