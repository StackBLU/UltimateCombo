using FFXIVClientStructs.FFXIV.Client.Game;
using System.Linq;
using UltimateCombo.Combos;
using UltimateCombo.Combos.General;
using UltimateCombo.Combos.PvE;
using UltimateCombo.Combos.PvP;
using UltimateCombo.Core;
using UltimateCombo.Data;

namespace UltimateCombo.ComboHelper.Functions;

internal abstract partial class CustomComboFunctions
{
    public static unsafe float ComboTime => ActionManager.Instance()->Combo.Timer;

    internal static uint OriginalHook(uint actionID)
    {
        return Service.IconReplacer.OriginalHook(actionID);
    }

    internal static bool IsEnabled(Presets preset)
    {
        return PresetStorage.IsEnabled(preset);
    }

    internal static bool LevelChecked(uint actionid)
    {
        return Level >= ActionWatching.GetLevel(actionid) && IsActionUnlocked(actionid);
    }

    internal static bool InActionRange(uint id)
    {
        var range = ActionWatching.GetActionRange(id);
        if (!HasTarget())
        {
            return false;
        }

        if (range == -2)
        {
            return false;
        }

        if (range == -1)
        {
            return InMeleeRange();
        }

        if (range == 0)
        {
            var radius = ActionWatching.GetActionEffectRange(id);
            return radius <= 0 || GetTargetDistance() <= (radius - 0.5f);
        }
        return GetTargetDistance() <= range;
    }

    internal static bool ActionReady(uint actionID)
    {
        ActionWatching.ActionAttackType attackType = ActionWatching.GetAttackType(actionID);

        if (!LevelChecked(actionID) || (!HasCharges(actionID) && GetCooldown(actionID).CooldownTotal > 6))
        {
            return false;
        }

        if (attackType == ActionWatching.ActionAttackType.Weaponskill)
        {
            return !HasPacification();
        }

        if (attackType == ActionWatching.ActionAttackType.Spell)
        {
            return !HasSilence();
        }

        if (attackType == ActionWatching.ActionAttackType.Ability)
        {
            return !HasAmnesia() && !HasSilence();
        }

        return false;
    }

    internal static bool DutyActionReady(uint id)
    {
        return IsActionEnabled(id) && IsOffCooldown(id);
    }

    internal static bool DutyActionEquipped(uint actionId)
    {
        return DutyActionManager.GetDutyActionId(0) == actionId || DutyActionManager.GetDutyActionId(1) == actionId;
    }

    internal static bool DutyActionNotEquipped(uint actionId)
    {
        return !DutyActionEquipped(actionId);
    }

    internal static bool WasLastAction(uint id)
    {
        return ActionWatching.LastAction == id;
    }

    internal static bool WasLastWeaponskill(uint id)
    {
        return ActionWatching.LastWeaponskill == id;
    }

    internal static bool WasLastSpell(uint id)
    {
        return ActionWatching.LastSpell == id;
    }

    internal static bool WasLastGCD(uint id)
    {
        return ActionWatching.LastGCD == id;
    }

    internal static bool WasLastAbility(uint? id)
    {
        return ActionWatching.LastAbility == id;
    }

    internal static bool IsSpellActive(uint id)
    {
        return Service.Configuration.ActiveBLUSpells.Contains(id);
    }

    internal static bool CanWeave(uint actionID, uint lastGCD)
    {
        var weaveTime = 0.6;
        var skillToUse = lastGCD;

        if (GetCooldown(lastGCD).CooldownTotal >= 6.5)
        {
            skillToUse = actionID;
        }

        if (HasSilence() || HasPacification())
        {
            return true;
        }

        if (Service.Configuration.DisableTripleWeaving)
        {
            return GetCooldown(skillToUse).CooldownRemaining >= weaveTime && !ActionWatching.HasDoubleWeaved();
        }

        return GetCooldown(skillToUse).CooldownRemaining >= weaveTime;
    }

