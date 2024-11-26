using Dalamud.Game.ClientState.JobGauge.Types;
using System.Collections.Generic;
using System.Linq;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.CustomCombo;
using UltimateCombo.Data;
using UltimateCombo.Services;

namespace UltimateCombo.Combos.PvE
{
	internal static class SGE
	{
		internal const byte JobID = 40;

		internal const uint
			Diagnosis = 24284,
			Prognosis = 24286,
			Physis = 24288,
			Druochole = 24296,
			Kerachole = 24298,
			Ixochole = 24299,
			Pepsis = 24301,
			Physis2 = 24302,
			Taurochole = 24303,
			Haima = 24305,
			Panhaima = 24311,
			Holos = 24310,
			EukrasianDiagnosis = 24291,
			EukrasianPrognosis = 24292,
			Egeiro = 24287,

			Dosis = 24283,
			Dosis2 = 24306,
			Dosis3 = 24312,
			EukrasianDosis = 24293,
			EukrasianDosis2 = 24308,
			EukrasianDosis3 = 24314,
			Phlegma = 24289,
			Phlegma2 = 24307,
			Phlegma3 = 24313,
			Dyskrasia = 24297,
			Dyskrasia2 = 24315,
			Toxikon = 24304,
			Pneuma = 24318,
			EukrasianDyskrasia = 37032,
			Psyche = 37033,
			EukrasianPrognosis2 = 37034,

			Soteria = 24294,
			Zoe = 24300,
			Krasis = 24317,
			Philosophia = 37035,

			Kardia = 24285,
			Eukrasia = 24290,
			Rhizomata = 24309;

		internal static class Buffs
		{
			internal const ushort
				Kardia = 2604,
				Kardion = 2605,
				Eukrasia = 2606,
				EukrasianDiagnosis = 2607,
				EukrasianPrognosis = 2609,
				Panhaima = 2613,
				Kerachole = 2618,
				Eudaimonia = 3899;
		}

		internal static class Debuffs
		{
			internal const ushort
				EukrasianDosis = 2614,
				EukrasianDosis2 = 2615,
				EukrasianDosis3 = 2616,
				EukrasianDyskrasia = 3897;
		}

		internal static readonly Dictionary<uint, ushort>
			DosisList = new()
			{
				{ Dosis,  Debuffs.EukrasianDosis  },
				{ Dosis2, Debuffs.EukrasianDosis2 },
				{ Dosis3, Debuffs.EukrasianDosis3 }
			};

		public static SGEGauge Gauge
		{
			get
			{
				return CustomComboFunctions.GetJobGauge<SGEGauge>();
			}
		}

		public static class Config
		{
			public static UserInt
				SGE_ST_DPS_Rhizo = new("SGE_ST_DPS_Rhizo", 0),
				SGE_ST_DPS_AddersgallProtect = new("SGE_ST_DPS_AddersgallProtect", 3),
				SGE_AoE_DPS_Rhizo = new("SGE_AoE_DPS_Rhizo", 0),
				SGE_AoE_DPS_AddersgallProtect = new("SGE_AoE_DPS_AddersgallProtect", 3);
		}

