using Dalamud.Game.ClientState.JobGauge.Types;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.CustomCombo;
using UltimateCombo.Data;
using UltimateCombo.Services;

namespace UltimateCombo.Combos.PvE
{
	internal class MCH
	{
		public const byte JobID = 31;

		public const uint
			CleanShot = 2873,
			HeatedCleanShot = 7413,
			SplitShot = 2866,
			HeatedSplitShot = 7411,
			SlugShot = 2868,
			HeatedSlugShot = 7412,
			GaussRound = 2874,
			Ricochet = 2890,
			Reassemble = 2876,
			Drill = 16498,
			HotShot = 2872,
			AirAnchor = 16500,
			Hypercharge = 17209,
			Heatblast = 7410,
			SpreadShot = 2870,
			Scattergun = 25786,
			AutoCrossbow = 16497,
			RookAutoturret = 2864,
			RookOverdrive = 7415,
			AutomatonQueen = 16501,
			QueenOverdrive = 16502,
			Tactician = 16889,
			Chainsaw = 25788,
			Bioblaster = 16499,
			BarrelStabilizer = 7414,
			Wildfire = 2878,
			Dismantle = 2887,
			Flamethrower = 7418,
			BlazingShot = 36978,
			DoubleCheck = 36979,
			Checkmate = 36980,
			Excavator = 36981,
			FullMetalField = 36982;

		public static class Buffs
		{
			public const ushort
				Reassembled = 851,
				Tactician = 1951,
				Wildfire = 1946,
				Overheated = 2688,
				Flamethrower = 1205,
				Hypercharged = 3864,
				ExcavatorReady = 3865,
				FullMetalMachinist = 3866;
		}

		public static class Debuffs
		{
			public const ushort
				Dismantled = 860,
				Bioblaster = 1866;
		}

		private static MCHGauge Gauge
		{
			get
			{
				return CustomComboFunctions.GetJobGauge<MCHGauge>();
			}
		}

		public static class Config
		{
			public static UserInt
				MCH_ST_Hypercharge = new("MCH_ST_Hypercharge", 50),
				MCH_ST_Queen = new("MCH_ST_Queen", 50),
				MCH_AoE_Hypercharge = new("MCH_AoE_Hypercharge", 50);
		}

