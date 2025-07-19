using Dalamud.Game.ClientState.JobGauge.Types;

using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Combos.Content;
using UltimateCombo.CustomCombo;
using UltimateCombo.Data;
using UltimateCombo.Services;

namespace UltimateCombo.Combos.PvE
{
    internal static class DRK
    {
        public const byte JobID = 32;

        public const uint
            HardSlash = 3617,
            SyphonStrike = 3623,
            Souleater = 3632,
            LivingDead = 3638,
            Unleash = 3621,
            StalwartSoul = 16468,
            CarveAndSpit = 3643,
            EdgeOfDarkness = 16467,
            EdgeOfShadow = 16470,
            Bloodspiller = 7392,
            ScarletDelirium = 36928,
            Comeuppance = 36929,
            Torcleaver = 36930,
            AbyssalDrain = 3641,
            FloodOfDarkness = 16466,
            FloodOfShadow = 16469,
            Quietus = 7391,
            SaltedEarth = 3639,
            SaltAndDarkness = 25755,
            Impalement = 36931,
            BloodWeapon = 3625,
            Delirium = 7390,
            LivingShadow = 16472,
            Oblation = 25754,
            Shadowbringer = 25757,
            Disesteem = 36932,
            Shadowstride = 36926,
            Unmend = 3624,
            TheBlackestNight = 7393;

        public static class Buffs
        {
            public const ushort
                BloodWeapon = 742,
                Delirium = 3836,
                BlackestNight = 1178,
                SaltedEarth = 749,
                Oblation = 2682,
                Scorn = 3837;
        }

        private static DRKGauge Gauge => CustomComboFunctions.GetJobGauge<DRKGauge>();

        public static class Config
        {
            public static UserInt
                DRK_ST_ManaSaver = new("DRK_ST_ManaSaver", 6000),
                DRK_AoE_ManaSaver = new("DRK_AoE_ManaSaver", 3000),
                DRK_BloodspillerGauge = new("DRK_BloodspillerGauge", 50),
                DRK_QuietusGauge = new("DRK_QuietusGauge", 50),
                DRK_AoE_Abyssal = new("DRK_AoE_Abyssal", 25),
                DRK_ST_Invuln = new("DRK_ST_Invuln", 10),
                DRK_AoE_Invuln = new("DRK_AoE_Invuln", 10);
        }

