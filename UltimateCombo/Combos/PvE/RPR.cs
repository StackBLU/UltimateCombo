using Dalamud.Game.ClientState.JobGauge.Types;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.CustomCombo;
using UltimateCombo.Data;
using UltimateCombo.Services;

namespace UltimateCombo.Combos.PvE
{
	internal class RPR
	{
		public const byte JobID = 39;

		public const uint
			Slice = 24373,
			WaxingSlice = 24374,
			InfernalSlice = 24375,
			ShadowOfDeath = 24378,
			SoulSlice = 24380,
			SpinningScythe = 24376,
			NightmareScythe = 24377,
			WhorlOfDeath = 24379,
			SoulScythe = 24381,
			Gibbet = 24382,
			Gallows = 24383,
			Guillotine = 24384,
			UnveiledGibbet = 24390,
			UnveiledGallows = 24391,
			ExecutionersGibbet = 36970,
			ExecutionersGallows = 36971,
			ExecutionersGuillotine = 36972,

			BloodStalk = 24389,
			GrimSwathe = 24392,
			Gluttony = 24393,
			ArcaneCircle = 24405,
			PlentifulHarvest = 24385,
			Enshroud = 24394,
			Communio = 24398,
			LemuresSlice = 24399,
			LemuresScythe = 24400,
			VoidReaping = 24395,
			CrossReaping = 24396,
			GrimReaping = 24397,
			Sacrificium = 36969,
			Perfectio = 36973,
			HellsIngress = 24401,
			HellsEgress = 24402,
			Regress = 24403,
			ArcaneCrest = 24404,
			Harpe = 24386,
			Soulsow = 24387,
			HarvestMoon = 24388;

		public static class Buffs
		{
			public const ushort
				SoulReaver = 2587,
				ImmortalSacrifice = 2592,
				ArcaneCircle = 2599,
				EnhancedGibbet = 2588,
				EnhancedGallows = 2589,
				EnhancedVoidReaping = 2590,
				EnhancedCrossReaping = 2591,
				EnhancedHarpe = 2845,
				Enshrouded = 2593,
				Soulsow = 2594,
				Threshold = 2595,
				BloodsownCircle = 2972,
				IdealHost = 3905,
				Oblatio = 3857,
				Executioner = 3858,
				PerfectioParata = 3860;
		}

		public static class Debuffs
		{
			public const ushort
				DeathsDesign = 2586;
		}

		private static RPRGauge Gauge
		{
			get
			{
				return CustomComboFunctions.GetJobGauge<RPRGauge>();
			}
		}

		public static class Config
		{

		}

