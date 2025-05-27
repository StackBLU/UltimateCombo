using Dalamud.Game.ClientState.JobGauge.Enums;
using Dalamud.Game.ClientState.JobGauge.Types;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvE
{
	internal class VPR
	{
		public const byte JobID = 41;

		public const uint
			ReavingFangs = 34607,
			ReavingMaw = 34615,
			Vicewinder = 34620,
			HuntersCoil = 34621,
			HuntersDen = 34624,
			HuntersSnap = 39166,
			Vicepit = 34623,
			RattlingCoil = 39189,
			Reawaken = 34626,
			SerpentsIre = 34647,
			SerpentsTail = 35920,
			Slither = 34646,
			SteelFangs = 34606,
			SteelMaw = 34614,
			SwiftskinsCoil = 34622,
			SwiftskinsDen = 34625,
			Twinblood = 35922,
			Twinfang = 35921,
			TwinbloodThresh = 34639,
			TwinfangThresh = 34638,
			UncoiledFury = 34633,
			WrithingSnap = 34632,
			SwiftskinsSting = 34609,
			TwinfangBite = 34636,
			TwinbloodBite = 34637,
			UncoiledTwinfang = 34644,
			UncoiledTwinblood = 34645,
			HindstingStrike = 34612,
			DeathRattle = 34634,
			HuntersSting = 34608,
			HindsbaneFang = 34613,
			FlankstingStrike = 34610,
			FlanksbaneFang = 34611,
			HuntersBite = 34616,
			JaggedMaw = 34618,
			SwiftskinsBite = 34617,
			BloodiedMaw = 34619,
			FirstGeneration = 34627,
			FirstLegacy = 34640,
			SecondGeneration = 34628,
			SecondLegacy = 34641,
			ThirdGeneration = 34629,
			ThirdLegacy = 34642,
			FourthGeneration = 34630,
			FourthLegacy = 34643,
			Ouroboros = 34631,
			LastLash = 34635;

		public static class Buffs
		{
			public const ushort
				FellhuntersVenom = 3659,
				FellskinsVenom = 3660,
				FlanksbaneVenom = 3646,
				FlankstungVenom = 3645,
				HindstungVenom = 3647,
				HindsbaneVenom = 3648,
				GrimhuntersVenom = 3649,
				GrimskinsVenom = 3650,
				HuntersVenom = 3657,
				SwiftskinsVenom = 3658,
				HuntersInstinct = 3668,
				Swiftscaled = 3669,
				Reawakened = 3670,
				ReadyToReawaken = 3671,
				PoisedForTwinfang = 3665,
				PoisedForTwinblood = 3666,
				HonedReavers = 3772,
				HonedSteel = 3672;
		}

		public static VPRGauge Gauge
		{
			get
			{
				return CustomComboFunctions.GetJobGauge<VPRGauge>();
			}
		}

		public static class Config
		{

		}

		internal class VPR_ST_DPS : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.VPR_ST_DPS;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is SteelFangs or ReavingFangs) && IsEnabled(CustomComboPreset.VPR_ST_DPS))
				{
					if (CanWeave(actionID))
					{
						if (IsEnabled(CustomComboPreset.VPR_ST_Uncoiled) && HasEffect(Buffs.PoisedForTwinfang))
						{
							return UncoiledTwinfang;
						}

						if (IsEnabled(CustomComboPreset.VPR_ST_Uncoiled) && HasEffect(Buffs.PoisedForTwinblood))
						{
							return UncoiledTwinblood;
						}

						if (IsEnabled(CustomComboPreset.VPR_ST_Vicewinder) && HasEffect(Buffs.SwiftskinsVenom))
						{
							return TwinbloodBite;
						}

						if (IsEnabled(CustomComboPreset.VPR_ST_Vicewinder) && HasEffect(Buffs.HuntersVenom))
						{
							return TwinfangBite;
						}

						if (IsEnabled(CustomComboPreset.VPR_ST_Finishers) && ActionReady(OriginalHook(SerpentsTail))
							&& Gauge.SerpentCombo is SerpentCombo.DeathRattle)
						{
							return DeathRattle;
						}

						if (IsEnabled(CustomComboPreset.VPR_ST_Reawaken) && ActionReady(OriginalHook(SerpentsTail)) &&
							(Gauge.SerpentCombo is SerpentCombo.FirstLegacy ||
							Gauge.SerpentCombo is SerpentCombo.SecondLegacy ||
							Gauge.SerpentCombo is SerpentCombo.ThirdLegacy ||
							Gauge.SerpentCombo is SerpentCombo.FourthLegacy))
						{
							return OriginalHook(SerpentsTail);
						}

						if (IsEnabled(CustomComboPreset.VPR_ST_SerpentsIre) && ActionReady(SerpentsIre) && TargetIsBoss())
						{
							return SerpentsIre;
						}
					}

					if (IsEnabled(CustomComboPreset.VPR_ST_Reawaken) && HasEffect(Buffs.Reawakened))
					{
						if (Gauge.AnguineTribute is 5 || (Gauge.AnguineTribute is 4 && !LevelChecked(Ouroboros)))
						{
							return OriginalHook(SteelMaw);
						}

						if (Gauge.AnguineTribute is 4 || (Gauge.AnguineTribute is 3 && !LevelChecked(Ouroboros)))
						{
							return OriginalHook(ReavingMaw);
						}

						if (Gauge.AnguineTribute is 3 || (Gauge.AnguineTribute is 2 && !LevelChecked(Ouroboros)))
						{
							return OriginalHook(HuntersDen);
						}

						if (Gauge.AnguineTribute is 2 || (Gauge.AnguineTribute is 1 && !LevelChecked(Ouroboros)))
						{
							return OriginalHook(SwiftskinsDen);
						}

						if (Gauge.AnguineTribute is 1)
						{
							return OriginalHook(Reawaken);
						}
					}

					if (GetBuffRemainingTime(Buffs.Swiftscaled) > 10 && GetBuffRemainingTime(Buffs.HuntersInstinct) > 10
						&& !Gauge.DreadCombo.HasFlag(DreadCombo.Dreadwinder)
						&& !Gauge.DreadCombo.HasFlag(DreadCombo.SwiftskinsCoil) && !Gauge.DreadCombo.HasFlag(DreadCombo.HuntersCoil))
					{
						if (IsEnabled(CustomComboPreset.VPR_ST_Reawaken) && ActionReady(Reawaken)
							&& (GetCooldownRemainingTime(SerpentsIre) > 30 || Gauge.SerpentOffering == 100)
							&& (Gauge.SerpentOffering >= 50 || HasEffect(Buffs.ReadyToReawaken)))
						{
							return Reawaken;
						}

						if (IsEnabled(CustomComboPreset.VPR_ST_Uncoiled) && ActionReady(UncoiledFury) && Gauge.RattlingCoilStacks > 0
							&& (GetCooldownRemainingTime(SerpentsIre) > 60 || Gauge.RattlingCoilStacks == 3))
						{
							return UncoiledFury;
						}
					}

					if (IsEnabled(CustomComboPreset.VPR_ST_Vicewinder) && Gauge.DreadCombo.HasFlag(DreadCombo.SwiftskinsCoil))
					{
						return HuntersCoil;
					}

					if (IsEnabled(CustomComboPreset.VPR_ST_Vicewinder) && Gauge.DreadCombo.HasFlag(DreadCombo.HuntersCoil))
					{
						return SwiftskinsCoil;
					}

					if (IsEnabled(CustomComboPreset.VPR_ST_Vicewinder) && Gauge.DreadCombo.HasFlag(DreadCombo.Dreadwinder))
					{
						if (GetBuffRemainingTime(Buffs.Swiftscaled) <= GetBuffRemainingTime(Buffs.HuntersInstinct))
						{
							return SwiftskinsCoil;
						}

						if (GetBuffRemainingTime(Buffs.HuntersInstinct) < GetBuffRemainingTime(Buffs.Swiftscaled))
						{
							return HuntersCoil;
						}
					}

					if (IsEnabled(CustomComboPreset.VPR_ST_Vicewinder) && ActionReady(Vicewinder)
						&& (WasLastWeaponskill(FlankstingStrike) || WasLastWeaponskill(FlanksbaneFang)
						|| WasLastWeaponskill(HindstingStrike) || WasLastWeaponskill(HindsbaneFang)))
					{
						return Vicewinder;
					}

					if (IsEnabled(CustomComboPreset.VPR_ST_UncoiledSnap) && ActionReady(WrithingSnap) && OutOfMeleeRange())
					{
						if (ActionReady(UncoiledFury) && Gauge.RattlingCoilStacks > 0)
						{
							return UncoiledFury;
						}

						return WrithingSnap;
					}

					if (comboTime > 0)
					{
						if (lastComboMove is SteelFangs or ReavingFangs)
						{
							if (HasEffect(Buffs.HindsbaneVenom) || HasEffect(Buffs.HindstungVenom))
							{
								return OriginalHook(SwiftskinsSting);
							}

							if (HasEffect(Buffs.FlanksbaneVenom) || HasEffect(Buffs.FlankstungVenom))
							{
								return OriginalHook(HuntersSting);
							}

							if (ActionReady(OriginalHook(SwiftskinsSting)))
							{
								return OriginalHook(SwiftskinsSting);
							}
						}

						if (lastComboMove is SwiftskinsSting or HuntersSting)
						{
							if (HasEffect(Buffs.FlankstungVenom))
							{
								return OriginalHook(FlankstingStrike);
							}

							if (HasEffect(Buffs.FlanksbaneVenom))
							{
								return OriginalHook(FlanksbaneFang);
							}

							if (HasEffect(Buffs.HindstungVenom))
							{
								return OriginalHook(HindstingStrike);
							}

							if (HasEffect(Buffs.HindsbaneVenom))
							{
								return OriginalHook(HindsbaneFang);
							}

							if (ActionReady(OriginalHook(HindstingStrike)))
							{
								return OriginalHook(HindstingStrike);
							}
						}

						if (HasEffect(Buffs.HonedReavers))
						{
							return OriginalHook(ReavingFangs);
						}

						if (HasEffect(Buffs.HonedSteel))
						{
							return OriginalHook(SteelFangs);
						}
					}

					return SteelFangs;
				}

				return actionID;
			}
		}

		internal class VPR_AoE_DPS : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.VPR_AoE_DPS;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is SteelMaw or ReavingMaw) && IsEnabled(CustomComboPreset.VPR_AoE_DPS))
				{
					if (CanWeave(actionID))
					{
						if (IsEnabled(CustomComboPreset.VPR_AoE_Uncoiled) && HasEffect(Buffs.PoisedForTwinfang))
						{
							return UncoiledTwinfang;
						}

						if (IsEnabled(CustomComboPreset.VPR_AoE_Uncoiled) && HasEffect(Buffs.PoisedForTwinblood))
						{
							return UncoiledTwinblood;
						}

						if (IsEnabled(CustomComboPreset.VPR_AoE_Vicepit) && HasEffect(Buffs.FellskinsVenom))
						{
							return TwinbloodThresh;
						}

						if (IsEnabled(CustomComboPreset.VPR_AoE_Vicepit) && HasEffect(Buffs.FellhuntersVenom))
						{
							return TwinfangThresh;
						}

						if (IsEnabled(CustomComboPreset.VPR_AoE_Finishers) && ActionReady(OriginalHook(LastLash))
							&& Gauge.SerpentCombo is SerpentCombo.LastLash)
						{
							return LastLash;
						}

						if (IsEnabled(CustomComboPreset.VPR_AoE_Reawaken) && ActionReady(OriginalHook(SerpentsTail)) &&
							(Gauge.SerpentCombo is SerpentCombo.FirstLegacy ||
							Gauge.SerpentCombo is SerpentCombo.SecondLegacy ||
							Gauge.SerpentCombo is SerpentCombo.ThirdLegacy ||
							Gauge.SerpentCombo is SerpentCombo.FourthLegacy))
						{
							return OriginalHook(SerpentsTail);
						}

						if (IsEnabled(CustomComboPreset.VPR_ST_SerpentsIre) && ActionReady(SerpentsIre))
						{
							return SerpentsIre;
						}
					}

					if (IsEnabled(CustomComboPreset.VPR_AoE_Reawaken) && HasEffect(Buffs.Reawakened))
					{
						if (Gauge.AnguineTribute is 5 || (Gauge.AnguineTribute is 4 && !LevelChecked(Ouroboros)))
						{
							return OriginalHook(SteelMaw);
						}

						if (Gauge.AnguineTribute is 4 || (Gauge.AnguineTribute is 3 && !LevelChecked(Ouroboros)))
						{
							return OriginalHook(ReavingMaw);
						}

						if (Gauge.AnguineTribute is 3 || (Gauge.AnguineTribute is 2 && !LevelChecked(Ouroboros)))
						{
							return OriginalHook(HuntersDen);
						}

						if (Gauge.AnguineTribute is 2 || (Gauge.AnguineTribute is 1 && !LevelChecked(Ouroboros)))
						{
							return OriginalHook(SwiftskinsDen);
						}

						if (Gauge.AnguineTribute is 1)
						{
							return OriginalHook(Reawaken);
						}
					}

					if (GetBuffRemainingTime(Buffs.Swiftscaled) > 10 && GetBuffRemainingTime(Buffs.HuntersInstinct) > 10
						&& !Gauge.DreadCombo.HasFlag(DreadCombo.PitOfDread)
						&& !Gauge.DreadCombo.HasFlag(DreadCombo.SwiftskinsDen) && !Gauge.DreadCombo.HasFlag(DreadCombo.HuntersDen))
					{
						if (IsEnabled(CustomComboPreset.VPR_AoE_Reawaken) && ActionReady(Reawaken)
							&& (GetCooldownRemainingTime(SerpentsIre) > 30 || Gauge.SerpentOffering == 100)
							&& (Gauge.SerpentOffering >= 50 || HasEffect(Buffs.ReadyToReawaken)))
						{
							return Reawaken;
						}

						if (IsEnabled(CustomComboPreset.VPR_AoE_Uncoiled) && ActionReady(UncoiledFury) && Gauge.RattlingCoilStacks > 0
							&& (GetCooldownRemainingTime(SerpentsIre) > 60 || Gauge.RattlingCoilStacks == 3))
						{
							return UncoiledFury;
						}
					}

					if (IsEnabled(CustomComboPreset.VPR_AoE_Vicepit) && Gauge.DreadCombo.HasFlag(DreadCombo.SwiftskinsDen))
					{
						return HuntersDen;
					}

					if (IsEnabled(CustomComboPreset.VPR_AoE_Vicepit) && Gauge.DreadCombo.HasFlag(DreadCombo.HuntersDen))
					{
						return SwiftskinsDen;
					}

					if (IsEnabled(CustomComboPreset.VPR_AoE_Vicepit) && Gauge.DreadCombo.HasFlag(DreadCombo.PitOfDread))
					{
						if (GetBuffRemainingTime(Buffs.Swiftscaled) <= GetBuffRemainingTime(Buffs.HuntersInstinct))
						{
							return SwiftskinsDen;
						}

						if (GetBuffRemainingTime(Buffs.HuntersInstinct) < GetBuffRemainingTime(Buffs.Swiftscaled))
						{
							return HuntersDen;
						}
					}

					if (IsEnabled(CustomComboPreset.VPR_AoE_Vicepit) && ActionReady(Vicepit)
						&& (WasLastWeaponskill(JaggedMaw) || WasLastWeaponskill(BloodiedMaw)))
					{
						return Vicepit;
					}

					if (comboTime > 0)
					{
						if (lastComboMove is SteelMaw or ReavingMaw)
						{
							if (HasEffect(Buffs.GrimhuntersVenom))
							{
								return OriginalHook(SwiftskinsBite);
							}

							if (HasEffect(Buffs.GrimskinsVenom))
							{
								return OriginalHook(HuntersBite);
							}

							if (ActionReady(OriginalHook(SwiftskinsBite)))
							{
								return OriginalHook(SwiftskinsBite);
							}
						}

						if (lastComboMove is SwiftskinsBite or HuntersBite)
						{
							if (HasEffect(Buffs.GrimhuntersVenom))
							{
								return OriginalHook(JaggedMaw);
							}

							if (HasEffect(Buffs.GrimskinsVenom))
							{
								return OriginalHook(BloodiedMaw);
							}

							if (ActionReady(OriginalHook(BloodiedMaw)))
							{
								return OriginalHook(BloodiedMaw);
							}
						}

						if (HasEffect(Buffs.HonedReavers))
						{
							return OriginalHook(ReavingMaw);
						}

						if (HasEffect(Buffs.HonedSteel))
						{
							return OriginalHook(SteelMaw);
						}
					}

					return SteelMaw;
				}

				return actionID;
			}
		}

		internal class VPR_Vicewinder : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.VPR_Vicewinder;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Vicewinder or HuntersCoil or SwiftskinsCoil) && IsEnabled(CustomComboPreset.VPR_Vicewinder))
				{
					if (CanWeave(SwiftskinsCoil))
					{
						if (HasEffect(Buffs.SwiftskinsVenom))
						{
							return TwinbloodBite;
						}

						if (HasEffect(Buffs.HuntersVenom))
						{
							return TwinfangBite;
						}
					}

					if (Gauge.DreadCombo.HasFlag(DreadCombo.SwiftskinsCoil))
					{
						return HuntersCoil;
					}

					if (Gauge.DreadCombo.HasFlag(DreadCombo.HuntersCoil))
					{
						return SwiftskinsCoil;
					}

					if (Gauge.DreadCombo.HasFlag(DreadCombo.Dreadwinder))
					{
						if (GetBuffRemainingTime(Buffs.Swiftscaled) <= GetBuffRemainingTime(Buffs.HuntersInstinct))
						{
							return SwiftskinsCoil;
						}

						if (GetBuffRemainingTime(Buffs.HuntersInstinct) < GetBuffRemainingTime(Buffs.Swiftscaled))
						{
							return HuntersCoil;
						}
					}
				}

				return actionID;
			}
		}

		internal class VPR_Vicepit : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.VPR_Vicepit;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Vicepit or HuntersDen or SwiftskinsDen) && IsEnabled(CustomComboPreset.VPR_Vicepit))
				{
					if (CanWeave(SwiftskinsDen))
					{
						if (HasEffect(Buffs.FellskinsVenom))
						{
							return TwinbloodThresh;
						}

						if (HasEffect(Buffs.FellhuntersVenom))
						{
							return TwinfangThresh;
						}
					}

					if (Gauge.DreadCombo.HasFlag(DreadCombo.SwiftskinsDen))
					{
						return HuntersDen;
					}

					if (Gauge.DreadCombo.HasFlag(DreadCombo.HuntersDen))
					{
						return SwiftskinsDen;
					}

					if (Gauge.DreadCombo.HasFlag(DreadCombo.PitOfDread))
					{
						if (GetBuffRemainingTime(Buffs.Swiftscaled) <= GetBuffRemainingTime(Buffs.HuntersInstinct))
						{
							return SwiftskinsDen;
						}

						if (GetBuffRemainingTime(Buffs.HuntersInstinct) < GetBuffRemainingTime(Buffs.Swiftscaled))
						{
							return HuntersDen;
						}
					}
				}

				return actionID;
			}
		}

		internal class VPR_Uncoiled : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.VPR_Uncoiled;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (actionID is UncoiledFury && IsEnabled(CustomComboPreset.VPR_Uncoiled))
				{
					if (CanWeave(UncoiledFury))
					{
						if (HasEffect(Buffs.PoisedForTwinfang))
						{
							return UncoiledTwinfang;
						}

						if (HasEffect(Buffs.PoisedForTwinblood))
						{
							return UncoiledTwinblood;
						}
					}
				}

				return actionID;
			}
		}
	}
}