using Dalamud.Game.ClientState.Conditions;
using UltimateCombo.Core;

namespace UltimateCombo.Combos.General;

internal static class FSH
{
    internal const byte JobID = 18;

    internal const uint
        AgelessWords = 215,
        SolidReason = 232,
        MinWiseToTheWorld = 26521,
        BtnWiseToTheWorld = 26522,
        Prospect = 227,
        LayOfTheLand = 228,
        LayOfTheLand2 = 291,
        TruthOfMountains = 238,
        Triangulate = 210,
        ArborCall = 211,
        ArborCall2 = 290,
        TruthOfForests = 221,
        Cast = 289,
        Hook = 296,
        Mooch = 297,
        MoochII = 268,
        CastLight = 2135,
        Snagging = 4100,
        Chum = 4104,
        FishEyes = 4105,
        PrecisionHookset = 4179,
        PowerfulHookset = 4103,
        SurfaceSlap = 4595,
        Gig = 7632,
        SharkEye = 7904,
        SharkEyeII = 7905,
        VeteranTrade = 7906,
        NaturesBounty = 7909,
        Salvage = 7910,
        PrizeCatch = 26806,
        VitalSight = 26870,
        BaitedBreath = 26871,
        ElectricCurrent = 26872,
        Rest = 37047;

    internal static class Buffs
    {
        internal const ushort
            TruthOfForests = 221,
            TruthOfMountains = 222,
            Triangulate = 217,
            Prospect = 225,
            Chum = 763,
            EurekaMoment = 2765;
    }

    internal static class Debuffs
    {

    }

    internal class FSH_CastRest : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.FSH_CastRest;
        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (actionID is Cast && IsEnabled(Presets.FSH_CastRest))
            {
                if (HasCondition(ConditionFlag.Fishing))
                {
                    return Rest;
                }
            }

            return actionID;
        }
    }

    internal class FSH_CastHook : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.FSH_CastHook;
        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (actionID is Cast && IsEnabled(Presets.FSH_CastHook))
            {
                if (HasCondition(ConditionFlag.Fishing))
                {
                    return Hook;
                }
            }

            return actionID;
        }
    }

    internal class FSH_FishingToSpearfishing : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.FSH_FishingToSpearfishing;
        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.FSH_FishingToSpearfishing)
                && HasCondition(ConditionFlag.Diving) && !HasCondition(ConditionFlag.Fishing))
            {
                if (actionID is Cast && IsEnabled(Presets.FSH_CastGig))
                {
                    return Gig;
                }

                if (actionID is PrecisionHookset && IsEnabled(Presets.FSH_SurfaceTrade))
                {
                    return VitalSight;
                }

                if (actionID is PrizeCatch && IsEnabled(Presets.FSH_PrizeBounty))
                {
                    return NaturesBounty;
                }

                if (actionID is PowerfulHookset && IsEnabled(Presets.FSH_PowerfulCurrent))
                {
                    return ElectricCurrent;
                }

                if (actionID is Chum && IsEnabled(Presets.FSH_Chum_BaitedBreath))
                {
                    return BaitedBreath;
                }

                if (IsEnabled(Presets.FSH_MoochEye))
                {
                    if (actionID is Mooch)
                    {
                        return SharkEye;
                    }

                    if (actionID is MoochII)
                    {
                        return SharkEyeII;
                    }
                }

                if (actionID is SurfaceSlap && IsEnabled(Presets.FSH_SurfaceTrade))
                {
                    return VeteranTrade;
                }
            }

            return actionID;
        }
    }
}
