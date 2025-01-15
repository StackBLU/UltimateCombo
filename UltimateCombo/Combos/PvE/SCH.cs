using Dalamud.Game.ClientState.JobGauge.Types;
using System.Collections.Generic;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.CustomCombo;
using UltimateCombo.Data;
using UltimateCombo.Services;

namespace UltimateCombo.Combos.PvE
{
	internal static class SCH
	{
		public const byte JobID = 28;

		internal const uint

			Physick = 190,
			Adloquium = 185,
			Succor = 186,
			Lustrate = 189,
			SacredSoil = 188,
			Indomitability = 3583,
			Excogitation = 7434,
			Consolation = 16546,
			Resurrection = 173,
			Concitation = 37013,

			Bio = 17864,
			Bio2 = 17865,
			Biolysis = 16540,
			Ruin = 17869,
			Ruin2 = 17870,
			Broil = 3584,
			Broil2 = 7435,
			Broil3 = 16541,
			Broil4 = 25865,
			EnergyDrain = 167,
			ArtOfWar = 16539,
			ArtOfWarII = 25866,
			BanefulImpaction = 37012,

			SummonSeraph = 16545,
			SummonEos = 17215,
			WhisperingDawn = 16537,
			FeyIllumination = 16538,
			Dissipation = 3587,
			Aetherpact = 7437,
			DissolveUnion = 7869,
			FeyBlessing = 16543,

			Aetherflow = 166,
			Protraction = 25867,
			Recitation = 16542,
			ChainStratagem = 7436,
			DeploymentTactics = 3585;

		internal static class Buffs
		{
			internal const ushort
				Galvanize = 297,
				SacredSoil = 299,
				Recitation = 1896,
				Dissipation = 791,
				ImpactImminent = 3882,
				Seraphism = 3884;
		}

		internal static class Debuffs
		{
			internal const ushort
				Bio1 = 179,
				Bio2 = 189,
				Biolysis = 1895,
				ChainStratagem = 1221;
		}

		internal static readonly Dictionary<uint, ushort>
			BioList = new() {
				{ Bio, Debuffs.Bio1 },
				{ Bio2, Debuffs.Bio2 },
				{ Biolysis, Debuffs.Biolysis }
			};

		private static SCHGauge Gauge
		{
			get
			{
				return CustomComboFunctions.GetJobGauge<SCHGauge>();
			}
		}

		public static class Config
		{

		}