		internal class RPR_ST_DPS : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.RPR_ST_DPS;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Slice or WaxingSlice or InfernalSlice) && IsEnabled(CustomComboPreset.RPR_ST_DPS))
				{
					if (!InCombat() && !HasEffect(Buffs.Soulsow) && ActionReady(Soulsow) && IsEnabled(CustomComboPreset.RPR_ST_Soulsow))
					{
						return Soulsow;
					}

					if (!InCombat() && ActionReady(Harpe) && !InMeleeRange())
					{
						return Harpe;
					}

					if (CanWeave(actionID) && (ActionWatching.NumberOfGcdsUsed >= 3 || Service.Configuration.IgnoreGCDChecks))
					{
						if (IsEnabled(CustomComboPreset.RPR_ST_Shield) && ActionReady(ArcaneCrest))
						{
							return ArcaneCrest;
						}

						if (IsEnabled(CustomComboPreset.RPR_ST_Enshroud) && Gauge.VoidShroud == 2)
						{
							return LemuresSlice;
						}

						if (IsEnabled(CustomComboPreset.RPR_ST_Enshroud) && HasEffect(Buffs.Oblatio))
						{
							return Sacrificium;
						}

						if (IsEnabled(CustomComboPreset.RPR_ST_Arcane) && ActionReady(ArcaneCircle))
						{
							return ArcaneCircle;
						}

						if (IsEnabled(CustomComboPreset.RPR_ST_Enshroud) && ActionReady(Enshroud)
							&& (Gauge.Shroud >= 50 || HasEffect(Buffs.IdealHost))
							&& !HasEffect(Buffs.Enshrouded) && !HasEffect(Buffs.ImmortalSacrifice))
						{
							return Enshroud;
						}

						if (IsEnabled(CustomComboPreset.RPR_ST_Gluttony) && ActionReady(Gluttony) && Gauge.Soul >= 50
							&& !HasEffect(Buffs.SoulReaver) && !HasEffect(Buffs.Executioner) && !HasEffect(Buffs.Enshrouded))
						{
							return Gluttony;
						}

						if (IsEnabled(CustomComboPreset.RPR_ST_BloodStalk) && ActionReady(BloodStalk) && Gauge.Soul >= 50
							&& !HasEffect(Buffs.SoulReaver) && !HasEffect(Buffs.Executioner) && !HasEffect(Buffs.Enshrouded))
						{
							return BloodStalk;
						}
					}

					if (IsEnabled(CustomComboPreset.RPR_ST_ShadowOfDeath) && ActionReady(ShadowOfDeath)
						&& !HasEffect(Buffs.SoulReaver) && !HasEffect(Buffs.Executioner) && EnemyHealthCurrentHp() > LocalPlayer.MaxHp
						&& (GetDebuffRemainingTime(Debuffs.DeathsDesign) < 3
						|| (GetCooldownRemainingTime(ArcaneCircle) < 10 && GetDebuffRemainingTime(Debuffs.DeathsDesign) < 30)))
					{
						return ShadowOfDeath;
					}

					if (IsEnabled(CustomComboPreset.RPR_ST_Soulsow) && HasEffect(Buffs.Soulsow)
						&& (ActionWatching.NumberOfGcdsUsed > 2 || Service.Configuration.IgnoreGCDChecks))
					{
						return HarvestMoon;
					}

					if (IsEnabled(CustomComboPreset.RPR_ST_PlentifulHarvest) && !HasEffect(Buffs.BloodsownCircle)
						&& HasEffect(Buffs.ImmortalSacrifice)
						&& !HasEffect(Buffs.SoulReaver) && !HasEffect(Buffs.Executioner))
					{
						return PlentifulHarvest;
					}

					if (IsEnabled(CustomComboPreset.RPR_ST_Enshroud) && Gauge.LemureShroud > 1)
					{
						if (WasLastWeaponskill(OriginalHook(VoidReaping)))
						{
							return OriginalHook(CrossReaping);
						}
						return OriginalHook(VoidReaping);
					}

					if (IsEnabled(CustomComboPreset.RPR_ST_Enshroud) && ActionReady(Communio) && Gauge.LemureShroud == 1)
					{
						return Communio;
					}

					if (IsEnabled(CustomComboPreset.RPR_ST_PlentifulHarvest) && HasEffect(Buffs.PerfectioParata))
					{
						return Perfectio;
					}

					if (IsEnabled(CustomComboPreset.RPR_ST_GibbetGallows) && (HasEffect(Buffs.SoulReaver) || HasEffect(Buffs.Executioner)))
					{
						if (HasEffect(Buffs.EnhancedGibbet))
						{
							return OriginalHook(Gibbet);
						}

						if (HasEffect(Buffs.EnhancedGallows))
						{
							return OriginalHook(Gallows);
						}

						return OriginalHook(Gallows);
					}

					if (IsEnabled(CustomComboPreset.RPR_ST_SoulSlice) && ActionReady(SoulSlice) && Gauge.Soul <= 40)
					{
						return SoulSlice;
					}

					if (comboTime > 0)
					{
						if (lastComboMove is WaxingSlice && ActionReady(InfernalSlice))
						{
							return InfernalSlice;
						}

						if (lastComboMove is Slice && ActionReady(WaxingSlice))
						{
							return WaxingSlice;
						}
					}

					return Slice;
				}

				return actionID;
			}
		}

		internal class RPR_AoE_DPS : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.RPR_AoE_DPS;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is SpinningScythe or NightmareScythe) && IsEnabled(CustomComboPreset.RPR_AoE_DPS))
				{
					if (IsEnabled(CustomComboPreset.RPR_AoE_Soulsow) && !InCombat() && !HasEffect(Buffs.Soulsow) && ActionReady(Soulsow))
					{
						return Soulsow;
					}

					if (CanWeave(actionID) && InCombat())
					{
						if (IsEnabled(CustomComboPreset.RPR_AoE_Shield) && ActionReady(ArcaneCrest))
						{
							return ArcaneCrest;
						}

						if (IsEnabled(CustomComboPreset.RPR_AoE_Enshroud) && Gauge.VoidShroud == 2)
						{
							return LemuresScythe;
						}

						if (IsEnabled(CustomComboPreset.RPR_AoE_Enshroud) && HasEffect(Buffs.Oblatio))
						{
							return Sacrificium;
						}

						if (IsEnabled(CustomComboPreset.RPR_AoE_Arcane) && ActionReady(ArcaneCircle))
						{
							return ArcaneCircle;
						}

						if (IsEnabled(CustomComboPreset.RPR_AoE_Enshroud) && ActionReady(Enshroud)
							&& (Gauge.Shroud >= 50 || HasEffect(Buffs.IdealHost))
							&& !HasEffect(Buffs.Enshrouded) && !HasEffect(Buffs.ImmortalSacrifice))
						{
							return Enshroud;
						}

						if (IsEnabled(CustomComboPreset.RPR_AoE_Gluttony) && ActionReady(Gluttony) && Gauge.Soul >= 50
							&& !HasEffect(Buffs.SoulReaver) && !HasEffect(Buffs.Executioner) && !HasEffect(Buffs.Enshrouded))
						{
							return Gluttony;
						}

						if (IsEnabled(CustomComboPreset.RPR_AoE_GrimSwathe) && ActionReady(GrimSwathe) && Gauge.Soul >= 50
							&& !HasEffect(Buffs.SoulReaver) && !HasEffect(Buffs.Executioner) && !HasEffect(Buffs.Enshrouded))
						{
							return GrimSwathe;
						}
					}

					if (IsEnabled(CustomComboPreset.RPR_AoE_WhorlOfDeath) && ActionReady(WhorlOfDeath)
						&& !HasEffect(Buffs.SoulReaver) && !HasEffect(Buffs.Executioner) && EnemyHealthCurrentHp() > LocalPlayer.MaxHp
						&& (GetDebuffRemainingTime(Debuffs.DeathsDesign) < 15
						|| (GetCooldownRemainingTime(ArcaneCircle) < 10 && GetDebuffRemainingTime(Debuffs.DeathsDesign) < 30)))
					{
						return WhorlOfDeath;
					}

					if (IsEnabled(CustomComboPreset.RPR_AoE_Soulsow) && HasEffect(Buffs.Soulsow))
					{
						return HarvestMoon;
					}

					if (IsEnabled(CustomComboPreset.RPR_AoE_PlentifulHarvest) && !HasEffect(Buffs.BloodsownCircle)
						&& HasEffect(Buffs.ImmortalSacrifice)
						&& !HasEffect(Buffs.SoulReaver) && !HasEffect(Buffs.Executioner))
					{
						return PlentifulHarvest;
					}

					if (IsEnabled(CustomComboPreset.RPR_AoE_Enshroud) && Gauge.LemureShroud > 1)
					{
						return OriginalHook(GrimReaping);
					}

					if (IsEnabled(CustomComboPreset.RPR_AoE_Enshroud) && ActionReady(Communio) && Gauge.LemureShroud == 1)
					{
						return Communio;
					}

					if (IsEnabled(CustomComboPreset.RPR_AoE_PlentifulHarvest) && HasEffect(Buffs.PerfectioParata))
					{
						return Perfectio;
					}

					if (IsEnabled(CustomComboPreset.RPR_AoE_Guillotine) && ActionReady(Guillotine)
						&& (HasEffect(Buffs.SoulReaver) || HasEffect(Buffs.Executioner)))
					{
						return OriginalHook(Guillotine);
					}

					if (IsEnabled(CustomComboPreset.RPR_AoE_SoulScythe) && ActionReady(SoulScythe) && Gauge.Soul <= 40)
					{
						return SoulScythe;
					}

					if (comboTime > 0)
					{
						if (lastComboMove is SpinningScythe)
						{
							return NightmareScythe;
						}
					}

					return SpinningScythe;
				}

				return actionID;
			}
		}

		internal class RPR_Regress : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.RPR_Regress;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is HellsIngress or HellsEgress) && IsEnabled(CustomComboPreset.RPR_Regress))
				{
					if (HasEffect(Buffs.Threshold))
					{
						return Regress;
					}
				}
				return actionID;
			}
		}
	}
}