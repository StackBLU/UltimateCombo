using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvE.Content
{
	public static class Eureka
	{
		public const uint
			Aetherweaver = 12958,
			Martialist = 12959,
			Platebearer = 12960,
			Guardian = 12961,
			Ordained = 12962,
			Skirmisher = 12963,
			Watcher = 12964,
			Templar = 12965,
			Irregular = 12966,
			Breathtaker = 12967,
			Remembered = 12968,
			Elder = 14477,
			Duelist = 14478,
			Fiendhunter = 14479,
			Indomitable = 14480,

			Bloodbath = 12985,
			MagicBurst = 13005,
			DoubleEdge = 13006,
			EagleEyeShot = 13007,

			Cure = 12989,
			Cure2 = 12990,

			Swift = 12975,
			Featherfoot = 12976,
			Feint = 12980,
			Backstep = 12983,

			Paralyze = 12973,
			SpiritDart = 12977,
			Dispel = 12979,
			Tranquilizer = 12984,

			Protect = 12969,
			Shell = 12970,
			Stoneskin = 12991;

		public static class Buffs
		{
			public const ushort
				Aetherweaver = 1631,
				Martialist = 1632,
				Platebearer = 1633,
				Guardian = 1634,
				Ordained = 1635,
				Skirmisher = 1636,
				Watcher = 1637,
				Templar = 1638,
				Irregular = 1639,
				Breathtaker = 1640,
				Remembered = 1641,
				Elder = 1739,
				Duelist = 1740,
				Fiendhunter = 1741,
				Indomitable = 1742,

				Protect = 1642,
				Shell = 1643,
				Stoneskin = 151;
		}

		public static class Debuffs
		{
			public const ushort
				Feint = 32,
				Paralyze = 17,
				SpiritDart = 1654;
		}

		public static class Items
		{
			public const uint
				EtherKit = 38;
		}

		public static class Config
		{
			public static UserInt
				Eureka_Cure = new("Eureka_Cure", 50),
				Eureka_Cure2 = new("Eureka_Cure2", 50);
		}

		internal class Eureka_Wisdoms : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.Eureka_Wisdoms;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.Eureka_Wisdoms))
				{
					if (DutyActionReady(Aetherweaver) && DutyActionEquipped(Aetherweaver) && !HasEffect(Buffs.Aetherweaver))
					{
						return Aetherweaver;
					}

					if (DutyActionReady(Martialist) && DutyActionEquipped(Martialist) && !HasEffect(Buffs.Martialist))
					{
						return Martialist;
					}

					if (DutyActionReady(Platebearer) && DutyActionEquipped(Platebearer) && !HasEffect(Buffs.Platebearer))
					{
						return Platebearer;
					}

					if (DutyActionReady(Guardian) && DutyActionEquipped(Guardian) && !HasEffect(Buffs.Guardian))
					{
						return Guardian;
					}

					if (DutyActionReady(Ordained) && DutyActionEquipped(Ordained) && !HasEffect(Buffs.Ordained))
					{
						return Ordained;
					}

					if (DutyActionReady(Skirmisher) && DutyActionEquipped(Skirmisher) && !HasEffect(Buffs.Skirmisher))
					{
						return Skirmisher;
					}

					if (DutyActionReady(Watcher) && DutyActionEquipped(Watcher) && !HasEffect(Buffs.Watcher))
					{
						return Watcher;
					}

					if (DutyActionReady(Templar) && DutyActionEquipped(Templar) && !HasEffect(Buffs.Templar))
					{
						return Templar;
					}

					if (DutyActionReady(Irregular) && DutyActionEquipped(Irregular) && !HasEffect(Buffs.Irregular))
					{
						return Irregular;
					}

					if (DutyActionReady(Breathtaker) && DutyActionEquipped(Breathtaker) && !HasEffect(Buffs.Breathtaker))
					{
						return Breathtaker;
					}

					if (DutyActionReady(Remembered) && DutyActionEquipped(Remembered) && !HasEffect(Buffs.Remembered))
					{
						return Remembered;
					}

					if (DutyActionReady(Elder) && DutyActionEquipped(Elder) && !HasEffect(Buffs.Elder))
					{
						return Elder;
					}

					if (DutyActionReady(Duelist) && DutyActionEquipped(Duelist) && !HasEffect(Buffs.Duelist))
					{
						return Duelist;
					}

					if (DutyActionReady(Fiendhunter) && DutyActionEquipped(Fiendhunter) && !HasEffect(Buffs.Fiendhunter))
					{
						return Fiendhunter;
					}

					if (DutyActionReady(Indomitable) && DutyActionEquipped(Indomitable) && !HasEffect(Buffs.Indomitable))
					{
						return Indomitable;
					}
				}

				return actionID;
			}
		}

		internal class Eureka_Offensive : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.Eureka_Offensive;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.Eureka_Offensive) && InCombat() && SafeToUse())
				{
					if (IsEnabled(CustomComboPreset.Eureka_Bloodbath) && DutyActionReady(Bloodbath) && DutyActionEquipped(Bloodbath))
					{
						return Bloodbath;
					}

					if (IsEnabled(CustomComboPreset.Eureka_MagicBurst) && DutyActionReady(MagicBurst) && DutyActionEquipped(MagicBurst))
					{
						return MagicBurst;
					}

					if (IsEnabled(CustomComboPreset.Eureka_DoubleEdge) && DutyActionReady(DoubleEdge) && DutyActionEquipped(DoubleEdge))
					{
						return DoubleEdge;
					}

					if (IsEnabled(CustomComboPreset.Eureka_EagleEyeShot) && DutyActionReady(EagleEyeShot) && DutyActionEquipped(EagleEyeShot))
					{
						return EagleEyeShot;
					}
				}

				return actionID;
			}
		}

		internal class Eureka_Curative : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.Eureka_Curative;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.Eureka_Curative) && SafeToUse())
				{
					if (IsEnabled(CustomComboPreset.Eureka_Cure) && DutyActionReady(Cure) && DutyActionEquipped(Cure)
						&& (PlayerHealthPercentageHp() < GetOptionValue(Config.Eureka_Cure)
						|| (HasFriendlyTarget() && GetTargetHPPercent() < GetOptionValue(Config.Eureka_Cure))))
					{
						return Cure;
					}

					if (IsEnabled(CustomComboPreset.Eureka_Cure2) && DutyActionReady(Cure2) && DutyActionEquipped(Cure2)
						&& (PlayerHealthPercentageHp() < GetOptionValue(Config.Eureka_Cure2)
						|| (HasFriendlyTarget() && GetTargetHPPercent() < GetOptionValue(Config.Eureka_Cure2))))
					{
						return Cure2;
					}
				}

				return actionID;
			}
		}

		internal class Eureka_Tactical : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.Eureka_Tactical;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.Eureka_Tactical) && InCombat() && SafeToUse())
				{
					if (IsEnabled(CustomComboPreset.Eureka_Swift) && DutyActionReady(Swift) && DutyActionEquipped(Swift))
					{
						return Swift;
					}

					if (IsEnabled(CustomComboPreset.Eureka_Featherfoot) && DutyActionReady(Featherfoot) && DutyActionEquipped(Featherfoot))
					{
						return Featherfoot;
					}

					if (IsEnabled(CustomComboPreset.Eureka_Feint) && DutyActionReady(Feint) && DutyActionEquipped(Feint))
					{
						return Feint;
					}
				}

				return actionID;
			}
		}

		internal class Eureka_Inimical : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.Eureka_Inimical;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.Eureka_Inimical) && InCombat() && SafeToUse())
				{
					if (IsEnabled(CustomComboPreset.Eureka_Paralyze) && DutyActionReady(Paralyze) && DutyActionEquipped(Paralyze))
					{
						return Paralyze;
					}

					if (IsEnabled(CustomComboPreset.Eureka_SpiritDart) && DutyActionReady(SpiritDart) && DutyActionEquipped(SpiritDart))
					{
						return SpiritDart;
					}

					if (IsEnabled(CustomComboPreset.Eureka_Tranquilizer) && DutyActionReady(Tranquilizer) && DutyActionEquipped(Tranquilizer))
					{
						return Tranquilizer;
					}
				}

				return actionID;
			}
		}

		internal class Eureka_Mitigative : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.Eureka_Mitigative;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (IsEnabled(CustomComboPreset.Eureka_Mitigative) && SafeToUse())
				{
					if (IsEnabled(CustomComboPreset.Eureka_Protect) && DutyActionReady(Protect) && DutyActionEquipped(Protect))
					{
						return Protect;
					}

					if (IsEnabled(CustomComboPreset.Eureka_Shell) && DutyActionReady(Shell) && DutyActionEquipped(Shell))
					{
						return Shell;
					}

					if (IsEnabled(CustomComboPreset.Eureka_Stoneskin) && DutyActionReady(Stoneskin) && DutyActionEquipped(Stoneskin))
					{
						return Stoneskin;
					}
				}

				return actionID;
			}
		}
	}
}