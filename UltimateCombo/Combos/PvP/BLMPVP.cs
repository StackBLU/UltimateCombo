using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvP
{
	internal static class BLMPvP
	{
		public const byte JobID = 25;

		public const uint
			Fire = 29649,
			Blizzard = 29653,
			Burst = 29657,
			Paradox = 29663,
			NightWing = 29659,
			AetherialManipulation = 29660,
			Superflare = 29661,
			Fire4 = 29650,
			Flare = 29651,
			Blizzard4 = 29654,
			Freeze = 29655,
			Foul = 29371,
			SoulResonance = 29662;

		public static class Buffs
		{
			public const ushort
				AstralFire2 = 3212,
				AstralFire3 = 3213,
				UmbralIce2 = 3214,
				UmbralIce3 = 3215,
				Burst = 3221,
				SoulResonance = 3222,
				Polyglot = 3169,
				Swiftcast = 1325;
		}

		public static class Debuffs
		{
			public const ushort
				AstralWarmth = 3216,
				UmbralFreeze = 3217,
				Burns = 3218,
				DeepFreeze = 3219;
		}

		internal class BLMPvP_Combo : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLMPvP_Combo;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Fire or Blizzard) && IsEnabled(CustomComboPreset.BLMPvP_Combo))
				{
					if (IsEnabled(CustomComboPreset.BLMPvP_SoulResonance) && GetLimitBreakCurrentValue() == GetLimitBreakMaxValue())
					{
						return SoulResonance;
					}

					if (IsEnabled(CustomComboPreset.BLMPvP_Foul)
						&& HasEffect(Buffs.Polyglot) && GetBuffRemainingTime(Buffs.Polyglot) < 5)
					{
						return Foul;
					}

					if (IsEnabled(CustomComboPreset.BLMPvP_Burst) && ActionReady(Burst) && HasEffect(Buffs.Swiftcast))
					{
						return Burst;
					}
				}

				return actionID;
			}
		}
	}
}