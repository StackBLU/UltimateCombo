using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvP
{
	internal static class WHMPvP
	{
		public const uint
			Glare = 29223,
			Cure2 = 29224,
			Cure3 = 29225,
			AfflatusMisery = 29226,
			Aquaveil = 29227,
			MiracleOfNature = 29228,
			SeraphStrike = 29229;

		internal class Buffs
		{
			internal const ushort
				Cure3Ready = 3083;
		}

		public static class Config
		{
			public static UserInt
				WHMPvP_Polymorph = new("WHMPvP_Polymorph", 30),
				WHMPvP_Cure3 = new("WHMPvP_Cure3", 40);
		}

		internal class WHMPvP_Combo : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.WHMPvP_Combo;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (actionID is Glare && IsEnabled(CustomComboPreset.WHMPvP_Combo))
				{
					if (!TargetHasEffectAny(PvPCommon.Buffs.Guard))
					{
						if (CanWeave(actionID))
						{
							if (IsEnabled(CustomComboPreset.WHMPvP_Polymorph) && ActionReady(MiracleOfNature)
								&& GetTargetHPPercent() < GetOptionValue(Config.WHMPvP_Polymorph))
							{
								return MiracleOfNature;
							}

							if (IsEnabled(CustomComboPreset.WHMPvP_Aquaveil) && ActionReady(Aquaveil))
							{
								return Aquaveil;
							}

							if (IsEnabled(CustomComboPreset.WHMPvP_Seraph) && ActionReady(SeraphStrike) && InMeleeRange())
							{
								return SeraphStrike;
							}
						}

						if (IsEnabled(CustomComboPreset.WHMPvP_Misery) && ActionReady(AfflatusMisery))
						{
							return AfflatusMisery;
						}
					}

					if (IsEnabled(CustomComboPreset.WHMPvP_Cure3) && HasEffect(Buffs.Cure3Ready) && !WasLastAction(SeraphStrike)
						&& (PlayerHealthPercentageHp() < GetOptionValue(Config.WHMPvP_Cure3) || GetBuffRemainingTime(Buffs.Cure3Ready) < 3
						|| (GetTargetHPPercent() < GetOptionValue(Config.WHMPvP_Cure3) && HasFriendlyTarget())))
					{
						return Cure3;
					}
				}

				return actionID;
			}
		}
	}
}