		internal class SGE_ST_DPS : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SGE_ST_DPS;
			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Dosis or Dosis2 or Dosis3) && IsEnabled(CustomComboPreset.SGE_ST_DPS))
				{
					if ((WasLastSpell(EukrasianPrognosis) || WasLastSpell(EukrasianPrognosis2)) && !InCombat())
					{
						ActionWatching.CombatActions.Clear();
					}

					if (CanWeave(actionID) && (ActionWatching.NumberOfGcdsUsed >= 2 || Service.Configuration.IgnoreGCDChecks)
						&& !WasLastSpell(Eukrasia) && !WasLastSpell(EukrasianPrognosis) && !WasLastSpell(EukrasianPrognosis2)
						&& !WasLastSpell(EukrasianDosis) && !WasLastSpell(EukrasianDosis2) && !WasLastSpell(EukrasianDosis2)
						&& !WasLastSpell(EukrasianDyskrasia))
					{
						if (IsEnabled(CustomComboPreset.SGE_ST_DPS_Kardia) && ActionReady(Kardia) && InCombat() && !TargetsTargetHasEffect(Buffs.Kardion)
							&& (GetPartyMembers().Any(x => x.GameObject == CurrentTarget.TargetObject) || GetPartyMembers().Length == 0)
							&& ((HasBattleTarget() && TargetIsBoss()) || !HasEffect(Buffs.Kardia)))
						{
							return Kardia;
						}

						if (IsEnabled(CustomComboPreset.SGE_ST_DPS_Psyche) && ActionReady(Psyche)
							&& (ActionWatching.NumberOfGcdsUsed >= 6 || Service.Configuration.IgnoreGCDChecks))
						{
							return Psyche;
						}

						if (IsEnabled(CustomComboPreset.SGE_ST_DPS_Rhizo) && ActionReady(Rhizomata) && Gauge.Addersgall <= GetOptionValue(Config.SGE_ST_DPS_Rhizo))
						{
							return Rhizomata;
						}

						if (IsEnabled(CustomComboPreset.SGE_ST_DPS_Soteria) && ActionReady(Soteria))
						{
							return Soteria;
						}

						if (IsEnabled(CustomComboPreset.SGE_ST_DPS_AddersgallProtect) && ActionReady(Druochole)
							&& Gauge.Addersgall >= GetOptionValue(Config.SGE_ST_DPS_AddersgallProtect))
						{
							return Druochole;
						}

						if (IsEnabled(CustomComboPreset.SGE_ST_DPS_Swiftcast) && ActionReady(All.Swiftcast) && IsMoving
							&& Gauge.Addersgall == 0)
						{
							return All.Swiftcast;
						}
					}

					if (IsEnabled(CustomComboPreset.SGE_ST_DPS_EDosis) && ActionReady(OriginalHook(EukrasianDosis))
						&& (ActionWatching.NumberOfGcdsUsed >= 3 || Service.Configuration.IgnoreGCDChecks)
						&& (EnemyHealthCurrentHp() >= LocalPlayer.MaxHp || EnemyHealthMaxHp() == 44)
						&& (!TargetHasEffect(DosisList[OriginalHook(Dosis)]) || GetDebuffRemainingTime(DosisList[OriginalHook(Dosis)]) <= 3
						|| ActionWatching.NumberOfGcdsUsed == 11))
					{
						if (!HasEffect(Buffs.Eukrasia) && ActionReady(Eukrasia))
						{
							return Eukrasia;
						}

						return OriginalHook(Dosis);
					}

					if (IsEnabled(CustomComboPreset.SGE_ST_DPS_Phlegma)
						&& InActionRange(OriginalHook(Phlegma)) && ActionReady(OriginalHook(Phlegma))
						&& (ActionReady(Psyche) || !LevelChecked(Psyche)
						|| WasLastSpell(OriginalHook(Phlegma)) || GetCooldownRemainingTime(Psyche) > 50)
						&& (ActionWatching.NumberOfGcdsUsed >= 5 || Service.Configuration.IgnoreGCDChecks))
					{
						return OriginalHook(Phlegma);
					}

					if (IsEnabled(CustomComboPreset.SGE_ST_DPS_Toxikon) && IsMoving && ActionReady(Toxikon) && Gauge.Addersting > 0
						&& !HasEffect(All.Buffs.Swiftcast))
					{
						return OriginalHook(Toxikon);
					}

					return OriginalHook(Dosis);
				}

				return actionID;
			}
		}

		internal class SGE_AoE_DPS : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SGE_AoE_DPS;
			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Dyskrasia or Dyskrasia2) && IsEnabled(CustomComboPreset.SGE_AoE_DPS))
				{
					if (CanWeave(actionID)
						&& !WasLastSpell(Eukrasia) && !WasLastSpell(EukrasianPrognosis) && !WasLastSpell(EukrasianPrognosis2)
						&& !WasLastSpell(EukrasianDosis) && !WasLastSpell(EukrasianDosis2) && !WasLastSpell(EukrasianDosis2)
						&& !WasLastSpell(EukrasianDyskrasia))
					{
						if (IsEnabled(CustomComboPreset.SGE_AoE_DPS_Kardia) && ActionReady(Kardia) && InCombat()
							&& !TargetsTargetHasEffect(Buffs.Kardion)
							&& ((HasBattleTarget() && TargetIsBoss()) || !HasEffect(Buffs.Kardia)))
						{
							return Kardia;
						}

						if (IsEnabled(CustomComboPreset.SGE_AoE_DPS_Psyche) && ActionReady(Psyche) && CanWeave(actionID))
						{
							return Psyche;
						}

						if (IsEnabled(CustomComboPreset.SGE_AoE_DPS_Rhizo) && ActionReady(Rhizomata)
							&& Gauge.Addersgall <= GetOptionValue(Config.SGE_AoE_DPS_Rhizo))
						{
							return Rhizomata;
						}

						if (IsEnabled(CustomComboPreset.SGE_AoE_DPS_Soteria) && ActionReady(Soteria))
						{
							return Soteria;
						}

						if (IsEnabled(CustomComboPreset.SGE_AoE_DPS_AddersgallProtect) && ActionReady(Druochole)
							&& Gauge.Addersgall >= GetOptionValue(Config.SGE_AoE_DPS_AddersgallProtect))
						{
							return Druochole;
						}
					}

					if (IsEnabled(CustomComboPreset.SGE_AoE_DPS_EDyskrasia) && ActionReady(OriginalHook(EukrasianDyskrasia))
						&& (!TargetHasEffect(Debuffs.EukrasianDyskrasia) || (GetDebuffRemainingTime(Debuffs.EukrasianDyskrasia) <= 3)))
					{
						if (!HasEffect(Buffs.Eukrasia) && ActionReady(Eukrasia))
						{
							return Eukrasia;
						}

						return OriginalHook(EukrasianDyskrasia);
					}

					if (IsEnabled(CustomComboPreset.SGE_AoE_DPS_Phlegma)
						&& InActionRange(OriginalHook(Phlegma)) && ActionReady(OriginalHook(Phlegma))
						&& (ActionReady(Psyche) || !LevelChecked(Psyche)
						|| WasLastSpell(OriginalHook(Phlegma)) || GetCooldownRemainingTime(Psyche) > 50))
					{
						return OriginalHook(Phlegma);
					}

					return OriginalHook(Dyskrasia);
				}

				return actionID;
			}
		}

		internal class SGE_ST_Heals : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SGE_ST_Heals;
			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Diagnosis or EukrasianDiagnosis) && IsEnabled(CustomComboPreset.SGE_ST_Heals))
				{
					if (!HasEffect(Buffs.Eukrasia))
					{
						if (IsEnabled(CustomComboPreset.SGE_ST_Heals_Krasis) && ActionReady(Krasis))
						{
							return Krasis;
						}

						if (IsEnabled(CustomComboPreset.SGE_ST_Heals_Haima) && ActionReady(Haima))
						{
							return Haima;
						}
					}
				}

				return actionID;
			}
		}

		internal class SGE_AoE_Heals : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SGE_AoE_Heals;
			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Prognosis or EukrasianPrognosis or EukrasianPrognosis2) && IsEnabled(CustomComboPreset.SGE_AoE_Heals))
				{
					if (IsEnabled(CustomComboPreset.SGE_AoE_Heals_Pepsis) && ActionReady(Pepsis)
						&& HasEffect(Buffs.EukrasianPrognosis) && !WasLastAction(EukrasianPrognosis) && !WasLastAction(EukrasianPrognosis2))
					{
						return Pepsis;
					}
				}

				return actionID;
			}
		}
	}
}