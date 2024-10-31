using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvP
{
	internal static class NINPvP
	{
		internal const uint
			SpinningEdge = 29500,
			GustSlash = 29501,
			AeolianEdge = 29502,
			FumaShuriken = 29505,
			Mug = 29509,
			Kassatsu = 29507,
			Bunshin = 29511,
			Shukuchi = 29513,
			SeitonTenchu = 29515,
			ForkedRaiju = 29510,
			FleetingRaiju = 29707,
			HyoshoRanryu = 29506,
			GokaMekkyaku = 29504,
			Meisui = 29508,
			Huton = 29512,
			Doton = 29514,
			Assassinate = 29503;

		internal class Buffs
		{
			internal const ushort
				Kassatsu = 1317,
				Hidden = 1316,
				Bunshin = 2010,
				ShadeShift = 2011,
				Huton = 3186,
				Meisui = 3189,
				FleetingRaijuReady = 3211,
				UnsealedSeitonTenchu = 3192;
		}

		internal class Debuffs
		{
			internal const ushort
				SealedHyoshoRanryu = 3194,
				GokaMekkyaku = 3184,
				SealedGokaMekkyaku = 3193,
				SealedHuton = 3196,
				SealedDoton = 3197,
				SealedForkedRaiju = 3195,
				SealedMeisui = 3198;
		}

		public static class Config
		{
			public static UserInt
				NINPvP_Huton = new("NINPvP_Huton", 100),
				NINPvP_Meisui = new("NINPvP_Meisui", 25);
		}

		internal class NINPvP_Combo : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.NINPvP_Combo;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is SpinningEdge or GustSlash or AeolianEdge) && IsEnabled(CustomComboPreset.NINPvP_Combo))
				{
					if (IsEnabled(CustomComboPreset.NINPvP_SeitonTenchu) && HasTarget()
						&& (GetLimitBreakCurrentValue() == GetLimitBreakMaxValue() || HasEffect(Buffs.UnsealedSeitonTenchu))
						&& (GetTargetHPPercent() <= 49
						|| (HasEffect(Buffs.UnsealedSeitonTenchu) && GetBuffRemainingTime(Buffs.UnsealedSeitonTenchu) <= 1)))
					{
						return OriginalHook(SeitonTenchu);
					}

					if (!TargetHasEffectAny(PvPCommon.Buffs.Guard) && (!HasEffect(Buffs.Hidden) || !WasLastAbility(Shukuchi)))
					{
						if (CanWeave(actionID))
						{
							if (IsEnabled(CustomComboPreset.NINPvP_Mug) && ActionReady(Mug) && GetRemainingCharges(FumaShuriken) <= 1
								&& InActionRange(Mug))
							{
								return Mug;
							}

							if (IsEnabled(CustomComboPreset.NINPvP_Bunshin) && ActionReady(Bunshin))
							{
								return Bunshin;
							}
						}

						if (IsEnabled(CustomComboPreset.NINPvP_Kassatsu) && ActionReady(Kassatsu) && !HasEffect(Buffs.Kassatsu)
							&& (CanWeave(actionID) || !InMeleeRange()))
						{
							return Kassatsu;
						}

						if (HasEffect(Buffs.Kassatsu))
						{
							if (IsEnabled(CustomComboPreset.NINPvP_Huton) && !HasEffect(Debuffs.SealedHuton)
								&& PlayerHealthPercentageHp() >= GetOptionValue(Config.NINPvP_Huton)
								&& !HasEffect(Buffs.Huton))
							{
								return Huton;
							}

							if (IsEnabled(CustomComboPreset.NINPvP_Meisui) && !HasEffect(Debuffs.SealedMeisui)
								&& PlayerHealthPercentageHp() <= GetOptionValue(Config.NINPvP_Huton)
								&& !HasEffect(Buffs.Meisui))
							{
								return Meisui;
							}

							if (IsEnabled(CustomComboPreset.NINPvP_Goka) && !HasEffect(Debuffs.SealedGokaMekkyaku)
								&& !TargetHasEffect(Debuffs.GokaMekkyaku))
							{
								return GokaMekkyaku;
							}

							if (IsEnabled(CustomComboPreset.NINPvP_Hyosho) && !HasEffect(Debuffs.SealedHyoshoRanryu)
								&& GetTargetHPPercent() > 60)
							{
								return HyoshoRanryu;
							}

							if (IsEnabled(CustomComboPreset.NINPvP_Raiju) && !HasEffect(Debuffs.SealedForkedRaiju)
								&& !TargetHasEffectAny(PvPCommon.Buffs.Resilience))
							{
								return ForkedRaiju;
							}

							if (IsEnabled(CustomComboPreset.NINPvP_Doton) && !HasEffect(Debuffs.SealedDoton))
							{
								return Doton;
							}
						}

						if (IsEnabled(CustomComboPreset.NINPvP_Raiju) && HasEffect(Buffs.FleetingRaijuReady))
						{
							return FleetingRaiju;
						}

						if (IsEnabled(CustomComboPreset.NINPvP_Shuriken) && ActionReady(FumaShuriken)
							&& (!HasEffect(Buffs.Hidden) || (HasEffect(Buffs.Hidden) && !InMeleeRange()))
							&& (!InMeleeRange() || GetRemainingCharges(FumaShuriken) == GetMaxCharges(FumaShuriken)))
						{
							return FumaShuriken;
						}
					}
				}

				return actionID;
			}
		}
	}
}