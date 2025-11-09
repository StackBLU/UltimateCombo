using Dalamud.Game.ClientState.JobGauge.Types;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Combos.General;
using UltimateCombo.Core;
using UltimateCombo.Data;

namespace UltimateCombo.Combos.PvE;

internal static class SAM
{
    internal const byte JobID = 34;

    internal const uint
        Hakaze = 7477,
        Yukikaze = 7480,
        Gekko = 7481,
        Enpi = 7486,
        Jinpu = 7478,
        Kasha = 7482,
        Shifu = 7479,
        Mangetsu = 7484,
        Fuga = 7483,
        Oka = 7485,
        Higanbana = 7489,
        TenkaGoken = 7488,
        Setsugekka = 7487,
        Shinten = 7490,
        Kyuten = 7491,
        Hagakure = 7495,
        Guren = 7496,
        Meditate = 7497,
        Senei = 16481,
        MeikyoShisui = 7499,
        Seigan = 7501,
        ThirdEye = 7498,
        Tengentsu = 36962,
        Iaijutsu = 7867,
        TsubameGaeshi = 16483,
        KaeshiHiganbana = 16484,
        Shoha = 16487,
        Ikishoten = 16482,
        Fuko = 25780,
        OgiNamikiri = 25781,
        KaeshiNamikiri = 25782,
        Yaten = 7493,
        Gyoten = 7492,
        Gyofu = 36963,
        Zanshin = 36964,
        TendoGoken = 36965,
        TendoSetsugekka = 36966,
        TendoKaeshiGoken = 36967,
        TendoKaeshiSetsugekka = 36968;

    internal static class Buffs
    {
        internal const ushort
            MeikyoShisui = 1233,
            EnhancedEnpi = 1236,
            EyesOpen = 1252,
            OgiNamikiriReady = 2959,
            Fuka = 1299,
            Fugetsu = 1298,
            ZanshinReady = 3855,
            Tendo = 3856,
            AoETsubameReady = 3852,
            TsubameReady = 4216,
            EnhancedAoETsubameReady = 4217,
            EnhancedTsubameReady = 4218;
    }

    internal static class Debuffs
    {
        internal const ushort
            Higanbana = 1228;
    }

    private static SAMGauge Gauge => CustomComboFunctions.GetJobGauge<SAMGauge>();

    internal static class Config
    {
        internal static UserBool
            SAM_ST_SaveKenkiDash = new("SAM_ST_SaveKenkiDash"),
            SAM_AoE_SaveKenkiDash = new("SAM_AoE_SaveKenkiDash");
    }

