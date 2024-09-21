using Dalamud.Game.ClientState.JobGauge.Enums;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Combos.PvE.Content;
using UltimateCombo.CustomCombo;
using UltimateCombo.Data;

namespace UltimateCombo.Combos.PvE
{
	internal class All
	{
		public const byte JobID = 0;

		public const uint
			Rampart = 7531,
			SecondWind = 7541,
			TrueNorth = 7546,
			Addle = 7560,
			Swiftcast = 7561,
			LucidDreaming = 7562,
			Resurrection = 173,
			Raise = 125,
			Provoke = 7533,
			Shirk = 7537,
			Reprisal = 7535,
			Esuna = 7568,
			Rescue = 7571,
			SolidReason = 232,
			AgelessWords = 215,
			Sleep = 25880,
			WiseToTheWorldMIN = 26521,
			WiseToTheWorldBTN = 26522,
			LowBlow = 7540,
			Bloodbath = 7542,
			HeadGraze = 7551,
			FootGraze = 7553,
			LegGraze = 7554,
			Feint = 7549,
			Interject = 7538,
			Peloton = 7557,
			LegSweep = 7863,
			Repose = 16560,
			Sprint = 3,
			ArmsLength = 7548;

		public static class Buffs
		{
			public const ushort
				Weakness = 43,
				Medicated = 49,
				Bloodbath = 84,
				Swiftcast = 167,
				Rampart = 1191,
				Peloton = 1199,
				LucidDreaming = 1204,
				TrueNorth = 1250,
				Sprint = 50,
				Raise1 = 148,
				Raise2 = 1140;
		}

		public static class Debuffs
		{
			public const ushort
				Sleep = 3,
				Bind = 13,
				Heavy = 14,
				Addle = 1203,
				Reprisal = 1193,
				Feint = 1195;
		}

		public static class Config
		{
			public static UserInt
				All_SecondWind = new("All_SecondWind", 50),
				All_Bloodbath = new("All_Bloodbath", 25),
				All_Healer_Lucid = new("All_Healer_Lucid", 7500),
				All_Mage_Lucid = new("All_Mage_Lucid", 7500),
				All_BLU_Lucid = new("All_BLU_Lucid", 7500),
				All_Variant_Cure = new("All_Variant_Cure", 50);

			public static UserBool
				All_BLM_Lucid = new("All_BLM_Lucid", false);
		}

		internal class All_Tank_Reprisal : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.All_Tank_Reprisal;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (actionID is Reprisal && IsEnabled(CustomComboPreset.All_Tank_Reprisal))
				{
					if (TargetHasEffectAny(Debuffs.Reprisal) && IsOffCooldown(Reprisal))
					{
						return OriginalHook(11);
					}
				}

