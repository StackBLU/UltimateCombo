using Dalamud.Game.ClientState.JobGauge.Types;
using System.Collections.Generic;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.CustomCombo;
using UltimateCombo.Data;

namespace UltimateCombo.Combos.PvE
{
	internal static class WHM
	{
		public const byte JobID = 24;

		public const uint
			Cure1 = 120,
			Cure2 = 135,
			Cure3 = 131,
			Regen = 137,
			AfflatusSolace = 16531,
			AfflatusRapture = 16534,
			Raise = 125,
			Benediction = 140,
			AfflatusMisery = 16535,
			Medica1 = 124,
			Medica2 = 133,
			Medica3 = 37010,
			Tetragrammaton = 3570,
			DivineBenison = 7432,
			Aquaveil = 25861,
			DivineCaress = 37011,
			Glare1 = 16533,
			Glare3 = 25859,
			Glare4 = 37009,
			Stone1 = 119,
			Stone2 = 127,
			Stone3 = 3568,
			Stone4 = 7431,
			Assize = 3571,
			Holy = 139,
			Holy3 = 25860,
			Aero = 121,
			Aero2 = 132,
			Dia = 16532,
			ThinAir = 7430,
			PresenceOfMind = 136,
			PlenaryIndulgence = 7433;

		public static class Buffs
		{
			public const ushort
			Regen = 158,
			Medica2 = 150,
			Medica3 = 3880,
			PresenceOfMind = 157,
			ThinAir = 1217,
			DivineBenison = 1218,
			Aquaveil = 2708,
			SacredSight = 3879,
			DivineGrace = 3881;
		}

		public static class Debuffs
		{
			public const ushort
			Aero = 143,
			Aero2 = 144,
			Dia = 1871;
		}

		internal static readonly Dictionary<uint, ushort>
			DiaList = new() {
				{ Aero, Debuffs.Aero },
				{ Aero2, Debuffs.Aero2 },
				{ Dia, Debuffs.Dia }
			};

		private static WHMGauge Gauge
		{
			get
			{
				return CustomComboFunctions.GetJobGauge<WHMGauge>();
			}
		}

		public static class Config
		{
			internal static UserInt
				WHM_ST_DPS_Lucid = new("WHM_ST_DPS_Lucid", 7500),
				WHM_AoE_DPS_Lucid = new("WHM_AoE_DPS_Lucid", 7500);
			internal static UserBool
				WHM_ST_Heals_TetraWeave = new("WHM_ST_Heals_TetraWeave"),
				WHM_Raise_ThinAir = new("WHM_Raise_ThinAir");
		}

