using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvP
{
	internal static class DRGPvP
	{
		public const byte ClassID = 4;
		public const byte JobID = 22;

		public const uint
			WheelingThrustCombo = 56,
			RaidenThrust = 29486,
			FangAndClaw = 29487,
			WheelingThrust = 29488,
			ChaoticSpring = 29490,
			Geirskogul = 29491,
			HighJump = 29493,
			ElusiveJump = 29494,
			WyrmwindThrust = 29495,
			HorridRoar = 29496,
			HeavensThrust = 29489,
			Nastrond = 29492,
			Purify = 29056,
			Guard = 29054;


		public static class Buffs
		{
			public const ushort
			FirstmindsFocus = 3178,
			LifeOfTheDragon = 3177,
			Heavensent = 3176;


		}
		internal static class Config
		{

		}

		internal class DRGPvP_Burst : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DRGPvP_Combo;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (actionID is RaidenThrust && IsEnabled(CustomComboPreset.DRGPvP_Combo))
				{
					if (!TargetHasEffectAny(PvPCommon.Buffs.Guard))
					{
						if (IsEnabled(CustomComboPreset.DRGPvP_Geirskogul) && HasEffect(Buffs.LifeOfTheDragon)
							&& GetBuffRemainingTime(Buffs.LifeOfTheDragon) < 2)
						{
							return Nastrond;
						}

						if (CanWeave(actionID))
						{
							if (IsEnabled(CustomComboPreset.DRGPvP_Roar) && ActionReady(HorridRoar) && WasLastAbility(HighJump))
							{
								return HorridRoar;
							}

							if (IsEnabled(CustomComboPreset.DRGPvP_Geirskogul) && ActionReady(Geirskogul))
							{
								return Geirskogul;
							}
						}

						if (IsEnabled(CustomComboPreset.DRGPvP_Wyrmwind) && HasEffect(Buffs.FirstmindsFocus)
							&& GetBuffRemainingTime(Buffs.FirstmindsFocus) < 3)
						{
							return WyrmwindThrust;
						}

						if (IsEnabled(CustomComboPreset.DRGPvP_Chaotic) && ActionReady(ChaoticSpring) && PlayerHealthPercentageHp() <= 100)
						{
							return ChaoticSpring;
						}
					}
				}

				return actionID;
			}
		}
	}
}