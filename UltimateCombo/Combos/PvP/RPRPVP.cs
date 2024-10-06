using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvP
{
	internal static class RPRPvP
	{
		public const byte JobID = 39;

		internal const uint
			Slice = 29538,
			WaxingSlice = 29539,
			InfernalSlice = 29540,
			VoidReaping = 29543,
			CrossReaping = 29544,
			HarvestMoon = 29545,
			PlentifulHarvest = 29546,
			GrimSwathe = 29547,
			LemuresSlice = 29548,
			DeathWarrant = 29549,
			ArcaneCrest = 29552,
			HellsIngress = 29550,
			Regress = 29551,
			Communio = 29554,
			SoulSlice = 29566,
			Guillotine = 34786;

		internal class Buffs
		{
			internal const ushort
				Soulsow = 2750,
				SoulReaver = 2854,
				GallowsOiled = 2856,
				RipeForReaping = 2858,
				Enshrouded = 2863,
				ImmortalSacrifice = 3204,
				PlentifulHarvest = 3205;
		}

		internal class Debuffs
		{
			internal const ushort
				DeathWarrant = 3206;
		}

		public static class Config
		{

		}

		internal class RPRPvP_Combo : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.RPRPvP_Combo;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if (actionID is Slice && IsEnabled(CustomComboPreset.RPRPvP_Combo))
				{
					if (IsEnabled(CustomComboPreset.RPRPvP_Enshrouded) && HasEffect(Buffs.Enshrouded))
					{
						if (GetBuffStacks(Buffs.Enshrouded) == 1)
						{
							return Communio;
						}
					}

					if (IsEnabled(CustomComboPreset.RPRPvP_Enshrouded) && HasEffect(Buffs.Enshrouded) && InActionRange(VoidReaping))
					{
						if (HasEffect(Buffs.RipeForReaping))
						{
							return CrossReaping;
						}

						return VoidReaping;
					}

					if (!TargetHasEffectAny(PvPCommon.Buffs.Guard))
					{
						if (CanWeave(actionID))
						{
							if (IsEnabled(CustomComboPreset.RPRPvP_Crest) && ActionReady(ArcaneCrest))
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

							if (IsEnabled(CustomComboPreset.RPRPvP_DeathWarrant) && HasEffect(Buffs.Soulsow)
								&& (GetTargetHPPercent() <= 50 || GetBuffRemainingTime(Buffs.Soulsow) < 1))
							{
								return HarvestMoon;
							}

							if (IsEnabled(CustomComboPreset.RPRPvP_GrimSwathe) && ActionReady(GrimSwathe) && InActionRange(GrimSwathe))
							{
								return GrimSwathe;
							}
						}

						if (IsEnabled(CustomComboPreset.RPRPvP_Plentiful) && ActionReady(PlentifulHarvest))
						{
							return PlentifulHarvest;
						}

						if (HasEffect(Buffs.SoulReaver) && InActionRange(Guillotine))
						{
							return Guillotine;
						}

						if (IsEnabled(CustomComboPreset.RPRPvP_SoulSlice) && ActionReady(SoulSlice) && InActionRange(SoulSlice))
						{
							return SoulSlice;
						}
					}
				}

				return actionID;
			}
		}
	}
}