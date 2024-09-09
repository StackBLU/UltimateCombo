using FFXIVClientStructs.FFXIV.Client.UI.Agent;

namespace StackCombo.ComboHelper.Functions
{
	internal abstract partial class CustomComboFunctions
	{
		/// <summary> Checks player movement </summary>
		public static unsafe bool IsMoving
		{
			get
			{
				return AgentMap.Instance() is not null && AgentMap.Instance()->IsPlayerMoving > 0;
			}
		}
	}
}