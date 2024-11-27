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
			Sprint = 29057;

		internal class Config
		{
			public const string
				QuickPurifyStatuses = "QuickPurifyStatuses";
			public static UserInt
				EmergencyHealThreshold = new("EmergencyHealThreshold", 90),
				EmergencyGuardThreshold = new("EmergencyGuardThreshold", 50);
		}

		internal class Debuffs
		{
			public const ushort
				Silence = 1347,
				Bind = 1345,
				Stun = 1343,
				HalfAsleep = 3022,
				Sleep = 1348,
				DeepFreeze = 3219,
				Heavy = 1344,
				Unguarded = 3021;
		}

		internal class Buffs
		{
			public const ushort
				Guard = 3054,
				Resilience = 3248;
		}

		internal static readonly List<uint>
			GlobalSkills = [Teleport, Guard, Recuperate, Purify, StandardElixir, Sprint];

		internal class GlobalEmergencyHeals : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.PvP_EmergencyHeals;

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

				if (Execute() && InPvP() && !GlobalSkills.Contains(actionID))
				{
					if ((LocalPlayer.ClassJob.Value.RowId == DRK.JobID && IsOffCooldown(DRKPvP.Impalement))
						|| (HasEffect(DRKPvP.Buffs.UndeadRedemption) && GetBuffRemainingTime(DRKPvP.Buffs.UndeadRedemption) > 2))
					{
						return actionID;
					}

					if (LocalPlayer.ClassJob.Value.RowId == NIN.JobID && HasEffect(NINPvP.Buffs.Hidden))
					{
						return actionID;
					}

					if (LocalPlayer.ClassJob.Value.RowId == VPR.JobID && actionID is VPRPvP.SnakeScales)
					{
						return actionID;
					}

					return OriginalHook(Recuperate);
				}

				return actionID;
			}

			public static bool Execute()
			{
				uint jobMaxHp = LocalPlayer.MaxHp;
				int threshold = GetOptionValue(Config.EmergencyHealThreshold);
				uint maxHPThreshold = jobMaxHp - 15000;
				float remainingPercentage = LocalPlayer.CurrentHp / (float)maxHPThreshold;


				if (HasEffect(3180))
				{
					return false; //DRG LB buff
				}

				if (HasEffectAny(1420))
				{
					return false; //Rival Wings Mounted
				}

				return LocalPlayer.CurrentMp >= 2500 && remainingPercentage * 100 <= threshold;
			}
		}

		internal class GlobalEmergencyGuard : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.PvP_EmergencyGuard;

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

				if (Execute() && InPvP() && !GlobalSkills.Contains(actionID))
				{
					if (LocalPlayer.ClassJob.Value.RowId == DRK.JobID && HasEffect(DRKPvP.Buffs.UndeadRedemption))
					{
						return actionID;
					}

					if (LocalPlayer.ClassJob.Value.RowId == NIN.JobID && HasEffect(NINPvP.Buffs.Hidden))
					{
						return actionID;
					}

					if (LocalPlayer.ClassJob.Value.RowId == VPR.JobID && actionID is VPRPvP.SnakeScales)
					{
						return actionID;
					}

					return OriginalHook(Guard);
				}

				return actionID;
			}

			public static bool Execute()
			{
				uint jobMaxHp = LocalPlayer.MaxHp;
				int threshold = GetOptionValue(Config.EmergencyGuardThreshold);
				float remainingPercentage = LocalPlayer.CurrentHp / (float)jobMaxHp;

				if (HasEffect(3180))
				{
					return false; //DRG LB buff
				}

				if (HasEffectAny(1420))
				{
					return false; //Rival Wings Mounted
				}

				return !HasEffect(DRKPvP.Buffs.UndeadRedemption)
					&& !HasEffectAny(Debuffs.Unguarded) && !HasEffect(WARPvP.Buffs.InnerRelease)
					&& !GetCooldown(Guard).IsCooldown && remainingPercentage * 100 <= threshold;
			}
		}

		internal class QuickPurify : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.PvP_QuickPurify;

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

				if (LocalPlayer.ClassJob.Value.RowId == NIN.JobID && HasEffect(NINPvP.Buffs.Hidden))
				{
					return actionID;
				}

				if (LocalPlayer.ClassJob.Value.RowId == VPR.JobID && actionID is VPRPvP.SnakeScales)
				{
					return actionID;
				}

				if (Execute() && InPvP() && !GlobalSkills.Contains(actionID))
				{
					return OriginalHook(Purify);
				}

				return actionID;
			}

			public static bool Execute()
			{
				bool[] selectedStatuses = PluginConfiguration.GetCustomBoolArrayValue(Config.QuickPurifyStatuses);

				if (HasEffect(3180))
				{
					return false; //DRG LB buff
				}

				if (HasEffectAny(1420))
				{
					return false; //Rival Wings Mounted
				}

				return selectedStatuses.Length != 0
					&& !GetCooldown(Purify).IsCooldown
					&& ((HasEffectAny(Debuffs.Stun) && selectedStatuses[0])
					|| (HasEffectAny(Debuffs.DeepFreeze) && selectedStatuses[1])
					|| (HasEffectAny(Debuffs.HalfAsleep) && selectedStatuses[2])
					|| (HasEffectAny(Debuffs.Sleep) && selectedStatuses[3])
					|| (HasEffectAny(Debuffs.Bind) && selectedStatuses[4])
					|| (HasEffectAny(Debuffs.Heavy) && selectedStatuses[5])
					|| (HasEffectAny(Debuffs.Silence) && selectedStatuses[6]));
			}
		}

		internal class IgnoreSAMKuzuchi : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.PvP_IgnoreSAMKuzuchi;

			protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.PvP_IgnoreSAMKuzuchi) && TargetHasEffectAny(SAMPvP.Buffs.Chiten))
				{
					return OriginalHook(11);
				}

				return actionID;
			}
		}
	}
}