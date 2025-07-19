using UltimateCombo.CustomCombo;

namespace UltimateCombo.Combos.PvP
{
    internal static class DNCPvP
    {
        public const uint
            Cascade = 29416,
            Fountain = 29417,
            EnAvant = 29430,
            ReverseCascade = 29418,
            Fountainfall = 29419,
            SaberDance = 29420,

            StarfallDance = 29421,
            FanDance = 29428,

            HoningDance = 29422,
            HoningOvation = 29423,

            ClosedPosition = 29431,
            CuringWaltz = 29429,

            Contradance = 29432,
            DanceOfTheDawn = 41472;

        public static class Buffs
        {
            public const ushort
                ClosedPosition = 2026,
                DancePartner = 2027,

                EnAvant = 2048,
                Bladecatcher = 3159,
                FlourishingSaberDance = 3160,
                SaberDance = 2022,

                HoningDance = 3162,
                Acclaim = 3163,
                HoningOvation = 3164,

                StarfallDance = 3161,
                FanDance = 2052;
        }

        internal class Debuffs
        {
            internal const ushort
                Seduced = 3024;
        }

        internal class DNCPvP_Combo : CustomComboClass
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DNCPvP_Combo;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if ((actionID is Cascade or Fountain or ReverseCascade or Fountainfall or SaberDance)
                    && IsEnabled(CustomComboPreset.DNCPvP_Combo))
                {
                    if (!TargetHasEffectAny(PvPCommon.Buffs.Guard))
                    {
                        if (CanWeave(actionID))
                        {
                            if (IsEnabled(CustomComboPreset.DNCPvP_FanDance) && ActionReady(FanDance))
                            {
                                return FanDance;
                            }

                            if (IsEnabled(CustomComboPreset.DNCPvP_CuringWaltz) && ActionReady(CuringWaltz) && LocalPlayer?.CurrentHp <= LocalPlayer?.MaxHp - 8000)
                            {
                                return CuringWaltz;
                            }
                        }

                        if (IsEnabled(CustomComboPreset.DNCPvP_StarfallDance) && ActionReady(StarfallDance))
                        {
                            return StarfallDance;
                        }
                    }
                }

                return actionID;
            }
        }
    }
}