		internal class WHM_ST_DPS : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.WHM_ST_DPS;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Stone1 or Stone2 or Stone3 or Stone4 or Glare1 or Glare3) && IsEnabled(CustomComboPreset.WHM_ST_DPS))
				{
					if (IsEnabled(CustomComboPreset.WHM_ST_DPS_Lucid) && ActionReady(All.LucidDreaming) && LocalPlayer.CurrentMp <= 1000)
					{
						return All.LucidDreaming;
					}

					if (CanWeave(actionID) && ActionWatching.NumberOfGcdsUsed >= 2)
					{
						if (IsEnabled(CustomComboPreset.WHM_ST_DPS_Lucid) && ActionReady(All.LucidDreaming)
							&& LocalPlayer.CurrentMp <= GetOptionValue(Config.WHM_ST_DPS_Lucid))
						{
							return All.LucidDreaming;
						}

						if (IsEnabled(CustomComboPreset.WHM_ST_DPS_PresenceOfMind) && ActionReady(PresenceOfMind))
						{
							return PresenceOfMind;
						}

						if (IsEnabled(CustomComboPreset.WHM_ST_DPS_Assize) && ActionReady(Assize))
						{
							return Assize;
						}

						if (IsEnabled(CustomComboPreset.WHM_ST_DPS_Swiftcast) && ActionReady(All.Swiftcast) && IsMoving)
						{
							return All.Swiftcast;
						}
					}

					if (IsEnabled(CustomComboPreset.AST_ST_DPS_CombustUptime) && ActionReady(OriginalHook(Dia)) && ActionWatching.NumberOfGcdsUsed >= 3
						&& (EnemyHealthCurrentHp() >= LocalPlayer.MaxHp || EnemyHealthMaxHp() == 44)
						&& (!TargetHasEffect(DiaList[OriginalHook(Dia)]) || GetDebuffRemainingTime(DiaList[OriginalHook(Dia)]) <= 3))
					{
						return OriginalHook(Dia);
					}

					if (IsEnabled(CustomComboPreset.WHM_ST_DPS_Misery) && ActionReady(AfflatusMisery) && Gauge.BloodLily == 3)
					{
						if ((IsEnabled(CustomComboPreset.WHM_ST_DPS_Misery_Save)
							&& Gauge.Lily == 2 && Gauge.LilyTimer >= 14500) || Gauge.Lily == 3)
						{
							return AfflatusMisery;
						}
						if (!IsEnabled(CustomComboPreset.WHM_ST_DPS_Misery_Save))
						{
							return AfflatusMisery;
						}
					}

					if (IsEnabled(CustomComboPreset.WHM_ST_DPS_LilyOvercap) && ActionReady(AfflatusRapture)
						&& ((Gauge.Lily == 2 && Gauge.LilyTimer >= 17000) || Gauge.Lily == 3))
					{
						return AfflatusRapture;
					}

					if (IsEnabled(CustomComboPreset.WHM_ST_DPS_PresenceOfMind) && GetBuffStacks(Buffs.SacredSight) > 0 && ActionReady(Glare4))
					{
						return Glare4;
					}
				}
				return actionID;
			}
		}

		internal class WHM_AoE_DPS : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.WHM_AoE_DPS;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Holy or Holy3) && IsEnabled(CustomComboPreset.WHM_AoE_DPS))
				{
					if (IsEnabled(CustomComboPreset.WHM_AoE_DPS_Lucid) && ActionReady(All.LucidDreaming) && LocalPlayer.CurrentMp <= 1000)
					{
						return All.LucidDreaming;
					}

					if (CanWeave(actionID))
					{
						if (IsEnabled(CustomComboPreset.WHM_AoE_DPS_Lucid) && ActionReady(All.LucidDreaming)
							&& LocalPlayer.CurrentMp <= GetOptionValue(Config.WHM_AoE_DPS_Lucid) && CanWeave(actionID))
						{
							return All.LucidDreaming;
						}

						if (IsEnabled(CustomComboPreset.WHM_AoE_DPS_PresenceOfMind) && ActionReady(PresenceOfMind))
						{
							return PresenceOfMind;
						}

						if (IsEnabled(CustomComboPreset.WHM_AoE_DPS_Swiftcast) && ActionReady(All.Swiftcast) && IsMoving)
						{
							return All.Swiftcast;
						}
					}

					if (IsEnabled(CustomComboPreset.WHM_AoE_DPS_Misery) && ActionReady(AfflatusMisery) && Gauge.BloodLily == 3)
					{
						if ((IsEnabled(CustomComboPreset.WHM_AoE_DPS_Misery_Save)
							&& Gauge.Lily == 2 && Gauge.LilyTimer >= 14500) || Gauge.Lily == 3)
						{
							return AfflatusMisery;
						}
						if (!IsEnabled(CustomComboPreset.WHM_AoE_DPS_Misery_Save))
						{
							return AfflatusMisery;
						}
					}

					if (IsEnabled(CustomComboPreset.WHM_AoE_DPS_LilyOvercap) && ActionReady(AfflatusRapture)
						&& ((Gauge.Lily == 2 && Gauge.LilyTimer >= 17000) || Gauge.Lily == 3))
					{
						return AfflatusRapture;
					}

					if (IsEnabled(CustomComboPreset.WHM_AoE_DPS_PresenceOfMind) && GetBuffStacks(Buffs.SacredSight) > 0 && ActionReady(Glare4))
					{
						return Glare4;
					}
				}
				return actionID;
			}
		}

		internal class WHM_ST_Heals : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.WHM_ST_Heals;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Cure1 or Cure2) && IsEnabled(CustomComboPreset.WHM_ST_Heals))
				{
					if (IsEnabled(CustomComboPreset.WHM_ST_Heals_Tetragrammaton) && ActionReady(Tetragrammaton) && CanWeave(actionID))
					{
						return Tetragrammaton;
					}

					if (IsEnabled(CustomComboPreset.WHM_ST_Heals_Misery) && ActionReady(AfflatusMisery) && Gauge.BloodLily == 3)
					{
						return AfflatusMisery;
					}

					if (IsEnabled(CustomComboPreset.WHM_ST_Heals_Solace) && ActionReady(AfflatusSolace) && Gauge.Lily > 0)
					{
						return AfflatusSolace;
					}

					if (!TargetHasEffect(Buffs.Regen) && ActionReady(Regen) && !HasEffect(Buffs.Regen))
					{
						return Regen;
					}

					if (IsEnabled(CustomComboPreset.WHM_ST_Heals_ThinAir) && ActionReady(ThinAir) && !HasEffect(Buffs.ThinAir) && CanWeave(actionID))
					{
						return ThinAir;
					}

					if (ActionReady(Cure2))
					{
						return Cure2;
					}

					return Cure1;
				}

				return actionID;
			}
		}

		internal class WHM_AoE_Heals : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.WHM_AoE_Heals;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Medica1 or Medica2 or Medica3) && IsEnabled(CustomComboPreset.WHM_AoE_Heals))
				{
					if (IsEnabled(CustomComboPreset.WHM_AoE_Heals_Plenary) & ActionReady(PlenaryIndulgence))
					{
						return PlenaryIndulgence;
					}

					if (IsEnabled(CustomComboPreset.WHM_AoE_Heals_Misery) && ActionReady(AfflatusMisery) && Gauge.BloodLily == 3)
					{
						return AfflatusMisery;
					}

					if (IsEnabled(CustomComboPreset.WHM_AoE_Heals_Rapture) && ActionReady(AfflatusRapture) && Gauge.Lily > 0)
					{
						return AfflatusRapture;
					}

					if (IsEnabled(CustomComboPreset.WHM_AoE_Heals_ThinAir) && ActionReady(ThinAir) && !HasEffect(Buffs.ThinAir) && CanWeave(actionID))
					{
						return ThinAir;
					}

					if (IsEnabled(CustomComboPreset.WHM_AoEHeals_Medica2) && !HasEffect(Buffs.Medica2) && !HasEffect(Buffs.Medica3)
						&& (ActionReady(Medica2) || ActionReady(Medica3)))
					{
						return OriginalHook(Medica3);
					}

					if (IsEnabled(CustomComboPreset.WHM_AoE_Heals_Cure3) && ActionReady(Cure3))
					{
						return Cure3;
					}

					return Medica1;
				}

				return actionID;
			}
		}

		internal class WHM_Raise : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.WHM_Raise;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (actionID is Raise && IsEnabled(CustomComboPreset.WHM_Raise))
				{
					if (IsOffCooldown(All.Swiftcast))
					{
						return All.Swiftcast;
					}

					if (ActionReady(ThinAir) && Config.WHM_Raise_ThinAir && !HasEffect(Buffs.ThinAir))
					{
						return ThinAir;
					}

					return Raise;
				}

				return actionID;
			}
		}
	}
}