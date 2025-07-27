using Dalamud.Game.ClientState.JobGauge.Enums;

using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.Game.UI;

using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.CustomCombo;
using UltimateCombo.Data;
using UltimateCombo.Services;

namespace UltimateCombo.Combos.PvE
{
    internal class All
    {
        public const byte JobID = 0;

        public const uint
            Rampart = 7531,
            SecondWind = 7541,
            TrueNorth = 7546,
            Addle = 7560,
            Swiftcast = 7561,
            LucidDreaming = 7562,
            Resurrection = 173,
            Raise = 125,
            Provoke = 7533,
            Shirk = 7537,
            Reprisal = 7535,
            Esuna = 7568,
            Rescue = 7571,
            SolidReason = 232,
            AgelessWords = 215,
            Sleep = 25880,
            WiseToTheWorldMIN = 26521,
            WiseToTheWorldBTN = 26522,
            LowBlow = 7540,
            Bloodbath = 7542,
            HeadGraze = 7551,
            FootGraze = 7553,
            LegGraze = 7554,
            Feint = 7549,
            Interject = 7538,
            Peloton = 7557,
            LegSweep = 7863,
            Repose = 16560,
            Sprint = 3,
            ArmsLength = 7548;

        public static class Buffs
        {
            public const ushort
                Weakness = 43,
                Medicated = 49,
                Bloodbath = 84,
                Swiftcast = 167,
                Rampart = 1191,
                Peloton = 1199,
                LucidDreaming = 1204,
                TrueNorth = 1250,
                Sprint = 50,
                Raise = 148;
        }

        public static class Debuffs
        {
            public const ushort
                Sleep = 3,
                Bind = 13,
                Heavy = 14,
                Addle = 1203,
                Reprisal = 1193,
                Feint = 1195;
        }

        public static class Config
        {
            public static UserInt
                All_SecondWind = new("All_SecondWind", 30),
                All_Bloodbath = new("All_Bloodbath", 50),
                All_Healer_Lucid = new("All_Healer_Lucid", 7500),
                All_Mage_Lucid = new("All_Mage_Lucid", 7500),
                All_BLU_Lucid = new("All_BLU_Lucid", 7500),
                All_ChocoHP = new("All_ChocoHP", 75),
                All_ChocoMode = new("All_ChocoMode", 1);

            public static UserBool
                All_BLM_Lucid = new("All_BLM_Lucid", false),
                All_SwiftRaise = new("All_SwiftRaise", false),
                All_ChocoAuto = new("All_ChocoAuto", false);
        }

        internal class All_Tank_Reprisal : CustomComboClass
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.All_Tank_Reprisal;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is Reprisal && IsEnabled(CustomComboPreset.All_Tank_Reprisal) && SafeToUse())
                {
                    if (TargetHasEffectAny(Debuffs.Reprisal) && IsOffCooldown(Reprisal))
                    {
                        return OriginalHook(11);
                    }
                }

