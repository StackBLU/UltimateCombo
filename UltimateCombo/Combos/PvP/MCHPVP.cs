using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvP
{
	internal static class MCHPvP
	{
		public const uint
			BlastCharge = 29402,
			HeatBlast = 29403,
			Scattergun = 29404,
			Drill = 29405,
			BioBlaster = 29406,
			AirAnchor = 29407,
			ChainSaw = 29408,
			Wildfire = 29409,
			BishopTurret = 29412,
			AetherMortar = 29413,
			Analysis = 29414,
			MarksmanSpite = 29415;

		public static class Buffs
		{
			public const ushort
				Heat = 3148,
				Overheated = 3149,
				DrillPrimed = 3150,
				BioblasterPrimed = 3151,
				AirAnchorPrimed = 3152,
				ChainSawPrimed = 3153,
				Analysis = 3158;
		}

		public static class Debuffs
		{
			public const ushort
				Wildfire = 1323;
		}

		internal class MCHPvP_Combo : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MCHPvP_Combo;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (actionID is BlastCharge && IsEnabled(CustomComboPreset.MCHPvP_Combo))
				{
					if (IsEnabled(CustomComboPreset.MCHPvP_Weapons) && ActionReady(OriginalHook(Drill)) && !HasEffect(Buffs.Overheated)
						&& OriginalHook(Drill) == Drill && !WasLastWeaponskill(Drill)
						&& (ActionReady(Analysis) || HasEffect(Buffs.Analysis)))
					{
						if (!HasEffect(Buffs.Analysis))
						{
							return Analysis;
						}

						return Drill;
					}

					if (!TargetHasEffectAny(PvPCommon.Buffs.Guard))
					{
						if (IsEnabled(CustomComboPreset.MCHPvP_Spite) && GetLimitBreakCurrentValue() == GetLimitBreakMaxValue()
							&& WasLastWeaponskill(AirAnchor) && !HasEffect(Buffs.Overheated) && EnemyHealthCurrentHp() >= 12000)
						{
							return MarksmanSpite;
						}

						if (CanWeave(actionID))
						{
							if (IsEnabled(CustomComboPreset.MCHPvP_Wildfire) && ActionReady(Wildfire) && HasEffect(Buffs.Overheated)
								&& WasLastWeaponskill(BlastCharge))
							{
								return Wildfire;
							}
						}

						if (IsEnabled(CustomComboPreset.MCHPvP_Scattergun) && ActionReady(Scattergun) && !HasEffect(Buffs.Overheated)
							&& (WasLastWeaponskill(HeatBlast) || InMeleeRange()) && InActionRange(Scattergun))
						{
							return Scattergun;
						}

						if (!HasEffect(Buffs.Overheated))
						{
							if (IsEnabled(CustomComboPreset.MCHPvP_Weapons) && ActionReady(OriginalHook(Drill))
								&& OriginalHook(Drill) == BioBlaster && !WasLastWeaponskill(BioBlaster) && InActionRange(BioBlaster))
							{
								return BioBlaster;
							}

							if (IsEnabled(CustomComboPreset.MCHPvP_Weapons) && ActionReady(OriginalHook(Drill))
								&& OriginalHook(Drill) == AirAnchor && !WasLastWeaponskill(AirAnchor)
								&& (ActionReady(Analysis) || HasEffect(Buffs.Analysis)))
							{
								if (!HasEffect(Buffs.Analysis))
								{
									return Analysis;
								}

								return AirAnchor;
							}

							if (IsEnabled(CustomComboPreset.MCHPvP_Weapons) && ActionReady(OriginalHook(Drill))
								&& OriginalHook(Drill) == ChainSaw && !WasLastWeaponskill(ChainSaw))
							{
								return ChainSaw;
							}
						}
					}
				}

				return actionID;
			}
		}
	}
}