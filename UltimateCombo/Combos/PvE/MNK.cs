using Dalamud.Game.ClientState.JobGauge.Enums;
using Dalamud.Game.ClientState.JobGauge.Types;
using System.Linq;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.CustomCombo;
using UltimateCombo.Data;

namespace UltimateCombo.Combos.PvE
{
	internal static class MNK
	{
		public const byte JobID = 20;

		public const uint
			Bootshine = 53,
			DragonKick = 74,
			SnapPunch = 56,
			TwinSnakes = 61,
			Demolish = 66,
			ArmOfTheDestroyer = 62,
			Rockbreaker = 70,
			FourPointFury = 16473,
			PerfectBalance = 69,
			TrueStrike = 54,
			SteeledMeditation = 36940,
			InspiritedMeditation = 36941,
			HowlingFist = 25763,
			SteelPeak = 25761,
			Enlightenment = 16474,
			SixSidedStar = 16476,
			MasterfulBlitz = 25764,
			ElixirField = 3545,
			FlintStrike = 25882,
			RisingPhoenix = 25768,
			ShadowOfTheDestroyer = 25767,
			RiddleOfFire = 7395,
			RiddleOfWind = 25766,
			Brotherhood = 7396,
			ForbiddenChakra = 3547,
			FormShift = 4262,
			Thunderclap = 25762,
			ForbiddenMeditation = 36942,
			EnlightenedMeditation = 36942,
			LeapingOpo = 36945,
			RisingRaptor = 36946,
			PouncingCoeurl = 36947,
			WindsReply = 36949,
			FiresReply = 36950;

		public static class Buffs
		{
			public const ushort
				TwinSnakes = 101,
				OpoOpoForm = 107,
				RaptorForm = 108,
				CoerlForm = 109,
				PerfectBalance = 110,
				RiddleOfFire = 1181,
				LeadenFist = 1861,
				FormlessFist = 2513,
				DisciplinedFist = 3001,
				Brotherhood = 1185,
				WindsRumination = 3842,
				FiresRumination = 3843;
		}

		private static MNKGauge Gauge
		{
			get
			{
				return CustomComboFunctions.GetJobGauge<MNKGauge>();
			}
		}

		public static class Config
		{
			public static UserInt
				MNK_Variant_Cure = new("MNK_Variant_Cure", 50);
		}

