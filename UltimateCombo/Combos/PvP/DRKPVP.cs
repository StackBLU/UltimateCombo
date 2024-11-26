using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvP
{
	internal static class DRKPvP
	{
		public const uint
			HardSlash = 29085,
			SyphonStrike = 29086,
			Souleater = 29087,

			Shadowbringer = 29091,
			ScarletDelirium = 41434,
			Comeuppance = 41435,
			Torcleaver = 41436,

			Impalement = 41438,
			Plunge = 29092,
			TheBlackestNight = 29093,
			SaltedEarth = 29094,
			SaltAndDarkness = 29095,

			Eventide = 29097,
			Disesteem = 41437;

		public static class Buffs
		{
			public const ushort
				Blackblood = 3033,
				ComeuppanceReady = 4288,
				TorcleaverReady = 4289,

				BlackestNight = 1308,
				DarkArts = 3034,

				SaltedEarth = 3036,
				SaltedEarthDefense = 3037,

				UndeadRedemption = 3039,
				Scorn = 4290;
		}

		public static class Debuffs
		{
			public const ushort
				SoleSurvivor = 1306,
				SaltsBane = 3038;
		}

		public static class Config
		{
			public static UserInt
				DRKPvP_Shadowbringer = new("DRKPvP_Shadowbringer", 75),
				DRKPvP_Impalement = new("DRKPvP_Impalement", 40);
		}

		internal class DRKPvP_Combo : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DRKPvP_Combo;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is HardSlash or SyphonStrike or Souleater or ScarletDelirium or Comeuppance or Torcleaver)
					&& IsEnabled(CustomComboPreset.DRKPvP_Combo))
				{
					if (IsEnabled(CustomComboPreset.DRKPvP_Eventide) && GetLimitBreakCurrentValue() == GetLimitBreakMaxValue())
					{
						if (PlayerHealthPercentageHp() < 90 && LocalPlayer.CurrentMp >= 2500)
						{
							return PvPCommon.Recuperate;
						}

						return Eventide;
					}

					if (!TargetHasEffectAny(PvPCommon.Buffs.Guard))
					{
						if (CanWeave(actionID))
						{
							if (IsEnabled(CustomComboPreset.DRKPvP_BlackestNight) && ActionReady(TheBlackestNight)
								&& !HasEffect(Buffs.BlackestNight) && !HasEffect(Buffs.DarkArts))
							{
								return TheBlackestNight;
							}

							if (IsEnabled(CustomComboPreset.DRKPvP_Shadowbringer) && ActionReady(Shadowbringer)
								&& PlayerHealthPercentageHp() > GetOptionValue(Config.DRKPvP_Shadowbringer))
							{
								return Shadowbringer;
							}
						}

						if (IsEnabled(CustomComboPreset.DRKPvP_SaltAndDarkness) && IsEnabled(SaltAndDarkness)
							&& WasLastAbility(SaltedEarth))
						{
							return SaltAndDarkness;
						}

						if (IsEnabled(CustomComboPreset.DRKPvP_SaltedEarth) && ActionReady(SaltedEarth)
							&& WasLastAbility(Plunge))
						{
							return SaltedEarth;
						}

						if (IsEnabled(CustomComboPreset.DRKPvP_Plunge) && ActionReady(Plunge))
						{
							return Plunge;
						}

						if (IsEnabled(CustomComboPreset.DRKPvP_Disesteem) && HasEffect(Buffs.Scorn))
						{
							return Disesteem;
						}

						if (IsEnabled(CustomComboPreset.DRKPvP_Impalement) && ActionReady(Impalement)
							&& PlayerHealthPercentageHp() > GetOptionValue(Config.DRKPvP_Impalement)
							&& InActionRange(Impalement))
						{
							return Impalement;
						}
					}
				}

				return actionID;
			}
		}
	}
}