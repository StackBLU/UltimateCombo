using Dalamud.Game.ClientState.JobGauge.Types;
using System.Collections.Generic;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Core;
using UltimateCombo.Data;

namespace UltimateCombo.Combos.PvE;

internal class NIN
{
    internal const byte JobID = 30;

    internal static List<uint> MudraCheck = ActionWatching.CombatActions;

    internal const uint
        SpinningEdge = 2240,
        ShadeShift = 2241,
        GustSlash = 2242,
        Hide = 2245,
        Assassinate = 2246,
        ThrowingDagger = 2247,
        Mug = 2248,
        DeathBlossom = 2254,
        AeolianEdge = 2255,
        TrickAttack = 2258,
        Kassatsu = 2264,
        ArmorCrush = 3563,
        DreamWithinADream = 3566,
        TenChiJin = 7403,
        Bhavacakra = 7402,
        HakkeMujinsatsu = 16488,
        Meisui = 16489,
        Bunshin = 16493,
        PhantomKamaitachi = 25774,
        ForkedRaiju = 25777,
        FleetingRaiju = 25778,
        Hellfrog = 7401,
        HollowNozuchi = 25776,
        KunaisBane = 36958,
        Dokumori = 36957,
        Ninjutsu = 2260,
        Rabbit = 2272,
        Ten = 2259,
        Chi = 2261,
        Shukuchi = 2262,
        Jin = 2263,
        TenCombo = 18805,
        ChiCombo = 18806,
        JinCombo = 18807,
        FumaShuriken = 2265,
        Hyoton = 2268,
        Doton = 2270,
        Katon = 2266,
        Suiton = 2271,
        Raiton = 2267,
        Huton = 2269,
        GokaMekkyaku = 16491,
        HyoshoRanryu = 16492,
        TCJFumaShurikenTen = 18873,
        TCJFumaShurikenChi = 18874,
        TCJFumaShurikenJin = 18875,
        TCJKaton = 18876,
        TCJRaiton = 18877,
        TCJHyoton = 18878,
        TCJHuton = 2269,
        TCJDoton = 18880,
        TCJSuiton = 18881,
        DeathfrogMedium = 36959,
        ZeshoMeppo = 36960,
        TenriJindo = 36961;

    internal static class Buffs
    {
        internal const ushort
            Mudra = 496,
            Kassatsu = 497,
            Suiton = 507,
            Hidden = 614,
            TenChiJin = 1186,
            AssassinateReady = 1955,
            RaijuReady = 2690,
            PhantomReady = 2723,
            Meisui = 2689,
            Doton = 501,
            Bunshin = 1954,
            ShadowWalker = 3848,
            Higi = 3850,
            TenriJindo = 3851;
    }

    internal static class Debuffs
    {
        internal const ushort
            TrickAttack = 3254,
            KunaisBane = 3906,
            Mug = 3183,
            Dokumori = 3849;
    }

    internal static Dictionary<uint, ushort>
        TrickList = new() {
                { TrickAttack,  Debuffs.TrickAttack  },
                { KunaisBane, Debuffs.KunaisBane }
        };

    internal static Dictionary<uint, ushort>
        MugList = new() {
                { Mug,  Debuffs.Mug  },
                { Dokumori, Debuffs.Dokumori }
        };

    internal static NINGauge Gauge => CustomComboFunctions.GetJobGauge<NINGauge>();

    internal static class Config
    {

    }

    internal class NIN_ST_DPS : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.NIN_ST_DPS;

