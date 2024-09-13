using Dalamud.Game.ClientState.JobGauge.Types;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Combos.PvE.Content;
using UltimateCombo.CustomCombo;
using UltimateCombo.Data;

namespace UltimateCombo.Combos.PvE
{
	internal static class GNB
	{
		public const byte JobID = 37;

		public static int MaxCartridges(byte level)
		{
			return level >= 88 ? 3 : 2;
		}

		public const uint
			KeenEdge = 16137,
			NoMercy = 16138,
			BrutalShell = 16139,
			DemonSlice = 16141,
			SolidBarrel = 16145,
			GnashingFang = 16146,
			SavageClaw = 16147,
			DemonSlaughter = 16149,
			WickedTalon = 16150,
			Superbolide = 16152,
			SonicBreak = 16153,
			Continuation = 16155,
			JugularRip = 16156,
			AbdomenTear = 16157,
			EyeGouge = 16158,
			BowShock = 16159,
			HeartOfLight = 16160,
			BurstStrike = 16162,
			FatedCircle = 16163,
			Aurora = 16151,
			DoubleDown = 25760,
			DangerZone = 16144,
			BlastingZone = 16165,
			Bloodfest = 16164,
			Hypervelocity = 25759,
			LionHeart = 36939,
			NobleBlood = 36938,
			ReignOfBeasts = 36937,
			FatedBrand = 36936,
			LightningShot = 16143;

		public static class Buffs
		{
			public const ushort
				NoMercy = 1831,
				Aurora = 1835,
				ReadyToRip = 1842,
				ReadyToTear = 1843,
				ReadyToGouge = 1844,
				ReadyToRaze = 3839,
				ReadyToBreak = 3886,
				ReadyToReign = 3840,
				ReadyToBlast = 2686;
		}

		public static class Debuffs
		{
			public const ushort
				BowShock = 1838,
				SonicBreak = 1837;
		}

		private static GNBGauge Gauge
		{
			get
			{
				return CustomComboFunctions.GetJobGauge<GNBGauge>();
			}
		}

		public static class Config
		{
			public static UserInt
				GNB_ST_Invuln = new("GNB_ST_Invuln", 10),
				GNB_AoE_Invuln = new("GNB_AoE_Invuln", 10),
				GNB_Variant_Cure = new("GNB_Variant_Cure", 50);
		}

