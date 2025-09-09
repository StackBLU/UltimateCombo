using FFXIVClientStructs.FFXIV.Client.UI.Agent;

namespace UltimateCombo.ComboHelper.Functions;

internal abstract partial class CustomComboFunctions
{
    internal static unsafe bool IsMoving => AgentMap.Instance() is not null && AgentMap.Instance()->IsPlayerMoving;
}
