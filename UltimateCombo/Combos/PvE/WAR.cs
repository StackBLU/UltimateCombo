using Dalamud.Game.ClientState.JobGauge.Types;
using System.Linq;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Combos.Content;
using UltimateCombo.Core;
using UltimateCombo.Data;

namespace UltimateCombo.Combos.PvE;

internal static class WAR
{
    internal const byte JobID = 21;

    internal const uint
        HeavySwing = 31,
        Maim = 37,
        Berserk = 38,
        ThrillOfBattle = 40,
        Overpower = 41,
        StormsPath = 42,
        Holmgang = 43,
        StormsEye = 45,
        Tomahawk = 46,
        InnerBeast = 49,
        SteelCyclone = 51,
        Infuriate = 52,
        FellCleave = 3549,
        Decimate = 3550,
        Upheaval = 7387,
        InnerRelease = 7389,
        RawIntuition = 3551,
        MythrilTempest = 16462,
        ChaoticCyclone = 16463,
        NascentFlash = 16464,
        InnerChaos = 16465,
        Orogeny = 25752,
        PrimalRend = 25753,
        PrimalWrath = 36924,
        PrimalRuination = 36925,
        Onslaught = 7386,
        ShakeItOff = 7388;

    internal static class Buffs
    {
        internal const ushort
            InnerRelease = 1177,
            SurgingTempest = 2677,
            NascentChaos = 1897,
            PrimalRendReady = 2624,
            Wrathful = 3901,
            PrimalRuinationReady = 3834,
            BurgeoningFury = 3833,
            Berserk = 86,
            ThrillOfBattle = 87;
    }

    private static WARGauge Gauge => CustomComboFunctions.GetJobGauge<WARGauge>();

    internal static class Config
    {
        internal static UserInt
            WAR_SurgingRefresh = new("WAR_SurgingRefresh", 20),
            WAR_FellCleaveGauge = new("WAR_FellCleaveGauge", 50),
            WAR_DecimateGauge = new("WAR_DecimateGauge", 50),
            WAR_ST_Invuln = new("WAR_ST_Invuln", 10),
            WAR_AoE_Invuln = new("WAR_AoE_Invuln", 10);
    }

