using Dalamud.Game.ClientState.JobGauge.Types;
using System.Collections.Generic;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Combos.Content;
using UltimateCombo.Combos.General;
using UltimateCombo.Core;
using UltimateCombo.Data;

namespace UltimateCombo.Combos.PvE;

internal class BLM
{
    internal const byte JobID = 25;

    internal const uint
        Fire = 141,
        Blizzard = 142,
        Thunder = 144,
        Fire2 = 147,
        Transpose = 149,
        Fire3 = 152,
        Thunder3 = 153,
        Blizzard3 = 154,
        AetherialManipulation = 155,
        Scathe = 156,
        Manafont = 158,
        Freeze = 159,
        Flare = 162,
        LeyLines = 3573,
        Sharpcast = 3574,
        Blizzard4 = 3576,
        Fire4 = 3577,
        BetweenTheLines = 7419,
        Thunder4 = 7420,
        Triplecast = 7421,
        Foul = 7422,
        Thunder2 = 7447,
        Despair = 16505,
        UmbralSoul = 16506,
        Xenoglossy = 16507,
        Blizzard2 = 25793,
        HighFire2 = 25794,
        HighBlizzard2 = 25795,
        Amplifier = 25796,
        Paradox = 25797,
        HighThunder = 36986,
        HighThunder2 = 36987,
        FlareStar = 36989;

    internal static class Buffs
    {
        internal const ushort
            Thunderhead = 3870,
            Firestarter = 165,
            LeyLines = 737,
            CircleOfPower = 738,
            Sharpcast = 867,
            Triplecast = 1211,
            EnhancedFlare = 2960;
    }

    internal static class Debuffs
    {
        internal const ushort
            Thunder = 161,
            Thunder2 = 162,
            Thunder3 = 163,
            Thunder4 = 1210,
            HighThunder = 3871,
            HighThunder2 = 3872;
    }

    internal static readonly Dictionary<uint, ushort>
        ThunderList = new()
        {
                { Thunder,  Debuffs.Thunder  },
                { Thunder2, Debuffs.Thunder2 },
                { Thunder3, Debuffs.Thunder3 },
                { Thunder4, Debuffs.Thunder4 },
                { HighThunder, Debuffs.HighThunder },
                { HighThunder2, Debuffs.HighThunder2 }
        };

    internal static int MaxPolyglot(byte level)
    {
        if (level >= 98)
        {
            return 3;
        }

        if (level >= 80)
        {
            return 2;
        }

        if (level >= 70)
        {
            return 1;
        }

        return 0;
    }

    private static BLMGauge Gauge => CustomComboFunctions.GetJobGauge<BLMGauge>();

    internal static class Config
    {

    }

