using Dalamud.Game.ClientState.JobGauge.Types;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.CustomCombo;
using UltimateCombo.Data;

namespace UltimateCombo.Combos.PvE
{
	internal class PCT
	{
		public const byte JobID = 42;

		public const uint
			Fire = 34650,
			Aero = 34651,
			Water = 34652,
			Fire2 = 34656,
			Aero2 = 34657,
			Water2 = 34658,
			Blizzard = 34653,
			Stone = 34654,
			Thunder = 34655,
			Blizzard2 = 34659,
			Stone2 = 34660,
			Thunder2 = 34661,
			HolyInWhite = 34662,
			CometInBlack = 34663,

			CreatureMotif = 34689,
			LivingMuse = 35347,

			WeaponMotif = 34690,
			SteelMuse = 35348,
			HammerStamp = 34678,

			LandscapeMotif = 34691,
			ScenicMuse = 35349,

			MogOfTheAges = 34676,
			RetributionOfTheMadeen = 34677,
			Smudge = 34684,
			SubtractivePalette = 34683,
			RainbowDrip = 34688,
			StarPrism = 34681;

		public static class Buffs
		{
			public const ushort
				Aetherhues = 3675,
				AetherhuesII = 3676,
				SubtractivePalette = 3674,
				RainbowBright = 3679,
				HammerTime = 3680,
				MonochromeTones = 3691,
				StarryMuse = 3685,
				Hyperphantasia = 3688,
				Inspiration = 3689,
				SubtractiveSpectrum = 3690,
				Starstruck = 3681;
		}

		public static class Debuffs
		{

		}

		private static PCTGauge Gauge
		{
			get
			{
				return CustomComboFunctions.GetJobGauge<PCTGauge>();
			}
		}

		public static class Config
		{

		}

		internal class PCT_ST_DPS : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.PCT_ST_DPS;

			protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
			{
				if ((actionID is Fire or Aero or Water or Blizzard or Stone or Thunder)
					&& IsEnabled(CustomComboPreset.PCT_ST_DPS))
				{
					if (!InCombat() && !HasEffect(Buffs.HammerTime))
					{
						if (IsEnabled(CustomComboPreset.PCT_ST_Creatures) && !Gauge.CreatureMotifDrawn)
						{
							return OriginalHook(CreatureMotif);
						}

						if (IsEnabled(CustomComboPreset.PCT_ST_Hammer) && !Gauge.WeaponMotifDrawn)
						{
							return OriginalHook(WeaponMotif);
						}

						if (IsEnabled(CustomComboPreset.PCT_ST_Landscape) && !Gauge.LandscapeMotifDrawn)
						{
							return OriginalHook(LandscapeMotif);
						}

						if (IsEnabled(CustomComboPreset.PCT_ST_RainbowDrip) && ActionReady(RainbowDrip))
						{
							return RainbowDrip;
						}
					}

					if (CanWeave(actionID) && InCombat())
					{
						if (IsEnabled(CustomComboPreset.PCT_ST_Landscape) && ActionReady(OriginalHook(ScenicMuse)) && Gauge.LandscapeMotifDrawn
							&& ActionWatching.NumberOfGcdsUsed >= 2)
						{
							return OriginalHook(ScenicMuse);
						}

						if (IsEnabled(CustomComboPreset.PCT_ST_Hammer) && ActionReady(OriginalHook(SteelMuse)) && Gauge.WeaponMotifDrawn
							&& ((GetRemainingCharges(OriginalHook(SteelMuse)) == GetMaxCharges(OriginalHook(SteelMuse)) - 1
							&& GetCooldownChargeRemainingTime(OriginalHook(SteelMuse)) < 3)
							|| GetRemainingCharges(OriginalHook(SteelMuse)) == GetMaxCharges(OriginalHook(SteelMuse))
							|| HasEffect(Buffs.StarryMuse)))
						{
							return OriginalHook(SteelMuse);
						}

						if (IsEnabled(CustomComboPreset.PCT_ST_Creatures) && ActionReady(OriginalHook(MogOfTheAges)))
						{
							if (Gauge.MooglePortraitReady
								&& ((Gauge.CreatureFlags.HasFlag(Dalamud.Game.ClientState.JobGauge.Enums.CreatureFlags.Pom)
								&& Gauge.CreatureFlags.HasFlag(Dalamud.Game.ClientState.JobGauge.Enums.CreatureFlags.Wings)
								&& Gauge.CreatureFlags.HasFlag(Dalamud.Game.ClientState.JobGauge.Enums.CreatureFlags.Claw)
								&& Gauge.CreatureMotifDrawn) || HasEffect(Buffs.StarryMuse)))
							{
								return OriginalHook(MogOfTheAges);
							}

							if (Gauge.MadeenPortraitReady
								&& ((Gauge.CreatureFlags.HasFlag(Dalamud.Game.ClientState.JobGauge.Enums.CreatureFlags.Pom)
								&& Gauge.CreatureMotifDrawn) || HasEffect(Buffs.StarryMuse)))
							{
								return OriginalHook(RetributionOfTheMadeen);
							}
						}

						if (IsEnabled(CustomComboPreset.PCT_ST_Creatures) && ActionReady(OriginalHook(LivingMuse)) && Gauge.CreatureMotifDrawn
							&& ((GetRemainingCharges(OriginalHook(LivingMuse)) == GetMaxCharges(OriginalHook(LivingMuse)) - 1
							&& GetCooldownChargeRemainingTime(OriginalHook(LivingMuse)) < 3)
							|| GetRemainingCharges(OriginalHook(LivingMuse)) == GetMaxCharges(OriginalHook(LivingMuse))
							|| HasEffect(Buffs.StarryMuse)))
						{
							return OriginalHook(LivingMuse);
						}

						if (IsEnabled(CustomComboPreset.PCT_ST_Subtractive) && ActionReady(SubtractivePalette) && !HasEffect(Buffs.SubtractivePalette)
							&& (HasEffect(Buffs.SubtractiveSpectrum)
							|| (Gauge.PalleteGauge >= 50 && HasEffect(Buffs.StarryMuse))
							|| Gauge.PalleteGauge == 100))
						{
							if (IsEnabled(CustomComboPreset.PCT_ST_Swiftcast) && ActionReady(All.Swiftcast))
							{
								return All.Swiftcast;
							}

							return SubtractivePalette;
						}

						if (IsEnabled(CustomComboPreset.PCT_ST_Swiftcast) && ActionReady(All.Swiftcast) && IsMoving)
						{
							return All.Swiftcast;
						}
					}

					if (IsEnabled(CustomComboPreset.PCT_ST_StarPrism) && ActionReady(StarPrism) && HasEffect(Buffs.Starstruck)
						&& !HasEffect(Buffs.SubtractivePalette))
					{
						return StarPrism;
					}

					if (IsEnabled(CustomComboPreset.PCT_ST_RainbowDrip) && ActionReady(RainbowDrip) && HasEffect(Buffs.RainbowBright))
					{
						return RainbowDrip;
					}

					if (IsEnabled(CustomComboPreset.PCT_ST_Creatures) && ActionReady(OriginalHook(CreatureMotif)) && !IsMoving
						&& !Gauge.CreatureMotifDrawn && !HasEffect(Buffs.StarryMuse) && !HasEffect(Buffs.Hyperphantasia)
						&& ((GetRemainingCharges(OriginalHook(LivingMuse)) == GetMaxCharges(OriginalHook(LivingMuse)) - 1)
						|| GetRemainingCharges(OriginalHook(LivingMuse)) == GetMaxCharges(OriginalHook(LivingMuse))))
					{
						return OriginalHook(CreatureMotif);
					}

					if (IsEnabled(CustomComboPreset.PCT_ST_Hammer) && ActionReady(OriginalHook(HammerStamp)) && HasEffect(Buffs.HammerTime)
						&& !HasEffect(Buffs.Hyperphantasia))
					{
						return OriginalHook(HammerStamp);
					}

					if (IsEnabled(CustomComboPreset.PCT_ST_Hammer) && ActionReady(OriginalHook(WeaponMotif)) && !IsMoving
						&& !Gauge.WeaponMotifDrawn && !HasEffect(Buffs.StarryMuse) && !HasEffect(Buffs.Hyperphantasia)
						&& ((GetRemainingCharges(OriginalHook(SteelMuse)) == GetMaxCharges(OriginalHook(SteelMuse)) - 1)
						|| GetRemainingCharges(OriginalHook(SteelMuse)) == GetMaxCharges(OriginalHook(SteelMuse))))
					{
						return OriginalHook(WeaponMotif);
					}

					if (IsEnabled(CustomComboPreset.PCT_ST_Landscape) && ActionReady(OriginalHook(LandscapeMotif)) && !IsMoving
						&& !Gauge.LandscapeMotifDrawn && !HasEffect(Buffs.StarryMuse) && !HasEffect(Buffs.Hyperphantasia)
						&& GetCooldownRemainingTime(OriginalHook(ScenicMuse)) < 15)
					{
						return OriginalHook(LandscapeMotif);
					}

					if (IsEnabled(CustomComboPreset.PCT_ST_Comet) && ActionReady(HolyInWhite) && Gauge.Paint > 0
						&& (Gauge.Paint > 4 || GetCooldownRemainingTime(OriginalHook(ScenicMuse)) < 30 || IsMoving
						|| (HasEffect(Buffs.MonochromeTones) && HasEffect(Buffs.StarryMuse))))
					{
						if (HasEffect(Buffs.MonochromeTones))
						{
							return CometInBlack;
						}

						if (!HasEffect(Buffs.HammerTime))
						{
							return HolyInWhite;
						}
					}

					if (HasEffect(Buffs.SubtractivePalette))
					{
						return OriginalHook(Blizzard);
					}
				}

				return actionID;
			}
		}

