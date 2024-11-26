using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvP
{
	internal static class PLDPvP
	{
		public const uint
			FastBlade = 29058,
			RiotBlade = 29059,
			RoyalAuthority = 29060,
			Atonement = 29061,
			Supplication = 41428,
			Sepulchre = 41429,

			HolySpirit = 29062,
			Intervene = 29065,
			Guardian = 29066,
			HolySheltron = 29067,
			ShieldSmite = 41430,
			Imperator = 41431,
			Confiteor = 42194,

			Phalanx = 29069,
			BladeOfFaith = 29071,
			BladeOfTruth = 29072,
			BladeOfValor = 29073;

		public static class Buffs
		{
			public const ushort
				AtonementReady = 2015,
				SupplicationReady = 2016,
				SepulchreReady = 2017,

				HolySheltron = 3026,
				SwordOath = 1991,
				ShieldOath = 3188,

				ConfiteorReady = 3028,

				Cover = 1300,
				Covered = 1301,

				HallowedGround = 1302,
				Phalanx = 3210,
				BladeOfFaithReady = 3250;
		}

		public static class Debuffs
		{
			public const ushort
				SacredClaim = 3025,
				ShieldSmite = 4283;
		}

		public static class Config
		{
			public static UserInt
				PLDPvP_Phalanx = new("PLDPvP_Phalanx", 25);
		}

		internal class PLDPvP_Combo : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.PLDPvP_Combo;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is FastBlade or RiotBlade or RoyalAuthority or Atonement or Supplication or Sepulchre)
					&& IsEnabled(CustomComboPreset.PLDPvP_Combo))
				{
					if (IsEnabled(CustomComboPreset.PLDPvP_Phalanx) && GetLimitBreakCurrentValue() == GetLimitBreakMaxValue()
						&& (PlayerHealthPercentageHp() <= GetOptionValue(Config.PLDPvP_Phalanx) || WasLastAbility(Guardian)))
					{
						return Phalanx;
					}

					if (IsEnabled(CustomComboPreset.PLDPvP_AutoGuard) && GetLimitBreakCurrentValue() != GetLimitBreakMaxValue()
						&& WasLastAbility(Guardian) && ActionReady(PvPCommon.Guard) && !HasEffect(Buffs.HallowedGround))
					{
						return PvPCommon.Guard;
					}

					if (IsEnabled(CustomComboPreset.PLDPvP_ShieldSmite) && ActionReady(ShieldSmite)
						&& InActionRange(ShieldSmite) && TargetHasEffectAny(PvPCommon.Buffs.Guard))
					{
						return ShieldSmite;
					}

					if (!TargetHasEffectAny(PvPCommon.Buffs.Guard))
					{
						if (CanWeave(actionID))
						{
							if (IsEnabled(CustomComboPreset.PLDPvP_Imperator) && ActionReady(Imperator) && InActionRange(Imperator))
							{
								return Imperator;
							}

							if (IsEnabled(CustomComboPreset.PLDPvP_HolySheltron) && ActionReady(HolySheltron))
							{
								return HolySheltron;
							}
						}

						if (IsEnabled(CustomComboPreset.PLDPvP_Blades)
							&& (HasEffect(Buffs.BladeOfFaithReady) || lastComboMove is BladeOfFaith || lastComboMove is BladeOfTruth))
						{
							return OriginalHook(Phalanx);
						}

						if (IsEnabled(CustomComboPreset.PLDPvP_Confiteor) && HasEffect(Buffs.ConfiteorReady))
						{
							return Confiteor;
						}

						if (IsEnabled(CustomComboPreset.PLDPvP_HolySpirit) && ActionReady(HolySpirit)
							&& LocalPlayer.CurrentHp <= LocalPlayer.MaxHp - 8000)
						{
							return HolySpirit;
						}

						if (IsEnabled(CustomComboPreset.PLDPvP_Intervene) && ActionReady(Intervene))
						{
							return Intervene;
						}
					}
				}

				return actionID;
			}
		}
	}
}