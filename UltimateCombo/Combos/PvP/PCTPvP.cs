using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvP
{
	internal static class PCTPvP
	{
		public const uint
			FireInRed = 39191,
			AeroInGreen = 39192,
			WaterInBlue = 39193,
			BlizzardInCyan = 39194,
			StoneInYellow = 39195,
			ThunderInMagenta = 39196,
			HolyInWhite = 39198,
			CometInBlack = 39199,

			CreatureMotif = 39204,
			PomMotif = 39200,
			WingMotif = 39201,
			ClawMotif = 39202,
			MawMotif = 39203,

			LivingMuse = 39209,
			PomMuse = 39205,
			WingedMuse = 39206,
			ClawedMused = 39207,
			FangedMuse = 39208,

			MogOfTheAges = 39782,
			RetributionOfTheMadeen = 39783,

			Smudge = 39210,
			TemperaCoat = 39211,
			TemperaGrassa = 39212,
			SubtractivePalette = 39213,
			AdventOfChoco = 39215,
			StarPrism = 39216;

		public static class Buffs
		{
			public const ushort
				SubtractivePalette = 4102,

				PomMotif = 4105,
				WingMotif = 4106,
				ClawMotif = 4107,
				MawMotif = 4108,

				PomSketch = 4124,
				WingSketch = 4125,
				ClawSketch = 4126,
				MawSketch = 4127,

				MooglePortrait = 4103,
				MadeenPortrait = 4104,

				PomMuse = 4109,
				WingedMuse = 4110,

				Starstruck = 4118;
		}

		public static class Debuffs
		{
			public const ushort
				ClawedMuse = 4111,
				FangedMused = 4112;
		}

		public static class Config
		{
			public static UserInt
				PCTPvP_AutoSubtractive = new("PCTPvP_AutoSubtractive", 50);
		}

		internal class PCTPvP_Combo : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.PCTPvP_Combo;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (actionID is FireInRed && IsEnabled(CustomComboPreset.PCTPvP_Combo))
				{
					if (IsEnabled(CustomComboPreset.PCTPvP_Motif) && ActionReady(OriginalHook(CreatureMotif))
						&& !IsMoving && !InCombat()
						&& (HasEffect(Buffs.PomSketch) || HasEffect(Buffs.WingSketch)
						|| HasEffect(Buffs.ClawSketch) || HasEffect(Buffs.MawSketch)))
					{
						return OriginalHook(CreatureMotif);
					}

					if (IsEnabled(CustomComboPreset.PCTPvP_AutoSubtractive) && ActionReady(OriginalHook(SubtractivePalette)))
					{
						if (IsMoving && HasEffect(Buffs.SubtractivePalette) && (GetRemainingCharges(OriginalHook(HolyInWhite)) == 0
							|| PlayerHealthPercentageHp() < GetOptionValue(Config.PCTPvP_AutoSubtractive)))
						{
							return OriginalHook(SubtractivePalette);
						}

						if ((!IsMoving || (IsMoving && GetRemainingCharges(OriginalHook(HolyInWhite)) >= 1
							&& PlayerHealthPercentageHp() >= GetOptionValue(Config.PCTPvP_AutoSubtractive)))
							&& !HasEffect(Buffs.SubtractivePalette))
						{
							return OriginalHook(SubtractivePalette);
						}
					}

					if (CanWeave(actionID))
					{
						if (IsEnabled(CustomComboPreset.PCTPvP_Tempera) && ActionReady(TemperaCoat))
						{
							return TemperaCoat;
						}
					}

					if (!TargetHasEffectAny(PvPCommon.Buffs.Guard))
					{
						if (IsEnabled(CustomComboPreset.PCTPvP_StarPrism) && ActionReady(OriginalHook(AdventOfChoco))
							&& HasEffect(Buffs.Starstruck) && (LocalPlayer.CurrentHp <= (LocalPlayer.MaxHp - 8000) ||
							GetBuffRemainingTime(Buffs.Starstruck) < 5))
						{
							return OriginalHook(AdventOfChoco);
						}

						if (IsEnabled(CustomComboPreset.PCTPvP_Paint) && ActionReady(OriginalHook(HolyInWhite))
							&& (IsMoving || GetRemainingCharges(OriginalHook(HolyInWhite)) == 2))
						{
							return OriginalHook(HolyInWhite);
						}

						if (IsEnabled(CustomComboPreset.PCTPvP_Portrait) && ActionReady(OriginalHook(MogOfTheAges))
							&& (HasEffect(Buffs.MooglePortrait) || HasEffect(Buffs.MadeenPortrait))
							&& (HasEffect(Buffs.PomMuse) || TargetHasEffect(Debuffs.ClawedMuse)
							|| HasEffect(Buffs.WingMotif) || HasEffect(Buffs.MawMotif)))
						{
							return OriginalHook(MogOfTheAges);
						}

						if (IsEnabled(CustomComboPreset.PCTPvP_Muse) && ActionReady(OriginalHook(LivingMuse))
							&& (HasEffect(Buffs.PomMotif) || HasEffect(Buffs.WingMotif)
							|| HasEffect(Buffs.ClawMotif) || HasEffect(Buffs.MawMotif)))
						{
							return OriginalHook(LivingMuse);
						}
					}

					if (IsEnabled(CustomComboPreset.PCTPvP_Motif) && ActionReady(OriginalHook(CreatureMotif))
						&& !IsMoving
						&& (HasEffect(Buffs.PomSketch) || HasEffect(Buffs.WingSketch)
						|| HasEffect(Buffs.ClawSketch) || HasEffect(Buffs.MawSketch)))
					{
						return OriginalHook(CreatureMotif);
					}
				}

				return actionID;
			}
		}
	}
}