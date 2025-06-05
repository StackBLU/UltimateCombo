using Dalamud.Game.ClientState.JobGauge.Types;
using System.Linq;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Combos.PvE.Content;
using UltimateCombo.CustomCombo;
using UltimateCombo.Data;
using UltimateCombo.Services;

namespace UltimateCombo.Combos.PvE
{
	internal static class WAR
	{
		public const byte JobID = 21;

		public const uint
			HeavySwing = 31,
			Maim = 37,
			Berserk = 38,
			ThrillOfBattle = 40,
			Overpower = 41,
			StormsPath = 42,
			Holmgang = 43,
			StormsEye = 45,
			Tomahawk = 46,
			InnerBeast = 49,
			SteelCyclone = 51,
			Infuriate = 52,
			FellCleave = 3549,
			Decimate = 3550,
			Upheaval = 7387,
			InnerRelease = 7389,
			RawIntuition = 3551,
			MythrilTempest = 16462,
			ChaoticCyclone = 16463,
			NascentFlash = 16464,
			InnerChaos = 16465,
			Orogeny = 25752,
			PrimalRend = 25753,
			PrimalWrath = 36924,
			PrimalRuination = 36925,
			Onslaught = 7386,
			ShakeItOff = 7388;

		public static class Buffs
		{
			public const ushort
				InnerRelease = 1177,
				SurgingTempest = 2677,
				NascentChaos = 1897,
				PrimalRendReady = 2624,
				Wrathful = 3901,
				PrimalRuinationReady = 3834,
				BurgeoningFury = 3833,
				Berserk = 86,
				ThrillOfBattle = 87;
		}

		private static WARGauge Gauge
		{
			get
			{
				return CustomComboFunctions.GetJobGauge<WARGauge>();
			}
		}

		public static class Config
		{
			public static UserInt
				WAR_SurgingRefresh = new("WAR_SurgingRefresh", 20),
				WAR_FellCleaveGauge = new("WAR_FellCleaveGauge", 50),
				WAR_DecimateGauge = new("WAR_DecimateGauge", 50),
				WAR_ST_Invuln = new("WAR_ST_Invuln", 10),
				WAR_AoE_Invuln = new("WAR_AoE_Invuln", 10);
		}

