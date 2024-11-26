using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvP
{
	internal static class BLMPvP
	{
		public const uint
			Fire = 29649,
			Fire3 = 30896,
			Fire4 = 29650,
			HighFire2 = 41473,
			Flare = 29651,

			Blizzard = 29653,
			Blizzard3 = 30897,
			Blizzard4 = 29654,
			HighBlizzard2 = 41474,
			Freeze = 29655,

			Paradox = 29663,
			Burst = 29657,
			Xenoglossy = 29658,
			Lethargy = 41510,
			AetherialManipulation = 29660,

			ElementalWave = 41475,
			WreathOfFire = 41476,
			WreathOfIce = 41478,

			SoulResonance = 29662,
			FlareStar = 41480,
			FrostStar = 41481;

		public static class Buffs
		{
			public const ushort
				Paradox = 3223,

				AstralFire1 = 3212,
				AstralFire2 = 3213,
				AstralFire3 = 3381,

				UmbralIce1 = 3214,
				UmbralIce2 = 3215,
				UmbralIce3 = 3382,

				Burst = 3221,

				WreathOfIce = 4316,
				WreathOfFice = 4315,

				SoulResonance = 3222,
				ElementalStar = 4317;
		}

		internal class Debuffs
		{
			internal const ushort
				Lethargy = 4333;
		}

		internal class BLMPvP_Combo : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLMPvP_Combo;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Fire or Fire3 or Fire4 or HighFire2 or Flare or Blizzard or Blizzard3 or Blizzard4 or HighBlizzard2 or Freeze)
					&& IsEnabled(CustomComboPreset.BLMPvP_Combo))
				{
					if (IsEnabled(CustomComboPreset.BLMPvP_SoulResonance) && GetLimitBreakCurrentValue() == GetLimitBreakMaxValue())
					{
						return SoulResonance;
					}

					if (!TargetHasEffectAny(PvPCommon.Buffs.Guard))
					{
						if (CanWeave(actionID))
						{
							if (IsEnabled(CustomComboPreset.BLMPvP_Lethargy) && ActionReady(Lethargy))
							{
								return Lethargy;
							}
						}

						if (IsEnabled(CustomComboPreset.BLMPvP_Burst) && WasLastAbility(AetherialManipulation))
						{
							return Burst;
						}

						if (IsEnabled(CustomComboPreset.BLMPvP_Xenoglossy) && ActionReady(Xenoglossy)
							&& (GetRemainingCharges(Xenoglossy) == GetMaxCharges(Xenoglossy) || PlayerHealthPercentageHp() <= 50))
						{
							return Paradox;
						}

						if (IsEnabled(CustomComboPreset.BLMPvP_Paradox) && HasEffect(Buffs.Paradox))
						{
							return Paradox;
						}

						if (IsEnabled(CustomComboPreset.BLMPvP_Swap) && WasLastSpell(HighFire2))
						{
							return Blizzard;
						}

						if (IsEnabled(CustomComboPreset.BLMPvP_Swap) && WasLastSpell(HighBlizzard2))
						{
							return Fire;
						}
					}
				}

				return actionID;
			}
		}
	}
}