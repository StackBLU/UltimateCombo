using ECommons.DalamudServices;
using FFXIVClientStructs.FFXIV.Client.Game;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Combos;
using UltimateCombo.Combos.PvE;

namespace UltimateCombo.CustomCombo
{
	internal abstract partial class CustomComboClass : CustomComboFunctions
	{
		protected internal abstract CustomComboPreset Preset { get; }

		protected byte ClassID { get; }
		protected byte JobID { get; }

		public unsafe bool TryInvoke(uint actionID, byte level, uint lastComboMove, float comboTime, out uint newActionID)
		{
			newActionID = 0;

			if (ActionManager.Instance()->QueuedActionType == ActionType.Action
				&& ActionManager.Instance()->QueuedActionId != actionID
				&& !Svc.ClientState.IsPvP)
			{
				return false;
			}

			if (!IsEnabled(Preset))
			{
				return false;
			}

			uint classJobID = LocalPlayer!.ClassJob.Value.RowId;

			if (JobID != All.JobID && JobID != classJobID && ClassID != classJobID)
			{
				return false;
			}

			uint resultingActionID = Invoke(actionID, lastComboMove, comboTime, level);
			if (resultingActionID == 0 || actionID == resultingActionID)
			{
				return false;
			}

			newActionID = resultingActionID;
			return true;
		}

		protected abstract uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level);
	}
}