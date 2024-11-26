using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvP
{
	internal static class GNBPvP
	{
		public const uint
			KeenEdge = 29098,
			BrutalShell = 29099,
			SolidBarrel = 29100,
			BurstStrike = 29101,
			Hypervelocity = 29107,

			Continuation = 29106,

			GnashingFang = 29102,
			JugularRip = 29108,
			SavageClaw = 29103,
			AbdomenTear = 29109,
			WickedTalon = 29104,
			EyeGouge = 29110,

			FatedCircle = 41511,
			FatedBrand = 41442,

			RoughDivide = 29123,
			BlastingZone = 29128,
			HeartOfCorundum = 41443,

			RelentlessRush = 29130,
			TerminalTrigger = 29469;

		public static class Buffs
		{
			public const ushort
				NoMercy = 3042,

				ReadyToBlast = 3041,
				Hypervelocity = 3047,

				ReadyToRip = 2002,
				JugularRip = 3048,

				ReadyToTear = 2003,
				AbdomenTear = 3049,

				ReadyToGouge = 2004,
				EyeGouge = 3050,

				HeartOfCorundum = 4295,
				CatharsisOfCorundum = 4296,

				ReadyToRaze = 4293,
				Nebula = 3051,
				FatedBrand = 4294,

				RelentlessRush = 3052;
		}

		public static class Debuffs
		{
			public const ushort
				RelentlessShrapnel = 3053;
		}

		internal class GNBPvP_Combo : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.GNBPvP_Combo;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is KeenEdge or BrutalShell or SolidBarrel or BurstStrike) && IsEnabled(CustomComboPreset.GNBPvP_Combo))
				{
					if (IsEnabled(CustomComboPreset.GNBPvP_RelentlessRush) && GetLimitBreakCurrentValue() == GetLimitBreakMaxValue())
					{
						return RelentlessRush;
					}

					if (!TargetHasEffectAny(PvPCommon.Buffs.Guard))
					{
						if (CanWeave(actionID))
						{
							if (IsEnabled(CustomComboPreset.GNBPvP_Continuation) && ActionReady(OriginalHook(Continuation)))
							{
								return OriginalHook(Continuation);
							}

							if (IsEnabled(CustomComboPreset.GNBPvP_HeartOfCorundum) && ActionReady(HeartOfCorundum))
							{
								return HeartOfCorundum;
							}

							if (IsEnabled(CustomComboPreset.GNBPvP_RoughDivide) && ActionReady(RoughDivide)
								&& (!InMeleeRange() || !HasEffect(Buffs.NoMercy)))
							{
								return RoughDivide;
							}

							if (IsEnabled(CustomComboPreset.GNBPvP_BlastingZone) && ActionReady(BlastingZone)
								&& GetTargetHPPercent() <= 50 && HasBattleTarget())
							{
								return BlastingZone;
							}
						}

						if (IsEnabled(CustomComboPreset.GNBPvP_FatedCircle) && ActionReady(FatedCircle))
						{
							return FatedCircle;
						}

						if (IsEnabled(CustomComboPreset.GNBPvP_GnashingCombo) && ActionReady(OriginalHook(GnashingFang)))
						{
							return OriginalHook(GnashingFang);
						}
					}
				}

				return actionID;
			}
		}
	}
}