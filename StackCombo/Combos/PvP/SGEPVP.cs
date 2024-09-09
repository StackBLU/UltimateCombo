using StackCombo.CustomCombo;

namespace StackCombo.Combos.PvP
{
	internal static class SGEPvP
	{
		internal const byte JobID = 40;

		internal const uint
			Dosis = 29256,
			Phlegma = 29259,
			Pneuma = 29260,
			Eukrasia = 29258,
			Icarus = 29261,
			Toxikon = 29262,
			Kardia = 29264,
			EukrasianDosis = 29257,
			Toxikon2 = 29263,
			Mesotes = 29266,
			MesotesRelocate = 29267;

		internal class Debuffs
		{
			internal const ushort
				EukrasianDosis = 3108,
				Toxikon = 3113;
		}

		internal class Buffs
		{
			internal const ushort
				Kardia = 2871,
				Kardion = 2872,
				Eukrasia = 3107,
				Diagnosis = 3109,
				Addersting = 3115,
				Haima = 3110,
				Haimatinon = 3111,
				Mesotes = 3118,
				MesotesBuff = 3119;
		}

		internal class SGEPvP_Combo : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SGEPvP_Combo;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (actionID is Dosis && IsEnabled(CustomComboPreset.SGEPvP_Combo))
				{
					if (IsEnabled(CustomComboPreset.SGEPvP_Kardia) && !HasEffect(Buffs.Kardia))
					{
						return Kardia;
					}

					if (IsEnabled(CustomComboPreset.SGEPvP_Mesotes) && ActionReady(MesotesRelocate)
						&& HasEffect(Buffs.Mesotes) && !HasEffect(Buffs.MesotesBuff))
					{
						return MesotesRelocate;
					}

					if (!TargetHasEffectAny(PvPCommon.Buffs.Guard))
					{
						if (CanWeave(actionID))
						{
							if (IsEnabled(CustomComboPreset.SGEPvP_Toxikon) && ActionReady(OriginalHook(Toxikon)) && !TargetHasEffect(Debuffs.Toxikon))
							{
								return OriginalHook(Toxikon);
							}
						}

						if (IsEnabled(CustomComboPreset.SGEPvP_Eukrasia) && ActionReady(Eukrasia) && !HasEffect(Buffs.Eukrasia)
							&& !HasEffect(Buffs.Diagnosis) && !TargetHasEffect(Debuffs.EukrasianDosis)
							&& (TargetHasEffect(Debuffs.Toxikon) || (GetRemainingCharges(Toxikon) == 0 && GetCooldownChargeRemainingTime(Toxikon) > 5)))
						{
							return Eukrasia;
						}

						if (IsEnabled(CustomComboPreset.SGEPvP_Pneuma) && ActionReady(Pneuma))
						{
							return Pneuma;
						}

						if (IsEnabled(CustomComboPreset.SGEPvP_Phlegma) && ActionReady(Phlegma) && InActionRange(Phlegma))
						{
							return Phlegma;
						}
					}
				}

				return actionID;
			}
		}
	}
}