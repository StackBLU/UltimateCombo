using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvP
{
	internal static class BRDPvP
	{
		public const uint
			PowerfulShot = 29391,
			SilentNocturne = 29395,
			RepellingShot = 29399,
			PitchPerfect = 29392,

			ApexArrow = 29393,
			BlastArrow = 29394,

			HarmonicArrow = 41964,

			TheWardensPaean = 29400,

			FinalFantasia = 29401,
			EncoreOfLight = 41467;

		public static class Buffs
		{
			public const ushort
				FrontlinersMarch = 3138,
				FrontlineMarch = 3139,
				BlastArrowReady = 3142,

				Repertoire = 3137,
				TheWardensPaean = 3143,

				EncoreOfLightReady = 4312,
				FinalFantasia = 3144,
				HeroesFantasia = 3145;
		}

		internal class BRDPvP_Combo : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BRDPvP_Combo;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is PowerfulShot or PitchPerfect) && IsEnabled(CustomComboPreset.BRDPvP_Combo))
				{
					if (IsEnabled(CustomComboPreset.BRDPvP_FinalFantasia) && GetLimitBreakCurrentValue() == GetLimitBreakMaxValue()
						&& !IsMoving)
					{
						return FinalFantasia;
					}

					if (!TargetHasEffectAny(PvPCommon.Buffs.Guard))
					{
						if (CanWeave(actionID))
						{
							if (IsEnabled(CustomComboPreset.BRDPvP_SilentNocturne) && ActionReady(SilentNocturne) && !HasEffect(Buffs.Repertoire))
							{
								return SilentNocturne;
							}

							if (IsEnabled(CustomComboPreset.BRDPvP_RepellingShot) && ActionReady(RepellingShot) && InActionRange(RepellingShot)
								&& !HasEffect(Buffs.Repertoire))
							{
								return RepellingShot;
							}
						}

						if (IsEnabled(CustomComboPreset.BRDPvP_HarmonicArrow) && ActionReady(HarmonicArrow)
							&& GetRemainingCharges(HarmonicArrow) == GetMaxCharges(HarmonicArrow))
						{
							return HarmonicArrow;
						}

						if (IsEnabled(CustomComboPreset.BRDPvP_ApexArrow) && ActionReady(ApexArrow))
						{
							return ApexArrow;
						}

						if (IsEnabled(CustomComboPreset.BRDPvP_BlastArrow) && IsEnabled(BlastArrow))
						{
							return BlastArrow;
						}

						if (IsEnabled(CustomComboPreset.BRDPvP_EncoreOfLight) && HasEffect(Buffs.EncoreOfLightReady))
						{
							return EncoreOfLight;
						}
					}
				}

				return actionID;
			}
		}
	}
}