using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvP
{
	internal static class MNKPvP
	{
		public const uint
			DragonKick = 29475,
			TwinSnakes = 29476,
			Demolish = 29477,
			LeapingOpo = 41444,
			RisingRaptor = 41445,
			PouncingCoeurl = 41446,
			PhantomRush = 29478,

			RisingPhoenix = 29481,

			FlintsReply = 41447,
			FiresReply = 41448,
			WindsReply = 41509,
			RiddleOfEarth = 29482,
			EarthsReply = 29483,

			Thunderclap = 29484,

			Meteodrive = 29485;

		public static class Buffs
		{
			public const ushort
				WindResonance = 2007,
				FiresRumination = 4301,
				FireResonance = 3170,
				EarthResonance = 3171,
				Thunderclap = 3173;
		}

		public static class Debuffs
		{
			public const ushort
				PressurePoint = 000000000000000000;
		}

		public static class Config
		{

		}

		internal class MNKPvP_Combo : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MNKPvP_Combo;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is DragonKick or TwinSnakes or Demolish or LeapingOpo or RisingRaptor or PouncingCoeurl or PhantomRush)
					&& IsEnabled(CustomComboPreset.MNKPvP_Combo))
				{
					if (IsEnabled(CustomComboPreset.MNKPvP_Meteodrive) && GetLimitBreakCurrentValue() == GetLimitBreakMaxValue() && TargetHasEffect(Debuffs.PressurePoint))
					{
						return Meteodrive;
					}

					if (!TargetHasEffectAny(PvPCommon.Buffs.Guard))
					{
						if (CanWeave(actionID))
						{
							if (IsEnabled(CustomComboPreset.MNKPvP_RiddleOfEarth) && ActionReady(RiddleOfEarth))
							{
								return RiddleOfEarth;
							}

							if (IsEnabled(CustomComboPreset.MNKPvP_Thunderclap) && ActionReady(Thunderclap) && (!InMeleeRange() || !HasEffect(Buffs.Thunderclap)))
							{
								return Thunderclap;
							}

							if (IsEnabled(CustomComboPreset.MNKPvP_RisingPhoenix) && ActionReady(RisingPhoenix) && WasLastWeaponskill(WindsReply)
								&& !HasEffect(Buffs.FireResonance))
							{
								return RisingPhoenix;
							}
						}

						if (IsEnabled(CustomComboPreset.MNKPvP_RiddleOfEarth) && IsEnabled(EarthsReply) && GetBuffRemainingTime(Buffs.EarthResonance) < 1)
						{
							return EarthsReply;
						}

						if (IsEnabled(CustomComboPreset.MNKPvP_FlintsReply) && ActionReady(FlintsReply) && !InMeleeRange())
						{
							return FlintsReply;
						}

						if (IsEnabled(CustomComboPreset.MNKPvP_WindsReply) && ActionReady(FlintsReply) && WasLastWeaponskill(PouncingCoeurl))
						{
							return WindsReply;
						}
					}
				}

				return actionID;
			}
		}
	}
}