    internal class WAR_ST_DPS : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.WAR_ST_DPS;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is HeavySwing or Maim or StormsPath or StormsEye) && IsEnabled(Presets.WAR_ST_DPS))
            {
                if (IsEnabled(Presets.WAR_ST_Invuln) && PlayerHealthPercentageHp() <= GetOptionValue(Config.WAR_ST_Invuln)
                    && ActionReady(Holmgang))
                {
                    return Holmgang;
                }

                if (IsEnabled(Presets.WAR_ST_Tomahawk) && !InCombat() && ActionReady(Tomahawk) && !InMeleeRange())
                {
                    return Tomahawk;
                }

                if (CanWeave(actionID, ActionWatching.LastGCD) && !HasEffect(Bozja.Buffs.BloodRage))
                {
                    if (IsEnabled(Presets.WAR_ST_Infuriate) && ActionReady(Infuriate)
                        && Gauge.BeastGauge <= 50 && !WasLastAbility(Infuriate)
                        && !HasEffect(Buffs.NascentChaos) && !HasEffect(Buffs.InnerRelease)
                        && !HasEffect(Buffs.PrimalRendReady) && !HasEffect(Buffs.PrimalRuinationReady)
                        && (HasEffect(Bozja.Buffs.BloodRush) || !HasEffect(Bozja.Buffs.Reminiscence)))
                    {
                        return Infuriate;
                    }

                    if ((EffectRemainingTime(Buffs.SurgingTempest) > GetOptionValue(Config.WAR_SurgingRefresh) || !LevelChecked(StormsEye))
                        && (ActionWatching.NumberOfGcdsUsed >= 4 || Service.Configuration.IgnoreGCDChecks || LevelIgnoreGCD()))
                    {
                        if (IsEnabled(Presets.WAR_ST_InnerRelease) && ActionReady(OriginalHook(InnerRelease)))
                        {
                            return OriginalHook(InnerRelease);
                        }

                        if (IsEnabled(Presets.WAR_ST_Onslaught) && ActionReady(Onslaught)
                            && InMeleeRangeNoMovement() && !WasLastAbility(Onslaught))
                        {
                            return Onslaught;
                        }

                        if (IsEnabled(Presets.WAR_ST_Upheaval) && ActionReady(Upheaval) & InActionRange(Upheaval))
                        {
                            return Upheaval;
                        }

                        if (IsEnabled(Presets.WAR_ST_Bloodwhetting) && ActionReady(OriginalHook(RawIntuition)))
                        {
                            if (!IsTargetOfTarget() && GetPartyMembers().Any(x => x.GameObject == TargetOfTarget))
                            {
                                return OriginalHook(NascentFlash);
                            }

                            if (IsTargetOfTarget())
                            {
                                return OriginalHook(RawIntuition);
                            }
                        }
                    }
                }

                if (IsEnabled(Presets.WAR_ST_FellCleave) && ActionReady(OriginalHook(FellCleave))
                    && (ActionWatching.NumberOfGcdsUsed >= 2 || Service.Configuration.IgnoreGCDChecks || LevelIgnoreGCD())
                    && (EffectRemainingTime(Buffs.SurgingTempest) > GetOptionValue(Config.WAR_SurgingRefresh) || !LevelChecked(StormsEye))
                    && (HasEffect(Buffs.InnerRelease) || HasEffect(Buffs.NascentChaos)
                    || (Gauge.BeastGauge >= GetOptionValue(Config.WAR_FellCleaveGauge)
                    && (LevelChecked(InnerRelease)
                    || ((HasEffect(Buffs.Berserk) || (!HasEffect(Buffs.Berserk)
                    && !LevelChecked(InnerRelease))) && Gauge.BeastGauge >= 50)))))
                {
                    return OriginalHook(FellCleave);
                }

                if (IsEnabled(Presets.WAR_ST_PrimalRend) && HasEffect(Buffs.PrimalRuinationReady))
                {
                    return PrimalRuination;
                }

                if (IsEnabled(Presets.WAR_ST_PrimalRend) && HasEffect(Buffs.PrimalRendReady))
                {
                    return PrimalRend;
                }

                if (IsEnabled(Presets.WAR_ST_Tomahawk) && ActionReady(Tomahawk) && OutOfMeleeRange())
                {
                    return Tomahawk;
                }

                if (ComboTime > 0)
                {
                    if (lastComboMove is HeavySwing && ActionReady(Maim))
                    {
                        return Maim;
                    }

                    if (lastComboMove is Maim && ActionReady(StormsPath) && IsEnabled(Presets.WAR_ST_StormsEye))
                    {
                        if (ActionReady(StormsEye)
                            && EffectRemainingTime(Buffs.SurgingTempest) <= GetOptionValue(Config.WAR_SurgingRefresh))
                        {
                            return StormsEye;
                        }
                        if (ActionReady(StormsPath))
                        {
                            return StormsPath;
                        }
                    }
                }

                return HeavySwing;
            }

            return actionID;
        }
    }

    internal class WAR_AoE_DPS : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.WAR_AoE_DPS;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is Overpower or MythrilTempest) && IsEnabled(Presets.WAR_AoE_DPS))
            {
                if (IsEnabled(Presets.WAR_AoE_Invuln) && PlayerHealthPercentageHp() <= GetOptionValue(Config.WAR_AoE_Invuln) && ActionReady(Holmgang))
                {
                    return Holmgang;
                }

                if (CanWeave(actionID, ActionWatching.LastGCD))
                {
                    if (IsEnabled(Presets.WAR_AoE_Infuriate) && ActionReady(Infuriate)
                        && Gauge.BeastGauge <= 50 && !WasLastAbility(Infuriate)
                        && !HasEffect(Buffs.NascentChaos) && !HasEffect(Buffs.InnerRelease)
                        && !HasEffect(Buffs.PrimalRendReady) && !HasEffect(Buffs.PrimalRuinationReady))
                    {
                        return Infuriate;
                    }

                    if (IsEnabled(Presets.WAR_AoE_Onslaught) && ActionReady(Onslaught) && !InMeleeRange()
                        && !InCombat())
                    {
                        return Onslaught;
                    }

                    if (EffectRemainingTime(Buffs.SurgingTempest) > GetOptionValue(Config.WAR_SurgingRefresh) || !LevelChecked(StormsEye))
                    {
                        if (IsEnabled(Presets.WAR_AoE_InnerRelease) && ActionReady(OriginalHook(InnerRelease)))
                        {
                            return OriginalHook(InnerRelease);
                        }

                        if (IsEnabled(Presets.WAR_AoE_Onslaught) && ActionReady(Onslaught)
                            && InMeleeRangeNoMovement() && !WasLastAbility(Onslaught))
                        {
                            return Onslaught;
                        }

                        if (IsEnabled(Presets.WAR_AoE_Orogeny) && ActionReady(Orogeny) & InActionRange(Orogeny))
                        {
                            return Orogeny;
                        }

                        if (IsEnabled(Presets.WAR_ST_Bloodwhetting) && ActionReady(OriginalHook(RawIntuition)))
                        {
                            if (!IsTargetOfTarget() && GetPartyMembers().Any(x => x.GameObject == TargetOfTarget))
                            {
                                return OriginalHook(NascentFlash);
                            }

                            if (IsTargetOfTarget())
                            {
                                return OriginalHook(RawIntuition);
                            }
                        }
                    }
                }

                if (IsEnabled(Presets.WAR_AoE_Decimate) && ActionReady(OriginalHook(Decimate))
                    && (EffectRemainingTime(Buffs.SurgingTempest) > GetOptionValue(Config.WAR_SurgingRefresh) || !LevelChecked(StormsEye))
                    && (HasEffect(Buffs.InnerRelease) || HasEffect(Buffs.NascentChaos)
                    || (Gauge.BeastGauge >= GetOptionValue(Config.WAR_DecimateGauge)
                    && (LevelChecked(InnerRelease)
                    || ((HasEffect(Buffs.Berserk) || (!HasEffect(Buffs.Berserk)
                    && !LevelChecked(InnerRelease))) && Gauge.BeastGauge >= 50)))))
                {
                    return OriginalHook(Decimate);
                }

                if (IsEnabled(Presets.WAR_AoE_PrimalRend) && HasEffect(Buffs.PrimalRuinationReady))
                {
                    return PrimalRuination;
                }

                if (IsEnabled(Presets.WAR_AoE_PrimalRend) && HasEffect(Buffs.PrimalRendReady))
                {
                    return PrimalRend;
                }

                if (ComboTime > 0)
                {
                    if (lastComboMove is Overpower && ActionReady(MythrilTempest))
                    {
                        return MythrilTempest;
                    }
                }

                return Overpower;
            }

            return actionID;
        }
    }

    internal class WAR_InfurCleav : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.WAR_InfurCleav;

        protected override uint Invoke(uint actionID, uint lastComboActionID)
        {
            if ((actionID is InnerBeast or FellCleave) && IsEnabled(Presets.WAR_InfurCleav))
            {
                if (ActionReady(Infuriate) && (!HasEffect(Buffs.NascentChaos) || (!LevelChecked(ChaoticCyclone) && Gauge.BeastGauge < 50)))
                {
                    return Infuriate;
                }

                if ((ActionReady(OriginalHook(FellCleave)) && HasEffect(Buffs.NascentChaos)) || Gauge.BeastGauge >= 50 || HasEffect(Buffs.InnerRelease))
                {
                    return OriginalHook(FellCleave);
                }
            }

            return actionID;
        }
    }

    internal class WAR_InfurCyclo : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.WAR_InfurCyclo;

        protected override uint Invoke(uint actionID, uint lastComboActionID)
        {
            if ((actionID is SteelCyclone or Decimate) && IsEnabled(Presets.WAR_InfurCyclo))
            {
                if (ActionReady(Infuriate) && (!HasEffect(Buffs.NascentChaos) || (!LevelChecked(ChaoticCyclone) && Gauge.BeastGauge < 50)))
                {
                    return Infuriate;
                }

                if ((ActionReady(OriginalHook(Decimate)) && HasEffect(Buffs.NascentChaos)) || Gauge.BeastGauge >= 50 || HasEffect(Buffs.InnerRelease))
                {
                    return OriginalHook(Decimate);
                }
            }

            return actionID;
        }
    }

    internal class WAR_Release : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.WAR_Release;

        protected override uint Invoke(uint actionID, uint lastComboActionID)
        {
            if ((actionID is InnerRelease or Berserk) && IsEnabled(Presets.WAR_Release))
            {
                if (ActionReady(OriginalHook(InnerRelease)))
                {
                    return OriginalHook(InnerRelease);
                }

                if (HasEffect(Buffs.InnerRelease))
                {
                    return OriginalHook(FellCleave);
                }

                if (HasEffect(Buffs.PrimalRendReady))
                {
                    return PrimalRend;
                }

                if (HasEffect(Buffs.PrimalRuinationReady))
                {
                    return PrimalRuination;
                }
            }

            return actionID;
        }
    }

    internal class WAR_ThrillShake : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.WAR_ThrillShake;

        protected override uint Invoke(uint actionID, uint lastComboActionID)
        {
            if ((actionID is ThrillOfBattle or ShakeItOff) && IsEnabled(Presets.WAR_ThrillShake))
            {
                if (ActionReady(ThrillOfBattle) && ActionReady(ShakeItOff))
                {
                    return ThrillOfBattle;
                }

                if (ActionReady(ShakeItOff))
                {
                    if (HasEffect(Buffs.ThrillOfBattle) || GetCooldownRemainingTime(ThrillOfBattle) < (GetCooldown(ThrillOfBattle).CooldownTotal - 10))
                    {
                        return ShakeItOff;
                    }

                    return OriginalHook(11);
                }
            }

            return actionID;
        }
    }
}
