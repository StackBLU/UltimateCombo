using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.Game.InstanceContent;
using System;
using UltimateCombo.Combos.PvE;
using UltimateCombo.Combos.PvP;
using UltimateCombo.Data;
using UltimateCombo.Services;

namespace UltimateCombo.ComboHelper.Functions
{
	internal abstract partial class CustomComboFunctions
	{
		public static uint OriginalHook(uint actionID)
		{
			return Service.IconReplacer.OriginalHook(actionID);
		}

		public static bool IsOriginal(uint actionID)
		{
			return Service.IconReplacer.OriginalHook(actionID) == actionID;
		}

		public static bool LevelChecked(uint actionid)
		{
			return SafeLocalPlayer.Level >= GetLevel(actionid) && IsActionUnlocked(actionid);
		}

		public static bool TraitActionReady(uint traitid)
		{
			return SafeLocalPlayer.Level >= GetTraitLevel(traitid);
		}

		public static string GetActionName(uint id)
		{
			return ActionWatching.GetActionName(id);
		}

		public static ActionWatching.ActionAttackType GetActionType(uint id)
		{
			return ActionWatching.GetAttackType(id);
		}

		public static int GetLevel(uint id)
		{
			return ActionWatching.GetLevel(id);
		}

		internal static unsafe float GetActionCastTime(uint id)
		{
			return ActionWatching.GetActionCastTime(id);
		}

		public static bool InActionRange(uint id)
		{
			var range = ActionWatching.GetActionRange(id);

			if (HasTarget())
			{
				switch (range)
				{
					case -2:
						return false;
					case -1:
						return InMeleeRange();
					case 0:
						{
							float radius = ActionWatching.GetActionEffectRange(id);
							return radius <= 0 || (HasTarget() && GetTargetDistance() <= (radius - 0.5f));
						}
					default:
						return GetTargetDistance() <= range;
				}
			}

			return false;
		}

		public static bool MaxActionRange(uint id)
		{
			var range = ActionWatching.GetActionRange(id);
			switch (range)
			{
				case -2:
					return false;
				case -1:
					return InMeleeRange();
				case 0:
					{
						float radius = ActionWatching.GetActionEffectRange(id);
						return radius <= 0 || (HasTarget() && GetTargetDistance() >= (radius - 2f));
					}
				default:
					return GetTargetDistance() >= range - 2;
			}
		}

		public static int GetTraitLevel(uint id)
		{
			return ActionWatching.GetTraitLevel(id);
		}

		public static bool ActionReady(uint id)
		{
			if (LevelChecked(id) && (HasCharges(id) || GetCooldown(id).CooldownTotal <= 5))
			{
				if (GetActionType(id) == ActionWatching.ActionAttackType.Weaponskill && !HasPacification())
				{
					return true;
				}

				if (GetActionType(id) == ActionWatching.ActionAttackType.Spell && !HasSilence())
				{
					return true;
				}

				if (GetActionType(id) == ActionWatching.ActionAttackType.Ability && !HasAmnesia() && !HasSilence())
				{
					return true;
				}
			}

			return false;
		}

		public static bool DutyActionReady(uint id)
		{
			return IsEnabled(id) && IsOffCooldown(id);
		}

		public static bool DutyActionEquipped(uint actionId)
		{
			return (DutyActionManager.GetDutyActionId(0) == actionId) || (DutyActionManager.GetDutyActionId(1) == actionId);
		}

		public static bool DutyActionNotEquipped(uint actionId)
		{
			return (DutyActionManager.GetDutyActionId(0) != actionId) && (DutyActionManager.GetDutyActionId(1) != actionId);
		}

		public static bool WasLastAction(uint id)
		{
			return ActionWatching.LastAction == id;
		}

		public static int LastActionCounter()
		{
			return ActionWatching.LastActionUseCount;
		}

		public static bool WasLastWeaponskill(uint id)
		{
			return ActionWatching.LastWeaponskill == id;
		}

		public static bool WasLastSpell(uint id)
		{
			return ActionWatching.LastSpell == id;
		}

		public static bool WasLastGCD(uint id)
		{
			return ActionWatching.LastGCD == id;
		}

		public static bool WasLastAbility(uint? id)
		{
			return ActionWatching.LastAbility == id;
		}

