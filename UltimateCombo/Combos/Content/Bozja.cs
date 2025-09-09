using UltimateCombo.Combos.PvE;
using UltimateCombo.Core;

namespace UltimateCombo.Combos.Content;

internal static class Bozja
{
    internal const uint
        Banish3 = 20702,
        FontOfMagic = 20715,
        FontOfPower = 20717,
        Slash = 20718,
        BannerOfNobleEnds = 20720,
        BannerOfHonoredSacrifice = 20721,
        Cure = 20726,
        Cure2 = 20727,
        Cure3 = 20728,
        Cure4 = 20729,
        Incense = 20731,
        FlareStar = 22352,
        RendArmor = 22353,
        SeraphStrike = 22354,
        Aethershield = 22355,
        Dervish = 22356,
        Burst = 23909,
        Rampage = 23910,
        Chainspell = 23913,
        Assassination = 23914,
        Excellence = 23919,
        BloodRage = 23921;

    internal static class Buffs
    {
        internal const ushort
            MPRefresh = 909,
            MPRefresh2 = 1198,
            ProfaneEssence = 2320,
            IrregularEssence = 2321,
            BeastEssence = 2324,
            BannerOfNobleEnds = 2326,
            BannerOfHonoredSacrifice = 2327,
            FontOfMagic = 2332,
            Bravery2 = 2341,
            AutoPotion = 2342,
            AutoEther = 2343,
            FontOfPower = 2346,
            Reminiscence = 2348,
            PureElder = 2435,
            PureFiendhunter = 2437,
            PureIndomitable = 2438,
            PureDivine = 2439,
            Aethershield = 2443,
            Dervish = 2444,
            ClericStance = 2484,
            Chainspell = 2560,
            Excellence = 2564,
            BloodRage = 2566,
            BloodRush = 2567;
    }

    internal static class Debuffs
    {
        internal const ushort
            FlareStar = 2440,
            RendArmor = 2441;
    }

    internal static class Items
    {
        internal const uint
            EtherKit = 38;
    }

