using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvP
{
	internal static class SAMPvP
	{
		public const uint
			KashakCombo = 58,
			Yukikaze = 29523,
			Gekko = 29524,
			Kasha = 29525,
			Hyosetsu = 29526,
			Mangetsu = 29527,
			Oka = 29528,
			OgiNamikiri = 29530,
			Soten = 29532,
			Chiten = 29533,
			Mineuchi = 29535,
			MeikyoShisui = 29536,
			Midare = 29529,
			Kaeshi = 29531,
			Zantetsuken = 29537;

		public static class Buffs
		{
			public const ushort
				Kaiten = 3201,
				Midare = 3203;
		}

		public static class Debuffs
		{
			public const ushort
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
								&& (GetLimitBreakCurrentValue() <= GetLimitBreakMaxValue() * 0.75
								|| GetLimitBreakCurrentValue() == GetLimitBreakMaxValue()))
							{
								return Chiten;
							}

							if (IsEnabled(CustomComboPreset.SAMPvP_Soten) && ActionReady(Soten)
								&& !HasEffect(Buffs.Kaiten) && InActionRange(Soten))
							{
								return Soten;
							}

							if (IsEnabled(CustomComboPreset.SAMPvP_Meikyo) && ActionReady(MeikyoShisui))
							{
								return MeikyoShisui;
							}

							if (IsEnabled(CustomComboPreset.SAMPvP_Mineuchi) && ActionReady(Mineuchi)
								&& InActionRange(Mineuchi) && !TargetHasEffectAny(PvPCommon.Buffs.Resilience))
							{
								return Mineuchi;
							}
						}

						if (IsEnabled(CustomComboPreset.SAMPvP_Soten) && ActionReady(Soten) && !HasEffect(Buffs.Kaiten)
							&& (!InActionRange(OgiNamikiri) || !HasBattleTarget()))
						{
							return Soten;
						}

						if (!WasLastAction(Soten))
						{
							if (IsEnabled(CustomComboPreset.SAMPvP_Meikyo) && HasEffect(Buffs.Midare)
								&& InActionRange(Midare))
							{
								return Midare;
							}

							if (IsEnabled(CustomComboPreset.SAMPvP_Namikiri) && ActionReady(OriginalHook(OgiNamikiri))
								&& InActionRange(OgiNamikiri))
							{
								return OriginalHook(OgiNamikiri);
							}
						}
					}
				}

				return actionID;
			}
		}
	}
}