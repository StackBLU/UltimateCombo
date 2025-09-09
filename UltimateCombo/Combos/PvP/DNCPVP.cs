using UltimateCombo.Combos.General;
using UltimateCombo.Core;

namespace UltimateCombo.Combos.PvP;

internal static class DNCPvP
{
    internal const uint
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

    internal static class Buffs
    {
        internal const ushort
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

    internal class DNCPvP_Combo : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.DNCPvP_Combo;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is Cascade or Fountain or ReverseCascade or Fountainfall or SaberDance)
                && IsEnabled(Presets.DNCPvP_Combo))
            {
                if (!TargetHasEffectAny(AllPvP.Buffs.Guard))
                {
                    if (CanWeave(actionID))
                    {
                        if (IsEnabled(Presets.DNCPvP_FanDance) && ActionReady(FanDance))
                        {
                            return FanDance;
                        }

                        if (IsEnabled(Presets.DNCPvP_CuringWaltz) && ActionReady(CuringWaltz) && CurrentHP <= MaxHP - 8000)
                        {
                            return CuringWaltz;
                        }
                    }

                    if (IsEnabled(Presets.DNCPvP_StarfallDance) && ActionReady(StarfallDance))
                    {
                        return StarfallDance;
                    }
                }
            }

            return actionID;
        }
    }
}