    internal class BLM_ST_DPS : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.BLM_ST_DPS;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is Fire or Fire3 or Fire4 or Blizzard or Blizzard3 or Blizzard4) && IsEnabled(Presets.BLM_ST_DPS))
            {
                //Bozja
                {
                    if (HasEffect(Bozja.Buffs.Reminiscence) && IsEnabled(Presets.Bozja_LFS))
                    {
                        if (HasEffect(Bozja.Buffs.FontOfMagic) && EffectRemainingTime(Bozja.Buffs.FontOfMagic) < 7)
                        {
                            if (WasLastSpell(Blizzard4) && EffectRemainingTime(Bozja.Buffs.FontOfMagic) < 5)
                            {
                                return Bozja.FlareStar;
                            }

                            if (Gauge.InUmbralIce && !WasLastSpell(Blizzard4))
                            {
                                return Blizzard4;
                            }

                            if (!Gauge.InUmbralIce)
                            {
                                return Blizzard3;
                            }
                        }

                        if (EffectRemainingTime(Bozja.Debuffs.FlareStar) < 5 || !HasEffect(Bozja.Debuffs.FlareStar) || DutyActionReady(Bozja.FontOfMagic))
                        {
                            if (WasLastAction(Blizzard4) && (HasEffect(Bozja.Buffs.FontOfMagic) || DutyActionNotEquipped(Bozja.FontOfMagic)))
                            {
                                return Bozja.FlareStar;
                            }

                            if (WasLastAction(Bozja.FontOfMagic))
                            {
                                return Blizzard4;
                            }

                            if (WasLastAction(Common.LucidDreaming) && DutyActionReady(Bozja.FontOfMagic))
                            {
                                return Bozja.FontOfMagic;
                            }

                            if (ActionReady(Common.LucidDreaming) && WasLastAction(Blizzard4))
                            {
                                return Common.LucidDreaming;
                            }

                            if (!Gauge.InUmbralIce)
                            {
                                return Blizzard3;
                            }

                            return Blizzard4;
                        }
                    }
                }

                if (CanWeave(actionID))
                {
                    if (IsEnabled(Presets.BLM_ST_Swiftcast) && ActionReady(Common.Swiftcast) && !Gauge.IsParadoxActive && CanLateWeave(actionID)
                        && Gauge.InAstralFire && !HasEffect(Buffs.Triplecast) && !HasEffect(Occult.Buffs.OccultQuick)
                        && (CurrentMP >= 4000 || ActionReady(Manafont)
                        || (GetCooldownRemainingTime(Manafont) < 5 && CurrentMP > 2000)))
                    {
                        return Common.Swiftcast;
                    }

                    if (IsEnabled(Presets.BLM_ST_Amplifier) && ActionReady(Amplifier) && Gauge.PolyglotStacks < MaxPolyglot(Level)
                        && (Gauge.InUmbralIce || Gauge.InAstralFire) && TargetIsBoss())
                    {
                        return Amplifier;
                    }

                    if (ActionWatching.NumberOfGcdsUsed >= 5 || Service.Configuration.IgnoreGCDChecks)
                    {
                        if (IsEnabled(Presets.BLM_ST_Triplecast) && ActionReady(Triplecast) && !Gauge.IsParadoxActive
                            && (HasEffect(Buffs.CircleOfPower) || GetRemainingCharges(Triplecast) == 2
                            || (GetCooldownChargeRemainingTime(Triplecast) < 10 && GetRemainingCharges(Triplecast) == 1))
                            && Gauge.InAstralFire
                            && !HasEffect(Common.Buffs.Swiftcast) && !HasEffect(Buffs.Triplecast) && !HasEffect(Occult.Buffs.OccultQuick)
                            && (CurrentMP >= 4000 || ActionReady(Manafont)
                            || (GetCooldownRemainingTime(Manafont) < 5 && CurrentMP > 2000)))
                        {
                            return Triplecast;
                        }

                        if (IsEnabled(Presets.BLM_ST_LeyLines) && ActionReady(LeyLines) && !HasEffect(Buffs.LeyLines) && !WasLastAction(LeyLines))
                        {
                            return LeyLines;
                        }
                    }
                }

                if (IsEnabled(Presets.BLM_ST_Thunder) && ActionReady(OriginalHook(Thunder))
                    && HasEffect(Buffs.Thunderhead) && !HasEffect(Buffs.Triplecast)
                    && TargetIsBoss() && TargetEffectRemainingTime(ThunderList[OriginalHook(Thunder)]) <= 1.5)
                {
                    return OriginalHook(Thunder);
                }

                if (IsEnabled(Presets.BLM_ST_Xeno) && ActionReady(Xenoglossy) && Gauge.PolyglotStacks >= 1
                    && !HasEffect(Buffs.Triplecast) && !HasEffect(Common.Buffs.Swiftcast)
                    && ((Gauge.PolyglotStacks == MaxPolyglot(Level) && Gauge.EnochianTimer <= 10000)
                    || (ActionReady(Amplifier) && Gauge.PolyglotStacks == MaxPolyglot(Level))
                    || ActionWatching.NumberOfGcdsUsed == 4 || IsMoving || TargetCloseToDeath()))
                {
                    if (IsEnabled(Presets.BLM_ST_Thunder) && ActionReady(OriginalHook(Thunder)) && HasEffect(Buffs.Thunderhead)
                        && TargetEffectRemainingTime(ThunderList[OriginalHook(Thunder)]) <= 3)
                    {
                        return OriginalHook(Thunder);
                    }

                    if (ActionReady(Paradox) && Gauge.IsParadoxActive)
                    {
                        if ((Gauge.InAstralFire && CurrentMP >= 1600) || Gauge.InUmbralIce)
                        {
                            return Paradox;
                        }
                    }

                    if (ActionReady(Despair) && Gauge.InAstralFire && CurrentMP <= 1600 && CurrentMP >= 800)
                    {
                        return Despair;
                    }

                    return Xenoglossy;
                }

                if (IsEnabled(Presets.BLM_ST_Manafont) && ActionReady(Manafont) && HasEffect(Buffs.CircleOfPower) && Gauge.InAstralFire
                    && CurrentMP == 0)
                {
                    return Manafont;
                }

                if (IsEnabled(Presets.BLM_ST_FlareStar) && ActionReady(FlareStar) && Gauge.InAstralFire && Gauge.AstralSoulStacks == 6)
                {
                    return FlareStar;
                }

                if (ActionReady(Paradox) && Gauge.IsParadoxActive)
                {
                    if (HasEffect(Buffs.Firestarter) && Gauge.InAstralFire)
                    {
                        return Fire3;
                    }

                    if ((Gauge.InAstralFire && CurrentMP >= 1600) || Gauge.InUmbralIce)
                    {
                        return Paradox;
                    }
                }

                if (ActionReady(Despair) && Gauge.InAstralFire && ((CurrentMP <= 1600 && CurrentMP >= 800) || HasEffect(Bozja.Buffs.AutoEther)))
                {
                    return Despair;
                }

                if (ActionReady(Blizzard4) && Gauge.InUmbralIce && CurrentMP < 10000 && !WasLastSpell(Blizzard4) && !WasLastAction(Bozja.FlareStar))
                {
                    return Blizzard4;
                }

                //Level Checks
                {
                    if (!LevelChecked(Blizzard3) && CurrentMP < 1600)
                    {
                        if (ActionReady(Common.LucidDreaming))
                        {
                            return Common.LucidDreaming;
                        }

                        return Blizzard;
                    }

                    if (!LevelChecked(Blizzard4) && LevelChecked(Blizzard3) && Gauge.InUmbralIce && CurrentMP < 10000 && !WasLastAction(UmbralSoul))
                    {
                        return UmbralSoul;
                    }

                    if (ActionReady(Blizzard3) && !WasLastAction(Transpose)
                        && ((Gauge.InAstralFire && CurrentMP < 800)
                        || (!Gauge.InAstralFire && !Gauge.InUmbralIce && CurrentMP < 10000 && CurrentMP >= 800)
                        || (!LevelChecked(Despair) && CurrentMP < 1600 && !WasLastSpell(Blizzard4))))
                    {
                        return Blizzard3;
                    }
                }

                if (ActionReady(Fire4) && Gauge.InAstralFire && CurrentMP >= 1600 && !WasLastAction(Transpose) && !WasLastSpell(Blizzard4))
                {
                    return Fire4;
                }

                if (ActionReady(Fire3) && ((!Gauge.InAstralFire && !Gauge.InUmbralIce) || (Gauge.InUmbralIce && CurrentMP == 10000)
                    || WasLastSpell(Blizzard4) || HasEffect(Buffs.Firestarter)))
                {
                    if (ActionReady(Transpose) && Gauge.InUmbralIce && HasEffect(Buffs.Firestarter))
                    {
                        return Transpose;
                    }

                    return Fire3;
                }
            }

            return actionID;
        }
    }

    internal class BLM_AoE_DPS : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.BLM_AoE_DPS;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is Fire2 or HighFire2 or Blizzard2 or HighBlizzard2) && IsEnabled(Presets.BLM_AoE_DPS))
            {
                if (CanWeave(actionID))
                {
                    if (IsEnabled(Presets.BLM_AoE_Swiftcast) && ActionReady(Common.Swiftcast) && CanLateWeave(actionID)
                        && Gauge.InAstralFire && !HasEffect(Buffs.Triplecast) && !HasEffect(Occult.Buffs.OccultQuick)
                        && (CurrentMP >= 4000 || ActionReady(Manafont)
                        || (GetCooldownRemainingTime(Manafont) < 5 && CurrentMP > 2000)))
                    {
                        return Common.Swiftcast;
                    }

                    if (IsEnabled(Presets.BLM_AoE_Amplifier) && ActionReady(Amplifier) && Gauge.PolyglotStacks < MaxPolyglot(Level)
                        && (Gauge.InUmbralIce || Gauge.InAstralFire))
                    {
                        return Amplifier;
                    }

                    if (IsEnabled(Presets.BLM_AoE_Triplecast) && ActionReady(Triplecast)
                        && (HasEffect(Buffs.CircleOfPower) || GetRemainingCharges(Triplecast) == 2
                        || (GetCooldownChargeRemainingTime(Triplecast) < 10 && GetRemainingCharges(Triplecast) == 1))
                        && Gauge.InAstralFire
                        && !HasEffect(Common.Buffs.Swiftcast) && !HasEffect(Buffs.Triplecast) && !HasEffect(Occult.Buffs.OccultQuick)
                        && (CurrentMP >= 4000 || ActionReady(Manafont)
                        || (GetCooldownRemainingTime(Manafont) < 5 && CurrentMP > 2000)))
                    {
                        return Triplecast;
                    }

                    if (IsEnabled(Presets.BLM_AoE_LeyLines) && ActionReady(LeyLines) && !HasEffect(Buffs.LeyLines) && !WasLastAction(LeyLines))
                    {
                        return LeyLines;
                    }
                }

                if (IsEnabled(Presets.BLM_AoE_Thunder) && ActionReady(OriginalHook(Thunder)) && HasEffect(Buffs.Thunderhead)
                    && !HasEffect(Buffs.Triplecast) && TargetIsBoss() && TargetEffectRemainingTime(ThunderList[OriginalHook(Thunder2)]) <= 1.5)
                {
                    return OriginalHook(Thunder2);
                }

                if (IsEnabled(Presets.BLM_AoE_Foul) && ActionReady(Foul) && Gauge.PolyglotStacks >= 1
                    && !HasEffect(Buffs.Triplecast) && !HasEffect(Common.Buffs.Swiftcast)
                    && ((Gauge.PolyglotStacks == MaxPolyglot(Level) && Gauge.EnochianTimer <= 10000)
                    || (ActionReady(Amplifier) && Gauge.PolyglotStacks == MaxPolyglot(Level)) || IsMoving || TargetCloseToDeath()))
                {
                    return Foul;
                }

                if (IsEnabled(Presets.BLM_AoE_Manafont) && ActionReady(Manafont) && HasEffect(Buffs.CircleOfPower) && Gauge.InAstralFire
                    && CurrentMP == 0)
                {
                    return Manafont;
                }

                if (IsEnabled(Presets.BLM_AoE_FlareStar) && ActionReady(FlareStar) && Gauge.InAstralFire && Gauge.AstralSoulStacks == 6)
                {
                    return FlareStar;
                }

                if (ActionReady(Flare) && Gauge.InAstralFire && CurrentMP <= 3000 && CurrentMP >= 800)
                {
                    return Flare;
                }

                if (ActionReady(Freeze) && Gauge.InUmbralIce && CurrentMP < 10000 && !WasLastSpell(OriginalHook(Freeze)))
                {
                    return Freeze;
                }

                if (ActionReady(OriginalHook(Blizzard2)) && Gauge.InAstralFire && CurrentMP == 0 && !WasLastSpell(OriginalHook(Freeze)))
                {
                    return OriginalHook(Blizzard2);
                }

                if (LevelChecked(FlareStar) && Gauge.InAstralFire && CurrentMP >= 3000)
                {
                    return Flare;
                }

                if (ActionReady(Transpose) && Gauge.InUmbralIce && (CurrentMP == 10000 || WasLastSpell(Freeze)))
                {
                    return Transpose;
                }

                if (ActionReady(OriginalHook(Fire2)) && Gauge.InAstralFire && CurrentMP >= 3000)
                {
                    return OriginalHook(Fire2);
                }
            }

            return actionID;
        }
    }

    internal class BLM_Fire4Blizzard4 : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.BLM_Fire4Blizzard4;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is Fire4 or Blizzard4) && IsEnabled(Presets.BLM_Fire4Blizzard4))
            {
                if (ActionReady(Fire4) && Gauge.InAstralFire)
                {
                    return Fire4;
                }

                if (ActionReady(Blizzard4) && Gauge.InUmbralIce)
                {
                    return Blizzard4;
                }
            }

            return actionID;
        }
    }

    internal class BLM_XenoParadox : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.BLM_XenoParadox;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is Xenoglossy or Paradox or Despair) && IsEnabled(Presets.BLM_XenoParadox))
            {
                if (ActionReady(Paradox) && Gauge.IsParadoxActive && ((Gauge.InAstralFire && CurrentMP >= 1600) || Gauge.InUmbralIce))
                {
                    return Paradox;
                }

                if (ActionReady(Despair) && Gauge.InAstralFire && (CurrentMP <= 1600))
                {
                    return Despair;
                }

                return Xenoglossy;
            }

            return actionID;
        }
    }

    internal class BLM_TriplecastProtect : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.BLM_TriplecastProtect;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is Triplecast or Common.Swiftcast) && IsEnabled(Presets.BLM_TriplecastProtect))
            {
                if (HasEffect(Buffs.Triplecast) || HasEffect(Common.Buffs.Swiftcast))
                {
                    return OriginalHook(11);
                }
            }

            return actionID;
        }
    }

    internal class BLM_UmbralTranspose : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.BLM_UmbralTranspose;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (actionID is UmbralSoul && IsEnabled(Presets.BLM_UmbralTranspose))
            {
                if (ActionReady(Transpose) && Gauge.InAstralFire)
                {
                    return Transpose;
                }

                if (ActionReady(UmbralSoul) && Gauge.InUmbralIce)
                {
                    return UmbralSoul;
                }
            }

            return actionID;
        }
    }

    internal class BLM_LeyLines : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.BLM_LeyLines;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if ((actionID is LeyLines or BetweenTheLines) && IsEnabled(Presets.BLM_LeyLines))
            {
                if (HasEffect(Buffs.LeyLines))
                {
                    return BetweenTheLines;
                }

                return LeyLines;
            }

            return actionID;
        }
    }
}
