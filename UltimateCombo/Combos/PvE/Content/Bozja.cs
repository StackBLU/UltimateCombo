﻿using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvE.Content
{
	public static class Bozja
	{
		public const uint
			Banish3 = 20702,
			FontOfMagic = 20715,
			FontOfPower = 20717,
			BannerOfNobleEnds = 20720,
			BannerOfHonoredSacrifice = 20721,
			Cure = 20726,
			Cure2 = 20727,
			Cure3 = 20728,
			Cure4 = 20729,
			Incense = 20731,
			FlareStar = 22352,
			RendArmor = 22353,
			SeraphStrike = 22354,
			Aethershield = 22355,
			Dervish = 22356,
			Burst = 23909,
			Rampage = 23910,
			Chainspell = 23913,
			Assassination = 23914,
			Excellence = 23919,
			BloodRage = 23921;

		public static class Buffs
		{
			public const ushort
				MPRefresh = 909,
				MPRefresh2 = 1198,
				ProfaneEssence = 2320,
				IrregularEssence = 2321,
				BeastEssence = 2324,
				BannerOfNobleEnds = 2326,
				BannerOfHonoredSacrifice = 2327,
				FontOfMagic = 2332,
				Bravery2 = 2341,
				AutoPotion = 2342,
				AutoEther = 2343,
				FontOfPower = 2346,
				Reminiscence = 2348,
				PureElder = 2435,
				PureFiendhunter = 2437,
				PureIndomitable = 2438,
				PureDivine = 2439,
				Aethershield = 2443,
				Dervish = 2444,
				ClericStance = 2484,
				Chainspell = 2560,
				Excellence = 2564,
				BloodRage = 2566,
				BloodRush = 2567;
		}

		public static class Debuffs
		{
			public const ushort
				FlareStar = 2440,
				RendArmor = 2441;
		}

		public static class Items
		{
			public const uint
				EtherKit = 38;
		}

		/*internal class Bozja_Test : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.Bozja_Test;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.Bozja_Test) && HasEffect(Buffs.Reminiscence))
				{
					PluginLog.Debug(FindHolsterItem(Items.EtherKit) + " " + HasHolsterItem(Items.EtherKit));
				}

				return actionID;
			}
		}*/

		internal class Bozja_FoP_HSac_NEnds : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.Bozja_FoP_HSac_NEnds;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.Bozja_FoP_HSac_NEnds) && HasEffect(Buffs.Reminiscence) && InCombat())
				{
					if ((IsEnabled(FontOfPower) && !HasEffect(Buffs.BloodRage)) || (IsEnabled(FontOfPower) && HasEffect(Buffs.BloodRush)))
					{
						return FontOfPower;
					}

					if ((WasLastAction(FontOfPower) || HasEffect(Buffs.FontOfPower)) && IsEnabled(BannerOfHonoredSacrifice))
					{
						return BannerOfHonoredSacrifice;
					}

					if ((WasLastAction(FontOfPower) || HasEffect(Buffs.FontOfPower)) && IsEnabled(BannerOfNobleEnds))
					{
						return BannerOfNobleEnds;
					}
				}

				return actionID;
			}
		}

		internal class Bozja_FoM_CS : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.Bozja_FoM_CS;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.Bozja_FoM_CS) && HasEffect(Buffs.Reminiscence) && InCombat())
				{
					if (IsEnabled(FontOfMagic))
					{
						return FontOfMagic;
					}

					if ((WasLastAction(FontOfMagic) || HasEffect(Buffs.FontOfMagic)) && IsEnabled(Chainspell))
					{
						return Chainspell;
					}
				}

				return actionID;
			}
		}

		internal class Bozja_Assassination : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.Bozja_Assassination;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.Bozja_Assassination) && HasEffect(Buffs.Reminiscence) && InCombat())
				{
					if (IsEnabled(Assassination) && InActionRange(Assassination))
					{
						return Assassination;
					}
				}

				return actionID;
			}
		}

		internal class Bozja_BloodRage : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.Bozja_BloodRage;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.Bozja_BloodRage) && HasEffect(Buffs.Reminiscence) && InCombat())
				{
					if (IsEnabled(BloodRage))
					{
						return BloodRage;
					}

					if (GetBuffStacks(Buffs.BloodRage) == 1
						|| (GetBuffStacks(Buffs.BloodRage) == 2 && GetBuffRemainingTime(Buffs.BloodRage) < 3)
						|| (GetBuffStacks(Buffs.BloodRage) == 3))
					{
						if (LocalPlayer.ClassJob.Value.RowId == PLD.JobID && ActionReady(PLD.Intervene))
						{
							return PLD.Intervene;
						}

						if (LocalPlayer.ClassJob.Value.RowId == WAR.JobID && ActionReady(WAR.Onslaught))
						{
							return WAR.Onslaught;
						}

						if (LocalPlayer.ClassJob.Value.RowId == DRK.JobID && ActionReady(DRK.Shadowstride))
						{
							return DRK.Shadowstride;
						}

						if (LocalPlayer.ClassJob.Value.RowId == GNB.JobID && ActionReady(GNB.Trajectory))
						{
							return GNB.Trajectory;
						}
					}
				}

				return actionID;
			}
		}
	}
}