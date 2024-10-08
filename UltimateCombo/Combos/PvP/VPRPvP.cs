using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvP
{
	internal static class VPRPvP
	{
		public const uint
				SteelFangs = 39157,
				HuntersSting = 39159,
				BarbarousBite = 39161,

				PiercingFangs = 39158,
				SwiftskinsSting = 39160,
				RavenousBite = 39163,

				HuntersSnap = 39166,

				SerpentsTail = 39183,

				UncoiledFury = 39168,

				Slither = 39184,
				SnakeScales = 39185,
				RattlingCoil = 39189,

				FourthGeneration = 39172,
				Ouroboros = 39173,

				WorldSwallower = 39190;

		public static class Buffs
		{
			public const ushort
				Reawakened = 4094,
				Slither = 4095,
				SnakesBane = 4098;
		}

		public static class Debuffs
		{

		}

		public static class Config
		{

		}

		internal class VPRPvP_Combo : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.VPRPvP_Combo;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is SteelFangs or HuntersSting or BarbarousBite or PiercingFangs or SwiftskinsSting or RavenousBite)
					&& IsEnabled(CustomComboPreset.VPRPvP_Combo))
				{
					if (IsEnabled(CustomComboPreset.VPRPvP_Backlash) && HasEffect(Buffs.SnakesBane))
					{
						return OriginalHook(SnakeScales);
					}

					if (CanWeave(actionID))
					{
						if (IsEnabled(OriginalHook(SerpentsTail)))
						{
							return OriginalHook(SerpentsTail);
						}
					}

					if (!TargetHasEffectAny(PvPCommon.Buffs.Guard))
					{
						if (IsEnabled(CustomComboPreset.VPRPvP_WorldSwallower) && GetLimitBreakCurrentValue() == GetLimitBreakMaxValue())
						{
							return WorldSwallower;
						}

						if (CanWeave(actionID))
						{
							if (IsEnabled(CustomComboPreset.VPRPvP_RattlingCoil) && ActionReady(RattlingCoil)
								&& IsOnCooldown(UncoiledFury) && IsOnCooldown(SnakeScales))
							{
								return RattlingCoil;
							}
						}

						if (HasEffect(Buffs.Reawakened))
						{
							if (WasLastWeaponskill(FourthGeneration))
							{
								if (IsEnabled(CustomComboPreset.VPRPvP_Slither) && ActionReady(Slither) && !HasEffect(Buffs.Slither)
									&& HasTarget())
								{
									return Slither;
								}

								return OriginalHook(Ouroboros);
							}

							return actionID;
						}

						if (IsEnabled(CustomComboPreset.VPRPvP_UncoiledFury) && ActionReady(UncoiledFury))
						{
							if (IsEnabled(CustomComboPreset.VPRPvP_Slither) && ActionReady(Slither) && !HasEffect(Buffs.Slither)
								&& InActionRange(OriginalHook(HuntersSnap)) && HasTarget())
							{
								return Slither;
							}

							return UncoiledFury;
						}

						if (IsEnabled(CustomComboPreset.VPRPvP_HuntersSnap) && ActionReady(OriginalHook(HuntersSnap)))
						{
							if (IsEnabled(CustomComboPreset.VPRPvP_Slither) && ActionReady(Slither) && !HasEffect(Buffs.Slither)
								&& HasTarget())
							{
								return Slither;
							}

							return OriginalHook(HuntersSnap);
						}
					}
				}

				return actionID;
			}
		}
	}
}