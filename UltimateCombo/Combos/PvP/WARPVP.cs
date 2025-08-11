using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvP
{
	internal static class WARPvP
	{
		public const uint
			HeavySwing = 29074,
			Maim = 29075,
			StormsPath = 29076,
			FellCleave = 29078,

			Onslaught = 29079,
			Orogeny = 29080,

			Blota = 29081,
			InnerChaos = 29077,

			Bloodwhetting = 29082,
			ChaoticCyclone = 29736,

			PrimalRend = 29084,
			PrimalRuination = 41432,

			PrimalScream = 29083,
			PrimalWrath = 41433;

		public static class Buffs
		{
			public const ushort
				Bloodwhetting = 3030,
				StemTheTide = 3031,

				ChaoticCycloneReady = 1992,
				InnerChaosReady = 4284,
				PrimalRuinationReady = 4285,

				InnerRelease = 1303,
				ThrillOfBattle = 3185,
				Wrathful = 4286,
				BurgeoningFury = 4287;
		}
		public static class Debuffs
		{
			public const ushort
				Onslaught = 3029,
				Orogeny = 3256,
				Unguarded = 3021;
		}

		public static class Config
		{
			public static UserInt
				WARPvP_Bloodwhetting = new("WARPvP_Bloodwhetting", 75);
		}

		internal class WARPvP_Combo : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.WARPvP_Combo;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is HeavySwing or Maim or StormsPath or FellCleave)
					&& IsEnabled(CustomComboPreset.WARPvP_Combo))
				{
					if (IsEnabled(CustomComboPreset.WARPvP_PrimalScream) && GetLimitBreakCurrentValue() == GetLimitBreakMaxValue())
					{
						return PrimalScream;
					}

					if (!TargetHasEffectAny(PvPCommon.Buffs.Guard))
					{
						if (CanWeave(actionID))
						{
							if (IsEnabled(CustomComboPreset.WARPvP_Bloodwhetting) && ActionReady(Bloodwhetting)
								&& PlayerHealthPercentageHp() <= GetOptionValue(Config.WARPvP_Bloodwhetting))
							{
								return Bloodwhetting;
							}

							if (IsEnabled(CustomComboPreset.WARPvP_Onslaught) && ActionReady(Onslaught))
							{
								return Onslaught;
							}

							if (IsEnabled(CustomComboPreset.WARPvP_Orogeny) && ActionReady(Orogeny)
								&& InActionRange(Orogeny))
							{
								return Orogeny;
							}
						}

						if (IsEnabled(CustomComboPreset.WARPvP_PrimalRend) && ActionReady(PrimalRend))
						{
							return PrimalRend;
						}

						if (IsEnabled(CustomComboPreset.WARPvP_Blota) && ActionReady(Blota) && !InActionRange(HeavySwing))
						{
							return Blota;
						}

						if (IsEnabled(CustomComboPreset.WARPvP_PrimalRuination) && HasEffect(Buffs.PrimalRuinationReady))
						{
							return PrimalRuination;
						}

						if (IsEnabled(CustomComboPreset.WARPvP_ChaoticCyclone) && HasEffect(Buffs.ChaoticCycloneReady))
						{
							return ChaoticCyclone;
						}

						if (IsEnabled(CustomComboPreset.WARPvP_InnerChaos) && HasEffect(Buffs.InnerChaosReady))
						{
							return InnerChaos;
						}

						if (IsEnabled(CustomComboPreset.WARPvP_PrimalWrath) && HasEffect(Buffs.Wrathful))
						{
							return PrimalWrath;
						}
					}
				}

				return actionID;
			}
		}
	}
}
