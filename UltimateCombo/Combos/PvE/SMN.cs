using Dalamud.Game.ClientState.JobGauge.Types;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.CustomCombo;
using UltimateCombo.Data;
using UltimateCombo.Services;

namespace UltimateCombo.Combos.PvE
{
	internal class SMN
	{
		public const byte JobID = 27;

		public const uint
			SummonRuby = 25802,
			SummonTopaz = 25803,
			SummonEmerald = 25804,
			SummonIfrit = 25805,
			SummonTitan = 25806,
			SummonGaruda = 25807,
			SummonIfrit2 = 25838,
			SummonTitan2 = 25839,
			SummonGaruda2 = 25840,
			SummonCarbuncle = 25798,
			Gemshine = 25883,
			PreciousBrilliance = 25884,
			DreadwyrmTrance = 3581,
			RubyRuin1 = 25808,
			RubyRuin2 = 25811,
			RubyRuin3 = 25817,
			TopazRuin1 = 25809,
			TopazRuin2 = 25812,
			TopazRuin3 = 25818,
			EmeraldRuin1 = 25810,
			EmeraldRuin2 = 25813,
			EmeraldRuin3 = 25819,
			Outburst = 16511,
			RubyOutburst = 25814,
			TopazOutburst = 25815,
			EmeraldOutburst = 25816,
			RubyRite = 25823,
			TopazRite = 25824,
			EmeraldRite = 25825,
			RubyCatastrophe = 25832,
			TopazCatastrophe = 25833,
			EmeraldCatastrophe = 25834,
			CrimsonCyclone = 25835,
			CrimsonStrike = 25885,
			MountainBuster = 25836,
			Slipstream = 25837,
			SummonBahamut = 7427,
			SummonPhoenix = 25831,
			SummonSolarBahamut = 36992,
			AstralImpulse = 25820,
			AstralFlare = 25821,
			Deathflare = 3582,
			EnkindleBahamut = 7429,
			FountainOfFire = 16514,
			BrandOfPurgatory = 16515,
			Rekindle = 25830,
			EnkindlePhoenix = 16516,
			UmbralImpulse = 36994,
			UmbralFlare = 36995,
			Sunflare = 36996,
			EnkindleSolarBahamut = 36998,
			LuxSolaris = 36997,
			AstralFlow = 25822,
			Ruin = 163,
			Ruin2 = 172,
			Ruin3 = 3579,
			Ruin4 = 7426,
			Tridisaster = 25826,
			RubyDisaster = 25827,
			TopazDisaster = 25828,
			EmeraldDisaster = 25829,
			EnergyDrain = 16508,
			Fester = 181,
			EnergySiphon = 16510,
			Painflare = 3578,
			Necrotize = 36990,
			SearingFlash = 36991,
			Resurrection = 173,
			RadiantAegis = 25799,
			Aethercharge = 25800,
			SearingLight = 25801;


		public static class Buffs
		{
			public const ushort
				FurtherRuin = 2701,
				GarudasFavor = 2725,
				TitansFavor = 2853,
				IfritsFavor = 2724,
				EverlastingFlight = 16517,
				SearingLight = 2703,
				RubysGlimmer = 3873,
				RefulgentLux = 3874;
		}

		private static SMNGauge Gauge
		{
			get
			{
				return CustomComboFunctions.GetJobGauge<SMNGauge>();
			}
		}

		public static class Config
		{
			public static UserBool
				SMN_ST_Astral_Swift = new("SMN_ST_Astral_Swift"),
				SMN_AoE_Astral_Swift = new("SMN_AoE_Astral_Swift");
		}

