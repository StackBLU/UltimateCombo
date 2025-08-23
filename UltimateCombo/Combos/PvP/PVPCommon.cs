using System.Collections.Generic;

using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Combos.PvE;
using UltimateCombo.Core;
using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvP
{
	internal static class PvPCommon
	{
		public const uint
			Teleport = 5,
			Return = 6,
			StandardElixir = 29055,
			Recuperate = 29711,
			Purify = 29056,
			Guard = 29054,
			Sprint = 29057,

			//Role Actions
			Rampage = 43243,
			Rampart = 43244,
			FullSwing = 43245,

			Haelan = 43255,
			Stoneskin2 = 43256,
			Diabrosis = 43257,

			Bloodbath = 43246,
			Swift = 43247,
			Smite = 43248,

			Dervish = 43249,
			Bravery = 43250,
			EagleEyeShot = 43251,

			Comet = 43252,
			PhantomDart = 43291,
			Rust = 43254;

		internal class Config
		{
			public static UserBool
				Purify_Stun = new("Purify_Stun"),
				Purify_Heavy = new("Purify_Heavy"),
				Purify_Bind = new("Purify_Bind"),
				Purify_Silence = new("Purify_Silence"),
				Purify_DeepFreeze = new("Purify_DeepFreeze"),
				Purify_MiracleOfNature = new("Purify_MiracleOfNature");

			public static UserInt
				Recuperate = new("Recuperate", 90),
				Guard = new("Guard", 50),
				TankPvP_Rampart = new("TankPvP_Rampart", 75),
				MeleePvP_Bloodbath = new("MeleePvP_Bloodbath", 55);
		}

		internal class Debuffs
		{
			public const ushort
				Stun = 1343,
				Heavy = 1344,
				Bind = 1345,
				Silence = 1347,
				DeepFreeze = 3219,
				MiracleOfNature = 3085,

				HalfAsleep = 3022,
				Sleep = 1348,

				Unguarded = 3021;
		}

		internal class Buffs
		{
			public const ushort
				Guard = 3054,
				Resilience = 3248,
				DRGLBInAir = 3180,
				RivalWingsMounted = 1420,

				Rampage = 4483,
				Rampart = 4484,
				FullSwing = 4485,

				Haelan = 4495,
				Stoneskin2 = 4496,
				Diabrosis = 4497,

				Bloodbath = 4486,
				Swift = 4487,
				Smite = 4488,

				Dervish = 4489,
				Bravery = 4490,
				EagleEyeShot = 4491,

				Comet = 4492,
				PhantomDart = 4516,
				Rust = 4494;
		}

		internal static readonly List<uint>
			GlobalSkills = [Teleport, Guard, Recuperate, Purify, StandardElixir, Sprint];

		internal class PvP_Purify : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.PvP_Purify;

			protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
			{
				if (HasEffect(Buffs.Guard))
				{
					if (actionID is Guard)
					{
						return Guard;
					}

					return OriginalHook(11);
				}

				if (InPvP() && !GlobalSkills.Contains(actionID) && !HasEffect(Buffs.DRGLBInAir) && !HasEffect(Buffs.RivalWingsMounted)
					&& IsOffCooldown(Purify) && LocalPlayer.CurrentMp >= 2500
					&& !HasEffect(NINPvP.Buffs.Hidden) && !HasEffect(Buffs.Resilience)
					&& ((HasEffectAny(Debuffs.Stun) && PluginConfiguration.GetCustomBoolValue(Config.Purify_Stun))
					|| (HasEffectAny(Debuffs.Heavy) && PluginConfiguration.GetCustomBoolValue(Config.Purify_Heavy))
					|| (HasEffectAny(Debuffs.Bind) && PluginConfiguration.GetCustomBoolValue(Config.Purify_Bind))
					|| (HasEffectAny(Debuffs.Silence) && PluginConfiguration.GetCustomBoolValue(Config.Purify_Silence))
					|| (HasEffectAny(Debuffs.DeepFreeze) && PluginConfiguration.GetCustomBoolValue(Config.Purify_DeepFreeze))
					|| (HasEffectAny(Debuffs.MiracleOfNature) && PluginConfiguration.GetCustomBoolValue(Config.Purify_MiracleOfNature))))
				{
					return OriginalHook(Purify);
				}

				return actionID;
			}
		}

		internal class PvP_Guard : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.PvP_Guard;

			protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
			{
				if (HasEffect(Buffs.Guard) && actionID == Guard)
				{
					if (actionID is Guard)
					{
						return Guard;
					}

					return OriginalHook(11);
				}

				if (InPvP() && !GlobalSkills.Contains(actionID)
					&& IsOffCooldown(Guard) && PlayerHealthPercentageHp() <= GetOptionValue(Config.Guard)
					&& !HasEffect(DRKPvP.Buffs.UndeadRedemption) && !HasEffectAny(Debuffs.Unguarded) && !HasEffect(WARPvP.Buffs.InnerRelease)
					&& !HasEffect(Buffs.DRGLBInAir) && !HasEffect(Buffs.RivalWingsMounted) && !HasEffect(NINPvP.Buffs.Hidden)
					&& actionID != VPRPvP.SnakeScales)
				{
					if (LocalPlayer.ClassJob.Value.RowId == DRK.JobID && IsEnabled(CustomComboPreset.DRKPvP_Impalement) && ActionReady(DRKPvP.Impalement))
					{
						return DRKPvP.Impalement;
					}

					return OriginalHook(Guard);
				}

				return actionID;
			}
		}

		internal class PvP_Recuperate : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.PvP_Recuperate;

			protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
			{
				if (HasEffect(Buffs.Guard))
				{
					if (actionID is Guard)
					{
						return Guard;
					}

					return OriginalHook(11);
				}

				if (InPvP() && !GlobalSkills.Contains(actionID)
					&& LocalPlayer.CurrentMp >= 2500 && PlayerHealthPercentageHpPvP() <= GetOptionValue(Config.Recuperate)
					&& !HasEffect(Buffs.DRGLBInAir) && !HasEffect(Buffs.RivalWingsMounted)
					&& !HasEffect(NINPvP.Buffs.Hidden) && actionID != VPRPvP.SnakeScales)
				{
					if ((LocalPlayer.ClassJob.Value.RowId == DRK.JobID && IsOffCooldown(DRKPvP.Impalement))
						|| (HasEffect(DRKPvP.Buffs.UndeadRedemption) && GetBuffRemainingTime(DRKPvP.Buffs.UndeadRedemption) > 2))
					{
						return actionID;
					}

					return OriginalHook(Recuperate);
				}

				return actionID;
			}
		}

		internal class IgnoreSAMKuzuchi : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.PvP_IgnoreSAMKuzuchi;

			protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.PvP_IgnoreSAMKuzuchi) && TargetHasEffectAny(SAMPvP.Buffs.Chiten)
					&& !HasEffect(NINPvP.Buffs.UnsealedSeitonTenchu) && IsPvPComboAction(actionID))
				{
					return OriginalHook(11);
				}

				return actionID;
			}
		}

		internal class TankPvP_RoleActions : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.TankPvP_RoleActions;

			protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.TankPvP_RoleActions) && InCombat() && IsPvPComboAction(actionID))
				{
					if (IsEnabled(CustomComboPreset.TankPvP_Rampage) && IsOffCooldown(Rampage) && HasEffect(Buffs.Rampage))
					{
						return Rampage;
					}

					if (IsEnabled(CustomComboPreset.TankPvP_Rampart) && IsOffCooldown(Rampart) && HasEffect(Buffs.Rampart)
						&& PlayerHealthPercentageHp() <= GetOptionValue(Config.TankPvP_Rampart))
					{
						return Rampart;
					}

					if (IsEnabled(CustomComboPreset.TankPvP_FullSwing) && IsOffCooldown(FullSwing) && HasEffect(Buffs.FullSwing)
						&& InActionRange(FullSwing) && TargetHasEffectAny(Buffs.Guard))
					{
						return FullSwing;
					}
				}

				return actionID;
			}
		}

		internal class HealerPvP_RoleActions : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.HealerPvP_RoleActions;

			protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.HealerPvP_RoleActions) && InCombat() && IsPvPComboAction(actionID))
				{
					if (IsEnabled(CustomComboPreset.HealerPvP_Stoneskin2) && IsOffCooldown(Stoneskin2) && HasEffect(Buffs.Stoneskin2))
					{
						return Stoneskin2;
					}

					if (IsEnabled(CustomComboPreset.HealerPvP_Diabrosis) && IsOffCooldown(Diabrosis) && HasEffect(Buffs.Diabrosis)
						&& InActionRange(Diabrosis))
					{
						return Diabrosis;
					}
				}

				return actionID;
			}
		}

		internal class MeleePvP_RoleActions : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MeleePvP_RoleActions;

			protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.MeleePvP_RoleActions) && InCombat() && IsPvPComboAction(actionID))
				{
					if (IsEnabled(CustomComboPreset.MeleePvP_Bloodbath) && IsOffCooldown(Bloodbath) && HasEffect(Buffs.Bloodbath)
						&& PlayerHealthPercentageHp() <= GetOptionValue(Config.MeleePvP_Bloodbath))
					{
						return Bloodbath;
					}

					if (IsEnabled(CustomComboPreset.MeleePvP_Swift) && IsOffCooldown(Swift) && HasEffect(Buffs.Swift)
						&& (HasEffectAny(Debuffs.Stun) || HasEffectAny(Debuffs.DeepFreeze) || HasEffectAny(Debuffs.HalfAsleep)
						|| HasEffectAny(Debuffs.Sleep) || HasEffectAny(Debuffs.Bind) || HasEffectAny(Debuffs.Heavy) || HasEffectAny(Debuffs.Silence)))
					{
						return Swift;
					}

					if (IsEnabled(CustomComboPreset.MeleePvP_Smite) && IsOffCooldown(Smite) && HasEffect(Buffs.Smite)
						&& GetTargetHPPercent() <= 25)
					{
						return Smite;
					}
				}

				return actionID;
			}
		}

		internal class RangedPvP_RoleActions : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.RangedPvP_RoleActions;

			protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.RangedPvP_RoleActions) && InCombat() && IsPvPComboAction(actionID))
				{
					if (IsEnabled(CustomComboPreset.RangedPvP_Dervish) && IsOffCooldown(Dervish) && HasEffect(Buffs.Dervish))
					{
						return Dervish;
					}

					if (IsEnabled(CustomComboPreset.RangedPvP_Bravery) && IsOffCooldown(Bravery) && HasEffect(Buffs.Bravery))
					{
						return Bravery;
					}

					if (IsEnabled(CustomComboPreset.RangedPvP_EagleEyeShot) && IsOffCooldown(EagleEyeShot) && HasEffect(Buffs.EagleEyeShot)
						&& (HasEffectAny(Buffs.Guard) || EnemyHealthCurrentHp() <= 12000))
					{
						return EagleEyeShot;
					}
				}

				return actionID;
			}
		}

		internal class MagePvP_RoleActions : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MagePvP_RoleActions;

			protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.MagePvP_RoleActions) && InCombat() && IsPvPComboAction(actionID))
				{
					if (IsEnabled(CustomComboPreset.MagePvP_PhantomDart) && IsOffCooldown(PhantomDart) && HasEffect(Buffs.PhantomDart))
					{
						return PhantomDart;
					}

					if (IsEnabled(CustomComboPreset.MagePvP_Rust) && IsOffCooldown(Rust) && HasEffect(Buffs.Rust))
					{
						return Rust;
					}
				}

				return actionID;
			}
		}
	}
}
