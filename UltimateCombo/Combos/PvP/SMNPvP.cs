using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvP
{
	internal static class SMNPvP
	{
		internal const uint
			Ruin3 = 29664,
			AstralImpulse = 29665,
			FountainOfFire = 29666,
			CrimsonCyclone = 29667,
			CrimsonStrike = 29668,
			Slipstream = 29669,
			RadiantAegis = 29670,
			MountainBuster = 29671,
			Fester = 29672,
			EnkindleBahamut = 29674,
			Megaflare = 29675,
			Wyrmwave = 29676,
			AkhMorn = 29677,
			EnkindlePhoenix = 29679,
			ScarletFlame = 29681,
			Revelation = 29682;

		public static class Config
		{

		}

		internal class SMNPvP_Combo : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SMNPvP_Combo;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (actionID is Ruin3 && IsEnabled(CustomComboPreset.SMNPvP_Combo))
				{
					if (!TargetHasEffectAny(PvPCommon.Buffs.Guard))
					{
						if (CanWeave(actionID))
						{
							if (IsEnabled(CustomComboPreset.SMNPvP_Enkindle))
							{
								if (ActionReady(EnkindleBahamut) && IsEnabled(EnkindleBahamut))
								{
									return EnkindleBahamut;
								}

								if (ActionReady(EnkindlePhoenix) && IsEnabled(EnkindlePhoenix))
								{
									return EnkindlePhoenix;
								}
							}

							if (IsEnabled(CustomComboPreset.SMNPvP_Mountain) && ActionReady(MountainBuster)
								&& (WasLastSpell(CrimsonCyclone) || WasLastSpell(CrimsonStrike))
								&& !TargetHasEffectAny(PvPCommon.Buffs.Resilience))
							{
								return MountainBuster;
							}

							if (IsEnabled(CustomComboPreset.SMNPvP_Aegis) && ActionReady(RadiantAegis))
							{
								return RadiantAegis;
							}

							if (IsEnabled(CustomComboPreset.SMNPvP_Fester) && ActionReady(Fester)
								&& (GetTargetHPPercent() <= 50 || GetRemainingCharges(Fester) == 2)
								&& HasTarget())
							{
								return Fester;
							}
						}

						if (IsEnabled(CustomComboPreset.SMNPvP_Slipstream) && ActionReady(Slipstream) && !IsMoving)
						{
							return Slipstream;
						}

						if (lastComboMove is CrimsonCyclone)
						{
							return CrimsonStrike;
						}

						if (IsEnabled(CustomComboPreset.SMNPvP_Crimson) && ActionReady(CrimsonCyclone))
						{
							return CrimsonCyclone;
						}
					}
				}

				return actionID;
			}
		}
	}
}