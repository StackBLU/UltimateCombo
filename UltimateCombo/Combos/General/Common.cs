using Dalamud.Game.ClientState.JobGauge.Enums;

using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Combos.PvE;
using UltimateCombo.Core;
using UltimateCombo.Data;

namespace UltimateCombo.Combos.General;

internal class Common
{
    internal const byte JobID = 0;

    internal const uint
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

    internal static class Buffs
    {
        internal const ushort
            Weakness = 43,
            Medicated = 49,
            Bloodbath = 84,
            Swiftcast = 167,
            Rampart = 1191,
            Peloton = 1199,
            LucidDreaming = 1204,
            TrueNorth = 1250,
            Sprint = 50,
            Raise = 148,
            EpicEcho = 2734;
    }

    internal static class Debuffs
    {
        internal const ushort
            Sleep = 3,
            Bind = 13,
            Heavy = 14,
            Addle = 1203,
            Reprisal = 1193,
            Feint = 1195;
    }

    internal static class Config
    {
        internal static UserInt
            All_SecondWind = new("All_SecondWind", 30),
            All_Bloodbath = new("All_Bloodbath", 50),
            All_Healer_Lucid = new("All_Healer_Lucid", 7500),
            All_Mage_Lucid = new("All_Mage_Lucid", 7500),
            All_BLU_Lucid = new("All_BLU_Lucid", 7500),
            All_ChocoHP = new("All_ChocoHP", 75),
            All_ChocoMode = new("All_ChocoMode", 1);

        internal static UserBool
            All_BLM_Lucid = new("All_BLM_Lucid", false),
            All_SwiftRaise = new("All_SwiftRaise", false),
            All_ChocoAuto = new("All_ChocoAuto", false);
    }

    internal class All_Tank_Reprisal : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.All_Tank_Reprisal;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (actionID is Reprisal && IsEnabled(Presets.All_Tank_Reprisal) && SafeToUse())
            {
                if (TargetHasEffectAny(Debuffs.Reprisal) && IsOffCooldown(Reprisal))
                {
                    return OriginalHook(11);
                }
            }