		internal class WAR_ST_DPS : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.WAR_ST_DPS;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is HeavySwing or Maim or StormsPath or StormsEye) && IsEnabled(CustomComboPreset.WAR_ST_DPS))
				{
					if (IsEnabled(CustomComboPreset.WAR_ST_Invuln) && PlayerHealthPercentageHp() <= GetOptionValue(Config.WAR_ST_Invuln)
						&& ActionReady(Holmgang))
					{
						return Holmgang;
					}

					if (IsEnabled(CustomComboPreset.WAR_ST_Tomahawk) && !InCombat() && ActionReady(Tomahawk) && !InMeleeRange())
					{
						return Tomahawk;
					}

					if (CanWeave(actionID) && !HasEffect(Bozja.Buffs.BloodRage))
					{
						if (IsEnabled(CustomComboPreset.WAR_ST_Infuriate) && ActionReady(Infuriate)
							&& Gauge.BeastGauge <= 50 && !WasLastAbility(Infuriate)
							&& !HasEffect(Buffs.NascentChaos) && !HasEffect(Buffs.InnerRelease)
							&& !HasEffect(Buffs.PrimalRendReady) && !HasEffect(Buffs.PrimalRuinationReady)
							&& (HasEffect(Bozja.Buffs.BloodRush) || !HasEffect(Bozja.Buffs.Reminiscence)))
						{
							return Infuriate;
						}

						if ((GetBuffRemainingTime(Buffs.SurgingTempest) > GetOptionValue(Config.WAR_SurgingRefresh) || !LevelChecked(StormsEye))
							&& (ActionWatching.NumberOfGcdsUsed >= 4 || Service.Configuration.IgnoreGCDChecks))
						{
							if (IsEnabled(CustomComboPreset.WAR_ST_InnerRelease) && ActionReady(OriginalHook(InnerRelease)))
							{
								return OriginalHook(InnerRelease);
							}

							if (IsEnabled(CustomComboPreset.WAR_ST_Onslaught) && ActionReady(Onslaught)
								&& InMeleeRangeNoMovement() && !WasLastAbility(Onslaught))
							{
								return Onslaught;
							}

							if (IsEnabled(CustomComboPreset.WAR_ST_Upheaval) && ActionReady(Upheaval) & InActionRange(Upheaval))
							{
								return Upheaval;
							}

							if (IsEnabled(CustomComboPreset.WAR_ST_Bloodwhetting) && ActionReady(OriginalHook(RawIntuition)))
							{
								if (CurrentTarget.TargetObject != LocalPlayer && GetPartyMembers().Any(x => x.GameObject == CurrentTarget.TargetObject))
								{
									return OriginalHook(NascentFlash);
								}

								if (CurrentTarget.TargetObject == LocalPlayer)
								{
									return OriginalHook(RawIntuition);
								}
							}
						}
					}

					if (IsEnabled(CustomComboPreset.WAR_ST_FellCleave) && ActionReady(OriginalHook(FellCleave))
						&& (ActionWatching.NumberOfGcdsUsed >= 2 || Service.Configuration.IgnoreGCDChecks)
						&& (GetBuffRemainingTime(Buffs.SurgingTempest) > GetOptionValue(Config.WAR_SurgingRefresh) || !LevelChecked(StormsEye))
						&& (HasEffect(Buffs.InnerRelease) || HasEffect(Buffs.NascentChaos)
						|| (Gauge.BeastGauge >= GetOptionValue(Config.WAR_FellCleaveGauge)
						&& (LevelChecked(InnerRelease)
						|| ((HasEffect(Buffs.Berserk) || (!HasEffect(Buffs.Berserk)
						&& !LevelChecked(InnerRelease))) && Gauge.BeastGauge >= 50)))))
					{
						return OriginalHook(FellCleave);
					}

					if (IsEnabled(CustomComboPreset.WAR_ST_PrimalRend) && HasEffect(Buffs.PrimalRuinationReady))
					{
						return PrimalRuination;
					}

					if (IsEnabled(CustomComboPreset.WAR_ST_PrimalRend) && HasEffect(Buffs.PrimalRendReady))
					{
						return PrimalRend;
					}

					if (IsEnabled(CustomComboPreset.WAR_ST_Tomahawk) && ActionReady(Tomahawk) && OutOfMeleeRange())
					{
						return Tomahawk;
					}

					if (comboTime > 0)
					{
						if (lastComboMove is HeavySwing && ActionReady(Maim))
						{
							return Maim;
						}

						if (lastComboMove is Maim && ActionReady(StormsPath) && IsEnabled(CustomComboPreset.WAR_ST_StormsEye))
						{
							if (ActionReady(StormsEye)
								&& GetBuffRemainingTime(Buffs.SurgingTempest) <= GetOptionValue(Config.WAR_SurgingRefresh))
							{
								return StormsEye;
							}
							if (ActionReady(StormsPath))
							{
								return StormsPath;
							}
						}
					}

					return HeavySwing;
				}

				return actionID;
			}
		}

		internal class WAR_AoE_DPS : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.WAR_AoE_DPS;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Overpower or MythrilTempest) && IsEnabled(CustomComboPreset.WAR_AoE_DPS))
				{
					if (IsEnabled(CustomComboPreset.WAR_AoE_Invuln) && PlayerHealthPercentageHp() <= GetOptionValue(Config.WAR_AoE_Invuln) && ActionReady(Holmgang))
					{
						return Holmgang;
					}

					if (CanWeave(actionID))
					{
						if (IsEnabled(CustomComboPreset.WAR_AoE_Infuriate) && ActionReady(Infuriate)
							&& Gauge.BeastGauge <= 50 && !WasLastAbility(Infuriate)
							&& !HasEffect(Buffs.NascentChaos) && !HasEffect(Buffs.InnerRelease)
							&& !HasEffect(Buffs.PrimalRendReady) && !HasEffect(Buffs.PrimalRuinationReady))
						{
							return Infuriate;
						}

						if (IsEnabled(CustomComboPreset.WAR_AoE_Onslaught) && ActionReady(Onslaught) && !InMeleeRange()
							&& !InCombat())
						{
							return Onslaught;
						}

						if (GetBuffRemainingTime(Buffs.SurgingTempest) > GetOptionValue(Config.WAR_SurgingRefresh) || !LevelChecked(StormsEye))
						{
							if (IsEnabled(CustomComboPreset.WAR_AoE_InnerRelease) && ActionReady(OriginalHook(InnerRelease)))
							{
								return OriginalHook(InnerRelease);
							}

							if (IsEnabled(CustomComboPreset.WAR_AoE_Onslaught) && ActionReady(Onslaught)
								&& InMeleeRangeNoMovement() && !WasLastAbility(Onslaught))
							{
								return Onslaught;
							}

							if (IsEnabled(CustomComboPreset.WAR_AoE_Orogeny) && ActionReady(Orogeny) & InActionRange(Orogeny))
							{
								return Orogeny;
							}

							if (IsEnabled(CustomComboPreset.WAR_ST_Bloodwhetting) && ActionReady(OriginalHook(RawIntuition)))
							{
								if (CurrentTarget.TargetObject != LocalPlayer && GetPartyMembers().Any(x => x.GameObject == CurrentTarget.TargetObject))
								{
									return OriginalHook(NascentFlash);
								}

								if (CurrentTarget.TargetObject == LocalPlayer)
								{
									return OriginalHook(RawIntuition);
								}
							}
						}
					}

					if (IsEnabled(CustomComboPreset.WAR_AoE_Decimate) && ActionReady(OriginalHook(Decimate))
						&& (GetBuffRemainingTime(Buffs.SurgingTempest) > GetOptionValue(Config.WAR_SurgingRefresh) || !LevelChecked(StormsEye))
						&& (HasEffect(Buffs.InnerRelease) || HasEffect(Buffs.NascentChaos)
						|| (Gauge.BeastGauge >= GetOptionValue(Config.WAR_DecimateGauge)
						&& (LevelChecked(InnerRelease)
						|| ((HasEffect(Buffs.Berserk) || (!HasEffect(Buffs.Berserk)
						&& !LevelChecked(InnerRelease))) && Gauge.BeastGauge >= 50)))))
					{
						return OriginalHook(Decimate);
					}

					if (IsEnabled(CustomComboPreset.WAR_AoE_PrimalRend) && HasEffect(Buffs.PrimalRuinationReady))
					{
						return PrimalRuination;
					}

					if (IsEnabled(CustomComboPreset.WAR_AoE_PrimalRend) && HasEffect(Buffs.PrimalRendReady))
					{
						return PrimalRend;
					}

					if (comboTime > 0)
					{
						if (lastComboMove is Overpower && ActionReady(MythrilTempest))
						{
							return MythrilTempest;
						}
					}

					return Overpower;
				}

				return actionID;
			}
		}

		internal class WAR_InfurCleav : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.WAR_InfurCleav;

			protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
			{
				if ((actionID is InnerBeast or FellCleave) && IsEnabled(CustomComboPreset.WAR_InfurCleav))
				{
					if (ActionReady(Infuriate) && (!HasEffect(Buffs.NascentChaos) || (!LevelChecked(ChaoticCyclone) && Gauge.BeastGauge < 50)))
					{
						return Infuriate;
					}

					if ((ActionReady(OriginalHook(FellCleave)) && HasEffect(Buffs.NascentChaos)) || Gauge.BeastGauge >= 50 || HasEffect(Buffs.InnerRelease))
					{
						return OriginalHook(FellCleave);
					}
				}

				return actionID;
			}
		}

		internal class WAR_InfurCyclo : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.WAR_InfurCyclo;

			protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
			{
				if ((actionID is SteelCyclone or Decimate) && IsEnabled(CustomComboPreset.WAR_InfurCyclo))
				{
					if (ActionReady(Infuriate) && (!HasEffect(Buffs.NascentChaos) || (!LevelChecked(ChaoticCyclone) && Gauge.BeastGauge < 50)))
					{
						return Infuriate;
					}

					if ((ActionReady(OriginalHook(Decimate)) && HasEffect(Buffs.NascentChaos)) || Gauge.BeastGauge >= 50 || HasEffect(Buffs.InnerRelease))
					{
						return OriginalHook(Decimate);
					}
				}

				return actionID;
			}
		}

		internal class WAR_Release : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.WAR_Release;

			protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
			{
				if ((actionID is InnerRelease or Berserk) && IsEnabled(CustomComboPreset.WAR_Release))
				{
					if (ActionReady(OriginalHook(InnerRelease)))
					{
						return OriginalHook(InnerRelease);
					}

					if (HasEffect(Buffs.InnerRelease))
					{
						return OriginalHook(FellCleave);
					}

					if (HasEffect(Buffs.PrimalRendReady))
					{
						return PrimalRend;
					}

					if (HasEffect(Buffs.PrimalRuinationReady))
					{
						return PrimalRuination;
					}
				}

				return actionID;
			}
		}

		internal class WAR_ThrillShake : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.WAR_ThrillShake;

			protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
			{
				if ((actionID is ThrillOfBattle or ShakeItOff) && IsEnabled(CustomComboPreset.WAR_ThrillShake))
				{
					if (ActionReady(ThrillOfBattle) && ActionReady(ShakeItOff))
					{
						return ThrillOfBattle;
					}

					if (ActionReady(ShakeItOff))
					{
						if (HasEffect(Buffs.ThrillOfBattle) || GetCooldownRemainingTime(ThrillOfBattle) < (GetCooldown(ThrillOfBattle).CooldownTotal - 10))
						{
							return ShakeItOff;
						}

						return OriginalHook(11);
					}
				}

				return actionID;
			}
		}
	}
}