        internal class DRK_ST_DPS : CustomComboClass
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DRK_ST_DPS;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if ((actionID is HardSlash or SyphonStrike or Souleater) && IsEnabled(CustomComboPreset.DRK_ST_DPS))
                {
                    if (IsEnabled(CustomComboPreset.DRK_ST_Invuln) && PlayerHealthPercentageHp() <= GetOptionValue(Config.DRK_ST_Invuln)
                        && ActionReady(LivingDead))
                    {
                        return LivingDead;
                    }

                    if (IsEnabled(CustomComboPreset.DRK_ST_BlackestNight) && ActionReady(TheBlackestNight)
                        && !InCombat() && LocalPlayer?.CurrentMp >= 3000)
                    {
                        return TheBlackestNight;
                    }

                    if (IsEnabled(CustomComboPreset.DRK_ST_Unmend) && !InCombat() && ActionReady(Unmend) && !InMeleeRange())
                    {
                        return Unmend;
                    }

                    if (CanWeave(actionID))
                    {
                        if (IsEnabled(CustomComboPreset.DRK_ST_Edge) && ActionReady(OriginalHook(EdgeOfShadow))
                            && Gauge.DarksideTimeRemaining == 0)
                        {
                            return OriginalHook(EdgeOfShadow);
                        }

                        if (!HasEffect(Bozja.Buffs.BloodRage))
                        {
                            if (IsEnabled(CustomComboPreset.DRK_ST_LivingShadow) && ActionReady(LivingShadow))
                            {
                                return LivingShadow;
                            }

                            if (ActionWatching.NumberOfGcdsUsed >= 4 || Service.Configuration.IgnoreGCDChecks)
                            {
                                if (IsEnabled(CustomComboPreset.DRK_ST_Edge) && ActionReady(OriginalHook(EdgeOfShadow))
                                    && ((LocalPlayer?.CurrentMp >= GetOptionValue(Config.DRK_ST_ManaSaver)
                                    && (GetCooldownRemainingTime(LivingShadow) > 20 || !LevelChecked(LivingShadow) || HasEffect(Bozja.Buffs.AutoEther)))
                                    || Gauge.HasDarkArts || (!LevelChecked(TheBlackestNight) && LocalPlayer?.CurrentMp >= GetOptionValue(Config.DRK_ST_ManaSaver))
                                    || LocalPlayer?.CurrentMp >= 9500 || (HasEffect(Bozja.Buffs.AutoEther) && LocalPlayer?.CurrentMp >= 3000)))
                                {
                                    return OriginalHook(EdgeOfShadow);
                                }

                                if (IsEnabled(CustomComboPreset.DRK_ST_Delirium) && ActionReady(OriginalHook(Delirium)))
                                {
                                    return OriginalHook(Delirium);
                                }

                                if (IsEnabled(CustomComboPreset.DRK_ST_SaltedEarth) && ActionReady(OriginalHook(SaltedEarth)))
                                {
                                    return OriginalHook(SaltedEarth);
                                }

                                if (IsEnabled(CustomComboPreset.DRK_ST_Shadowbringer) && ActionReady(Shadowbringer)
                                    && !WasLastAbility(Shadowbringer) && Gauge.ShadowTimeRemaining > 0)
                                {
                                    return Shadowbringer;
                                }

                                if (IsEnabled(CustomComboPreset.DRK_ST_Carve) && ActionReady(CarveAndSpit))
                                {
                                    return CarveAndSpit;
                                }

                                if (IsEnabled(CustomComboPreset.DRK_ST_Oblation) && ActionReady(Oblation)
                                    && (GetRemainingCharges(Oblation) == GetMaxCharges(Oblation)
                                    || (GetRemainingCharges(Oblation) == 1 && GetCooldownChargeRemainingTime(Oblation) < 30)))
                                {
                                    return Oblation;
                                }
                            }
                        }
                    }

                    if (IsEnabled(CustomComboPreset.DRK_ST_LivingShadow) && HasEffect(Buffs.Scorn)
                        && (ActionWatching.NumberOfGcdsUsed >= 2 || Service.Configuration.IgnoreGCDChecks)
                        && (HasEffect(Buffs.Delirium) || GetBuffRemainingTime(Buffs.Scorn) <= 3))
                    {
                        return Disesteem;
                    }

                    if (IsEnabled(CustomComboPreset.DRK_ST_Bloodspiller) && ActionReady(OriginalHook(Bloodspiller))
                        && Gauge.DarksideTimeRemaining > 0
                        && (HasEffect(Buffs.Delirium) || Gauge.Blood >= GetOptionValue(Config.DRK_BloodspillerGauge)))
                    {
                        return OriginalHook(Bloodspiller);
                    }

                    if (IsEnabled(CustomComboPreset.DRK_ST_Unmend) && ActionReady(Unmend) && OutOfMeleeRange())
                    {
                        return Unmend;
                    }

                    if (comboTime > 0)
                    {
                        if (lastComboMove is HardSlash && ActionReady(SyphonStrike))
                        {
                            return SyphonStrike;
                        }

                        if (lastComboMove is SyphonStrike && ActionReady(Souleater))
                        {
                            return Souleater;
                        }
                    }

                    return HardSlash;
                }

