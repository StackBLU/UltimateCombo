using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvP
{
	internal static class RPRPvP
	{
		public const uint
			Slice = 29538,
			WaxingSlice = 29539,
			InfernalSlice = 29540,

			HarvestMoon = 29545,

			GrimSwathe = 29547,
			ExecutionersGuillotine = 41456,

			PlentifulHarvest = 29546,

			DeathWarrant = 29549,
			FateSealed = 41457,

			HellsIngress = 29550,
			Regress = 29551,

			ArcaneCrest = 29552,

			TenebraeLemurum = 29553,
			VoidReaping = 29543,
			CrossReaping = 29544,
			LemuresSlice = 29548,
			Communio = 29554,
			Perfectio = 41458;

		public static class Buffs
		{
			public const ushort
				ExecutionersGuillotineReady = 4307,
				ImmortalSacrifice = 3204,

				DeathWarrant = 4308,

				Threshold = 2860,
				HellsIngress = 3207,

				CrestOfTimeBorrowed = 2861,

				Enshrouded = 2863,
				RipeForReaping = 2858,
				PerfectioParata = 4309;
		}

		internal class Debuffs
		{
			internal const ushort
				DeathWarrant = 3206;
		}

		internal class RPRPvP_Combo : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.RPRPvP_Combo;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Slice or WaxingSlice or InfernalSlice) && IsEnabled(CustomComboPreset.RPRPvP_Combo))
				{
					if (!TargetHasEffectAny(PvPCommon.Buffs.Guard))
					{
						if (CanWeave(actionID))
						{
							if (IsEnabled(CustomComboPreset.RPRPvP_ArcaneCrest) && ActionReady(ArcaneCrest))
							{
								return ArcaneCrest;
							}

							if (IsEnabled(CustomComboPreset.RPRPvP_DeathWarrant) && ActionReady(DeathWarrant) && InActionRange(DeathWarrant))
							{
								return DeathWarrant;
							}

							if (IsEnabled(CustomComboPreset.RPRPvP_Enshrouded) && ActionReady(LemuresSlice) && HasEffect(Buffs.Enshrouded))
							{
								return LemuresSlice;
							}

							if (IsEnabled(CustomComboPreset.RPRPvP_HarvestMoon) && ActionReady(HarvestMoon)
								&& (GetTargetHPPercent() <= 50 || GetRemainingCharges(HarvestMoon) == 2))
							{
								return HarvestMoon;
							}

							if (IsEnabled(CustomComboPreset.RPRPvP_GrimSwathe) && ActionReady(GrimSwathe) && InActionRange(GrimSwathe))
							{
								return GrimSwathe;
							}
						}

						if (IsEnabled(CustomComboPreset.RPRPvP_Enshrouded) && HasEffect(Buffs.Enshrouded) && GetBuffStacks(Buffs.Enshrouded) == 1)
						{
							return Communio;
						}

						if (IsEnabled(CustomComboPreset.RPRPvP_Enshrouded) && HasEffect(Buffs.PerfectioParata))
						{
							return Perfectio;
						}

						if (IsEnabled(CustomComboPreset.RPRPvP_Enshrouded) && HasEffect(Buffs.Enshrouded) && InActionRange(VoidReaping))
						{
							if (HasEffect(Buffs.RipeForReaping))
							{
								return CrossReaping;
							}

							return VoidReaping;
						}

						if (IsEnabled(CustomComboPreset.RPRPvP_PlentifulHarvest) && ActionReady(PlentifulHarvest) && GetBuffStacks(Buffs.ImmortalSacrifice) == 8)
						{
							return PlentifulHarvest;
						}
					}
				}

				return actionID;
			}
		}
	}
}