    internal static bool CanLateWeave(uint actionID, uint lastGCD)
    {
        var weaveTime = 0.6;
        var weaveStart = 0.8;
        var skillToUse = lastGCD;

        if (GetCooldown(lastGCD).CooldownTotal >= 6.5)
        {
            skillToUse = actionID;
        }

        if (HasSilence() || HasPacification())
        {
            return true;
        }

        if (Service.Configuration.DisableTripleWeaving)
        {
            return GetCooldown(skillToUse).CooldownRemaining >= weaveTime && !ActionWatching.HasDoubleWeaved();
        }

        return GetCooldown(skillToUse).CooldownRemaining <= weaveStart && GetCooldown(skillToUse).CooldownRemaining >= weaveTime;
    }

    internal static unsafe bool UseAction(ActionType actionType, uint actionID)
    {
        return ActionManager.Instance()->UseAction(actionType, actionID);
    }

    internal static unsafe bool ActionQueued()
    {
        return ActionManager.Instance()->ActionQueued;
    }

    internal static unsafe void AssignBlueMageActionToSlot(int slot, uint actionID)
    {
        ActionManager.Instance()->AssignBlueMageActionToSlot(slot, actionID);
    }

    internal static unsafe uint GetActiveBlueMageActionInSlot(int slot)
    {
        return ActionManager.Instance()->GetActiveBlueMageActionInSlot(slot);
    }

    internal static unsafe void PopulateBLUSpells()
    {
        var prevList = Service.Configuration.ActiveBLUSpells.ToList();
        Service.Configuration.ActiveBLUSpells.Clear();

        for (var i = 0; i <= 24; i++)
        {
            var id = ActionManager.Instance()->GetActiveBlueMageActionInSlot(i);
            if (id != 0)
            {
                Service.Configuration.ActiveBLUSpells.Add(id);
            }
        }

        if (Service.Configuration.ActiveBLUSpells.Except(prevList).Any())
        {
            Service.Configuration.Save();
        }
    }

    internal static bool SafeToUse()
    {
        var rdmSafe = !WasLastSpell(RDM.Verflare) && !WasLastSpell(RDM.Verholy) && !WasLastSpell(RDM.Scorch)
                   && !WasLastGCD(RDM.EnchantedRiposte) && !WasLastGCD(RDM.EnchantedZwerchhau) && !WasLastGCD(RDM.EnchantedRedoublement)
                   && !WasLastGCD(RDM.EnchantedMoulinet) && !WasLastGCD(RDM.EnchantedMoulinetDeux) && !WasLastGCD(RDM.EnchantedMoulinetTrois);

        var ninSafe = !HasEffect(NIN.Buffs.Mudra) && !HasEffect(NIN.Buffs.TenChiJin);

        var vprSafe = !HasEffect(VPR.Buffs.Reawakened);

        var sgeSafe = !WasLastSpell(SGE.Eukrasia) && !WasLastSpell(SGE.EukrasianDosis1)
                   && !WasLastSpell(SGE.EukrasianDosis2) && !WasLastSpell(SGE.EukrasianDosis3)
                   && !WasLastSpell(SGE.EukrasianDyskrasia);

        var dncSafe = !HasEffect(DNC.Buffs.StandardStep) && !HasEffect(DNC.Buffs.TechnicalStep);

        return rdmSafe && ninSafe && vprSafe && sgeSafe && dncSafe;
    }

