using ECommons.Logging;
using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvP
{
	internal static class RDMPvP
	{
		public const uint
			EnchantedCombo = 29689,

			Resolution = 29695,
			CorpsACorps = 29699,
			Displacement = 29700,

			Verstone = 29683,
			Veraero3 = 29684,

			Verfire = 29686,
			Verthunder3 = 29687,

			MagickBarrier = 29697,

			BlackShift = 29702,

			SouthernCross = 29704;

		public static class Buffs
		{
			public const ushort
				Manafication = 3243,
				WhiteShift = 3245,
				BlackShift = 3246,
				Dualcast = 1393,
				EnchantedRiposte = 3234,
				EnchantedRedoublement = 3236,
				EnchantedZwerchhau = 3235,
				VermilionRadiance = 3233,
				MagickBarrier = 3240;
		}

		public static class Debuffs
		{
			public const ushort
				Monomachy = 3242;
		}

		public static class Config
		{

		}

		internal class RDMPvP_Combo : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.RDMPvP_Combo;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (actionID is Verstone && IsEnabled(CustomComboPreset.RDMPvP_Combo))
				{

					PluginLog.Debug(GetLimitBreakCurrentValue() + " " + GetLimitBreakMaxValue() + " ");
					if (!TargetHasEffectAny(PvPCommon.Buffs.Guard))
					{
						if (!HasEffect(Buffs.BlackShift))
						{
							return OriginalHook(BlackShift);
						}

						if (IsEnabled(CustomComboPreset.RDMPvP_BarrierFrazzle) && ActionReady(OriginalHook(MagickBarrier))
							&& InMeleeRange())
						{
							return OriginalHook(MagickBarrier);
						}

						if (!HasEffect(Buffs.Dualcast))
						{
							if (HasEffect(Buffs.VermilionRadiance))
							{
								if (ActionReady(Displacement) && !HasEffect(Buffs.Manafication) && InActionRange(Displacement))
								{
									return Displacement;
								}

								if (GetLimitBreakCurrentValue() == GetLimitBreakMaxValue())
								{
									if (IsEnabled(CustomComboPreset.RDMPvP_Resolution) && ActionReady(Resolution))
									{
										return OriginalHook(Resolution);
									}

									if (IsEnabled(CustomComboPreset.RDMPvP_Cross))
									{
										return OriginalHook(SouthernCross);
									}
								}

								if (InActionRange(OriginalHook(EnchantedCombo)))
								{
									return OriginalHook(EnchantedCombo);
								}
							}

							if (ActionReady(OriginalHook(EnchantedCombo)))
							{
								if (ActionReady(CorpsACorps)
									&& (!TargetHasEffect(Debuffs.Monomachy) || !InActionRange(OriginalHook(EnchantedCombo))))
								{
									return CorpsACorps;
								}

								if (IsEnabled(CustomComboPreset.RDMPvP_Resolution) && ActionReady(OriginalHook(Resolution))
									&& GetLimitBreakCurrentValue() <= GetLimitBreakMaxValue() - 500
									&& !TargetHasEffectAny(PvPCommon.Buffs.Resilience))
								{
									return OriginalHook(Resolution);
								}

								if (InActionRange(OriginalHook(EnchantedCombo)))
								{
									return OriginalHook(EnchantedCombo);
								}
							}
						}
					}
				}

				return actionID;
			}
		}
	}
}