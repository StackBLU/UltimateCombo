using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvP
{
	internal static class SAMPvP
	{
		public const uint
			Yukikaze = 29523,
			Gekko = 29524,
			Kasha = 29525,

			OgiNamikiri = 29530,
			KaeshiNamikiri = 29531,

			Mineuchi = 29535,

			Soten = 29532,
			Hyosetsu = 29526,
			Mangetsu = 29527,
			Oka = 29528,

			Chiten = 29533,
			Zanshin = 41577,

			MeikyoShisui = 29536,
			TendoSetsugekka = 41454,
			TendoKaeshiSetsugekka = 41455,

			Zantetsuken = 29537;

		public static class Buffs
		{
			public const ushort
				Kaiten = 3201,

				Chiten = 1240,
				ZanshinReady = 1318,

				MeikyoShisui = 1320,
				TendoSetsugekkaReady = 3203,

				OgiNamikiri = 3199,
				KaeshiNamikiri = 3200;
		}

		internal class Debuffs
		{
			internal const ushort
				Debana = 4306,
				Kuzushi = 3202;
		}

		internal class SAMPvP_Combo : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SAMPvP_Combo;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Yukikaze or Gekko or Kasha or Hyosetsu or Mangetsu or Oka)
					&& IsEnabled(CustomComboPreset.SAMPvP_Combo))
				{
					if (IsEnabled(CustomComboPreset.SAMPvP_Zantetsuken) && GetLimitBreakCurrentValue() == GetLimitBreakMaxValue()
						&& TargetHasEffect(Debuffs.Kuzushi))
					{
						return Zantetsuken;
					}

					if (!TargetHasEffectAny(PvPCommon.Buffs.Guard))
					{
						if (CanWeave(actionID))
						{
							if (IsEnabled(CustomComboPreset.SAMPvP_Chiten) && ActionReady(Chiten)
								&& (GetLimitBreakCurrentValue() <= GetLimitBreakMaxValue() * 0.75 || GetLimitBreakCurrentValue() == GetLimitBreakMaxValue()))
							{
								return Chiten;
							}

							if (IsEnabled(CustomComboPreset.SAMPvP_Soten) && ActionReady(Soten)
								&& !HasEffect(Buffs.Kaiten) && InActionRange(Soten))
							{
								return Soten;
							}

							if (IsEnabled(CustomComboPreset.SAMPvP_MeikyoShisui) && ActionReady(MeikyoShisui))
							{
								return MeikyoShisui;
							}

							if (IsEnabled(CustomComboPreset.SAMPvP_Mineuchi) && ActionReady(Mineuchi)
								&& InActionRange(Mineuchi) && !TargetHasEffectAny(PvPCommon.Buffs.Resilience))
							{
								return Mineuchi;
							}
						}

						if (IsEnabled(CustomComboPreset.SAMPvP_Soten) && ActionReady(Soten) && !HasEffect(Buffs.Kaiten) && !InActionRange(OgiNamikiri))
						{
							return Soten;
						}

						if (IsEnabled(CustomComboPreset.SAMPvP_MeikyoShisui)
							&& (HasEffect(Buffs.TendoSetsugekkaReady) || lastComboMove is TendoSetsugekka) && InActionRange(TendoSetsugekka))
						{
							return OriginalHook(TendoSetsugekka);
						}

						if (IsEnabled(CustomComboPreset.SAMPvP_Namikiri)
							&& (ActionReady(OgiNamikiri) || lastComboMove is OgiNamikiri) && InActionRange(OgiNamikiri))
						{
							return OriginalHook(OgiNamikiri);
						}
					}
				}

				return actionID;
			}
		}
	}
}