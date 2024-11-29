using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvP
{
	internal static class WHMPvP
	{
		public const uint
			Glare3 = 29223,
			Cure2 = 29224,

			AfflatusMisery = 29226,
			Aquaveil = 29227,
			MiracleOfNature = 29228,

			SeraphStrike = 29229,
			Cure3 = 29225,
			Glare4 = 41499,

			AfflatusPurgation = 29230;

		public static class Buffs
		{
			public const ushort
				Aquaveil = 3086,

				Protect = 1415,
				SacredSight = 4326,
				Cure3Ready = 3083,

				Temperance = 2037,
				TemperanceRegen = 2038;
		}

		internal class Debuffs
		{
			internal const ushort
				MiracleOfNature = 3085;
		}

		internal class WHMPvP_Combo : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.WHMPvP_Combo;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (actionID is Glare3 && IsEnabled(CustomComboPreset.WHMPvP_Combo))
				{
					if (!TargetHasEffectAny(PvPCommon.Buffs.Guard))
					{
						if (CanWeave(actionID))
						{
							if (IsEnabled(CustomComboPreset.WHMPvP_Aquaveil) && ActionReady(Aquaveil))
							{
								return Aquaveil;
							}
						}

						if (IsEnabled(CustomComboPreset.WHMPvP_AfflatusMisery) && ActionReady(AfflatusMisery))
						{
							return AfflatusMisery;
						}
					}

					if (IsEnabled(CustomComboPreset.WHMPvP_Cure3) && HasEffect(Buffs.Cure3Ready) && !WasLastAction(SeraphStrike)
						&& (LocalPlayer.CurrentHp <= LocalPlayer.MaxHp - 16000 || GetBuffRemainingTime(Buffs.Cure3Ready) < 5
						|| HasFriendlyTarget()))
					{
						return Cure3;
					}

					if (HasEffect(Buffs.SacredSight))
					{
						return Glare4;
					}
				}

				return actionID;
			}
		}
	}
}