		internal class PCT_AoE_DPS : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.PCT_AoE_DPS;

			protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
			{
				if ((actionID is Fire2 or Aero2 or Water2 or Blizzard2 or Stone2 or Thunder2)
					&& IsEnabled(CustomComboPreset.PCT_AoE_DPS))
				{
					if (!InCombat() && !HasEffect(Buffs.HammerTime))
					{
						if (IsEnabled(CustomComboPreset.PCT_AoE_Creatures) && !Gauge.CreatureMotifDrawn)
						{
							return OriginalHook(CreatureMotif);
						}

						if (IsEnabled(CustomComboPreset.PCT_AoE_Hammer) && !Gauge.WeaponMotifDrawn)
						{
							return OriginalHook(WeaponMotif);
						}

						if (IsEnabled(CustomComboPreset.PCT_AoE_Landscape) && !Gauge.LandscapeMotifDrawn)
						{
							return OriginalHook(LandscapeMotif);
						}

						if (IsEnabled(CustomComboPreset.PCT_AoE_RainbowDrip) && ActionReady(RainbowDrip))
						{
							return RainbowDrip;
						}
					}

					if (CanWeave(actionID) && InCombat())
					{
						if (IsEnabled(CustomComboPreset.PCT_AoE_Landscape) && ActionReady(OriginalHook(ScenicMuse))
							&& Gauge.LandscapeMotifDrawn)
						{
							return OriginalHook(ScenicMuse);
						}

						if (IsEnabled(CustomComboPreset.PCT_AoE_Hammer) && ActionReady(OriginalHook(SteelMuse)) && Gauge.WeaponMotifDrawn
							&& ((GetRemainingCharges(OriginalHook(SteelMuse)) == GetMaxCharges(OriginalHook(SteelMuse)) - 1
							&& GetCooldownChargeRemainingTime(OriginalHook(SteelMuse)) < 3)
							|| GetRemainingCharges(OriginalHook(SteelMuse)) == GetMaxCharges(OriginalHook(SteelMuse))
							|| HasEffect(Buffs.StarryMuse)))
						{
							return OriginalHook(SteelMuse);
						}

						if (IsEnabled(CustomComboPreset.PCT_AoE_Creatures) && ActionReady(OriginalHook(MogOfTheAges)))
						{
							if (Gauge.MooglePortraitReady
								&& ((Gauge.CreatureFlags.HasFlag(Dalamud.Game.ClientState.JobGauge.Enums.CreatureFlags.Pom)
								&& Gauge.CreatureFlags.HasFlag(Dalamud.Game.ClientState.JobGauge.Enums.CreatureFlags.Wings)
								&& Gauge.CreatureFlags.HasFlag(Dalamud.Game.ClientState.JobGauge.Enums.CreatureFlags.Claw)
								&& Gauge.CreatureMotifDrawn) || HasEffect(Buffs.StarryMuse)))
							{
								return OriginalHook(MogOfTheAges);
							}

							if (Gauge.MadeenPortraitReady
								&& ((Gauge.CreatureFlags.HasFlag(Dalamud.Game.ClientState.JobGauge.Enums.CreatureFlags.Pom)
								&& Gauge.CreatureMotifDrawn) || HasEffect(Buffs.StarryMuse)))
							{
								return OriginalHook(RetributionOfTheMadeen);
							}
						}

						if (IsEnabled(CustomComboPreset.PCT_AoE_Creatures) && ActionReady(OriginalHook(LivingMuse)) && Gauge.CreatureMotifDrawn
							&& ((GetRemainingCharges(OriginalHook(LivingMuse)) == GetMaxCharges(OriginalHook(LivingMuse)) - 1
							&& GetCooldownChargeRemainingTime(OriginalHook(LivingMuse)) < 3)
							|| GetRemainingCharges(OriginalHook(LivingMuse)) == GetMaxCharges(OriginalHook(LivingMuse))
							|| HasEffect(Buffs.StarryMuse)))
						{
							return OriginalHook(LivingMuse);
						}

						if (IsEnabled(CustomComboPreset.PCT_AoE_Subtractive) && ActionReady(SubtractivePalette) && !HasEffect(Buffs.SubtractivePalette)
							&& (HasEffect(Buffs.SubtractiveSpectrum)
							|| (Gauge.PalleteGauge >= 50 && HasEffect(Buffs.StarryMuse))
							|| Gauge.PalleteGauge == 100))
						{
							if (IsEnabled(CustomComboPreset.PCT_AoE_Swiftcast) && ActionReady(All.Swiftcast))
							{
								return All.Swiftcast;
							}

							return SubtractivePalette;
						}

						if (IsEnabled(CustomComboPreset.PCT_AoE_Swiftcast) && ActionReady(All.Swiftcast) && IsMoving)
						{
							return All.Swiftcast;
						}
					}

					if (IsEnabled(CustomComboPreset.PCT_AoE_StarPrism) && ActionReady(StarPrism) && HasEffect(Buffs.Starstruck)
						&& !HasEffect(Buffs.SubtractivePalette))
					{
						return StarPrism;
					}

					if (IsEnabled(CustomComboPreset.PCT_AoE_RainbowDrip) && ActionReady(RainbowDrip) && HasEffect(Buffs.RainbowBright))
					{
						return RainbowDrip;
					}

					if (IsEnabled(CustomComboPreset.PCT_AoE_Creatures) && ActionReady(OriginalHook(CreatureMotif)) && !IsMoving
						&& !Gauge.CreatureMotifDrawn && !HasEffect(Buffs.StarryMuse) && !HasEffect(Buffs.Hyperphantasia)
						&& ((GetRemainingCharges(OriginalHook(LivingMuse)) == GetMaxCharges(OriginalHook(LivingMuse)) - 1)
						|| GetRemainingCharges(OriginalHook(LivingMuse)) == GetMaxCharges(OriginalHook(LivingMuse))))
					{
						return OriginalHook(CreatureMotif);
					}

					if (IsEnabled(CustomComboPreset.PCT_AoE_Hammer) && ActionReady(OriginalHook(HammerStamp)) && HasEffect(Buffs.HammerTime)
						&& !HasEffect(Buffs.Hyperphantasia))
					{
						return OriginalHook(HammerStamp);
					}

					if (IsEnabled(CustomComboPreset.PCT_AoE_Hammer) && ActionReady(OriginalHook(WeaponMotif)) && !IsMoving
						&& !Gauge.WeaponMotifDrawn && !HasEffect(Buffs.StarryMuse) && !HasEffect(Buffs.Hyperphantasia)
						&& ((GetRemainingCharges(OriginalHook(SteelMuse)) == GetMaxCharges(OriginalHook(SteelMuse)) - 1)
						|| GetRemainingCharges(OriginalHook(SteelMuse)) == GetMaxCharges(OriginalHook(SteelMuse))))
					{
						return OriginalHook(WeaponMotif);
					}

					if (IsEnabled(CustomComboPreset.PCT_AoE_Landscape) && ActionReady(OriginalHook(LandscapeMotif)) && !IsMoving
						&& !Gauge.LandscapeMotifDrawn && !HasEffect(Buffs.StarryMuse) && !HasEffect(Buffs.Hyperphantasia)
						&& GetCooldownRemainingTime(OriginalHook(ScenicMuse)) < 15)
					{
						return OriginalHook(LandscapeMotif);
					}

					if (IsEnabled(CustomComboPreset.PCT_AoE_Comet) && ActionReady(HolyInWhite) && Gauge.Paint > 0
						&& (Gauge.Paint > 4 || GetCooldownRemainingTime(OriginalHook(ScenicMuse)) < 30 || IsMoving
						|| (HasEffect(Buffs.MonochromeTones) && HasEffect(Buffs.StarryMuse))))
					{
						if (HasEffect(Buffs.MonochromeTones))
						{
							return CometInBlack;
						}

						if (!HasEffect(Buffs.HammerTime))
						{
							return HolyInWhite;
						}
					}

					if (HasEffect(Buffs.SubtractivePalette))
					{
						return OriginalHook(Blizzard);
					}
				}

				return actionID;
			}
		}
	}
}