        protected override uint Invoke(uint actionID, uint lastComboActionID)
        {
            if ((actionID is SpinningEdge or GustSlash or AeolianEdge or ArmorCrush)
                && IsEnabled(Presets.NIN_ST_DPS))
            {
                if (IsEnabled(Presets.NIN_ST_Mudras) && ActionReady(Ten) && (!InCombat() || (ActionWatching.NumberOfGcdsUsed == 0 && HasEffect(Buffs.Mudra))))
                {
                    if (OriginalHook(Ninjutsu) == Suiton)
                    {
                        return OriginalHook(Ninjutsu);
                    }

                    if (WasLastAction(ChiCombo))
                    {
                        return JinCombo;
                    }

                    if (WasLastAction(Ten))
                    {
                        return ChiCombo;
                    }

                    return Ten;
                }

                if (CanWeave(actionID, ActionWatching.LastGCD) && !HasEffect(Buffs.Mudra) && !HasEffect(Buffs.TenChiJin))
                {
                    if (IsEnabled(Presets.NIN_ST_Kassatsu) && ActionReady(Kassatsu))
                    {
                        return Kassatsu;
                    }

                    if (ActionWatching.NumberOfGcdsUsed >= 2 || Service.Configuration.IgnoreGCDChecks || LevelIgnoreGCD())
                    {
                        if (IsEnabled(Presets.NIN_ST_Mug) && ActionReady(OriginalHook(Mug)) && TargetIsBoss() && !TargetHasEffectAny(Debuffs.Dokumori))
                        {
                            return OriginalHook(Mug);
                        }

                        if (IsEnabled(Presets.NIN_ST_Bunshin) && ActionReady(Bunshin) && Gauge.Ninki >= 50)
                        {
                            return Bunshin;
                        }
                    }

                    if (ActionWatching.NumberOfGcdsUsed >= 4 || Service.Configuration.IgnoreGCDChecks || LevelIgnoreGCD())
                    {
                        if (IsEnabled(Presets.NIN_ST_Trick) && ActionReady(OriginalHook(TrickAttack)) && HasEffect(Buffs.ShadowWalker)
                            && (GetCooldownRemainingTime(OriginalHook(Mug)) > 30 || TargetHasEffectAny(MugList[OriginalHook(Mug)])))
                        {
                            return OriginalHook(TrickAttack);
                        }

                        if (IsEnabled(Presets.NIN_ST_Assassinate) && ActionReady(OriginalHook(Assassinate)))
                        {
                            return OriginalHook(Assassinate);
                        }

                        if (IsEnabled(Presets.NIN_ST_TenChiJin) && ActionReady(TenChiJin) && !HasEffect(Buffs.ShadowWalker) && TargetIsBoss()
                            && !HasEffect(Buffs.Mudra) && !HasEffect(Buffs.Kassatsu) && !IsMoving
                            && (GetCooldownRemainingTime(OriginalHook(TrickAttack)) <= 15
                            || (GetCooldownRemainingTime(Meisui) <= 15 && LevelChecked(Meisui))
                            || !LevelChecked(Meisui)))
                        {
                            return TenChiJin;
                        }

                        if (IsEnabled(Presets.NIN_ST_Meisui) && ActionReady(Meisui) && HasEffect(Buffs.ShadowWalker))
                        {
                            return Meisui;
                        }

                        if (IsEnabled(Presets.NIN_ST_Bhav) && HasEffect(Buffs.Higi) && Gauge.Ninki > 50
                            && TargetHasEffectAny(TrickList[OriginalHook(TrickAttack)]))
                        {
                            return ZeshoMeppo;
                        }

                        if (IsEnabled(Presets.NIN_ST_TenChiJin) && HasEffect(Buffs.TenriJindo) && ActionReady(TenriJindo)
                            && (TargetHasEffectAny(MugList[OriginalHook(Mug)]) || EffectRemainingTime(Buffs.TenriJindo) <= 5))
                        {
                            return TenriJindo;
                        }

                        if (IsEnabled(Presets.NIN_ST_Bhav) && ActionReady(Bhavacakra)
                            && (Gauge.Ninki >= 70
                            || (Gauge.Ninki >= 50 && (TargetHasEffectAny(TrickList[OriginalHook(TrickAttack)])
                            || TargetHasEffectAny(MugList[OriginalHook(Mug)])))))
                        {
                            return Bhavacakra;
                        }
                    }
                }

                if (IsEnabled(Presets.NIN_ST_Mudras))
                {
                    if (HasEffect(Buffs.TenChiJin))
                    {
                        if (OriginalHook(Jin) is TCJSuiton)
                        {
                            return TCJSuiton;
                        }

                        if (OriginalHook(Chi) is TCJRaiton)
                        {
                            return TCJRaiton;
                        }

                        if (OriginalHook(Ten) is TCJFumaShurikenTen)
                        {
                            return TCJFumaShurikenTen;
                        }
                    }

                    if (HasEffect(Buffs.Kassatsu) || WasLastAbility(Kassatsu))
                    {
                        if (LevelChecked(HyoshoRanryu))
                        {
                            if (OriginalHook(Ninjutsu) == HyoshoRanryu && MudraCheck[^2] == TenCombo && MudraCheck[^1] == JinCombo)
                            {
                                return OriginalHook(Ninjutsu);
                            }

                            if (WasLastAction(TenCombo))
                            {
                                return JinCombo;
                            }

                            if (TargetHasEffectAny(TrickList[OriginalHook(TrickAttack)]) || (EffectRemainingTime(Buffs.Kassatsu) < 5 && !WasLastAbility(Kassatsu)))
                            {
                                return TenCombo;
                            }
                        }

                        if (OriginalHook(Ninjutsu) == Raiton && MudraCheck[^2] == JinCombo && MudraCheck[^1] == ChiCombo)
                        {
                            return OriginalHook(Ninjutsu);
                        }

                        if (WasLastAction(JinCombo))
                        {
                            return ChiCombo;
                        }

                        if (HasEffect(TrickList[OriginalHook(TrickAttack)])
                            || (EffectRemainingTime(Buffs.Kassatsu) < 5 && !WasLastAbility(Kassatsu)))
                        {
                            return JinCombo;
                        }
                    }

                    if (!HasEffect(Buffs.Kassatsu) && !WasLastAbility(Kassatsu)
                        && (ActionWatching.NumberOfGcdsUsed >= 4 || Service.Configuration.IgnoreGCDChecks || LevelIgnoreGCD())
                        && (HasEffect(Buffs.Mudra) || HasCharges(Ten) || WasLastAction(Jin)))
                    {
                        if (OriginalHook(Ninjutsu) == Raiton && MudraCheck[^2] == Jin && MudraCheck[^1] == ChiCombo)
                        {
                            return OriginalHook(Ninjutsu);
                        }

                        if (WasLastAction(Jin))
                        {
                            return ChiCombo;
                        }

                        if (GetCooldownRemainingTime(OriginalHook(TrickAttack)) > 20)
                        {
                            return Jin;
                        }
                    }

                    if (!HasEffect(Buffs.Kassatsu) && !WasLastAbility(Kassatsu) && (HasEffect(Buffs.Mudra) || HasCharges(Ten) || WasLastAction(Ten)))
                    {
                        if (OriginalHook(Ninjutsu) == Suiton && MudraCheck[^3] == Ten && MudraCheck[^2] == ChiCombo && MudraCheck[^1] == JinCombo)
                        {
                            return OriginalHook(Ninjutsu);
                        }

                        if (WasLastAction(ChiCombo))
                        {
                            return JinCombo;
                        }

                        if (WasLastAction(Ten))
                        {
                            return ChiCombo;
                        }

                        if (GetCooldownRemainingTime(OriginalHook(TrickAttack)) <= 15 && !HasEffect(Buffs.ShadowWalker))
                        {
                            return Ten;
                        }
                    }
                }

                if (IsEnabled(Presets.NIN_ST_Bunshin) && HasEffect(Buffs.PhantomReady))
                {
                    return PhantomKamaitachi;
                }

                if (IsEnabled(Presets.NIN_ST_Raiju) && HasEffect(Buffs.RaijuReady))
                {
                    return FleetingRaiju;
                }

                if (IsEnabled(Presets.NIN_ST_ThrowingDagger) && ActionReady(ThrowingDagger) && ProjectileThresholdDistance())
                {
                    return ThrowingDagger;
                }

                if (ComboTime > 0)
                {
                    if (lastComboActionID is GustSlash)
                    {
                        if (ActionReady(AeolianEdge) && (Gauge.Kazematoi >= 4 || !LevelChecked(ArmorCrush)
                            || (Gauge.Kazematoi >= 1
                            && (TargetHasEffectAny(TrickList[OriginalHook(TrickAttack)]) || TargetHasEffectAny(MugList[OriginalHook(Mug)]) || BossAlmostDead()))))
                        {
                            return AeolianEdge;
                        }

                        if (ActionReady(ArmorCrush))
                        {
                            return ArmorCrush;
                        }
                    }

                    if (lastComboActionID is SpinningEdge && ActionReady(GustSlash))
                    {
                        return GustSlash;
                    }
                }

                return SpinningEdge;
            }

            return actionID;
        }
    }

    internal class NIN_AoE_DPS : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.NIN_AoE_DPS;

        protected override uint Invoke(uint actionID, uint lastComboActionID)
        {
            if ((actionID is DeathBlossom or HakkeMujinsatsu) && IsEnabled(Presets.NIN_AoE_DPS))
            {
                if (IsEnabled(Presets.NIN_AoE_Mudras) && ActionReady(Ten) && (!InCombat() || (ActionWatching.NumberOfGcdsUsed == 0 && HasEffect(Buffs.Mudra))))
                {
                    if (OriginalHook(Ninjutsu) is Huton)
                    {
                        return OriginalHook(Ninjutsu);
                    }

                    if (WasLastAction(JinCombo))
                    {
                        return TenCombo;
                    }

                    if (WasLastAction(Chi))
                    {
                        return JinCombo;
                    }

                    return Chi;
                }

                if (CanWeave(actionID, ActionWatching.LastGCD) && !HasEffect(Buffs.Mudra) && !HasEffect(Buffs.TenChiJin))
                {
                    if (IsEnabled(Presets.NIN_AoE_Kassatsu) && ActionReady(Kassatsu))
                    {
                        return Kassatsu;
                    }

                    if (IsEnabled(Presets.NIN_AoE_Mug) && ActionReady(OriginalHook(Mug)) && TargetIsBoss() && !TargetHasEffectAny(Debuffs.Dokumori))
                    {
                        return OriginalHook(Mug);
                    }

                    if (IsEnabled(Presets.NIN_AoE_Trick) && ActionReady(OriginalHook(TrickAttack))
                        && HasEffect(Buffs.ShadowWalker)
                        && (GetCooldownRemainingTime(OriginalHook(Mug)) > 30 || TargetHasEffectAny(MugList[OriginalHook(Mug)])))
                    {
                        return OriginalHook(TrickAttack);
                    }

                    if (IsEnabled(Presets.NIN_AoE_TenChiJin) && HasEffect(Buffs.TenriJindo) && ActionReady(TenriJindo)
                        && (TargetHasEffectAny(MugList[OriginalHook(Mug)]) || EffectRemainingTime(Buffs.TenriJindo) <= 5))
                    {
                        return TenriJindo;
                    }

                    if (IsEnabled(Presets.NIN_AoE_TenChiJin) && ActionReady(TenChiJin) && !HasEffect(Buffs.ShadowWalker)
                        && !HasEffect(Buffs.Mudra) && !HasEffect(Buffs.Kassatsu) && !IsMoving
                        && (GetCooldownRemainingTime(OriginalHook(TrickAttack)) <= 15
                        || (GetCooldownRemainingTime(Meisui) <= 15 && LevelChecked(Meisui))))
                    {
                        return TenChiJin;
                    }

                    if (IsEnabled(Presets.NIN_AoE_Meisui) && ActionReady(Meisui)
                        && HasEffect(Buffs.ShadowWalker) && Gauge.Ninki <= 50)
                    {
                        return Meisui;
                    }

                    if (IsEnabled(Presets.NIN_AoE_Bunshin) && ActionReady(Bunshin) && Gauge.Ninki >= 50)
                    {
                        return Bunshin;
                    }

                    if (IsEnabled(Presets.NIN_AoE_Hellfrog) && HasEffect(Buffs.Higi) && Gauge.Ninki > 50)
                    {
                        return DeathfrogMedium;
                    }

                    if (IsEnabled(Presets.NIN_AoE_Hellfrog) && ActionReady(Hellfrog) && Gauge.Ninki >= 50)
                    {
                        return Hellfrog;
                    }

                    if (IsEnabled(Presets.NIN_AoE_Assassinate) && ActionReady(OriginalHook(Assassinate)))
                    {
                        return OriginalHook(Assassinate);
                    }
                }

                if (IsEnabled(Presets.NIN_AoE_Mudras))
                {
                    if (HasEffect(Buffs.TenChiJin))
                    {
                        if (OriginalHook(Chi) is TCJDoton)
                        {
                            return TCJDoton;
                        }

                        if (OriginalHook(Ten) is TCJKaton)
                        {
                            return TCJKaton;
                        }

                        if (OriginalHook(Jin) is TCJFumaShurikenJin)
                        {
                            return TCJFumaShurikenJin;
                        }
                    }

                    if (HasEffect(Buffs.Kassatsu) && !WasLastAbility(Kassatsu))
                    {
                        if ((OriginalHook(Ninjutsu) == GokaMekkyaku || OriginalHook(Ninjutsu) == Katon)
                            && MudraCheck[^2] == ChiCombo && MudraCheck[^1] == TenCombo)
                        {
                            return OriginalHook(Ninjutsu);
                        }

                        if (WasLastAction(ChiCombo))
                        {
                            return TenCombo;
                        }

                        if (TargetHasEffectAny(TrickList[OriginalHook(TrickAttack)]) || (EffectRemainingTime(Buffs.Kassatsu) < 5 && !WasLastAbility(Kassatsu)))
                        {
                            return ChiCombo;
                        }
                    }

                    if (!HasEffect(Buffs.Kassatsu) && !WasLastAbility(Kassatsu)
                        && (HasEffect(Buffs.Mudra) || HasCharges(Ten)))
                    {
                        if (OriginalHook(Ninjutsu) == Katon && MudraCheck[^2] == Chi && MudraCheck[^1] == TenCombo)
                        {
                            return OriginalHook(Ninjutsu);
                        }

                        if (WasLastAction(Chi))
                        {
                            return TenCombo;
                        }

                        if (GetCooldownRemainingTime(OriginalHook(TrickAttack)) > 20)
                        {
                            return Chi;
                        }
                    }

                    if ((HasEffect(Buffs.Mudra) || HasCharges(Ten)) && !HasEffect(Buffs.Kassatsu) && !WasLastAbility(Kassatsu))
                    {
                        if (OriginalHook(Ninjutsu) == Huton && MudraCheck[^3] == Chi && MudraCheck[^2] == JinCombo && MudraCheck[^1] == TenCombo)
                        {
                            return OriginalHook(Ninjutsu);
                        }

                        if (WasLastAction(JinCombo))
                        {
                            return TenCombo;
                        }

                        if (WasLastAction(Chi))
                        {
                            return JinCombo;
                        }

                        if (GetCooldownRemainingTime(OriginalHook(TrickAttack)) <= 15 && !HasEffect(Buffs.ShadowWalker))
                        {
                            return Chi;
                        }
                    }
                }

                if (IsEnabled(Presets.NIN_AoE_Bunshin) && HasEffect(Buffs.PhantomReady))
                {
                    return PhantomKamaitachi;
                }

                if (lastComboActionID is DeathBlossom && ComboTime > 0 && ActionReady(HakkeMujinsatsu))
                {
                    return HakkeMujinsatsu;
                }

                return DeathBlossom;
            }

            return actionID;
        }
    }

    internal class NIN_Raijus : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.NIN_Raijus;

        protected override uint Invoke(uint actionID, uint lastComboActionID)
        {
            if ((actionID is FleetingRaiju or ForkedRaiju) && IsEnabled(Presets.NIN_Raijus))
            {
                if (HasEffect(Buffs.RaijuReady))
                {
                    if (InActionRange(FleetingRaiju))
                    {
                        return FleetingRaiju;
                    }

                    return ForkedRaiju;
                }
            }

            return actionID;
        }
    }

    internal class NIN_Doton : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.NIN_Doton;

        protected override uint Invoke(uint actionID, uint lastComboActionID)
        {
            if (actionID is Chi && IsEnabled(Presets.NIN_Doton))
            {
                if (HasEffect(Buffs.Kassatsu))
                {
                    return OriginalHook(11);
                }

                if ((ActionReady(Jin) || HasEffect(Buffs.Mudra)) && actionID is Chi)
                {
                    if (OriginalHook(Ninjutsu) is Doton && actionID is Chi)
                    {
                        return OriginalHook(Ninjutsu);
                    }

                    if (WasLastAction(JinCombo) && actionID is Chi)
                    {
                        return ChiCombo;
                    }

                    if (WasLastAction(Ten) && actionID is Chi)
                    {
                        return JinCombo;
                    }

                    if (actionID is Chi)
                    {
                        return Ten;
                    }
                }
            }

            return actionID;
        }
    }

    internal class NIN_MudraProtection : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.NIN_MudraProtection;

        protected override uint Invoke(uint actionID, uint lastComboActionID)
        {
            if (actionID is not SpinningEdge && actionID is not GustSlash && actionID is not AeolianEdge && actionID is not ArmorCrush
                && actionID is not DeathBlossom && actionID is not HakkeMujinsatsu && actionID is not Ten && actionID is not Chi && actionID is not Jin
                && IsEnabled(Presets.NIN_MudraProtection))
            {
                if (HasEffect(Buffs.Mudra))
                {
                    return OriginalHook(11);
                }

                return actionID;
            }

            return actionID;
        }
    }
}
