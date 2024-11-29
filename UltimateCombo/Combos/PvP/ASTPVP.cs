using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvP
{
	internal static class ASTPvP
	{
		public const uint
			FallMalefic = 29242,
			AspectedBenefic = 29243,
			Gravity2 = 29244,
			DoubleCast = 29245,

			MinorArcana = 41503,
			LadyOfCrowns = 41504,
			LordOfCrowns = 41505,

			Macrocosmos = 29253,
			Microcosmos = 29254,

			Epicycle = 41506,
			Retrograde = 41507,

			CelestialRiver = 29255,
			Oracle = 41508;

		public static class Buffs
		{
			public const ushort
				DiurnalBenefic = 3099,
				NocturnalBenefic = 3100,

				LadyOfCrownsCard = 4328,
				LadyOfCrownsBuff = 1452,
				LordOfCrownsCard = 4329,

				Macrocosmos = 3104,

				RetrogradeReady = 4331,
				Epicycle = 4330,

				CelestialRiver = 3105,
				Divining = 4332;
		}

		internal class Debuffs
		{
			internal const ushort
				LordOfCrownsDebuff = 1451,
				CelestialTide = 3106;
		}

		public static class Config
		{
			public static UserInt
				ASTPvP_AspectedBenefic = new("ASTPvP_AspectedBenefic", 75);
		}

		internal class ASTPvP_Combo : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.ASTPvP_Combo;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (actionID is FallMalefic && IsEnabled(CustomComboPreset.ASTPvP_Combo))
				{
					if (!TargetHasEffectAny(PvPCommon.Buffs.Guard))
					{
						if (CanWeave(actionID))
						{
							if (IsEnabled(CustomComboPreset.ASTPvP_MinorArcana) && ActionReady(MinorArcana))
							{
								return MinorArcana;
							}
						}

						if (IsEnabled(CustomComboPreset.ASTPvP_AspectedBenefic) && ActionReady(AspectedBenefic)
							&& PlayerHealthPercentageHp() < GetOptionValue(Config.ASTPvP_AspectedBenefic))
						{
							return AspectedBenefic;
						}
					}
				}

				return actionID;
			}
		}
	}
}