		internal class MNK_ST_DPS : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MNK_ST_DPS;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Bootshine or LeapingOpo or TrueStrike or RisingRaptor or SnapPunch or PouncingCoeurl or DragonKick or TwinSnakes or Demolish)
					&& IsEnabled(CustomComboPreset.MNK_ST_DPS))
				{
					if (ActionReady(OriginalHook(ForbiddenMeditation)) && Gauge.Chakra != 5 && !InCombat() && !InMeleeRange())
					{
						return OriginalHook(ForbiddenMeditation);
					}

					if (ActionReady(FormShift) && !HasEffect(Buffs.FormlessFist) && Gauge.BeastChakra.Contains(BeastChakra.NONE) && !InMeleeRange()
						&& (!InCombat() || (!HasEffect(Buffs.OpoOpoForm) && !HasEffect(Buffs.RaptorForm)
						&& !HasEffect(Buffs.CoerlForm) && !HasEffect(Buffs.PerfectBalance))))
					{
						return FormShift;
					}

					if (CanWeave(actionID))
					{
						if (IsEnabled(CustomComboPreset.MNK_ST_Fire) && ActionReady(RiddleOfFire) && ActionWatching.NumberOfGcdsUsed >= 3)
						{
							return RiddleOfFire;
						}

						if (IsEnabled(CustomComboPreset.MNK_ST_Brother) && ActionReady(Brotherhood) && ActionWatching.NumberOfGcdsUsed >= 3)
						{
							return Brotherhood;
						}

						if (IsEnabled(CustomComboPreset.MNK_ST_Wind) && ActionReady(RiddleOfWind) && ActionWatching.NumberOfGcdsUsed >= 3)
						{
							return RiddleOfWind;
						}

						if (IsEnabled(CustomComboPreset.MNK_ST_PerfectBalance) && ActionReady(PerfectBalance)
							&& (GetCooldownRemainingTime(RiddleOfFire) < 5 || !LevelChecked(RiddleOfFire) || HasEffect(Buffs.RiddleOfFire))
							&& !HasEffect(Buffs.PerfectBalance) && Gauge.BeastChakra.Contains(BeastChakra.NONE)
							&& (WasLastWeaponskill(OriginalHook(Bootshine)) || WasLastWeaponskill(DragonKick)))
						{
							return PerfectBalance;
						}

						if (IsEnabled(CustomComboPreset.MNK_ST_Meditation) && ActionReady(OriginalHook(SteelPeak)) && Gauge.Chakra >= 5 && InCombat()
							&& (!HasEffect(Buffs.Brotherhood) || GetBuffRemainingTime(Buffs.Brotherhood) < 2 || !LevelChecked(SixSidedStar)
							|| !IsEnabled(CustomComboPreset.MNK_ST_SixStar)))
						{
							return OriginalHook(SteelPeak);
						}
					}

					if (IsEnabled(CustomComboPreset.MNK_ST_Fire) && ActionReady(FiresReply) && HasEffect(Buffs.FiresRumination))
					{
						return FiresReply;
					}

					if (IsEnabled(CustomComboPreset.MNK_ST_Wind) && ActionReady(WindsReply) && HasEffect(Buffs.WindsRumination))
					{
						return WindsReply;
					}

					if (IsEnabled(CustomComboPreset.MNK_ST_SixStar) && ActionReady(SixSidedStar) && HasEffect(Buffs.Brotherhood) & Gauge.Chakra == 10
						&& !HasEffect(Buffs.PerfectBalance))
					{
						return SixSidedStar;
					}

					if (IsEnabled(CustomComboPreset.MNK_ST_Blitz) && ActionReady(OriginalHook(MasterfulBlitz)) && !Gauge.BeastChakra.Contains(BeastChakra.NONE))
					{
						return OriginalHook(MasterfulBlitz);
					}

					if (HasEffect(Buffs.PerfectBalance))
					{
						if (Gauge.Nadi is Nadi.SOLAR && Gauge.Nadi is Nadi.LUNAR)
						{
							if (ActionReady(OriginalHook(Bootshine)) && Gauge.OpoOpoFury >= 1)
							{
								return OriginalHook(Bootshine);
							}

							if (ActionReady(DragonKick))
							{
								return DragonKick;
							}
						}

						if (Gauge.Nadi is not Nadi.SOLAR)
						{
							if (!Gauge.BeastChakra.Contains(BeastChakra.OPOOPO))
							{
								if (ActionReady(OriginalHook(Bootshine)) && Gauge.OpoOpoFury >= 1)
								{
									return OriginalHook(Bootshine);
								}

								if (ActionReady(DragonKick))
								{
									return DragonKick;
								}
							}

							if (!Gauge.BeastChakra.Contains(BeastChakra.RAPTOR))
							{
								if (ActionReady(OriginalHook(TrueStrike)) && Gauge.RaptorFury >= 1)
								{
									return OriginalHook(TrueStrike);
								}

								if (ActionReady(TwinSnakes))
								{
									return TwinSnakes;
								}
							}

							if (!Gauge.BeastChakra.Contains(BeastChakra.COEURL))
							{
								if (ActionReady(OriginalHook(SnapPunch)) && Gauge.CoeurlFury >= 1)
								{
									return OriginalHook(SnapPunch);
								}

								if (ActionReady(Demolish))
								{
									return Demolish;
								}
							}
						}

						if (Gauge.Nadi is not Nadi.LUNAR)
						{
							if (ActionReady(OriginalHook(Bootshine)) && Gauge.OpoOpoFury >= 1)
							{
								return OriginalHook(Bootshine);
							}

							if (ActionReady(DragonKick))
							{
								return DragonKick;
							}
						}
					}

					if (HasEffect(Buffs.RaptorForm))
					{
						if ((ActionReady(OriginalHook(TrueStrike)) && Gauge.RaptorFury >= 1) || !LevelChecked(TwinSnakes))
						{
							return OriginalHook(TrueStrike);
						}

						if (ActionReady(TwinSnakes))
						{
							return TwinSnakes;
						}
					}

					if (HasEffect(Buffs.CoerlForm))
					{
						if ((ActionReady(OriginalHook(SnapPunch)) && Gauge.CoeurlFury >= 1) || !LevelChecked(Demolish))
						{
							return OriginalHook(SnapPunch);
						}

						if (ActionReady(Demolish))
						{
							return Demolish;
						}
					}

					if ((ActionReady(OriginalHook(Bootshine)) && Gauge.OpoOpoFury >= 1) || !LevelChecked(DragonKick))
					{
						return OriginalHook(Bootshine);
					}

					if (ActionReady(DragonKick))
					{
						return DragonKick;
					}
				}
				return actionID;
			}
		}

		internal class MNK_AoE_DPS : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MNK_AoE_DPS;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is ArmOfTheDestroyer or ShadowOfTheDestroyer or FourPointFury or Rockbreaker) && IsEnabled(CustomComboPreset.MNK_AoE_DPS))
				{
					if (ActionReady(OriginalHook(InspiritedMeditation)) && Gauge.Chakra != 5 && !InCombat() && !InMeleeRange())
					{
						return OriginalHook(InspiritedMeditation);
					}

					if (ActionReady(FormShift) && !HasEffect(Buffs.FormlessFist) && Gauge.BeastChakra.Contains(BeastChakra.NONE) && !InMeleeRange()
						&& (!InCombat() || (!HasEffect(Buffs.OpoOpoForm) && !HasEffect(Buffs.RaptorForm)
						&& !HasEffect(Buffs.CoerlForm) && !HasEffect(Buffs.PerfectBalance))))
					{
						return FormShift;
					}

					if (CanWeave(actionID))
					{
						if (IsEnabled(CustomComboPreset.MNK_AoE_Fire) && ActionReady(RiddleOfFire))
						{
							return RiddleOfFire;
						}

						if (IsEnabled(CustomComboPreset.MNK_AoE_Brother) && ActionReady(Brotherhood))
						{
							return Brotherhood;
						}

						if (IsEnabled(CustomComboPreset.MNK_AoE_Wind) && ActionReady(RiddleOfWind))
						{
							return RiddleOfWind;
						}

						if (IsEnabled(CustomComboPreset.MNK_AoE_PerfectBalance) && ActionReady(PerfectBalance)
							&& (GetCooldownRemainingTime(RiddleOfFire) < 5 || !LevelChecked(RiddleOfFire) || HasEffect(Buffs.RiddleOfFire))
							&& !HasEffect(Buffs.PerfectBalance) && Gauge.BeastChakra.Contains(BeastChakra.NONE)
							&& WasLastWeaponskill(OriginalHook(ArmOfTheDestroyer)))
						{
							return PerfectBalance;
						}

						if (IsEnabled(CustomComboPreset.MNK_AoE_Meditation) && ActionReady(OriginalHook(HowlingFist)) && Gauge.Chakra >= 5 && InCombat())
						{
							return OriginalHook(HowlingFist);
						}
					}

					if (IsEnabled(CustomComboPreset.MNK_AoE_Fire) && ActionReady(FiresReply) && HasEffect(Buffs.FiresRumination))
					{
						return FiresReply;
					}

					if (IsEnabled(CustomComboPreset.MNK_AoE_Wind) && ActionReady(WindsReply) && HasEffect(Buffs.WindsRumination))
					{
						return WindsReply;
					}

					if (IsEnabled(CustomComboPreset.MNK_AoE_Blitz) && ActionReady(OriginalHook(MasterfulBlitz)) && !Gauge.BeastChakra.Contains(BeastChakra.NONE))
					{
						return OriginalHook(MasterfulBlitz);
					}

					if (HasEffect(Buffs.PerfectBalance))
					{
						if (Gauge.Nadi is Nadi.SOLAR && Gauge.Nadi is Nadi.LUNAR)
						{
							if (ActionReady(OriginalHook(ArmOfTheDestroyer)))
							{
								return OriginalHook(ArmOfTheDestroyer);
							}
						}

						if (Gauge.Nadi is not Nadi.SOLAR)
						{
							if (!Gauge.BeastChakra.Contains(BeastChakra.OPOOPO))
							{
								if (ActionReady(OriginalHook(ArmOfTheDestroyer)))
								{
									return OriginalHook(ArmOfTheDestroyer);
								}
							}

							if (!Gauge.BeastChakra.Contains(BeastChakra.RAPTOR))
							{
								if (ActionReady(FourPointFury))
								{
									return FourPointFury;
								}
							}

							if (!Gauge.BeastChakra.Contains(BeastChakra.COEURL))
							{
								if (ActionReady(Rockbreaker))
								{
									return Rockbreaker;
								}
							}
						}

						if (Gauge.Nadi is not Nadi.LUNAR)
						{
							if (ActionReady(OriginalHook(ArmOfTheDestroyer)))
							{
								return OriginalHook(ArmOfTheDestroyer);
							}
						}
					}

					if (ActionReady(FourPointFury) && HasEffect(Buffs.RaptorForm))
					{
						return FourPointFury;
					}

					if (ActionReady(Rockbreaker) && HasEffect(Buffs.CoerlForm))
					{
						return Rockbreaker;
					}

					if (ActionReady(OriginalHook(ArmOfTheDestroyer)))
					{
						return OriginalHook(ArmOfTheDestroyer);
					}
				}
				return actionID;
			}
		}
	}
}