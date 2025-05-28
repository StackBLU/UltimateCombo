using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvE.Content
{
	public static class Occult
	{
		public const uint
			PhantomKick = 41595,
			OccultCounter = 41596,
			Counterstance = 41597,
			OccultChakra = 41598;

		public static class PhantomJobs
		{
			public const ushort
				Knight = 4358,
				Berserker = 4359,
				Monk = 4360,
				Ranger = 4361,
				Samurai = 4362,
				Bard = 4363,
				Geomancer = 4364,
				TimeMage = 4365,
				Cannoneer = 4366,
				Chemist = 4367,
				Oracle = 4368,
				Thief = 4369;
		}

		public static class Buffs
		{
			public const ushort
				PhantomKick = 4237,
				Counterstance = 4238;
		}

		public static class Debuffs
		{
			public const ushort
				FlareStar = 2440,
				RendArmor = 2441;
		}

		public static class Items
		{
			public const uint
				EtherKit = 38;
		}

		internal class Occult_Monk : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.Occult_Monk;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.Occult_Monk) && HasEffect(PhantomJobs.Monk) && InCombat())
				{
					if (IsEnabled(CustomComboPreset.Occult_PhantomKick) && DutyActionReady(PhantomKick))
					{
						return PhantomKick;
					}

					if (IsEnabled(CustomComboPreset.Occult_Counter) && DutyActionReady(OccultCounter))
					{
						return OccultCounter;
					}

					if (IsEnabled(CustomComboPreset.Occult_Counterstance) && DutyActionReady(Counterstance)
						&& CurrentTarget.TargetObject == LocalPlayer && GetBuffRemainingTime(Buffs.Counterstance) < 2)
					{
						return Counterstance;
					}

					if (IsEnabled(CustomComboPreset.Occult_Chakra) && DutyActionReady(OccultChakra) && PlayerHealthPercentageHp() < 25)
					{
						return OccultChakra;
					}
				}

				return actionID;
			}
		}
	}
}