    internal static bool IsComboAction(uint actionID)
    {
        return actionID
            is AST.Malefic or AST.Malefic2 or AST.Malefic3 or AST.Malefic4 or AST.FallMalefic
            or AST.Gravity or AST.Gravity2

            or BLM.Fire or BLM.Fire3 or BLM.Fire4 or BLM.Blizzard or BLM.Blizzard3 or BLM.Blizzard4
            or BLM.Fire2 or BLM.HighFire2 or BLM.Blizzard2 or BLM.HighBlizzard2

            or BLU.PeripheralSynthesis or BLU.MustardBomb
            or BLU.GoblinPunch or BLU.SonicBoom or BLU.ChocoMeteor

            or BRD.HeavyShot or BRD.BurstShot or BRD.StraightShot or BRD.RefulgentArrow
            or BRD.QuickNock or BRD.Ladonsbite or BRD.WideVolley or BRD.Shadowbite

            or DNC.Cascade or DNC.Fountain
            or DNC.Windmill or DNC.Bladeshower

            or DRG.TrueThrust or DRG.VorpalThrust or DRG.LanceBarrage or DRG.Disembowel or DRG.SpiralBlow or DRG.FullThrust
            or DRG.HeavensThrust or DRG.ChaosThrust or DRG.ChaoticSpring or DRG.FangAndClaw or DRG.WheelingThrust
            or DRG.DoomSpike or DRG.SonicThrust or DRG.CoerthanTorment

            or DRK.HardSlash or DRK.SyphonStrike or DRK.Souleater
            or DRK.Unleash or DRK.StalwartSoul

            or GNB.KeenEdge or GNB.BrutalShell or GNB.SolidBarrel
            or GNB.DemonSlice or GNB.DemonSlaughter

            or MCH.SplitShot or MCH.SlugShot or MCH.CleanShot or MCH.HeatedSplitShot or MCH.HeatedSlugShot or MCH.HeatedCleanShot
            or MCH.SpreadShot or MCH.Scattergun or MCH.AutoCrossbow

            or MNK.Bootshine or MNK.LeapingOpo or MNK.TrueStrike or MNK.RisingRaptor or MNK.SnapPunch
            or MNK.PouncingCoeurl or MNK.DragonKick or MNK.TwinSnakes or MNK.Demolish
            or MNK.ArmOfTheDestroyer or MNK.ShadowOfTheDestroyer or MNK.FourPointFury or MNK.Rockbreaker

            or NIN.SpinningEdge or NIN.GustSlash or NIN.AeolianEdge or NIN.ArmorCrush
            or NIN.DeathBlossom or NIN.HakkeMujinsatsu

            or PCT.Fire or PCT.Aero or PCT.Water or PCT.Blizzard or PCT.Stone or PCT.Thunder
            or PCT.Fire2 or PCT.Aero2 or PCT.Water2 or PCT.Blizzard2 or PCT.Stone2 or PCT.Thunder2

            or PLD.FastBlade or PLD.RiotBlade or PLD.RageOfHalone or PLD.RoyalAuthority
            or PLD.TotalEclipse or PLD.Prominence

            or RDM.Jolt or RDM.Jolt2 or RDM.Jolt3 or RDM.Verthunder or RDM.Verthunder3 or RDM.Veraero or RDM.Veraero3 or RDM.Verfire or RDM.Verstone
            or RDM.Scatter or RDM.Impact or RDM.Verthunder2 or RDM.Veraero2

            or RPR.Slice or RPR.WaxingSlice or RPR.InfernalSlice
            or RPR.SpinningScythe or RPR.NightmareScythe

            or SAM.Hakaze or SAM.Gyofu or SAM.Jinpu or SAM.Gekko or SAM.Shifu or SAM.Kasha or SAM.Yukikaze
            or SAM.Fuga or SAM.Fuko or SAM.Mangetsu or SAM.Oka

            or SCH.Ruin or SCH.Broil or SCH.Broil2 or SCH.Broil3 or SCH.Broil4
            or SCH.ArtOfWar or SCH.ArtOfWar2

            or SGE.Dosis1 or SGE.Dosis2 or SGE.Dosis3 or SGE.EukrasianDosis1 or SGE.EukrasianDosis2 or SGE.EukrasianDosis3
            or SGE.Dyskrasia1 or SGE.Dyskrasia2 or SGE.EukrasianDyskrasia

            or SMN.Ruin or SMN.Ruin2 or SMN.Ruin3
            or SMN.Outburst or SMN.Tridisaster

            or VPR.SteelFangs or VPR.ReavingFangs
            or VPR.SteelMaw or VPR.ReavingMaw

            or WAR.HeavySwing or WAR.Maim or WAR.StormsPath or WAR.StormsEye
            or WAR.Overpower or WAR.MythrilTempest

            or WHM.Stone1 or WHM.Stone2 or WHM.Stone3 or WHM.Stone4 or WHM.Glare1 or WHM.Glare3
            or WHM.Holy or WHM.Holy3;
    }

