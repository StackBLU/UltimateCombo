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

		}

		/*internal class DRKPvP_Combo : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DRKPvP_Combo;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is HardSlash or SyphonStrike or Souleater) && IsEnabled(CustomComboPreset.DRKPvP_Combo))
				{
					if (IsEnabled(CustomComboPreset.DRKPvP_Eventide) && GetLimitBreakCurrentValue() == GetLimitBreakMaxValue())
					{
						return Eventide;
					}

					if (!TargetHasEffectAny(PvPCommon.Buffs.Guard))
					{
						if (CanWeave(actionID))
						{
							if (IsEnabled(CustomComboPreset.SGEPvP_Toxikon) && ActionReady(OriginalHook(Toxikon)) && !TargetHasEffect(Debuffs.Toxikon))
							{
								return OriginalHook(Toxikon);
							}
						}

						if (IsEnabled(CustomComboPreset.SGEPvP_Eukrasia) && ActionReady(Eukrasia) && !HasEffect(Buffs.Eukrasia)
							&& !HasEffect(Buffs.Diagnosis) && !TargetHasEffect(Debuffs.EukrasianDosis)
							&& (TargetHasEffect(Debuffs.Toxikon) || (GetRemainingCharges(Toxikon) == 0 && GetCooldownChargeRemainingTime(Toxikon) > 5)))
						{
							return Eukrasia;
						}

						if (IsEnabled(CustomComboPreset.SGEPvP_Pneuma) && ActionReady(Pneuma))
						{
							return Pneuma;
						}

						if (IsEnabled(CustomComboPreset.SGEPvP_Phlegma) && ActionReady(Phlegma) && InActionRange(Phlegma))
						{
							return Phlegma;
						}
					}
				}

				return actionID;
			}
		}*/
	}
}