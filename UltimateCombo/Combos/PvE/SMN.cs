using Dalamud.Game.ClientState.JobGauge.Types;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.CustomCombo;

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
			EmeralRuin1 = 25810,
			EmeralRuin2 = 25813,
			EmeralRuin3 = 25819,
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
			public static UserInt
				SMN_ST_Lucid = new("SMN_ST_Lucid", 7500),
				SMN_AoE_Lucid = new("SMN_AoE_Lucid", 7500);
		}

		internal class SMN_ST_DPS : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SMN_ST_DPS;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Ruin or Ruin2 or Ruin3) && IsEnabled(CustomComboPreset.SMN_ST_DPS))
				{
					if (IsEnabled(CustomComboPreset.SMN_ST_Lucid) && ActionReady(All.LucidDreaming) && LocalPlayer.CurrentMp <= 1000)
					{
						return All.LucidDreaming;
					}

					if (IsEnabled(CustomComboPreset.SMN_Reminder) && !HasPetPresent())
					{
						return SummonCarbuncle;
					}

					if (CanWeave(actionID))
					{
						if (IsEnabled(CustomComboPreset.SMN_ST_SearingLight) && HasEffect(Buffs.RubysGlimmer))
						{
							return SearingFlash;
						}

						if (IsEnabled(CustomComboPreset.SMN_ST_SearingLight) && ActionReady(SearingLight))
						{
							return SearingLight;
						}

						if (IsEnabled(CustomComboPreset.SMN_ST_EnergyDrain) && ActionReady(EnergyDrain))
						{
							return EnergyDrain;
						}

						if (WasLastSpell(OriginalHook(SummonBahamut)) || WasLastSpell(AstralImpulse)
							|| WasLastSpell(FountainOfFire) || WasLastSpell(UmbralImpulse))
						{
							if (ActionReady(OriginalHook(AstralFlow)))
							{
								return OriginalHook(AstralFlow);
							}
						}

						if (WasLastSpell(OriginalHook(SummonTitan2)) || WasLastSpell(OriginalHook(TopazRite)))
						{
							if (HasEffect(Buffs.TitansFavor))
							{
								return OriginalHook(AstralFlow);
							}
						}

						if (IsEnabled(CustomComboPreset.SMN_ST_RadiantAegis)
							&& GetRemainingCharges(RadiantAegis) == GetMaxCharges(RadiantAegis))
						{
							return RadiantAegis;
						}
					}

					if (HasEffect(Buffs.GarudasFavor))
					{
						if (OriginalHook(Gemshine) is EmeralRuin1 or EmeralRuin2 or EmeralRuin3 or EmeraldRite)
						{
							return OriginalHook(Gemshine);
						}

						if (ActionReady(All.Swiftcast))
						{
							return All.Swiftcast;
						}
						return OriginalHook(AstralFlow);
					}

					if (WasLastSpell(OriginalHook(SummonTitan2)) || WasLastSpell(OriginalHook(TopazRite)))
					{
						if (OriginalHook(Gemshine) is TopazRuin1 or TopazRuin2 or TopazRuin3 or TopazRite)
						{
							return OriginalHook(Gemshine);
						}
					}

					if (lastComboMove is CrimsonCyclone)
					{
						return OriginalHook(AstralFlow);
					}

					if (HasEffect(Buffs.IfritsFavor))
					{
						if (OriginalHook(Gemshine) is RubyRuin1 or RubyRuin2 or RubyRuin3 or RubyRite)
						{
							return OriginalHook(Gemshine);
						}

						if (ActionReady(CrimsonCyclone))
						{
							return OriginalHook(AstralFlow);
						}
					}

					if (IsEnabled(CustomComboPreset.SMN_ST_Ruin4) && ActionReady(Ruin4)
						&& HasEffect(Buffs.FurtherRuin) && Gauge.SummonTimerRemaining == 0)
					{
						return Ruin4;
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
					if (IsEnabled(CustomComboPreset.SMN_AoE_Lucid) && ActionReady(All.LucidDreaming) && LocalPlayer.CurrentMp <= 1000)
					{
						return All.LucidDreaming;
					}

					if (IsEnabled(CustomComboPreset.SMN_Reminder) && !HasPetPresent())
					{
						return SummonCarbuncle;
					}

					if (IsEnabled(CustomComboPreset.SMN_AoE_EnergySiphon) && ActionReady(EnergyDrain) && CanWeave(actionID))
					{
						return EnergySiphon;
					}

					if (IsEnabled(CustomComboPreset.SMN_AoE_Painflare) && ActionReady(Painflare)
						&& CanWeave(actionID) && Gauge.HasAetherflowStacks)
					{
						return Painflare;
					}

					if (WasLastSpell(OriginalHook(SummonBahamut)) || WasLastSpell(AstralImpulse)
						|| WasLastSpell(FountainOfFire) || WasLastSpell(UmbralImpulse))
					{
						if (CanWeave(actionID) && IsOffCooldown(OriginalHook(AstralFlow)))
						{
							return OriginalHook(AstralFlow);
						}
					}

					if (HasEffect(Buffs.GarudasFavor))
					{
						if (OriginalHook(PreciousBrilliance) is EmeraldOutburst or EmeraldDisaster or EmeraldCatastrophe)
						{
							return OriginalHook(PreciousBrilliance);
						}

						if (ActionReady(All.Swiftcast))
						{
							return All.Swiftcast;
						}
						return OriginalHook(AstralFlow);
					}

					if (WasLastSpell(OriginalHook(SummonTitan2)) || WasLastSpell(OriginalHook(TopazRite))
						|| WasLastSpell(OriginalHook(TopazCatastrophe)))
					{
						if (CanWeave(actionID) && HasEffect(Buffs.TitansFavor))
						{
							return OriginalHook(AstralFlow);
						}

						if (OriginalHook(PreciousBrilliance) is TopazOutburst or TopazDisaster or TopazCatastrophe)
						{
							return OriginalHook(PreciousBrilliance);
						}
					}

					if (HasEffect(Buffs.IfritsFavor))
					{
						if (OriginalHook(PreciousBrilliance) is RubyOutburst or RubyDisaster or RubyCatastrophe)
						{
							return OriginalHook(PreciousBrilliance);
						}

						if (ActionReady(CrimsonCyclone))
						{
							return OriginalHook(AstralFlow);
						}
					}

					if (lastComboMove is CrimsonCyclone)
					{
						return OriginalHook(AstralFlow);
					}

					if (IsEnabled(CustomComboPreset.SMN_AoE_Ruin4) && ActionReady(Ruin4)
						&& HasEffect(Buffs.FurtherRuin) && Gauge.SummonTimerRemaining == 0)
					{
						return Ruin4;
					}

					if (ActionReady(OriginalHook(Tridisaster)))
					{
						return OriginalHook(Tridisaster);
					}
				}
				return actionID;
			}
		}

		internal class SMN_EnergyDrainNecro : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SMN_EnergyDrainNecro;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is EnergyDrain or Fester or Necrotize) && IsEnabled(CustomComboPreset.SMN_EnergyDrainNecro))
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

		internal class SMN_Raise : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SMN_Raise;
			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (actionID is Resurrection && IsEnabled(CustomComboPreset.SMN_Raise))
				{
					if (IsOffCooldown(All.Swiftcast))
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