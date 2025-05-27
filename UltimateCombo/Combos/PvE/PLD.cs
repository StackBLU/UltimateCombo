﻿using Dalamud.Game.ClientState.JobGauge.Types;
using System.Linq;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Combos.PvE.Content;
using UltimateCombo.CustomCombo;
using UltimateCombo.Data;
using UltimateCombo.Services;

namespace UltimateCombo.Combos.PvE
{
	internal static class PLD
	{
		public const byte JobID = 19;

		public const uint
			FastBlade = 9,
			RiotBlade = 15,
			ShieldBash = 16,
			Sentinel = 17,
			RageOfHalone = 21,
			CircleOfScorn = 23,
			ShieldLob = 24,
			SpiritsWithin = 29,
			HallowedGround = 30,
			GoringBlade = 3538,
			RoyalAuthority = 3539,
			TotalEclipse = 7381,
			Intervention = 7382,
			Requiescat = 7383,
			Imperator = 36921,
			HolySpirit = 7384,
			Prominence = 16457,
			HolyCircle = 16458,
			Confiteor = 16459,
			Expiacion = 25747,
			BladeOfFaith = 25748,
			BladeOfTruth = 25749,
			BladeOfValor = 25750,
			FightOrFlight = 20,
			Atonement = 16460,
			Intervene = 16461,
			Guardian = 36920,
			BladeOfHonor = 36922,
			Supplication = 36918,
			Sepulchre = 36919,
			Sheltron = 3542;

		public static class Buffs
		{
			public const ushort
				Requiescat = 1368,
				AtonementReady = 1902,
				SupplicationReady = 3827,
				SepulchreReady = 3828,
				GoringBladeReady = 3847,
				BladeOfHonor = 3831,
				FightOrFlight = 76,
				ConfiteorReady = 3019,
				DivineMight = 2673,
				HolySheltron = 2674,
				Sheltron = 1856,
				Sentinel = 74,
				Guardian = 3829;
		}

		private static PLDGauge Gauge
		{
			get
			{
				return CustomComboFunctions.GetJobGauge<PLDGauge>();
			}
		}

		public static class Config
		{
			public static UserInt
				PLD_ST_Sheltron = new("PLD_ST_Sheltron", 100),
				PLD_ST_Intervention = new("PLD_ST_Intervention", 50),
				PLD_AoE_Sheltron = new("PLD_AoE_Sheltron", 50),
				PLD_AoE_Intervention = new("PLD_AoE_Intervention", 50),
				PLD_ST_Invuln = new("PLD_ST_Invuln", 10),
				PLD_AoE_Invuln = new("PLD_AoE_Invuln", 10);
		}

		internal class PLD_ST_DPS : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.PLD_ST_DPS;

			protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
			{
				if ((actionID is FastBlade or RiotBlade or RageOfHalone or RoyalAuthority) && IsEnabled(CustomComboPreset.PLD_ST_DPS))
				{
					if (IsEnabled(CustomComboPreset.PLD_ST_Invuln) && PlayerHealthPercentageHp() <= GetOptionValue(Config.PLD_ST_Invuln)
						&& ActionReady(HallowedGround))
					{
						return HallowedGround;
					}

					if (IsEnabled(CustomComboPreset.PLD_ST_HolySpirit) && !InCombat() && ActionReady(HolySpirit) && LocalPlayer.CurrentMp >= 1000 && !InMeleeRange())
					{
						return HolySpirit;
					}

					if (CanWeave(actionID) && (ActionWatching.NumberOfGcdsUsed >= 4 || Service.Configuration.IgnoreGCDChecks) && !HasEffect(Bozja.Buffs.BloodRage))
					{
						if (IsEnabled(CustomComboPreset.PLD_ST_FightOrFlight) && ActionReady(FightOrFlight))
						{
							return FightOrFlight;
						}

						if (IsEnabled(CustomComboPreset.PLD_ST_Imperator) && ActionReady(OriginalHook(Imperator)))
						{
							return OriginalHook(Imperator);
						}

						if (IsEnabled(CustomComboPreset.PLD_ST_CircleOfScorn) && ActionReady(CircleOfScorn) && InMeleeRange())
						{
							return CircleOfScorn;
						}

						if (IsEnabled(CustomComboPreset.PLD_ST_SpiritsWithin) && ActionReady(OriginalHook(Expiacion))
							&& InActionRange(OriginalHook(Expiacion)))
						{
							return OriginalHook(Expiacion);
						}

						if (IsEnabled(CustomComboPreset.PLD_ST_Intervene) && ActionReady(Intervene) && InMeleeRangeNoMovement())
						{
							return Intervene;
						}

						if (IsEnabled(CustomComboPreset.PLD_ST_Intervention) && CurrentTarget.TargetObject != LocalPlayer
							&& GetPartyMembers().Any(x => x.GameObject == CurrentTarget.TargetObject)
							&& ActionReady(Intervention) && Gauge.OathGauge >= GetOptionValue(Config.PLD_ST_Intervention))
						{
							if (ActionReady(All.Rampart) && !HasEffect(Buffs.Sentinel) && !HasEffect(Buffs.Guardian)
								&& !WasLastAbility(OriginalHook(Sentinel)))
							{
								return All.Rampart;
							}

							if (ActionReady(OriginalHook(Sentinel)) && !HasEffect(All.Buffs.Rampart) && !WasLastAbility(All.Rampart))
							{
								return OriginalHook(Sentinel);
							}

							return Intervention;
						}

						if (IsEnabled(CustomComboPreset.PLD_ST_Sheltron) && ActionReady(Sheltron)
							&& !HasEffect(Buffs.Sheltron) && !HasEffect(Buffs.HolySheltron) &&
							Gauge.OathGauge >= GetOptionValue(Config.PLD_ST_Sheltron))
						{
							return OriginalHook(Sheltron);
						}
					}

					if (IsEnabled(CustomComboPreset.PLD_ST_Confiteor) && LocalPlayer.CurrentMp >= 1000
						&& (HasEffect(Buffs.ConfiteorReady) || WasLastSpell(Confiteor) || WasLastSpell(BladeOfFaith) || WasLastSpell(BladeOfTruth)))
					{
						return OriginalHook(Confiteor);
					}

					if (IsEnabled(CustomComboPreset.PLD_ST_FightOrFlight) && HasEffect(Buffs.GoringBladeReady)
						&& (WasLastSpell(BladeOfValor) || !LevelChecked(OriginalHook(BladeOfFaith)) || GetBuffRemainingTime(Buffs.GoringBladeReady) < 5))
					{
						return GoringBlade;
					}

					if ((HasEffect(Buffs.FightOrFlight) && GetCooldownRemainingTime(FightOrFlight) > 5)
						|| (GetCooldownRemainingTime(OriginalHook(Imperator)) > 20 && !HasEffect(Buffs.Requiescat))
						|| lastComboActionID is RiotBlade)
					{
						if (IsEnabled(CustomComboPreset.PLD_ST_Atonement) && ActionReady(OriginalHook(Atonement))
							&& (HasEffect(Buffs.AtonementReady) || HasEffect(Buffs.SupplicationReady) || HasEffect(Buffs.SepulchreReady)))
						{
							return OriginalHook(Atonement);
						}

						if (IsEnabled(CustomComboPreset.PLD_ST_HolySpirit) && ActionReady(OriginalHook(HolySpirit))
							&& (HasEffect(Buffs.DivineMight) || (HasEffect(Buffs.Requiescat) && !LevelChecked(BladeOfFaith)))
							&& LocalPlayer.CurrentMp >= GetResourceCost(HolySpirit))
						{
							return HolySpirit;
						}
					}

					if (IsEnabled(CustomComboPreset.PLD_ST_ShieldLob) && ActionReady(ShieldLob) && OutOfMeleeRange())
					{
						return ShieldLob;
					}

					if (comboTime > 0)
					{
						if (lastComboActionID is FastBlade && ActionReady(RiotBlade))
						{
							return RiotBlade;
						}

						if (lastComboActionID is RiotBlade && ActionReady(OriginalHook(RoyalAuthority)))
						{
							return OriginalHook(RoyalAuthority);
						}
					}

					return FastBlade;
				}

				return actionID;
			}
		}

		internal class PLD_AoE_DPS : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.PLD_AoE_DPS;

			protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
			{
				if ((actionID is TotalEclipse or Prominence) && IsEnabled(CustomComboPreset.PLD_AoE_DPS))
				{
					if (IsEnabled(CustomComboPreset.PLD_AoE_Invuln) && PlayerHealthPercentageHp() <= GetOptionValue(Config.PLD_AoE_Invuln)
						&& ActionReady(HallowedGround))
					{
						return HallowedGround;
					}

					if (IsEnabled(CustomComboPreset.PLD_AoE_Intervene) && ActionReady(Intervene) && !InMeleeRange()
						&& !InCombat())
					{
						return Intervene;
					}

					if (CanWeave(actionID))
					{
						if (IsEnabled(CustomComboPreset.PLD_AoE_FightOrFlight) && ActionReady(FightOrFlight))
						{
							return FightOrFlight;
						}

						if (IsEnabled(CustomComboPreset.PLD_AoE_Imperator) && ActionReady(OriginalHook(Imperator)))
						{
							return OriginalHook(Imperator);
						}

						if (IsEnabled(CustomComboPreset.PLD_AoE_CircleOfScorn) && ActionReady(CircleOfScorn) && InMeleeRange())
						{
							return CircleOfScorn;
						}

						if (IsEnabled(CustomComboPreset.PLD_AoE_SpiritsWithin) && ActionReady(OriginalHook(Expiacion))
							&& InActionRange(OriginalHook(Expiacion)))
						{
							return OriginalHook(Expiacion);
						}

						if (IsEnabled(CustomComboPreset.PLD_AoE_Intervene) && ActionReady(Intervene) && InMeleeRangeNoMovement())
						{
							return Intervene;
						}

						if (IsEnabled(CustomComboPreset.PLD_AoE_Intervention) && CurrentTarget.TargetObject != LocalPlayer
							&& GetPartyMembers().Any(x => x.GameObject == CurrentTarget.TargetObject)
							&& ActionReady(Intervention) && Gauge.OathGauge >= GetOptionValue(Config.PLD_AoE_Intervention))
						{
							if (ActionReady(All.Rampart) && !HasEffect(Buffs.Sentinel) && !HasEffect(Buffs.Guardian)
								&& !WasLastAbility(OriginalHook(Sentinel)))
							{
								return All.Rampart;
							}

							if (ActionReady(OriginalHook(Sentinel)) && !HasEffect(All.Buffs.Rampart) && !WasLastAbility(All.Rampart))
							{
								return OriginalHook(Sentinel);
							}

							return Intervention;
						}

						if (IsEnabled(CustomComboPreset.PLD_AoE_Sheltron) && ActionReady(Sheltron)
							&& !HasEffect(Buffs.Sheltron) && !HasEffect(Buffs.HolySheltron) &&
							Gauge.OathGauge >= GetOptionValue(Config.PLD_AoE_Sheltron))
						{
							return OriginalHook(Sheltron);
						}
					}

					if (IsEnabled(CustomComboPreset.PLD_AoE_Confiteor) && LocalPlayer.CurrentMp >= 1000
						&& (HasEffect(Buffs.ConfiteorReady) || WasLastSpell(Confiteor) || WasLastSpell(BladeOfFaith) || WasLastSpell(BladeOfTruth)))
					{
						return OriginalHook(Confiteor);
					}

					if (IsEnabled(CustomComboPreset.PLD_AoE_HolyCircle) && ActionReady(HolyCircle)
						&& LocalPlayer.CurrentMp >= GetResourceCost(HolyCircle)
						&& (HasEffect(Buffs.DivineMight) || (HasEffect(Buffs.Requiescat) && !LevelChecked(BladeOfFaith))))
					{
						return HolyCircle;
					}

					if (comboTime > 0 && WasLastWeaponskill(TotalEclipse) && ActionReady(Prominence))
					{
						return Prominence;
					}
				}

				return actionID;
			}
		}

		internal class PLD_Blades : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.PLD_Blades;

			protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
			{
				if ((actionID is Requiescat or Imperator) && IsEnabled(CustomComboPreset.PLD_Blades))
				{
					if (ActionReady(OriginalHook(Confiteor)) && HasEffect(Buffs.Requiescat))
					{
						return OriginalHook(Confiteor);
					}

					if (ActionReady(OriginalHook(Imperator)))
					{
						return OriginalHook(Imperator);
					}
				}

				return actionID;
			}
		}

		internal class PLD_ExpiScorn : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.PLD_ExpiScorn;

			protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
			{
				if ((actionID is CircleOfScorn or SpiritsWithin or Expiacion) && IsEnabled(CustomComboPreset.PLD_ExpiScorn))
				{
					if (ActionReady(CircleOfScorn))
					{
						return CircleOfScorn;
					}

					if (ActionReady(OriginalHook(Expiacion)))
					{
						return Expiacion;
					}
				}

				return actionID;
			}
		}

		internal class PLD_Intervention : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.PLD_Intervention;

			protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
			{
				if (actionID is Intervention && IsEnabled(CustomComboPreset.PLD_Intervention))
				{
					if (Gauge.OathGauge >= 50)
					{
						if (ActionReady(All.Rampart) && !HasEffect(Buffs.Sentinel) && !HasEffect(Buffs.Guardian) && !WasLastAbility(OriginalHook(Guardian)))
						{
							return All.Rampart;
						}

						if (ActionReady(OriginalHook(Guardian)) && !HasEffect(All.Buffs.Rampart) && !WasLastAbility(All.Rampart))
						{
							return OriginalHook(Guardian);
						}
					}
				}

				return actionID;
			}
		}
	}
}