		internal class MCH_ST_DPS : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MCH_ST_DPS;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is SplitShot or SlugShot or CleanShot or HeatedSplitShot or HeatedSlugShot or HeatedCleanShot) && IsEnabled(CustomComboPreset.MCH_ST_DPS))
				{
					if (IsEnabled(CustomComboPreset.MCH_ST_Reassemble) && ActionReady(Reassemble) && !InCombat()
						&& !HasEffect(Buffs.Reassembled))
					{
						return Reassemble;
					}

					if (CanWeave(actionID))
					{
						if (ActionWatching.NumberOfGcdsUsed >= 2 || Service.Configuration.IgnoreGCDChecks)
						{
							if (IsEnabled(CustomComboPreset.MCH_ST_Barrel) && ActionReady(BarrelStabilizer) && TargetIsBoss())
							{
								return BarrelStabilizer;
							}
						}

						if (IsEnabled(CustomComboPreset.MCH_ST_Queen) && ActionReady(OriginalHook(AutomatonQueen))
							&& Gauge.Battery >= GetOptionValue(Config.MCH_ST_Queen))
						{
							return OriginalHook(AutomatonQueen);
						}

						if (ActionWatching.NumberOfGcdsUsed >= 4 || Service.Configuration.IgnoreGCDChecks)
						{
							if (IsEnabled(CustomComboPreset.MCH_ST_Reassemble) && !HasEffect(Buffs.Reassembled)
								&& ActionReady(Reassemble) && !HasEffect(Buffs.Overheated)
								&& (ActionReady(Drill) || GetCooldownRemainingTime(Drill) < 1
								|| ActionReady(OriginalHook(AirAnchor)) || GetCooldownRemainingTime(OriginalHook(AirAnchor)) < 1
								|| ActionReady(Chainsaw) || GetCooldownRemainingTime(Chainsaw) < 1))
							{
								return Reassemble;
							}
						}

						if (IsEnabled(CustomComboPreset.MCH_ST_Wildfire) && ActionReady(Wildfire)
							&& (HasEffect(Buffs.Hypercharged) || HasEffect(Buffs.Overheated)))
						{
							return Wildfire;
						}

						if (IsEnabled(CustomComboPreset.MCH_ST_GaussRico) && ActionReady(OriginalHook(GaussRound))
							&& GetRemainingCharges(OriginalHook(GaussRound)) >= GetRemainingCharges(OriginalHook(Ricochet))
							&& !HasEffect(Buffs.Overheated)
							&& GetRemainingCharges(OriginalHook(GaussRound)) >= GetMaxCharges(OriginalHook(GaussRound)) - 1)
						{
							return OriginalHook(GaussRound);
						}

						if (IsEnabled(CustomComboPreset.MCH_ST_GaussRico) && ActionReady(OriginalHook(Ricochet))
							&& GetRemainingCharges(OriginalHook(Ricochet)) >= GetRemainingCharges(OriginalHook(GaussRound))
							&& !HasEffect(Buffs.Overheated)
							&& GetRemainingCharges(OriginalHook(Ricochet)) >= GetMaxCharges(OriginalHook(Ricochet)) - 1)
						{
							return OriginalHook(Ricochet);
						}

						if (IsEnabled(CustomComboPreset.MCH_ST_GaussRico) && ActionReady(OriginalHook(GaussRound))
							&& ActionWatching.GetAttackType(ActionWatching.LastAction) != ActionWatching.ActionAttackType.Ability
							&& GetRemainingCharges(OriginalHook(GaussRound)) >= GetRemainingCharges(OriginalHook(Ricochet))
							&& HasEffect(Buffs.Overheated))
						{
							return OriginalHook(GaussRound);
						}

						if (IsEnabled(CustomComboPreset.MCH_ST_GaussRico) && ActionReady(OriginalHook(Ricochet))
							&& ActionWatching.GetAttackType(ActionWatching.LastAction) != ActionWatching.ActionAttackType.Ability
							&& GetRemainingCharges(OriginalHook(Ricochet)) >= GetRemainingCharges(OriginalHook(GaussRound))
							&& HasEffect(Buffs.Overheated))
						{
							return OriginalHook(Ricochet);
						}

						if (IsEnabled(CustomComboPreset.MCH_ST_Hypercharge) && !HasEffect(Buffs.Overheated) && ActionReady(Hypercharge)
							&& (GetCooldownRemainingTime(Drill) > 5 || !LevelChecked(Drill) || !IsEnabled(CustomComboPreset.MCH_ST_Drill))
							&& (GetCooldownRemainingTime(OriginalHook(AirAnchor)) > 5 || !LevelChecked(OriginalHook(AirAnchor)) || !IsEnabled(CustomComboPreset.MCH_ST_AirAnchor))
							&& (GetCooldownRemainingTime(Chainsaw) > 5 || !LevelChecked(Chainsaw) || !IsEnabled(CustomComboPreset.MCH_ST_Chainsaw))
							&& (Gauge.Heat >= GetOptionValue(Config.MCH_ST_Hypercharge) || HasEffect(Buffs.Hypercharged)))
						{
							return Hypercharge;
						}
					}

					if (IsEnabled(CustomComboPreset.MCH_ST_HeatBlast) && ActionReady(OriginalHook(Heatblast)) && HasEffect(Buffs.Overheated))
					{
						return OriginalHook(Heatblast);
					}

					if (IsEnabled(CustomComboPreset.MCH_ST_AirAnchor) && ActionReady(OriginalHook(AirAnchor))
						&& !HasEffect(Buffs.Reassembled) && !WasLastAbility(Reassemble)
						&& GetCooldownRemainingTime(OriginalHook(AirAnchor)) < 1 && !HasEffect(Buffs.Overheated))
					{
						return OriginalHook(AirAnchor);
					}

					if (IsEnabled(CustomComboPreset.MCH_ST_Drill) && ActionReady(Drill) && !HasEffect(Buffs.Overheated))
					{
						return Drill;
					}

					if (IsEnabled(CustomComboPreset.MCH_ST_Chainsaw) && ActionReady(Chainsaw)
						&& GetCooldownRemainingTime(Chainsaw) < 1 && !HasEffect(Buffs.Overheated))
					{
						return Chainsaw;
					}

					if (IsEnabled(CustomComboPreset.MCH_ST_Chainsaw) && HasEffect(Buffs.ExcavatorReady))
					{
						return Excavator;
					}

					if (IsEnabled(CustomComboPreset.MCH_ST_Barrel) && HasEffect(Buffs.FullMetalMachinist))
					{
						return FullMetalField;
					}

					if ((comboTime > 0
						&& (GetCooldownRemainingTime(Drill) > 0.5 || !LevelChecked(Drill)
						|| ActionWatching.NumberOfGcdsUsed <= 2 || !IsEnabled(CustomComboPreset.MCH_ST_Drill))
						&& (GetCooldownRemainingTime(OriginalHook(AirAnchor)) > 0.5 || !LevelChecked(OriginalHook(AirAnchor))
						|| ActionWatching.NumberOfGcdsUsed <= 2 || !IsEnabled(CustomComboPreset.MCH_ST_AirAnchor))
						&& (GetCooldownRemainingTime(Chainsaw) > 0.5 || !LevelChecked(Chainsaw)))
						|| ActionWatching.NumberOfGcdsUsed <= 2 || !IsEnabled(CustomComboPreset.MCH_ST_Chainsaw))
					{
						if ((lastComboMove is SplitShot or HeatedSplitShot) && ActionReady(OriginalHook(SlugShot)))
						{
							return OriginalHook(SlugShot);
						}

						if ((lastComboMove is SlugShot or HeatedSlugShot) && ActionReady(OriginalHook(CleanShot)))
						{
							return OriginalHook(CleanShot);
						}
					}

					return OriginalHook(SplitShot);
				}

				return actionID;
			}
		}

		internal class MCH_AoE_DPS : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MCH_AoE_DPS;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is SpreadShot or Scattergun or AutoCrossbow) && IsEnabled(CustomComboPreset.MCH_AoE_DPS))
				{
					if (CanWeave(actionID))
					{
						if (IsEnabled(CustomComboPreset.MCH_AoE_Barrel) && ActionReady(BarrelStabilizer))
						{
							return BarrelStabilizer;
						}

						if (IsEnabled(CustomComboPreset.MCH_AoE_Reassemble) && ActionReady(Reassemble)
							&& !HasEffect(Buffs.Reassembled) && !HasEffect(Buffs.Overheated)
							&& (ActionReady(Chainsaw) || GetCooldownRemainingTime(Chainsaw) < 1 || HasEffect(Buffs.ExcavatorReady)))
						{
							return Reassemble;
						}

						if (IsEnabled(CustomComboPreset.MCH_AoE_GaussRico) && ActionReady(OriginalHook(GaussRound))
							&& GetRemainingCharges(OriginalHook(GaussRound)) >= GetRemainingCharges(OriginalHook(Ricochet))
							&& !HasEffect(Buffs.Overheated)
							&& GetRemainingCharges(OriginalHook(GaussRound)) >= GetMaxCharges(OriginalHook(GaussRound)) - 1)
						{
							return OriginalHook(GaussRound);
						}

						if (IsEnabled(CustomComboPreset.MCH_AoE_GaussRico) && ActionReady(OriginalHook(Ricochet))
							&& GetRemainingCharges(OriginalHook(Ricochet)) >= GetRemainingCharges(OriginalHook(GaussRound))
							&& !HasEffect(Buffs.Overheated)
							&& GetRemainingCharges(OriginalHook(Ricochet)) >= GetMaxCharges(OriginalHook(Ricochet)) - 1)
						{
							return OriginalHook(Ricochet);
						}

						if (IsEnabled(CustomComboPreset.MCH_AoE_GaussRico) && ActionReady(OriginalHook(GaussRound))
							&& ActionWatching.GetAttackType(ActionWatching.LastAction) != ActionWatching.ActionAttackType.Ability
							&& GetRemainingCharges(OriginalHook(GaussRound)) >= GetRemainingCharges(OriginalHook(Ricochet))
							&& HasEffect(Buffs.Overheated))
						{
							return OriginalHook(GaussRound);
						}

						if (IsEnabled(CustomComboPreset.MCH_AoE_GaussRico) && ActionReady(OriginalHook(Ricochet))
							&& ActionWatching.GetAttackType(ActionWatching.LastAction) != ActionWatching.ActionAttackType.Ability
							&& GetRemainingCharges(OriginalHook(Ricochet)) >= GetRemainingCharges(OriginalHook(GaussRound))
							&& HasEffect(Buffs.Overheated))
						{
							return OriginalHook(Ricochet);
						}

						if (IsEnabled(CustomComboPreset.MCH_AoE_Hypercharge) && !HasEffect(Buffs.Overheated) && ActionReady(Hypercharge)
						&& (GetCooldownRemainingTime(Bioblaster) > 5 || !LevelChecked(Bioblaster))
						&& (Gauge.Heat >= GetOptionValue(Config.MCH_AoE_Hypercharge) || HasEffect(Buffs.Hypercharged)))
						{
							return Hypercharge;
						}
					}

					if (IsEnabled(CustomComboPreset.MCH_AoE_Crossbow) && ActionReady(AutoCrossbow) && HasEffect(Buffs.Overheated))
					{
						return AutoCrossbow;
					}

					if (IsEnabled(CustomComboPreset.MCH_AoE_Bioblaster) && ActionReady(Bioblaster)
						&& !HasEffect(Buffs.Overheated) && !HasEffect(Buffs.Reassembled)
						&& (!TargetHasEffect(Debuffs.Bioblaster) || GetDebuffRemainingTime(Debuffs.Bioblaster) < 3))
					{
						return Bioblaster;
					}

					if (IsEnabled(CustomComboPreset.MCH_AoE_Chainsaw) && ActionReady(Chainsaw)
						&& GetCooldownRemainingTime(Chainsaw) < 1 && !HasEffect(Buffs.Overheated))
					{
						return Chainsaw;
					}

					if (IsEnabled(CustomComboPreset.MCH_AoE_Chainsaw) && HasEffect(Buffs.ExcavatorReady))
					{
						return Excavator;
					}

					if (IsEnabled(CustomComboPreset.MCH_AoE_Barrel) && HasEffect(Buffs.FullMetalMachinist))
					{
						return FullMetalField;
					}

					if (ActionReady(OriginalHook(Scattergun)))
					{
						return OriginalHook(Scattergun);
					}

					return OriginalHook(SpreadShot);
				}

				return actionID;
			}
		}

		internal class MCH_GaussRoundRicochet : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MCH_GaussRicochet;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is GaussRound or Ricochet or Checkmate or DoubleCheck) && IsEnabled(CustomComboPreset.MCH_GaussRicochet))
				{
					if (ActionReady(OriginalHook(GaussRound)) && GetRemainingCharges(OriginalHook(GaussRound)) >= GetRemainingCharges(OriginalHook(Ricochet)))
					{
						return OriginalHook(GaussRound);
					}

					if (ActionReady(OriginalHook(Ricochet)) && GetRemainingCharges(OriginalHook(Ricochet)) > GetRemainingCharges(OriginalHook(GaussRound)))
					{
						return OriginalHook(Ricochet);
					}
				}
				return actionID;
			}
		}

		internal class MCH_DismantleProtect : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MCH_DismantleProtect;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (actionID is Dismantle && IsEnabled(CustomComboPreset.MCH_DismantleProtect))
				{
					if (TargetHasEffectAny(Debuffs.Dismantled))
					{
						return OriginalHook(11);
					}
				}
				return actionID;
			}
		}
	}
}