                return actionID;
            }
        }

        internal class DRK_AoE_DPS : CustomComboClass
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DRK_AoE_DPS;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if ((actionID is Unleash or StalwartSoul) && IsEnabled(CustomComboPreset.DRK_AoE_DPS))
                {
                    if (IsEnabled(CustomComboPreset.DRK_AoE_Abyssal) && PlayerHealthPercentageHp() <= GetOptionValue(Config.DRK_AoE_Abyssal)
                        && ActionReady(AbyssalDrain))
                    {
                        return AbyssalDrain;
                    }

                    if (IsEnabled(CustomComboPreset.DRK_AoE_Invuln) && PlayerHealthPercentageHp() <= GetOptionValue(Config.DRK_AoE_Invuln)
                        && ActionReady(LivingDead))
                    {
                        return LivingDead;
                    }

                    if (IsEnabled(CustomComboPreset.DRK_AoE_Shadowstride) && ActionReady(Shadowstride) && !InMeleeRange()
                        && !InCombat())
                    {
                        return Shadowstride;
                    }

                    if (CanWeave(actionID))
                    {
                        if (IsEnabled(CustomComboPreset.DRK_AoE_BlackestNight) && ActionReady(TheBlackestNight)
                            && LocalPlayer?.CurrentMp >= 3000)
                        {
                            return TheBlackestNight;
                        }

                        if (IsEnabled(CustomComboPreset.DRK_AoE_Flood) && ActionReady(OriginalHook(FloodOfShadow))
                            && ((LocalPlayer?.CurrentMp >= GetOptionValue(Config.DRK_AoE_ManaSaver)
                            && (GetCooldownRemainingTime(LivingShadow) > 20 || !LevelChecked(LivingShadow)))
                            || Gauge.HasDarkArts || (!LevelChecked(TheBlackestNight) && LocalPlayer?.CurrentMp >= GetOptionValue(Config.DRK_ST_ManaSaver))
                            || LocalPlayer?.CurrentMp >= 9500))
                        {
                            return OriginalHook(FloodOfShadow);
                        }

                        if (IsEnabled(CustomComboPreset.DRK_AoE_LivingShadow) && ActionReady(LivingShadow))
                        {
                            return LivingShadow;
                        }

                        if (IsEnabled(CustomComboPreset.DRK_AoE_SaltedEarth) && ActionReady(OriginalHook(SaltedEarth)))
                        {
                            return OriginalHook(SaltedEarth);
                        }

                        if (IsEnabled(CustomComboPreset.DRK_AoE_Shadowbringer) && ActionReady(Shadowbringer))
                        {
                            return Shadowbringer;
                        }

                        if (IsEnabled(CustomComboPreset.DRK_AoE_Delirium) && ActionReady(OriginalHook(Delirium)))
                        {
                            return OriginalHook(Delirium);
                        }

                        if (IsEnabled(CustomComboPreset.DRK_AoE_Oblation) && ActionReady(Oblation)
                            && (GetRemainingCharges(Oblation) == GetMaxCharges(Oblation)
                            || (GetRemainingCharges(Oblation) == 1 && GetCooldownChargeRemainingTime(Oblation) < 30)))
                        {
                            return Oblation;
                        }
                    }

                    if (IsEnabled(CustomComboPreset.DRK_AoE_LivingShadow) && HasEffect(Buffs.Scorn) && HasEffect(Buffs.Delirium))
                    {
                        return Disesteem;
                    }

                    if (IsEnabled(CustomComboPreset.DRK_AoE_Quietus) && ActionReady(OriginalHook(Quietus)) && Gauge.DarksideTimeRemaining > 0
                        && (HasEffect(Buffs.Delirium) || Gauge.Blood >= GetOptionValue(Config.DRK_QuietusGauge)))
                    {
                        return OriginalHook(Quietus);
                    }

                    if (comboTime > 0)
                    {
                        if (lastComboMove is Unleash && ActionReady(StalwartSoul))
                        {
                            return StalwartSoul;
                        }
                    }

                    return Unleash;
                }

                return actionID;
            }
        }

        internal class DRK_DelirSpiller : CustomComboClass
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DRK_DelirSpiller;

            protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
            {
                if (actionID is Bloodspiller && IsEnabled(CustomComboPreset.DRK_DelirSpiller))
                {
                    if (ActionReady(OriginalHook(Delirium)))
                    {
                        return OriginalHook(Delirium);
                    }

                    if (HasEffect(Buffs.Delirium) || Gauge.Blood >= 50)
                    {
                        return OriginalHook(Bloodspiller);
                    }
                }

                return actionID;
            }
        }

        internal class DRK_DelirQuiet : CustomComboClass
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DRK_DelirQuiet;

            protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
            {
                if (actionID is Quietus && IsEnabled(CustomComboPreset.DRK_DelirQuiet))
                {
                    if (ActionReady(OriginalHook(Delirium)))
                    {
                        return OriginalHook(Delirium);
                    }

                    if (HasEffect(Buffs.Delirium) || Gauge.Blood >= 50)
                    {
                        return OriginalHook(Quietus);
                    }
                }

                return actionID;
            }
        }
    }
}