                return actionID;
            }
        }

        internal class All_Caster_Addle : CustomComboClass
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.All_Caster_Addle;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is Addle && IsEnabled(CustomComboPreset.All_Caster_Addle) && SafeToUse())
                {
                    if (TargetHasEffectAny(Debuffs.Addle) && IsOffCooldown(Addle))
                    {
                        return OriginalHook(11);
                    }
                }

                return actionID;
            }
        }

        internal class All_Melee_Feint : CustomComboClass
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.All_Melee_Feint;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is Feint && IsEnabled(CustomComboPreset.All_Melee_Feint) && SafeToUse())
                {
                    if (TargetHasEffectAny(Debuffs.Feint) && IsOffCooldown(Feint))
                    {
                        return OriginalHook(11);
                    }
                }

                return actionID;
            }
        }

        internal class All_Melee_TrueNorth : CustomComboClass
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.All_Melee_TrueNorth;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is TrueNorth && IsEnabled(CustomComboPreset.All_Melee_TrueNorth) && SafeToUse())
                {
                    if (HasEffect(Buffs.TrueNorth))
                    {
                        return OriginalHook(11);
                    }
                }

                return actionID;
            }
        }

        internal class All_Ranged_Mitigation : CustomComboClass
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.All_Ranged_Mitigation;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if ((actionID is BRD.Troubadour or MCH.Tactician or DNC.ShieldSamba) && IsEnabled(CustomComboPreset.All_Ranged_Mitigation))
                {
                    if ((HasEffectAny(BRD.Buffs.Troubadour) || HasEffectAny(MCH.Buffs.Tactician)
                        || HasEffectAny(DNC.Buffs.ShieldSamba)) && IsOffCooldown(actionID))
                    {
                        return OriginalHook(11);
                    }
                }

                return actionID;
            }
        }

        internal class All_Raise : CustomComboClass
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.All_Raise;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if ((actionID is WHM.Raise or SCH.Resurrection or AST.Ascend or SGE.Egeiro
                    or SMN.Resurrection or RDM.Verraise or BLU.AngelWhisper)
                    && IsEnabled(CustomComboPreset.All_Raise))
                {
                    if (TargetHasEffectAny(Buffs.Raise))
                    {
                        return OriginalHook(11);
                    }

                    if (ActionReady(Swiftcast) && !HasEffect(RDM.Buffs.Dualcast))
                    {
                        return Swiftcast;
                    }

                    if (LocalPlayer?.ClassJob.Value.RowId == AST.JobID)
                    {
                        if (ActionReady(AST.Lightspeed) && !HasEffect(AST.Buffs.Lightspeed) && !HasEffect(Buffs.Swiftcast))
                        {
                            return AST.Lightspeed;
                        }

                        return AST.Ascend;
                    }

                    if (LocalPlayer?.ClassJob.Value.RowId == BLU.JobID)
                    {
                        return BLU.AngelWhisper;
                    }

                    if (LocalPlayer?.ClassJob.Value.RowId == RDM.JobID)
                    {
                        if (!HasEffect(Buffs.Swiftcast) && !HasEffect(RDM.Buffs.Dualcast))
                        {
                            return RDM.Vercure;
                        }

                        return RDM.Verraise;
                    }

                    if (LocalPlayer?.ClassJob.Value.RowId == SCH.JobID)
                    {
                        return SCH.Resurrection;
                    }

                    if (LocalPlayer?.ClassJob.Value.RowId == SGE.JobID)
                    {
                        return SGE.Egeiro;
                    }

                    if (LocalPlayer?.ClassJob.Value.RowId == SMN.JobID)
                    {
                        return SMN.Resurrection;
                    }

                    if (LocalPlayer?.ClassJob.Value.RowId == WHM.JobID)
                    {
                        if (ActionReady(WHM.ThinAir) && !HasEffect(WHM.Buffs.ThinAir))
                        {
                            return WHM.ThinAir;
                        }

                        return WHM.Raise;
                    }
                }

                return actionID;
            }
        }

        internal class All_RoleActions : CustomComboClass
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.All_RoleActions;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (IsEnabled(CustomComboPreset.All_RoleActions) && IsComboAction(actionID))
                {
                    if (CanDelayedWeave(actionID) && SafeToUse())
                    {
                        if (IsEnabled(CustomComboPreset.All_TrueNorth) && ActionReady(TrueNorth) && IsEnabled(TrueNorth)
                            && TargetNeedsPositionals() && (!HasEffect(Buffs.TrueNorth) || GetBuffRemainingTime(Buffs.TrueNorth) < 1))
                        {
                            if (LocalPlayer?.ClassJob.Value.RowId == MNK.JobID)
                            {
                                if (actionID
                                    is MNK.Bootshine or MNK.LeapingOpo or MNK.TrueStrike
                                    or MNK.RisingRaptor or MNK.SnapPunch or MNK.PouncingCoeurl
                                    or MNK.DragonKick or MNK.TwinSnakes or MNK.Demolish)
                                {
                                    if (HasEffect(MNK.Buffs.CoeurlForm) && MNK.Gauge.CoeurlFury == 0 && LevelChecked(MNK.Demolish)
                                    && !OnTargetsRear())
                                    {
                                        return TrueNorth;
                                    }

                                    if (HasEffect(MNK.Buffs.CoeurlForm) && (MNK.Gauge.CoeurlFury >= 1 || !LevelChecked(MNK.Demolish))
                                        && !OnTargetsFlank())
                                    {
                                        return TrueNorth;
                                    }
                                }
                            }

                            if (LocalPlayer?.ClassJob.Value.RowId == DRG.JobID)
                            {
                                if (actionID
                                    is DRG.TrueThrust or DRG.VorpalThrust or DRG.LanceBarrage
                                    or DRG.Disembowel or DRG.SpiralBlow or DRG.FullThrust
                                    or DRG.HeavensThrust or DRG.ChaosThrust or DRG.ChaoticSpring
                                    or DRG.FangAndClaw or DRG.WheelingThrust)
                                {
                                    if ((lastComboMove is DRG.Disembowel or DRG.SpiralBlow or DRG.ChaosThrust or DRG.ChaoticSpring)
                                        && LevelChecked(DRG.ChaosThrust) && !OnTargetsRear())
                                    {
                                        return TrueNorth;
                                    }

                                    if ((lastComboMove is DRG.FullThrust or DRG.HeavensThrust)
                                        && LevelChecked(DRG.FangAndClaw) && !OnTargetsFlank())
                                    {
                                        return TrueNorth;
                                    }
                                }
                            }

                            if (LocalPlayer?.ClassJob.Value.RowId == NIN.JobID)
                            {
                                if (lastComboMove is NIN.GustSlash
                                    && (actionID is NIN.SpinningEdge or NIN.GustSlash or NIN.AeolianEdge or NIN.ArmorCrush))
                                {
                                    if (ActionReady(NIN.AeolianEdge) && !OnTargetsRear()
                                        && ((NIN.Gauge.Kazematoi >= 1
                                        && (TargetHasEffect(NIN.TrickList[OriginalHook(NIN.TrickAttack)])
                                        || TargetHasEffect(NIN.MugList[OriginalHook(NIN.Mug)])
                                        || (EnemyHealthCurrentHp() <= LocalPlayer?.MaxHp * 5 && EnemyHealthMaxHp() != 44)))
                                        || NIN.Gauge.Kazematoi > 3 || !LevelChecked(NIN.ArmorCrush)))
                                    {
                                        return TrueNorth;
                                    }

                                    if (ActionReady(NIN.ArmorCrush) && !OnTargetsFlank()
                                        && (NIN.Gauge.Kazematoi <= 3))
                                    {
                                        return TrueNorth;
                                    }
                                }
                            }

                            if (LocalPlayer?.ClassJob.Value.RowId == SAM.JobID)
                            {
                                if (actionID
                                    is SAM.Hakaze or SAM.Gyofu or SAM.Jinpu
                                    or SAM.Gekko or SAM.Shifu or SAM.Kasha or SAM.Yukikaze)
                                {
                                    if (HasEffect(SAM.Buffs.MeikyoShisui)
                                        || (!HasEffect(SAM.Buffs.MeikyoShisui) && lastComboMove is SAM.Jinpu && !OnTargetsRear()))
                                    {
                                        return TrueNorth;
                                    }

                                    if (HasEffect(SAM.Buffs.MeikyoShisui)
                                        || (!HasEffect(SAM.Buffs.MeikyoShisui) && lastComboMove is SAM.Shifu && !OnTargetsFlank()))
                                    {
                                        return TrueNorth;
                                    }
                                }
                            }

                            if (LocalPlayer?.ClassJob.Value.RowId == RPR.JobID)
                            {
                                if (actionID is RPR.Slice or RPR.WaxingSlice or RPR.InfernalSlice)
                                {
                                    if (HasEffect(RPR.Buffs.SoulReaver) || HasEffect(RPR.Buffs.Executioner))
                                    {
                                        if (HasEffect(RPR.Buffs.EnhancedGallows) && !OnTargetsRear())
                                        {
                                            return TrueNorth;
                                        }

                                        if (HasEffect(RPR.Buffs.EnhancedGibbet) && !OnTargetsFlank())
                                        {
                                            return TrueNorth;
                                        }
                                    }
                                }
                            }

                            if (LocalPlayer?.ClassJob.Value.RowId == VPR.JobID)
                            {
                                if (actionID is VPR.SteelFangs or VPR.ReavingFangs)
                                {
                                    if (VPR.Gauge.DreadCombo.HasFlag(DreadCombo.Dreadwinder))
                                    {
                                        if (GetBuffRemainingTime(VPR.Buffs.Swiftscaled) <= GetBuffRemainingTime(VPR.Buffs.HuntersInstinct)
                                            && !OnTargetsRear())
                                        {
                                            return TrueNorth;
                                        }

                                        if (GetBuffRemainingTime(VPR.Buffs.HuntersInstinct) < GetBuffRemainingTime(VPR.Buffs.Swiftscaled)
                                            && !OnTargetsFlank())
                                        {
                                            return TrueNorth;
                                        }
                                    }

                                    if ((HasEffect(VPR.Buffs.HindstungVenom) || HasEffect(VPR.Buffs.HindsbaneVenom))
                                        && (lastComboMove is VPR.HuntersSting or VPR.SwiftskinsSting) && !OnTargetsRear())
                                    {
                                        return TrueNorth;
                                    }

                                    if ((HasEffect(VPR.Buffs.FlankstungVenom) || HasEffect(VPR.Buffs.FlanksbaneVenom))
                                        && (lastComboMove is VPR.HuntersSting or VPR.SwiftskinsSting) && !OnTargetsFlank())
                                    {
                                        return TrueNorth;
                                    }
                                }
                            }
                        }
                    }

                    if (CanWeave(actionID) && SafeToUse())
                    {
                        if (IsEnabled(CustomComboPreset.All_SecondWind) && ActionReady(SecondWind) && IsEnabled(SecondWind)
                            && PlayerHealthPercentageHp() <= GetOptionValue(Config.All_SecondWind))
                        {
                            return SecondWind;
                        }

                        if (IsEnabled(CustomComboPreset.All_Bloodbath) && ActionReady(Bloodbath) && IsEnabled(Bloodbath)
                            && PlayerHealthPercentageHp() <= GetOptionValue(Config.All_Bloodbath))
                        {
                            return Bloodbath;
                        }



                        if (IsEnabled(CustomComboPreset.All_ArmsLength) && ActionReady(ArmsLength) && IsEnabled(ArmsLength)
                            && (ActionWatching.NumberOfGcdsUsed >= 5 || Service.Configuration.IgnoreGCDChecks))
                        {
                            return ArmsLength;
                        }
                    }

                    if (IsEnabled(CustomComboPreset.All_Healer_Lucid) && ActionReady(LucidDreaming) && IsEnabled(LucidDreaming)
                        && ((LocalPlayer?.CurrentMp <= GetOptionValue(Config.All_Healer_Lucid) && CanWeave(actionID)) || LocalPlayer?.CurrentMp <= 1000
                        || (!InCombat() && LocalPlayer?.CurrentMp <= 8000)))
                    {
                        if (LocalPlayer?.ClassJob.Value.RowId == SGE.JobID)
                        {
                            if (SafeToUse())
                            {
                                return LucidDreaming;
                            }
                        }

                        if (LocalPlayer?.ClassJob.Value.RowId is WHM.JobID or SCH.JobID or AST.JobID)
                        {
                            return LucidDreaming;
                        }
                    }

                    if (IsEnabled(CustomComboPreset.All_Mage_Lucid) && ActionReady(LucidDreaming) && IsEnabled(LucidDreaming)
                        && ((LocalPlayer?.CurrentMp <= GetOptionValue(Config.All_Mage_Lucid) && CanWeave(actionID)) || LocalPlayer?.CurrentMp <= 1000
                        || (!InCombat() && LocalPlayer?.CurrentMp <= 8000))
                        && (LocalPlayer?.ClassJob.Value.RowId == SMN.JobID
                        || LocalPlayer?.ClassJob.Value.RowId == RDM.JobID
                        || LocalPlayer?.ClassJob.Value.RowId == PCT.JobID))
                    {
                        return LucidDreaming;
                    }

                    if (IsEnabled(CustomComboPreset.All_BLU_Lucid) && ActionReady(LucidDreaming) && IsEnabled(LucidDreaming)
                        && ((LocalPlayer?.CurrentMp <= GetOptionValue(Config.All_BLU_Lucid) && CanWeave(actionID)) || LocalPlayer?.CurrentMp <= 4000
                        || (!InCombat() && LocalPlayer?.CurrentMp <= 8000))
                        && LocalPlayer?.ClassJob.Value.RowId == BLU.JobID)
                    {
                        if (actionID is BLU.SonicBoom or BLU.GoblinPunch or BLU.ChocoMeteor or BLU.Electrogenesis or BLU.Blaze)
                        {
                            return LucidDreaming;
                        }
                    }
                }

                return actionID;
            }
        }

        internal class All_Choco : CustomComboClass
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.All_Choco;

            protected override unsafe uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                //5 is Tank Mode
                //6 is Attack Mode
                //7 is Healing Mode

                if (IsEnabled(CustomComboPreset.All_Choco) && HasCompanionPresent())
                {
                    if (Config.All_ChocoAuto)
                    {
                        if (UIState.Instance()->Buddy.CompanionInfo.ActiveCommand != 6 && !ActionQueued()
                            && PlayerHealthPercentageHp() >= GetOptionValue(Config.All_ChocoHP))
                        {
                            _ = UseAction(ActionType.BuddyAction, 6);
                        }

                        if (UIState.Instance()->Buddy.CompanionInfo.ActiveCommand != 7 && !ActionQueued()
                            && PlayerHealthPercentageHp() <= GetOptionValue(Config.All_ChocoHP))
                        {
                            _ = UseAction(ActionType.BuddyAction, 7);
                        }
                    }

                    if (!Config.All_ChocoAuto)
                    {
                        if (Config.All_ChocoMode == 1 && UIState.Instance()->Buddy.CompanionInfo.ActiveCommand != 6
                             && !ActionQueued())
                        {
                            _ = UseAction(ActionType.BuddyAction, 6);
                        }

                        if (Config.All_ChocoMode == 2 && UIState.Instance()->Buddy.CompanionInfo.ActiveCommand != 7
                             && !ActionQueued())
                        {
                            _ = UseAction(ActionType.BuddyAction, 7);
                        }

                        if (Config.All_ChocoMode == 3 && UIState.Instance()->Buddy.CompanionInfo.ActiveCommand != 5
                             && !ActionQueued())
                        {
                            _ = UseAction(ActionType.BuddyAction, 5);
                        }
                    }
                }

                return actionID;
            }
        }
    }
}
