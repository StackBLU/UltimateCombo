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

			CreatureMotif = 39204,
			PomMotif = 39200,
			WingMotif = 39201,
			ClawMotif = 39202,
			MawMotif = 39203,

			LivingMuse = 39209,
			PomMuse = 39205,
			WingedMuse = 39206,
			ClawedMuse = 39207,
			FangedMuse = 39208,

			SubtractivePalette = 39213,

			Smudge = 39210,

			HolyInWhite = 39198,
			CometInBlack = 39199,

			MogOfTheAges = 39782,
			RetributionOfTheMadeen = 39783,

			TemperaCoat = 39211,
			TemperaGrassa = 39212,

			AdventOfChocobastion = 39215,
			StarPrism = 39216;

		public static class Buffs
		{
			public const ushort
				Aetherhues1 = 4100,
				Aetherhues2 = 4101,

				Smudge = 4113,
				QuickSketch = 4324,

				TemperaCoat = 4114,
				TemperaGrassa = 4115,

				SubtractivePalette = 4102,

				PomSketch = 4124,
				PomMotif = 4105,
				PomMuse = 4109,

				WingSketch = 4125,
				WingMotif = 4106,
				WingedMuse = 4110,

				ClawSketch = 4126,
				ClawMotif = 4107,

				MawSketch = 4127,
				MawMotif = 4108,

				MooglePortrait = 4103,
				MadeenPortrait = 4104,

				AdventOfChocobastion = 4116,
				Chocobastion = 4117,
				Starstruck = 4118;
		}

		internal class Debuffs
		{
			internal const ushort
				ClawedMuse = 4111,
				FangedMuse = 4112;
		}

		public static class Config
		{
			public static UserInt
				PCTPvP_AutoPalette = new("PCTPvP_AutoPalette", 50);
		}

		internal class PCTPvP_Combo : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.PCTPvP_Combo;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is FireInRed or AeroInGreen or WaterInBlue or BlizzardInCyan or StoneInYellow or ThunderInMagenta)
					&& IsEnabled(CustomComboPreset.PCTPvP_Combo))
				{
					if (IsEnabled(CustomComboPreset.PCTPvP_CreatureMotifs) && ActionReady(OriginalHook(CreatureMotif))
						&& (!IsMoving || HasEffect(Buffs.QuickSketch)) && !InCombat()
						&& (HasEffect(Buffs.PomSketch) || HasEffect(Buffs.WingSketch)
						|| HasEffect(Buffs.ClawSketch) || HasEffect(Buffs.MawSketch)))
					{
						return OriginalHook(CreatureMotif);
					}

					if (CanWeave(actionID))
					{
						if (IsEnabled(CustomComboPreset.PCTPvP_TemperaCoat) && ActionReady(TemperaCoat))
						{
							return TemperaCoat;
						}
					}

					if (!TargetHasEffectAny(PvPCommon.Buffs.Guard))
					{
						if (IsEnabled(CustomComboPreset.PCTPvP_StarPrism) && ActionReady(OriginalHook(AdventOfChocobastion))
							&& HasEffect(Buffs.Starstruck) && (LocalPlayer.CurrentHp <= (LocalPlayer.MaxHp - 8000) ||
							GetBuffRemainingTime(Buffs.Starstruck) < 5) && !WasLastAction(AdventOfChocobastion))
						{
							return OriginalHook(AdventOfChocobastion);
						}

						if (IsEnabled(CustomComboPreset.PCTPvP_Paints) && ActionReady(OriginalHook(HolyInWhite))
							&& (IsMoving || GetRemainingCharges(OriginalHook(HolyInWhite)) == 2))
						{
							return OriginalHook(HolyInWhite);
						}

						if (IsEnabled(CustomComboPreset.PCTPvP_Portraits) && ActionReady(OriginalHook(MogOfTheAges))
							&& (HasEffect(Buffs.MooglePortrait) || HasEffect(Buffs.MadeenPortrait))
							&& (HasEffect(Buffs.PomMuse) || TargetHasEffect(Debuffs.ClawedMuse)
							|| HasEffect(Buffs.WingMotif) || HasEffect(Buffs.MawMotif)))
						{
							return OriginalHook(MogOfTheAges);
						}

						if (IsEnabled(CustomComboPreset.PCTPvP_LivingMuses) && ActionReady(OriginalHook(LivingMuse))
							&& (HasEffect(Buffs.PomMotif) || HasEffect(Buffs.WingMotif)
							|| HasEffect(Buffs.ClawMotif) || HasEffect(Buffs.MawMotif)))
						{
							return OriginalHook(LivingMuse);
						}
					}

					if (IsEnabled(CustomComboPreset.PCTPvP_AutoPalette) && ActionReady(OriginalHook(SubtractivePalette)) && CanLateWeave(actionID))
					{
						if (IsMoving && HasEffect(Buffs.SubtractivePalette) && (GetRemainingCharges(OriginalHook(HolyInWhite)) == 0
							|| PlayerHealthPercentageHp() <= GetOptionValue(Config.PCTPvP_AutoPalette)))
						{
							return OriginalHook(SubtractivePalette);
						}

						if ((!IsMoving || (IsMoving && GetRemainingCharges(OriginalHook(HolyInWhite)) >= 1
							&& PlayerHealthPercentageHp() >= GetOptionValue(Config.PCTPvP_AutoPalette)))
							&& !HasEffect(Buffs.SubtractivePalette))
						{
							return OriginalHook(SubtractivePalette);
						}
					}

					if (IsEnabled(CustomComboPreset.PCTPvP_CreatureMotifs) && ActionReady(OriginalHook(CreatureMotif))
						&& (!IsMoving || HasEffect(Buffs.QuickSketch))
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