		internal class SCH_ST_DPS : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SCH_ST_DPS;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Ruin or Broil or Broil2 or Broil3 or Broil4) && IsEnabled(CustomComboPreset.SCH_ST_DPS))
				{
					if ((WasLastSpell(Succor) || WasLastSpell(Concitation) || WasLastSpell(Adloquium)
						|| WasLastSpell(SummonEos)) && !InCombat())
					{
						ActionWatching.CombatActions.Clear();
					}

					if (IsEnabled(CustomComboPreset.SCH_ST_DPS_Aetherflow) && ActionReady(Aetherflow) && Gauge.Aetherflow == 0
						&& LocalPlayer.CurrentMp < 1000)
					{
						return Aetherflow;
					}

					if (CanWeave(actionID))
					{
						if (ActionWatching.NumberOfGcdsUsed >= 4 || Service.Configuration.IgnoreGCDChecks)
						{
							if (IsEnabled(CustomComboPreset.SCH_ST_DPS_ChainStrat))
							{
								if (ActionReady(ChainStratagem) && !TargetHasEffectAny(Debuffs.ChainStratagem))
								{
									return ChainStratagem;
								}

								if (ActionReady(BanefulImpaction) && HasEffect(Buffs.ImpactImminent))
								{
									return BanefulImpaction;
								}
							}

							if (IsEnabled(CustomComboPreset.SCH_ST_DPS_Seraph) && ActionReady(OriginalHook(SummonSeraph)) && Gauge.SeraphTimer > 0
							&& Gauge.SeraphTimer < 5000)
							{
								return OriginalHook(SummonSeraph);
							}

							if (IsEnabled(CustomComboPreset.SCH_ST_DPS_EnergyDrain) && ActionReady(EnergyDrain) && Gauge.Aetherflow > 0
								&& (GetCooldownRemainingTime(Aetherflow) <= (Gauge.Aetherflow * GetCooldown(actionID).CooldownTotal) + 0.5
								|| TargetHasEffectAny(Debuffs.ChainStratagem)))
							{
								return EnergyDrain;
							}

							if (IsEnabled(CustomComboPreset.SCH_ST_DPS_Dissipation) && ActionReady(Dissipation)
								&& Gauge.Aetherflow == 0 && IsOnCooldown(Aetherflow) && GetDebuffRemainingTime(Debuffs.ChainStratagem) >= 5)
							{
								return Dissipation;
							}
						}

						if (ActionWatching.NumberOfGcdsUsed >= 2 || Service.Configuration.IgnoreGCDChecks)
						{
							if (IsEnabled(CustomComboPreset.SCH_ST_DPS_Aetherflow) && ActionReady(Aetherflow) && Gauge.Aetherflow == 0)
							{
								return Aetherflow;
							}
						}
					}

					if (IsEnabled(CustomComboPreset.SCH_ST_DPS_Fairy) && !HasPetPresent() && !HasEffect(Buffs.Dissipation))
					{
						return SummonEos;
					}

					if (IsEnabled(CustomComboPreset.SCH_ST_DPS_Bio) && ActionReady(OriginalHook(Biolysis))
						&& (ActionWatching.NumberOfGcdsUsed >= 1 || Service.Configuration.IgnoreGCDChecks)
						&& (EnemyHealthCurrentHp() >= LocalPlayer.MaxHp || EnemyHealthMaxHp() == 44)
						&& (!TargetHasEffect(BioList[OriginalHook(Biolysis)])
						|| GetDebuffRemainingTime(BioList[OriginalHook(Biolysis)]) <= 3
						|| ActionWatching.NumberOfGcdsUsed == 11))
					{
						return OriginalHook(Biolysis);
					}

					if (IsEnabled(CustomComboPreset.SCH_ST_DPS_Ruin2Movement) && ActionReady(Ruin2) && IsMoving)
					{
						return OriginalHook(Ruin2);
					}

					return OriginalHook(Ruin);
				}

				return actionID;
			}
		}

		internal class SCH_AoE_DPS : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SCH_AoE_DPS;
			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is ArtOfWar or ArtOfWarII) && IsEnabled(CustomComboPreset.SCH_AoE_DPS))
				{
					if (IsEnabled(CustomComboPreset.SCH_ST_DPS_Aetherflow) && ActionReady(Aetherflow) && Gauge.Aetherflow == 0
						&& LocalPlayer.CurrentMp < 1000)
					{
						return Aetherflow;
					}

					if (CanWeave(actionID))
					{
						if (IsEnabled(CustomComboPreset.SCH_AoE_Aetherflow) && ActionReady(Aetherflow) && Gauge.Aetherflow == 0)
						{
							return Aetherflow;
						}

						if (IsEnabled(CustomComboPreset.SCH_AoE_DPS_ChainStrat) && LevelChecked(BanefulImpaction))
						{
							if (ActionReady(ChainStratagem) && !TargetHasEffectAny(Debuffs.ChainStratagem))
							{
								return ChainStratagem;
							}

							if (ActionReady(BanefulImpaction) && HasEffect(Buffs.ImpactImminent))
							{
								return BanefulImpaction;
							}
						}

						if (IsEnabled(CustomComboPreset.SCH_AoE_DPS_EnergyDrain) && ActionReady(EnergyDrain) && Gauge.Aetherflow > 0
							&& (GetCooldownRemainingTime(Aetherflow) <= (Gauge.Aetherflow * GetCooldown(actionID).CooldownTotal) + 0.5
							|| TargetHasEffect(Debuffs.ChainStratagem)))
						{
							return EnergyDrain;
						}

						if (IsEnabled(CustomComboPreset.SCH_AoE_DPS_Dissipation) && ActionReady(Dissipation)
							&& Gauge.Aetherflow == 0 && IsOnCooldown(Aetherflow) && GetDebuffRemainingTime(Debuffs.ChainStratagem) >= 10)
						{
							return Dissipation;
						}
					}

					if (IsEnabled(CustomComboPreset.SCH_AoE_DPS_Fairy) && !HasPetPresent() && !HasEffect(Buffs.Dissipation))
					{
						return SummonEos;
					}

					return OriginalHook(ArtOfWar);
				}

				return actionID;
			}
		}

		internal class SCH_ST_Heals : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SCH_ST_Heals;
			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Physick or Adloquium) && IsEnabled(CustomComboPreset.SCH_ST_Heals))
				{
					if (ActionReady(Adloquium))
					{
						return Adloquium;
					}

					return Physick;
				}

				return actionID;
			}
		}

		internal class SCH_Lustrate : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SCH_Lustrate;
			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Lustrate or Excogitation) && IsEnabled(CustomComboPreset.SCH_Lustrate))
				{
					if (ActionReady(Excogitation))
					{
						return Excogitation;
					}

					if (ActionReady(Lustrate) && Gauge.Aetherflow > 0)
					{
						return Lustrate;
					}
				}

				return actionID;
			}
		}

		internal class SCH_DissipationDrain : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SCH_DissipationDrain;
			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Dissipation or EnergyDrain or Aetherflow) && IsEnabled(CustomComboPreset.SCH_DissipationDrain))
				{
					if (ActionReady(EnergyDrain) && Gauge.Aetherflow > 0 && ActionReady(Dissipation))
					{
						return EnergyDrain;
					}

					if (ActionReady(Dissipation) || (GetCooldownRemainingTime(Dissipation) < 30 && LevelChecked(Dissipation)
						&& !HasEffect(Buffs.Seraphism)))
					{
						return Dissipation;
					}

					if (LevelChecked(Dissipation))
					{
						return Aetherflow;
					}

					return OriginalHook(11);
				}

				return actionID;
			}
		}

		internal class SCH_SeraphBlessing : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SCH_SeraphBlessing;
			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is FeyBlessing or SummonSeraph or Consolation) && IsEnabled(CustomComboPreset.SCH_SeraphBlessing))
				{
					if (ActionReady(Consolation) && Gauge.SeraphTimer > 0)
					{
						return Consolation;
					}

					if (ActionReady(SummonSeraph))
					{
						return SummonSeraph;
					}

					if (ActionReady(FeyBlessing) && Gauge.SeraphTimer == 0)
					{
						return FeyBlessing;
					}
				}

				return actionID;
			}
		}

		internal class SCH_ProRecitation : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SCH_ProRecitation;
			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Protraction or Recitation) && IsEnabled(CustomComboPreset.SCH_ProRecitation))
				{
					if (ActionReady(Recitation))
					{
						return Recitation;
					}

					if (ActionReady(Protraction))
					{
						return Protraction;
					}
				}

				return actionID;
			}
		}

		internal class SCH_NoDissipate : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SCH_NoDissipate;
			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (actionID is Dissipation && IsEnabled(CustomComboPreset.SCH_NoDissipate))
				{
					if (HasEffect(Buffs.Seraphism))
					{
						return OriginalHook(11);
					}
				}

				return actionID;
			}
		}

		internal class SCH_SeraphNoWaste : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SCH_SeraphNoWaste;
			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (actionID is SummonSeraph && IsEnabled(CustomComboPreset.SCH_SeraphNoWaste))
				{
					if (WasLastAction(WhisperingDawn) || WasLastAction(FeyIllumination) || WasLastAction(FeyBlessing))
					{
						return OriginalHook(11);
					}
				}

				return actionID;
			}
		}
	}
}