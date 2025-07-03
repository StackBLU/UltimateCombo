using Dalamud.Game.ClientState.JobGauge.Types;
using System.Collections.Generic;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Combos.Content;
using UltimateCombo.CustomCombo;
using UltimateCombo.Data;
using UltimateCombo.Services;

namespace UltimateCombo.Combos.PvE
{
	internal class BLM
	{
		public const byte JobID = 25;

		public const uint
			Fire = 141,
			Blizzard = 142,
			Thunder = 144,
			Fire2 = 147,
			Transpose = 149,
			Fire3 = 152,
			Thunder3 = 153,
			Blizzard3 = 154,
			AetherialManipulation = 155,
			Scathe = 156,
			Manafont = 158,
			Freeze = 159,
			Flare = 162,
			LeyLines = 3573,
			Sharpcast = 3574,
			Blizzard4 = 3576,
			Fire4 = 3577,
			BetweenTheLines = 7419,
			Thunder4 = 7420,
			Triplecast = 7421,
			Foul = 7422,
			Thunder2 = 7447,
			Despair = 16505,
			UmbralSoul = 16506,
			Xenoglossy = 16507,
			Blizzard2 = 25793,
			HighFire2 = 25794,
			HighBlizzard2 = 25795,
			Amplifier = 25796,
			Paradox = 25797,
			HighThunder = 36986,
			HighThunder2 = 36987,
			FlareStar = 36989;

		public static class Buffs
		{
			public const ushort
				Thunderhead = 3870,
				Firestarter = 165,
				LeyLines = 737,
				CircleOfPower = 738,
				Sharpcast = 867,
				Triplecast = 1211,
				EnhancedFlare = 2960;
		}

		public static class Debuffs
		{
			public const ushort
				Thunder = 161,
				Thunder2 = 162,
				Thunder3 = 163,
				Thunder4 = 1210,
				HighThunder = 3871,
				HighThunder2 = 3872;
		}

		public static readonly Dictionary<uint, ushort>
			ThunderList = new()
			{
				{ Thunder,  Debuffs.Thunder  },
				{ Thunder2, Debuffs.Thunder2 },
				{ Thunder3, Debuffs.Thunder3 },
				{ Thunder4, Debuffs.Thunder4 },
				{ HighThunder, Debuffs.HighThunder },
				{ HighThunder2, Debuffs.HighThunder2 }
			};

		public static int MaxPolyglot(byte level)
		{
			if (level >= 98)
			{
				return 3;
			}

			if (level >= 80)
			{
				return 2;
			}

			if (level >= 70)
			{
				return 1;
			}

			return 0;
		}

		private static BLMGauge Gauge
		{
			get
			{
				return CustomComboFunctions.GetJobGauge<BLMGauge>();
			}
		}

		public static class Config
		{

		}

