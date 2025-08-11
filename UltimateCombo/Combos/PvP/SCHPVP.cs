using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvP
{
	internal static class SCHPvP
	{
		public const uint
			Broil4 = 29231,

			Adloquium = 29232,
			Biolysis = 29233,
			DeploymentTactics = 29234,
			Expedient = 29236,

			ChainStratagem = 29716,

			SummonSeraph = 29237,
			SeraphicVeil = 29240,

			Seraphism = 41502,
			SeraphicHalo = 41500,
			Accession = 41501;

		public static class Buffs
		{
			public const ushort
				Galvanize = 3087,
				Catalyze = 3088,

				Expedience = 3092,
				DesperateMeasures = 3093,
				Recitation = 3094,

				SummonSeraph = 3095,
				SeraphicIllumination = 4402,
				SeraphicVeil = 3097,

				Seraphism = 4327,
				SeraphFlight = 3096,
				Consolation = 3098;
		}

		internal class Debuffs
		{
			internal const ushort
				Biolysis = 3089,
				Biolytic = 3090,

				ChainStratagem = 1406;
		}

		internal class SCHPvP_Combo : CustomComboClass
		{
			protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SCHPvP_Combo;

			protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
			{
				if ((actionID is Broil4 or SeraphicHalo) && IsEnabled(CustomComboPreset.SCHPvP_Combo))
				{
					if (IsEnabled(CustomComboPreset.SCHPvP_ChainStratagem) && ActionReady(ChainStratagem)
						&& TargetHasEffectAny(PvPCommon.Buffs.Guard))
					{
						return ChainStratagem;
					}
				}

				if (actionID is Biolysis && IsEnabled(CustomComboPreset.SCHPvP_Biolysis))
				{
					if (!TargetHasEffectAny(PvPCommon.Buffs.Guard))
					{
						return actionID;
					}

					return OriginalHook(11);
				}

				return actionID;
			}
		}
	}
}
