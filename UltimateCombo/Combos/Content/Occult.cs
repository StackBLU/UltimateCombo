using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Combos.PvE;
using UltimateCombo.CustomCombo;
using UltimateCombo.Data;
using UltimateCombo.Services;

namespace UltimateCombo.Combos.Content
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

			PhantomAim = 41599,

			OccultSlowga = 41621,
			OccultDispel = 41622,
			OccultComet = 41623,
			OccultMageMasher = 41624,
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
			SilverCannon = 41630,

			Mineuchi = 41603,
			Shirahadori = 41604,
			Iainuki = 41605,
			Zeninage = 41606,

			BattleBell = 41611,
			Weather = 41612,
			Sunbath = 41613,
			CloudyCaress = 41614,
			BlessedRain = 41615,
			MistyMirage = 41616,
			HastyMirage = 41617,
			AetherialGain = 41618,
			RingingRespite = 41619,
			Suspend = 41620,

			Sprint = 41646,
			Steal = 41645,
			Vigilance = 41647,
			TrapDetection = 41648,
			PilferWeapon = 41649,

			DeadlyBlow = 41594,
			Rage = 41592;

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
				PredictionOfStarfall = 4268,
				Rejuvination = 44274,

				BattleBell = 4251,
				BattlesClangor = 4252,
				RingingRespite = 4257,

				Vigilance = 4277,

				Rage = 4235,
				PentUpRage = 4236,

				OccultQuick = 4260,
				OccultSwift = 4261;
		}

		public static class Debuffs
		{
			public const ushort
				Slow = 3493,
				OccultMageMasher = 4259,
				WeaponPilfered = 4279;
		}

		public static class Config
		{
			public static UserBool
				Occult_KickNotice = new("Occult_KickNotice");

			public static UserInt
				Occult_PhantomResuscitation = new("Occult_PhantomResuscitation", 70),
				Occult_Pray = new("Occult_Pray", 75),
				Occult_Heal = new("Occult_Heal", 25),
				Occult_Sunbath = new("Occult_Sunbath", 75);

			public static UserIntArray
				Occult_Prediction = new("Occult_Prediction"),
				Occult_HolySilverCannon = new("Occult_HolySilverCannon"),
				Occult_DarkShockCannon = new("Occult_DarkShockCannon");
		}

		internal class Occult_Knight : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.Occult_Knight;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.Occult_Knight) && HasEffect(PhantomJobs.Knight) && InCombat() && SafeToUse())
				{
					if (IsEnabled(CustomComboPreset.Occult_Pray) && DutyActionReady(Pray) && !HasEffect(Buffs.Pray)
						&& PlayerHealthPercentageHp() <= GetOptionValue(Config.Occult_Pray))
					{
						return Pray;
					}

					if (IsEnabled(CustomComboPreset.Occult_Heal) && DutyActionReady(OccultHeal)
						&& PlayerHealthPercentageHp() <= GetOptionValue(Config.Occult_Heal)
						&& LocalPlayer.CurrentMp >= 5000)
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
				if (IsEnabled(CustomComboPreset.Occult_Monk) && HasEffect(PhantomJobs.Monk) && InCombat() && SafeToUse())
				{
					if (IsEnabled(CustomComboPreset.Occult_Chakra) && DutyActionReady(OccultChakra) && PlayerHealthPercentageHp() < 30)
					{
						return OccultChakra;
					}

					if (IsEnabled(CustomComboPreset.Occult_PhantomKick) && DutyActionReady(PhantomKick))
					{
						if (GetBuffRemainingTime(Buffs.PhantomKick) < 5 && !InActionRange(PhantomKick) && GetOptionBool(Config.Occult_KickNotice))
						{
							return PhantomKick;
						}

						if (GetBuffRemainingTime(Buffs.PhantomKick) < 1 && InActionRange(PhantomKick))
						{
							return PhantomKick;
						}
					}

					if (IsEnabled(CustomComboPreset.Occult_Counter) && DutyActionReady(OccultCounter) && InActionRange(OccultCounter)
						&& CanWeave(actionID))
					{
						return OccultCounter;
					}

					if (IsEnabled(CustomComboPreset.Occult_Counterstance) && DutyActionReady(Counterstance)
						&& CurrentTarget.TargetObject == LocalPlayer && GetBuffRemainingTime(Buffs.Counterstance) < 2
						&& (ActionWatching.NumberOfGcdsUsed >= 3 || Service.Configuration.IgnoreGCDChecks))
					{
						return Counterstance;
					}
				}

				return actionID;
			}
		}

		internal class Occult_Thief : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.Occult_Thief;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.Occult_Thief) && HasEffect(PhantomJobs.Thief) && SafeToUse())
				{
					if (IsEnabled(CustomComboPreset.Occult_Vigilance) && DutyActionReady(Vigilance)
						&& !InCombat() && !HasEffect(Buffs.Vigilance) && HasBattleTarget())
					{
						return Vigilance;
					}

					if (InCombat() && CanWeave(actionID))
					{
						if (IsEnabled(CustomComboPreset.Occult_PilferWeapon) && DutyActionReady(PilferWeapon)
							&& !TargetHasEffectAny(Debuffs.WeaponPilfered) && InActionRange(PilferWeapon))
						{
							return PilferWeapon;
						}

						if (IsEnabled(CustomComboPreset.Occult_Steal) && DutyActionReady(Steal) && InActionRange(Steal) && HasBattleTarget())
						{
							return Steal;
						}
					}
				}

				return actionID;
			}
		}

		internal class Occult_Samurai : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.Occult_Samurai;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.Occult_Samurai) && HasEffect(PhantomJobs.Samurai) && InCombat() && SafeToUse())
				{
					if (IsEnabled(CustomComboPreset.Occult_Zeninage) && DutyActionReady(Zeninage) && InActionRange(Zeninage)
						&& (ActionWatching.NumberOfGcdsUsed >= 5 || Service.Configuration.IgnoreGCDChecks))
					{
						return Zeninage;
					}

					if (IsEnabled(CustomComboPreset.Occult_Iainuki) && DutyActionReady(Iainuki) && InActionRange(Iainuki) && !IsMoving
						&& (ActionWatching.NumberOfGcdsUsed >= 5 || Service.Configuration.IgnoreGCDChecks))
					{
						return Iainuki;
					}
				}

				return actionID;
			}
		}

		internal class Occult_Berserker : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.Occult_Berserker;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.Occult_Berserker) && HasEffect(PhantomJobs.Berserker) && InCombat())
				{
					if (IsEnabled(CustomComboPreset.Occult_DeadlyBlow) && DutyActionReady(DeadlyBlow) && InActionRange(DeadlyBlow)
						&& !HasEffect(Buffs.Rage) && (HasEffect(Buffs.PentUpRage) || GetCooldownRemainingTime(Rage) > 20))
					{
						return DeadlyBlow;
					}
				}

				return actionID;
			}
		}

		internal class Occult_Ranger : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.Occult_Thief;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.Occult_Ranger) && HasEffect(PhantomJobs.Ranger) && SafeToUse() && InCombat() && CanWeave(actionID))
				{
					if (IsEnabled(CustomComboPreset.Occult_Aim) && DutyActionReady(PhantomAim)
						&& (ActionWatching.NumberOfGcdsUsed >= 2 || Service.Configuration.IgnoreGCDChecks))
					{
						return PhantomAim;
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
				if (IsEnabled(CustomComboPreset.Occult_TimeMage) && HasEffect(PhantomJobs.TimeMage) && InCombat() && SafeToUse())
				{
					if (IsEnabled(CustomComboPreset.Occult_Quick) && DutyActionReady(OccultQuick))
					{
						return OccultQuick;
					}

					if (IsEnabled(CustomComboPreset.Occult_Comet) && DutyActionReady(OccultComet)
						&& (ActionWatching.NumberOfGcdsUsed >= 5 || Service.Configuration.IgnoreGCDChecks))
					{
						if (HasEffect(All.Buffs.Swiftcast) || HasEffect(RDM.Buffs.Dualcast) || HasEffect(BLM.Buffs.Triplecast) || HasEffect(Buffs.OccultQuick))
						{
							return OccultComet;
						}
					}

					if (IsEnabled(CustomComboPreset.Occult_MageMasher) && DutyActionReady(OccultMageMasher) && !TargetHasEffectAny(Debuffs.OccultMageMasher)
						&& HasBattleTarget() && CanWeave(actionID))
					{
						return OccultMageMasher;
					}
				}

				return actionID;
			}
		}

		internal class Occult_Chemist : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.Occult_Thief;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				return actionID;
			}
		}

		internal class Occult_Geomancer : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.Occult_Geomancer;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.Occult_Geomancer) && HasEffect(PhantomJobs.Geomancer) && SafeToUse())
				{
					if (IsEnabled(CustomComboPreset.Occult_BattleBell) && DutyActionReady(BattleBell) && CanWeave(actionID)
						&& (!HasEffect(Buffs.BattleBell) || (HasFriendlyTarget() && !TargetHasEffect(Buffs.BattleBell))))
					{
						return BattleBell;
					}

					if (IsEnabled(CustomComboPreset.Occult_Weather))
					{
						if (DutyActionReady(Sunbath) && PlayerHealthPercentageHp() <= GetOptionValue(Config.Occult_Sunbath))
						{
							return Sunbath;
						}

						if (DutyActionReady(CloudyCaress))
						{
							return CloudyCaress;
						}

						if (DutyActionReady(BlessedRain))
						{
							return BlessedRain;
						}

						if (DutyActionReady(MistyMirage))
						{
							return MistyMirage;
						}

						if (DutyActionReady(HastyMirage))
						{
							return HastyMirage;
						}

						if (DutyActionReady(AetherialGain))
						{
							return AetherialGain;
						}
					}

					if (IsEnabled(CustomComboPreset.Occult_RingingRespite) && DutyActionReady(RingingRespite) && CanWeave(actionID)
						&& (!HasEffect(Buffs.RingingRespite) || (HasFriendlyTarget() && !TargetHasEffect(Buffs.RingingRespite))))
					{
						return RingingRespite;
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
				if (IsEnabled(CustomComboPreset.Occult_Bard) && HasEffect(PhantomJobs.Bard) && SafeToUse() && CanWeave(actionID))
				{
					if (IsEnabled(CustomComboPreset.Occult_HerosRime) && DutyActionReady(HerosRime)
						&& (ActionWatching.NumberOfGcdsUsed >= 3 || Service.Configuration.IgnoreGCDChecks))
					{
						return HerosRime;
					}

					if (IsEnabled(CustomComboPreset.Occult_OffensiveAria) && DutyActionReady(OffensiveAria)
						&& (!HasEffect(Buffs.HerosRime) || GetBuffRemainingTime(Buffs.HerosRime) < 2)
						&& (!HasEffect(Buffs.OffensiveAria) || GetBuffRemainingTime(Buffs.OffensiveAria) < 5))
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
				if (IsEnabled(CustomComboPreset.Occult_Oracle) && HasEffect(PhantomJobs.Oracle) && SafeToUse())
				{
					if (IsEnabled(CustomComboPreset.Occult_Predict) && DutyActionReady(Predict) && InCombat()
						&& (!DutyActionReady(PhantomRejuvenation) || (DutyActionReady(PhantomRejuvenation) && GetBuffRemainingTime(Buffs.Rejuvination) <= 15)))
					{
						return Predict;
					}

					if (IsEnabled(CustomComboPreset.Occult_Predict))
					{
						if (DutyActionReady(PhantomJudgement) && (GetOptionValue(Config.Occult_Prediction) == 1
							|| (GetOptionValue(Config.Occult_Prediction) == 4 && PlayerHealthPercentageHp() <= 90)))
						{
							return PhantomJudgement;
						}

						if (DutyActionReady(Cleansing) && (GetOptionValue(Config.Occult_Prediction) == 2
							|| (GetOptionValue(Config.Occult_Prediction) == 4 && PlayerHealthPercentageHp() <= 90)))
						{
							return Cleansing;
						}

						if (DutyActionReady(Blessing) && (GetOptionValue(Config.Occult_Prediction) == 3
							|| (GetOptionValue(Config.Occult_Prediction) == 4 && PlayerHealthPercentageHp() <= 90)))
						{
							return Blessing;
						}

						if (DutyActionReady(Starfall) && GetOptionValue(Config.Occult_Prediction) == 4 && PlayerHealthPercentageHp() > 90)
						{
							return Starfall;
						}
					}

					if (IsEnabled(CustomComboPreset.Occult_PhantomRejuvination) && DutyActionReady(PhantomRejuvenation) && InCombat()
						&& CanWeave(actionID))
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
				if (IsEnabled(CustomComboPreset.Occult_Cannoneer) && HasEffect(PhantomJobs.Cannoneer) && SafeToUse()
					&& ((IsEnabled(CustomComboPreset.Occult_Cannoneer_Utility)
					&& (actionID is PhantomFire or HolyCannon or DarkCannon or ShockCannon or SilverCannon))
					|| (!IsEnabled(CustomComboPreset.Occult_Cannoneer_Utility) && InCombat())))
				{
					if (IsEnabled(CustomComboPreset.Occult_HolySilverCannon))
					{
						if (DutyActionReady(HolyCannon) && (GetOptionValue(Config.Occult_HolySilverCannon) == 1 || !DutyActionReady(SilverCannon)))
						{
							return HolyCannon;
						}

						if (DutyActionReady(SilverCannon) && GetOptionValue(Config.Occult_HolySilverCannon) == 2)
						{
							return SilverCannon;
						}
					}

					if (IsEnabled(CustomComboPreset.Occult_PhantomFire) && DutyActionReady(PhantomFire))
					{
						return PhantomFire;
					}

					if (IsEnabled(CustomComboPreset.Occult_DarkShockCannon))
					{
						if (DutyActionReady(DarkCannon) && (GetOptionValue(Config.Occult_DarkShockCannon) == 1 || !DutyActionReady(ShockCannon)))
						{
							return DarkCannon;
						}

						if (DutyActionReady(ShockCannon) && GetOptionValue(Config.Occult_DarkShockCannon) == 2)
						{
							return ShockCannon;
						}
					}
				}

				return actionID;
			}
		}

		internal class Occult_Freelancer : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.Occult_Freelancer;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.Occult_Freelancer) && HasEffect(PhantomJobs.Freelancer) && InCombat() && SafeToUse())
				{
					if (IsEnabled(CustomComboPreset.Occult_PhantomResuscitation) && DutyActionReady(OccultResuscitation)
						&& PlayerHealthPercentageHp() <= GetOptionValue(Config.Occult_PhantomResuscitation))
					{
						return OccultResuscitation;
					}
				}

				return actionID;
			}
		}
	}
}