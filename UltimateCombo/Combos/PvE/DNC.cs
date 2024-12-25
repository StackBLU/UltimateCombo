using Dalamud.Game.ClientState.JobGauge.Types;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.CustomCombo;
using UltimateCombo.Data;
using UltimateCombo.Services;

namespace UltimateCombo.Combos.PvE
{
	internal static class DNC
	{
		public const byte JobID = 38;

		public const uint
			Cascade = 15989,
			Fountain = 15990,
			ReverseCascade = 15991,
			Fountainfall = 15992,
			StarfallDance = 25792,
			Windmill = 15993,
			Bladeshower = 15994,
			RisingWindmill = 15995,
			Bloodshower = 15996,
			Tillana = 25790,
			StandardStep = 15997,
			TechnicalStep = 15998,
			StandardFinish0 = 16003,
			StandardFinish1 = 16191,
			StandardFinish2 = 16192,
			TechnicalFinish0 = 16004,
			TechnicalFinish1 = 16193,
			TechnicalFinish2 = 16194,
			TechnicalFinish3 = 16195,
			TechnicalFinish4 = 16196,
			ClosedPosition = 16006,
			FanDance1 = 16007,
			FanDance2 = 16008,
			FanDance3 = 16009,
			FanDance4 = 25791,
			Peloton = 7557,
			SaberDance = 16005,
			EnAvant = 16010,
			Devilment = 16011,
			ShieldSamba = 16012,
			Flourish = 16013,
			Improvisation = 16014,
			CuringWaltz = 16015,
			LastDance = 36983,
			FinishingMove = 36984,
			DanceOfTheDawn = 36985;

		public static class Buffs
		{
			public const ushort
				FlourishingCascade = 1814,
				FlourishingFountain = 1815,
				FlourishingWindmill = 1816,
				FlourishingShower = 1817,
				FlourishingFanDance = 2021,
				SilkenSymmetry = 2693,
				SilkenFlow = 2694,
				FlourishingFinish = 2698,
				FlourishingStarfall = 2700,
				FlourishingSymmetry = 3017,
				FlourishingFlow = 3018,
				StandardStep = 1818,
				TechnicalStep = 1819,
				StandardFinish = 1821,
				TechnicalFinish = 1822,
				ThreeFoldFanDance = 1820,
				FourFoldFanDance = 2699,
				Peloton = 1199,
				ShieldSamba = 1826,
				LastDanceReady = 3867,
				FinishingMoveReady = 3868,
				DanceOfTheDawnReady = 3869,
				Improvisation = 1827,
				Devilment = 1825,
				ClosedPosition = 1823;
		}

		public static class Steps
		{
			public const ushort
				Emboite = 15999,
				Entrechat = 16000,
				Jete = 16001,
				Pirouette = 16002;
		}

		private static DNCGauge Gauge
		{
			get
			{
				return CustomComboFunctions.GetJobGauge<DNCGauge>();
			}
		}

		public static class Config
		{

		}

