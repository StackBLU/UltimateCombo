using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvP
{
	internal static class SGEPvP
	{
		public const uint
			Dosis3 = 29256,
			Eukrasia = 29258,
			EukrasianDosis3 = 29257,

			Phlegma3 = 29259,
			Psyche = 41658,

			Pneuma = 29260,
			Icarus = 29261,

			Toxikon = 29262,
			Toxikon2 = 29263,

			Kardia = 29264,

			Mesotes = 29266;

		public static class Buffs
		{
			public const ushort
				Kardia = 2871,
				Kardion = 2872,

				Haima = 3110,
				Haimatinon = 3111,

				Eukrasia = 3107,
				EukrasianDiagnosis = 3109,

				Mesotes = 3118,
				MesotesBuff = 3119;
		}

		internal class Debuffs
		{
			internal const ushort
				Toxikon = 3113,
				EukrasianDosis = 3108,
				Lype = 3120;
		}

		internal class SGEPvP_Combo : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SGEPvP_Combo;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Dosis3 or EukrasianDosis3) && IsEnabled(CustomComboPreset.SGEPvP_Combo))
				{
					if (IsEnabled(CustomComboPreset.SGEPvP_Kardia) && !HasEffect(Buffs.Kardia))
					{
						return Kardia;
					}

					if (!TargetHasEffectAny(PvPCommon.Buffs.Guard))
					{
						if (CanWeave(actionID))
						{
							if (IsEnabled(CustomComboPreset.SGEPvP_Toxikon) && ActionReady(OriginalHook(Toxikon)) && !TargetHasEffect(Debuffs.Toxikon)
								&& !WasLastAction(OriginalHook(Toxikon)))
							{
								return OriginalHook(Toxikon);
							}
						}

						if (IsEnabled(CustomComboPreset.SGEPvP_Eukrasia) && ActionReady(Eukrasia) && !HasEffect(Buffs.Eukrasia)
							&& !HasEffect(Buffs.EukrasianDiagnosis) && !TargetHasEffect(Debuffs.EukrasianDosis)
							&& (TargetHasEffect(Debuffs.Toxikon) || (GetRemainingCharges(Toxikon) == 0 && GetCooldownChargeRemainingTime(Toxikon) > 5)))
						{
							return Eukrasia;
						}

						if (IsEnabled(CustomComboPreset.SGEPvP_Pneuma) && ActionReady(Pneuma))
						{
							return Pneuma;
						}

						if (IsEnabled(CustomComboPreset.SGEPvP_Phlegma) && ActionReady(Phlegma3) && InActionRange(Phlegma3))
						{
							return Phlegma3;
						}

						if (IsEnabled(CustomComboPreset.SGEPvP_Psyche) && IsEnabled(Psyche))
						{
							return Psyche;
						}
					}
				}

				return actionID;
			}
		}
	}
}