    internal class Bozja_BloodRage : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.Bozja_BloodRage;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.Bozja_BloodRage) && HasEffect(Buffs.Reminiscence) && InCombat() && IsComboAction(actionID))
            {
                if (DutyActionReady(BloodRage) && DutyActionEquipped(BloodRage))
                {
                    return BloodRage;
                }

                if (EffectStacks(Buffs.BloodRage) == 1
                    || (EffectStacks(Buffs.BloodRage) == 2 && EffectRemainingTime(Buffs.BloodRage) < 3)
                    || EffectStacks(Buffs.BloodRage) == 3)
                {
                    if (CurrentJobId is PLD.JobID && ActionReady(PLD.Intervene))
                    {
                        return PLD.Intervene;
                    }

                    if (CurrentJobId is WAR.JobID && ActionReady(WAR.Onslaught))
                    {
                        return WAR.Onslaught;
                    }

                    if (CurrentJobId is DRK.JobID && ActionReady(DRK.Shadowstride))
                    {
                        return DRK.Shadowstride;
                    }

                    if (CurrentJobId is GNB.JobID && ActionReady(GNB.Trajectory))
                    {
                        return GNB.Trajectory;
                    }
                }
            }

            return actionID;
        }
    }

    internal class Bozja_Seraph : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.Bozja_Seraph;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.Bozja_Seraph) && HasEffect(Buffs.Reminiscence) && InCombat() && CurrentJobId is WHM.JobID
                 && IsComboAction(actionID))
            {
                if (DutyActionReady(SeraphStrike) && DutyActionEquipped(SeraphStrike) && ActionReady(WHM.ThinAir) && !HasEffect(WHM.Buffs.ThinAir))
                {
                    return WHM.ThinAir;
                }

                if (DutyActionReady(SeraphStrike) && DutyActionEquipped(SeraphStrike) && HasEffect(WHM.Buffs.ThinAir)
                    && (InMeleeRange() || EffectRemainingTime(WHM.Buffs.ThinAir) < 3))
                {
                    return SeraphStrike;
                }
            }

            return actionID;
        }
    }

    internal class Bozja_FoP_HSac_NEnds : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.Bozja_FoP_HSac_NEnds;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.Bozja_FoP_HSac_NEnds) && HasEffect(Buffs.Reminiscence) && InCombat() && SafeToUse()
                 && IsComboAction(actionID))
            {
                if (DutyActionReady(FontOfPower) && DutyActionEquipped(FontOfPower)
                    && (!HasEffect(Buffs.BloodRage) || HasEffect(Buffs.BloodRush)))
                {
                    return FontOfPower;
                }

                if (DutyActionReady(BannerOfHonoredSacrifice) && DutyActionEquipped(BannerOfHonoredSacrifice)
                    && (WasLastAction(FontOfPower) || HasEffect(Buffs.FontOfPower)))
                {
                    return BannerOfHonoredSacrifice;
                }

                if (DutyActionReady(BannerOfNobleEnds) && DutyActionEquipped(BannerOfNobleEnds)
                    && (WasLastAction(FontOfPower) || HasEffect(Buffs.FontOfPower)))
                {
                    return BannerOfNobleEnds;
                }
            }

            return actionID;
        }
    }

    internal class Bozja_Assassination : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.Bozja_Assassination;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.Bozja_Assassination) && HasEffect(Buffs.Reminiscence) && InCombat() && SafeToUse()
                 && IsComboAction(actionID))
            {
                if (DutyActionReady(Assassination) && DutyActionEquipped(Assassination) && InActionRange(Assassination))
                {
                    return Assassination;
                }
            }

            return actionID;
        }
    }

    internal class Bozja_FoM_CS : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.Bozja_FoM_CS;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.Bozja_FoM_CS) && HasEffect(Buffs.Reminiscence) && InCombat() && DutyActionNotEquipped(FlareStar)
                 && IsComboAction(actionID))
            {
                if (DutyActionReady(FontOfMagic) && DutyActionEquipped(FontOfMagic))
                {
                    return FontOfMagic;
                }

                if (DutyActionReady(Chainspell) && DutyActionEquipped(Chainspell) && (WasLastAction(FontOfMagic) || HasEffect(Buffs.FontOfMagic)))
                {
                    return Chainspell;
                }
            }

            return actionID;
        }
    }

    internal class Bozja_Slash : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.Bozja_Slash;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.Bozja_Slash) && HasEffect(Buffs.Reminiscence) && InCombat() && DutyActionEquipped(Slash)
                 && IsComboAction(actionID))
            {
                if (DutyActionReady(Slash) && DutyActionEquipped(Slash) && JobID != MCH.JobID)
                {
                    return Slash;
                }

                if (DutyActionReady(Slash) && DutyActionEquipped(Slash) && CurrentJobId is MCH.JobID)
                {
                    if (ActionReady(MCH.Reassemble))
                    {
                        return MCH.Reassemble;
                    }

                    if (HasEffect(MCH.Buffs.Reassembled) || WasLastAbility(MCH.Reassemble))
                    {
                        return Slash;
                    }
                }
            }

            return actionID;
        }
    }

    internal class Bozja_CureIV : CustomComboBase
    {
        protected internal override Presets Preset { get; } = Presets.Bozja_CureIV;

        protected override uint Invoke(uint actionID, uint lastComboMove)
        {
            if (IsEnabled(Presets.Bozja_CureIV) && HasEffect(Buffs.Reminiscence) && InCombat() && DutyActionEquipped(Cure4)
                 && IsComboAction(actionID))
            {
                if (DutyActionReady(Cure4) && DutyActionEquipped(Cure4)
                    && (PlayerHealthPercentageHp() <= 50 || (HasEffect(Buffs.PureElder) && EffectRemainingTimeAny(Buffs.Bravery2) < 3)))
                {
                    return Cure4;
                }
            }

            return actionID;
        }
    }
}