    internal class SAM_ST_DPS : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.SAM_ST_DPS;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is Hakaze or Gyofu or Jinpu or Gekko or Shifu or Kasha or Yukikaze)
                && IsEnabled(Presets.SAM_ST_DPS))
            {
                if (IsEnabled(Presets.SAM_ST_Meikyo) && ActionReady(MeikyoShisui) && !InCombat()
                    && !HasEffect(Buffs.MeikyoShisui) && GetRemainingCharges(MeikyoShisui) == 2)
                {
                    return MeikyoShisui;
                }

                if (IsEnabled(Presets.All_TrueNorth) && ActionReady(Common.TrueNorth) && !InCombat()
                    && HasEffect(Buffs.MeikyoShisui) && !HasEffect(Common.Buffs.TrueNorth))
                {
                    return Common.TrueNorth;
                }

                if (IsEnabled(Presets.SAM_ST_Meditate) && ActionReady(Meditate) && InCombat() && !HasBattleTarget())
                {
                    return Meditate;
                }

                if (CanWeave(actionID, ActionWatching.LastGCD))
                {
                    if (ActionWatching.NumberOfGcdsUsed >= 2 || Service.Configuration.IgnoreGCDChecks || LevelIgnoreGCD())
                    {
                        if (IsEnabled(Presets.SAM_ST_Hagakure) && ActionReady(Hagakure)
                            && ActionReady(Ikishoten)
                            && (GetRemainingCharges(MeikyoShisui) == 2
                            || (GetRemainingCharges(MeikyoShisui) == 1 && GetCooldownChargeRemainingTime(MeikyoShisui) < 5 && GetCooldownRemainingTime(Ikishoten) < 5))
                            && (Gauge.HasKa || Gauge.HasGetsu || Gauge.HasSetsu)
                            && !(Gauge.HasKa && Gauge.HasGetsu && Gauge.HasSetsu))
                        {
                            return Hagakure;
                        }

                        if (IsEnabled(Presets.SAM_ST_Ikishoten) && ActionReady(Ikishoten) && Gauge.Kenki <= 50 && TargetIsBoss())
                        {
                            return Ikishoten;
                        }
                    }

                    if (ActionWatching.NumberOfGcdsUsed >= 4 || Service.Configuration.IgnoreGCDChecks || LevelIgnoreGCD())
                    {
                        if (IsEnabled(Presets.SAM_ST_Shield) && ActionReady(OriginalHook(ThirdEye)))
                        {
                            return OriginalHook(ThirdEye);
                        }

                        if (IsEnabled(Presets.SAM_ST_Senei) && ActionReady(Senei) && ActionReady(Senei) && Gauge.Kenki >= 25)
                        {
                            return Senei;
                        }

                        if (IsEnabled(Presets.SAM_ST_Meikyo) && ActionReady(MeikyoShisui) && !HasEffect(Buffs.MeikyoShisui)
                            && !HasEffect(Buffs.Tendo) && !WasLastWeaponskill(Hakaze) && !WasLastWeaponskill(Gyofu)
                            && !WasLastWeaponskill(Jinpu) && !WasLastWeaponskill(Shifu)
                            && (HasEffect(Buffs.OgiNamikiriReady) || !LevelChecked(OgiNamikiri) || !TargetWorthDoT()
                            || GetCooldownRemainingTime(Ikishoten) < 5))
                        {
                            return MeikyoShisui;
                        }

                        if (IsEnabled(Presets.SAM_ST_Ikishoten) && HasEffect(Buffs.ZanshinReady) && Gauge.Kenki >= 50)
                        {
                            return Zanshin;
                        }

                        if (IsEnabled(Presets.SAM_ST_Shoha) && ActionReady(Shoha) && Gauge.MeditationStacks == 3)
                        {
                            return Shoha;
                        }

                        if (IsEnabled(Presets.SAM_ST_Shinten) && ActionReady(Shinten) && Gauge.Kenki >= 25
                            && ((GetCooldownRemainingTime(Ikishoten) > 15 && !GetOptionBool(Config.SAM_ST_SaveKenkiDash))
                            || (Gauge.Kenki >= 35 && GetCooldownRemainingTime(Ikishoten) > 15 && GetOptionBool(Config.SAM_ST_SaveKenkiDash))
                            || Gauge.Kenki == 100
                            || (LevelChecked(Ikishoten) && GetCooldownRemainingTime(Ikishoten) < 5 && Gauge.Kenki > 50) || BossAlmostDead()))
                        {
                            return Shinten;
                        }
                    }
                }

                if (TargetHasEffect(Debuffs.Higanbana))
                {
                    if (IsEnabled(Presets.SAM_ST_Kaeshi) && WasLastWeaponskill(OgiNamikiri))
                    {
                        return KaeshiNamikiri;
                    }

                    if (IsEnabled(Presets.SAM_ST_Kaeshi) && HasEffect(Buffs.OgiNamikiriReady) && GetRemainingCharges(MeikyoShisui) == 0)
                    {
                        return OgiNamikiri;
                    }
                }

                if (IsEnabled(Presets.SAM_ST_Tsubame)
                    && (HasEffect(Buffs.TsubameReady) || HasEffect(Buffs.EnhancedTsubameReady)
                    || HasEffect(Buffs.AoETsubameReady) || HasEffect(Buffs.EnhancedAoETsubameReady)))
                {
                    return OriginalHook(TsubameGaeshi);
                }

                if (IsEnabled(Presets.SAM_ST_Higanbana) && TargetEffectRemainingTime(Debuffs.Higanbana) < 5
                    && (!IsMoving || WasLastWeaponskill(OriginalHook(Gyofu)))
                    && OriginalHook(Iaijutsu) != TenkaGoken && OriginalHook(Iaijutsu) != TendoGoken
                    && (Gauge.HasGetsu || Gauge.HasKa || Gauge.HasSetsu)
                    && (ActionWatching.NumberOfGcdsUsed > 2 || Service.Configuration.IgnoreGCDChecks || LevelIgnoreGCD())
                    && TargetWorthDoT())
                {
                    return OriginalHook(Iaijutsu);
                }

                if (Gauge.HasGetsu && Gauge.HasKa && Gauge.HasSetsu
                    && (!IsMoving || WasLastWeaponskill(Jinpu) || WasLastWeaponskill(Shifu))
                    && IsEnabled(Presets.SAM_ST_Iaijutsu)
                    && (ActionWatching.NumberOfGcdsUsed > 2 || Service.Configuration.IgnoreGCDChecks || LevelIgnoreGCD()))
                {
                    return OriginalHook(Iaijutsu);
                }

                if (HasEffect(Buffs.MeikyoShisui))
                {
                    if (!Gauge.HasGetsu && ActionReady(Gekko))
                    {
                        return Gekko;
                    }

                    if (!Gauge.HasKa && ActionReady(Kasha))
                    {
                        return Kasha;
                    }

                    if (!Gauge.HasSetsu && ActionReady(Yukikaze))
                    {
                        return Yukikaze;
                    }
                }

                if (IsEnabled(Presets.SAM_ST_Enpi) && ActionReady(Enpi) && OutOfMeleeRange())
                {
                    return Enpi;
                }

                if (ComboTime > 0)
                {
                    if (lastComboMove is Jinpu && ActionReady(Gekko))
                    {
                        return Gekko;
                    }

                    if (lastComboMove is Shifu && ActionReady(Kasha))
                    {
                        return Kasha;
                    }

                    if (lastComboMove is Hakaze or Gyofu)
                    {
                        if (ActionReady(Yukikaze) && !Gauge.HasSetsu)
                        {
                            return Yukikaze;
                        }

                        if (ActionReady(Jinpu) && (!HasEffect(Buffs.Fugetsu) || !Gauge.HasGetsu))
                        {
                            return Jinpu;
                        }

                        if (ActionReady(Shifu) && (!HasEffect(Buffs.Fuka) || !Gauge.HasKa))
                        {
                            return Shifu;
                        }
                    }
                }

                return OriginalHook(Hakaze);
            }

            return actionID;
        }
    }

    internal class SAM_AoE_DPS : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.SAM_AoE_DPS;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is Fuga or Fuko or Mangetsu or Oka) && IsEnabled(Presets.SAM_AoE_DPS))
            {
                if (IsEnabled(Presets.SAM_AoE_Meikyo) && ActionReady(MeikyoShisui) && !InCombat()
                    && !HasEffect(Buffs.MeikyoShisui) && GetRemainingCharges(MeikyoShisui) == 2)
                {
                    return MeikyoShisui;
                }

                if (IsEnabled(Presets.SAM_AoE_Meditate) && ActionReady(Meditate) && InCombat() && !HasBattleTarget())
                {
                    return Meditate;
                }

                if (CanWeave(actionID, ActionWatching.LastGCD))
                {
                    if (IsEnabled(Presets.SAM_AoE_Hagakure) && ActionReady(Hagakure)
                        && Gauge.HasKa && Gauge.HasGetsu && Gauge.HasSetsu)
                    {
                        return Hagakure;
                    }

                    if (IsEnabled(Presets.SAM_AoE_Ikishoten) && ActionReady(Ikishoten) && Gauge.Kenki <= 50)
                    {
                        return Ikishoten;
                    }

                    if (IsEnabled(Presets.SAM_AoE_Shield) && ActionReady(OriginalHook(ThirdEye)))
                    {
                        return OriginalHook(ThirdEye);
                    }

                    if (IsEnabled(Presets.SAM_AoE_Meikyo) && ActionReady(MeikyoShisui) && !HasEffect(Buffs.MeikyoShisui)
                        && !HasEffect(Buffs.Tendo) && !WasLastWeaponskill(Fuga) && !WasLastWeaponskill(Fuko))
                    {
                        return MeikyoShisui;
                    }

                    if (IsEnabled(Presets.SAM_AoE_Ikishoten) && HasEffect(Buffs.ZanshinReady) && Gauge.Kenki >= 50)
                    {
                        return Zanshin;
                    }

                    if (IsEnabled(Presets.SAM_AoE_Shoha) && ActionReady(Shoha) && Gauge.MeditationStacks == 3)
                    {
                        return Shoha;
                    }

                    if (IsEnabled(Presets.SAM_AoE_Guren) && ActionReady(Guren) && Gauge.Kenki >= 25)
                    {
                        return Guren;
                    }

                    if (IsEnabled(Presets.SAM_AoE_Kyuten) && ActionReady(Kyuten) && Gauge.Kenki >= 25
                        && ((GetCooldownRemainingTime(Ikishoten) > 15 && !GetOptionBool(Config.SAM_AoE_SaveKenkiDash))
                        || (Gauge.Kenki >= 35 && GetCooldownRemainingTime(Ikishoten) > 15 && GetOptionBool(Config.SAM_AoE_SaveKenkiDash))
                        || Gauge.Kenki == 100
                        || (LevelChecked(Ikishoten) && GetCooldownRemainingTime(Ikishoten) < 5 && Gauge.Kenki > 50) || BossAlmostDead()))
                    {
                        return Kyuten;
                    }
                }

                if (IsEnabled(Presets.SAM_AoE_Kaeshi) && WasLastWeaponskill(OgiNamikiri))
                {
                    return KaeshiNamikiri;
                }

                if (IsEnabled(Presets.SAM_AoE_Kaeshi) && HasEffect(Buffs.OgiNamikiriReady))
                {
                    return OgiNamikiri;
                }

                if (IsEnabled(Presets.SAM_AoE_Tsubame)
                    && (HasEffect(Buffs.TsubameReady) || HasEffect(Buffs.EnhancedTsubameReady)
                    || HasEffect(Buffs.AoETsubameReady) || HasEffect(Buffs.EnhancedAoETsubameReady)))
                {
                    return OriginalHook(TsubameGaeshi);
                }

                if (IsEnabled(Presets.SAM_AoE_Iaijutsu)
                    && ((Gauge.HasGetsu && Gauge.HasKa) || (Gauge.HasGetsu && Gauge.HasSetsu) || (Gauge.HasSetsu && Gauge.HasKa)))
                {
                    return OriginalHook(Iaijutsu);
                }

                if (HasEffect(Buffs.MeikyoShisui))
                {
                    if (!Gauge.HasKa && ActionReady(Oka))
                    {
                        return Oka;
                    }

                    if (!Gauge.HasGetsu && ActionReady(Mangetsu))
                    {
                        return Mangetsu;
                    }
                }

                if (ComboTime > 0)
                {
                    if (lastComboMove is Fuga or Fuko)
                    {
                        if (ActionReady(Oka) && (!HasEffect(Buffs.Fuka) || !Gauge.HasKa))
                        {
                            return Oka;
                        }

                        if (ActionReady(Mangetsu) && (!HasEffect(Buffs.Fugetsu) || !Gauge.HasGetsu))
                        {
                            return Mangetsu;
                        }
                    }
                }

                return OriginalHook(Fuga);
            }

            return actionID;
        }
    }

    internal class SAM_Iaijutsu : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.SAM_Iaijutsu;

        protected override uint Invoke(uint actionID, uint lastComboActionID)
        {
            if ((actionID is Iaijutsu or TsubameGaeshi) && IsEnabled(Presets.SAM_Iaijutsu))
            {
                if (HasEffect(Buffs.TsubameReady) || HasEffect(Buffs.EnhancedTsubameReady)
                    || HasEffect(Buffs.AoETsubameReady) || HasEffect(Buffs.EnhancedAoETsubameReady))
                {
                    return OriginalHook(TsubameGaeshi);
                }

                return OriginalHook(Iaijutsu);
            }

            return actionID;
        }
    }
}
