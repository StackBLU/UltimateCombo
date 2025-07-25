using Dalamud.Game.ClientState.JobGauge.Enums;
using Dalamud.Game.ClientState.JobGauge.Types;

using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.CustomCombo;
using UltimateCombo.Data;
using UltimateCombo.Services;

namespace UltimateCombo.Combos.PvE
{
    internal static class BRD
    {
        public const byte JobID = 23;

        public const uint
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

        public static class Buffs
        {
            public const ushort
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

        public static class Debuffs
        {
            public const ushort
                VenomousBite = 124,
                Windbite = 129,
                CausticBite = 1200,
                Stormbite = 1201;
        }

        private static BRDGauge Gauge => CustomComboFunctions.GetJobGauge<BRDGauge>();

        public static class Config
        {

        }

        internal class BRD_ST_DPS : CustomComboClass
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BRD_ST_DPS;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if ((actionID is HeavyShot or BurstShot or StraightShot or RefulgentArrow) && IsEnabled(CustomComboPreset.BRD_ST_DPS))
                {
                    if (CanWeave(actionID) && (ActionWatching.NumberOfGcdsUsed >= 1 || Service.Configuration.IgnoreGCDChecks))
                    {
                        if (IsEnabled(CustomComboPreset.BRD_ST_Songs) && InCombat())
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

                        if (ActionWatching.NumberOfGcdsUsed >= 3 || Service.Configuration.IgnoreGCDChecks)
                        {
                            if (TargetIsBoss())
                            {
                                if (IsEnabled(CustomComboPreset.BRD_ST_BattleVoice) && ActionReady(BattleVoice))
                                {
                                    return BattleVoice;
                                }

                                if (IsEnabled(CustomComboPreset.BRD_ST_Radiant) && ActionReady(RadiantFinale)
                                    && (HasEffect(Buffs.BattleVoice) || WasLastAbility(BattleVoice)))
                                {
                                    return RadiantFinale;
                                }

                                if (IsEnabled(CustomComboPreset.BRD_ST_Raging) && ActionReady(RagingStrikes)
                                    && (HasEffect(Buffs.RadiantFinale) || WasLastAbility(RadiantFinale) || !LevelChecked(RadiantFinale)))
                                {
                                    return RagingStrikes;
                                }

                                if (IsEnabled(CustomComboPreset.BRD_ST_Barrage) && ActionReady(Barrage)
                                    && (HasEffect(Buffs.RagingStrikes) || WasLastAbility(RagingStrikes)))
                                {
                                    return Barrage;
                                }
                            }

                            if (IsEnabled(CustomComboPreset.BRD_ST_Sidewinder) && ActionReady(Sidewinder))
                            {
                                return Sidewinder;
                            }

                            if (IsEnabled(CustomComboPreset.BRD_ST_Bloodletter) && ActionReady(OriginalHook(Bloodletter))
                                && ((GetRemainingCharges(OriginalHook(Bloodletter)) == GetMaxCharges(OriginalHook(Bloodletter)))
                                || (GetRemainingCharges(OriginalHook(Bloodletter)) == GetMaxCharges(OriginalHook(Bloodletter)) - 1
                                && GetCooldownChargeRemainingTime(OriginalHook(Bloodletter)) <= 8)
                                || HasEffect(Buffs.BattleVoice)))
                            {
                                return OriginalHook(Bloodletter);
                            }
                        }

                        if (IsEnabled(CustomComboPreset.BRD_ST_Songs) && Gauge.Song is Song.Wanderer && (Gauge.Repertoire == 3
                            || (Gauge.SongTimer <= 4000 && Gauge.Repertoire >= 1)))
                        {
                            return PitchPerfect;
                        }

                        if (IsEnabled(CustomComboPreset.BRD_ST_Empyreal) && ActionReady(EmpyrealArrow))
                        {
                            return EmpyrealArrow;
                        }
                    }

                    if (IsEnabled(CustomComboPreset.BRD_ST_Apex) && HasEffect(Buffs.BlastArrowReady))
                    {
                        return BlastArrow;
                    }

                    if (IsEnabled(CustomComboPreset.BRD_ST_DoTs) && TargetIsBoss())
                    {
                        if (ActionReady(IronJaws)
                            && ((TargetHasEffect(Debuffs.VenomousBite) && GetDebuffRemainingTime(Debuffs.VenomousBite) < 3)
                            || (TargetHasEffect(Debuffs.CausticBite) && GetDebuffRemainingTime(Debuffs.CausticBite) < 3)
                            || (TargetHasEffect(Debuffs.Windbite) && GetDebuffRemainingTime(Debuffs.Windbite) < 3)
                            || (TargetHasEffect(Debuffs.Stormbite) && GetDebuffRemainingTime(Debuffs.Stormbite) < 3)
                            || WasLastWeaponskill(ResonantArrow)))
                        {
                            return IronJaws;
                        }

                        if (ActionReady(OriginalHook(Stormbite)) && ((!TargetHasEffect(Debuffs.Windbite) && !TargetHasEffect(Debuffs.Stormbite))
                            || (TargetHasEffect(Debuffs.Windbite) && GetDebuffRemainingTime(Debuffs.Windbite) < 3)
                            || (TargetHasEffect(Debuffs.Stormbite) && GetDebuffRemainingTime(Debuffs.Stormbite) < 3)))
                        {
                            return OriginalHook(Stormbite);
                        }

                        if (ActionReady(OriginalHook(CausticBite)) && ((!TargetHasEffect(Debuffs.VenomousBite) && !TargetHasEffect(Debuffs.CausticBite))
                            || (TargetHasEffect(Debuffs.VenomousBite) && GetDebuffRemainingTime(Debuffs.VenomousBite) < 3)
                            || (TargetHasEffect(Debuffs.CausticBite) && GetDebuffRemainingTime(Debuffs.CausticBite) < 3)))
                        {
                            return OriginalHook(CausticBite);
                        }
                    }

                    if (IsEnabled(CustomComboPreset.BRD_ST_Radiant) && HasEffect(Buffs.RadiantEncoreReady) && HasEffect(Buffs.RagingStrikes))
                    {
                        return RadiantEncore;
                    }

                    if (IsEnabled(CustomComboPreset.BRD_ST_Barrage) && HasEffect(Buffs.ResonantArrowReady))
                    {
                        return ResonantArrow;
                    }

                    if (IsEnabled(CustomComboPreset.BRD_ST_Apex) && ActionReady(ApexArrow) && Gauge.SoulVoice == 100)
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

        internal class BRD_AoE_DPS : CustomComboClass
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BRD_AoE_DPS;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if ((actionID is QuickNock or Ladonsbite or WideVolley or Shadowbite) && IsEnabled(CustomComboPreset.BRD_AoE_DPS))
                {
                    if (CanWeave(actionID))
                    {
                        if (IsEnabled(CustomComboPreset.BRD_AoE_Songs) && InCombat())
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

                        if (IsEnabled(CustomComboPreset.BRD_AoE_BattleVoice) && ActionReady(BattleVoice))
                        {
                            return BattleVoice;
                        }

                        if (IsEnabled(CustomComboPreset.BRD_AoE_Radiant) && ActionReady(RadiantFinale) && (HasEffect(Buffs.BattleVoice) || WasLastAbility(BattleVoice)))
                        {
                            return RadiantFinale;
                        }

                        if (IsEnabled(CustomComboPreset.BRD_AoE_Raging) && ActionReady(RagingStrikes) && (HasEffect(Buffs.RadiantFinale) || WasLastAbility(RadiantFinale) || !LevelChecked(RadiantFinale)))
                        {
                            return RagingStrikes;
                        }

                        if (IsEnabled(CustomComboPreset.BRD_AoE_Barrage) && ActionReady(Barrage) && (HasEffect(Buffs.RagingStrikes) || WasLastAbility(RagingStrikes)))
                        {
                            return Barrage;
                        }

                        if (Gauge.Song is Song.Wanderer && (Gauge.Repertoire == 3 || (Gauge.SongTimer <= 4000 && Gauge.Repertoire >= 1)))
                        {
                            return PitchPerfect;
                        }

                        if (IsEnabled(CustomComboPreset.BRD_AoE_Empyreal) && ActionReady(EmpyrealArrow))
                        {
                            return EmpyrealArrow;
                        }

                        if (IsEnabled(CustomComboPreset.BRD_AoE_Sidewinder) && ActionReady(Sidewinder))
                        {
                            return Sidewinder;
                        }

                        if (IsEnabled(CustomComboPreset.BRD_AoE_Rain) && ActionReady(OriginalHook(RainOfDeath))
                            && ((GetRemainingCharges(OriginalHook(RainOfDeath)) == GetMaxCharges(OriginalHook(RainOfDeath)))
                            || (GetRemainingCharges(OriginalHook(RainOfDeath)) == GetMaxCharges(OriginalHook(RainOfDeath)) - 1
                            && GetCooldownChargeRemainingTime(OriginalHook(RainOfDeath)) <= 8)
                            || HasEffect(Buffs.BattleVoice)))
                        {
                            return OriginalHook(RainOfDeath);
                        }
                    }

                    if (IsEnabled(CustomComboPreset.BRD_AoE_Apex) && HasEffect(Buffs.BlastArrowReady))
                    {
                        return BlastArrow;
                    }

                    if (IsEnabled(CustomComboPreset.BRD_AoE_Radiant) && ActionReady(RadiantEncore) && HasEffect(Buffs.RadiantEncoreReady))
                    {
                        return RadiantEncore;
                    }

                    if (IsEnabled(CustomComboPreset.BRD_AoE_Barrage) && ActionReady(ResonantArrow) && HasEffect(Buffs.ResonantArrowReady))
                    {
                        return ResonantArrow;
                    }

                    if (IsEnabled(CustomComboPreset.BRD_AoE_Apex) && ActionReady(ApexArrow) && Gauge.SoulVoice == 100)
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

        internal class BRD_DoTs : CustomComboClass
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BRD_DoTs;
            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if ((actionID is VenomousBite or CausticBite or Windbite or Stormbite or IronJaws) && IsEnabled(CustomComboPreset.BRD_DoTs))
                {
                    if (ActionReady(IronJaws)
                        && ((TargetHasEffect(Debuffs.VenomousBite) && GetDebuffRemainingTime(Debuffs.VenomousBite) < 3)
                        || (TargetHasEffect(Debuffs.CausticBite) && GetDebuffRemainingTime(Debuffs.CausticBite) < 3)
                        || (TargetHasEffect(Debuffs.Windbite) && GetDebuffRemainingTime(Debuffs.Windbite) < 3)
                        || (TargetHasEffect(Debuffs.Stormbite) && GetDebuffRemainingTime(Debuffs.Stormbite) < 3)
                        || WasLastWeaponskill(ResonantArrow)))
                    {
                        return IronJaws;
                    }

                    if (ActionReady(OriginalHook(Stormbite)) && ((!TargetHasEffect(Debuffs.Windbite) && !TargetHasEffect(Debuffs.Stormbite))
                        || (TargetHasEffect(Debuffs.Windbite) && GetDebuffRemainingTime(Debuffs.Windbite) < 3)
                        || (TargetHasEffect(Debuffs.Stormbite) && GetDebuffRemainingTime(Debuffs.Stormbite) < 3)))
                    {
                        return OriginalHook(Stormbite);
                    }

                    if (ActionReady(OriginalHook(CausticBite)) && ((!TargetHasEffect(Debuffs.VenomousBite) && !TargetHasEffect(Debuffs.CausticBite))
                        || (TargetHasEffect(Debuffs.VenomousBite) && GetDebuffRemainingTime(Debuffs.VenomousBite) < 3)
                        || (TargetHasEffect(Debuffs.CausticBite) && GetDebuffRemainingTime(Debuffs.CausticBite) < 3)))
                    {
                        return OriginalHook(CausticBite);
                    }
                }

                return actionID;
            }
        }

        internal class BRD_Songs : CustomComboClass
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BRD_Songs;
            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if ((actionID is MagesBallad or ArmysPaeon or WanderersMinuet) && IsEnabled(CustomComboPreset.BRD_Songs))
                {
                    if (IsEnabled(CustomComboPreset.BRD_ST_Songs) && InCombat())
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
}
