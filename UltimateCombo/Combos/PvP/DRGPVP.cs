using UltimateCombo.Combos.General;
using UltimateCombo.Core;

namespace UltimateCombo.Combos.PvP;

internal static class DRGPvP
{
    internal const uint
        RaidenThrust = 29486,
        FangAndClaw = 29487,
        WheelingThrust = 29488,
        Drakesbane = 41449,

        ChaoticSpring = 29490,

        Geirskogul = 29491,
        Nastrond = 29492,
        Starcross = 41450,

        HighJump = 29493,
        HorridRoar = 29496,
        HeavensThrust = 29489,

        ElusiveJump = 29494,
        WyrmwindThrust = 29495,

        SkyHigh = 29497,
        SkyShatter = 29499;

    internal static class Buffs
    {
        internal const ushort
            LifeOfTheDragon = 3177,
            NastrondReady = 4404,
            StarcrossReady = 4302,

            Heavensent = 3176,

            ElusiveJump = 3209,
            FirstmindsFocus = 3178,

            SkyHigh = 3180,
            SkyShatter = 3183;
    }

    internal static class Debuffs
    {
        internal const ushort
            HorridRoad = 3179;
    }

    internal class DRGPvP_Combo : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.DRGPvP_Combo;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is RaidenThrust or FangAndClaw or WheelingThrust or Drakesbane or HeavensThrust)
                && IsEnabled(Presets.DRGPvP_Combo))
            {
                if (IsEnabled(Presets.DRGPvP_ChaoticSpring) && ActionReady(ChaoticSpring)
                    && CurrentHP <= MaxHP - 12000 && InActionRange(ChaoticSpring))
                {
                    return ChaoticSpring;
                }

                if (!TargetHasEffectAny(AllPvP.Buffs.Guard))
                {
                    if (CanWeave(actionID))
                    {
                        if (IsEnabled(Presets.DRGPvP_HorridRoar) && ActionReady(HorridRoar) && WasLastAbility(HighJump))
                        {
                            return HorridRoar;
                        }
                    }

                    if (IsEnabled(Presets.DRGPvP_Nastrond) && HasEffect(Buffs.NastrondReady) && EffectRemainingTime(Buffs.NastrondReady) <= 1)
                    {
                        return Nastrond;
                    }

                    if (IsEnabled(Presets.DRGPvP_WyrmwindThrust) && HasEffect(Buffs.FirstmindsFocus)
                        && HasTarget() && GetTargetDistance() > 15)
                    {
                        return WyrmwindThrust;
                    }
                }
            }

            return actionID;
        }
    }
}