            return actionID;
        }
    }

    internal class All_Caster_Addle : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.All_Caster_Addle;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (actionID is Addle && IsEnabled(Presets.All_Caster_Addle) && SafeToUse())
            {
                if (TargetHasEffectAny(Debuffs.Addle) && IsOffCooldown(Addle))
                {
                    return OriginalHook(11);
                }
            }

            return actionID;
        }
    }

    internal class All_Melee_Feint : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.All_Melee_Feint;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (actionID is Feint && IsEnabled(Presets.All_Melee_Feint) && SafeToUse())
            {
                if (TargetHasEffectAny(Debuffs.Feint) && IsOffCooldown(Feint))
                {
                    return OriginalHook(11);
                }
            }

            return actionID;
        }
    }

    internal class All_Melee_TrueNorth : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.All_Melee_TrueNorth;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (actionID is TrueNorth && IsEnabled(Presets.All_Melee_TrueNorth) && SafeToUse())
            {
                if (HasEffect(Buffs.TrueNorth))
                {
                    return OriginalHook(11);
                }
            }

            return actionID;
        }
    }

    internal class All_Ranged_Mitigation : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.All_Ranged_Mitigation;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (actionID is BRD.Troubadour or MCH.Tactician or DNC.ShieldSamba && IsEnabled(Presets.All_Ranged_Mitigation))
            {
                if ((HasEffectAny(BRD.Buffs.Troubadour) || HasEffectAny(MCH.Buffs.Tactician) || HasEffectAny(DNC.Buffs.ShieldSamba)) && IsOffCooldown(actionID))
                {
                    return OriginalHook(11);
                }
            }

            return actionID;
        }
    }

    internal class All_Raise : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.All_Raise;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (actionID is WHM.Raise or SCH.Resurrection or AST.Ascend or SGE.Egeiro or SMN.Resurrection or RDM.Verraise or BLU.AngelWhisper
                && IsEnabled(Presets.All_Raise))
            {
                if (TargetHasEffectAny(Buffs.Raise))
                {
                    return OriginalHook(11);
                }

                if (ActionReady(Swiftcast) && !HasEffect(RDM.Buffs.Dualcast))
                {
                    return Swiftcast;
                }

                if (CurrentJobId is AST.JobID)
                {
                    if (ActionReady(AST.Lightspeed)
                        && !HasEffect(AST.Buffs.Lightspeed) && !HasEffect(Buffs.Swiftcast)
                        && !WasLastAction(AST.Lightspeed) && !WasLastAction(Swiftcast) && !WasLastAction(AST.Ascend))
                    {
                        return AST.Lightspeed;
                    }

                    return AST.Ascend;
                }

                if (CurrentJobId is BLU.JobID)
                {
                    return BLU.AngelWhisper;
                }

                if (CurrentJobId is RDM.JobID)
                {
                    if (!HasEffect(Buffs.Swiftcast) && !HasEffect(RDM.Buffs.Dualcast))
                    {
                        return RDM.Vercure;
                    }

                    return RDM.Verraise;
                }

                if (CurrentJobId is SCH.JobID)
                {
                    return SCH.Resurrection;
                }

                if (CurrentJobId is SGE.JobID)
                {
                    return SGE.Egeiro;
                }

                if (CurrentJobId is SMN.JobID)
                {
                    return SMN.Resurrection;
                }

                if (CurrentJobId is WHM.JobID)
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

    internal class All_RoleActions : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.All_RoleActions;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.All_RoleActions) && IsComboAction(actionID))
            {
                //True North
                if (CanLateWeave(actionID) && SafeToUse())
                {
                    if (IsEnabled(Presets.All_TrueNorth) && ActionReady(TrueNorth) && IsActionEnabled(TrueNorth)
                        && TargetNeedsPositionals() && (!HasEffect(Buffs.TrueNorth) || EffectRemainingTime(Buffs.TrueNorth) < 1))
                    {
                        if (CurrentJobId is MNK.JobID)
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

                        if (CurrentJobId is DRG.JobID)
                        {
                            if (actionID
                                is DRG.TrueThrust or DRG.VorpalThrust or DRG.LanceBarrage
                                or DRG.Disembowel or DRG.SpiralBlow or DRG.FullThrust
                                or DRG.HeavensThrust or DRG.ChaosThrust or DRG.ChaoticSpring
                                or DRG.FangAndClaw or DRG.WheelingThrust)
                            {
                                if (lastComboMove is DRG.Disembowel or DRG.SpiralBlow or DRG.ChaosThrust or DRG.ChaoticSpring
                                    && LevelChecked(DRG.ChaosThrust) && !OnTargetsRear())
                                {
                                    return TrueNorth;
                                }

                                if (lastComboMove is DRG.FullThrust or DRG.HeavensThrust
                                    && LevelChecked(DRG.FangAndClaw) && !OnTargetsFlank())
                                {
                                    return TrueNorth;
                                }
                            }
                        }

                        if (CurrentJobId is NIN.JobID)
                        {
                            if (lastComboMove is NIN.GustSlash
                                && actionID is NIN.SpinningEdge or NIN.GustSlash or NIN.AeolianEdge or NIN.ArmorCrush)
                            {
                                if (ActionReady(NIN.AeolianEdge) && !OnTargetsRear()
                                    && ((NIN.Gauge.Kazematoi >= 1
                                    && (TargetHasEffect(NIN.TrickList[OriginalHook(NIN.TrickAttack)])
                                    || TargetHasEffect(NIN.MugList[OriginalHook(NIN.Mug)])
                                    || TargetIsBoss()))
                                    || NIN.Gauge.Kazematoi > 3 || !LevelChecked(NIN.ArmorCrush)))
                                {
                                    return TrueNorth;
                                }

                                if (ActionReady(NIN.ArmorCrush) && !OnTargetsFlank()
                                    && NIN.Gauge.Kazematoi <= 3)
                                {
                                    return TrueNorth;
                                }
                            }
                        }

                        if (CurrentJobId is SAM.JobID)
                        {
                            if (actionID is SAM.Hakaze or SAM.Gyofu or SAM.Jinpu or SAM.Gekko or SAM.Shifu or SAM.Kasha or SAM.Yukikaze)
                            {
                                if (HasEffect(SAM.Buffs.MeikyoShisui) || (!HasEffect(SAM.Buffs.MeikyoShisui) && lastComboMove is SAM.Jinpu && !OnTargetsRear()))
                                {
                                    return TrueNorth;
                                }

                                if (HasEffect(SAM.Buffs.MeikyoShisui) || (!HasEffect(SAM.Buffs.MeikyoShisui) && lastComboMove is SAM.Shifu && !OnTargetsFlank()))
                                {
                                    return TrueNorth;
                                }
                            }
                        }

                        if (CurrentJobId is RPR.JobID)
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

                        if (CurrentJobId is VPR.JobID)
                        {
                            if (actionID is VPR.SteelFangs or VPR.ReavingFangs)
                            {
                                if (VPR.Gauge.DreadCombo.HasFlag(DreadCombo.Dreadwinder))
                                {
                                    if (EffectRemainingTime(VPR.Buffs.Swiftscaled) <= EffectRemainingTime(VPR.Buffs.HuntersInstinct)
                                        && !OnTargetsRear())
                                    {
                                        return TrueNorth;
                                    }

                                    if (EffectRemainingTime(VPR.Buffs.HuntersInstinct) < EffectRemainingTime(VPR.Buffs.Swiftscaled)
                                        && !OnTargetsFlank())
                                    {
                                        return TrueNorth;
                                    }
                                }

                                if ((HasEffect(VPR.Buffs.HindstungVenom) || HasEffect(VPR.Buffs.HindsbaneVenom))
                                    && lastComboMove is VPR.HuntersSting or VPR.SwiftskinsSting && !OnTargetsRear())
                                {
                                    return TrueNorth;
                                }

                                if ((HasEffect(VPR.Buffs.FlankstungVenom) || HasEffect(VPR.Buffs.FlanksbaneVenom))
                                    && lastComboMove is VPR.HuntersSting or VPR.SwiftskinsSting && !OnTargetsFlank())
                                {
                                    return TrueNorth;
                                }
                            }
                        }
                    }
                }

                //Second Wind, Bloodbath, Arm's Length
                if (CanWeave(actionID) && SafeToUse())
                {
                    if (IsEnabled(Presets.All_SecondWind) && ActionReady(SecondWind) && IsActionEnabled(SecondWind)
                        && PlayerHealthPercentageHp() <= GetOptionValue(Config.All_SecondWind))
                    {
                        return SecondWind;
                    }

                    if (IsEnabled(Presets.All_Bloodbath) && ActionReady(Bloodbath) && IsActionEnabled(Bloodbath)
                        && PlayerHealthPercentageHp() <= GetOptionValue(Config.All_Bloodbath))
                    {
                        return Bloodbath;
                    }



                    if (IsEnabled(Presets.All_ArmsLength) && ActionReady(ArmsLength) && IsActionEnabled(ArmsLength)
                        && (ActionWatching.NumberOfGcdsUsed >= 5 || Service.Configuration.IgnoreGCDChecks))
                    {
                        return ArmsLength;
                    }
                }

                if (IsEnabled(Presets.All_Healer_Lucid) && ActionReady(LucidDreaming) && IsActionEnabled(LucidDreaming)
                    && ((CurrentMP <= GetOptionValue(Config.All_Healer_Lucid) && CanWeave(actionID))
                    || CurrentMP <= 1000 || (!InCombat() && CurrentMP <= 8000)))
                {
                    if (CurrentJobId is SGE.JobID)
                    {
                        if (SafeToUse())
                        {
                            return LucidDreaming;
                        }
                    }

                    if (CurrentJobId is WHM.JobID or SCH.JobID or AST.JobID)
                    {
                        return LucidDreaming;
                    }
                }

                if (IsEnabled(Presets.All_Mage_Lucid) && ActionReady(LucidDreaming) && IsActionEnabled(LucidDreaming)
                    && ((CurrentMP <= GetOptionValue(Config.All_Mage_Lucid) && CanWeave(actionID))
                    || CurrentMP <= 1000
                    || (!InCombat() && CurrentMP <= 8000))
                    && (CurrentJobId is SMN.JobID
                    || CurrentJobId is RDM.JobID
                    || CurrentJobId is PCT.JobID))
                {
                    return LucidDreaming;
                }

                if (IsEnabled(Presets.All_BLU_Lucid) && ActionReady(LucidDreaming) && IsActionEnabled(LucidDreaming)
                    && ((CurrentMP <= GetOptionValue(Config.All_BLU_Lucid) && CanWeave(actionID))
                    || CurrentMP <= 4000
                    || (!InCombat() && CurrentMP <= 8000))
                    && CurrentJobId is BLU.JobID)
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

    internal class All_Choco : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.All_Choco;

        protected override unsafe uint Invoke(uint actionID, uint lastComboMove)
        {
            //5 is Tank Mode
            //6 is Attack Mode
            //7 is Healing Mode

            if (IsEnabled(Presets.All_Choco) && HasCompanionPresent())
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
