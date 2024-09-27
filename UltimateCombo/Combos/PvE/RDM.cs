using Dalamud.Game.ClientState.JobGauge.Types;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.CustomCombo;
using UltimateCombo.Data;

namespace UltimateCombo.Combos.PvE
{
	internal class RDM
	{
		public const byte JobID = 35;

		public const uint
			Verthunder = 7505,
			Veraero = 7507,
			Veraero2 = 16525,
			Veraero3 = 25856,
			Verthunder2 = 16524,
			Verthunder3 = 25855,
			Impact = 16526,
			Redoublement = 7516,
			EnchantedRedoublement = 7529,
			Zwerchhau = 7512,
			EnchantedZwerchhau = 7528,
			Riposte = 7504,
			EnchantedRiposte = 7527,
			Scatter = 7509,
			Verstone = 7511,
			Verfire = 7510,
			Vercure = 7514,
			Jolt = 7503,
			Jolt2 = 7524,
			Jolt3 = 37004,
			Verholy = 7526,
			Verflare = 7525,
			Fleche = 7517,
			ContreSixte = 7519,
			Engagement = 16527,
			Verraise = 7523,
			Scorch = 16530,
			Resolution = 25858,
			Moulinet = 7513,
			EnchantedMoulinet = 7530,
			EnchantedMoulinetDeux = 37002,
			EnchantedMoulinetTrois = 37003,
			Corpsacorps = 7506,
			Displacement = 7515,
			Reprise = 16529,
			ViceOfThorns = 37005,
			GrandImpact = 37006,
			Prefulgence = 37007,
			Acceleration = 7518,
			Manafication = 7521,
			Embolden = 7520,
			MagickBarrier = 25857;

		public static class Buffs
		{
			public const ushort
				VerfireReady = 1234,
				VerstoneReady = 1235,
				Dualcast = 1249,
				Chainspell = 2560,
				Acceleration = 1238,
				Embolden = 1239,
				EmboldenOthers = 1297,
				Manafication = 1971,
				MagickBarrier = 2707,
				MagickedSwordPlay = 3875,
				ThornedFlourish = 3876,
				GrandImpactReady = 3877,
				PrefulugenceReady = 3878;
		}

		public static class Traits
		{
			public const uint
				EnhancedEmbolden = 620,
				EnhancedManaficationII = 622,
				EnhancedManaficationIII = 622,
				EnhancedAccelerationII = 624;
		}

		public static RDMGauge Gauge
		{
			get
			{
				return CustomComboFunctions.GetJobGauge<RDMGauge>();
			}
		}

		public static class Config
		{

		}

