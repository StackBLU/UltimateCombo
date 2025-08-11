using FFXIVClientStructs.FFXIV.Client.UI.Agent;

namespace UltimateCombo.ComboHelper.Functions
{
	internal abstract partial class CustomComboFunctions
	{
		/// <summary> Checks player movement </summary>
		public static unsafe bool IsMoving => AgentMap.Instance() is not null && AgentMap.Instance()->IsPlayerMoving;
	}
}
