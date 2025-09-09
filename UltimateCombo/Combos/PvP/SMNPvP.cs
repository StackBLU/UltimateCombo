using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Combos.General;
using UltimateCombo.Core;

namespace UltimateCombo.Combos.PvP;

internal static class SMNPvP
{
    internal const uint
        Ruin3 = 29664,
        Necrotize = 41483,
        Ruin4 = 41482,
        MountainBuster = 29671,
        Slipstream = 29669,
        CrimsonCyclone = 29667,
        CrimsonStrike = 29668,
        RadiantAegis = 29670,

        SummonBahamut = 29673,
        SummonPhoenix = 29678,
        Deathflare = 41484,
        BrandOfPurgatory = 41485,

        AstralImpulse = 29665,
        FountainOfFire = 29666,

        //Unused (Pet skills)
        Megaflare = 29675,
        Wyrmwave = 29676,
        EverlastingFlight = 29680,
        ScarletFlame = 29681;

    internal static class Buffs
    {
        internal const ushort
            FurtherRuin = 4399,
            Slipstream = 3226,
            CrimsonStrikeReady = 4400,
            RadiantAegis = 3224,

            DreadwyrmTrance = 3228,
            FirebirdTrance = 3229,
            EverlastingFlight = 3230;
    }

    internal class Debuffs
    {
        internal const ushort
            Slipping = 3227,
            ScarletFlame = 3231,
            Revelation = 3232;
    }

    internal static class Config
    {
        internal static UserInt
            WARPvP_Bloodwhetting = new("WARPvP_Bloodwhetting", 50);
    }

    internal class SMNPvP_Combo : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.SMNPvP_Combo;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is Ruin3 or Ruin4) && IsEnabled(Presets.SMNPvP_Combo))
            {
                if (!TargetHasEffectAny(AllPvP.Buffs.Guard))
                {
                    if (CanWeave(actionID))
                    {
                        if (IsEnabled(Presets.SMNPvP_Deathflare) && IsActionEnabled(Deathflare))
                        {
                            return Deathflare;
                        }

                        if (IsEnabled(Presets.SMNPvP_BrandOfPurgatory) && IsActionEnabled(BrandOfPurgatory))
                        {
                            return BrandOfPurgatory;
                        }

                        if (IsEnabled(Presets.SMNPvP_Necrotize) && ActionReady(Necrotize)
                            && !HasEffect(Buffs.FurtherRuin))
                        {
                            return Necrotize;
                        }

                        if (IsEnabled(Presets.SMNPvP_RadiantAegis) && ActionReady(RadiantAegis))
                        {
                            return RadiantAegis;
                        }
                    }

                    if (IsEnabled(Presets.SMNPvP_Slipstream) && ActionReady(Slipstream) && !IsMoving)
                    {
                        return Slipstream;
                    }

                    if (IsEnabled(Presets.SMNPvP_CrimsonCyclone) && ActionReady(CrimsonCyclone))
                    {
                        return CrimsonCyclone;
                    }

                    if (IsEnabled(Presets.SMNPvP_MountainBuster) && ActionReady(MountainBuster) && InActionRange(MountainBuster))
                    {
                        return MountainBuster;
                    }

                    if (IsEnabled(Presets.SMNPvP_CrimsonStrike) && HasEffect(Buffs.CrimsonStrikeReady))
                    {
                        return CrimsonStrike;
                    }
                }
            }

            return actionID;
        }
    }
}
