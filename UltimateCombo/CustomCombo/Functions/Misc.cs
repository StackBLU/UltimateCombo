using System.Collections.Generic;
using UltimateCombo.Combos;
using UltimateCombo.Combos.PvE;
using UltimateCombo.Core;

namespace UltimateCombo.ComboHelper.Functions
{
	internal abstract partial class CustomComboFunctions
	{
		/// <summary> Determine if the given preset is enabled. </summary>
		/// <param name="preset"> Preset to check. </param>
		/// <returns> A value indicating whether the preset is enabled. </returns>
		public static bool IsEnabled(CustomComboPreset preset)
		{
			return (int)preset < 100 || PresetStorage.IsEnabled(preset);
		}

		/// <summary> Determine if the given preset is not enabled. </summary>
		/// <param name="preset"> Preset to check. </param>
		/// <returns> A value indicating whether the preset is not enabled. </returns>
		public static bool IsNotEnabled(CustomComboPreset preset)
		{
			return !IsEnabled(preset);
		}

		public class JobIDs
		{
			//  Job IDs     ClassIDs (no jobstone) (Lancer, Pugilist, etc)
			public static readonly List<byte> Melee =
			[
				DRG.JobID,
				MNK.JobID,
				NIN.JobID,
				VPR.JobID,
				RPR.JobID,
				SAM.JobID
			];

			public static readonly List<byte> Ranged =
			[
				BLM.JobID,
				BRD.JobID,
				SMN.JobID,
				PCT.JobID,
				MCH.JobID,
				RDM.JobID,
				DNC.JobID,
				BLU.JobID
			];

			public static readonly List<byte> Tank =
			[
				PLD.JobID,
				WAR.JobID,
				DRK.JobID,
				GNB.JobID
			];

			public static readonly List<byte> Healer =
			[
				WHM.JobID,
				SCH.JobID,
				AST.JobID,
				SGE.JobID
			];

		}
	}
}
