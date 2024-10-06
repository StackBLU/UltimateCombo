using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvP
{
	internal static class MNKPvP
	{
		public const byte ClassID = 2;
		public const byte JobID = 20;

		public const uint
			PhantomRushCombo = 55,
			Bootshine = 29472,
			TrueStrike = 29473,
			SnapPunch = 29474,
			DragonKick = 29475,
			TwinSnakes = 29476,
			Demolish = 29477,
			PhantomRush = 29478,
			SixSidedStar = 29479,
			Enlightenment = 29480,
			RisingPhoenix = 29481,
			RiddleOfEarth = 29482,
			ThunderClap = 29484,
			EarthsReply = 29483,
			Meteodrive = 29485;

		public static class Buffs
		{
			public const ushort
				WindResonance = 2007,
				FireResonance = 3170,
				EarthResonance = 3171;
		}

		public static class Debuffs
		{
			public const ushort
				PressurePoint = 3172;
		}

		internal class MNKPvP_Combo : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MNKPvP_Combo;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (actionID is Bootshine && IsEnabled(CustomComboPreset.MNKPvP_Combo))
				{
					if (IsEnabled(CustomComboPreset.MNKPvP_MeteoDrive) && GetLimitBreakCurrentValue() == GetLimitBreakMaxValue()
						&& TargetHasEffect(Debuffs.PressurePoint))
					{
						return Meteodrive;
					}

					if (!TargetHasEffectAny(PvPCommon.Buffs.Guard))
					{
						if (CanWeave(actionID))
						{
							if (IsEnabled(CustomComboPreset.MNKPvP_SixSidedStar) && ActionReady(SixSidedStar) && lastComboMove is TwinSnakes)
							{
								return SixSidedStar;
							}

							if (IsEnabled(CustomComboPreset.MNKPvP_Rising) && ActionReady(RisingPhoenix) && !HasEffect(Buffs.FireResonance)
								&& lastComboMove is Demolish)
							{
								return RisingPhoenix;
							}

							if (IsEnabled(CustomComboPreset.MNKPvP_RiddleOfEarth) && HasEffect(Buffs.EarthResonance)
								&& (GetBuffRemainingTime(Buffs.EarthResonance) < 5 || PlayerHealthPercentageHp() <= 50))
							{
								return EarthsReply;
							}
						}

						if (IsEnabled(CustomComboPreset.MNKPvP_Enlightenment) && ActionReady(Enlightenment) && HasEffect(Buffs.FireResonance))
						{
							return Enlightenment;
						}

						if (IsEnabled(CustomComboPreset.MNKPvP_Enlightenment) && ActionReady(RiddleOfEarth) && PlayerHealthPercentageHp() <= 95)
						{
							return RiddleOfEarth;
						}

						if (IsEnabled(CustomComboPreset.MNKPvP_Thunderclap) && ActionReady(ThunderClap) && !HasEffect(Buffs.WindResonance))
						{
							return ThunderClap;
						}
					}
				}

				return actionID;
			}
		}
	}
}