		internal class RDM_ST_DPS : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.RDM_ST_DPS;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Jolt or Jolt2 or Jolt3 or Verthunder or Verthunder3 or Veraero or Veraero3 or Verfire or Verstone)
					&& IsEnabled(CustomComboPreset.RDM_ST_DPS))
				{
					if (LocalPlayer.IsDead && ActionWatching.LastWeaponskill != 0)
					{
						ActionWatching.LastWeaponskill = 0;
					}

					if (IsEnabled(CustomComboPreset.RDM_ST_Opener) && !InCombat() && ActionReady(OriginalHook(Verthunder3))
						&& !HasEffect(Buffs.MagickedSwordPlay) && Gauge.ManaStacks == 0)
					{
						return OriginalHook(Verthunder3);
					}

					if (CanWeave(actionID)
						&& (!WasLastWeaponskill(EnchantedRiposte) || (WasLastWeaponskill(EnchantedRiposte)
							&& ActionWatching.GetAttackType(ActionWatching.LastAction) != ActionWatching.ActionAttackType.Ability))
							&& (!WasLastWeaponskill(EnchantedZwerchhau) || (WasLastWeaponskill(EnchantedZwerchhau)
							&& ActionWatching.GetAttackType(ActionWatching.LastAction) != ActionWatching.ActionAttackType.Ability))
							&& (!WasLastWeaponskill(EnchantedRedoublement) || (WasLastWeaponskill(EnchantedRedoublement)
							&& ActionWatching.GetAttackType(ActionWatching.LastAction) != ActionWatching.ActionAttackType.Ability)))
					{
						if (ActionWatching.NumberOfGcdsUsed >= 2)
						{
							if (IsEnabled(CustomComboPreset.RDM_ST_Swift) && ActionReady(All.Swiftcast)
								&& Gauge.WhiteMana < 100 && Gauge.BlackMana < 100
								&& !HasEffect(Buffs.Acceleration) && !HasEffect(Buffs.Embolden))
							{
								return All.Swiftcast;
							}
						}

						if (ActionWatching.NumberOfGcdsUsed >= 3)
						{
							if (IsEnabled(CustomComboPreset.RDM_ST_Fleche) && ActionReady(Fleche))
							{
								return Fleche;
							}

							if (IsEnabled(CustomComboPreset.RDM_ST_Accel) && ActionReady(Acceleration)
								&& !HasEffect(Buffs.Acceleration) && !HasEffect(All.Buffs.Swiftcast) && Gauge.ManaStacks == 0
								&& (HasEffect(Buffs.Embolden) || GetRemainingCharges(Acceleration) == GetMaxCharges(Acceleration))
								&& (Gauge.BlackMana < 50 || Gauge.WhiteMana < 50 || GetCooldownRemainingTime(Manafication) > 15))
							{
								return Acceleration;
							}
						}

						if (ActionWatching.NumberOfGcdsUsed >= 4)
						{
							if (IsEnabled(CustomComboPreset.RDM_ST_Embolden) && ActionReady(Embolden))
							{
								return Embolden;
							}

							if (IsEnabled(CustomComboPreset.RDM_ST_Manafication) && ActionReady(Manafication)
								&& (HasEffect(Buffs.Embolden) || GetCooldownRemainingTime(Embolden) > 90)
								&& !WasLastSpell(Verholy) && !WasLastSpell(Verflare) && !WasLastSpell(Scorch) && !WasLastSpell(Resolution)
								&& Gauge.ManaStacks == 0)
							{
								return Manafication;
							}

							if (IsEnabled(CustomComboPreset.RDM_ST_Contre) && ActionReady(ContreSixte))
							{
								return ContreSixte;
							}

							if (IsEnabled(CustomComboPreset.RDM_ST_Engagement) && ActionReady(Engagement) && InActionRange(Engagement))
							{
								return Engagement;
							}

							if (IsEnabled(CustomComboPreset.RDM_ST_Corps) && ActionReady(Corpsacorps) && InMeleeRangeNoMovement())
							{
								return Corpsacorps;
							}

							if (IsEnabled(CustomComboPreset.RDM_ST_Embolden) && HasEffect(Buffs.ThornedFlourish))
							{
								return ViceOfThorns;
							}

							if (IsEnabled(CustomComboPreset.RDM_ST_Manafication) && HasEffect(Buffs.PrefulugenceReady))
							{
								return Prefulgence;
							}
						}
					}

					if (ActionReady(Resolution) && WasLastSpell(Scorch))
					{
						return Resolution;
					}

					if (ActionReady(Scorch) && (WasLastSpell(Verholy) || WasLastSpell(Verflare)))
					{
						return Scorch;
					}

					if ((ActionReady(Verholy) || ActionReady(Verflare)) && Gauge.ManaStacks is 3)
					{
						ActionWatching.LastWeaponskill = 0;
						if (Gauge.BlackMana >= Gauge.WhiteMana)
						{
							return Verholy;
						}

						if (Gauge.WhiteMana >= Gauge.BlackMana)
						{
							return Verflare;
						}
					}

					if ((IsEnabled(CustomComboPreset.RDM_ST_Swords) || IsEnabled(CustomComboPreset.RDM_ST_Manafication))
						&& (Gauge.BlackMana == 100 || Gauge.WhiteMana == 100
						|| (HasEffect(Buffs.Embolden) && Gauge.WhiteMana >= 50 && Gauge.BlackMana >= 50)
						|| HasEffect(Buffs.MagickedSwordPlay)
						|| WasLastWeaponskill(OriginalHook(EnchantedRiposte))
						|| WasLastWeaponskill(OriginalHook(EnchantedZwerchhau))))
					{
						if (WasLastWeaponskill(OriginalHook(EnchantedZwerchhau))
							&& ((Gauge.WhiteMana >= 15 && Gauge.BlackMana >= 15) || HasEffect(Buffs.MagickedSwordPlay)))
						{
							if (IsEnabled(CustomComboPreset.RDM_ST_Corps) && ActionReady(Corpsacorps)
								&& InActionRange(EnchantedRedoublement))
							{
								return Corpsacorps;
							}

							if (Gauge.ManaStacks == 2)
							{
								return EnchantedRedoublement;
							}
						}

						if (WasLastWeaponskill(OriginalHook(EnchantedRiposte))
							&& ((Gauge.WhiteMana >= 15 && Gauge.BlackMana >= 15) || HasEffect(Buffs.MagickedSwordPlay)))
						{
							if (IsEnabled(CustomComboPreset.RDM_ST_Corps) && ActionReady(Corpsacorps)
								&& InActionRange(EnchantedZwerchhau))
							{
								return Corpsacorps;
							}

							if (Gauge.ManaStacks == 1)
							{
								return EnchantedZwerchhau;
							}
						}

						if ((Gauge.WhiteMana >= 20 && Gauge.BlackMana >= 20) || HasEffect(Buffs.MagickedSwordPlay))
						{
							if (IsEnabled(CustomComboPreset.RDM_ST_Corps) && ActionReady(Corpsacorps)
							&& InActionRange(EnchantedRiposte))
							{
								return Corpsacorps;
							}

							if (Gauge.ManaStacks == 0 && InActionRange(OriginalHook(EnchantedRiposte)))
							{
								return EnchantedRiposte;
							}
						}
					}

					if (IsEnabled(CustomComboPreset.RDM_ST_Accel) && HasEffect(Buffs.GrandImpactReady)
						&& (HasEffect(Buffs.Embolden) || GetBuffRemainingTime(Buffs.GrandImpactReady) < 5))
					{
						return GrandImpact;
					}

					if (HasEffect(Buffs.Dualcast) || HasEffect(Buffs.Acceleration) || HasEffect(All.Buffs.Swiftcast))
					{
						if (Gauge.BlackMana >= Gauge.WhiteMana)
						{
							return OriginalHook(Veraero3);
						}

						if (Gauge.WhiteMana >= Gauge.BlackMana)
						{
							return OriginalHook(Verthunder3);
						}
					}

					if (HasEffect(Buffs.VerstoneReady))
					{
						return Verstone;
					}

					if (HasEffect(Buffs.VerfireReady))
					{
						return Verfire;
					}

					return OriginalHook(Jolt3);
				}

				return actionID;
			}
		}

		internal class RDM_AoE_DPS : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.RDM_AoE_DPS;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Scatter or Impact or Verthunder2 or Veraero2) && IsEnabled(CustomComboPreset.RDM_AoE_DPS))
				{
					if (LocalPlayer.IsDead && ActionWatching.LastWeaponskill != 0)
					{
						ActionWatching.LastWeaponskill = 0;
					}

					if (CanWeave(actionID)
						&& (!WasLastWeaponskill(EnchantedMoulinet) || (WasLastWeaponskill(EnchantedMoulinet)
							&& ActionWatching.GetAttackType(ActionWatching.LastAction) != ActionWatching.ActionAttackType.Ability))
							&& (!WasLastWeaponskill(EnchantedMoulinetDeux) || (WasLastWeaponskill(EnchantedMoulinetDeux)
							&& ActionWatching.GetAttackType(ActionWatching.LastAction) != ActionWatching.ActionAttackType.Ability))
							&& (!WasLastWeaponskill(EnchantedMoulinetTrois) || (WasLastWeaponskill(EnchantedMoulinetTrois)
							&& ActionWatching.GetAttackType(ActionWatching.LastAction) != ActionWatching.ActionAttackType.Ability)))
					{
						if (ActionWatching.NumberOfGcdsUsed >= 2)
						{
							if (IsEnabled(CustomComboPreset.RDM_AoE_Embolden) && ActionReady(Embolden))
							{
								return Embolden;
							}

							if (IsEnabled(CustomComboPreset.RDM_AoE_Manafication) && ActionReady(Manafication)
								&& (HasEffect(Buffs.Embolden) || GetCooldownRemainingTime(Embolden) > 90)
								&& !WasLastSpell(Verholy) && !WasLastSpell(Verflare) && !WasLastSpell(Scorch) && !WasLastSpell(Resolution)
								&& Gauge.ManaStacks == 0)
							{
								return Manafication;
							}
						}

						if (IsEnabled(CustomComboPreset.RDM_AoE_Swift) && ActionReady(All.Swiftcast)
							&& Gauge.WhiteMana < 100 && Gauge.BlackMana < 100
							&& !HasEffect(Buffs.Acceleration) && !HasEffect(Buffs.Embolden))
						{
							return All.Swiftcast;
						}

						if (IsEnabled(CustomComboPreset.RDM_AoE_Fleche) && ActionReady(Fleche))
						{
							return Fleche;
						}

						if (IsEnabled(CustomComboPreset.RDM_AoE_Accel) && ActionReady(Acceleration)
							&& !HasEffect(Buffs.Acceleration) && !HasEffect(All.Buffs.Swiftcast)
							&& (HasEffect(Buffs.Embolden) || GetRemainingCharges(Acceleration) == GetMaxCharges(Acceleration))
							&& (Gauge.BlackMana < 50 || Gauge.WhiteMana < 50 || GetCooldownRemainingTime(Manafication) > 15))
						{
							return Acceleration;
						}

						if (IsEnabled(CustomComboPreset.RDM_AoE_Embolden) && HasEffect(Buffs.ThornedFlourish))
						{
							return ViceOfThorns;
						}

						if (IsEnabled(CustomComboPreset.RDM_AoE_Manafication) && HasEffect(Buffs.PrefulugenceReady))
						{
							return Prefulgence;
						}

						if (IsEnabled(CustomComboPreset.RDM_AoE_Contre) && ActionReady(ContreSixte))
						{
							return ContreSixte;
						}
					}

					if (ActionReady(Resolution) && WasLastSpell(Scorch))
					{
						return Resolution;
					}

					if (ActionReady(Scorch) && (WasLastSpell(Verholy) || WasLastSpell(Verflare)))
					{
						return Scorch;
					}

					if (IsEnabled(CustomComboPreset.RDM_AoE_Accel) && HasEffect(Buffs.GrandImpactReady))
					{
						return GrandImpact;
					}

					if ((ActionReady(Verholy) || ActionReady(Verflare)) && Gauge.ManaStacks is 3)
					{
						ActionWatching.LastWeaponskill = 0;
						if (Gauge.BlackMana >= Gauge.WhiteMana)
						{
							return Verholy;
						}

						if (Gauge.WhiteMana >= Gauge.BlackMana)
						{
							return Verflare;
						}
					}

					if ((IsEnabled(CustomComboPreset.RDM_AoE_Swords) || IsEnabled(CustomComboPreset.RDM_AoE_Manafication))
						&& (Gauge.BlackMana == 100 || Gauge.WhiteMana == 100
						|| (HasEffect(Buffs.Embolden) && Gauge.WhiteMana >= 50 && Gauge.BlackMana >= 50)
						|| HasEffect(Buffs.MagickedSwordPlay)
						|| WasLastWeaponskill(OriginalHook(EnchantedMoulinetDeux))
						|| WasLastWeaponskill(OriginalHook(EnchantedMoulinetTrois))))
					{
						if (WasLastWeaponskill(OriginalHook(EnchantedMoulinetDeux))
							&& ((Gauge.WhiteMana >= 15 && Gauge.BlackMana >= 15) || HasEffect(Buffs.MagickedSwordPlay)))
						{
							if (IsEnabled(CustomComboPreset.RDM_AoE_Corps) && ActionReady(Corpsacorps)
								&& !InActionRange(EnchantedMoulinetTrois))
							{
								return Corpsacorps;
							}

							if (Gauge.ManaStacks == 2)
							{
								return EnchantedMoulinetTrois;
							}
						}

						if (WasLastWeaponskill(OriginalHook(EnchantedMoulinet))
							&& ((Gauge.WhiteMana >= 15 && Gauge.BlackMana >= 15) || HasEffect(Buffs.MagickedSwordPlay)))
						{
							if (IsEnabled(CustomComboPreset.RDM_AoE_Corps) && ActionReady(Corpsacorps)
								&& !InActionRange(EnchantedMoulinetDeux))
							{
								return Corpsacorps;
							}

							if (Gauge.ManaStacks == 1)
							{
								return EnchantedMoulinetDeux;
							}
						}

						if ((Gauge.WhiteMana >= 20 && Gauge.BlackMana >= 20) || HasEffect(Buffs.MagickedSwordPlay))
						{
							if (IsEnabled(CustomComboPreset.RDM_AoE_Corps) && ActionReady(Corpsacorps)
								&& !InActionRange(EnchantedMoulinet))
							{
								return Corpsacorps;
							}

							if (Gauge.ManaStacks == 0 && InActionRange(OriginalHook(EnchantedMoulinet)))
							{
								return EnchantedMoulinet;
							}
						}
					}

					if (HasEffect(Buffs.Dualcast) || HasEffect(Buffs.Acceleration) || HasEffect(All.Buffs.Swiftcast))
					{
						return OriginalHook(Impact);
					}

					if (Gauge.BlackMana >= Gauge.WhiteMana)
					{
						return Veraero2;
					}

					if (Gauge.WhiteMana >= Gauge.BlackMana)
					{
						return Verthunder2;
					}
				}

				return actionID;
			}
		}

		internal class RDM_ST_Melee : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.RDM_ST_Melee;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Riposte or Zwerchhau or Redoublement) && IsEnabled(CustomComboPreset.RDM_ST_Melee))
				{
					if (ActionReady(Resolution) && WasLastSpell(Scorch))
					{
						return Resolution;
					}

					if (ActionReady(Scorch) && (WasLastSpell(Verholy) || WasLastSpell(Verflare)))
					{
						return Scorch;
					}

					if ((ActionReady(Verholy) || ActionReady(Verflare)) && Gauge.ManaStacks is 3)
					{
						if (Gauge.BlackMana >= Gauge.WhiteMana)
						{
							return Verholy;
						}

						if (Gauge.WhiteMana >= Gauge.BlackMana)
						{
							return Verflare;
						}
					}

					if (WasLastWeaponskill(OriginalHook(EnchantedZwerchhau)) && Gauge.WhiteMana >= 15 && Gauge.BlackMana >= 15)
					{
						return EnchantedRedoublement;
					}

					if (WasLastWeaponskill(OriginalHook(EnchantedRiposte)) && Gauge.WhiteMana >= 15 && Gauge.BlackMana >= 15)
					{
						return EnchantedZwerchhau;
					}

					if (Gauge.WhiteMana >= 20 && Gauge.BlackMana >= 20)
					{
						return EnchantedRiposte;
					}
				}

				return actionID;
			}
		}

		internal class RDM_AoE_Melee : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.RDM_AoE_Melee;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (actionID is Moulinet && IsEnabled(CustomComboPreset.RDM_AoE_Melee))
				{
					if (ActionReady(Resolution) && WasLastSpell(Scorch))
					{
						return Resolution;
					}

					if (ActionReady(Scorch) && (WasLastSpell(Verholy) || WasLastSpell(Verflare)))
					{
						return Scorch;
					}

					if ((ActionReady(Verholy) || ActionReady(Verflare)) && Gauge.ManaStacks is 3)
					{
						if (Gauge.BlackMana >= Gauge.WhiteMana)
						{
							return Verholy;
						}

						if (Gauge.WhiteMana >= Gauge.BlackMana)
						{
							return Verflare;
						}
					}

					if (WasLastWeaponskill(OriginalHook(EnchantedMoulinetDeux)) && Gauge.WhiteMana >= 15 && Gauge.BlackMana >= 15)
					{
						return EnchantedMoulinetTrois;
					}

					if (WasLastWeaponskill(OriginalHook(EnchantedMoulinet)) && Gauge.WhiteMana >= 15 && Gauge.BlackMana >= 15)
					{
						return EnchantedMoulinetDeux;
					}

					if (Gauge.WhiteMana >= 20 && Gauge.BlackMana >= 20)
					{
						return EnchantedMoulinet;
					}
				}

				return actionID;
			}
		}
	}
}