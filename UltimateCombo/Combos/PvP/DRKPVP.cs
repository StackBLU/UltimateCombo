using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvP
{
	internal class DRKPvP
	{
		public const byte JobID = 32;

		public const uint
			HardSlash = 29085,
			SyphonStrike = 29086,
			Souleater = 29087,
			Quietus = 29737,
			Shadowbringer = 29091,
			Plunge = 29092,
			BlackestNight = 29093,
			SaltedEarth = 29094,
			Bloodspiller = 29088,
			SaltAndDarkness = 29095,
			Eventide = 29097;

		public class Buffs
		{
			public const ushort
				Blackblood = 3033,
				BlackestNight = 1038,
				SaltedEarthDMG = 3036,
				SaltedEarthDEF = 3037,
				DarkArts = 3034,
				UndeadRedemption = 3039;
		}

		public class Config
		{
			public static UserInt
				DRKPvP_ShadowbringerHP = new("DRKPvP_ShadowbringerHP", 50),
				DRKPvP_ShadowbringerMP = new("DRKPvP_ShadowbringerMP", 5000),
				DRKPvP_Quietus = new("DRKPvP_Quietus", 75);
		}

		internal class DRKPvP_Combo : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DRKPvP_Combo;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (actionID is HardSlash && IsEnabled(CustomComboPreset.DRKPvP_Combo))
				{
					if (IsEnabled(CustomComboPreset.DRKPvP_Eventide) && GetLimitBreakCurrentValue() == GetLimitBreakMaxValue())
					{
						if (PlayerHealthPercentageHp() < 100 && LocalPlayer.CurrentMp >= 2500)
						{
							return PvPCommon.Recuperate;
						}

						if (!InMeleeRange() && ActionReady(Plunge))
						{
							return Plunge;
						}

						if ((PlayerHealthPercentageHp() == 100 && (InMeleeRange() || WasLastAbility(Plunge)))
							|| (PlayerHealthPercentageHp() < 25 && LocalPlayer.CurrentMp < 2500))
						{
							return Eventide;
						}
					}

					if (!TargetHasEffectAny(PvPCommon.Buffs.Guard))
					{
						if (IsEnabled(CustomComboPreset.DRKPvP_SaltCombo)
							&& ((ActionReady(Plunge) && ActionReady(SaltedEarth))
							|| (ActionReady(SaltedEarth) && WasLastAbility(Plunge))
							|| IsEnabled(SaltAndDarkness)))
						{
							if (IsEnabled(CustomComboPreset.DRKPvP_BlackestNight) && ActionReady(BlackestNight))
							{
								return BlackestNight;
							}

							if (ActionReady(Plunge))
							{
								return Plunge;
							}

							return OriginalHook(SaltedEarth);
						}

						if (CanWeave(actionID))
						{
							if (IsEnabled(CustomComboPreset.DRKPvP_Plunge) && ActionReady(Plunge)
								&& GetCooldownRemainingTime(SaltedEarth) > 13)
							{
								if (IsEnabled(CustomComboPreset.DRKPvP_BlackestNight) && ActionReady(BlackestNight))
								{
									return BlackestNight;
								}

								return Plunge;
							}

							if (IsEnabled(CustomComboPreset.DRKPvP_BlackestNight) && ActionReady(BlackestNight)
								&& GetCooldownRemainingTime(SaltedEarth) > 13 && GetCooldownRemainingTime(Plunge) > 10)
							{
								return BlackestNight;
							}

							if (IsEnabled(CustomComboPreset.DRKPvP_Shadowbringer) && ActionReady(Shadowbringer)
								&& !HasEffect(Buffs.Blackblood)
								&& PlayerHealthPercentageHp() >= GetOptionValue(Config.DRKPvP_ShadowbringerHP)
								&& LocalPlayer.CurrentMp >= GetOptionValue(Config.DRKPvP_ShadowbringerMP))
							{
								return Shadowbringer;
							}
						}

						if (IsEnabled(CustomComboPreset.DRKPvP_Quietus) && ActionReady(Quietus)
							&& PlayerHealthPercentageHp() <= GetOptionValue(Config.DRKPvP_Quietus) && InActionRange(Quietus))
						{
							return Quietus;
						}
					}
				}

				return actionID;
			}
		}
	}
}