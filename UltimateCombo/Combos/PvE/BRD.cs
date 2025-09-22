using Dalamud.Game.ClientState.JobGauge.Enums;
using Dalamud.Game.ClientState.JobGauge.Types;

using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Core;
using UltimateCombo.Data;

namespace UltimateCombo.Combos.PvE;

internal static class BRD
{
    internal const byte JobID = 23;

    internal const uint
        HeavyShot = 97,
        StraightShot = 98,
        VenomousBite = 100,
        RagingStrikes = 101,
        QuickNock = 106,
        Barrage = 107,
        Bloodletter = 110,
        Windbite = 113,
        MagesBallad = 114,
        ArmysPaeon = 116,
        RainOfDeath = 117,
        BattleVoice = 118,
        EmpyrealArrow = 3558,
        WanderersMinuet = 3559,
        IronJaws = 3560,
        Sidewinder = 3562,
        PitchPerfect = 7404,
        Troubadour = 7405,
        CausticBite = 7406,
        Stormbite = 7407,
        RefulgentArrow = 7409,
        BurstShot = 16495,
        ApexArrow = 16496,
        Shadowbite = 16494,
        Ladonsbite = 25783,
        BlastArrow = 25784,
        RadiantFinale = 25785,
        WideVolley = 36974,
        ResonantArrow = 36976,
        RadiantEncore = 36977;

    internal static class Buffs
    {
        internal const ushort
            RagingStrikes = 125,
            Barrage = 128,
            MagesBallad = 135,
            ArmysPaeon = 137,
            BattleVoice = 141,
            WanderersMinuet = 865,
            Troubadour = 1934,
            BlastArrowReady = 2692,
            RadiantFinale = 2722,
            ShadowbiteReady = 3002,
            HawksEye = 3861,
            ResonantArrowReady = 3862,
            RadiantEncoreReady = 3863;
    }

    internal static class Debuffs
    {
        internal const ushort
            VenomousBite = 124,
            Windbite = 129,
            CausticBite = 1200,
            Stormbite = 1201;
    }

    private static BRDGauge Gauge => CustomComboFunctions.GetJobGauge<BRDGauge>();

    internal static class Config
    {

    }

