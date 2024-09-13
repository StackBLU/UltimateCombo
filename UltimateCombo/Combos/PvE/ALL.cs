using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Combos.PvE.Content;
using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvE
{
	internal class All
	{
		public const byte JobID = 0;

		public const uint
			Rampart = 7531,
			SecondWind = 7541,
			TrueNorth = 7546,
			Addle = 7560,
			Swiftcast = 7561,
			LucidDreaming = 7562,
			Resurrection = 173,
			Raise = 125,
			Provoke = 7533,
			Shirk = 7537,
			Reprisal = 7535,
			Esuna = 7568,
			Rescue = 7571,
			SolidReason = 232,
			AgelessWords = 215,
			Sleep = 25880,
			WiseToTheWorldMIN = 26521,
			WiseToTheWorldBTN = 26522,
			LowBlow = 7540,
			Bloodbath = 7542,
			HeadGraze = 7551,
			FootGraze = 7553,
			LegGraze = 7554,
			Feint = 7549,
			Interject = 7538,
			Peloton = 7557,
			LegSweep = 7863,
			Repose = 16560,
			Sprint = 3;

		public static class Buffs
		{
			public const ushort
				Weakness = 43,
				Medicated = 49,
				Bloodbath = 84,
				Swiftcast = 167,
				Rampart = 1191,
				Peloton = 1199,
				LucidDreaming = 1204,
				TrueNorth = 1250,
				Sprint = 50,
				Raise1 = 148,
				Raise2 = 1140;
		}

		public static class Debuffs
		{
			public const ushort
				Sleep = 3,
				Bind = 13,
				Heavy = 14,
				Addle = 1203,
				Reprisal = 1193,
				Feint = 1195;
		}

		public static class Config
		{
			public static UserInt
				All_Variant_Cure = new("All_Variant_Cure", 50);
		}

		internal class All_Tank_Reprisal : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.All_Tank_Reprisal;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (actionID is Reprisal && IsEnabled(CustomComboPreset.All_Tank_Reprisal))
				{
					if (TargetHasEffectAny(Debuffs.Reprisal) && IsOffCooldown(Reprisal))
					{
						return OriginalHook(11);
					}
				}

				return actionID;
			}
		}

		internal class All_Caster_Addle : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.All_Caster_Addle;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (actionID is Addle && IsEnabled(CustomComboPreset.All_Caster_Addle))
				{
					if (TargetHasEffectAny(Debuffs.Addle) && IsOffCooldown(Addle))
					{
						return OriginalHook(11);
					}
				}

				return actionID;
			}
		}

		internal class All_Melee_Feint : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.All_Melee_Feint;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (actionID is Feint && IsEnabled(CustomComboPreset.All_Melee_Feint))
				{
					if (TargetHasEffectAny(Debuffs.Feint) && IsOffCooldown(Feint))
					{
						return OriginalHook(11);
					}
				}

				return actionID;
			}
		}

		internal class All_Melee_TrueNorth : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.All_Melee_TrueNorth;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (actionID is TrueNorth && IsEnabled(CustomComboPreset.All_Melee_TrueNorth))
				{
					if (HasEffect(Buffs.TrueNorth))
					{
						return OriginalHook(11);
					}
				}

				return actionID;
			}
		}

		internal class All_Ranged_Mitigation : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.All_Ranged_Mitigation;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is BRD.Troubadour or MCH.Tactician or DNC.ShieldSamba) && IsEnabled(CustomComboPreset.All_Ranged_Mitigation))
				{
					if ((HasEffectAny(BRD.Buffs.Troubadour) || HasEffectAny(MCH.Buffs.Tactician)
						|| HasEffectAny(DNC.Buffs.ShieldSamba)) && IsOffCooldown(actionID))
					{
						return OriginalHook(11);
					}
				}

				return actionID;
			}
		}

		internal class All_Raise_Protection : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.All_Raise_Protection;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is WHM.Raise or SCH.Resurrection or AST.Ascend or SGE.Egeiro
					or SMN.Resurrection or RDM.Verraise or BLU.AngelWhisper)
					&& IsEnabled(CustomComboPreset.All_Raise_Protection))
				{
					if (TargetHasEffectAny(Buffs.Raise1) || TargetHasEffectAny(Buffs.Raise2))
					{
						return OriginalHook(11);
					}
				}

				return actionID;
			}
		}

		internal class All_Variant : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.All_Variant;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.All_Variant))
				{
					if (IsEnabled(CustomComboPreset.All_Variant_Cure) && IsEnabled(Variant.VariantCure)
						&& PlayerHealthPercentageHp() <= GetOptionValue(Config.All_Variant_Cure))
					{
						return Variant.VariantCure;
					}

					if (CanWeave(actionID))
					{
						if (IsEnabled(CustomComboPreset.All_Variant_Ultimatum) && IsEnabled(Variant.VariantUltimatum)
						&& ActionReady(Variant.VariantUltimatum))
						{
							return Variant.VariantUltimatum;
						}

						if (IsEnabled(CustomComboPreset.All_Variant_SpiritDart) && IsEnabled(Variant.VariantSpiritDart)
							&& !TargetHasEffectAny(Variant.Debuffs.SustainedDamage))
						{
							return Variant.VariantSpiritDart;
						}

						if (IsEnabled(CustomComboPreset.All_Variant_Rampart) && IsEnabled(Variant.VariantRampart)
							&& ActionReady(Variant.VariantRampart))
						{
							return Variant.VariantRampart;
						}

					}
				}

				return actionID;
			}
		}
	}
}