		internal class GNB_ST_DPS : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.GNB_ST_DPS;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is KeenEdge or BrutalShell or SolidBarrel) && IsEnabled(CustomComboPreset.GNB_ST_DPS))
				{
					if (IsEnabled(CustomComboPreset.GNB_ST_Invuln) && PlayerHealthPercentageHp() <= GetOptionValue(Config.GNB_ST_Invuln) && ActionReady(Superbolide))
					{
						return Superbolide;
					}

					if (CanWeave(actionID) && ActionWatching.NumberOfGcdsUsed >= 1)
					{
						if (IsEnabled(CustomComboPreset.GNB_Variant_SpiritDart) && IsEnabled(Variant.VariantSpiritDart)
							&& ActionReady(Variant.VariantSpiritDart) && !TargetHasEffectAny(Variant.Debuffs.SustainedDamage))
						{
							return Variant.VariantSpiritDart;
						}

						if (IsEnabled(CustomComboPreset.GNB_Variant_Ultimatum) && IsEnabled(Variant.VariantUltimatum)
							&& ActionReady(Variant.VariantUltimatum))
						{
							return Variant.VariantUltimatum;
						}

						if (IsEnabled(CustomComboPreset.GNB_ST_Bloodfest) && ActionReady(Bloodfest) && Gauge.Ammo == 0)
						{
							return Bloodfest;
						}

						if (IsEnabled(CustomComboPreset.GNB_ST_NoMercy) && ActionReady(NoMercy))
						{
							return NoMercy;
						}

						if (IsEnabled(CustomComboPreset.GNB_ST_BowShock) && ActionReady(BowShock))
						{
							return BowShock;
						}

						if (IsEnabled(CustomComboPreset.GNB_ST_BlastingZone) && ActionReady(OriginalHook(BlastingZone)))
						{
							return OriginalHook(BlastingZone);
						}

						if (IsEnabled(CustomComboPreset.GNB_ST_AutoAurora) && ActionReady(Aurora) && PlayerHealthPercentageHp() < 100
							&& !HasEffect(Buffs.Aurora))
						{
							return Aurora;
						}
					}

					if (IsEnabled(CustomComboPreset.GNB_Variant_Cure) && IsEnabled(Variant.VariantCure)
						&& PlayerHealthPercentageHp() <= GetOptionValue(Config.GNB_Variant_Cure))
					{
						return Variant.VariantCure;
					}

					if (ActionReady(Continuation)
						&& (HasEffect(Buffs.ReadyToRip) || HasEffect(Buffs.ReadyToTear)
						|| HasEffect(Buffs.ReadyToGouge) || HasEffect(Buffs.ReadyToBlast)))
					{
						return OriginalHook(Continuation);
					}

					if (IsEnabled(CustomComboPreset.GNB_ST_Gnashing) && ActionReady(OriginalHook(GnashingFang)) && Gauge.Ammo > 0
						&& Gauge.AmmoComboStep == 0)
					{
						return OriginalHook(GnashingFang);
					}

					if (IsEnabled(CustomComboPreset.GNB_ST_DoubleDown) && ActionReady(DoubleDown) && Gauge.Ammo >= 2
						&& HasEffect(Buffs.NoMercy))
					{
						return DoubleDown;
					}

					if (IsEnabled(CustomComboPreset.GNB_ST_NoMercy) && ActionReady(SonicBreak) && HasEffect(Buffs.ReadyToBreak))
					{
						return SonicBreak;
					}

					if (Gauge.AmmoComboStep is 1 or 2)
					{
						return OriginalHook(GnashingFang);
					}

					if (IsEnabled(CustomComboPreset.GNB_ST_Bloodfest) && ActionReady(ReignOfBeasts) && HasEffect(Buffs.NoMercy)
						&& (HasEffect(Buffs.ReadyToReign) || WasLastWeaponskill(ReignOfBeasts) || WasLastWeaponskill(NobleBlood)))
					{
						return OriginalHook(ReignOfBeasts);
					}

					if (IsEnabled(CustomComboPreset.GNB_ST_Burst) && ActionReady(BurstStrike)
						&& ((Gauge.Ammo == MaxCartridges(level) && lastComboMove == BrutalShell)
						|| (HasEffect(Buffs.NoMercy) && Gauge.Ammo > 0 && GetCooldownRemainingTime(OriginalHook(GnashingFang)) > 5
						&& GetCooldownRemainingTime(DoubleDown) > 10)))
					{
						return BurstStrike;
					}

					if (comboTime > 0)
					{
						if (lastComboMove is KeenEdge && ActionReady(BrutalShell))
						{
							return BrutalShell;
						}

						if (lastComboMove is BrutalShell && ActionReady(SolidBarrel))
						{
							return SolidBarrel;
						}
					}
					return KeenEdge;
				}
				return actionID;
			}
		}

		internal class GNB_AoE_DPS : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.GNB_AoE_DPS;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is DemonSlice or DemonSlaughter) && IsEnabled(CustomComboPreset.GNB_AoE_DPS))
				{
					if (IsEnabled(CustomComboPreset.GNB_AoE_Invuln) && PlayerHealthPercentageHp() <= GetOptionValue(Config.GNB_AoE_Invuln)
						&& ActionReady(Superbolide))
					{
						return Superbolide;
					}

					if (CanWeave(actionID))
					{
						if (IsEnabled(CustomComboPreset.GNB_Variant_SpiritDart) && IsEnabled(Variant.VariantSpiritDart)
							&& ActionReady(Variant.VariantSpiritDart) && !TargetHasEffectAny(Variant.Debuffs.SustainedDamage))
						{
							return Variant.VariantSpiritDart;
						}

						if (IsEnabled(CustomComboPreset.GNB_Variant_Ultimatum) && IsEnabled(Variant.VariantUltimatum)
							&& ActionReady(Variant.VariantUltimatum))
						{
							return Variant.VariantUltimatum;
						}

						if (IsEnabled(CustomComboPreset.GNB_AoE_Bloodfest) && ActionReady(Bloodfest) && Gauge.Ammo == 0)
						{
							return Bloodfest;
						}

						if (IsEnabled(CustomComboPreset.GNB_AoE_NoMercy) && ActionReady(NoMercy))
						{
							return NoMercy;
						}

						if (IsEnabled(CustomComboPreset.GNB_AoE_AutoAurora) && PlayerHealthPercentageHp() < 100 && ActionReady(Aurora)
							&& !HasEffect(Buffs.Aurora) && CanWeave(actionID))
						{
							return Aurora;
						}
					}

					if (IsEnabled(CustomComboPreset.GNB_Variant_Cure) && IsEnabled(Variant.VariantCure)
						&& PlayerHealthPercentageHp() <= GetOptionValue(Config.GNB_Variant_Cure))
					{
						return Variant.VariantCure;
					}

					if (ActionReady(Continuation) && HasEffect(Buffs.ReadyToRaze))
					{
						return OriginalHook(Continuation);
					}

					if (IsEnabled(CustomComboPreset.GNB_AoE_DoubleDown) && ActionReady(DoubleDown) && Gauge.Ammo >= 2)
					{
						return DoubleDown;
					}

					if (IsEnabled(CustomComboPreset.GNB_AoE_Bloodfest) && ActionReady(ReignOfBeasts)
						&& (HasEffect(Buffs.ReadyToReign) || WasLastWeaponskill(ReignOfBeasts) || WasLastWeaponskill(NobleBlood)))
					{
						return OriginalHook(ReignOfBeasts);
					}

					if (IsEnabled(CustomComboPreset.GNB_AoE_Fated) && ActionReady(FatedCircle)
						&& ((Gauge.Ammo == MaxCartridges(level) && lastComboMove is DemonSlice)
						|| (HasEffect(Buffs.NoMercy) && Gauge.Ammo > 0 && GetCooldownRemainingTime(DoubleDown) > 10)))
					{
						return FatedCircle;
					}

					if (comboTime > 0 && lastComboMove is DemonSlice && ActionReady(DemonSlaughter))
					{
						return DemonSlaughter;
					}
				}
				return actionID;
			}
		}

		internal class GNB_AuroraProtection : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.GNB_AuroraProtection;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (actionID is Aurora)
				{
					if ((HasFriendlyTarget() && TargetHasEffectAny(Buffs.Aurora)) || (!HasFriendlyTarget() && HasEffectAny(Buffs.Aurora)))
					{
						return OriginalHook(11);
					}
				}
				return actionID;
			}
		}
	}
}