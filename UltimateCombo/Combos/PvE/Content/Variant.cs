using ECommons.DalamudServices;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvE.Content
{
	internal static class Variant
	{
		public const uint
			VariantUltimatum = 29730,
			VariantRaise = 29731,
			VariantRaise2 = 29734,

			//Don't use these for logic - they are just for the ReplaceSkill icon
			VariantCure_Image = 29729,
			VariantSpiritDart_Image = 29732,
			VariantRampart_Image = 29733;

		//1069 = The Sil'dihn Subterrane
		//1137 = Mount Rokkon
		//1176 = Aloalo Island

		public static uint VariantCure
		{
			get
			{
				return Svc.ClientState.TerritoryType switch
				{
					1069 => 29729,
					1137 or 1176 => 33862,
					_ => 0
				};
			}
		}

		public static uint VariantSpiritDart
		{
			get
			{
				return Svc.ClientState.TerritoryType switch
				{
					1069 => 29732,
					1137 or 1176 => 33863,
					_ => 0
				};
			}
		}

		public static uint VariantRampart
		{
			get
			{
				return Svc.ClientState.TerritoryType switch
				{
					1069 => 29733,
					1137 or 1176 => 33864,
					_ => 0
				};
			}
		}

		public static class Buffs
		{
			public const ushort
				EmnityUp = 3358,
				VulnDown = 3360,
				Rehabilitation = 3367,
				DamageBarrier = 3405;
		}

		public static class Debuffs
		{
			public const ushort
				SustainedDamage = 3359;
		}
		public static class Config
		{
			public static UserInt
				Variant_Cure = new("All_Variant_Cure", 50);
		}

		internal class Variant_Cure : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.Variant_Cure;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.Variant_Cure))
				{
					if (ActionReady(VariantCure) && IsEnabled(VariantCure)
						&& PlayerHealthPercentageHp() <= GetOptionValue(Config.Variant_Cure)
						&& !HasEffect(NIN.Buffs.Mudra) && !HasEffect(NIN.Buffs.TenChiJin) && HasEffect(NIN.Buffs.Kassatsu))
					{
						return VariantCure;
					}
				}

				return actionID;
			}
		}

		internal class Variant_Raise : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.Variant_Raise;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.Variant_Raise))
				{
					if (IsEnabled(VariantCure)
						&& (actionID is WHM.Raise or SCH.Resurrection or AST.Ascend
							or SGE.Egeiro or SMN.Resurrection or RDM.Verraise))
					{
						if (ActionReady(All.Swiftcast))
						{
							return All.Swiftcast;
						}

						if (ActionReady(VariantRaise2) && IsEnabled(VariantRaise2))
						{
							return VariantRaise2;
						}

						if (ActionReady(VariantRaise) && IsEnabled(VariantRaise))
						{
							return VariantRaise;
						}
					}
				}

				return actionID;
			}
		}

		internal class Variant_Ultimatum : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.Variant_Ultimatum;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.Variant_Ultimatum))
				{
					if (ActionReady(VariantUltimatum) && IsEnabled(VariantUltimatum) && CanWeave(actionID)
						&& !HasEffect(NIN.Buffs.Mudra) && !HasEffect(NIN.Buffs.TenChiJin) && HasEffect(NIN.Buffs.Kassatsu))
					{
						return VariantUltimatum;
					}
				}

				return actionID;
			}
		}

		internal class Variant_SpiritDart : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.Variant_SpiritDart;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.Variant_SpiritDart))
				{
					if (ActionReady(VariantSpiritDart) && IsEnabled(VariantSpiritDart)
						&& CanWeave(actionID) && !TargetHasEffectAny(Debuffs.SustainedDamage))
					{
						return VariantSpiritDart;
					}
				}

				return actionID;
			}
		}

		internal class Variant_Rampart : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.Variant_Rampart;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.Variant_Rampart))
				{
					if (ActionReady(VariantRampart) && IsEnabled(VariantRampart) && CanWeave(actionID)
						&& !HasEffect(NIN.Buffs.Mudra) && !HasEffect(NIN.Buffs.TenChiJin) && HasEffect(NIN.Buffs.Kassatsu))
					{
						return VariantRampart;
					}
				}

				return actionID;
			}
		}
	}
}