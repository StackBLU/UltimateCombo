using StackCombo.CustomCombo;

namespace StackCombo.Combos.PvP
{
	internal static class SCHPvP
	{
		public const byte JobID = 28;

		public const uint
			Broil = 29231,
			Adloquium = 29232,
			Biolysis = 29233,
			DeploymentTactics = 29234,
			Mummification = 29235,
			Expedient = 29236,
			Seraph = 29237,
			Consolation = 29238;

		internal class Buffs
		{
			internal const ushort
				Catalyze = 3088,
				Recitation = 3094,
				Seraph = 3095;
		}
		internal class Debuffs
		{
			internal const ushort
				Biolysis = 3089,
				Biolytic = 3090;
		}

		internal class SCHPvP_Combo : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SCHPvP_Combo;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (actionID is Broil && IsEnabled(CustomComboPreset.SCHPvP_Combo))
				{
					if (IsEnabled(CustomComboPreset.SCHPvP_Seraph) && GetLimitBreakCurrentValue() == GetLimitBreakMaxValue()
						&& !HasEffect(Buffs.Recitation) && GetCooldownRemainingTime(Expedient) > 10 && GetCooldownRemainingTime(Biolysis) <= 5
						&& GetBuffRemainingTime(Buffs.Catalyze) >= 10)
					{
						return Seraph;
					}

					if (!TargetHasEffectAny(PvPCommon.Buffs.Guard))
					{
						if (CanWeave(actionID))
						{
							if (IsEnabled(CustomComboPreset.SCHPvP_Expedient) && ActionReady(Expedient) && !HasEffect(Buffs.Recitation)
								&& GetBuffRemainingTime(Buffs.Catalyze) >= 10)
							{
								return Expedient;
							}

							if (IsEnabled(CustomComboPreset.SCHPvP_Deploy) && ActionReady(DeploymentTactics)
								&& TargetHasEffect(Debuffs.Biolysis) && GetDebuffRemainingTime(Debuffs.Biolytic) >= 10
								&& !WasLastAction(DeploymentTactics))
							{
								return DeploymentTactics;
							}

							if (IsEnabled(CustomComboPreset.SCHPvP_Mummy) && ActionReady(Mummification) && InActionRange(Mummification))
							{
								return Mummification;
							}

							if (IsEnabled(CustomComboPreset.SCHPvP_Seraph) && ActionReady(Consolation) && HasEffect(Buffs.Seraph))
							{
								return Consolation;
							}
						}
					}

					if (IsEnabled(CustomComboPreset.SCHPvP_Adloquium) && ActionReady(Adloquium)
						&& (!HasEffect(Buffs.Catalyze) || GetBuffRemainingTime(Buffs.Catalyze) <= 1 || !InCombat())
						&& !HasEffect(Buffs.Recitation))
					{
						return Adloquium;
					}

					if (IsEnabled(CustomComboPreset.SCHPvP_Expedient) && ActionReady(Expedient) && GetBuffRemainingTime(Buffs.Catalyze) >= 10
						&& !InCombat())
					{
						return Expedient;
					}

					if (IsEnabled(CustomComboPreset.SCHPvP_Bio) && GetBuffRemainingTime(Buffs.Catalyze) >= 1
						&& (GetCooldownRemainingTime(Expedient) >= 2 || HasEffect(Buffs.Recitation))
						&& (ActionReady(Biolysis) || GetCooldownRemainingTime(Biolysis) <= 2.4))
					{
						return Biolysis;
					}
				}

				return actionID;
			}
		}
	}
}