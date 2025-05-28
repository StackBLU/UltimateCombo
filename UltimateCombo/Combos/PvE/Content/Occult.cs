using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvE.Content
{
	public static class Occult
	{
		public const uint
			OccultResuscitation = 41650,
			OccultTreasureSight = 41651,

			PhantomGuard = 41588,
			Pray = 41589,
			OccultHeal = 41590,
			Pledge = 41591,

			PhantomKick = 41595,
			OccultCounter = 41596,
			Counterstance = 41597,
			OccultChakra = 41598,

			OccultSlowga = 41621,
			OccultComet = 41622,
			OccultMageMasher = 41623,
			OccultDispel = 41624,
			OccultQuick = 41625,

			OffensiveAria = 41608,
			RomeosBallad = 41609,
			MightyMarch = 41607,
			HerosRime = 41610,

			Predict = 41636,
			PhantomJudgement = 41637,
			Cleansing = 41638,
			Blessing = 41639,
			Starfall = 41640,
			Recuperation = 41641,
			PhantomDoom = 41642,
			PhantomRejuvenation = 41643,
			Invulnerability = 41644,

			PhantomFire = 41626,
			HolyCannon = 41627,
			DarkCannon = 41628,
			ShockCannon = 41629,
			SilverCannon = 41630;

		public static class PhantomJobs
		{
			public const ushort
				Freelancer = 4242,
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
				Pray = 4232,

				PhantomKick = 4237,
				Counterstance = 4238,

				OffensiveAria = 4247,
				HerosRime = 4249,

				PredictionOfJudgement = 4265,
				PredictionOfCleansing = 4266,
				PredictionOfBlessing = 4267,
				PredictionOfStarfall = 4268;
		}

		public static class Debuffs
		{
			public const ushort
				Slow = 3493,

				OccultMageMasher = 4259;
		}

		public static class Config
		{
			public static UserInt
				Occult_PhantomResuscitation = new("Occult_PhantomResuscitation", 65),
				Occult_Pray = new("Occult_Pray", 50),
				Occult_Heal = new("Occult_Heal", 50);

			public static UserIntArray
				Occult_Prediction = new("Occult_Prediction"),
				Occult_HolySilverCannon = new("Occult_HolySilverCannon"),
				Occult_DarkShockCannon = new("Occult_DarkShockCannon");
		}

		internal class Occult_Freelancer : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.Occult_Freelancer;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.Occult_Freelancer) && HasEffect(PhantomJobs.Freelancer) && InCombat())
				{
					if (IsEnabled(CustomComboPreset.Occult_PhantomResuscitation) && DutyActionReady(OccultResuscitation)
						&& PlayerHealthPercentageHp() < GetOptionValue(Config.Occult_PhantomResuscitation))
					{
						return OccultResuscitation;
					}
				}

				return actionID;
			}
		}

		internal class Occult_Knight : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.Occult_Knight;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.Occult_Knight) && HasEffect(PhantomJobs.Knight) && InCombat())
				{
					if (IsEnabled(CustomComboPreset.Occult_Pray) && DutyActionReady(Pray)
						&& !HasEffect(Buffs.Pray) && PlayerHealthPercentageHp() < GetOptionValue(Config.Occult_Pray))
					{
						return Pray;
					}

					if (IsEnabled(CustomComboPreset.Occult_Heal) && DutyActionReady(OccultHeal)
						&& PlayerHealthPercentageHp() < GetOptionValue(Config.Occult_Heal))
					{
						return OccultHeal;
					}
				}

				return actionID;
			}
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

		internal class Occult_TimeMage : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.Occult_TimeMage;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.Occult_TimeMage) && HasEffect(PhantomJobs.TimeMage) && InCombat())
				{
					if (IsEnabled(CustomComboPreset.Occult_Slowga) && DutyActionReady(OccultSlowga) && !TargetHasEffectAny(Debuffs.Slow))
					{
						return OccultSlowga;
					}

					if (IsEnabled(CustomComboPreset.Occult_Comet) && DutyActionReady(OccultComet))
					{
						if (ActionReady(All.Swiftcast))
						{
							return All.Swiftcast;
						}

						if (HasEffect(All.Buffs.Swiftcast))
						{
							return OccultComet;
						}
					}

					if (IsEnabled(CustomComboPreset.Occult_MageMasher) && DutyActionReady(OccultMageMasher) && !TargetHasEffectAny(Debuffs.OccultMageMasher))
					{
						return OccultMageMasher;
					}

					/*if (IsEnabled(CustomComboPreset.Occult_Dispel) && DutyActionReady(OccultDispel))
					{
						return OccultDispel;
					}*/

					if (IsEnabled(CustomComboPreset.Occult_Quick) && DutyActionReady(OccultQuick))
					{
						return OccultQuick;
					}
				}

				return actionID;
			}
		}

		internal class Occult_Bard : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.Occult_Bard;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.Occult_Bard) && HasEffect(PhantomJobs.Bard) && InCombat())
				{
					if (IsEnabled(CustomComboPreset.Occult_HerosRime) && DutyActionReady(HerosRime))
					{
						return HerosRime;
					}

					if (IsEnabled(CustomComboPreset.Occult_OffensiveAria) && DutyActionReady(OffensiveAria) && !HasEffect(Buffs.HerosRime))
					{
						return OffensiveAria;
					}
				}

				return actionID;
			}
		}

		internal class Occult_Oracle : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.Occult_Oracle;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.Occult_Oracle) && HasEffect(PhantomJobs.Oracle) && InCombat())
				{
					if (IsEnabled(CustomComboPreset.Occult_Predict) && DutyActionReady(Predict))
					{
						return Predict;
					}

					if (IsEnabled(CustomComboPreset.Occult_Predict))
					{
						if (DutyActionReady(PhantomJudgement) && GetOptionValue(Config.Occult_Prediction) == 1)
						{
							return PhantomJudgement;
						}

						if (DutyActionReady(Cleansing) && GetOptionValue(Config.Occult_Prediction) == 2)
						{
							return Cleansing;
						}

						if (DutyActionReady(Blessing) && GetOptionValue(Config.Occult_Prediction) == 3)
						{
							return Blessing;
						}

						if (DutyActionReady(Starfall) && GetOptionValue(Config.Occult_Prediction) == 4)
						{
							return Starfall;
						}
					}

					if (IsEnabled(CustomComboPreset.Occult_PhantomRejuvination) && DutyActionReady(PhantomRejuvenation))
					{
						return PhantomRejuvenation;
					}
				}

				return actionID;
			}
		}

		internal class Occult_Cannoneer : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.Occult_Cannoneer;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.Occult_Cannoneer) && HasEffect(PhantomJobs.Cannoneer) && InCombat())
				{
					if (IsEnabled(CustomComboPreset.Occult_PhantomFire) && DutyActionReady(PhantomFire))
					{
						return PhantomFire;
					}

					if (IsEnabled(CustomComboPreset.Occult_HolySilverCannon))
					{
						if (DutyActionReady(HolyCannon) && GetOptionValue(Config.Occult_HolySilverCannon) == 1)
						{
							return HolyCannon;
						}

						if (DutyActionReady(SilverCannon) && GetOptionValue(Config.Occult_HolySilverCannon) == 2)
						{
							return SilverCannon;
						}
					}

					if (IsEnabled(CustomComboPreset.Occult_DarkShockCannon))
					{
						if (DutyActionReady(Blessing) && GetOptionValue(Config.Occult_DarkShockCannon) == 1)
						{
							return DarkCannon;
						}

						if (DutyActionReady(Starfall) && GetOptionValue(Config.Occult_DarkShockCannon) == 2)
						{
							return ShockCannon;
						}
					}
				}

				return actionID;
			}
		}
	}
}