		internal class SMN_ST_DPS : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SMN_ST_DPS;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Ruin or Ruin2 or Ruin3) && IsEnabled(CustomComboPreset.SMN_ST_DPS))
				{
					if (IsEnabled(CustomComboPreset.SMN_ST_Reminder) && !HasPetPresent() && !InCombat())
					{
						return SummonCarbuncle;
					}

					if (WasLastSpell(SummonCarbuncle) && !InCombat())
					{
						ActionWatching.CombatActions.Clear();
					}

					if (!InCombat() && ActionReady(OriginalHook(Ruin)))
					{
						return OriginalHook(Ruin);
					}

					if (CanWeave(actionID) && (ActionWatching.NumberOfGcdsUsed >= 4 || Service.Configuration.IgnoreGCDChecks))
					{
						if (IsEnabled(CustomComboPreset.SMN_ST_SearingLight) && HasEffect(Buffs.RubysGlimmer))
						{
							return SearingFlash;
						}

						if (IsEnabled(CustomComboPreset.SMN_ST_SearingLight) && ActionReady(SearingLight))
						{
							return SearingLight;
						}

						if (IsEnabled(CustomComboPreset.SMN_ST_EnergyDrain)
							&& (ActionReady(EnergyDrain) || Gauge.AetherflowStacks > 0))
						{
							if (Gauge.AetherflowStacks > 0 && HasEffect(Buffs.SearingLight))
							{
								return OriginalHook(Necrotize);
							}

							if (ActionReady(EnergyDrain) && Gauge.AetherflowStacks == 0)
							{
								return EnergyDrain;
							}
						}

						if (IsEnabled(CustomComboPreset.SMN_ST_Astral) && ActionReady(OriginalHook(AstralFlow))
							&& (WasLastSpell(AstralImpulse) || WasLastSpell(FountainOfFire) || WasLastSpell(UmbralImpulse)
							|| HasEffect(Buffs.TitansFavor)))
						{
							return OriginalHook(AstralFlow);
						}

						if (IsEnabled(CustomComboPreset.SMN_ST_Enkindle) && ActionReady(OriginalHook(EnkindleBahamut))
							&& Gauge.AttunmentTimerRemaining == 0 && Gauge.Attunement == 0
							&& (WasLastSpell(AstralImpulse) || WasLastSpell(FountainOfFire) || WasLastSpell(UmbralImpulse)))
						{
							return OriginalHook(EnkindleBahamut);
						}
					}

					if (HasEffect(Buffs.GarudasFavor))
					{
						if (ActionReady(All.Swiftcast) && Config.SMN_ST_Astral_Swift)
						{
							return All.Swiftcast;
						}

						return OriginalHook(AstralFlow);
					}

					if (lastComboMove is CrimsonCyclone)
					{
						return OriginalHook(AstralFlow);
					}

					if (IsEnabled(CustomComboPreset.SMN_ST_Ruin4) && HasEffect(Buffs.FurtherRuin) && !LevelChecked(CrimsonCyclone)
						&& Gauge.SummonTimerRemaining == 0)
					{
						return Ruin4;
					}

					if (HasEffect(Buffs.IfritsFavor) && ActionReady(CrimsonCyclone))
					{
						if (IsEnabled(CustomComboPreset.SMN_ST_Ruin4) && HasEffect(Buffs.FurtherRuin))
						{
							return Ruin4;
						}

						return OriginalHook(AstralFlow);
					}

					if (Gauge.AttunmentTimerRemaining > 0 && Gauge.Attunement > 0)
					{
						return OriginalHook(Gemshine);
					}

					if (IsEnabled(CustomComboPreset.SMN_ST_SummonElements) && Gauge.AttunmentTimerRemaining == 0
						&& Gauge.SummonTimerRemaining == 0 && !IsOffCooldown(OriginalHook(SummonBahamut)))
					{
						if (ActionReady(OriginalHook(SummonGaruda)) && Gauge.IsGarudaReady)
						{
							return OriginalHook(SummonGaruda);
						}

						if (ActionReady(OriginalHook(SummonTitan)) && Gauge.IsTitanReady)
						{
							return OriginalHook(SummonTitan);
						}

						if (ActionReady(OriginalHook(SummonIfrit)) && Gauge.IsIfritReady)
						{
							return OriginalHook(SummonIfrit);
						}
					}

					if (IsEnabled(CustomComboPreset.SMN_ST_SummonBahaPhoenix) && ActionReady(OriginalHook(SummonBahamut)))
					{
						return OriginalHook(SummonBahamut);
					}

					if (ActionReady(OriginalHook(Ruin3)))
					{
						return OriginalHook(Ruin3);
					}
				}

				return actionID;
			}
		}

		internal class SMN_AoE_DPS : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SMN_AoE_DPS;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Outburst or Tridisaster) && IsEnabled(CustomComboPreset.SMN_AoE_DPS))
				{
					if (IsEnabled(CustomComboPreset.SMN_AoE_Reminder) && !HasPetPresent() && !InCombat())
					{
						return SummonCarbuncle;
					}

					if (!InCombat() && ActionReady(OriginalHook(Tridisaster)))
					{
						return OriginalHook(Tridisaster);
					}

					if (CanWeave(actionID))
					{
						if (IsEnabled(CustomComboPreset.SMN_AoE_SearingLight) && HasEffect(Buffs.RubysGlimmer))
						{
							return SearingFlash;
						}

						if (IsEnabled(CustomComboPreset.SMN_AoE_SearingLight) && ActionReady(SearingLight))
						{
							return SearingLight;
						}

						if (IsEnabled(CustomComboPreset.SMN_AoE_EnergySiphon)
							&& (ActionReady(EnergySiphon) || Gauge.AetherflowStacks > 0))
						{
							if (Gauge.AetherflowStacks > 0 && HasEffect(Buffs.SearingLight))
							{
								return OriginalHook(Painflare);
							}

							if (ActionReady(EnergySiphon) && Gauge.AetherflowStacks == 0)
							{
								return EnergySiphon;
							}
						}

						if (IsEnabled(CustomComboPreset.SMN_AoE_Astral) && ActionReady(OriginalHook(AstralFlow))
							&& (WasLastSpell(AstralFlare) || WasLastSpell(BrandOfPurgatory) || WasLastSpell(UmbralFlare)
							|| HasEffect(Buffs.TitansFavor)))
						{
							return OriginalHook(AstralFlow);
						}

						if (IsEnabled(CustomComboPreset.SMN_AoE_Enkindle) && ActionReady(OriginalHook(EnkindleBahamut))
							&& Gauge.AttunmentTimerRemaining == 0 && Gauge.Attunement == 0
							&& (WasLastSpell(AstralFlare) || WasLastSpell(BrandOfPurgatory) || WasLastSpell(UmbralFlare)))
						{
							return OriginalHook(EnkindleBahamut);
						}
					}

					if (HasEffect(Buffs.GarudasFavor))
					{
						if (ActionReady(All.Swiftcast) && Config.SMN_AoE_Astral_Swift)
						{
							return All.Swiftcast;
						}

						return OriginalHook(AstralFlow);
					}

					if (lastComboMove is CrimsonCyclone)
					{
						return OriginalHook(AstralFlow);
					}

					if (IsEnabled(CustomComboPreset.SMN_AoE_Ruin4) && HasEffect(Buffs.FurtherRuin) && !LevelChecked(CrimsonCyclone))
					{
						return Ruin4;
					}

					if (HasEffect(Buffs.IfritsFavor) && ActionReady(CrimsonCyclone))
					{
						if (IsEnabled(CustomComboPreset.SMN_AoE_Ruin4) && HasEffect(Buffs.FurtherRuin))
						{
							return Ruin4;
						}

						return OriginalHook(AstralFlow);
					}

					if (Gauge.AttunmentTimerRemaining > 0 && Gauge.Attunement > 0)
					{
						return OriginalHook(PreciousBrilliance);
					}

					if (IsEnabled(CustomComboPreset.SMN_AoE_SummonElements) && Gauge.AttunmentTimerRemaining == 0
						&& Gauge.SummonTimerRemaining == 0 && !IsOffCooldown(OriginalHook(SummonBahamut)))
					{
						if (ActionReady(OriginalHook(SummonGaruda)) && Gauge.IsGarudaReady)
						{
							return OriginalHook(SummonGaruda);
						}

						if (ActionReady(OriginalHook(SummonTitan)) && Gauge.IsTitanReady)
						{
							return OriginalHook(SummonTitan);
						}

						if (ActionReady(OriginalHook(SummonIfrit)) && Gauge.IsIfritReady)
						{
							return OriginalHook(SummonIfrit);
						}
					}

					if (IsEnabled(CustomComboPreset.SMN_AoE_SummonBahaPhoenix) && ActionReady(OriginalHook(SummonBahamut)))
					{
						return OriginalHook(SummonBahamut);
					}

					if (ActionReady(OriginalHook(Tridisaster)))
					{
						return OriginalHook(Tridisaster);
					}
				}

				return actionID;
			}
		}

		internal class SMN_EnergyDrain : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SMN_EnergyDrain;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is EnergyDrain or Fester or Necrotize) && IsEnabled(CustomComboPreset.SMN_EnergyDrain))
				{
					if (Gauge.AetherflowStacks > 0)
					{
						return OriginalHook(Necrotize);
					}

					return EnergyDrain;
				}

				return actionID;
			}
		}

		internal class SMN_EnergySiphon : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SMN_EnergySiphon;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is EnergySiphon or Painflare) && IsEnabled(CustomComboPreset.SMN_EnergySiphon))
				{
					if (Gauge.AetherflowStacks > 0)
					{
						return Painflare;
					}

					return EnergySiphon;
				}

				return actionID;
			}
		}

		internal class SMN_Enkindle : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SMN_Enkindle;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is SummonBahamut or SummonPhoenix or SummonSolarBahamut) && IsEnabled(CustomComboPreset.SMN_Enkindle))
				{
					if (CanWeave(actionID) && ActionReady(OriginalHook(EnkindleBahamut)))
					{
						return OriginalHook(EnkindleBahamut);
					}

					return SummonBahamut;
				}

				return actionID;
			}
		}
	}
}