    internal static bool IsPvPComboAction(uint actionID)
    {
        return actionID
            is ASTPvP.FallMalefic

            or BLMPvP.Fire or BLMPvP.Fire3 or BLMPvP.Fire4 or BLMPvP.HighFire2 or BLMPvP.Flare or BLMPvP.Blizzard or BLMPvP.Blizzard3
            or BLMPvP.Blizzard4 or BLMPvP.HighBlizzard2 or BLMPvP.Freeze

            or BRDPvP.PowerfulShot or BRDPvP.PitchPerfect

            or DNCPvP.Cascade or DNCPvP.Fountain or DNCPvP.ReverseCascade or DNCPvP.Fountainfall or DNCPvP.SaberDance

            or DRGPvP.RaidenThrust or DRGPvP.FangAndClaw or DRGPvP.WheelingThrust or DRGPvP.Drakesbane or DRGPvP.HeavensThrust

            or DRKPvP.HardSlash or DRKPvP.SyphonStrike or DRKPvP.Souleater or DRKPvP.ScarletDelirium or DRKPvP.Comeuppance or DRKPvP.Torcleaver

            or GNBPvP.KeenEdge or GNBPvP.BrutalShell or GNBPvP.SolidBarrel or GNBPvP.BurstStrike

            or MCHPvP.BlastCharge or MCHPvP.BlazingShot

            or MNKPvP.DragonKick or MNKPvP.TwinSnakes or MNKPvP.Demolish or MNKPvP.LeapingOpo or MNKPvP.RisingRaptor or MNKPvP.PouncingCoeurl or MNKPvP.PhantomRush

            or NINPvP.SpinningEdge or NINPvP.GustSlash or NINPvP.AeolianEdge or NINPvP.ZeshoMeppo or NINPvP.ForkedRaiju or NINPvP.FleetingRaiju or NINPvP.Assassinate

            or PCTPvP.FireInRed or PCTPvP.AeroInGreen or PCTPvP.WaterInBlue or PCTPvP.BlizzardInCyan or PCTPvP.StoneInYellow or PCTPvP.ThunderInMagenta

            or PLDPvP.FastBlade or PLDPvP.RiotBlade or PLDPvP.RoyalAuthority or PLDPvP.Atonement or PLDPvP.Supplication or PLDPvP.Sepulchre

            or RDMPvP.Jolt3 or RDMPvP.GrandImpact

            or RPRPvP.Slice or RPRPvP.WaxingSlice or RPRPvP.InfernalSlice

            or SAMPvP.Yukikaze or SAMPvP.Gekko or SAMPvP.Kasha or SAMPvP.Hyosetsu or SAMPvP.Mangetsu or SAMPvP.Oka

            or SCHPvP.Broil4 or SCHPvP.SeraphicHalo

            or SGEPvP.Dosis3 or SGEPvP.EukrasianDosis3

            or SMNPvP.Ruin3 or SMNPvP.Ruin4

            or VPRPvP.SteelFangs or VPRPvP.HuntersSting or VPRPvP.BarbarousBite or VPRPvP.PiercingFangs or VPRPvP.SwiftskinsSting or VPRPvP.RavenousBite

            or WARPvP.HeavySwing or WARPvP.Maim or WARPvP.StormsPath or WARPvP.FellCleave

            or WHMPvP.Glare3;
    }
}
