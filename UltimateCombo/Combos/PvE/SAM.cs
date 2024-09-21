using Dalamud.Game.ClientState.JobGauge.Enums;
using Dalamud.Game.ClientState.JobGauge.Types;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.CustomCombo;
using UltimateCombo.Data;

namespace UltimateCombo.Combos.PvE
{
	internal static class SAM
	{
		public const byte JobID = 34;

		public const uint
			Hakaze = 7477,
			Yukikaze = 7480,
			Gekko = 7481,
			Enpi = 7486,
			Jinpu = 7478,
			Kasha = 7482,
			Shifu = 7479,
			Mangetsu = 7484,
			Fuga = 7483,
			Oka = 7485,
			Higanbana = 7489,
			TenkaGoken = 7488,
			Setsugekka = 7487,
			Shinten = 7490,
			Kyuten = 7491,
			Hagakure = 7495,
			Guren = 7496,
			Senei = 16481,
			MeikyoShisui = 7499,
			Seigan = 7501,
			ThirdEye = 7498,
			Tengentsu = 36962,
			Iaijutsu = 7867,
			TsubameGaeshi = 16483,
			KaeshiHiganbana = 16484,
			Shoha = 16487,
			Ikishoten = 16482,
			Fuko = 25780,
			OgiNamikiri = 25781,
			KaeshiNamikiri = 25782,
			Yaten = 7493,
			Gyoten = 7492,
			Gyofu = 36963,
			Zanshin = 36964,
			TendoGoken = 36965,
			TendoSetsugekka = 36966,
			TendoKaeshiGoken = 36967,
			TendoKaeshiSetsugekka = 36968;

		public static class Buffs
		{
			public const ushort
				MeikyoShisui = 1233,
				EnhancedEnpi = 1236,
				EyesOpen = 1252,
				OgiNamikiriReady = 2959,
				Fuka = 1299,
				Fugetsu = 1298,
				Zanshin = 3855,
				Tendo = 3856,
				AoETsubameReady = 3852,
				TsubameReady = 4216,
				EnhancedAoETsubameReady = 4217,
				EnhancedTsubameReady = 4218;
		}

		public static class Debuffs
		{
			public const ushort
				Higanbana = 1228;
		}

		private static SAMGauge Gauge
		{
			get
			{
				return CustomComboFunctions.GetJobGauge<SAMGauge>();
			}
		}

		public static class Config
		{
			public static UserInt
				SAM_Variant_Cure = new("SAM_Variant_Cure", 50);
		}