    internal class BRD_ST_DPS : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.BRD_ST_DPS;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is HeavyShot or BurstShot or StraightShot or RefulgentArrow) && IsEnabled(Presets.BRD_ST_DPS))
            {
                if (ActionWatching.NumberOfGcdsUsed >= 1 || Service.Configuration.IgnoreGCDChecks)
                {
                    if (IsEnabled(Presets.BRD_ST_Songs) && InCombat() && (CanWeave(actionID) || !HasTarget()))
                    {
                        if (ActionReady(WanderersMinuet) && (Gauge.Song is Song.Army || Gauge.Song is Song.None) && Gauge.SongTimer <= 12000)
                        {
                            return WanderersMinuet;
                        }

                        if (ActionReady(MagesBallad) && (Gauge.Song is Song.Wanderer || Gauge.Song is Song.None) && Gauge.SongTimer <= 3000)
                        {
                            return MagesBallad;
                        }

                        if (ActionReady(ArmysPaeon) && (Gauge.Song is Song.Mage || Gauge.Song is Song.None) && Gauge.SongTimer <= 3000)
                        {
                            return ArmysPaeon;
                        }
                    }

                    if (CanWeave(actionID))
                    {
                        if (ActionWatching.NumberOfGcdsUsed >= 3 || Service.Configuration.IgnoreGCDChecks)
                        {
                            if (TargetIsBoss())
                            {
                                if (IsEnabled(Presets.BRD_ST_BattleVoice) && ActionReady(BattleVoice) && !HasEffectAny(Buffs.BattleVoice))
                                {
                                    return BattleVoice;
                                }

                                if (IsEnabled(Presets.BRD_ST_Radiant) && ActionReady(RadiantFinale)
                                    && (HasEffect(Buffs.BattleVoice) || WasLastAbility(BattleVoice)))
                                {
                                    return RadiantFinale;
                                }

                                if (IsEnabled(Presets.BRD_ST_Raging) && ActionReady(RagingStrikes)
                                    && (HasEffect(Buffs.RadiantFinale) || WasLastAbility(RadiantFinale) || !LevelChecked(RadiantFinale)))
                                {
                                    return RagingStrikes;
                                }

                                if (IsEnabled(Presets.BRD_ST_Barrage) && ActionReady(Barrage)
                                    && (HasEffect(Buffs.RagingStrikes) || WasLastAbility(RagingStrikes)))
                                {
                                    return Barrage;
                                }
                            }

                            if (IsEnabled(Presets.BRD_ST_Sidewinder) && ActionReady(Sidewinder))
                            {
                                return Sidewinder;
                            }

                            if (IsEnabled(Presets.BRD_ST_Bloodletter) && ActionReady(OriginalHook(Bloodletter))
                                && ((GetRemainingCharges(OriginalHook(Bloodletter)) == GetMaxCharges(OriginalHook(Bloodletter)))
                                || (GetRemainingCharges(OriginalHook(Bloodletter)) == GetMaxCharges(OriginalHook(Bloodletter)) - 1
                                && GetCooldownChargeRemainingTime(OriginalHook(Bloodletter)) <= 8)
                                || HasEffect(Buffs.BattleVoice) || TargetCloseToDeath()))
                            {
                                return OriginalHook(Bloodletter);
                            }
                        }

                        if (IsEnabled(Presets.BRD_ST_Songs) && Gauge.Song is Song.Wanderer && (Gauge.Repertoire == 3
                            || (Gauge.SongTimer <= 4000 && Gauge.Repertoire >= 1)))
                        {
                            return PitchPerfect;
                        }

                        if (IsEnabled(Presets.BRD_ST_Empyreal) && ActionReady(EmpyrealArrow))
                        {
                            return EmpyrealArrow;
                        }
                    }
                }

                if (IsEnabled(Presets.BRD_ST_Apex) && HasEffect(Buffs.BlastArrowReady))
                {
                    return BlastArrow;
                }

                if (IsEnabled(Presets.BRD_ST_DoTs) && TargetWorthDoT())
                {
                    if (ActionReady(IronJaws)
                        && ((TargetHasEffect(Debuffs.VenomousBite) && TargetEffectRemainingTime(Debuffs.VenomousBite) < 3)
                        || (TargetHasEffect(Debuffs.CausticBite) && TargetEffectRemainingTime(Debuffs.CausticBite) < 3)
                        || (TargetHasEffect(Debuffs.Windbite) && TargetEffectRemainingTime(Debuffs.Windbite) < 3)
                        || (TargetHasEffect(Debuffs.Stormbite) && TargetEffectRemainingTime(Debuffs.Stormbite) < 3)
                        || WasLastWeaponskill(ResonantArrow)))
                    {
                        return IronJaws;
                    }

                    if (ActionReady(OriginalHook(Stormbite)) && ((!TargetHasEffect(Debuffs.Windbite) && !TargetHasEffect(Debuffs.Stormbite))
                        || (TargetHasEffect(Debuffs.Windbite) && TargetEffectRemainingTime(Debuffs.Windbite) < 3)
                        || (TargetHasEffect(Debuffs.Stormbite) && TargetEffectRemainingTime(Debuffs.Stormbite) < 3)))
                    {
                        return OriginalHook(Stormbite);
                    }

                    if (ActionReady(OriginalHook(CausticBite)) && ((!TargetHasEffect(Debuffs.VenomousBite) && !TargetHasEffect(Debuffs.CausticBite))
                        || (TargetHasEffect(Debuffs.VenomousBite) && TargetEffectRemainingTime(Debuffs.VenomousBite) < 3)
                        || (TargetHasEffect(Debuffs.CausticBite) && TargetEffectRemainingTime(Debuffs.CausticBite) < 3)))
                    {
                        return OriginalHook(CausticBite);
                    }
                }

                if (IsEnabled(Presets.BRD_ST_Radiant) && HasEffect(Buffs.RadiantEncoreReady) && HasEffect(Buffs.RagingStrikes))
                {
                    return RadiantEncore;
                }

                if (IsEnabled(Presets.BRD_ST_Barrage) && HasEffect(Buffs.ResonantArrowReady))
                {
                    return ResonantArrow;
                }

                if (IsEnabled(Presets.BRD_ST_Apex) && ActionReady(ApexArrow) && Gauge.SoulVoice == 100)
                {
                    return ApexArrow;
                }

                if (HasEffect(Buffs.HawksEye) || HasEffect(Buffs.Barrage))
                {
                    return OriginalHook(RefulgentArrow);
                }

                return OriginalHook(BurstShot);
            }

            return actionID;
        }
    }

    internal class BRD_AoE_DPS : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.BRD_AoE_DPS;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is QuickNock or Ladonsbite or WideVolley or Shadowbite) && IsEnabled(Presets.BRD_AoE_DPS))
            {
                if (IsEnabled(Presets.BRD_AoE_Songs) && InCombat() && (CanWeave(actionID) || !HasTarget()))
                {
                    if (ActionReady(WanderersMinuet) && (Gauge.Song is Song.Army || Gauge.Song is Song.None) && Gauge.SongTimer <= 12000)
                    {
                        return WanderersMinuet;
                    }

                    if (ActionReady(MagesBallad) && (Gauge.Song is Song.Wanderer || Gauge.Song is Song.None) && Gauge.SongTimer <= 3000)
                    {
                        return MagesBallad;
                    }

                    if (ActionReady(ArmysPaeon) && (Gauge.Song is Song.Mage || Gauge.Song is Song.None) && Gauge.SongTimer <= 3000)
                    {
                        return ArmysPaeon;
                    }
                }

                if (CanWeave(actionID))
                {
                    if (TargetIsBoss())
                    {
                        if (IsEnabled(Presets.BRD_AoE_BattleVoice) && ActionReady(BattleVoice) && !HasEffectAny(Buffs.BattleVoice))
                        {
                            return BattleVoice;
                        }

                        if (IsEnabled(Presets.BRD_AoE_Radiant) && ActionReady(RadiantFinale) && (HasEffect(Buffs.BattleVoice) || WasLastAbility(BattleVoice)))
                        {
                            return RadiantFinale;
                        }

                        if (IsEnabled(Presets.BRD_AoE_Raging) && ActionReady(RagingStrikes) && (HasEffect(Buffs.RadiantFinale) || WasLastAbility(RadiantFinale) || !LevelChecked(RadiantFinale)))
                        {
                            return RagingStrikes;
                        }

                        if (IsEnabled(Presets.BRD_AoE_Barrage) && ActionReady(Barrage) && (HasEffect(Buffs.RagingStrikes) || WasLastAbility(RagingStrikes)))
                        {
                            return Barrage;
                        }
                    }

                    if (Gauge.Song is Song.Wanderer && (Gauge.Repertoire == 3 || (Gauge.SongTimer <= 4000 && Gauge.Repertoire >= 1)))
                    {
                        return PitchPerfect;
                    }

                    if (IsEnabled(Presets.BRD_AoE_Empyreal) && ActionReady(EmpyrealArrow))
                    {
                        return EmpyrealArrow;
                    }

                    if (IsEnabled(Presets.BRD_AoE_Sidewinder) && ActionReady(Sidewinder))
                    {
                        return Sidewinder;
                    }

                    if (IsEnabled(Presets.BRD_AoE_Rain) && ActionReady(OriginalHook(RainOfDeath))
                        && ((GetRemainingCharges(OriginalHook(RainOfDeath)) == GetMaxCharges(OriginalHook(RainOfDeath)))
                        || (GetRemainingCharges(OriginalHook(RainOfDeath)) == GetMaxCharges(OriginalHook(RainOfDeath)) - 1
                        && GetCooldownChargeRemainingTime(OriginalHook(RainOfDeath)) <= 8)
                        || HasEffect(Buffs.BattleVoice) || TargetCloseToDeath()))
                    {
                        return OriginalHook(RainOfDeath);
                    }
                }

                if (IsEnabled(Presets.BRD_AoE_Apex) && HasEffect(Buffs.BlastArrowReady))
                {
                    return BlastArrow;
                }

                if (IsEnabled(Presets.BRD_AoE_Radiant) && ActionReady(RadiantEncore) && HasEffect(Buffs.RadiantEncoreReady))
                {
                    return RadiantEncore;
                }

                if (IsEnabled(Presets.BRD_AoE_Barrage) && ActionReady(ResonantArrow) && HasEffect(Buffs.ResonantArrowReady))
                {
                    return ResonantArrow;
                }

                if (IsEnabled(Presets.BRD_AoE_Apex) && ActionReady(ApexArrow) && Gauge.SoulVoice == 100)
                {
                    return ApexArrow;
                }

                if (HasEffect(Buffs.HawksEye) || HasEffect(Buffs.Barrage))
                {
                    return OriginalHook(Shadowbite);
                }

                return OriginalHook(Ladonsbite);
            }

            return actionID;
        }
    }

    internal class BRD_DoTs : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.BRD_DoTs;
        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is VenomousBite or CausticBite or Windbite or Stormbite or IronJaws) && IsEnabled(Presets.BRD_DoTs))
            {
                if (ActionReady(IronJaws)
                    && ((TargetHasEffect(Debuffs.VenomousBite) && TargetEffectRemainingTime(Debuffs.VenomousBite) < 3)
                    || (TargetHasEffect(Debuffs.CausticBite) && TargetEffectRemainingTime(Debuffs.CausticBite) < 3)
                    || (TargetHasEffect(Debuffs.Windbite) && TargetEffectRemainingTime(Debuffs.Windbite) < 3)
                    || (TargetHasEffect(Debuffs.Stormbite) && TargetEffectRemainingTime(Debuffs.Stormbite) < 3)
                    || WasLastWeaponskill(ResonantArrow)))
                {
                    return IronJaws;
                }

                if (ActionReady(OriginalHook(Stormbite)) && ((!TargetHasEffect(Debuffs.Windbite) && !TargetHasEffect(Debuffs.Stormbite))
                    || (TargetHasEffect(Debuffs.Windbite) && TargetEffectRemainingTime(Debuffs.Windbite) < 3)
                    || (TargetHasEffect(Debuffs.Stormbite) && TargetEffectRemainingTime(Debuffs.Stormbite) < 3)))
                {
                    return OriginalHook(Stormbite);
                }

                if (ActionReady(OriginalHook(CausticBite)) && ((!TargetHasEffect(Debuffs.VenomousBite) && !TargetHasEffect(Debuffs.CausticBite))
                    || (TargetHasEffect(Debuffs.VenomousBite) && TargetEffectRemainingTime(Debuffs.VenomousBite) < 3)
                    || (TargetHasEffect(Debuffs.CausticBite) && TargetEffectRemainingTime(Debuffs.CausticBite) < 3)))
                {
                    return OriginalHook(CausticBite);
                }
            }

            return actionID;
        }
    }

    internal class BRD_Songs : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.BRD_Songs;
        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is MagesBallad or ArmysPaeon or WanderersMinuet) && IsEnabled(Presets.BRD_Songs))
            {
                if (IsEnabled(Presets.BRD_ST_Songs) && InCombat())
                {
                    if (ActionReady(WanderersMinuet) && (Gauge.Song is Song.Army || Gauge.Song is Song.None) && Gauge.SongTimer <= 12000)
                    {
                        return WanderersMinuet;
                    }

                    if (ActionReady(MagesBallad) && (Gauge.Song is Song.Wanderer || Gauge.Song is Song.None) && Gauge.SongTimer <= 3000)
                    {
                        return MagesBallad;
                    }

                    if (ActionReady(ArmysPaeon) && (Gauge.Song is Song.Mage || Gauge.Song is Song.None) && Gauge.SongTimer <= 3000)
                    {
                        return ArmysPaeon;
                    }
                }
            }

            return actionID;
        }
    }
}