		internal class DNC_ST_DPS : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DNC_ST_DPS;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Cascade or Fountain) && IsEnabled(CustomComboPreset.DNC_ST_DPS))
				{
					if (ActionReady(ClosedPosition) && !HasEffect(Buffs.ClosedPosition) && GetPartyMembers().Length > 0)
					{
						return ClosedPosition;
					}

					if (CanWeave(actionID) && !HasEffect(Buffs.TechnicalStep) && !HasEffect(Buffs.StandardStep)
						&& (ActionWatching.NumberOfGcdsUsed >= 10 || Service.Configuration.IgnoreGCDChecks))
					{
						if (IsEnabled(CustomComboPreset.DNC_ST_Devilment) && ActionReady(Devilment) && HasEffect(Buffs.TechnicalFinish))
						{
							return Devilment;
						}

						if (IsEnabled(CustomComboPreset.DNC_ST_Flourish) && ActionReady(Flourish) && !WasLastWeaponskill(StandardFinish2))
						{
							return Flourish;
						}

						if (IsEnabled(CustomComboPreset.DNC_ST_FanDance4) && HasEffect(Buffs.FourFoldFanDance))
						{
							return FanDance4;
						}

						if (IsEnabled(CustomComboPreset.DNC_ST_FanDance3) && HasEffect(Buffs.ThreeFoldFanDance))
						{
							return FanDance3;
						}

						if (IsEnabled(CustomComboPreset.DNC_ST_FanDance) && ActionReady(FanDance1)
							&& ((HasEffect(Buffs.TechnicalFinish) && Gauge.Feathers >= 1) || Gauge.Feathers == 4))
						{
							return FanDance1;
						}
					}

					if (HasEffect(Buffs.Improvisation))
					{
						return OriginalHook(Improvisation);
					}

					if (IsEnabled(CustomComboPreset.DNC_ST_Standard) && ActionReady(StandardStep) && !HasEffect(Buffs.TechnicalStep))
					{
						return OriginalHook(StandardStep);
					}

					if (HasEffect(Buffs.StandardStep))
					{
						if (Gauge.CompletedSteps == 2)
						{
							return StandardFinish2;
						}
						if (Gauge.NextStep is Steps.Emboite)
						{
							return Steps.Emboite;
						}
						if (Gauge.NextStep is Steps.Entrechat)
						{
							return Steps.Entrechat;
						}
						if (Gauge.NextStep is Steps.Jete)
						{
							return Steps.Jete;
						}
						if (Gauge.NextStep is Steps.Pirouette)
						{
							return Steps.Pirouette;
						}
					}

					if (IsEnabled(CustomComboPreset.DNC_ST_Technical) && ActionReady(TechnicalStep) && !HasEffect(Buffs.StandardStep))
					{
						return TechnicalStep;
					}

					if (HasEffect(Buffs.TechnicalStep))
					{
						if (Gauge.CompletedSteps == 4)
						{
							return TechnicalFinish4;
						}
						if (Gauge.NextStep is Steps.Emboite)
						{
							return Steps.Emboite;
						}
						if (Gauge.NextStep is Steps.Entrechat)
						{
							return Steps.Entrechat;
						}
						if (Gauge.NextStep is Steps.Jete)
						{
							return Steps.Jete;
						}
						if (Gauge.NextStep is Steps.Pirouette)
						{
							return Steps.Pirouette;
						}
					}

					if (IsEnabled(CustomComboPreset.DNC_ST_Technical) && HasEffect(Buffs.FlourishingFinish))
					{
						return Tillana;
					}

					if (IsEnabled(CustomComboPreset.DNC_ST_Technical) && HasEffect(Buffs.DanceOfTheDawnReady) && Gauge.Esprit >= 50)
					{
						return DanceOfTheDawn;
					}

					if (IsEnabled(CustomComboPreset.DNC_ST_Starfall) && HasEffect(Buffs.FlourishingStarfall))
					{
						return StarfallDance;
					}

					if (IsEnabled(CustomComboPreset.DNC_ST_LastDance) && HasEffect(Buffs.LastDanceReady))
					{
						return LastDance;
					}

					if (IsEnabled(CustomComboPreset.DNC_ST_Saber) && ActionReady(SaberDance)
						&& (Gauge.Esprit == 100 || (HasEffect(Buffs.TechnicalFinish) && Gauge.Esprit >= 50)
						|| (Gauge.Esprit > 50 && (IsOffCooldown(Flourish) || GetCooldownRemainingTime(Flourish) < 3))))
					{
						return SaberDance;
					}

					if (HasEffect(Buffs.SilkenSymmetry) || HasEffect(Buffs.FlourishingSymmetry))
					{
						return ReverseCascade;
					}

					if (HasEffect(Buffs.SilkenFlow) || HasEffect(Buffs.FlourishingFlow))
					{
						return Fountainfall;
					}

					if (comboTime > 0 && lastComboMove is Cascade && ActionReady(Fountain))
					{
						return Fountain;
					}

					return Cascade;
				}

				return actionID;
			}
		}

		internal class DNC_AoE_DPS : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DNC_AoE_DPS;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Windmill or Bladeshower) && IsEnabled(CustomComboPreset.DNC_AoE_DPS))
				{
					if (ActionReady(ClosedPosition) && !HasEffect(Buffs.ClosedPosition) && GetPartyMembers().Length > 0)
					{
						return ClosedPosition;
					}

					if (CanWeave(actionID) && !HasEffect(Buffs.TechnicalStep) && !HasEffect(Buffs.StandardStep))
					{
						if (IsEnabled(CustomComboPreset.DNC_AoE_Devilment) && ActionReady(Devilment) && HasEffect(Buffs.TechnicalFinish))
						{
							return Devilment;
						}

						if (IsEnabled(CustomComboPreset.DNC_AoE_Flourish) && ActionReady(Flourish) && !WasLastWeaponskill(StandardFinish2))
						{
							return Flourish;
						}

						if (IsEnabled(CustomComboPreset.DNC_AoE_FanDance4) && HasEffect(Buffs.FourFoldFanDance))
						{
							return FanDance4;
						}

						if (IsEnabled(CustomComboPreset.DNC_AoE_FanDance3) && HasEffect(Buffs.ThreeFoldFanDance))
						{
							return FanDance3;
						}

						if (IsEnabled(CustomComboPreset.DNC_AoE_FanDance2) && ActionReady(FanDance2)
							&& ((HasEffect(Buffs.TechnicalFinish) && Gauge.Feathers >= 1) || Gauge.Feathers == 4))
						{
							return FanDance2;
						}
					}

					if (HasEffect(Buffs.Improvisation))
					{
						return OriginalHook(Improvisation);
					}

					if (IsEnabled(CustomComboPreset.DNC_AoE_Standard) && ActionReady(StandardStep) && !HasEffect(Buffs.TechnicalStep))
					{
						return OriginalHook(StandardStep);
					}

					if (HasEffect(Buffs.StandardStep))
					{
						if (Gauge.CompletedSteps == 2)
						{
							return StandardFinish2;
						}
						if (Gauge.NextStep is Steps.Emboite)
						{
							return Steps.Emboite;
						}
						if (Gauge.NextStep is Steps.Entrechat)
						{
							return Steps.Entrechat;
						}
						if (Gauge.NextStep is Steps.Jete)
						{
							return Steps.Jete;
						}
						if (Gauge.NextStep is Steps.Pirouette)
						{
							return Steps.Pirouette;
						}
					}

					if (IsEnabled(CustomComboPreset.DNC_AoE_Technical) && ActionReady(TechnicalStep) && !HasEffect(Buffs.StandardStep))
					{
						return TechnicalStep;
					}

					if (HasEffect(Buffs.TechnicalStep))
					{
						if (Gauge.CompletedSteps == 4)
						{
							return TechnicalFinish4;
						}
						if (Gauge.NextStep is Steps.Emboite)
						{
							return Steps.Emboite;
						}
						if (Gauge.NextStep is Steps.Entrechat)
						{
							return Steps.Entrechat;
						}
						if (Gauge.NextStep is Steps.Jete)
						{
							return Steps.Jete;
						}
						if (Gauge.NextStep is Steps.Pirouette)
						{
							return Steps.Pirouette;
						}
					}

					if (IsEnabled(CustomComboPreset.DNC_AoE_Technical) && HasEffect(Buffs.FlourishingFinish))
					{
						return Tillana;
					}

					if (IsEnabled(CustomComboPreset.DNC_AoE_Technical) && HasEffect(Buffs.DanceOfTheDawnReady) && Gauge.Esprit >= 50)
					{
						return DanceOfTheDawn;
					}

					if (IsEnabled(CustomComboPreset.DNC_AoE_Starfall) && HasEffect(Buffs.FlourishingStarfall))
					{
						return StarfallDance;
					}

					if (IsEnabled(CustomComboPreset.DNC_AoE_LastDance) && HasEffect(Buffs.LastDanceReady))
					{
						return LastDance;
					}

					if (IsEnabled(CustomComboPreset.DNC_AoE_Saber) && ActionReady(SaberDance)
						&& (Gauge.Esprit == 100 || (HasEffect(Buffs.TechnicalFinish) && Gauge.Esprit >= 50)
						|| (Gauge.Esprit > 50 && (IsOffCooldown(Flourish) || GetCooldownRemainingTime(Flourish) < 3))))
					{
						return SaberDance;
					}

					if (HasEffect(Buffs.SilkenSymmetry) || HasEffect(Buffs.FlourishingSymmetry))
					{
						return RisingWindmill;
					}

					if (HasEffect(Buffs.SilkenFlow) || HasEffect(Buffs.FlourishingFlow))
					{
						return Bloodshower;
					}

					if (comboTime > 0 && lastComboMove is Windmill)
					{
						return Bladeshower;
					}

					return Windmill;
				}

				return actionID;
			}
		}
	}
}