		internal class SAM_ST_DPS : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SAM_ST_DPS;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Hakaze or Gyofu or Jinpu or Gekko or Shifu or Kasha or Yukikaze)
					&& IsEnabled(CustomComboPreset.SAM_ST_DPS))
				{
					if (IsEnabled(CustomComboPreset.SAM_ST_Meikyo) && !InCombat() && !HasEffect(Buffs.MeikyoShisui)
						&& ActionReady(MeikyoShisui))
					{
						return MeikyoShisui;
					}

					if (CanWeave(actionID) && ActionWatching.NumberOfGcdsUsed > 2)
					{
						if (IsEnabled(CustomComboPreset.SAM_ST_Shield) && ActionReady(OriginalHook(ThirdEye)))
						{
							return OriginalHook(ThirdEye);
						}

						if (IsEnabled(CustomComboPreset.SAM_ST_Ikishoten) && ActionReady(Ikishoten) && Gauge.Kenki <= 50)
						{
							return Ikishoten;
						}

						if (IsEnabled(CustomComboPreset.SAM_ST_Meikyo) && ActionReady(MeikyoShisui) && !HasEffect(Buffs.MeikyoShisui)
							&& !HasEffect(Buffs.Tendo) && !WasLastWeaponskill(Hakaze) && !WasLastWeaponskill(Gyofu)
							&& !WasLastWeaponskill(Jinpu) && !WasLastWeaponskill(Shifu)
							&& (HasEffect(Buffs.OgiNamikiriReady) || !LevelChecked(OgiNamikiri)))
						{
							return MeikyoShisui;
						}

						if (IsEnabled(CustomComboPreset.SAM_ST_Ikishoten) && HasEffect(Buffs.Zanshin))
						{
							return Zanshin;
						}

						if (IsEnabled(CustomComboPreset.SAM_ST_Shoha) && ActionReady(Shoha) && Gauge.MeditationStacks == 3)
						{
							return Shoha;
						}

						if (IsEnabled(CustomComboPreset.SAM_ST_Senei) && ActionReady(Senei) && ActionReady(Senei) && Gauge.Kenki >= 25)
						{
							return Senei;
						}

						if (IsEnabled(CustomComboPreset.SAM_ST_Shinten) && ActionReady(Shinten)
							&& ((Gauge.Kenki >= 25 && (GetCooldownRemainingTime(Ikishoten) > 15 || GetCooldownRemainingTime(Ikishoten) < 5))
							|| Gauge.Kenki == 100))
						{
							return Shinten;
						}
					}

					if (IsEnabled(CustomComboPreset.SAM_ST_Kaeshi) && WasLastWeaponskill(OgiNamikiri))
					{
						return KaeshiNamikiri;
					}

					if (IsEnabled(CustomComboPreset.SAM_ST_Kaeshi) && HasEffect(Buffs.OgiNamikiriReady) && GetRemainingCharges(MeikyoShisui) == 0)
					{
						return OgiNamikiri;
					}

					if (IsEnabled(CustomComboPreset.SAM_ST_Tsubame) && (HasEffect(Buffs.TsubameReady) || HasEffect(Buffs.EnhancedTsubameReady))
						&& ActionWatching.NumberOfGcdsUsed > 2)
					{
						return OriginalHook(TsubameGaeshi);
					}

					if (IsEnabled(CustomComboPreset.SAM_ST_Higanbana) && GetDebuffRemainingTime(Debuffs.Higanbana) < 3
						&& OriginalHook(Iaijutsu) != TenkaGoken && OriginalHook(Iaijutsu) != TendoGoken
						&& EnemyHealthCurrentHp() > LocalPlayer.MaxHp
						&& (Gauge.Sen.HasFlag(Sen.GETSU) || Gauge.Sen.HasFlag(Sen.KA) || Gauge.Sen.HasFlag(Sen.SETSU))
						&& ActionWatching.NumberOfGcdsUsed > 2 && (EnemyHealthMaxHp() >= LocalPlayer.MaxHp * 10 || EnemyHealthMaxHp() == 44))
					{
						return OriginalHook(Iaijutsu);
					}

					if (Gauge.Sen.HasFlag(Sen.GETSU) && Gauge.Sen.HasFlag(Sen.KA) && Gauge.Sen.HasFlag(Sen.SETSU)
						&& IsEnabled(CustomComboPreset.SAM_ST_Iaijutsu) && ActionWatching.NumberOfGcdsUsed > 2)
					{
						return OriginalHook(Iaijutsu);
					}

					if (HasEffect(Buffs.MeikyoShisui))
					{
						if (!Gauge.Sen.HasFlag(Sen.KA) && ActionReady(Kasha))
						{
							return Kasha;
						}

						if (!Gauge.Sen.HasFlag(Sen.GETSU) && ActionReady(Gekko))
						{
							return Gekko;
						}

						if (!Gauge.Sen.HasFlag(Sen.SETSU) && ActionReady(Yukikaze))
						{
							return Yukikaze;
						}
					}

					if (comboTime > 0)
					{
						if (lastComboMove is Jinpu && ActionReady(Gekko))
						{
							return Gekko;
						}

						if (lastComboMove is Shifu && ActionReady(Kasha))
						{
							return Kasha;
						}

						if (lastComboMove is Hakaze or Gyofu)
						{
							if (!HasEffect(Buffs.Fuka) || (!Gauge.Sen.HasFlag(Sen.KA) && ActionReady(Shifu)))
							{
								return Shifu;
							}

							if (!HasEffect(Buffs.Fugetsu) || (!Gauge.Sen.HasFlag(Sen.GETSU) && ActionReady(Jinpu)))
							{
								return Jinpu;
							}

							if (!Gauge.Sen.HasFlag(Sen.SETSU) && ActionReady(Yukikaze))
							{
								return Yukikaze;
							}
						}
					}

					return OriginalHook(Hakaze);
				}

				return actionID;
			}
		}

		internal class SAM_AoE_DPS : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SAM_AoE_DPS;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Fuga or Fuko or Mangetsu or Oka) && IsEnabled(CustomComboPreset.SAM_AoE_DPS))
				{
					if (IsEnabled(CustomComboPreset.SAM_AoE_Meikyo) && !InCombat() && !HasEffect(Buffs.MeikyoShisui)
						&& ActionReady(MeikyoShisui))
					{
						return MeikyoShisui;
					}

					if (CanWeave(actionID))
					{
						if (IsEnabled(CustomComboPreset.SAM_AoE_Shield) && ActionReady(OriginalHook(ThirdEye)))
						{
							return OriginalHook(ThirdEye);
						}

						if (IsEnabled(CustomComboPreset.SAM_AoE_Ikishoten) && ActionReady(Ikishoten) && Gauge.Kenki <= 50)
						{
							return Ikishoten;
						}

						if (IsEnabled(CustomComboPreset.SAM_AoE_Meikyo) && ActionReady(MeikyoShisui) && !HasEffect(Buffs.MeikyoShisui)
							&& !HasEffect(Buffs.Tendo) && !WasLastWeaponskill(Fuga) && !WasLastWeaponskill(Fuko))
						{
							return MeikyoShisui;
						}

						if (IsEnabled(CustomComboPreset.SAM_AoE_Ikishoten) && HasEffect(Buffs.Zanshin))
						{
							return Zanshin;
						}

						if (IsEnabled(CustomComboPreset.SAM_AoE_Shoha) && ActionReady(Shoha) && Gauge.MeditationStacks == 3)
						{
							return Shoha;
						}

						if (IsEnabled(CustomComboPreset.SAM_AoE_Guren) && ActionReady(Guren) && Gauge.Kenki >= 25)
						{
							return Guren;
						}

						if (IsEnabled(CustomComboPreset.SAM_AoE_Kyuten) && ActionReady(Shinten)
							&& ((Gauge.Kenki >= 25 && (GetCooldownRemainingTime(Ikishoten) > 15 || GetCooldownRemainingTime(Ikishoten) < 5))
							|| Gauge.Kenki == 100))
						{
							return Kyuten;
						}
					}

					if (IsEnabled(CustomComboPreset.SAM_AoE_Kaeshi) && WasLastWeaponskill(OgiNamikiri))
					{
						return KaeshiNamikiri;
					}

					if (IsEnabled(CustomComboPreset.SAM_AoE_Kaeshi) && HasEffect(Buffs.OgiNamikiriReady))
					{
						return OgiNamikiri;
					}

					if (IsEnabled(CustomComboPreset.SAM_AoE_Tsubame) && (HasEffect(Buffs.AoETsubameReady) || HasEffect(Buffs.EnhancedAoETsubameReady)))
					{
						return OriginalHook(TsubameGaeshi);
					}

					if (IsEnabled(CustomComboPreset.SAM_AoE_Iaijutsu) && Gauge.Sen.HasFlag(Sen.GETSU) && Gauge.Sen.HasFlag(Sen.KA))
					{
						return OriginalHook(Iaijutsu);
					}

					if (HasEffect(Buffs.MeikyoShisui))
					{
						if (!Gauge.Sen.HasFlag(Sen.KA) && ActionReady(Oka))
						{
							return Oka;
						}

						if (!Gauge.Sen.HasFlag(Sen.GETSU) && ActionReady(Mangetsu))
						{
							return Mangetsu;
						}
					}

					if (comboTime > 0)
					{
						if (lastComboMove is Fuga or Fuko)
						{
							if (!HasEffect(Buffs.Fuka) || (!Gauge.Sen.HasFlag(Sen.KA) && ActionReady(Oka)))
							{
								return Oka;
							}

							if (!HasEffect(Buffs.Fugetsu) || (!Gauge.Sen.HasFlag(Sen.GETSU) && ActionReady(Mangetsu)))
							{
								return Mangetsu;
							}
						}
					}
					return OriginalHook(Fuga);
				}
				return actionID;
			}
		}
	}
}