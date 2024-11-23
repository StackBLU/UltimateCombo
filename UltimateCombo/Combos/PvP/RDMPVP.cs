using UltimateCombo.ComboHelper.Functions;

namespace UltimateCombo.Combos.PvP
{
	internal static class RDMPvP
	{
		public const uint
			Ruin3 = 29664,
			Necrotize = 41483,
			Ruin4 = 41482,
			MountainBuster = 29671,
			Slipstream = 29669,
			CrimsonCyclone = 29667,
			CrimsonStrike = 29668,
			RadiantAegis = 29670,

			SummonBahamut = 29673,
			SummonPhoenix = 29678,
			Deathflare = 41484,
			BrandOfPurgatory = 41485,

			AstralImpulse = 29665,
			FountainOfFire = 29666,

			//Unused (Pet skills)
			Megaflare = 29675,
			Wyrmwave = 29676,
			EverlastingFlight = 29680,
			ScarletFlame = 29681;

		public static class Buffs
		{
			public const ushort
				FurtherRuin = 4399,
				Slipstream = 3226,
				CrimsonStrikeReady = 4400,
				RadiantAegis = 3224,

				DreadwyrmTrance = 3228,
				FirebirdTrance = 3229,
				EverlastingFlight = 3230;
		}

		internal class Debuffs
		{
			internal const ushort
				Slipping = 3227,
				ScarletFlame = 3231,
				Revelation = 3232;
		}

		public static class Config
		{
			public static UserInt
				WARPvP_Bloodwhetting = new("WARPvP_Bloodwhetting", 50);
		}

		/*internal class SMNPvP_Combo : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SMNPvP_Combo;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Ruin3 or Ruin4) && IsEnabled(CustomComboPreset.SMNPvP_Combo))
				{
					if (IsEnabled(CustomComboPreset.WARPvP_PrimalScream) && GetLimitBreakCurrentValue() == GetLimitBreakMaxValue())
					{
						return PrimalScream;
					}

					if (!TargetHasEffectAny(PvPCommon.Buffs.Guard))
					{
						if (CanWeave(actionID))
						{
							if (IsEnabled(CustomComboPreset.SMNPvP_Deathflare) && IsEnabled(Deathflare))
							{
								return Deathflare;
							}

							if (IsEnabled(CustomComboPreset.SMNPvP_BrandOfPurgatory) && IsEnabled(BrandOfPurgatory))
							{
								return BrandOfPurgatory;
							}

							if (IsEnabled(CustomComboPreset.SMNPvP_CrimsonCyclone) && ActionReady(CrimsonCyclone))
							{
								return CrimsonCyclone;
							}

							if (IsEnabled(CustomComboPreset.SMNPvP_Necrotize) && ActionReady(Necrotize)
								&& !HasEffect(Buffs.FurtherRuin))
							{
								return Necrotize;
							}

							if (IsEnabled(CustomComboPreset.SMNPvP_RadiantAegis) && ActionReady(RadiantAegis))
							{
								return RadiantAegis;
							}
						}

						if (IsEnabled(CustomComboPreset.SMNPvP_Slipstream) && ActionReady(Slipstream)
							&& !IsMoving)
						{
							return Slipstream;
						}

						if (IsEnabled(CustomComboPreset.SMNPvP_MountainBuster) && ActionReady(MountainBuster))
						{
							return MountainBuster;
						}

						if (IsEnabled(CustomComboPreset.SMNPvP_CrimsonStrike) && HasEffect(Buffs.CrimsonStrikeReady))
						{
							return CrimsonCyclone;
						}
					}
				}

				return actionID;
			}
		}*/
	}
}