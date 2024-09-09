using StackCombo.ComboHelper.Functions;
using StackCombo.Combos;
using StackCombo.Combos.PvE;

namespace StackCombo.CustomCombo
{
	internal abstract partial class CustomComboClass : CustomComboFunctions
	{

		protected CustomComboClass()
		{
			StartTimer();
		}

		protected internal abstract CustomComboPreset Preset { get; }

		protected byte ClassID { get; }
		protected byte JobID { get; }

		public unsafe bool TryInvoke(uint actionID, byte level, uint lastComboMove, float comboTime, out uint newActionID)
		{
			newActionID = 0;

			if (!IsEnabled(Preset))
			{
				return false;
			}

			uint classJobID = LocalPlayer!.ClassJob.Id;

			if (classJobID is >= 16 and <= 18)
			{
				classJobID = FSH.JobID;
			}

			if (JobID != ADV.JobID && JobID != classJobID && ClassID != classJobID)
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