		internal class BLM_ST_DPS : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLM_ST_DPS;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Fire or Fire3 or Fire4 or Blizzard or Blizzard3 or Blizzard4) && IsEnabled(CustomComboPreset.BLM_ST_DPS))
				{
					//Bozja
					{
						if (HasEffect(Bozja.Buffs.Reminiscence) && IsEnabled(CustomComboPreset.Bozja_LFS))
						{
							if (HasEffect(Bozja.Buffs.FontOfMagic) && GetBuffRemainingTime(Bozja.Buffs.FontOfMagic) < 7)
							{
								if (WasLastSpell(Blizzard4) && GetBuffRemainingTime(Bozja.Buffs.FontOfMagic) < 5)
								{
									return Bozja.FlareStar;
								}

								if (Gauge.InUmbralIce && !WasLastSpell(Blizzard4))
								{
									return Blizzard4;
								}

								if (!Gauge.InUmbralIce)
								{
									return Blizzard3;
								}
							}

							if (GetDebuffRemainingTime(Bozja.Debuffs.FlareStar) < 5 || !TargetHasEffect(Bozja.Debuffs.FlareStar) || DutyActionReady(Bozja.FontOfMagic))
							{
								if (WasLastAction(Blizzard4) && (HasEffect(Bozja.Buffs.FontOfMagic) || DutyActionNotEquipped(Bozja.FontOfMagic)))
								{
									return Bozja.FlareStar;
								}

								if (WasLastAction(Bozja.FontOfMagic))
								{
									return Blizzard4;
								}

								if (WasLastAction(All.LucidDreaming) && DutyActionReady(Bozja.FontOfMagic))
								{
									return Bozja.FontOfMagic;
								}

								if (ActionReady(All.LucidDreaming) && WasLastAction(Blizzard4))
								{
									return All.LucidDreaming;
								}

								if (!Gauge.InUmbralIce)
								{
									return Blizzard3;
								}

								return Blizzard4;
							}
						}
					}

					if (CanWeave(actionID))
					{
						if (IsEnabled(CustomComboPreset.BLM_ST_Swiftcast) && ActionReady(All.Swiftcast) && !Gauge.IsParadoxActive
							&& Gauge.InAstralFire && !HasEffect(Buffs.Triplecast) && !HasEffect(Occult.Buffs.OccultQuick)
							&& (LocalPlayer.CurrentMp >= 4000 || ActionReady(Manafont)
							|| (GetCooldownRemainingTime(Manafont) < 5 && LocalPlayer.CurrentMp > 2000)))
						{
							return All.Swiftcast;
						}

						if (IsEnabled(CustomComboPreset.BLM_ST_Amplifier) && ActionReady(Amplifier) && Gauge.PolyglotStacks < MaxPolyglot(LocalPlayer.Level)
							&& (Gauge.InUmbralIce || Gauge.InAstralFire) && TargetIsBoss())
						{
							return Amplifier;
						}

						if (ActionWatching.NumberOfGcdsUsed >= 5 || Service.Configuration.IgnoreGCDChecks)
						{
							if (IsEnabled(CustomComboPreset.BLM_ST_Triplecast) && ActionReady(Triplecast) && !Gauge.IsParadoxActive
								&& (HasEffect(Buffs.CircleOfPower) || GetRemainingCharges(Triplecast) == 2
								|| (GetCooldownChargeRemainingTime(Triplecast) < 10 && GetRemainingCharges(Triplecast) == 1))
								&& Gauge.InAstralFire
								&& !HasEffect(All.Buffs.Swiftcast) && !HasEffect(Buffs.Triplecast) && !HasEffect(Occult.Buffs.OccultQuick)
								&& (LocalPlayer.CurrentMp >= 4000 || ActionReady(Manafont)
								|| (GetCooldownRemainingTime(Manafont) < 5 && LocalPlayer.CurrentMp > 2000)))
							{
								return Triplecast;
							}

							if (IsEnabled(CustomComboPreset.BLM_ST_LeyLines) && ActionReady(LeyLines) && !HasEffect(Buffs.LeyLines) && !WasLastAction(LeyLines))
							{
								return LeyLines;
							}
						}
					}

					if (IsEnabled(CustomComboPreset.BLM_ST_Thunder) && ActionReady(OriginalHook(Thunder))
						&& HasEffect(Buffs.Thunderhead) && !HasEffect(Buffs.Triplecast)
						&& TargetIsBoss() && GetDebuffRemainingTime(ThunderList[OriginalHook(Thunder)]) <= 1.5)
					{
						return OriginalHook(Thunder);
					}

					if (IsEnabled(CustomComboPreset.BLM_ST_Xeno) && ActionReady(Xenoglossy) && Gauge.PolyglotStacks >= 1
						&& !HasEffect(Buffs.Triplecast) && !HasEffect(All.Buffs.Swiftcast)
						&& ((Gauge.PolyglotStacks == MaxPolyglot(LocalPlayer.Level) && Gauge.EnochianTimer <= 10000)
						|| (ActionReady(Amplifier) && Gauge.PolyglotStacks == MaxPolyglot(LocalPlayer.Level))
						|| ActionWatching.NumberOfGcdsUsed == 4 || IsMoving))
					{
						if (IsEnabled(CustomComboPreset.BLM_ST_Thunder) && ActionReady(OriginalHook(Thunder)) && HasEffect(Buffs.Thunderhead)
							&& GetDebuffRemainingTime(ThunderList[OriginalHook(Thunder)]) <= 3)
						{
							return OriginalHook(Thunder);
						}

						if (ActionReady(Paradox) && Gauge.IsParadoxActive)
						{
							return Paradox;
						}

						if (ActionReady(Despair) && Gauge.InAstralFire && LocalPlayer.CurrentMp <= 1600 && LocalPlayer.CurrentMp >= 800)
						{
							return Despair;
						}

						return Xenoglossy;
					}

					if (IsEnabled(CustomComboPreset.BLM_ST_Manafont) && ActionReady(Manafont) && HasEffect(Buffs.CircleOfPower) && Gauge.InAstralFire
						&& LocalPlayer.CurrentMp == 0)
					{
						return Manafont;
					}

					if (IsEnabled(CustomComboPreset.BLM_ST_FlareStar) && ActionReady(FlareStar) && Gauge.InAstralFire && Gauge.AstralSoulStacks == 6)
					{
						return FlareStar;
					}

					if (ActionReady(Paradox) && Gauge.IsParadoxActive)
					{
						if (HasEffect(Buffs.Firestarter) && Gauge.InAstralFire)
						{
							return Fire3;
						}

						if ((Gauge.InAstralFire && LocalPlayer.CurrentMp >= 1600) || Gauge.InUmbralIce)
						{
							return Paradox;
						}
					}

					if (ActionReady(Despair) && Gauge.InAstralFire && ((LocalPlayer.CurrentMp <= 1600 && LocalPlayer.CurrentMp >= 800) || HasEffect(Bozja.Buffs.AutoEther)))
					{
						return Despair;
					}

					if (ActionReady(Blizzard4) && Gauge.InUmbralIce && LocalPlayer.CurrentMp < 10000 && !WasLastSpell(Blizzard4) && !WasLastAction(Bozja.FlareStar))
					{
						return Blizzard4;
					}

					//Level Checks
					{
						if (!LevelChecked(Blizzard3) && LocalPlayer.CurrentMp < 1600)
						{
							if (ActionReady(All.LucidDreaming))
							{
								return All.LucidDreaming;
							}

							return Blizzard;
						}

						if (!LevelChecked(Blizzard4) && LevelChecked(Blizzard3) && Gauge.InUmbralIce && LocalPlayer.CurrentMp < 10000 && !WasLastAction(UmbralSoul))
						{
							return UmbralSoul;
						}

						if (ActionReady(Blizzard3) && !WasLastAction(Transpose)
							&& ((Gauge.InAstralFire && LocalPlayer.CurrentMp < 800)
							|| (!Gauge.InAstralFire && !Gauge.InUmbralIce && LocalPlayer.CurrentMp < 10000 && LocalPlayer.CurrentMp >= 800)
							|| (!LevelChecked(Despair) && LocalPlayer.CurrentMp < 1600 && !WasLastSpell(Blizzard4))))
						{
							return Blizzard3;
						}
					}

					if (ActionReady(Fire4) && Gauge.InAstralFire && LocalPlayer.CurrentMp >= 1600 && !WasLastAction(Transpose) && !WasLastSpell(Blizzard4))
					{
						return Fire4;
					}

					if (ActionReady(Fire3)
						&& ((!Gauge.InAstralFire && !Gauge.InUmbralIce)
						|| (Gauge.InUmbralIce && LocalPlayer.CurrentMp == 10000) || WasLastSpell(Blizzard4)
						|| HasEffect(Buffs.Firestarter)))
					{
						if (ActionReady(Transpose) && Gauge.InUmbralIce && HasEffect(Buffs.Firestarter))
						{
							return Transpose;
						}

						return Fire3;
					}
				}

				return actionID;
			}
		}

		internal class BLM_AoE_DPS : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLM_AoE_DPS;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Fire2 or HighFire2 or Blizzard2 or HighBlizzard2) && IsEnabled(CustomComboPreset.BLM_AoE_DPS))
				{
					if (CanWeave(actionID))
					{
						if (IsEnabled(CustomComboPreset.BLM_AoE_Swiftcast) && ActionReady(All.Swiftcast)
							&& Gauge.InAstralFire && !HasEffect(Buffs.Triplecast) && !HasEffect(Occult.Buffs.OccultQuick)
							&& (LocalPlayer.CurrentMp >= 4000 || ActionReady(Manafont)
							|| (GetCooldownRemainingTime(Manafont) < 5 && LocalPlayer.CurrentMp > 2000)))
						{
							return All.Swiftcast;
						}

						if (IsEnabled(CustomComboPreset.BLM_AoE_Amplifier) && ActionReady(Amplifier) && Gauge.PolyglotStacks < MaxPolyglot(LocalPlayer.Level)
							&& (Gauge.InUmbralIce || Gauge.InAstralFire))
						{
							return Amplifier;
						}

						if (IsEnabled(CustomComboPreset.BLM_AoE_Triplecast) && ActionReady(Triplecast)
							&& (HasEffect(Buffs.CircleOfPower) || GetRemainingCharges(Triplecast) == 2
							|| (GetCooldownChargeRemainingTime(Triplecast) < 10 && GetRemainingCharges(Triplecast) == 1))
							&& Gauge.InAstralFire
							&& !HasEffect(All.Buffs.Swiftcast) && !HasEffect(Buffs.Triplecast) && !HasEffect(Occult.Buffs.OccultQuick)
							&& (LocalPlayer.CurrentMp >= 4000 || ActionReady(Manafont)
							|| (GetCooldownRemainingTime(Manafont) < 5 && LocalPlayer.CurrentMp > 2000)))
						{
							return Triplecast;
						}

						if (IsEnabled(CustomComboPreset.BLM_AoE_LeyLines) && ActionReady(LeyLines) && !HasEffect(Buffs.LeyLines) && !WasLastAction(LeyLines))
						{
							return LeyLines;
						}
					}

					if (IsEnabled(CustomComboPreset.BLM_AoE_Thunder) && ActionReady(OriginalHook(Thunder)) && HasEffect(Buffs.Thunderhead)
						&& !HasEffect(Buffs.Triplecast) && TargetIsBoss() && GetDebuffRemainingTime(ThunderList[OriginalHook(Thunder2)]) <= 1.5)
					{
						return OriginalHook(Thunder2);
					}

					if (IsEnabled(CustomComboPreset.BLM_AoE_Foul) && ActionReady(Foul) && Gauge.PolyglotStacks >= 1
						&& !HasEffect(Buffs.Triplecast) && !HasEffect(All.Buffs.Swiftcast)
						&& ((Gauge.PolyglotStacks == MaxPolyglot(LocalPlayer.Level) && Gauge.EnochianTimer <= 10000)
						|| (ActionReady(Amplifier) && Gauge.PolyglotStacks == MaxPolyglot(LocalPlayer.Level)) || IsMoving))
					{
						return Foul;
					}

					if (IsEnabled(CustomComboPreset.BLM_AoE_Manafont) && ActionReady(Manafont) && HasEffect(Buffs.CircleOfPower) && Gauge.InAstralFire
						&& LocalPlayer.CurrentMp == 0)
					{
						return Manafont;
					}

					if (IsEnabled(CustomComboPreset.BLM_AoE_FlareStar) && ActionReady(FlareStar) && Gauge.InAstralFire && Gauge.AstralSoulStacks == 6)
					{
						return FlareStar;
					}

					if (ActionReady(Flare) && Gauge.InAstralFire && LocalPlayer.CurrentMp <= 3000 && LocalPlayer.CurrentMp >= 800)
					{
						return Flare;
					}

					if (ActionReady(Freeze) && Gauge.InUmbralIce && LocalPlayer.CurrentMp < 10000 && !WasLastSpell(OriginalHook(Freeze)))
					{
						return Freeze;
					}

					if (ActionReady(OriginalHook(Blizzard2)) && Gauge.InAstralFire && LocalPlayer.CurrentMp == 0 && !WasLastSpell(OriginalHook(Freeze)))
					{
						return OriginalHook(Blizzard2);
					}

					if (LevelChecked(FlareStar) && Gauge.InAstralFire && LocalPlayer.CurrentMp >= 3000)
					{
						return Flare;
					}

					if (ActionReady(Transpose) && Gauge.InUmbralIce && (LocalPlayer.CurrentMp == 10000 || WasLastSpell(Freeze)))
					{
						return Transpose;
					}

					if (ActionReady(OriginalHook(Fire2)) && Gauge.InAstralFire && LocalPlayer.CurrentMp >= 3000)
					{
						return OriginalHook(Fire2);
					}
				}

				return actionID;
			}
		}

		internal class BLM_Fire4Blizzard4 : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLM_Fire4Blizzard4;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Fire4 or Blizzard4) && IsEnabled(CustomComboPreset.BLM_Fire4Blizzard4))
				{
					if (ActionReady(Fire4) && Gauge.InAstralFire)
					{
						return Fire4;
					}

					if (ActionReady(Blizzard4) && Gauge.InUmbralIce)
					{
						return Blizzard4;
					}
				}

				return actionID;
			}
		}

		internal class BLM_XenoParadox : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLM_XenoParadox;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Xenoglossy or Paradox or Despair) && IsEnabled(CustomComboPreset.BLM_XenoParadox))
				{
					if (ActionReady(Paradox) && Gauge.IsParadoxActive && ((Gauge.InAstralFire && LocalPlayer.CurrentMp >= 1600) || Gauge.InUmbralIce))
					{
						return Paradox;
					}

					if (ActionReady(Despair) && Gauge.InAstralFire && (LocalPlayer.CurrentMp <= 1600))
					{
						return Despair;
					}

					return Xenoglossy;
				}

				return actionID;
			}
		}

		internal class BLM_TriplecastProtect : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLM_TriplecastProtect;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Triplecast or All.Swiftcast) && IsEnabled(CustomComboPreset.BLM_TriplecastProtect))
				{
					if (HasEffect(Buffs.Triplecast) || HasEffect(All.Buffs.Swiftcast))
					{
						return OriginalHook(11);
					}
				}

				return actionID;
			}
		}

		internal class BLM_UmbralTranspose : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLM_UmbralTranspose;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (actionID is UmbralSoul && IsEnabled(CustomComboPreset.BLM_UmbralTranspose))
				{
					if (ActionReady(Transpose) && Gauge.InAstralFire)
					{
						return Transpose;
					}

					if (ActionReady(UmbralSoul) && Gauge.InUmbralIce)
					{
						return UmbralSoul;
					}
				}

				return actionID;
			}
		}

		internal class BLM_LeyLines : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLM_LeyLines;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is LeyLines or BetweenTheLines) && IsEnabled(CustomComboPreset.BLM_LeyLines))
				{
					if (HasEffect(Buffs.LeyLines))
					{
						return BetweenTheLines;
					}

					return LeyLines;
				}

				return actionID;
			}
		}
	}
}