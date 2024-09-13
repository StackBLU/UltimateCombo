using Dalamud.Game.ClientState.JobGauge.Types;
using System.Collections.Generic;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.CustomCombo;
using UltimateCombo.Data;

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
			Recitation = 16542,
			ChainStratagem = 7436,
			DeploymentTactics = 3585;

		internal static readonly List<uint>
			BroilList = [Ruin, Broil, Broil2, Broil3, Broil4],
			AetherflowList = [EnergyDrain, Lustrate, SacredSoil, Indomitability, Excogitation],
			FairyList = [WhisperingDawn, FeyBlessing, FeyIllumination, Dissipation, Aetherpact];

		internal static class Buffs
		{
			internal const ushort
				Galvanize = 297,
				SacredSoil = 299,
				Recitation = 1896,
				Dissipation = 791,
				ImpactImminent = 3882;
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
			public static UserInt
				SCH_ST_DPS_Lucid = new("SCH_ST_DPS_LucidOption", 7500),
				SCH_AoE_DPS_Lucid = new("SCH_ST_DPS_LucidOption", 7500);
		}

		internal class SCH_ST_DPS : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SCH_ST_DPS;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Ruin or Broil or Broil2 or Broil3 or Broil4) && IsEnabled(CustomComboPreset.SCH_ST_DPS))
				{
					if (IsEnabled(CustomComboPreset.SCH_ST_DPS_Lucid) && ActionReady(All.LucidDreaming) && LocalPlayer.CurrentMp <= 1000)
					{
						return All.LucidDreaming;
					}

					if (CanWeave(actionID) && ActionWatching.NumberOfGcdsUsed >= 2)
					{
						if (IsEnabled(CustomComboPreset.SCH_ST_DPS_Lucid) && ActionReady(All.LucidDreaming)
							&& LocalPlayer.CurrentMp <= GetOptionValue(Config.SCH_ST_DPS_Lucid))
						{
							return All.LucidDreaming;
						}

						if (IsEnabled(CustomComboPreset.SCH_ST_DPS_Seraph) && ActionReady(OriginalHook(SummonSeraph)) && Gauge.SeraphTimer > 0
							&& Gauge.SeraphTimer < 5000)
						{
							return OriginalHook(SummonSeraph);
						}

						if (IsEnabled(CustomComboPreset.SCH_ST_DPS_Aetherflow) && ActionReady(Aetherflow) && Gauge.Aetherflow == 0)
						{
							return Aetherflow;
						}

						if (IsEnabled(CustomComboPreset.SCH_ST_DPS_Dissipation) && ActionReady(Dissipation) && HasPetPresent() && Gauge.Aetherflow == 0
							&& Gauge.SeraphTimer == 0)
						{
							return Dissipation;
						}

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

						if (IsEnabled(CustomComboPreset.SCH_ST_DPS_EnergyDrain) && ActionReady(EnergyDrain) && Gauge.Aetherflow > 0)
						{
							if (((GetCooldownRemainingTime(Aetherflow) <= 10f || GetCooldownRemainingTime(Dissipation) <= 10f)
							&& !HasEffect(Buffs.Dissipation)) || TargetHasEffect(Debuffs.ChainStratagem))
							{
								return EnergyDrain;
							}
						}
					}

					if (IsEnabled(CustomComboPreset.SCH_ST_DPS_Fairy) && !HasPetPresent() && !HasEffect(Buffs.Dissipation))
					{
						return SummonEos;
					}

					if (IsEnabled(CustomComboPreset.SCH_ST_DPS_Bio) && ActionReady(OriginalHook(Biolysis)) && ActionWatching.NumberOfGcdsUsed >= 3
						&& (EnemyHealthCurrentHp() >= LocalPlayer.MaxHp || EnemyHealthMaxHp() == 44)
						&& (!TargetHasEffect(BioList[OriginalHook(Biolysis)]) || GetDebuffRemainingTime(BioList[OriginalHook(Biolysis)]) <= 3))
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
					if (IsEnabled(CustomComboPreset.SCH_AoE_DPS_Lucid) && ActionReady(All.LucidDreaming) && LocalPlayer.CurrentMp <= 1000)
					{
						return All.LucidDreaming;
					}

					if (CanWeave(actionID))
					{
						if (IsEnabled(CustomComboPreset.SCH_AoE_DPS_Lucid) && ActionReady(All.LucidDreaming)
							&& LocalPlayer.CurrentMp <= GetOptionValue(Config.SCH_AoE_DPS_Lucid))
						{
							return All.LucidDreaming;
						}

						if (IsEnabled(CustomComboPreset.SCH_AoE_Aetherflow) && ActionReady(Aetherflow) && Gauge.Aetherflow == 0)
						{
							return Aetherflow;
						}

						if (IsEnabled(CustomComboPreset.SCH_AoE_DPS_EnergyDrain) && ActionReady(EnergyDrain) && Gauge.Aetherflow > 0)
						{
							if (((GetCooldownRemainingTime(Aetherflow) <= 10f || GetCooldownRemainingTime(Dissipation) <= 10f)
								&& !HasEffect(Buffs.Dissipation)) || TargetHasEffect(Debuffs.ChainStratagem))
							{
								return EnergyDrain;
							}
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

					return Lustrate;
				}

				return actionID;
			}
		}

		internal class SCH_Raise : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SCH_Raise;
			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (actionID is Resurrection && IsEnabled(CustomComboPreset.SCH_Raise))
				{
					if (ActionReady(All.Swiftcast))
					{
						return All.Swiftcast;
					}

					return Resurrection;
				}

				return actionID;
			}
		}
	}
}