		public static bool IsSpellActive(uint id)
		{
			return Service.Configuration.ActiveBLUSpells.Contains(id);
		}

		public static bool CanWeave(uint actionID, double weaveTime = 0.6)
		{
			if (Service.Configuration.DisableTripleWeaving)
			{
				return (GetCooldown(actionID).CooldownRemaining >= weaveTime && !ActionWatching.HasDoubleWeaved()) || HasSilence() || HasPacification();
			}

			return GetCooldown(actionID).CooldownRemaining >= weaveTime || HasSilence() || HasPacification();
		}

		public static bool CanDelayedWeave(uint actionID, double weaveTime = 0.6, double weaveStart = 0.8)
		{
			return (GetCooldown(actionID).CooldownRemaining <= weaveStart && GetCooldown(actionID).CooldownRemaining >= weaveTime)
				|| HasSilence() || HasPacification();
		}

		public static unsafe float ComboTimer => ActionManager.Instance()->Combo.Timer;

		public static unsafe uint ComboAction => ActionManager.Instance()->Combo.Action;

		public static unsafe int FindHolsterItem(uint itemID)
		{
			return PublicContentBozja.GetState()->HolsterActions.IndexOf((byte) itemID);
		}

		public static unsafe bool HasHolsterItem(uint itemID)
		{
			return FindHolsterItem(itemID) >= 0;
		}

		public static unsafe bool UseFromHolster(uint holsterIndex, uint slot)
		{
			return PublicContentBozja.GetInstance()->UseFromHolster(holsterIndex, slot);
		}

		public static unsafe bool UseAction(ActionType actionType, uint actionID)
		{
			return ActionManager.Instance()->UseAction(actionType, actionID);
		}

		public static unsafe bool UseActionNow(ActionType actionType, uint actionID)
		{
			return ActionManager.Instance()->UseActionLocation(actionType, actionID);
		}

		public static unsafe float AnimationLock()
		{
			return ActionManager.Instance()->AnimationLock;
		}

		public static unsafe bool ActionQueued()
		{
			return ActionManager.Instance()->ActionQueued;
		}

		public static unsafe void AssignBlueMageActionToSlot(int slot, uint actionID)
		{
			ActionManager.Instance()->AssignBlueMageActionToSlot(slot, actionID);
		}

		public static unsafe uint GetActiveBlueMageActionInSlot(int slot)
		{
			return ActionManager.Instance()->GetActiveBlueMageActionInSlot(slot);
		}

		public static unsafe bool SetBlueMageActions(uint* actionArray)
		{
			return ActionManager.Instance()->SetBlueMageActions(actionArray);
		}

		public static unsafe void SwapBlueMageActionSlots(int slotA, int slotB)
		{
			ActionManager.Instance()->SwapBlueMageActionSlots(slotA, slotB);
		}

		public static bool SafeToUse()
		{
			return !WasLastSpell(RDM.Verflare) && !WasLastSpell(RDM.Verholy) && !WasLastSpell(RDM.Scorch)
				&& !WasLastGCD(RDM.EnchantedRiposte) && !WasLastGCD(RDM.EnchantedZwerchhau) && !WasLastGCD(RDM.EnchantedRedoublement)
				&& !WasLastGCD(RDM.EnchantedMoulinet) && !WasLastGCD(RDM.EnchantedMoulinetDeux) && !WasLastGCD(RDM.EnchantedMoulinetTrois)
				&& !HasEffect(NIN.Buffs.Mudra) && !HasEffect(NIN.Buffs.TenChiJin) && !HasEffect(VPR.Buffs.Reawakened)
				&& !WasLastSpell(SGE.Eukrasia)
				&& !WasLastSpell(SGE.EukrasianDosis1) && !WasLastSpell(SGE.EukrasianDosis2) && !WasLastSpell(SGE.EukrasianDosis3)
				&& !WasLastSpell(SGE.EukrasianDyskrasia)
				&& !HasEffect(DNC.Buffs.StandardStep) && !HasEffect(DNC.Buffs.TechnicalStep);
		}

		public static bool IsComboAction(uint actionID)
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

		public static bool IsPvPComboAction(uint actionID)
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
}