				return actionID;
			}
		}

		internal class All_Caster_Addle : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.All_Caster_Addle;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (actionID is Addle && IsEnabled(CustomComboPreset.All_Caster_Addle))
				{
					if (TargetHasEffectAny(Debuffs.Addle) && IsOffCooldown(Addle))
					{
						return OriginalHook(11);
					}
				}

				return actionID;
			}
		}

		internal class All_Melee_Feint : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.All_Melee_Feint;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (actionID is Feint && IsEnabled(CustomComboPreset.All_Melee_Feint))
				{
					if (TargetHasEffectAny(Debuffs.Feint) && IsOffCooldown(Feint))
					{
						return OriginalHook(11);
					}
				}

				return actionID;
			}
		}

		internal class All_Melee_TrueNorth : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.All_Melee_TrueNorth;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (actionID is TrueNorth && IsEnabled(CustomComboPreset.All_Melee_TrueNorth))
				{
					if (HasEffect(Buffs.TrueNorth))
					{
						return OriginalHook(11);
					}
				}

				return actionID;
			}
		}

		internal class All_Ranged_Mitigation : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.All_Ranged_Mitigation;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is BRD.Troubadour or MCH.Tactician or DNC.ShieldSamba) && IsEnabled(CustomComboPreset.All_Ranged_Mitigation))
				{
					if ((HasEffectAny(BRD.Buffs.Troubadour) || HasEffectAny(MCH.Buffs.Tactician)
						|| HasEffectAny(DNC.Buffs.ShieldSamba)) && IsOffCooldown(actionID))
					{
						return OriginalHook(11);
					}
				}

				return actionID;
			}
		}

		internal class All_Raise_Protection : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.All_Raise_Protection;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is WHM.Raise or SCH.Resurrection or AST.Ascend or SGE.Egeiro
					or SMN.Resurrection or RDM.Verraise or BLU.AngelWhisper)
					&& IsEnabled(CustomComboPreset.All_Raise_Protection))
				{
					if (TargetHasEffectAny(Buffs.Raise1) || TargetHasEffectAny(Buffs.Raise2))
					{
						return OriginalHook(11);
					}
				}

				return actionID;
			}
		}

		internal class All_RoleActions : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.All_RoleActions;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.All_RoleActions) && ActionWatching.NumberOfGcdsUsed >= 15)
				{
					if (CanDelayedWeave(actionID))
					{
						if (IsEnabled(CustomComboPreset.All_TrueNorth) && ActionReady(TrueNorth) && IsEnabled(TrueNorth)
							&& TargetNeedsPositionals() && !HasEffect(Buffs.TrueNorth))
						{
							if (LocalPlayer.ClassJob.Id == MNK.JobID)
							{
								if (HasEffect(MNK.Buffs.CoeurlForm) && MNK.Gauge.CoeurlFury == 0 && LevelChecked(MNK.Demolish)
									&& !OnTargetsRear())
								{
									return TrueNorth;
								}

								if (HasEffect(MNK.Buffs.CoeurlForm) && (MNK.Gauge.CoeurlFury >= 1 || !LevelChecked(MNK.Demolish))
									&& !OnTargetsFlank())
								{
									return TrueNorth;
								}
							}

							if (LocalPlayer.ClassJob.Id == DRG.JobID)
							{
								if ((lastComboMove is DRG.Disembowel or DRG.SpiralBlow or DRG.ChaosThrust or DRG.ChaoticSpring)
									&& LevelChecked(DRG.ChaosThrust) && !OnTargetsRear())
								{
									return TrueNorth;
								}

								if ((lastComboMove is DRG.FullThrust or DRG.HeavensThrust)
									&& LevelChecked(DRG.FangAndClaw) && !OnTargetsFlank())
								{
									return TrueNorth;
								}
							}

							if (LocalPlayer.ClassJob.Id == NIN.JobID)
							{
								if (lastComboMove is NIN.GustSlash)
								{
									if (ActionReady(NIN.AeolianEdge) && !OnTargetsRear()
										&& ((NIN.Gauge.Kazematoi >= 1
										&& (TargetHasEffect(NIN.TrickList[OriginalHook(NIN.TrickAttack)])
										|| TargetHasEffect(NIN.MugList[OriginalHook(NIN.Mug)])
										|| (EnemyHealthCurrentHp() <= LocalPlayer.MaxHp * 10 && EnemyHealthCurrentHp() != 44)))
										|| NIN.Gauge.Kazematoi > 3 || !LevelChecked(NIN.ArmorCrush)))
									{
										return TrueNorth;
									}

									if (ActionReady(NIN.ArmorCrush) && !OnTargetsFlank())
									{
										return TrueNorth;
									}
								}
							}

							if (LocalPlayer.ClassJob.Id == SAM.JobID)
							{
								if (HasEffect(SAM.Buffs.MeikyoShisui)
									|| (!HasEffect(SAM.Buffs.MeikyoShisui) && lastComboMove is SAM.Jinpu && !OnTargetsRear()))
								{
									return TrueNorth;
								}

								if (HasEffect(SAM.Buffs.MeikyoShisui)
									|| (!HasEffect(SAM.Buffs.MeikyoShisui) && lastComboMove is SAM.Shifu && !OnTargetsFlank()))
								{
									return TrueNorth;
								}
							}

							if (LocalPlayer.ClassJob.Id == RPR.JobID)
							{
								if (HasEffect(RPR.Buffs.SoulReaver) || HasEffect(RPR.Buffs.Executioner))
								{
									if (HasEffect(RPR.Buffs.EnhancedGibbet) && !OnTargetsRear())
									{
										return TrueNorth;
									}

									if (HasEffect(RPR.Buffs.EnhancedGallows) && !OnTargetsFlank())
									{
										return TrueNorth;
									}
								}
							}

							if (LocalPlayer.ClassJob.Id == VPR.JobID)
							{
								if (VPR.Gauge.DreadCombo.HasFlag(DreadCombo.Dreadwinder))
								{
									if (GetBuffRemainingTime(VPR.Buffs.Swiftscaled) <= GetBuffRemainingTime(VPR.Buffs.HuntersInstinct)
										&& !OnTargetsRear())
									{
										return TrueNorth;
									}

									if (GetBuffRemainingTime(VPR.Buffs.HuntersInstinct) < GetBuffRemainingTime(VPR.Buffs.Swiftscaled)
										&& !OnTargetsFlank())
									{
										return TrueNorth;
									}
								}

								if ((HasEffect(VPR.Buffs.HindstungVenom) || HasEffect(VPR.Buffs.HindsbaneVenom))
									&& (lastComboMove is VPR.HuntersSting or VPR.SwiftskinsSting) && !OnTargetsRear())
								{
									return TrueNorth;
								}

								if ((HasEffect(VPR.Buffs.FlankstungVenom) || HasEffect(VPR.Buffs.FlanksbaneVenom))
									&& (lastComboMove is VPR.HuntersSting or VPR.SwiftskinsSting) && !OnTargetsFlank())
								{
									return TrueNorth;
								}
							}
						}
					}

					if (CanWeave(actionID) && !HasEffect(NIN.Buffs.Mudra))
					{
						if (IsEnabled(CustomComboPreset.All_SecondWind) && ActionReady(SecondWind) && IsEnabled(SecondWind)
							&& PlayerHealthPercentageHp() <= GetOptionValue(Config.All_SecondWind))
						{
							return SecondWind;
						}

						if (IsEnabled(CustomComboPreset.All_Bloodbath) && ActionReady(Bloodbath) && IsEnabled(Bloodbath)
							&& PlayerHealthPercentageHp() <= GetOptionValue(Config.All_Bloodbath))
						{
							return Bloodbath;
						}

						if (IsEnabled(CustomComboPreset.All_Healer_Lucid) && ActionReady(LucidDreaming) && IsEnabled(LucidDreaming)
							&& (LocalPlayer.CurrentMp <= GetOptionValue(Config.All_Healer_Lucid) || LocalPlayer.CurrentMp <= 1000)
							&& (LocalPlayer.ClassJob.Id == WHM.JobID
							|| LocalPlayer.ClassJob.Id == SCH.JobID
							|| LocalPlayer.ClassJob.Id == AST.JobID
							|| LocalPlayer.ClassJob.Id == SGE.JobID))
						{
							return LucidDreaming;
						}

						if (IsEnabled(CustomComboPreset.All_Mage_Lucid) && ActionReady(LucidDreaming) && IsEnabled(LucidDreaming)
							&& (LocalPlayer.CurrentMp <= GetOptionValue(Config.All_Mage_Lucid) || LocalPlayer.CurrentMp <= 1000)
							&& (LocalPlayer.ClassJob.Id == SMN.JobID
							|| LocalPlayer.ClassJob.Id == RDM.JobID
							|| LocalPlayer.ClassJob.Id == PCT.JobID))
						{
							return LucidDreaming;
						}

						if (IsEnabled(CustomComboPreset.All_BLU_Lucid) && ActionReady(LucidDreaming) && IsEnabled(LucidDreaming)
							&& (LocalPlayer.CurrentMp <= GetOptionValue(Config.All_BLU_Lucid) || LocalPlayer.CurrentMp <= 1000)
							&& LocalPlayer.ClassJob.Id == BLU.JobID)
						{
							return LucidDreaming;
						}

						if (IsEnabled(CustomComboPreset.All_ArmsLength) && ActionReady(ArmsLength) && IsEnabled(ArmsLength))
						{
							return ArmsLength;
						}
					}
				}

				return actionID;
			}
		}

		internal class All_Variant : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.All_Variant;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.All_Variant))
				{
					if (IsEnabled(CustomComboPreset.All_Variant_Cure) && IsEnabled(Variant.VariantCure)
						&& PlayerHealthPercentageHp() <= GetOptionValue(Config.All_Variant_Cure))
					{
						return Variant.VariantCure;
					}

					if (CanWeave(actionID))
					{
						if (IsEnabled(CustomComboPreset.All_Variant_Ultimatum) && IsEnabled(Variant.VariantUltimatum)
						&& ActionReady(Variant.VariantUltimatum))
						{
							return Variant.VariantUltimatum;
						}

						if (IsEnabled(CustomComboPreset.All_Variant_SpiritDart) && IsEnabled(Variant.VariantSpiritDart)
							&& !TargetHasEffectAny(Variant.Debuffs.SustainedDamage))
						{
							return Variant.VariantSpiritDart;
						}

						if (IsEnabled(CustomComboPreset.All_Variant_Rampart) && IsEnabled(Variant.VariantRampart)
							&& ActionReady(Variant.VariantRampart))
						{
							return Variant.VariantRampart;
						}

					}
				}

				return actionID;
			}
		}
	}
}