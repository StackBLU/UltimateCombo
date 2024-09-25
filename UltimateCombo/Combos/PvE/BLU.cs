using Dalamud.Game.ClientState.Conditions;
using ECommons.DalamudServices;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvE
{
	internal static class BLU
	{
		public const byte JobID = 36;

		public const uint
			RoseOfDestruction = 23275,
			ShockStrike = 11429,
			FeatherRain = 11426,
			JKick = 18325,
			PeculiarLight = 11421,
			Eruption = 11427,
			SharpenedKnife = 11400,
			GlassDance = 11430,
			SonicBoom = 18308,
			Surpanakha = 18323,
			Nightbloom = 23290,
			MoonFlute = 11415,
			Whistle = 18309,
			Tingle = 23265,
			TripleTrident = 23264,
			MatraMagic = 23285,
			FinalSting = 11407,
			Bristle = 11393,
			PhantomFlurry = 23288,
			PerpetualRay = 18314,
			AngelWhisper = 18317,
			SongOfTorment = 11386,
			RamsVoice = 11419,
			Ultravibration = 23277,
			Devour = 18320,
			Pomcure = 18303,
			Gobskin = 18304,
			Offguard = 11411,
			BadBreath = 11388,
			MagicHammer = 18305,
			WhiteKnightsTour = 18310,
			BlackKnightsTour = 18311,
			PeripheralSynthesis = 23286,
			BasicInstinct = 23276,
			HydroPull = 23282,
			MustardBomb = 23279,
			AngelsSnack = 23272,
			WingedReprobation = 34576,
			SeaShanty = 34580,
			BeingMortal = 34582,
			BreathOfMagic = 34567,
			MortalFlame = 34579,
			PeatPelt = 34569,
			DeepClean = 34570,
			GoblinPunch = 34563,
			BloodDrain = 11395,
			SelfDestruct = 11408,
			ToadOil = 11410,
			MightyGuard = 11417,
			ChocoMeteor = 23284,
			Quasar = 18324,
			Missile = 11405;

		public static class Buffs
		{
			public const ushort
				MoonFlute = 1718,
				Bristle = 1716,
				WaningNocturne = 1727,
				PhantomFlurry = 2502,
				Tingle = 2492,
				AngelsSnack = 2495,
				Whistle = 2118,
				TankMimicry = 2124,
				DPSMimicry = 2125,
				HealerMimicry = 2126,
				BasicInstinct = 2498,
				ToadOil = 1737,
				MightyGuard = 1719,
				Devour = 2120,
				DeepClean = 3637,
				Gobskin = 2114,
				WingedReprobation = 3640;
		}

		public static class Debuffs
		{
			public const ushort
				Slow = 9,
				Bind = 13,
				Stun = 142,
				DeepFreeze = 1731,
				Offguard = 1717,
				Bleeding = 1714,
				Malodorous = 1715,
				MustardBomb = 2499,
				Conked = 2115,
				Lightheaded = 2501,
				MortalFlame = 3643,
				BreathOfMagic = 3712,
				PeatPelt = 3636;
		}

		public static class Maps
		{
			public const ushort
				Dragonskin = 558,
				Gazelle1 = 712,
				Thief = 725,
				Gazelle2 = 794,
				Zonure1 = 879,
				Zonure2 = 924;

			/*
			TheAquapolis = 558,
			TheLostCanalsOfUznair = 712,
			TheHiddenCanalsOfUznair = 725,
			TheShiftingAltarsOfUznair = 794,
			TheDungeonsOfLyheGhiah = 879,
			TheShiftingOubliettesOfLyheGhiah = 924;
			*/
		}

		public static class Config
		{
			public static UserInt
				BLU_ManaGain = new("BLU_ManaGain", 2000),
				BLU_TreasurePomcure = new("BLU_TreasurePomcure", 75),
				BLU_TreasureGobskin = new("BLU_TreasureGobskin", 5);
		}

		internal class BLU_MoonFluteOpener : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLU_MoonFluteOpener;

			protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
			{
				if (actionID is MoonFlute && IsEnabled(CustomComboPreset.BLU_MoonFluteOpener))
				{
					if (HasEffect(Buffs.PhantomFlurry))
					{
						return OriginalHook(11);
					}
					if (GetCooldownRemainingTime(PhantomFlurry) > 60 && HasEffect(Buffs.WingedReprobation) && IsNotEnabled(CustomComboPreset.BLU_MoonFluteOpener_DoTOpener))
					{
						return WingedReprobation;
					}
					if (GetCooldownRemainingTime(PhantomFlurry) > 20)
					{
						return PhantomFlurry;
					}
					if (!HasEffect(Buffs.MoonFlute) && actionID is MoonFlute)
					{
						if (!HasEffect(Buffs.Whistle) && actionID is MoonFlute && IsSpellActive(Whistle))
						{
							return Whistle;
						}

						if (!HasEffect(Buffs.Tingle) && actionID is MoonFlute && IsSpellActive(Tingle))
						{
							return Tingle;
						}

						if (IsOffCooldown(RoseOfDestruction) && actionID is MoonFlute && IsSpellActive(RoseOfDestruction))
						{
							return RoseOfDestruction;
						}
						if (IsSpellActive(MoonFlute))
						{
							return MoonFlute;
						}
					}

					if (IsOffCooldown(JKick) && actionID is MoonFlute && IsSpellActive(JKick) && WasLastSpell(MoonFlute))
					{
						return JKick;
					}

					if (IsOffCooldown(TripleTrident) && actionID is MoonFlute && IsSpellActive(TripleTrident) && WasLastAbility(JKick))
					{
						return TripleTrident;
					}

					if (IsOffCooldown(Nightbloom) && actionID is MoonFlute && IsSpellActive(Nightbloom) && WasLastSpell(TripleTrident))
					{
						return Nightbloom;
					}

					if (IsEnabled(CustomComboPreset.BLU_MoonFluteOpener_DoTOpener) && actionID is MoonFlute)
					{
						if (WasLastAbility(Nightbloom) && actionID is MoonFlute && IsSpellActive(Bristle) && !WasLastSpell(Bristle))
						{
							return Bristle;
						}
						if (IsOffCooldown(FeatherRain) && actionID is MoonFlute && IsSpellActive(FeatherRain) && WasLastSpell(Bristle))
						{
							return FeatherRain;
						}

						if (IsOffCooldown(SeaShanty) && actionID is MoonFlute && IsSpellActive(SeaShanty) && WasLastAbility(FeatherRain))
						{
							return SeaShanty;
						}
						if (!TargetHasEffectAny(Debuffs.BreathOfMagic) && !TargetHasEffectAny(Debuffs.MortalFlame) && actionID is MoonFlute)
						{
							if (!WasLastSpell(BreathOfMagic) && !WasLastSpell(MortalFlame) && actionID is MoonFlute)
							{
								if (IsSpellActive(BreathOfMagic) && actionID is MoonFlute && IsSpellActive(BreathOfMagic) && WasLastAbility(SeaShanty))
								{
									return BreathOfMagic;
								}

								if (IsSpellActive(MortalFlame) && actionID is MoonFlute && IsSpellActive(MortalFlame) && WasLastAbility(SeaShanty))
								{
									return MortalFlame;
								}
							}
						}
						if (IsOffCooldown(ShockStrike) && actionID is MoonFlute && IsSpellActive(ShockStrike)
							&& (WasLastSpell(BreathOfMagic) || WasLastSpell(MortalFlame)))
						{
							return ShockStrike;
						}

						if (WasLastAbility(ShockStrike) && actionID is MoonFlute && IsSpellActive(Bristle) && !WasLastSpell(Bristle))
						{
							return Bristle;
						}

						if (IsOffCooldown(All.Swiftcast) && WasLastSpell(Bristle) && actionID is MoonFlute)
						{
							return All.Swiftcast;
						}

						if (GetRemainingCharges(Surpanakha) > 0 && actionID is MoonFlute && IsSpellActive(Surpanakha))
						{
							return Surpanakha;
						}

						if (IsOffCooldown(MatraMagic) && actionID is MoonFlute && IsSpellActive(MatraMagic))
						{
							return MatraMagic;
						}

						if (IsOffCooldown(BeingMortal) && actionID is MoonFlute && IsSpellActive(BeingMortal))
						{
							return BeingMortal;
						}

						if (IsOffCooldown(PhantomFlurry) && actionID is MoonFlute && IsSpellActive(PhantomFlurry))
						{
							return PhantomFlurry;
						}
					}

					if (IsNotEnabled(CustomComboPreset.BLU_MoonFluteOpener_DoTOpener) && actionID is MoonFlute)
					{
						if (IsOffCooldown(WingedReprobation) && actionID is MoonFlute && IsSpellActive(WingedReprobation)
							&& !WasLastSpell(WingedReprobation) && !WasLastAbility(FeatherRain) && !HasEffect(Buffs.WingedReprobation))
						{
							return WingedReprobation;
						}

						if (IsOffCooldown(FeatherRain) && actionID is MoonFlute && IsSpellActive(FeatherRain) && WasLastSpell(WingedReprobation))
						{
							return FeatherRain;
						}

						if (IsOffCooldown(SeaShanty) && actionID is MoonFlute && IsSpellActive(SeaShanty) && WasLastAbility(FeatherRain))
						{
							return SeaShanty;
						}

						if (IsOffCooldown(WingedReprobation) && actionID is MoonFlute && IsSpellActive(WingedReprobation) && WasLastAbility(SeaShanty)
							&& FindEffect(Buffs.WingedReprobation).StackCount == 1)
						{
							return WingedReprobation;
						}

						if (IsOffCooldown(ShockStrike) && actionID is MoonFlute && IsSpellActive(ShockStrike) && WasLastSpell(WingedReprobation))
						{
							return ShockStrike;
						}

						if (IsOffCooldown(BeingMortal) && actionID is MoonFlute && IsSpellActive(BeingMortal) && WasLastAbility(ShockStrike))
						{
							return BeingMortal;
						}

						if (!HasEffect(Buffs.Bristle) && actionID is MoonFlute && IsSpellActive(Bristle) && WasLastAbility(BeingMortal))
						{
							return Bristle;
						}

						if (IsOffCooldown(All.Swiftcast) && WasLastSpell(Bristle) && actionID is MoonFlute)
						{
							return All.Swiftcast;
						}

						if (GetRemainingCharges(Surpanakha) > 0 && actionID is MoonFlute && IsSpellActive(Surpanakha))
						{
							return Surpanakha;
						}

						if (IsOffCooldown(MatraMagic) && actionID is MoonFlute && IsSpellActive(MatraMagic))
						{
							return MatraMagic;
						}

						if (IsOffCooldown(PhantomFlurry) && actionID is MoonFlute && IsSpellActive(PhantomFlurry))
						{
							return PhantomFlurry;
						}
					}
				}
				return actionID;
			}
		}

		internal class BLU_TripleTrident : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLU_TripleTrident;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (actionID is TripleTrident && IsEnabled(CustomComboPreset.BLU_TripleTrident) && IsSpellActive(TripleTrident))
				{
					if (GetCooldownRemainingTime(TripleTrident) > 3 && actionID is TripleTrident)
					{
						return TripleTrident;
					}
					if (!HasEffect(Buffs.Whistle) && actionID is TripleTrident && IsSpellActive(Whistle))
					{
						return Whistle;
					}
					if (!HasEffect(Buffs.Tingle) && actionID is TripleTrident && IsSpellActive(Tingle))
					{
						return Tingle;
					}
					if (HasEffect(Buffs.Whistle) && HasEffect(Buffs.Tingle) && actionID is TripleTrident)
					{
						return TripleTrident;
					}
				}
				return actionID;
			}
		}

		internal class BLU_Sting : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLU_Sting;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (actionID is FinalSting && IsEnabled(CustomComboPreset.BLU_Sting) && IsSpellActive(FinalSting))
				{
					if (!HasEffect(Buffs.Whistle) && actionID is FinalSting && IsSpellActive(Whistle))
					{
						return Whistle;
					}
					if (!TargetHasEffectAny(Debuffs.Offguard) && IsOffCooldown(Offguard) && actionID is FinalSting && IsSpellActive(Offguard))
					{
						return Offguard;
					}
					if (!HasEffect(Buffs.Tingle) && actionID is FinalSting && IsSpellActive(Tingle))
					{
						return Tingle;
					}
					if (!HasEffect(Buffs.BasicInstinct) && IsSpellActive(BasicInstinct) && actionID is FinalSting && HasCondition(ConditionFlag.BoundByDuty))
					{
						return BasicInstinct;
					}
					if (!HasEffect(Buffs.MoonFlute) && actionID is FinalSting && IsSpellActive(MoonFlute))
					{
						return MoonFlute;
					}
					if (IsOffCooldown(All.Swiftcast) && actionID is FinalSting)
					{
						return All.Swiftcast;
					}
					if (HasEffect(Buffs.Whistle) && HasEffect(Buffs.Tingle) && HasEffect(Buffs.MoonFlute) && actionID is FinalSting)
					{
						return FinalSting;
					}
				}
				return actionID;
			}
		}

		internal class BLU_Explode : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLU_Explode;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (actionID is SelfDestruct && IsEnabled(CustomComboPreset.BLU_Explode) && IsSpellActive(SelfDestruct))
				{
					if (!HasEffect(Buffs.ToadOil) && actionID is SelfDestruct && IsSpellActive(ToadOil))
					{
						return ToadOil;
					}
					if (!HasEffect(Buffs.Bristle) && actionID is SelfDestruct && IsSpellActive(Bristle))
					{
						return Bristle;
					}
					if (!HasEffect(Buffs.MoonFlute) && actionID is SelfDestruct && IsSpellActive(MoonFlute))
					{
						return MoonFlute;
					}
					if (IsOffCooldown(All.Swiftcast) && actionID is SelfDestruct)
					{
						return All.Swiftcast;
					}
					if (HasEffect(Buffs.ToadOil) && HasEffect(Buffs.Bristle) && HasEffect(Buffs.MoonFlute) && actionID is SelfDestruct)
					{
						return SelfDestruct;
					}
				}
				return actionID;
			}
		}

		internal class BLU_DoTs : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLU_DoTs;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (actionID is Bristle && IsEnabled(CustomComboPreset.BLU_DoTs) && !HasEffect(Buffs.MoonFlute))
				{
					if (!HasEffect(Buffs.Bristle) && IsSpellActive(Bristle) && actionID is Bristle)
					{
						return Bristle;
					}
					if (IsSpellActive(BreathOfMagic) && actionID is Bristle
						&& (!TargetHasEffectAny(Debuffs.BreathOfMagic) || GetDebuffRemainingTime(Debuffs.BreathOfMagic) < 3))
					{
						return BreathOfMagic;
					}
					if (IsSpellActive(MortalFlame) && !TargetHasEffectAny(Debuffs.MortalFlame) && actionID is Bristle)
					{
						return MortalFlame;
					}
					if (IsSpellActive(SongOfTorment) && actionID is Bristle
						&& (!TargetHasEffectAny(Debuffs.Bleeding) || GetDebuffRemainingTime(Debuffs.Bleeding) < 3))
					{
						return SongOfTorment;
					}
				}
				return actionID;
			}
		}

		internal class BLU_Periph : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLU_Periph;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is PeripheralSynthesis or MustardBomb) && IsEnabled(CustomComboPreset.BLU_Periph))
				{
					if (IsSpellActive(MustardBomb)
						&& (WasLastSpell(PeripheralSynthesis) || HasEffect(Buffs.Bristle)
						|| TargetHasEffectAny(Debuffs.MustardBomb) || TargetHasEffectAny(Debuffs.Lightheaded)))
					{
						return MustardBomb;
					}
					return PeripheralSynthesis;
				}
				return actionID;
			}
		}

		internal class BLU_Ultravibration : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLU_Ultravibration;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is HydroPull or RamsVoice or Ultravibration) && IsEnabled(CustomComboPreset.BLU_Ultravibration))
				{
					if (!InCombat())
					{
						return Ultravibration;
					}

					if (IsSpellActive(HydroPull) && !WasLastSpell(HydroPull) && !WasLastSpell(RamsVoice) && !TargetHasEffectAny(Debuffs.DeepFreeze))
					{
						return HydroPull;
					}

					if (IsSpellActive(RamsVoice) && !TargetHasEffectAny(Debuffs.DeepFreeze) && (WasLastSpell(HydroPull) || !IsSpellActive(HydroPull)))
					{
						return RamsVoice;
					}

					if (WasLastSpell(RamsVoice) && IsOffCooldown(All.Swiftcast) && IsOffCooldown(Ultravibration))
					{
						return All.Swiftcast;
					}

					if (IsSpellActive(Ultravibration) && WasLastSpell(RamsVoice))
					{
						return Ultravibration;
					}
				}

				return actionID;
			}
		}

		internal class BLU_ManaGain : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLU_ManaGain;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is GoblinPunch or SonicBoom or ChocoMeteor) && IsEnabled(CustomComboPreset.BLU_ManaGain)
					&& !HasEffect(Buffs.PhantomFlurry))
				{
					if (LocalPlayer.CurrentMp <= GetOptionValue(Config.BLU_ManaGain) && IsSpellActive(BloodDrain))
					{
						return BloodDrain;
					}
				}
				return actionID;
			}
		}

		internal class BLU_Tanking : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLU_Tanking;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (actionID is GoblinPunch && IsEnabled(CustomComboPreset.BLU_Tanking) && HasEffect(Buffs.TankMimicry) && !HasEffect(Buffs.PhantomFlurry))
				{
					if (!HasEffect(Buffs.MightyGuard) && IsSpellActive(MightyGuard))
					{
						return MightyGuard;
					}
					if (!HasEffect(Buffs.ToadOil) && IsSpellActive(ToadOil) && !WasLastSpell(ToadOil))
					{
						return ToadOil;
					}
					if (IsOffCooldown(Devour) & InActionRange(Devour) && IsSpellActive(Devour))
					{
						return Devour;
					}
					if (IsOffCooldown(PeculiarLight) & InMeleeRange() && IsSpellActive(PeculiarLight))
					{
						return PeculiarLight;
					}
					if (!TargetHasEffect(Debuffs.PeatPelt) && !HasEffect(Buffs.DeepClean) && !WasLastSpell(PeatPelt) && IsSpellActive(PeatPelt))
					{
						return PeatPelt;
					}
					if ((WasLastSpell(PeatPelt) || TargetHasEffect(Debuffs.PeatPelt)) && IsSpellActive(DeepClean))
					{
						return DeepClean;
					}
					if (HasEffect(Buffs.DeepClean) && IsSpellActive(GoblinPunch))
					{
						return GoblinPunch;
					}
				}
				return actionID;
			}
		}

		internal class BLU_PhantomEnder : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLU_PhantomEnder;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is GoblinPunch or SonicBoom) && IsEnabled(CustomComboPreset.BLU_PhantomEnder))
				{
					if (HasEffect(Buffs.PhantomFlurry))
					{
						if (GetBuffRemainingTime(Buffs.PhantomFlurry) <= 0.75f)
						{
							return OriginalHook(PhantomFlurry);
						}
						return OriginalHook(11);
					}
				}
				return actionID;
			}
		}

		internal class BLU_Treasure : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLU_Treasure;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				bool notInMap = Svc.ClientState.TerritoryType is not Maps.Dragonskin and not Maps.Gazelle1
					and not Maps.Gazelle2 and not Maps.Thief and not Maps.Zonure1 and not Maps.Zonure2;

				bool inMap = Svc.ClientState.TerritoryType is Maps.Dragonskin or Maps.Gazelle1
					or Maps.Gazelle2 or Maps.Thief or Maps.Zonure1 or Maps.Zonure2;

				if (actionID is GoblinPunch && IsEnabled(CustomComboPreset.BLU_Treasure) && HasEffect(Buffs.HealerMimicry)
					&& !HasEffect(Buffs.PhantomFlurry))
				{
					if (notInMap)
					{
						if (IsEnabled(CustomComboPreset.BLU_Treasure_MightyGuard) && HasEffect(Buffs.MightyGuard))
						{
							return MightyGuard;
						}

						if (IsEnabled(CustomComboPreset.BLU_Treasure_AutoSpell)
							&& CurrentTarget == null && !InCombat() && !IsCasting()
							&& IsOffCooldown(ShockStrike) && IsOffCooldown(GlassDance) && IsOffCooldown(Quasar) && IsOffCooldown(SeaShanty)
							&& (!IsSpellActive(RamsVoice) || !IsSpellActive(Missile)
							|| !IsSpellActive(Ultravibration) || !IsSpellActive(HydroPull)))
						{
							for (int i = 0; i < 24; i++)
							{
								if (GetActiveBlueMageActionInSlot(20) == ShockStrike)
								{
									if (IsOffCooldown(ShockStrike) && IsSpellActive(ShockStrike))
									{
										AssignBlueMageActionToSlot(20, RamsVoice);
									}
								}

								if (GetActiveBlueMageActionInSlot(21) == GlassDance)
								{
									if (IsOffCooldown(GlassDance) && IsSpellActive(GlassDance))
									{
										AssignBlueMageActionToSlot(21, Missile);
									}
								}

								if (GetActiveBlueMageActionInSlot(22) == Quasar)
								{
									if (IsOffCooldown(Quasar) && IsSpellActive(Quasar))
									{
										AssignBlueMageActionToSlot(22, Ultravibration);
									}
								}

								if (GetActiveBlueMageActionInSlot(23) == SeaShanty)
								{
									if (IsOffCooldown(SeaShanty) && IsSpellActive(SeaShanty))
									{
										AssignBlueMageActionToSlot(23, HydroPull);
									}
								}
							}
						}

						if (!IsSpellActive(RamsVoice) || !IsSpellActive(Missile) || !IsSpellActive(Ultravibration) || !IsSpellActive(HydroPull))
						{
							return OriginalHook(11);
						}

						if (IsEnabled(CustomComboPreset.BLU_Treasure_AngelsSnack)
							&& PlayerHealthPercentageHp() < GetOptionValue(Config.BLU_TreasurePomcure) && IsOffCooldown(AngelsSnack)
							&& IsSpellActive(AngelsSnack))
						{
							if (ActionReady(All.Swiftcast))
							{
								return All.Swiftcast;
							}

							return AngelsSnack;
						}
					}

					if (inMap)
					{
						if (IsEnabled(CustomComboPreset.BLU_Treasure_BasicInstinct) && !HasEffect(Buffs.BasicInstinct))
						{
							return BasicInstinct;
						}

						if (IsEnabled(CustomComboPreset.BLU_Treasure_MightyGuard) && !HasEffect(Buffs.MightyGuard))
						{
							return MightyGuard;
						}

						if (IsEnabled(CustomComboPreset.BLU_Treasure_AutoSpell)
							&& CurrentTarget == null && !InCombat() && !IsCasting() && Svc.DutyState.IsDutyStarted
							&& IsOffCooldown(RamsVoice) && IsOffCooldown(Missile) && IsOffCooldown(Ultravibration) && IsOffCooldown(HydroPull)
							&& (!IsSpellActive(ShockStrike) || !IsSpellActive(GlassDance)
							|| !IsSpellActive(Quasar) || !IsSpellActive(SeaShanty)))
						{
							for (int i = 0; i < 24; i++)
							{
								if (GetActiveBlueMageActionInSlot(20) == RamsVoice)
								{
									if (IsOffCooldown(RamsVoice) && IsSpellActive(RamsVoice))
									{
										AssignBlueMageActionToSlot(20, ShockStrike);
									}
								}


								if (GetActiveBlueMageActionInSlot(21) == Missile)
								{
									if (IsOffCooldown(Missile) && IsSpellActive(Missile))
									{
										AssignBlueMageActionToSlot(21, GlassDance);
									}
								}

								if (GetActiveBlueMageActionInSlot(22) == Ultravibration)
								{
									if (IsOffCooldown(Ultravibration) && IsSpellActive(Ultravibration))
									{
										AssignBlueMageActionToSlot(22, Quasar);
									}
								}

								if (GetActiveBlueMageActionInSlot(23) == HydroPull)
								{
									if (IsOffCooldown(HydroPull) && IsSpellActive(HydroPull))
									{
										AssignBlueMageActionToSlot(23, SeaShanty);
									}
								}
							}
						}

						if (IsEnabled(CustomComboPreset.BLU_Treasure_AngelsSnack)
							&& PlayerHealthPercentageHp() < GetOptionValue(Config.BLU_TreasurePomcure)
							&& ActionReady(AngelsSnack) && IsSpellActive(AngelsSnack))
						{
							if (PlayerHealthPercentageHp() < 50 && ActionReady(All.Swiftcast))
							{
								return All.Swiftcast;
							}

							return AngelsSnack;
						}

						if (IsEnabled(CustomComboPreset.BLU_Treasure_Pomcure)
							&& PlayerHealthPercentageHp() < GetOptionValue(Config.BLU_TreasurePomcure)
							&& IsOnCooldown(AngelsSnack) && IsSpellActive(Pomcure)
							&& !HasEffect(Buffs.AngelsSnack))
						{
							if (PlayerHealthPercentageHp() < 50 && ActionReady(All.Swiftcast))
							{
								return All.Swiftcast;
							}

							return Pomcure;
						}

						if (IsEnabled(CustomComboPreset.BLU_Treasure_Gobskin)
							&& LocalPlayer.ShieldPercentage <= GetOptionValue(Config.BLU_TreasureGobskin) && IsSpellActive(Gobskin))
						{
							return Gobskin;
						}

						if (IsEnabled(CustomComboPreset.BLU_Treasure_FeatherRain) && IsOffCooldown(FeatherRain)
							&& CanWeave(GoblinPunch) && IsSpellActive(FeatherRain))
						{
							return FeatherRain;
						}

						if (IsEnabled(CustomComboPreset.BLU_Treasure_BreathOfMagic) && EnemyHealthMaxHp() >= LocalPlayer.MaxHp * 10
							&& !TargetHasEffect(Debuffs.BreathOfMagic) && !WasLastSpell(BreathOfMagic)
							&& (EnemyHealthCurrentHp() / EnemyHealthMaxHp() * 100) > 10)
						{
							if (!HasEffect(Buffs.Bristle))
							{
								return Bristle;
							}

							return BreathOfMagic;
						}

						if (IsEnabled(CustomComboPreset.BLU_Treasure_MortalFlame) && EnemyHealthMaxHp() >= LocalPlayer.MaxHp * 10
							&& !TargetHasEffect(Debuffs.MortalFlame) && !WasLastSpell(MortalFlame)
							&& (EnemyHealthCurrentHp() / EnemyHealthMaxHp() * 100) > 10)
						{
							if (!HasEffect(Buffs.Bristle))
							{
								return Bristle;
							}

							return MortalFlame;
						}

						if (IsEnabled(CustomComboPreset.BLU_Treasure_TripleTrident) && IsSpellActive(TripleTrident)
							&& EnemyHealthMaxHp() >= LocalPlayer.MaxHp * 10
							&& (IsOffCooldown(TripleTrident) || GetCooldownRemainingTime(TripleTrident) < 5) && !WasLastSpell(TripleTrident)
							&& (EnemyHealthCurrentHp() / EnemyHealthMaxHp() * 100) > 10)
						{
							if (!HasEffect(Buffs.Whistle))
							{
								return Whistle;
							}

							if (!HasEffect(Buffs.Tingle))
							{
								return Tingle;
							}

							return TripleTrident;
						}
					}
				}

				return actionID;
			}
		}
	}
}