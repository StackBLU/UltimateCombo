using Dalamud.Game.ClientState.JobGauge.Types;
using FFXIVClientStructs.FFXIV.Client.Game.InstanceContent;
using System.Collections.Generic;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Combos.PvE.Content;
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
			return 1;
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

		public static PublicContentBozja BozjaContent = new();

		internal class BLM_ST_DPS : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLM_ST_DPS;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Fire or Fire3 or Fire4 or Blizzard or Blizzard3 or Blizzard4) && IsEnabled(CustomComboPreset.BLM_ST_DPS))
				{
					if (CanWeave(actionID))
					{
						if (IsEnabled(CustomComboPreset.BLM_ST_Swiftcast) && ActionReady(All.Swiftcast)
							&& !HasEffect(Buffs.Triplecast) && Gauge.InAstralFire)
						{
							return All.Swiftcast;
						}

						if (IsEnabled(CustomComboPreset.BLM_ST_Amplifier) && ActionReady(Amplifier)
							&& Gauge.PolyglotStacks < MaxPolyglot(LocalPlayer.Level)
							&& (Gauge.InUmbralIce || Gauge.InAstralFire))
						{
							return Amplifier;
						}

						if (ActionWatching.NumberOfGcdsUsed >= 5 || Service.Configuration.IgnoreGCDChecks)
						{
							if (IsEnabled(CustomComboPreset.BLM_ST_Triplecast) && ActionReady(Triplecast)
							&& !HasEffect(Buffs.Triplecast) && Gauge.InAstralFire && !HasEffect(All.Buffs.Swiftcast)
							&& (GetRemainingCharges(Triplecast) == GetMaxCharges(Triplecast) || (IsMoving && Gauge.PolyglotStacks == 0) || HasEffect(Buffs.CircleOfPower)))
							{
								return Triplecast;
							}

							if (IsEnabled(CustomComboPreset.BLM_ST_LeyLines) && ActionReady(LeyLines) && !HasEffect(Buffs.LeyLines))
							{
								return LeyLines;
							}
						}
					}

					// Bozja
					if (IsEnabled(CustomComboPreset.Bozja_LFS) && HasEffect(Bozja.Buffs.Reminiscence)
						&& (GetBuffRemainingTime(Bozja.Buffs.FontOfMagic) < 7 || LocalPlayer.CurrentMp <= 4000)
						&& HasEffect(Bozja.Buffs.FontOfMagic) && !WasLastAction(Bozja.FontOfMagic) && !WasLastSpell(Bozja.FlareStar))
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

					if (IsEnabled(CustomComboPreset.Bozja_LFS) && HasEffect(Bozja.Buffs.Reminiscence)
						&& (GetDebuffRemainingTime(Bozja.Debuffs.FlareStar) < 5 || !TargetHasEffect(Bozja.Debuffs.FlareStar)
						|| (IsOffCooldown(Bozja.FontOfMagic) && IsEnabled(Bozja.FontOfMagic))))
					{
						if (WasLastAction(Blizzard4) && (IsOnCooldown(Bozja.FontOfMagic) || !IsEnabled(Bozja.FontOfMagic)))
						{
							return Bozja.FlareStar;
						}

						if (WasLastAction(Bozja.FontOfMagic))
						{
							return Blizzard4;
						}

						if (WasLastAction(All.LucidDreaming) && IsEnabled(Bozja.FontOfMagic))
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

					if (IsEnabled(CustomComboPreset.BLM_ST_FlareStar) && ActionReady(FlareStar) && Gauge.InAstralFire && Gauge.AstralSoulStacks == 6)
					{
						return FlareStar;
					}

					if (IsEnabled(CustomComboPreset.BLM_ST_Manafont) && ActionReady(Manafont)
						&& (LocalPlayer.CurrentMp == 0 || (LocalPlayer.CurrentMp < 1600 && LocalPlayer.Level < 72))
						&& HasEffect(Buffs.CircleOfPower) && Gauge.InAstralFire)
					{
						return Manafont;
					}

					//90+
					if (ActionReady(Paradox) && Gauge.IsParadoxActive && Gauge.InAstralFire)
					{
						return Paradox;
					}

					//90+
					if (ActionReady(Despair) && Gauge.InAstralFire && !Gauge.IsParadoxActive && LevelChecked(Paradox)
						&& LocalPlayer.CurrentMp <= 1200 && LocalPlayer.CurrentMp >= 800)
					{
						return Despair;
					}

					if (IsEnabled(CustomComboPreset.BLM_ST_Thunder) && ActionReady(OriginalHook(Thunder))
						&& HasEffect(Buffs.Thunderhead) && LocalPlayer.CurrentMp != 0
						&& (EnemyHealthMaxHp() >= LocalPlayer.MaxHp * 10 || EnemyHealthMaxHp() == 44)
						&& (Gauge.InUmbralIce
						|| (GetDebuffRemainingTime(ThunderList[OriginalHook(Thunder)]) < 3 && !WasLastSpell(Blizzard4))))
					{
						return OriginalHook(Thunder);
					}

					if (IsEnabled(CustomComboPreset.BLM_ST_Xeno) && ActionReady(Xenoglossy) && Gauge.PolyglotStacks >= 1
						&& ((Gauge.PolyglotStacks == MaxPolyglot(LocalPlayer.Level) && Gauge.EnochianTimer <= 10000)
						|| Gauge.PolyglotStacks == MaxPolyglot(LocalPlayer.Level) + 1
						|| (ActionReady(Amplifier) && Gauge.PolyglotStacks == MaxPolyglot(LocalPlayer.Level))
						|| (Gauge.InUmbralIce && Gauge.PolyglotStacks > MaxPolyglot(LocalPlayer.Level) - MaxPolyglot(LocalPlayer.Level))
						|| ActionWatching.NumberOfGcdsUsed == 4
						|| (IsMoving && ActionWatching.NumberOfGcdsUsed >= 10))
						&& (GetBuffRemainingTime(Buffs.Firestarter) >= 10 || !HasEffect(Buffs.Firestarter)))
					{
						return Xenoglossy;
					}

					if (ActionReady(All.LucidDreaming) && !Gauge.InAstralFire && !Gauge.InUmbralIce && LocalPlayer.CurrentMp < 800)
					{
						return All.LucidDreaming;
					}

					//90+
					if (ActionReady(Paradox) && Gauge.IsParadoxActive && Gauge.InUmbralIce)
					{
						return Paradox;
					}

					if (LocalPlayer.Level < 35)
					{
						if (LocalPlayer.CurrentMp < 1600 || (Gauge.InUmbralIce && LocalPlayer.CurrentMp < 8000))
						{
							if (ActionReady(All.Swiftcast))
							{
								return All.Swiftcast;
							}

							return Blizzard;
						}
					}

					if (LocalPlayer.Level < 72)
					{
						if (ActionReady(Fire) && Gauge.InAstralFire && LocalPlayer.CurrentMp >= 3200
							&& !HasEffect(Buffs.Triplecast) && !HasEffect(All.Buffs.Swiftcast))
						{
							return Fire;
						}

						if (ActionReady(UmbralSoul) && Gauge.InUmbralIce && !WasLastSpell(UmbralSoul)
							&& !WasLastSpell(Blizzard4) && LocalPlayer.CurrentMp < 10000)
						{
							if (LocalPlayer.Level >= 58)
							{
								return Blizzard4;
							}

							return UmbralSoul;
						}

						if (ActionReady(Blizzard3) && LocalPlayer.CurrentMp < 1600)
						{
							return Blizzard3;
						}

						if (ActionReady(Fire4) && Gauge.InAstralFire && LocalPlayer.CurrentMp >= 1600)
						{
							if (IsEnabled(CustomComboPreset.BLM_ST_Triplecast) && ActionReady(Triplecast) && CanWeave(actionID)
								&& !HasEffect(Buffs.Triplecast) && !HasEffect(All.Buffs.Swiftcast))
							{
								return Triplecast;
							}

							return Fire4;
						}
					}

					if (LocalPlayer.Level < 90)
					{
						if (ActionReady(Despair) && Gauge.InAstralFire && LocalPlayer.CurrentMp >= 800
							|| HasEffect(Bozja.Buffs.AutoEther) || (LocalPlayer.CurrentMp < 2400 && !HasEffect(Bozja.Buffs.FontOfMagic)))
						{
							if (ActionReady(All.Swiftcast) && !HasEffect(Buffs.Triplecast))
							{
								return All.Swiftcast;
							}

							if (IsEnabled(CustomComboPreset.BLM_ST_Triplecast) && ActionReady(Triplecast)
								&& !HasEffect(All.Buffs.Swiftcast) && !HasEffect(Buffs.Triplecast))
							{
								return Triplecast;
							}

							if (IsEnabled(CustomComboPreset.BLM_ST_LeyLines) && ActionReady(LeyLines) && !HasEffect(Buffs.LeyLines))
							{
								return LeyLines;
							}

							if (LocalPlayer.CurrentMp >= 3200 && !HasEffect(Bozja.Buffs.AutoEther))
							{
								return Fire;
							}

							return Despair;
						}

						if (ActionReady(Blizzard3) && !Gauge.InAstralFire && !Gauge.InUmbralIce && HasEffect(Bozja.Buffs.Reminiscence))
						{
							return Blizzard3;
						}
					}

					if (ActionReady(Blizzard4) && Gauge.InUmbralIce && LocalPlayer.CurrentMp < 10000 && !WasLastAction(Blizzard4)
						&& !WasLastAction(Bozja.FlareStar))
					{
						return Blizzard4;
					}

					if (ActionReady(Blizzard3) && !WasLastAction(Transpose) && ((Gauge.InAstralFire && LocalPlayer.CurrentMp < 800)
						|| (!Gauge.InAstralFire && !Gauge.InUmbralIce && LocalPlayer.CurrentMp < 10000 && LocalPlayer.CurrentMp >= 800
						&& !WasLastAbility(Manafont))))
					{
						return Blizzard3;
					}

					if (ActionReady(Fire4) && Gauge.InAstralFire && LocalPlayer.CurrentMp >= 1600
						&& !WasLastAction(Transpose) && !WasLastAction(Blizzard4))
					{
						if (IsEnabled(CustomComboPreset.BLM_ST_Triplecast) && ActionReady(Triplecast) && CanWeave(actionID)
							&& !HasEffect(Buffs.Triplecast) && !HasEffect(All.Buffs.Swiftcast) && LocalPlayer.Level < 86)
						{
							return Triplecast;
						}

						return Fire4;
					}

					if (ActionReady(Fire3) && ((!Gauge.InAstralFire && !Gauge.InUmbralIce)
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
							&& !HasEffect(Buffs.Triplecast) && Gauge.InAstralFire)
						{
							return All.Swiftcast;
						}

						if (IsEnabled(CustomComboPreset.BLM_AoE_Triplecast) && ActionReady(Triplecast)
							&& !HasEffect(Buffs.Triplecast) && Gauge.InAstralFire && !HasEffect(All.Buffs.Swiftcast)
							&& (GetRemainingCharges(Triplecast) == GetMaxCharges(Triplecast) || (IsMoving && Gauge.PolyglotStacks == 0) || HasEffect(Buffs.CircleOfPower)))
						{
							return Triplecast;
						}

						if (IsEnabled(CustomComboPreset.BLM_AoE_Amplifier) && ActionReady(Amplifier)
							&& Gauge.PolyglotStacks < MaxPolyglot(LocalPlayer.Level)
							&& (Gauge.InUmbralIce || Gauge.InAstralFire))
						{
							return Amplifier;
						}

						if (IsEnabled(CustomComboPreset.BLM_AoE_LeyLines) && ActionReady(LeyLines) && !HasEffect(Buffs.LeyLines))
						{
							return LeyLines;
						}
					}

					if (IsEnabled(CustomComboPreset.BLM_AoE_FlareStar) && ActionReady(FlareStar)
						&& LocalPlayer.CurrentMp == 0 && Gauge.InAstralFire && Gauge.AstralSoulStacks == 6)
					{
						return FlareStar;
					}

					if (IsEnabled(CustomComboPreset.BLM_AoE_Manafont) && ActionReady(Manafont) && LocalPlayer.CurrentMp == 0
						&& HasEffect(Buffs.CircleOfPower))
					{
						return Manafont;
					}

					if (IsEnabled(CustomComboPreset.BLM_AoE_Foul) && ActionReady(Foul) && Gauge.PolyglotStacks >= 1)
					{
						return Foul;
					}

					if (IsEnabled(CustomComboPreset.BLM_AoE_Thunder) && ActionReady(OriginalHook(Thunder2)) && HasEffect(Buffs.Thunderhead)
						&& GetDebuffRemainingTime(ThunderList[OriginalHook(Thunder2)]) < 3)
					{
						return OriginalHook(Thunder2);
					}

					if (LocalPlayer.Level < 60)
					{
						if (Gauge.InUmbralIce && !WasLastSpell(UmbralSoul) && LocalPlayer.CurrentMp < 10000)
						{
							if (LocalPlayer.Level < 35)
							{
								if (ActionReady(All.LucidDreaming))
								{
									return All.LucidDreaming;
								}

								return Blizzard;
							}

							return UmbralSoul;
						}

						if (LocalPlayer.CurrentMp == 0)
						{
							return Blizzard2;
						}

						if (ActionReady(Flare) && LocalPlayer.CurrentMp < 3000 && LocalPlayer.CurrentMp >= 800)
						{
							return Flare;
						}

						if (ActionReady(OriginalHook(Fire2)) && LocalPlayer.CurrentMp >= 3000)
						{
							return Fire2;
						}
					}

					if (ActionReady(Flare) && Gauge.InAstralFire && LocalPlayer.CurrentMp > 0)
					{
						return Flare;
					}

					if (ActionReady(OriginalHook(Fire2)) && Gauge.UmbralHearts == 3)
					{
						return OriginalHook(Fire2);
					}

					if (ActionReady(Freeze) && Gauge.InUmbralIce)
					{
						return Freeze;
					}

					return OriginalHook(Blizzard2);
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
					if (Gauge.InAstralFire)
					{
						return Fire4;
					}

					if (Gauge.InUmbralIce)
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
				if (actionID is Xenoglossy && IsEnabled(CustomComboPreset.BLM_XenoParadox))
				{
					if (ActionReady(Paradox) && Gauge.IsParadoxActive && Gauge.InAstralFire)
					{
						return Paradox;
					}

					if (ActionReady(Despair) && ActionReady(All.Swiftcast) && Gauge.InAstralFire
						&& !Gauge.IsParadoxActive && (LocalPlayer.CurrentMp == 1200))
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
				if ((actionID is Transpose or UmbralSoul) && IsEnabled(CustomComboPreset.BLM_UmbralTranspose))
				{
					if (Gauge.InAstralFire)
					{
						return Transpose;
					}

					if (Gauge.InUmbralIce)
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