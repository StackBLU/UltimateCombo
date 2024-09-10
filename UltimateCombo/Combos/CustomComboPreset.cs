using UltimateCombo.Attributes;
using UltimateCombo.Combos.PvE;
using UltimateCombo.Combos.PvE.Content;
using UltimateCombo.Combos.PvP;

namespace UltimateCombo.Combos
{
	public enum CustomComboPreset
	{
		#region PvE

		#region Tanks

		#region PALADIN - 11000

		#region Single Target DPS

		[ReplaceSkill(PLD.FastBlade, PLD.RiotBlade, PLD.RageOfHalone, PLD.RoyalAuthority)]
		[CustomComboInfo("Single Target DPS", "", PLD.JobID)]
		PLD_ST_DPS = 11000,

		[ParentCombo(PLD_ST_DPS)]
		[CustomComboInfo("Fight or Flight > Goring Blade", "", PLD.JobID)]
		PLD_ST_FightOrFlight = 11001,

		[ParentCombo(PLD_ST_DPS)]
		[CustomComboInfo("Expiacion", "", PLD.JobID)]
		PLD_ST_SpiritsWithin = 11002,

		[ParentCombo(PLD_ST_DPS)]
		[CustomComboInfo("Circle of Scorn", "", PLD.JobID)]
		PLD_ST_CircleOfScorn = 11003,

		[ParentCombo(PLD_ST_DPS)]
		[CustomComboInfo("Atonement > Supplication > Sepulchre", "", PLD.JobID)]
		PLD_ST_Atonement = 11004,

		[ParentCombo(PLD_ST_DPS)]
		[CustomComboInfo("Requisecat / Imperator > Blade of Honor", "", PLD.JobID)]
		PLD_ST_Imperator = 11005,

		[ParentCombo(PLD_ST_DPS)]
		[CustomComboInfo("Confiteor > Blade of Faith/Truth/Valor", "", PLD.JobID)]
		PLD_ST_Confiteor = 11006,

		[ParentCombo(PLD_ST_DPS)]
		[CustomComboInfo("Holy Spirit", "", PLD.JobID)]
		PLD_ST_HolySpirit = 11007,

		[ParentCombo(PLD_ST_DPS)]
		[CustomComboInfo("Intervene", "", PLD.JobID)]
		PLD_ST_Intervene = 11008,

		[ParentCombo(PLD_ST_DPS)]
		[CustomComboInfo("Sheltron", "", PLD.JobID)]
		PLD_ST_Sheltron = 11009,

		[ParentCombo(PLD_ST_DPS)]
		[CustomComboInfo("Intervention", "", PLD.JobID)]
		PLD_ST_Intervention = 11010,

		[ParentCombo(PLD_ST_DPS)]
		[CustomComboInfo("Hallowed Ground", "", PLD.JobID)]
		PLD_ST_Invuln = 11011,

		#endregion

		#region AoE DPS

		[ReplaceSkill(PLD.TotalEclipse, PLD.Prominence)]
		[CustomComboInfo("AoE DPS", "", PLD.JobID)]
		PLD_AoE_DPS = 11020,

		[ParentCombo(PLD_AoE_DPS)]
		[CustomComboInfo("Fight or Flight", "", PLD.JobID)]
		PLD_AoE_FightOrFlight = 11021,

		[ParentCombo(PLD_AoE_DPS)]
		[CustomComboInfo("Expiacion", "", PLD.JobID)]
		PLD_AoE_SpiritsWithin = 11022,

		[ParentCombo(PLD_AoE_DPS)]
		[CustomComboInfo("Circle of Scorn", "", PLD.JobID)]
		PLD_AoE_CircleOfScorn = 11023,

		[ParentCombo(PLD_AoE_DPS)]
		[CustomComboInfo("Requisecat / Imperator > Blade of Honor", "", PLD.JobID)]
		PLD_AoE_Imperator = 11024,

		[ParentCombo(PLD_AoE_DPS)]
		[CustomComboInfo("Confiteor > Blade of Faith/Truth/Valor", "", PLD.JobID)]
		PLD_AoE_Confiteor = 11025,

		[ParentCombo(PLD_AoE_DPS)]
		[CustomComboInfo("Holy Circle", "", PLD.JobID)]
		PLD_AoE_HolyCircle = 11026,

		[ParentCombo(PLD_AoE_DPS)]
		[CustomComboInfo("Sheltron", "", PLD.JobID)]
		PLD_AoE_Sheltron = 11027,

		[ParentCombo(PLD_AoE_DPS)]
		[CustomComboInfo("Intervention", "", PLD.JobID)]
		PLD_AoE_Intervention = 11028,

		[ParentCombo(PLD_AoE_DPS)]
		[CustomComboInfo("Hallowed Ground", "", PLD.JobID)]
		PLD_AoE_Invuln = 11029,

		#endregion

		#region Utility

		[ReplaceSkill(PLD.Requiescat, PLD.Imperator)]
		[CustomComboInfo("Requiescat / Imperator > Confiteor > Faith > Truth > Valor > Honor", "", PLD.JobID)]
		PLD_Blades = 11040,

		[ReplaceSkill(PLD.SpiritsWithin, PLD.Expiacion, PLD.CircleOfScorn)]
		[CustomComboInfo("Spirits Within & Circle of Scorn", "", PLD.JobID)]
		PLD_ExpiScorn = 11041,

		#endregion

		#region Variant



		#endregion

		#endregion

		#region WARRIOR - 18000

		#region Single Target DPS

		[ReplaceSkill(WAR.HeavySwing, WAR.Maim, WAR.StormsPath, WAR.StormsEye)]
		[CustomComboInfo("Single Target DPS", "", WAR.JobID)]
		WAR_ST_DPS = 18000,

		[ParentCombo(WAR_ST_DPS)]
		[CustomComboInfo("Storm's Eye", "", WAR.JobID)]
		WAR_ST_StormsEye = 18001,

		[ParentCombo(WAR_ST_DPS)]
		[CustomComboInfo("Upheaval", "", WAR.JobID)]
		WAR_ST_Upheaval = 18002,

		[ParentCombo(WAR_ST_DPS)]
		[CustomComboInfo("Onslaught", "", WAR.JobID)]
		WAR_ST_Onslaught = 18003,

		[ParentCombo(WAR_ST_DPS)]
		[CustomComboInfo("Inner Beast / Fell Cleave & Inner Chaos", "", WAR.JobID)]
		WAR_ST_FellCleave = 18004,

		[ParentCombo(WAR_ST_DPS)]
		[CustomComboInfo("Infuriate", "", WAR.JobID)]
		WAR_ST_Infuriate = 18005,

		[ParentCombo(WAR_ST_DPS)]
		[CustomComboInfo("Inner Release > Primal Wrath", "", WAR.JobID)]
		WAR_ST_InnerRelease = 18006,

		[ParentCombo(WAR_ST_DPS)]
		[CustomComboInfo("Primal Rend > Primal Ruination", "", WAR.JobID)]
		WAR_ST_PrimalRend = 18007,

		[ParentCombo(WAR_ST_DPS)]
		[CustomComboInfo("Holmgang", "", WAR.JobID)]
		WAR_ST_Invuln = 18008,

		#endregion

		#region AoE DPS

		[ReplaceSkill(WAR.Overpower, WAR.MythrilTempest)]
		[CustomComboInfo("AoE DPS", "", WAR.JobID)]
		WAR_AoE_DPS = 18020,

		[ParentCombo(WAR_AoE_DPS)]
		[CustomComboInfo("Orogeny", "", WAR.JobID)]
		WAR_AoE_Orogeny = 18021,

		[ParentCombo(WAR_AoE_DPS)]
		[CustomComboInfo("Steel Cyclone / Decimate & Chaotic Cyclone", "", WAR.JobID)]
		WAR_AoE_Decimate = 18022,

		[ParentCombo(WAR_AoE_DPS)]
		[CustomComboInfo("Infuriate", "", WAR.JobID)]
		WAR_AoE_Infuriate = 18023,

		[ParentCombo(WAR_AoE_DPS)]
		[CustomComboInfo("Berserk / Inner Release > Primal Wrath", "", WAR.JobID)]
		WAR_AoE_InnerRelease = 18024,

		[ParentCombo(WAR_AoE_DPS)]
		[CustomComboInfo("Primal Rend > Primal Ruination", "", WAR.JobID)]
		WAR_AoE_PrimalRend = 18025,

		[ParentCombo(WAR_AoE_DPS)]
		[CustomComboInfo("Holmgang", "", WAR.JobID)]
		WAR_AoE_Invuln = 18026,

		#endregion

		#region Utility



		#endregion

		#region Variant



		#endregion

		#endregion

		#region DARK KNIGHT - 5000

		#region Single Target DPS

		[ReplaceSkill(DRK.HardSlash, DRK.SyphonStrike, DRK.Souleater)]
		[CustomComboInfo("Single Target DPS", "", DRK.JobID)]
		DRK_ST_DPS = 5000,

		[ParentCombo(DRK_ST_DPS)]
		[CustomComboInfo("Edge of Darkness / Edge of Shadow", "", DRK.JobID)]
		DRK_ST_Edge = 5001,

		[ParentCombo(DRK_ST_DPS)]
		[CustomComboInfo("Bloodspiller & Scarlet Delirium > Comeuppance > Torcleaver", "", DRK.JobID)]
		DRK_ST_Bloodspiller = 5002,

		[ParentCombo(DRK_ST_DPS)]
		[CustomComboInfo("Blood Weapon / Delirium", "", DRK.JobID)]
		DRK_ST_Delirium = 5003,

		[ParentCombo(DRK_ST_DPS)]
		[CustomComboInfo("Carve and Spit", "", DRK.JobID)]
		DRK_ST_Carve = 5004,

		[ParentCombo(DRK_ST_DPS)]
		[CustomComboInfo("Living Shadow > Disesteem", "", DRK.JobID)]
		DRK_ST_LivingShadow = 5005,

		[ParentCombo(DRK_ST_DPS)]
		[CustomComboInfo("Salted Earth > Salt and Darkness", "", DRK.JobID)]
		DRK_ST_SaltedEarth = 5006,

		[ParentCombo(DRK_ST_DPS)]
		[CustomComboInfo("Shadowbringer", "", DRK.JobID)]
		DRK_ST_Shadowbringer = 5007,

		[ParentCombo(DRK_ST_DPS)]
		[CustomComboInfo("Oblation", "", DRK.JobID)]
		DRK_ST_Oblation = 5008,

		[ParentCombo(DRK_ST_DPS)]
		[CustomComboInfo("Living Dead", "", DRK.JobID)]
		DRK_ST_Invuln = 5009,

		#endregion

		#region AoE DPS

		[ReplaceSkill(DRK.Unleash, DRK.StalwartSoul)]
		[CustomComboInfo("AoE DPS", "", DRK.JobID)]
		DRK_AoE_DPS = 5020,

		[ParentCombo(DRK_AoE_DPS)]
		[CustomComboInfo("Flood of Darkness / Flood of Shadow", "", DRK.JobID)]
		DRK_AoE_Flood = 5021,

		[ParentCombo(DRK_AoE_DPS)]
		[CustomComboInfo("Quietus & Impalement", "", DRK.JobID)]
		DRK_AoE_Quietus = 5022,

		[ParentCombo(DRK_AoE_DPS)]
		[CustomComboInfo("Blood Weapon / Delirium", "", DRK.JobID)]
		DRK_AoE_Delirium = 5023,

		[ParentCombo(DRK_AoE_DPS)]
		[CustomComboInfo("Abyssal Drain", "", DRK.JobID)]
		DRK_AoE_Abyssal = 5024,

		[ParentCombo(DRK_AoE_DPS)]
		[CustomComboInfo("Living Shadow > Disesteem", "", DRK.JobID)]
		DRK_AoE_LivingShadow = 5025,

		[ParentCombo(DRK_AoE_DPS)]
		[CustomComboInfo("Salted Earth > Salt and Darkness", "", DRK.JobID)]
		DRK_AoE_SaltedEarth = 5026,

		[ParentCombo(DRK_AoE_DPS)]
		[CustomComboInfo("Shadowbringer", "", DRK.JobID)]
		DRK_AoE_Shadowbringer = 5027,

		[ParentCombo(DRK_AoE_DPS)]
		[CustomComboInfo("Oblation", "", DRK.JobID)]
		DRK_AoE_Oblation = 5028,

		[ParentCombo(DRK_AoE_DPS)]
		[CustomComboInfo("Living Dead", "", DRK.JobID)]
		DRK_AoE_Invuln = 5029,

		#endregion

		#region Utility



		#endregion

		#region Variant



		#endregion

		#endregion

		#region GUNBREAKER - 7000

		#region Single Target DPS

		[ReplaceSkill(GNB.KeenEdge, GNB.BrutalShell, GNB.SolidBarrel)]
		[CustomComboInfo("Single Target DPS", "", GNB.JobID)]
		GNB_ST_DPS = 7000,

		[ParentCombo(GNB_ST_DPS)]
		[CustomComboInfo("No Mercy > Sonic Break", "", GNB.JobID)]
		GNB_ST_NoMercy = 7001,

		[ParentCombo(GNB_ST_DPS)]
		[CustomComboInfo("Burst Strike > Hypervelocity", "", GNB.JobID)]
		GNB_ST_Burst = 7002,

		[ParentCombo(GNB_ST_DPS)]
		[CustomComboInfo("Gnashing Fang > Savage Claw > Wicked Talon", "", GNB.JobID)]
		GNB_ST_Gnashing = 7003,

		[ParentCombo(GNB_ST_DPS)]
		[CustomComboInfo("Danger Zone / Blasting Zone", "", GNB.JobID)]
		GNB_ST_BlastingZone = 7004,

		[ParentCombo(GNB_ST_DPS)]
		[CustomComboInfo("Bow Shock", "", GNB.JobID)]
		GNB_ST_BowShock = 7005,

		[ParentCombo(GNB_ST_DPS)]
		[CustomComboInfo("Double Down", "", GNB.JobID)]
		GNB_ST_DoubleDown = 7006,

		[ParentCombo(GNB_ST_DPS)]
		[CustomComboInfo("Bloodfest > Reign of Beasts > Noble Blood > Lion Heart", "", GNB.JobID)]
		GNB_ST_Bloodfest = 7007,

		[ParentCombo(GNB_ST_DPS)]
		[CustomComboInfo("Aurora", "", GNB.JobID)]
		GNB_ST_AutoAurora = 7008,

		[ParentCombo(GNB_ST_DPS)]
		[CustomComboInfo("Superbolide", "", GNB.JobID)]
		GNB_ST_Invuln = 7009,

		#endregion

		#region AoE DPS

		[ReplaceSkill(GNB.DemonSlice, GNB.DemonSlaughter)]
		[CustomComboInfo("AoE DPS", "", GNB.JobID)]
		GNB_AoE_DPS = 7020,

		[ParentCombo(GNB_AoE_DPS)]
		[CustomComboInfo("No Mercy", "", GNB.JobID)]
		GNB_AoE_NoMercy = 7021,

		[ParentCombo(GNB_AoE_DPS)]
		[CustomComboInfo("Fated Circle > Fated Brand", "", GNB.JobID)]
		GNB_AoE_Fated = 7022,

		[ParentCombo(GNB_AoE_DPS)]
		[CustomComboInfo("Double Down", "", GNB.JobID)]
		GNB_AoE_DoubleDown = 7023,

		[ParentCombo(GNB_AoE_DPS)]
		[CustomComboInfo("Bloodfest > Reign of Beasts > Noble Blood > Lion Heart", "", GNB.JobID)]
		GNB_AoE_Bloodfest = 7024,

		[ParentCombo(GNB_AoE_DPS)]
		[CustomComboInfo("Aurora", "", GNB.JobID)]
		GNB_AoE_AutoAurora = 7025,

		[ParentCombo(GNB_AoE_DPS)]
		[CustomComboInfo("Superbolide", "", GNB.JobID)]
		GNB_AoE_Invuln = 7026,

		#endregion

		#region Utility

		[ReplaceSkill(GNB.Aurora)]
		[CustomComboInfo("Aurora Overwrite Protection", "", GNB.JobID)]
		GNB_AuroraProtection = 7040,

		#endregion

		#region Variant



		#endregion

		#endregion

		#endregion

		#region Healers

		#region WHITE MAGE - 19000

		#region Single Target DPS

		[ReplaceSkill(WHM.Stone1, WHM.Stone2, WHM.Stone3, WHM.Stone4, WHM.Glare1, WHM.Glare3)]
		[CustomComboInfo("Single Target DPS", "", WHM.JobID)]
		WHM_ST_DPS = 19000,

		[ParentCombo(WHM_ST_DPS)]
		[CustomComboInfo("Aero / Aero II / Dia", "", WHM.JobID)]
		WHM_ST_DPS_DoT = 19001,

		[ParentCombo(WHM_ST_DPS)]
		[CustomComboInfo("Presence of Mind > Glare IV", "", WHM.JobID)]
		WHM_ST_DPS_PresenceOfMind = 19002,

		[ParentCombo(WHM_ST_DPS)]
		[CustomComboInfo("Assize", "", WHM.JobID)]
		WHM_ST_DPS_Assize = 19003,

		[ParentCombo(WHM_ST_DPS)]
		[CustomComboInfo("Afflatus Misery", "", WHM.JobID)]
		WHM_ST_DPS_Misery = 19004,

		[ParentCombo(WHM_ST_DPS_Misery)]
		[CustomComboInfo("Lily Overcap Protection", "", WHM.JobID)]
		WHM_ST_DPS_LilyOvercap = 19005,

		[ParentCombo(WHM_ST_DPS_Misery)]
		[CustomComboInfo("Save Misery", "", WHM.JobID)]
		WHM_ST_DPS_Misery_Save = 19006,

		[ParentCombo(WHM_ST_DPS)]
		[CustomComboInfo("Lucid Dreaming", "", WHM.JobID)]
		WHM_ST_DPS_Lucid = 19007,

		[ParentCombo(WHM_ST_DPS)]
		[CustomComboInfo("Swiftcast Movement", "", WHM.JobID)]
		WHM_ST_DPS_Swiftcast = 19008,

		#endregion

		#region AoE DPS

		[ReplaceSkill(WHM.Holy, WHM.Holy3)]
		[CustomComboInfo("AoE DPS", "", WHM.JobID)]
		WHM_AoE_DPS = 19020,

		[ParentCombo(WHM_AoE_DPS)]
		[CustomComboInfo("Presence of Mind > Glare IV", "", WHM.JobID)]
		WHM_AoE_DPS_PresenceOfMind = 19021,

		[ParentCombo(WHM_AoE_DPS)]
		[CustomComboInfo("Assize", "", WHM.JobID)]
		WHM_AoE_DPS_Assize = 19022,

		[ParentCombo(WHM_AoE_DPS)]
		[CustomComboInfo("Afflatus Misery", "", WHM.JobID)]
		WHM_AoE_DPS_Misery = 19023,

		[ParentCombo(WHM_AoE_DPS_Misery)]
		[CustomComboInfo("Lily Overcap Protection", "", WHM.JobID)]
		WHM_AoE_DPS_LilyOvercap = 19024,

		[ParentCombo(WHM_AoE_DPS_Misery)]
		[CustomComboInfo("Save Misery", "", WHM.JobID)]
		WHM_AoE_DPS_Misery_Save = 19025,

		[ParentCombo(WHM_AoE_DPS)]
		[CustomComboInfo("Lucid Dreaming", "", WHM.JobID)]
		WHM_AoE_DPS_Lucid = 19026,

		[ParentCombo(WHM_AoE_DPS)]
		[CustomComboInfo("Swiftcast Movement", "", WHM.JobID)]
		WHM_AoE_DPS_Swiftcast = 19027,

		#endregion

		#region Single Target Heals

		[ReplaceSkill(WHM.Cure1, WHM.Cure2)]
		[CustomComboInfo("Single Target Heals", "", WHM.JobID)]
		WHM_ST_Heals = 19030,

		[ParentCombo(WHM_ST_Heals)]
		[CustomComboInfo("Tetragrammaton", "", WHM.JobID)]
		WHM_ST_Heals_Tetragrammaton = 19031,

		[ParentCombo(WHM_ST_Heals)]
		[CustomComboInfo("Afflatus Solace", "", WHM.JobID)]
		WHM_ST_Heals_Solace = 19032,

		[ParentCombo(WHM_ST_Heals)]
		[CustomComboInfo("Afflatus Misery", "", WHM.JobID)]
		WHM_ST_Heals_Misery = 19033,

		[ParentCombo(WHM_ST_Heals)]
		[CustomComboInfo("Regen", "", WHM.JobID)]
		WHM_ST_Heals_Regen = 19034,

		[ParentCombo(WHM_ST_Heals)]
		[CustomComboInfo("Thin Air", "", WHM.JobID)]
		WHM_ST_Heals_ThinAir = 19035,

		#endregion

		#region AoE Heals

		[ReplaceSkill(WHM.Medica1, WHM.Medica2, WHM.Medica3)]
		[CustomComboInfo("AoE Heals", "", WHM.JobID)]
		WHM_AoE_Heals = 19050,

		[ParentCombo(WHM_AoE_Heals)]
		[CustomComboInfo("Plenary Indulgence", "", WHM.JobID)]
		WHM_AoE_Heals_Plenary = 19051,

		[ParentCombo(WHM_AoE_Heals)]
		[CustomComboInfo("Afflatus Rapture", "", WHM.JobID)]
		WHM_AoE_Heals_Rapture = 19052,

		[ParentCombo(WHM_AoE_Heals)]
		[CustomComboInfo("Afflatus Misery", "", WHM.JobID)]
		WHM_AoE_Heals_Misery = 19053,

		[ParentCombo(WHM_AoE_Heals)]
		[CustomComboInfo("Thin Air", "", WHM.JobID)]
		WHM_AoE_Heals_ThinAir = 19054,

		[ParentCombo(WHM_AoE_Heals)]
		[CustomComboInfo("Medica II / Medica III", "", WHM.JobID)]
		WHM_AoEHeals_Medica2 = 19055,

		[ParentCombo(WHM_AoE_Heals)]
		[CustomComboInfo("Cure III", "", WHM.JobID)]
		WHM_AoE_Heals_Cure3 = 19056,

		#endregion

		#region Utility

		[ReplaceSkill(WHM.Raise)]
		[CustomComboInfo("Swiftcast > Raise", "", WHM.JobID)]
		WHM_Raise = 19073,

		#endregion

		#region Variant



		#endregion

		#endregion

		#region SCHOLAR - 16000

		#region Single Target DPS

		[ReplaceSkill(SCH.Ruin, SCH.Broil, SCH.Broil2, SCH.Broil3, SCH.Broil4)]
		[CustomComboInfo("Single Target DPS", "", SCH.JobID)]
		SCH_ST_DPS = 16000,

		[ParentCombo(SCH_ST_DPS)]
		[CustomComboInfo("Bio / Bio II / Biolysis", "", SCH.JobID)]
		SCH_ST_DPS_Bio = 16001,

		[ParentCombo(SCH_ST_DPS)]
		[CustomComboInfo("Aetherflow", "", SCH.JobID)]
		SCH_ST_DPS_Aetherflow = 16002,

		[ParentCombo(SCH_ST_DPS)]
		[CustomComboInfo("Energy Drain", "", SCH.JobID)]
		SCH_ST_DPS_EnergyDrain = 16003,

		[ParentCombo(SCH_ST_DPS)]
		[CustomComboInfo("Dissipation", "", SCH.JobID)]
		SCH_ST_DPS_Dissipation = 16004,

		[ParentCombo(SCH_ST_DPS)]
		[CustomComboInfo("Consolation", "", SCH.JobID)]
		SCH_ST_DPS_Seraph = 16005,

		[ParentCombo(SCH_ST_DPS)]
		[CustomComboInfo("Chain Stratagem > Baneful Impact", "", SCH.JobID)]
		SCH_ST_DPS_ChainStrat = 16006,

		[ParentCombo(SCH_ST_DPS)]
		[CustomComboInfo("Ruin II Movement", "", SCH.JobID)]
		SCH_ST_DPS_Ruin2Movement = 16007,

		[ParentCombo(SCH_ST_DPS)]
		[CustomComboInfo("Fairy Reminder", "", SCH.JobID)]
		SCH_ST_DPS_Fairy = 16008,

		[ParentCombo(SCH_ST_DPS)]
		[CustomComboInfo("Lucid Dreaming", "", SCH.JobID)]
		SCH_ST_DPS_Lucid = 16009,

		#endregion

		#region AoE DPS

		[ReplaceSkill(SCH.ArtOfWar, SCH.ArtOfWarII)]
		[CustomComboInfo("AoE DPS", "", SCH.JobID)]
		SCH_AoE_DPS = 16020,

		[ParentCombo(SCH_AoE_DPS)]
		[CustomComboInfo("Aetherflow", "", SCH.JobID)]
		SCH_AoE_Aetherflow = 16021,

		[ParentCombo(SCH_AoE_DPS)]
		[CustomComboInfo("Energy Drain", "", SCH.JobID)]
		SCH_AoE_DPS_EnergyDrain = 16022,

		[ParentCombo(SCH_AoE_DPS)]
		[CustomComboInfo("Fairy Reminder", "", SCH.JobID)]
		SCH_AoE_DPS_Fairy = 16023,

		[ParentCombo(SCH_AoE_DPS)]
		[CustomComboInfo("Lucid Dreaming", "", SCH.JobID)]
		SCH_AoE_DPS_Lucid = 16024,

		#endregion

		#region Utility

		[ReplaceSkill(SCH.Resurrection)]
		[CustomComboInfo("Swiftcast > Raise", "", SCH.JobID)]
		SCH_Raise = 16040,

		[ReplaceSkill(SCH.Lustrate)]
		[CustomComboInfo("Lustrate > Excogitation", "", SCH.JobID)]
		SCH_Lustrate = 16041,

		#endregion

		#region Variant



		#endregion

		#endregion

		#region ASTROLOGIAN - 1000

		#region Single Target DPS

		[ReplaceSkill(AST.Malefic, AST.Malefic2, AST.Malefic3, AST.Malefic4, AST.FallMalefic, AST.Combust, AST.Combust2, AST.Combust3, AST.Gravity, AST.Gravity2)]
		[CustomComboInfo("Single Target DPS", "", AST.JobID)]
		AST_ST_DPS = 1000,

		[ParentCombo(AST_ST_DPS)]
		[CustomComboInfo("Combust / Combust II / Combust III", "", AST.JobID)]
		AST_ST_DPS_CombustUptime = 1001,

		[ParentCombo(AST_ST_DPS)]
		[CustomComboInfo("Astral Draw & Umbral Draw", "", AST.JobID)]
		AST_ST_DPS_AutoDraw = 1002,

		[ParentCombo(AST_ST_DPS)]
		[CustomComboInfo("Lightspeed", "", AST.JobID)]
		AST_ST_DPS_Lightspeed = 1003,

		[ParentCombo(AST_ST_DPS)]
		[CustomComboInfo("Divination > Oracle", "", AST.JobID)]
		AST_ST_DPS_Divination = 1004,

		[ParentCombo(AST_ST_DPS)]
		[CustomComboInfo("Sun Sign", "", AST.JobID)]
		AST_ST_DPS_SunSign = 1005,

		[ParentCombo(AST_ST_DPS)]
		[CustomComboInfo("Lucid Dreaming", "", AST.JobID)]
		AST_ST_DPS_Lucid = 1006,

		[ParentCombo(AST_ST_DPS)]
		[CustomComboInfo("Swiftcast Movement", "", AST.JobID)]
		AST_ST_DPS_Swiftcast = 1007,

		#endregion

		#region AoE DPS

		[ReplaceSkill(AST.Gravity, AST.Gravity2)]
		[CustomComboInfo("AoE DPS", "", AST.JobID)]
		AST_AoE_DPS = 1020,

		[ParentCombo(AST_AoE_DPS)]
		[CustomComboInfo("Astral Draw & Umbral Draw", "", AST.JobID)]
		AST_AoE_DPS_AutoDraw = 1021,

		[ParentCombo(AST_AoE_DPS)]
		[CustomComboInfo("Lightspeed", "", AST.JobID)]
		AST_AoE_DPS_Lightspeed = 1022,

		[ParentCombo(AST_AoE_DPS)]
		[CustomComboInfo("Divination > Oracle", "", AST.JobID)]
		AST_AoE_DPS_Divination = 1023,

		[ParentCombo(AST_AoE_DPS)]
		[CustomComboInfo("Sun Sign", "", AST.JobID)]
		AST_AoE_DPS_SunSign = 1024,

		[ParentCombo(AST_AoE_DPS)]
		[CustomComboInfo("Lucid Dreaming", "", AST.JobID)]
		AST_AoE_DPS_Lucid = 1025,

		[ParentCombo(AST_AoE_DPS)]
		[CustomComboInfo("Swiftcast Movement", "", AST.JobID)]
		AST_AoE_DPS_Swiftcast = 1026,

		#endregion

		#region Single Target Heals

		[ReplaceSkill(AST.Benefic1, AST.Benefic2)]
		[CustomComboInfo("Single Target Heals", "", AST.JobID)]
		AST_ST_Heals = 1040,

		[ParentCombo(AST_ST_Heals)]
		[CustomComboInfo("Aspected Benefic", "", AST.JobID)]
		AST_ST_Heals_AspectedBenefic = 1041,

		#endregion

		#region Utility

		[ReplaceSkill(AST.Ascend)]
		[CustomComboInfo("Swiftcast > Raise", "", AST.JobID)]
		AST_Raise_Alternative = 1060,

		[ReplaceSkill(AST.Lightspeed)]
		[CustomComboInfo("Lightspeed Overwrite Protection", "", AST.JobID)]
		AST_Lightspeed_Protection = 1061,

		[ReplaceSkill(AST.Play1, AST.Play2, AST.Play3)]
		[CustomComboInfo("Astral/Umbral Draw Cooldown on Play I/II/III", "", AST.JobID)]
		AST_DrawCooldown = 1062,

		#endregion

		#region Variant



		#endregion

		#endregion

		#region SAGE - 14000

		#region Single Target DPS

		[ReplaceSkill(SGE.Dosis, SGE.Dosis2, SGE.Dosis3)]
		[CustomComboInfo("Single Target DPS", "", SGE.JobID)]
		SGE_ST_DPS = 14000,

		[ParentCombo(SGE_ST_DPS)]
		[CustomComboInfo("Eukrasian Dosis / Eukrasian Dosis II / Eukrasian Dosis III", "", SGE.JobID)]
		SGE_ST_DPS_EDosis = 14001,

		[ParentCombo(SGE_ST_DPS)]
		[CustomComboInfo("Phlegma / Phlegma II / Phlegma III", "", SGE.JobID)]
		SGE_ST_DPS_Phlegma = 14002,

		[ParentCombo(SGE_ST_DPS)]
		[CustomComboInfo("Toxikon / Toxikon II", "", SGE.JobID)]
		SGE_ST_DPS_Toxikon = 14003,

		[ParentCombo(SGE_ST_DPS)]
		[CustomComboInfo("Psych", "", SGE.JobID)]
		SGE_ST_DPS_Psyche = 14004,

		[ParentCombo(SGE_ST_DPS)]
		[CustomComboInfo("Rhizomata", "", SGE.JobID)]
		SGE_ST_DPS_Rhizo = 14005,

		[ParentCombo(SGE_ST_DPS)]
		[CustomComboInfo("Addersgall Overflow Protection", "", SGE.JobID)]
		SGE_ST_DPS_AddersgallProtect = 14006,

		[ParentCombo(SGE_ST_DPS)]
		[CustomComboInfo("Soteria", "", SGE.JobID)]
		SGE_ST_DPS_Soteria = 14007,

		[ParentCombo(SGE_ST_DPS)]
		[CustomComboInfo("Kardia", "", SGE.JobID)]
		SGE_ST_DPS_Kardia = 14008,

		[ParentCombo(SGE_ST_DPS)]
		[CustomComboInfo("Lucid Dreaming", "", SGE.JobID)]
		SGE_ST_DPS_Lucid = 14009,

		[ParentCombo(SGE_ST_DPS)]
		[CustomComboInfo("Swiftcast Movement", "", SGE.JobID)]
		SGE_ST_DPS_Swiftcast = 14010,

		#endregion

		#region AoE DPS

		[ReplaceSkill(SGE.Dyskrasia, SGE.Dyskrasia2)]
		[CustomComboInfo("AoE DPS", "", SGE.JobID)]
		SGE_AoE_DPS = 14020,

		[ParentCombo(SGE_AoE_DPS)]
		[CustomComboInfo("Eukrasian Dyskrasia", "", SGE.JobID)]
		SGE_AoE_DPS_EDyskrasia = 14021,

		[ParentCombo(SGE_AoE_DPS)]
		[CustomComboInfo("Phlegma / Phlegma II / Phlegma III", "", SGE.JobID)]
		SGE_AoE_DPS_Phlegma = 14022,

		[ParentCombo(SGE_AoE_DPS)]
		[CustomComboInfo("Psyche", "", SGE.JobID)]
		SGE_AoE_DPS_Psyche = 14023,

		[ParentCombo(SGE_AoE_DPS)]
		[CustomComboInfo("Rhizomata", "", SGE.JobID)]
		SGE_AoE_DPS_Rhizo = 14024,

		[ParentCombo(SGE_AoE_DPS)]
		[CustomComboInfo("Addersgall Overflow Protection", "", SGE.JobID)]
		SGE_AoE_DPS_AddersgallProtect = 14025,

		[ParentCombo(SGE_AoE_DPS)]
		[CustomComboInfo("Soteria", "", SGE.JobID)]
		SGE_AoE_DPS_Soteria = 14026,

		[ParentCombo(SGE_AoE_DPS)]
		[CustomComboInfo("Kardia", "", SGE.JobID)]
		SGE_AoE_DPS_Kardia = 14027,

		[ParentCombo(SGE_AoE_DPS)]
		[CustomComboInfo("Lucid Dreaming", "", SGE.JobID)]
		SGE_AoE_DPS_Lucid = 14028,

		#endregion

		#region Single Target Heals

		[ReplaceSkill(SGE.Diagnosis, SGE.EukrasianDiagnosis)]
		[CustomComboInfo("Single Target Heals", "", SGE.JobID)]
		SGE_ST_Heals = 14040,

		[ParentCombo(SGE_ST_Heals)]
		[CustomComboInfo("Haima", "", SGE.JobID)]
		SGE_ST_Heals_Haima = 14041,

		[ParentCombo(SGE_ST_Heals)]
		[CustomComboInfo("Krasis", "", SGE.JobID)]
		SGE_ST_Heals_Krasis = 14042,

		#endregion

		#region AoE Heals

		[ReplaceSkill(SGE.Prognosis, SGE.EukrasianPrognosis)]
		[CustomComboInfo("AoE Heals", "", SGE.JobID)]
		SGE_AoE_Heals = 14060,

		[ParentCombo(SGE_AoE_Heals)]
		[CustomComboInfo("Pepsis", "", SGE.JobID)]
		SGE_AoE_Heals_Pepsis = 14061,

		#endregion

		#region Utility

		[ReplaceSkill(SGE.Egeiro)]
		[CustomComboInfo("Swiftcast > Raise", "", SGE.JobID)]
		SGE_Raise = 14080,

		[ReplaceSkill(SGE.Kerachole, SGE.Panhaima, SGE.Philosophia)]
		[CustomComboInfo("Spell Overlap Protection", "", SGE.JobID)]
		SGE_OverProtect = 14083,

		#endregion

		#region Variant



		#endregion

		#endregion

		#endregion

		#region Melee DPS

		#region MONK - 9000

		#region Single Target DPS

		[ReplaceSkill(MNK.Bootshine, MNK.LeapingOpo, MNK.TrueStrike, MNK.RisingRaptor, MNK.SnapPunch, MNK.PouncingCoeurl, MNK.DragonKick, MNK.TwinSnakes, MNK.Demolish)]
		[CustomComboInfo("Single Target DPS", "", MNK.JobID)]
		MNK_ST_DPS = 9000,

		[ParentCombo(MNK_ST_DPS)]
		[CustomComboInfo("Steeled Meditation / Forbidden Meditation > Steel Peak / The Forbidden Chakra", "", MNK.JobID)]
		MNK_ST_Meditation = 9001,

		[ParentCombo(MNK_ST_DPS)]
		[CustomComboInfo("Perfect Balance", "", MNK.JobID)]
		MNK_ST_PerfectBalance = 9002,

		[ParentCombo(MNK_ST_DPS)]
		[CustomComboInfo("Masterful Blitz", "", MNK.JobID)]
		MNK_ST_Blitz = 9003,

		[ParentCombo(MNK_ST_DPS)]
		[CustomComboInfo("Six-sided Star", "", MNK.JobID)]
		MNK_ST_SixStar = 9004,

		[ParentCombo(MNK_ST_DPS)]
		[CustomComboInfo("Riddle of Fire > Fire's Reply", "", MNK.JobID)]
		MNK_ST_Fire = 9005,

		[ParentCombo(MNK_ST_DPS)]
		[CustomComboInfo("Riddle of Wind > Wind's Reply", "", MNK.JobID)]
		MNK_ST_Wind = 9007,

		[ParentCombo(MNK_ST_DPS)]
		[CustomComboInfo("Brotherhood", "", MNK.JobID)]
		MNK_ST_Brother = 9006,

		#endregion

		#region AoE DPS

		[ReplaceSkill(MNK.ArmOfTheDestroyer, MNK.ShadowOfTheDestroyer, MNK.FourPointFury, MNK.Rockbreaker)]
		[CustomComboInfo("AoE DPS", "", MNK.JobID)]
		MNK_AoE_DPS = 9020,

		[ParentCombo(MNK_AoE_DPS)]
		[CustomComboInfo("Inspirited Meditation / Enlightened Meditation > Howling Fist / Enlightenment", "", MNK.JobID)]
		MNK_AoE_Meditation = 9021,

		[ParentCombo(MNK_AoE_DPS)]
		[CustomComboInfo("Perfect Balance", "", MNK.JobID)]
		MNK_AoE_PerfectBalance = 9022,

		[ParentCombo(MNK_AoE_DPS)]
		[CustomComboInfo("Masterful Blitz", "", MNK.JobID)]
		MNK_AoE_Blitz = 9023,

		[ParentCombo(MNK_AoE_DPS)]
		[CustomComboInfo("Riddle of Fire > Fire's Reply", "", MNK.JobID)]
		MNK_AoE_Fire = 9024,

		[ParentCombo(MNK_AoE_DPS)]
		[CustomComboInfo("Riddle of Wind > Wind's Reply", "", MNK.JobID)]
		MNK_AoE_Wind = 9026,

		[ParentCombo(MNK_AoE_DPS)]
		[CustomComboInfo("Brotherhood", "", MNK.JobID)]
		MNK_AoE_Brother = 9025,

		#endregion

		#region Utility



		#endregion

		#region Variant



		#endregion

		#endregion

		#region DRAGOON - 6000

		#region Single Target

		[ReplaceSkill(DRG.TrueThrust, DRG.RaidenThrust, DRG.VorpalThrust, DRG.LanceBarrage, DRG.Disembowel, DRG.SpiralBlow, DRG.FullThrust,
			DRG.HeavensThrust, DRG.ChaosThrust, DRG.ChaoticSpring, DRG.FangAndClaw, DRG.WheelingThrust)]
		[CustomComboInfo("Single Target DPS", "", DRG.JobID)]
		DRG_ST_DPS = 6000,

		[ParentCombo(DRG_ST_DPS)]
		[CustomComboInfo("Life Surge", "", DRG.JobID)]
		DRG_ST_LifeSurge = 6001,

		[ParentCombo(DRG_ST_DPS)]
		[CustomComboInfo("Lance Charge", "", DRG.JobID)]
		DRG_ST_LanceCharge = 6002,

		[ParentCombo(DRG_ST_DPS)]
		[CustomComboInfo("Geirskogul > Nastrond", "", DRG.JobID)]
		DRG_ST_Geirskogul = 6003,

		[ParentCombo(DRG_ST_DPS)]
		[CustomComboInfo("Wyrmwind Thrust", "", DRG.JobID)]
		DRG_ST_Wyrmwind = 6004,

		[ParentCombo(DRG_ST_DPS)]
		[CustomComboInfo("Jump / High Jump > Mirage Dive", "", DRG.JobID)]
		DRG_ST_HighJump = 6005,

		[ParentCombo(DRG_ST_DPS)]
		[CustomComboInfo("Dragonfire Dive > Rise of the Dragon", "", DRG.JobID)]
		DRG_ST_Dragonfire = 6006,

		[ParentCombo(DRG_ST_DPS)]
		[CustomComboInfo("Stardiver > Starcross", "", DRG.JobID)]
		DRG_ST_Stardiver = 6007,

		[ParentCombo(DRG_ST_DPS)]
		[CustomComboInfo("Battle Litany", "", DRG.JobID)]
		DRG_ST_BattleLitany = 6008,

		#endregion

		#region AoE DPS

		[ReplaceSkill(DRG.DoomSpike, DRG.DraconianFury, DRG.SonicThrust, DRG.CoerthanTorment)]
		[CustomComboInfo("AoE DPS", "", DRG.JobID)]
		DRG_AoE_DPS = 6020,

		[ParentCombo(DRG_AoE_DPS)]
		[CustomComboInfo("Life Surge", "", DRG.JobID)]
		DRG_AoE_LifeSurge = 6021,

		[ParentCombo(DRG_AoE_DPS)]
		[CustomComboInfo("Lance Charge", "", DRG.JobID)]
		DRG_AoE_LanceCharge = 6022,

		[ParentCombo(DRG_AoE_DPS)]
		[CustomComboInfo("Geirskogul > Nastrond", "", DRG.JobID)]
		DRG_AoE_Geirskogul = 6023,

		[ParentCombo(DRG_AoE_DPS)]
		[CustomComboInfo("Wyrmwind Thrust", "", DRG.JobID)]
		DRG_AoE_Wyrmwind = 6024,

		[ParentCombo(DRG_AoE_DPS)]
		[CustomComboInfo("Stardiver > Starcross", "", DRG.JobID)]
		DRG_AoE_Stardiver = 6025,

		[ParentCombo(DRG_AoE_DPS)]
		[CustomComboInfo("Dragonfire Dive > Rise of the Dragon", "", DRG.JobID)]
		DRG_AoE_Dragonfire = 6026,

		[ParentCombo(DRG_AoE_DPS)]
		[CustomComboInfo("Battle Litany", "", DRG.JobID)]
		DRG_AoE_BattleLitany = 6027,

		#endregion

		#region Utility



		#endregion

		#region Variant



		#endregion

		#endregion

		#region NINJA - 10000

		#region Singlet Target DPS

		[ReplaceSkill(NIN.SpinningEdge, NIN.GustSlash, NIN.AeolianEdge, NIN.ArmorCrush)]
		[CustomComboInfo("Single Target DPS", "", NIN.JobID)]
		NIN_ST_DPS = 10000,

		[ParentCombo(NIN_ST_DPS)]
		[CustomComboInfo("Mudras", "", NIN.JobID)]
		NIN_ST_Mudras = 10001,

		[ParentCombo(NIN_ST_DPS)]
		[CustomComboInfo("Trick Attack / Kunai's Bane", "", NIN.JobID)]
		NIN_ST_Trick = 10002,

		[ParentCombo(NIN_ST_DPS)]
		[CustomComboInfo("Mug / Dokumori", "", NIN.JobID)]
		NIN_ST_Mug = 10003,

		[ParentCombo(NIN_ST_DPS)]
		[CustomComboInfo("Assassinate / Dream Within a Dream", "", NIN.JobID)]
		NIN_ST_Assassinate = 10004,

		[ParentCombo(NIN_ST_DPS)]
		[CustomComboInfo("Bhavacakra & Zesho Meppo", "", NIN.JobID)]
		NIN_ST_Bhav = 10005,

		[ParentCombo(NIN_ST_DPS)]
		[CustomComboInfo("Fleeting Raiju & Forked Raiju", "", NIN.JobID)]
		NIN_ST_Raiju = 10010,

		[ParentCombo(NIN_ST_DPS)]
		[CustomComboInfo("Kassatsu", "", NIN.JobID)]
		NIN_ST_Kassatsu = 10006,

		[ParentCombo(NIN_ST_DPS)]
		[CustomComboInfo("Bunshin > Phantom Kamaitachi", "", NIN.JobID)]
		NIN_ST_Bunshin = 10007,

		[ParentCombo(NIN_ST_DPS)]
		[CustomComboInfo("Ten Chi Jin > Tenri Jindo", "", NIN.JobID)]
		NIN_ST_TenChiJin = 10008,

		[ParentCombo(NIN_ST_DPS)]
		[CustomComboInfo("Meisui", "", NIN.JobID)]
		NIN_ST_Meisui = 10009,

		#endregion

		#region AoE DPS

		[ReplaceSkill(NIN.DeathBlossom, NIN.HakkeMujinsatsu)]
		[CustomComboInfo("AoE DPS", "", NIN.JobID)]
		NIN_AoE_DPS = 10020,

		[ParentCombo(NIN_AoE_DPS)]
		[CustomComboInfo("Mudras", "", NIN.JobID)]
		NIN_AoE_Mudras = 10021,

		[ParentCombo(NIN_AoE_DPS)]
		[CustomComboInfo("Trick Attack / Kunai's Bane", "", NIN.JobID)]
		NIN_AoE_Trick = 10022,

		[ParentCombo(NIN_AoE_DPS)]
		[CustomComboInfo("Assassinate / Dream Within a Dream", "", NIN.JobID)]
		NIN_AoE_Assassinate = 10023,

		[ParentCombo(NIN_AoE_DPS)]
		[CustomComboInfo("Hellfrog Medium & Deathfrog Medium", "", NIN.JobID)]
		NIN_AoE_Hellfrog = 10024,

		[ParentCombo(NIN_AoE_DPS)]
		[CustomComboInfo("Kassatsu", "", NIN.JobID)]
		NIN_AoE_Kassatsu = 10025,

		[ParentCombo(NIN_AoE_DPS)]
		[CustomComboInfo("Bunshin > Phantom Kamaitachi", "", NIN.JobID)]
		NIN_AoE_Bunshin = 10026,

		[ParentCombo(NIN_AoE_DPS)]
		[CustomComboInfo("Ten Chi Jin > Tenri Jindo", "", NIN.JobID)]
		NIN_AoE_TenChiJin = 10027,

		[ParentCombo(NIN_AoE_DPS)]
		[CustomComboInfo("Meisui", "", NIN.JobID)]
		NIN_AoE_Meisui = 10028,

		#endregion

		#region Utility



		#endregion

		#region Variant



		#endregion

		#endregion

		#region SAMURAI - 15000

		#region Single Target DPS

		[ReplaceSkill(SAM.Hakaze, SAM.Gyofu, SAM.Jinpu, SAM.Gekko, SAM.Shifu, SAM.Kasha, SAM.Yukikaze)]
		[CustomComboInfo("Single Target DPS", "", SAM.JobID)]
		SAM_ST_DPS = 15000,

		[ParentCombo(SAM_ST_DPS)]
		[CustomComboInfo("Higanbana", "", SAM.JobID)]
		SAM_ST_Higanbana = 15001,

		[ParentCombo(SAM_ST_DPS)]
		[CustomComboInfo("Iaijutsu", "", SAM.JobID)]
		SAM_ST_Iaijutsu = 15002,

		[ParentCombo(SAM_ST_DPS)]
		[CustomComboInfo("Tsubame Gaeshi", "", SAM.JobID)]
		SAM_ST_Tsubame = 15003,

		[ParentCombo(SAM_ST_DPS)]
		[CustomComboInfo("Shinten", "", SAM.JobID)]
		SAM_ST_Shinten = 15004,

		[ParentCombo(SAM_ST_DPS)]
		[CustomComboInfo("Senei", "", SAM.JobID)]
		SAM_ST_Senei = 15005,

		[ParentCombo(SAM_ST_DPS)]
		[CustomComboInfo("Shoha", "", SAM.JobID)]
		SAM_ST_Shoha = 15006,

		[ParentCombo(SAM_ST_DPS)]
		[CustomComboInfo("Meikyo Shisui", "", SAM.JobID)]
		SAM_ST_Meikyo = 15007,

		[ParentCombo(SAM_ST_DPS)]
		[CustomComboInfo("Ikishoten > Zanshin", "", SAM.JobID)]
		SAM_ST_Ikishoten = 15008,

		[ParentCombo(SAM_ST_DPS)]
		[CustomComboInfo("Ogi Namikiri > Kaeshi Namikiri", "", SAM.JobID)]
		SAM_ST_Kaeshi = 15009,

		#endregion

		#region AoE DPS

		[ReplaceSkill(SAM.Fuga, SAM.Fuko, SAM.Mangetsu, SAM.Oka)]
		[CustomComboInfo("AoE DPS", "", SAM.JobID)]
		SAM_AoE_DPS = 15020,

		[ParentCombo(SAM_AoE_DPS)]
		[CustomComboInfo("Iaijutsu", "", SAM.JobID)]
		SAM_AoE_Iaijutsu = 15021,

		[ParentCombo(SAM_AoE_DPS)]
		[CustomComboInfo("Tsubame Gaeshi", "", SAM.JobID)]
		SAM_AoE_Tsubame = 15022,

		[ParentCombo(SAM_AoE_DPS)]
		[CustomComboInfo("Kyuten", "", SAM.JobID)]
		SAM_AoE_Kyuten = 15023,

		[ParentCombo(SAM_AoE_DPS)]
		[CustomComboInfo("Guren", "", SAM.JobID)]
		SAM_AoE_Guren = 15024,

		[ParentCombo(SAM_AoE_DPS)]
		[CustomComboInfo("Shoha", "", SAM.JobID)]
		SAM_AoE_Shoha = 15025,

		[ParentCombo(SAM_AoE_DPS)]
		[CustomComboInfo("Meikyo Shisui", "", SAM.JobID)]
		SAM_AoE_Meikyo = 15026,

		[ParentCombo(SAM_AoE_DPS)]
		[CustomComboInfo("Ikishoten > Zanshin", "", SAM.JobID)]
		SAM_AoE_Ikishoten = 15027,

		[ParentCombo(SAM_AoE_DPS)]
		[CustomComboInfo("Ogi Namikiri > Kaeshi Namikiri", "", SAM.JobID)]
		SAM_AoE_Kaeshi = 15028,

		#endregion

		#region Utility



		#endregion

		#region Variant



		#endregion

		#endregion

		#region REAPER - 12000

		#region Single Target DPS

		[ReplaceSkill(RPR.Slice, RPR.WaxingSlice, RPR.InfernalSlice)]
		[CustomComboInfo("Single Target DPS", "", RPR.JobID)]
		RPR_ST_DPS = 12000,

		[ParentCombo(RPR_ST_DPS)]
		[CustomComboInfo("Shadow of Death", "", RPR.JobID)]
		RPR_ST_ShadowOfDeath = 12001,

		[ParentCombo(RPR_ST_DPS)]
		[CustomComboInfo("Soul Slice", "", RPR.JobID)]
		RPR_ST_SoulSlice = 12002,

		[ParentCombo(RPR_ST_DPS)]
		[CustomComboInfo("Blood Stalk", "", RPR.JobID)]
		RPR_ST_BloodStalk = 12003,

		[ParentCombo(RPR_ST_DPS)]
		[CustomComboInfo("Gluttony", "", RPR.JobID)]
		RPR_ST_Gluttony = 12004,

		[ParentCombo(RPR_ST_DPS)]
		[CustomComboInfo("Gibbet & Gallows", "", RPR.JobID)]
		RPR_ST_GibbetGallows = 12005,

		[ParentCombo(RPR_ST_DPS)]
		[CustomComboInfo("Arcane Circle", "", RPR.JobID)]
		RPR_ST_Arcane = 12006,

		[ParentCombo(RPR_ST_DPS)]
		[CustomComboInfo("Plentiful Harvest > Perfectio", "", RPR.JobID)]
		RPR_ST_PlentifulHarvest = 12007,

		[ParentCombo(RPR_ST_DPS)]
		[CustomComboInfo("Enshroud > Cross Reaping > Void Reaping > Lemure's Slice > Communio > Sacrificium", "", RPR.JobID)]
		RPR_ST_Enshroud = 12008,

		[ParentCombo(RPR_ST_DPS)]
		[CustomComboInfo("Soulsow", "", RPR.JobID)]
		RPR_ST_Soulsow = 12009,

		#endregion

		#region AoE DPS

		[ReplaceSkill(RPR.SpinningScythe, RPR.NightmareScythe)]
		[CustomComboInfo("AoE DPS", "", RPR.JobID)]
		RPR_AoE_DPS = 12020,

		[ParentCombo(RPR_AoE_DPS)]
		[CustomComboInfo("Whorl of Death", "", RPR.JobID)]
		RPR_AoE_WhorlOfDeath = 12021,

		[ParentCombo(RPR_AoE_DPS)]
		[CustomComboInfo("Soul Scythe", "", RPR.JobID)]
		RPR_AoE_SoulScythe = 12022,

		[ParentCombo(RPR_AoE_DPS)]
		[CustomComboInfo("Grim Swathe", "", RPR.JobID)]
		RPR_AoE_GrimSwathe = 12023,

		[ParentCombo(RPR_AoE_DPS)]
		[CustomComboInfo("Gluttony", "", RPR.JobID)]
		RPR_AoE_Gluttony = 12024,

		[ParentCombo(RPR_AoE_DPS)]
		[CustomComboInfo("Guillotine", "", RPR.JobID)]
		RPR_AoE_Guillotine = 12025,

		[ParentCombo(RPR_AoE_DPS)]
		[CustomComboInfo("Arcane Circle", "", RPR.JobID)]
		RPR_AoE_Arcane = 12026,

		[ParentCombo(RPR_AoE_DPS)]
		[CustomComboInfo("Plentiful Harvest > Perfectio", "", RPR.JobID)]
		RPR_AoE_PlentifulHarvest = 12027,

		[ParentCombo(RPR_AoE_DPS)]
		[CustomComboInfo("Enshroud > Grim Reaping > Lemure's Scythe > Communio > Sacrificium", "", RPR.JobID)]
		RPR_AoE_Enshroud = 12028,

		[ParentCombo(RPR_AoE_DPS)]
		[CustomComboInfo("Soulsow", "", RPR.JobID)]
		RPR_AoE_Soulsow = 12029,

		#endregion

		#region Utility

		[ReplaceSkill(RPR.HellsIngress, RPR.HellsEgress)]
		[CustomComboInfo("Regress on Ingress and Egress", "", RPR.JobID)]
		RPR_Regress = 12040,

		#endregion

		#region Variant



		#endregion

		#endregion

		#region VIPER - 30000

		#region Single Target DPS

		[ReplaceSkill(VPR.SteelFangs, VPR.HuntersSting, VPR.FlankstingStrike, VPR.FlanksbaneFang,
			VPR.ReavingFangs, VPR.SwiftskinsSting, VPR.HindstingStrike, VPR.HindsbaneFang)]
		[CustomComboInfo("Single Target DPS", "", VPR.JobID)]
		VPR_ST_DPS = 30000,

		[ParentCombo(VPR_ST_DPS)]
		[CustomComboInfo("Serpent's Tail & Twinfang & Twinblood", "", VPR.JobID)]
		VPR_ST_Finishers = 30001,

		[ParentCombo(VPR_ST_DPS)]
		[CustomComboInfo("Vicewinder > Hunter's Coil & Swiftskin's Coil", "", VPR.JobID)]
		VPR_ST_Vicewinder = 30002,

		[ParentCombo(VPR_ST_DPS)]
		[CustomComboInfo("Uncoiled Fury", "", VPR.JobID)]
		VPR_ST_Uncoiled = 30003,

		[ParentCombo(VPR_ST_DPS)]
		[CustomComboInfo("Reawaken", "", VPR.JobID)]
		VPR_ST_Reawaken = 30004,

		[ParentCombo(VPR_ST_DPS)]
		[CustomComboInfo("Serpent's Ire", "", VPR.JobID)]
		VPR_ST_SerpentsIre = 30005,

		#endregion

		#region AoE DPS

		[ReplaceSkill(VPR.SteelMaw, VPR.HuntersBite, VPR.JaggedMaw, VPR.ReavingMaw, VPR.SwiftskinsBite, VPR.BloodiedMaw)]
		[CustomComboInfo("AoE DPS", "", VPR.JobID)]
		VPR_AoE_DPS = 30020,

		[ParentCombo(VPR_AoE_DPS)]
		[CustomComboInfo("Serpent's Tail & Twinfang & Twinblood", "", VPR.JobID)]
		VPR_AoE_Finishers = 30021,

		[ParentCombo(VPR_AoE_DPS)]
		[CustomComboInfo("Vicepit > Hunter's Den & Swiftskin's Den", "", VPR.JobID)]
		VPR_AoE_Vicepit = 30022,

		[ParentCombo(VPR_AoE_DPS)]
		[CustomComboInfo("Uncoiled Fury", "", VPR.JobID)]
		VPR_AoE_Uncoiled = 30023,

		[ParentCombo(VPR_AoE_DPS)]
		[CustomComboInfo("Reawaken", "", VPR.JobID)]
		VPR_AoE_Reawaken = 30024,

		[ParentCombo(VPR_AoE_DPS)]
		[CustomComboInfo("Serpent's Ire", "", VPR.JobID)]
		VPR_AoE_SerpentsIre = 30025,

		#endregion

		#region Utility

		[ReplaceSkill(VPR.Vicewinder, VPR.HuntersCoil, VPR.SwiftskinsCoil, VPR.TwinbloodBite, VPR.TwinfangBite)]
		[CustomComboInfo("Vicewinder", "", VPR.JobID)]
		VPR_Vicewinder = 30040,

		[ReplaceSkill(VPR.Vicepit, VPR.HuntersDen, VPR.SwiftskinsDen, VPR.TwinbloodThresh, VPR.TwinfangThresh)]
		[CustomComboInfo("Vicepit", "", VPR.JobID)]
		VPR_Vicepit = 30041,

		[ReplaceSkill(VPR.UncoiledFury, VPR.UncoiledTwinblood, VPR.UncoiledTwinfang)]
		[CustomComboInfo("Uncoiled Fury", "", VPR.JobID)]
		VPR_Uncoiled = 30042,

		#endregion

		#region Variant



		#endregion

		#endregion

		#endregion

		#region Physical Ranged DPS

		#region BARD - 3000

		#region Single Target DPS

		[ReplaceSkill(BRD.HeavyShot, BRD.BurstShot, BRD.StraightShot, BRD.RefulgentArrow)]
		[CustomComboInfo("Single Target DPS", "", BRD.JobID)]
		BRD_ST_DPS = 3000,

		[ParentCombo(BRD_ST_DPS)]
		[CustomComboInfo("Venomous Bite / Caustic Bite & Windbite / Stormbite & Iron Jaws", "", BRD.JobID)]
		BRD_ST_DoTs = 3001,

		[ParentCombo(BRD_ST_DPS)]
		[CustomComboInfo("Bloodletter / Heartbreak Shot", "", BRD.JobID)]
		BRD_ST_Bloodletter = 3002,

		[ParentCombo(BRD_ST_DPS)]
		[CustomComboInfo("Empyreal Arrow", "", BRD.JobID)]
		BRD_ST_Empyreal = 3003,

		[ParentCombo(BRD_ST_DPS)]
		[CustomComboInfo("Sidewinder", "", BRD.JobID)]
		BRD_ST_Sidewinder = 3004,

		[ParentCombo(BRD_ST_DPS)]
		[CustomComboInfo("Mage's Ballad & Army's Paeon & The Wanderer's Minuet > Pitch Perfect", "", BRD.JobID)]
		BRD_ST_Songs = 3005,

		[ParentCombo(BRD_ST_DPS)]
		[CustomComboInfo("Apex Arrow > Blast Arrow", "", BRD.JobID)]
		BRD_ST_Apex = 3006,

		[ParentCombo(BRD_ST_DPS)]
		[CustomComboInfo("Raging Strikes", "", BRD.JobID)]
		BRD_ST_Raging = 3007,

		[ParentCombo(BRD_ST_DPS)]
		[CustomComboInfo("Battle Voice", "", BRD.JobID)]
		BRD_ST_BattleVoice = 3008,

		[ParentCombo(BRD_ST_DPS)]
		[CustomComboInfo("Barrage > Resonant Arrow", "", BRD.JobID)]
		BRD_ST_Barrage = 3009,

		[ParentCombo(BRD_ST_DPS)]
		[CustomComboInfo("Radiant Finale > Radiant Encore", "", BRD.JobID)]
		BRD_ST_Radiant = 3010,

		#endregion

		#region AoE DPS

		[ReplaceSkill(BRD.QuickNock, BRD.Ladonsbite, BRD.WideVolley, BRD.Shadowbite)]
		[CustomComboInfo("AoE DPS", "", BRD.JobID)]
		BRD_AoE_DPS = 3020,

		[ParentCombo(BRD_AoE_DPS)]
		[CustomComboInfo("Rain of Death", "", BRD.JobID)]
		BRD_AoE_Rain = 3021,

		[ParentCombo(BRD_AoE_DPS)]
		[CustomComboInfo("Empyreal Arrow", "", BRD.JobID)]
		BRD_AoE_Empyreal = 3022,

		[ParentCombo(BRD_AoE_DPS)]
		[CustomComboInfo("Sidewinder", "", BRD.JobID)]
		BRD_AoE_Sidewinder = 3023,

		[ParentCombo(BRD_AoE_DPS)]
		[CustomComboInfo("Mage's Ballad & Army's Paeon & The Wanderer's Minuet > Pitch Perfect", "", BRD.JobID)]
		BRD_AoE_Songs = 3024,

		[ParentCombo(BRD_AoE_DPS)]
		[CustomComboInfo("Apex Arrow > Blast Arrow", "", BRD.JobID)]
		BRD_AoE_Apex = 3025,

		[ParentCombo(BRD_AoE_DPS)]
		[CustomComboInfo("Raging Strikes", "", BRD.JobID)]
		BRD_AoE_Raging = 3026,

		[ParentCombo(BRD_AoE_DPS)]
		[CustomComboInfo("Battle Voice", "", BRD.JobID)]
		BRD_AoE_BattleVoice = 3027,

		[ParentCombo(BRD_AoE_DPS)]
		[CustomComboInfo("Barrage > Resonant Arrow", "", BRD.JobID)]
		BRD_AoE_Barrage = 3028,

		[ParentCombo(BRD_AoE_DPS)]
		[CustomComboInfo("Radiant Finale > Radiant Encore", "", BRD.JobID)]
		BRD_AoE_Radiant = 3029,

		#endregion

		#region Utility



		#endregion

		#region Variant



		#endregion

		#endregion

		#region MACHINIST - 8000

		#region Single Target DPS

		[ReplaceSkill(MCH.SplitShot, MCH.SlugShot, MCH.CleanShot, MCH.HeatedSplitShot, MCH.HeatedSlugShot, MCH.HeatedCleanShot)]
		[CustomComboInfo("Single Target DPS", "", MCH.JobID)]
		MCH_ST_DPS = 8000,

		[ParentCombo(MCH_ST_DPS)]
		[CustomComboInfo("Hypercharge", "", MCH.JobID)]
		MCH_ST_Hypercharge = 8002,

		[ParentCombo(MCH_ST_DPS)]
		[CustomComboInfo("Heat Blast / Blazing Shot", "", MCH.JobID)]
		MCH_ST_HeatBlast = 8001,

		[ParentCombo(MCH_ST_DPS)]
		[CustomComboInfo("Gauss Round & Ricochet / Double Check & Checkmate", "", MCH.JobID)]
		MCH_ST_GaussRico = 8010,

		[ParentCombo(MCH_ST_DPS)]
		[CustomComboInfo("Drill", "", MCH.JobID)]
		MCH_ST_Drill = 8005,

		[ParentCombo(MCH_ST_DPS)]
		[CustomComboInfo("Hot Shot / Air Anchor", "", MCH.JobID)]
		MCH_ST_AirAnchor = 8006,

		[ParentCombo(MCH_ST_DPS)]
		[CustomComboInfo("Chainsaw > Excavator", "", MCH.JobID)]
		MCH_ST_Chainsaw = 8007,

		[ParentCombo(MCH_ST_DPS)]
		[CustomComboInfo("Reassemble", "", MCH.JobID)]
		MCH_ST_Reassemble = 8008,

		[ParentCombo(MCH_ST_DPS)]
		[CustomComboInfo("Wildfire", "", MCH.JobID)]
		MCH_ST_Wildfire = 8004,

		[ParentCombo(MCH_ST_DPS)]
		[CustomComboInfo("Rook Autoturret / Automaton Queen", "", MCH.JobID)]
		MCH_ST_Queen = 8003,

		[ParentCombo(MCH_ST_DPS)]
		[CustomComboInfo("Barrel Stabilizer > Full Metal Field", "", MCH.JobID)]
		MCH_ST_Barrel = 8009,

		#endregion

		#region AoE DPS

		[ReplaceSkill(MCH.SpreadShot, MCH.Scattergun, MCH.AutoCrossbow)]
		[CustomComboInfo("AoE DPS", "", MCH.JobID)]
		MCH_AoE_DPS = 8020,

		[ParentCombo(MCH_AoE_DPS)]
		[CustomComboInfo("Hypercharge", "", MCH.JobID)]
		MCH_AoE_Hypercharge = 8021,

		[ParentCombo(MCH_AoE_DPS)]
		[CustomComboInfo("Auto Crossbow", "", MCH.JobID)]
		MCH_AoE_Crossbow = 8022,

		[ParentCombo(MCH_AoE_DPS)]
		[CustomComboInfo("Gauss Round & Ricochet / Double Check & Checkmate", "", MCH.JobID)]
		MCH_AoE_GaussRico = 8023,

		[ParentCombo(MCH_AoE_DPS)]
		[CustomComboInfo("Bioblaster", "", MCH.JobID)]
		MCH_AoE_Bioblaster = 8024,

		[ParentCombo(MCH_AoE_DPS)]
		[CustomComboInfo("Chainsaw > Excavator", "", MCH.JobID)]
		MCH_AoE_Chainsaw = 8025,

		[ParentCombo(MCH_AoE_DPS)]
		[CustomComboInfo("Reassemble", "", MCH.JobID)]
		MCH_AoE_Reassemble = 8026,

		[ParentCombo(MCH_AoE_DPS)]
		[CustomComboInfo("Barrel Stabilizer > Full Metal Field", "", MCH.JobID)]
		MCH_AoE_Barrel = 8027,

		#endregion

		#region Utility

		[ReplaceSkill(MCH.GaussRound, MCH.DoubleCheck, MCH.Ricochet, MCH.Checkmate)]
		[CustomComboInfo("Gauss Round & Ricochet / Double Check & Checkmate", "", MCH.JobID)]
		MCH_GaussRicochet = 8040,

		[ReplaceSkill(MCH.Dismantle)]
		[CustomComboInfo("Dismantle Protection", "", MCH.JobID)]
		MCH_DismantleProtect = 8041,

		#endregion

		#region Variant



		#endregion

		#endregion

		#region DANCER - 4000

		#region Single Target DPS

		[ReplaceSkill(DNC.Cascade, DNC.Fountain)]
		[CustomComboInfo("Single Target DPS", "", DNC.JobID)]
		DNC_ST_DPS = 4000,

		[ParentCombo(DNC_ST_DPS)]
		[CustomComboInfo("Fan Dance", "", DNC.JobID)]
		DNC_ST_FanDance = 4001,

		[ParentCombo(DNC_ST_DPS)]
		[CustomComboInfo("Fan Dance III", "", DNC.JobID)]
		DNC_ST_FanDance3 = 4002,

		[ParentCombo(DNC_ST_DPS)]
		[CustomComboInfo("Fan Dance IV", "", DNC.JobID)]
		DNC_ST_FanDance4 = 4003,

		[ParentCombo(DNC_ST_DPS)]
		[CustomComboInfo("Standard Step > Finishing Move", "", DNC.JobID)]
		DNC_ST_Standard = 4004,

		[ParentCombo(DNC_ST_DPS)]
		[CustomComboInfo("Technical Step > Tillana", "", DNC.JobID)]
		DNC_ST_Technical = 4005,

		[ParentCombo(DNC_ST_DPS)]
		[CustomComboInfo("Saber Dance > Dance of the Dawn", "", DNC.JobID)]
		DNC_ST_Saber = 4006,

		[ParentCombo(DNC_ST_DPS)]
		[CustomComboInfo("Starfall Dance", "", DNC.JobID)]
		DNC_ST_Starfall = 4007,

		[ParentCombo(DNC_ST_DPS)]
		[CustomComboInfo("Last Dance", "", DNC.JobID)]
		DNC_ST_LastDance = 4008,

		[ParentCombo(DNC_ST_DPS)]
		[CustomComboInfo("Devilment", "", DNC.JobID)]
		DNC_ST_Devilment = 4009,

		[ParentCombo(DNC_ST_DPS)]
		[CustomComboInfo("Flourish", "", DNC.JobID)]
		DNC_ST_Flourish = 4010,

		#endregion

		#region AoE DPS

		[ReplaceSkill(DNC.Windmill, DNC.Bladeshower)]
		[CustomComboInfo("AoE DPS", "", DNC.JobID)]
		DNC_AoE_DPS = 4020,

		[ParentCombo(DNC_AoE_DPS)]
		[CustomComboInfo("Fan Dance II", "", DNC.JobID)]
		DNC_AoE_FanDance2 = 4021,

		[ParentCombo(DNC_AoE_DPS)]
		[CustomComboInfo("Fan Dance III", "", DNC.JobID)]
		DNC_AoE_FanDance3 = 4022,

		[ParentCombo(DNC_AoE_DPS)]
		[CustomComboInfo("Fan Dance IV", "", DNC.JobID)]
		DNC_AoE_FanDance4 = 4023,

		[ParentCombo(DNC_AoE_DPS)]
		[CustomComboInfo("Standard Step > Finishing Move", "", DNC.JobID)]
		DNC_AoE_Standard = 4024,

		[ParentCombo(DNC_AoE_DPS)]
		[CustomComboInfo("Technical Step > Tillana", "", DNC.JobID)]
		DNC_AoE_Technical = 4025,

		[ParentCombo(DNC_AoE_DPS)]
		[CustomComboInfo("Saber Dance > Dance of the Dawn", "", DNC.JobID)]
		DNC_AoE_Saber = 4026,

		[ParentCombo(DNC_AoE_DPS)]
		[CustomComboInfo("Starfall Dance", "", DNC.JobID)]
		DNC_AoE_Starfall = 4027,

		[ParentCombo(DNC_AoE_DPS)]
		[CustomComboInfo("Last Dance", "", DNC.JobID)]
		DNC_AoE_LastDance = 4028,

		[ParentCombo(DNC_AoE_DPS)]
		[CustomComboInfo("Devilment", "", DNC.JobID)]
		DNC_AoE_Devilment = 4029,

		[ParentCombo(DNC_AoE_DPS)]
		[CustomComboInfo("Flourish", "", DNC.JobID)]
		DNC_AoE_Flourish = 4030,

		#endregion

		#region Utility


		#endregion

		#region Variant



		#endregion

		#endregion

		#endregion

		#region Magical Ranged DPS

		#region BLACK MAGE - 2000

		#region Single Target DPS

		[ReplaceSkill(BLM.Fire, BLM.Fire3, BLM.Fire4, BLM.Blizzard, BLM.Blizzard3, BLM.Blizzard4)]
		[CustomComboInfo("Single Target DPS", "", BLM.JobID)]
		BLM_ST_DPS = 2000,

		[ParentCombo(BLM_ST_DPS)]
		[CustomComboInfo("Thunder", "", BLM.JobID)]
		BLM_ST_Thunder = 2001,

		[ParentCombo(BLM_ST_DPS)]
		[CustomComboInfo("Xenoglossy", "", BLM.JobID)]
		BLM_ST_Xeno = 2002,

		[ParentCombo(BLM_ST_DPS)]
		[CustomComboInfo("Amplifier", "", BLM.JobID)]
		BLM_ST_Amplifier = 2003,

		[ParentCombo(BLM_ST_DPS)]
		[CustomComboInfo("Manafont", "", BLM.JobID)]
		BLM_ST_Manafont = 2004,

		[ParentCombo(BLM_ST_DPS)]
		[CustomComboInfo("Ley Lines > Between the Lines", "", BLM.JobID)]
		BLM_ST_LeyLines = 2005,

		[ParentCombo(BLM_ST_DPS)]
		[CustomComboInfo("Triplecast", "", BLM.JobID)]
		BLM_ST_Triplecast = 2006,

		[ParentCombo(BLM_ST_DPS)]
		[CustomComboInfo("Swiftcast", "", BLM.JobID)]
		BLM_ST_Swiftcast = 2007,

		[ParentCombo(BLM_ST_DPS)]
		[CustomComboInfo("Flare Star", "", BLM.JobID)]
		BLM_ST_FlareStar = 2008,

		#endregion

		#region AoE DPS

		[ReplaceSkill(BLM.Fire2, BLM.HighFire2, BLM.Blizzard2, BLM.HighBlizzard2)]
		[CustomComboInfo("AoE DPS", "", BLM.JobID)]
		BLM_AoE_DPS = 2020,

		[ParentCombo(BLM_AoE_DPS)]
		[CustomComboInfo("Thunder", "", BLM.JobID)]
		BLM_AoE_Thunder = 2021,

		[ParentCombo(BLM_AoE_DPS)]
		[CustomComboInfo("Foul", "", BLM.JobID)]
		BLM_AoE_Foul = 2022,

		[ParentCombo(BLM_AoE_DPS)]
		[CustomComboInfo("Amplifier", "", BLM.JobID)]
		BLM_AoE_Amplifier = 2023,

		[ParentCombo(BLM_AoE_DPS)]
		[CustomComboInfo("Manafont", "", BLM.JobID)]
		BLM_AoE_Manafont = 2024,

		[ParentCombo(BLM_AoE_DPS)]
		[CustomComboInfo("Ley Lines > Between the Lines", "", BLM.JobID)]
		BLM_AoE_LeyLines = 2025,

		[ParentCombo(BLM_AoE_DPS)]
		[CustomComboInfo("Triplecast", "", BLM.JobID)]
		BLM_AoE_Triplecast = 2026,

		[ParentCombo(BLM_AoE_DPS)]
		[CustomComboInfo("Swiftcast", "", BLM.JobID)]
		BLM_AoE_Swiftcast = 2027,

		[ParentCombo(BLM_AoE_DPS)]
		[CustomComboInfo("Flare Star", "", BLM.JobID)]
		BLM_AoE_FlareStar = 2028,

		#endregion

		#region Utility

		[ReplaceSkill(BLM.Fire4, BLM.Blizzard4)]
		[CustomComboInfo("Fire 4 / Blizzard 4 Swapper", "", BLM.JobID)]
		BLM_Fire4Blizzard4 = 2040,

		[ReplaceSkill(BLM.Xenoglossy, BLM.Paradox, BLM.Despair)]
		[CustomComboInfo("Xenoglossy / Paradox / Despair + Swiftcast - Movement", "", BLM.JobID)]
		BLM_XenoParadox = 2044,

		[ReplaceSkill(BLM.Transpose, BLM.UmbralSoul)]
		[CustomComboInfo("Tranpose / Umbral Soul", "", BLM.JobID)]
		BLM_UmbralTranspose = 2043,

		[ReplaceSkill(BLM.LeyLines, BLM.BetweenTheLines)]
		[CustomComboInfo("Leylines > Between the Lines", "", BLM.JobID)]
		BLM_LeyLines = 2041,

		[ReplaceSkill(BLM.Triplecast, All.Swiftcast)]
		[CustomComboInfo("Triple Cast / Swiftcast Protection", "", BLM.JobID)]
		BLM_TriplecastProtect = 2045,

		#endregion

		#region Bozja

		[Bozja]
		[BozjaParent(BLM_ST_DPS)]
		[ReplaceSkill(Bozja.FlareStar)]
		[CustomComboInfo("Lost Flare Star", "", BLM.JobID)]
		BLM_Bozja_LFS = 2060,

		#endregion

		#region Variant



		#endregion

		#endregion

		#region SUMMONER - 17000

		#region Single Target DPS

		[ReplaceSkill(SMN.Ruin, SMN.Ruin2, SMN.Ruin3)]
		[CustomComboInfo("Single Target DPS", "", SMN.JobID)]
		SMN_ST_DPS = 17000,

		[ParentCombo(SMN_ST_DPS)]
		[CustomComboInfo("Energy Drain", "", SMN.JobID)]
		SMN_ST_EnergyDrain = 17001,

		[ParentCombo(SMN_ST_DPS)]
		[CustomComboInfo("Gemshine", "", SMN.JobID)]
		SMN_ST_Gemshine = 17002,

		[ParentCombo(SMN_ST_DPS)]
		[CustomComboInfo("Astral Flow", "", SMN.JobID)]
		SMN_ST_Astral = 17003,

		[ParentCombo(SMN_ST_DPS)]
		[CustomComboInfo("Ruin IV Mobility", "", SMN.JobID)]
		SMN_ST_Ruin4 = 17004,

		[ParentCombo(SMN_ST_DPS)]
		[CustomComboInfo("Lucid Dreaming", "", SMN.JobID)]
		SMN_ST_Lucid = 17005,

		[ParentCombo(SMN_ST_DPS)]
		[CustomComboInfo("Lucid Dreaming", "", SMN.JobID)]
		SMN_ST_SearingLight = 17006,

		[ParentCombo(SMN_ST_DPS)]
		[CustomComboInfo("Lucid Dreaming", "", SMN.JobID)]
		SMN_ST_RadiantAegis = 17007,

		#endregion

		#region AoE DPS

		[ReplaceSkill(SMN.Outburst, SMN.Tridisaster)]
		[CustomComboInfo("AoE DPS", "", SMN.JobID)]
		SMN_AoE_DPS = 17020,

		[ParentCombo(SMN_AoE_DPS)]
		[CustomComboInfo("Energy Siphon", "", SMN.JobID)]
		SMN_AoE_EnergySiphon = 17021,

		[ParentCombo(SMN_AoE_DPS)]
		[CustomComboInfo("Pain Flare", "", SMN.JobID)]
		SMN_AoE_Painflare = 17022,

		[ParentCombo(SMN_AoE_DPS)]
		[CustomComboInfo("Precious Brilliance", "", SMN.JobID)]
		SMN_AoE_PreciousBrilliance = 17023,

		[ParentCombo(SMN_AoE_DPS)]
		[CustomComboInfo("Astral Flow", "", SMN.JobID)]
		SMN_AoE_Astral = 17024,

		[ParentCombo(SMN_AoE_DPS)]
		[CustomComboInfo("Ruin IV Mobility", "", SMN.JobID)]
		SMN_AoE_Ruin4 = 17025,

		[ParentCombo(SMN_AoE_DPS)]
		[CustomComboInfo("Lucid Dreaming", "", SCH.JobID)]
		SMN_AoE_Lucid = 17026,

		#endregion

		#region Utility

		[ReplaceSkill(SMN.EnergyDrain, SMN.Fester, SMN.Necrotize)]
		[CustomComboInfo("Energy Drain > Fester / Necrotize", "", SMN.JobID)]
		SMN_EnergyDrainNecro = 17040,

		[ReplaceSkill(SMN.SummonBahamut, SMN.EnkindleBahamut, SMN.SummonPhoenix, SMN.EnkindlePhoenix, SMN.SummonSolarBahamut, SMN.EnkindleSolarBahamut)]
		[CustomComboInfo("Summon > Enkindle", "", SMN.JobID)]
		SMN_Enkindle = 17041,

		[ReplaceSkill(SMN.Resurrection)]
		[CustomComboInfo("Swiftcast > Raise", "", SMN.JobID)]
		SMN_Raise = 17042,

		[ReplaceSkill(SMN.Ruin, SMN.Ruin2, SMN.Ruin3)]
		[CustomComboInfo("Carbuncle Reminder", "", SMN.JobID)]
		SMN_Reminder = 17043,

		#endregion

		#region Variant



		#endregion

		#endregion

		#region RED MAGE - 13000

		#region Single Target DPS

		[ReplaceSkill(RDM.Jolt, RDM.Jolt2, RDM.Jolt3, RDM.Verthunder, RDM.Verthunder3, RDM.Verfire, RDM.Veraero, RDM.Veraero3, RDM.Verstone)]
		[CustomComboInfo("Single Target DPS", "", RDM.JobID)]
		RDM_ST_DPS = 13000,

		[ParentCombo(RDM_ST_DPS)]
		[CustomComboInfo("Enchanted Riposte > Enchanted Zwerchhau > Enchanted Redoublement", "", RDM.JobID)]
		RDM_ST_Swords = 13001,

		[ParentCombo(RDM_ST_DPS)]
		[CustomComboInfo("Fleche", "", RDM.JobID)]
		RDM_ST_Fleche = 13002,

		[ParentCombo(RDM_ST_DPS)]
		[CustomComboInfo("Contre Sixte", "", RDM.JobID)]
		RDM_ST_Contre = 13003,

		[ParentCombo(RDM_ST_DPS)]
		[CustomComboInfo("Engagement", "", RDM.JobID)]
		RDM_ST_Engagement = 13004,

		[ParentCombo(RDM_ST_DPS)]
		[CustomComboInfo("Corpsacorps", "", RDM.JobID)]
		RDM_ST_Corps = 13005,

		[ParentCombo(RDM_ST_DPS)]
		[CustomComboInfo("Embolden > Vice of Thorns", "", RDM.JobID)]
		RDM_ST_Embolden = 13006,

		[ParentCombo(RDM_ST_DPS)]
		[CustomComboInfo("Manafication > Prefulgence", "", RDM.JobID)]
		RDM_ST_Manafication = 13007,

		[ParentCombo(RDM_ST_DPS)]
		[CustomComboInfo("Acceleration > Grand Impact", "", RDM.JobID)]
		RDM_ST_Accel = 13008,

		[ParentCombo(RDM_ST_DPS)]
		[CustomComboInfo("Swiftcast", "", RDM.JobID)]
		RDM_ST_Swift = 13009,

		[ParentCombo(RDM_ST_DPS)]
		[CustomComboInfo("Lucid Dreaming", "", RDM.JobID)]
		RDM_ST_Lucid = 13010,

		#endregion

		#region AoE DPS

		[ReplaceSkill(RDM.Scatter, RDM.Impact, RDM.Verthunder2, RDM.Veraero2)]
		[CustomComboInfo("AoE DPS", "", RDM.JobID)]
		RDM_AoE_DPS = 13020,

		[ParentCombo(RDM_AoE_DPS)]
		[CustomComboInfo("Enchanted Moulinet > Enchanted Moulinet Deux > Enchanted Moulinet Trois", "", RDM.JobID)]
		RDM_AoE_Swords = 13021,

		[ParentCombo(RDM_AoE_DPS)]
		[CustomComboInfo("Fleche", "", RDM.JobID)]
		RDM_AoE_Fleche = 13022,

		[ParentCombo(RDM_AoE_DPS)]
		[CustomComboInfo("Contre Sixte", "", RDM.JobID)]
		RDM_AoE_Contre = 13023,

		[ParentCombo(RDM_AoE_DPS)]
		[CustomComboInfo("Corpsacorps", "", RDM.JobID)]
		RDM_AoE_Corps = 13024,

		[ParentCombo(RDM_AoE_DPS)]
		[CustomComboInfo("Embolden > Vice of Thorns", "", RDM.JobID)]
		RDM_AoE_Embolden = 13025,

		[ParentCombo(RDM_AoE_DPS)]
		[CustomComboInfo("Manafication > Prefulgence", "", RDM.JobID)]
		RDM_AoE_Manafication = 13026,

		[ParentCombo(RDM_AoE_DPS)]
		[CustomComboInfo("Acceleration > Grand Impact", "", RDM.JobID)]
		RDM_AoE_Accel = 13027,

		[ParentCombo(RDM_AoE_DPS)]
		[CustomComboInfo("Swiftcast", "", RDM.JobID)]
		RDM_AoE_Swift = 13028,

		[ParentCombo(RDM_AoE_DPS)]
		[CustomComboInfo("Lucid Dreaming", "", RDM.JobID)]
		RDM_AoE_Lucid = 13029,

		#endregion

		#region Utility

		[ReplaceSkill(RDM.Riposte, RDM.Zwerchhau, RDM.Redoublement, RDM.EnchantedRiposte, RDM.EnchantedZwerchhau, RDM.EnchantedRedoublement)]
		[CustomComboInfo("Melee Single Target Combo", "", RDM.JobID)]
		RDM_ST_Melee = 13040,

		[ReplaceSkill(RDM.Moulinet, RDM.EnchantedMoulinet, RDM.EnchantedMoulinetDeux, RDM.EnchantedMoulinetTrois)]
		[CustomComboInfo("Melee AoE Combo", "", RDM.JobID)]
		RDM_AoE_Melee = 13041,

		[ReplaceSkill(RDM.Verraise)]
		[CustomComboInfo("Verraise", "", RDM.JobID)]
		RDM_Raise = 13042,

		#endregion

		#region Variant



		#endregion

		#endregion

		#region PICTOMANCER - 20000

		#region Single Target DPS

		[ReplaceSkill(PCT.Fire, PCT.Aero, PCT.Water, PCT.Blizzard, PCT.Stone, PCT.Thunder)]
		[CustomComboInfo("Single Target DPS", "", PCT.JobID)]
		PCT_ST_DPS = 20000,

		[ParentCombo(PCT_ST_DPS)]
		[CustomComboInfo("Creature Motif > Living Muse", "", PCT.JobID)]
		PCT_ST_Creatures = 20001,

		[ParentCombo(PCT_ST_DPS)]
		[CustomComboInfo("Weapon Motif > Steel Muse", "", PCT.JobID)]
		PCT_ST_Hammer = 20002,

		[ParentCombo(PCT_ST_DPS)]
		[CustomComboInfo("Landscape Motif > Scenic Muse", "", PCT.JobID)]
		PCT_ST_Landscape = 20003,

		[ParentCombo(PCT_ST_DPS)]
		[CustomComboInfo("Subtractive Palette", "", PCT.JobID)]
		PCT_ST_Subtractive = 20004,

		[ParentCombo(PCT_ST_DPS)]
		[CustomComboInfo("Holy in White & Comet in Black", "", PCT.JobID)]
		PCT_ST_Comet = 20005,

		[ParentCombo(PCT_ST_DPS)]
		[CustomComboInfo("Mog of the Ages & Retribution of the Madeen", "", PCT.JobID)]
		PCT_ST_MogMadeen = 20006,

		[ParentCombo(PCT_ST_DPS)]
		[CustomComboInfo("Rainbow Drip", "", PCT.JobID)]
		PCT_ST_RainbowDrip = 20007,

		[ParentCombo(PCT_ST_DPS)]
		[CustomComboInfo("Star Prism", "", PCT.JobID)]
		PCT_ST_StarPrism = 20008,

		[ParentCombo(PCT_ST_DPS)]
		[CustomComboInfo("Swiftcast Movement", "", PCT.JobID)]
		PCT_ST_Swiftcast = 20009,

		[ParentCombo(PCT_ST_DPS)]
		[CustomComboInfo("Lucid Dreaming", "", PCT.JobID)]
		PCT_ST_Lucid = 20010,

		#endregion

		#region AoE DPS

		[ReplaceSkill(PCT.Fire2, PCT.Aero2, PCT.Water2, PCT.Blizzard2, PCT.Stone2, PCT.Thunder2)]
		[CustomComboInfo("AoE DPS", "", PCT.JobID)]
		PCT_AoE_DPS = 20020,

		[ParentCombo(PCT_AoE_DPS)]
		[CustomComboInfo("Creature Motif > Living Muse", "", PCT.JobID)]
		PCT_AoE_Creatures = 20021,

		[ParentCombo(PCT_AoE_DPS)]
		[CustomComboInfo("Weapon Motif > Steel Muse", "", PCT.JobID)]
		PCT_AoE_Hammer = 20022,

		[ParentCombo(PCT_AoE_DPS)]
		[CustomComboInfo("Landscape Motif > Scenic Muse", "", PCT.JobID)]
		PCT_AoE_Landscape = 20023,

		[ParentCombo(PCT_AoE_DPS)]
		[CustomComboInfo("Subtractive Palette", "", PCT.JobID)]
		PCT_AoE_Subtractive = 20024,

		[ParentCombo(PCT_AoE_DPS)]
		[CustomComboInfo("Holy in White & Comet in Black", "", PCT.JobID)]
		PCT_AoE_Comet = 20025,

		[ParentCombo(PCT_AoE_DPS)]
		[CustomComboInfo("Mog of the Ages & Retribution of the Madeen", "", PCT.JobID)]
		PCT_AoE_MogMadeen = 20026,

		[ParentCombo(PCT_AoE_DPS)]
		[CustomComboInfo("Rainbow Drip", "", PCT.JobID)]
		PCT_AoE_RainbowDrip = 20027,

		[ParentCombo(PCT_AoE_DPS)]
		[CustomComboInfo("Star Prism", "", PCT.JobID)]
		PCT_AoE_StarPrism = 20028,

		[ParentCombo(PCT_AoE_DPS)]
		[CustomComboInfo("Swiftcast Movement", "", PCT.JobID)]
		PCT_AoE_Swiftcast = 20029,

		[ParentCombo(PCT_AoE_DPS)]
		[CustomComboInfo("Lucid Dreaming", "", PCT.JobID)]
		PCT_AoE_Lucid = 20030,

		#endregion

		#region Utility



		#endregion

		#region Variant



		#endregion

		#endregion

		#region BLUE MAGE - 70000

		[ReplaceSkill(BLU.MoonFlute)]
		[BlueInactive(BLU.Whistle, BLU.Tingle, BLU.RoseOfDestruction, BLU.MoonFlute, BLU.JKick, BLU.TripleTrident, BLU.Nightbloom, BLU.WingedReprobation, BLU.SeaShanty, BLU.BeingMortal, BLU.ShockStrike, BLU.Surpanakha, BLU.MatraMagic, BLU.PhantomFlurry, BLU.Bristle, BLU.FeatherRain)]
		[CustomComboInfo("Moon Flute Combo", "Turns Moon Flute into a full opener\nUse the remaining 2 charges of Winged Reprobation before starting the opener again!\nCan be done with 2.50 spell speed", BLU.JobID, 1)]
		BLU_MoonFluteOpener = 70001,

		[BlueInactive(BLU.BreathOfMagic, BLU.MortalFlame)]
		[ParentCombo(BLU_MoonFluteOpener)]
		[CustomComboInfo("DoT Alternative", "Only have 1 DoT active (Breath of Magic OR Mortal Flame)\nRequires 2.20 spell speed or faster", BLU.JobID)]
		BLU_MoonFluteOpener_DoTOpener = 70002,

		#region Combos

		[BlueInactive(BLU.Whistle, BLU.Offguard, BLU.Tingle, BLU.BasicInstinct, BLU.MoonFlute, BLU.FinalSting)]
		[ReplaceSkill(BLU.FinalSting)]
		[CustomComboInfo("Final Sting Combo", "Whistle > Off-guard > Tingle > [Basic Instinct] > Moon Flute > Swiftcast > Final Sting", BLU.JobID)]
		BLU_Sting = 70011,

		[BlueInactive(BLU.ToadOil, BLU.Bristle, BLU.MoonFlute, BLU.SelfDestruct)]
		[ReplaceSkill(BLU.SelfDestruct)]
		[CustomComboInfo("Self-destruct Combo", "Toad Oil > Bristle > Moon Flute > Self-destruct", BLU.JobID)]
		BLU_Explode = 70012,

		[BlueInactive(BLU.Whistle, BLU.Tingle, BLU.TripleTrident)]
		[ReplaceSkill(BLU.TripleTrident)]
		[CustomComboInfo("Triple Trident Combo", "Whistle > Tingle > Triple Trident", BLU.JobID)]
		BLU_TripleTrident = 70013,

		[BlueInactive(BLU.Bristle, BLU.BreathOfMagic, BLU.MortalFlame, BLU.SongOfTorment, BLU.MatraMagic)]
		[ReplaceSkill(BLU.BreathOfMagic, BLU.MortalFlame, BLU.SongOfTorment, BLU.MatraMagic)]
		[CustomComboInfo("Buffed Breath of Magic, Mortal Flame, Song of Torment, and Matra Magic", "Bristle > Breath of Magic > Bristle > Mortal Flame > Bristle > Song of Torment > Bristle > Matra Magic", BLU.JobID)]
		BLU_DoTs = 70014,

		[BlueInactive(BLU.RamsVoice, BLU.Ultravibration)]
		[ReplaceSkill(BLU.Ultravibration)]
		[CustomComboInfo("Vibe Check", "Hydro Pull > Ram's Voice > Swiftcast > Ultravibration", BLU.JobID)]
		BLU_Ultravibration = 70010,

		[BlueInactive(BLU.PeripheralSynthesis, BLU.MustardBomb)]
		[ReplaceSkill(BLU.PeripheralSynthesis, BLU.MustardBomb)]
		[CustomComboInfo("Bomb Combo", "Peripheral Synthesis > Mustard Bomb", BLU.JobID)]
		BLU_Periph = 70015,

		#endregion

		#region Utility

		[BlueInactive(BLU.GoblinPunch, BLU.MightyGuard, BLU.ToadOil, BLU.Devour, BLU.PeatPelt, BLU.DeepClean)]
		[ReplaceSkill(BLU.GoblinPunch)]
		[CustomComboInfo("Tank Combo", "Mighty Guard, Toad Oil, and Devour Checks, then Peculiar Light, Peat Pelt, and Deep Clean", BLU.JobID)]
		BLU_Tanking = 70030,

		[BlueInactive(BLU.GoblinPunch, BLU.BloodDrain, BLU.ChocoMeteor)]
		[ReplaceSkill(BLU.GoblinPunch, BLU.SonicBoom, BLU.ChocoMeteor)]
		[CustomComboInfo("Lucid Dreaming and Blood Drain", "", BLU.JobID)]
		BLU_ManaGain = 70031,

		[BlueInactive(BLU.AngelWhisper)]
		[ReplaceSkill(BLU.AngelWhisper)]
		[CustomComboInfo("Swiftcast > Angel Whisper", "", BLU.JobID)]
		BLU_Raise = 70032,

		[BlueInactive(BLU.GoblinPunch, BLU.SonicBoom, BLU.PhantomFlurry)]
		[ReplaceSkill(BLU.GoblinPunch, BLU.SonicBoom, BLU.PhantomFlurry)]
		[CustomComboInfo("Phantom Flurry Perfect Ending", "", BLU.JobID)]
		BLU_PhantomEnder = 70033,

		[BlueInactive(BLU.GoblinPunch, BLU.Gobskin, BLU.Pomcure, BLU.BasicInstinct, BLU.MightyGuard, BLU.AngelsSnack, BLU.FeatherRain, BLU.BreathOfMagic,
			BLU.MortalFlame, BLU.TripleTrident)]
		[ReplaceSkill(BLU.GoblinPunch)]
		[CustomComboInfo("Solo Treasure Mappin' (Healer Mimic required)", "", BLU.JobID)]
		BLU_Treasure = 70040,

		[ParentCombo(BLU_Treasure)]
		[CustomComboInfo("Auto Spell Setting - Only check if you are actively solo mapping.\n" +
			"Ram's Voice = Shock Strike\n" +
			"Missile = Glass Dance\n" +
			"Ultravibration = Quasar\n" +
			"Hydro Pull = Sea Shanty\n" +
			"You MUST have Ram's Voice, Missile, Ultravibration, and Hydropull, in that order, in the last 4 slots of your spellbook.\n" +
			"You MUST NOT have Shock Strike, Glass Dance, Quasar, or Sea Shanty active in the spellbook.", "", BLU.JobID)]
		BLU_Treasure_AutoSpell = 70041,

		[ParentCombo(BLU_Treasure)]
		[CustomComboInfo("Basic Instinct", "", BLU.JobID)]
		BLU_Treasure_BasicInstinct = 70042,

		[ParentCombo(BLU_Treasure)]
		[CustomComboInfo("Mighty Guard", "", BLU.JobID)]
		BLU_Treasure_MightyGuard = 70043,

		[ParentCombo(BLU_Treasure)]
		[CustomComboInfo("Angel's Snack", "", BLU.JobID)]
		BLU_Treasure_AngelsSnack = 70044,

		[ParentCombo(BLU_Treasure)]
		[CustomComboInfo("Pomcure", "", BLU.JobID)]
		BLU_Treasure_Pomcure = 70045,

		[ParentCombo(BLU_Treasure)]
		[CustomComboInfo("Gobskin", "", BLU.JobID)]
		BLU_Treasure_Gobskin = 70046,

		[ParentCombo(BLU_Treasure)]
		[CustomComboInfo("Feather Rain", "", BLU.JobID)]
		BLU_Treasure_FeatherRain = 70047,

		[ParentCombo(BLU_Treasure)]
		[CustomComboInfo("Triple Trident", "", BLU.JobID)]
		BLU_Treasure_TripleTrident = 70048,

		[ParentCombo(BLU_Treasure)]
		[CustomComboInfo("Breath of Magic", "", BLU.JobID)]
		BLU_Treasure_BreathOfMagic = 70049,

		[ParentCombo(BLU_Treasure)]
		[CustomComboInfo("Mortal Flame", "", BLU.JobID)]
		BLU_Treasure_MortalFlame = 70050,

		#endregion

		#endregion

		#endregion

		#endregion

		#region PvP

		#region ASTROLOGIAN - 111000



		#endregion

		#region BLACK MAGE - 112000



		#endregion

		#region BARD - 113000



		#endregion

		#region DANCER - 114000



		#endregion

		#region DARK KNIGHT - 115000



		#endregion

		#region DRAGOON - 116000

		[PvPCustomCombo]
		[ReplaceSkill(DRGPvP.RaidenThrust, DRGPvP.FangAndClaw, DRGPvP.WheelingThrust, DRGPvP.HeavensThrust)]
		[CustomComboInfo("Combo Mode", "", DRG.JobID)]
		DRGPvP_Combo = 116000,

		[PvPCustomCombo]
		[ParentCombo(DRGPvP_Combo)]
		[CustomComboInfo("Geirskogul", "", DRG.JobID)]
		DRGPvP_Geirskogul = 116001,

		[PvPCustomCombo]
		[ParentCombo(DRGPvP_Combo)]
		[CustomComboInfo("Wyrmwind Thrust", "", DRG.JobID)]
		DRGPvP_Wyrmwind = 116002,

		[PvPCustomCombo]
		[ParentCombo(DRGPvP_Combo)]
		[CustomComboInfo("Horrid Roar", "", DRG.JobID)]
		DRGPvP_Roar = 116003,

		[PvPCustomCombo]
		[ParentCombo(DRGPvP_Combo)]
		[CustomComboInfo("Chaotic Spring", "", DRG.JobID)]
		DRGPvP_Chaotic = 116004,

		#endregion

		#region GUNBREAKER - 117000



		#endregion

		#region MACHINIST - 118000



		#endregion

		#region MONK - 119000

		[PvPCustomCombo]
		[ReplaceSkill(MNKPvP.Bootshine, MNKPvP.TrueStrike, MNKPvP.SnapPunch, MNKPvP.DragonKick, MNKPvP.TwinSnakes, MNKPvP.Demolish, MNKPvP.PhantomRush)]
		[CustomComboInfo("Combo Mode", "", MNK.JobID)]
		MNKPvP_Combo = 119000,

		[PvPCustomCombo]
		[ParentCombo(MNKPvP_Combo)]
		[CustomComboInfo("Thunderclap", "", MNK.JobID)]
		MNKPvP_Thunderclap = 119001,

		[PvPCustomCombo]
		[ParentCombo(MNKPvP_Combo)]
		[CustomComboInfo("Rising Phoenix", "", MNK.JobID)]
		MNKPvP_Rising = 119002,

		[PvPCustomCombo]
		[ParentCombo(MNKPvP_Combo)]
		[CustomComboInfo("Enlightenment", "", MNK.JobID)]
		MNKPvP_Enlightenment = 119003,

		[PvPCustomCombo]
		[ParentCombo(MNKPvP_Combo)]
		[CustomComboInfo("Six-sided Star", "", MNK.JobID)]
		MNKPvP_SixSidedStar = 119004,

		[PvPCustomCombo]
		[ParentCombo(MNKPvP_Combo)]
		[CustomComboInfo("Riddle of Earth", "", MNK.JobID)]
		MNKPvP_RiddleOfEarth = 119005,

		[PvPCustomCombo]
		[ParentCombo(MNKPvP_Combo)]
		[CustomComboInfo("Meteodrive", "", MNK.JobID)]
		MNKPvP_MeteoDrive = 119006,

		#endregion

		#region NINJA - 120000

		[PvPCustomCombo]
		[ReplaceSkill(NINPvP.SpinningEdge, NINPvP.GustSlash, NINPvP.AeolianEdge)]
		[CustomComboInfo("Combo Mode", "", NIN.JobID)]
		NINPvP_Combo = 120000,

		[PvPCustomCombo]
		[ParentCombo(NINPvP_Combo)]
		[CustomComboInfo("Mug", "", NIN.JobID)]
		NINPvP_Mug = 120001,

		[PvPCustomCombo]
		[ParentCombo(NINPvP_Combo)]
		[CustomComboInfo("Bunshin", "", NIN.JobID)]
		NINPvP_Bunshin = 120002,

		[PvPCustomCombo]
		[ParentCombo(NINPvP_Combo)]
		[CustomComboInfo("Fuma Shuriken", "", NIN.JobID)]
		NINPvP_Shuriken = 120003,

		[PvPCustomCombo]
		[ParentCombo(NINPvP_Combo)]
		[CustomComboInfo("Kassatsu (Three Mudra)\nAlthough Doton, Huton, and Meisui are scripted in, they would be better used manually in ideal situations.", "", NIN.JobID)]
		NINPvP_Kassatsu = 120004,

		[PvPCustomCombo]
		[ParentCombo(NINPvP_Kassatsu)]
		[CustomComboInfo("Forked Raiju / Fleeting Raiju", "", NIN.JobID)]
		NINPvP_Raiju = 120005,

		[PvPCustomCombo]
		[ParentCombo(NINPvP_Kassatsu)]
		[CustomComboInfo("Hyosho Ranryu", "", NIN.JobID)]
		NINPvP_Hyosho = 120006,

		[PvPCustomCombo]
		[ParentCombo(NINPvP_Kassatsu)]
		[CustomComboInfo("Goka Makkyaku", "", NIN.JobID)]
		NINPvP_Goka = 120007,

		[PvPCustomCombo]
		[ParentCombo(NINPvP_Kassatsu)]
		[CustomComboInfo("Doton", "", NIN.JobID)]
		NINPvP_Doton = 120008,

		[PvPCustomCombo]
		[ParentCombo(NINPvP_Kassatsu)]
		[CustomComboInfo("Huton", "", NIN.JobID)]
		NINPvP_Huton = 120009,

		[PvPCustomCombo]
		[ParentCombo(NINPvP_Kassatsu)]
		[CustomComboInfo("Meisui", "", NIN.JobID)]
		NINPvP_Meisui = 120010,

		[PvPCustomCombo]
		[ParentCombo(NINPvP_Combo)]
		[CustomComboInfo("Seiton Tenchu", "", NIN.JobID)]
		NINPvP_SeitonTenchu = 120011,

		#endregion

		#region PICTOMANCER - 130000



		#endregion

		#region PALADIN - 121000



		#endregion

		#region REAPER - 122000

		[PvPCustomCombo]
		[ReplaceSkill(RPRPvP.Slice, RPRPvP.WaxingSlice, RPRPvP.InfernalSlice)]
		[CustomComboInfo("Combo Mode", "", RPR.JobID)]
		RPRPvP_Combo = 122000,

		[PvPCustomCombo]
		[ParentCombo(RPRPvP_Combo)]
		[CustomComboInfo("Soul Slice", "", RPR.JobID)]
		RPRPvP_SoulSlice = 122001,

		[PvPCustomCombo]
		[ParentCombo(RPRPvP_Combo)]
		[CustomComboInfo("Grim Swathe / Guillotine", "", RPR.JobID)]
		RPRPvP_GrimSwathe = 122002,

		[PvPCustomCombo]
		[ParentCombo(RPRPvP_Combo)]
		[CustomComboInfo("Death Warrant / Harvest Moon", "", RPR.JobID)]
		RPRPvP_DeathWarrant = 122003,

		[PvPCustomCombo]
		[ParentCombo(RPRPvP_Combo)]
		[CustomComboInfo("Plentiful Harvest", "", RPR.JobID)]
		RPRPvP_Plentiful = 122004,

		[PvPCustomCombo]
		[ParentCombo(RPRPvP_Combo)]
		[CustomComboInfo("Enshrouded", "", RPR.JobID)]
		RPRPvP_Enshrouded = 122005,

		[PvPCustomCombo]
		[ParentCombo(RPRPvP_Combo)]
		[CustomComboInfo("Arcane Crest", "", RPR.JobID)]
		RPRPvP_Crest = 122006,

		#endregion

		#region RED MAGE - 123000



		#endregion

		#region SAGE - 124000

		[PvPCustomCombo]
		[ReplaceSkill(SGEPvP.Dosis)]
		[CustomComboInfo("Combo Mode", "", SGE.JobID)]
		SGEPvP_Combo = 124000,

		[PvPCustomCombo]
		[ParentCombo(SGEPvP_Combo)]
		[CustomComboInfo("Kardia", "", SGE.JobID)]
		SGEPvP_Kardia = 124001,

		[PvPCustomCombo]
		[ParentCombo(SGEPvP_Combo)]
		[CustomComboInfo("Eukrasia", "", SGE.JobID)]
		SGEPvP_Eukrasia = 124002,

		[PvPCustomCombo]
		[ParentCombo(SGEPvP_Combo)]
		[CustomComboInfo("Toxikon / Toxikon II", "", SGE.JobID)]
		SGEPvP_Toxikon = 124003,

		[PvPCustomCombo]
		[ParentCombo(SGEPvP_Combo)]
		[CustomComboInfo("Phlegma III", "", SGE.JobID)]
		SGEPvP_Phlegma = 124004,

		[PvPCustomCombo]
		[ParentCombo(SGEPvP_Combo)]
		[CustomComboInfo("Pneuma", "", SGE.JobID)]
		SGEPvP_Pneuma = 124005,

		[PvPCustomCombo]
		[ParentCombo(SGEPvP_Combo)]
		[CustomComboInfo("Mesotes Relocation", "", SGE.JobID)]
		SGEPvP_Mesotes = 124006,

		#endregion

		#region SAMURAI - 125000



		#endregion

		#region SCHOLAR - 126000

		[PvPCustomCombo]
		[ReplaceSkill(SCHPvP.Broil)]
		[CustomComboInfo("Combo Mode", "", SCH.JobID)]
		SCHPvP_Combo = 126000,

		[PvPCustomCombo]
		[ParentCombo(SCHPvP_Combo)]
		[CustomComboInfo("Adloquium", "", SCH.JobID)]
		SCHPvP_Adloquium = 126001,

		[PvPCustomCombo]
		[ParentCombo(SCHPvP_Combo)]
		[CustomComboInfo("Biolysis", "", SCH.JobID)]
		SCHPvP_Bio = 126002,

		[PvPCustomCombo]
		[ParentCombo(SCHPvP_Combo)]
		[CustomComboInfo("Deployment Tactics", "", SCH.JobID)]
		SCHPvP_Deploy = 126003,

		[PvPCustomCombo]
		[ParentCombo(SCHPvP_Combo)]
		[CustomComboInfo("Mummification", "", SCH.JobID)]
		SCHPvP_Mummy = 126004,

		[PvPCustomCombo]
		[ParentCombo(SCHPvP_Combo)]
		[CustomComboInfo("Expedient", "", SCH.JobID)]
		SCHPvP_Expedient = 126005,

		[PvPCustomCombo]
		[ParentCombo(SCHPvP_Combo)]
		[CustomComboInfo("Summon Seraph", "", SCH.JobID)]
		SCHPvP_Seraph = 126006,

		#endregion

		#region SUMMONER - 127000



		#endregion

		#region VIPER - 131000



		#endregion

		#region WARRIOR - 128000



		#endregion

		#region WHITE MAGE - 129000

		[PvPCustomCombo]
		[ReplaceSkill(WHMPvP.Glare)]
		[CustomComboInfo("Combo Mode", "", WHM.JobID)]
		WHMPvP_Combo = 129000,

		[PvPCustomCombo]
		[ParentCombo(WHMPvP_Combo)]
		[CustomComboInfo("Seraph Strike - Only when in melee range", "", WHM.JobID)]
		WHMPvP_Seraph = 129001,

		[PvPCustomCombo]
		[ParentCombo(WHMPvP_Seraph)]
		[CustomComboInfo("Cure 3", "", WHM.JobID)]
		WHMPvP_Cure3 = 129002,

		[PvPCustomCombo]
		[ParentCombo(WHMPvP_Combo)]
		[CustomComboInfo("Afflatus Misery", "", WHM.JobID)]
		WHMPvP_Misery = 129003,

		[PvPCustomCombo]
		[ParentCombo(WHMPvP_Combo)]
		[CustomComboInfo("Aquaveil", "", WHM.JobID)]
		WHMPvP_Aquaveil = 129004,

		[PvPCustomCombo]
		[ParentCombo(WHMPvP_Combo)]
		[CustomComboInfo("Miracle of Nature", "", WHM.JobID)]
		WHMPvP_Polymorph = 129005,

		#endregion

		#endregion

		#region Extra

		#region REQUIRED - 0

		[CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled", ADV.JobID)]
		AdvAny = 0,

		[CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled", FSH.JobID)]
		DolAny = AdvAny + FSH.JobID,

		[CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled", PLD.JobID)]
		PldAny = AdvAny + PLD.JobID,

		[CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled", WAR.JobID)]
		WarAny = AdvAny + WAR.JobID,

		[CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled", DRK.JobID)]
		DrkAny = AdvAny + DRK.JobID,

		[CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled", GNB.JobID)]
		GnbAny = AdvAny + GNB.JobID,

		[CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled", WHM.JobID)]
		WhmAny = AdvAny + WHM.JobID,

		[CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled", SCH.JobID)]
		SchAny = AdvAny + SCH.JobID,

		[CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled", AST.JobID)]
		AstAny = AdvAny + AST.JobID,

		[CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled", SGE.JobID)]
		SgeAny = AdvAny + SGE.JobID,

		[CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled", MNK.JobID)]
		MnkAny = AdvAny + MNK.JobID,

		[CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled", DRG.JobID)]
		DrgAny = AdvAny + DRG.JobID,

		[CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled", NIN.JobID)]
		NinAny = AdvAny + NIN.JobID,

		[CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled", SAM.JobID)]
		SamAny = AdvAny + SAM.JobID,

		[CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled", RPR.JobID)]
		RprAny = AdvAny + RPR.JobID,

		[CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled", VPR.JobID)]
		VprAny = AdvAny + VPR.JobID,

		[CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled", BRD.JobID)]
		BrdAny = AdvAny + BRD.JobID,

		[CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled", MCH.JobID)]
		MchAny = AdvAny + MCH.JobID,

		[CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled", DNC.JobID)]
		DncAny = AdvAny + DNC.JobID,

		[CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled", BLM.JobID)]
		BlmAny = AdvAny + BLM.JobID,

		[CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled", SMN.JobID)]
		SmnAny = AdvAny + SMN.JobID,

		[CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled", RDM.JobID)]
		RdmAny = AdvAny + RDM.JobID,

		[CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled", PCT.JobID)]
		PctAny = AdvAny + PCT.JobID,

		[CustomComboInfo("Any", "This should not be displayed. This always returns true when used with IsEnabled", BLU.JobID)]
		BluAny = AdvAny + BLU.JobID,

		[CustomComboInfo("Disabled", "This should not be used", ADV.JobID)]
		Disabled = 99999,

		#endregion

		#region FISHING - 51000

		[CustomComboInfo("Fishing skills to Spearfishing skills when underwater\n ", "", FSH.JobID)]
		FSH_Swim = 51008,

		[ReplaceSkill(FSH.Cast, FSH.Gig)]
		[ParentCombo(FSH_Swim)]
		[CustomComboInfo("Cast to Gig", "", FSH.JobID)]
		FSH_CastGig = 51003,

		[ReplaceSkill(FSH.SurfaceSlap, FSH.VeteranTrade)]
		[ParentCombo(FSH_Swim)]
		[CustomComboInfo("Surface Slap to Veteran Trade", "", FSH.JobID)]
		FSH_SurfaceTrade = 51004,

		[ReplaceSkill(FSH.PrizeCatch, FSH.NaturesBounty)]
		[ParentCombo(FSH_Swim)]
		[CustomComboInfo("Prize Catch to Nature's Bounty", "", FSH.JobID)]
		FSH_PrizeBounty = 51005,

		[ReplaceSkill(FSH.Snagging, FSH.Salvage)]
		[ParentCombo(FSH_Swim)]
		[CustomComboInfo("Snagging to Salvage", "", FSH.JobID)]
		FSH_SnaggingSalvage = 51006,

		[ReplaceSkill(FSH.CastLight, FSH.ElectricCurrent)]
		[ParentCombo(FSH_Swim)]
		[CustomComboInfo("Cast Light to Electric Current", "", FSH.JobID)]
		FSH_CastLight_ElectricCurrent = 51007,

		[ReplaceSkill(FSH.Mooch, FSH.SharkEye)]
		[ParentCombo(FSH_Swim)]
		[CustomComboInfo("Mooch to Shark Eye", "", FSH.JobID)]
		FSH_Mooch_SharkEye = 51009,

		[ReplaceSkill(FSH.FishEyes, FSH.VitalSight)]
		[ParentCombo(FSH_Swim)]
		[CustomComboInfo("Fish Eyes to Vital Sight", "", FSH.JobID)]
		FSH_FishEyes_VitalSight = 51010,

		[ReplaceSkill(FSH.Chum, FSH.BaitedBreath)]
		[ParentCombo(FSH_Swim)]
		[CustomComboInfo("Chum to Baited Breath", "", FSH.JobID)]
		FSH_Chum_BaitedBreath = 51011,

		#endregion

		#region ALL - 100000

		[ReplaceSkill(All.Reprisal)]
		[CustomComboInfo("Reprisal Protection", "", ADV.JobID)]
		ALL_Tank_Reprisal = 100001,

		[ReplaceSkill(All.Feint)]
		[CustomComboInfo("Feint Protection", "", ADV.JobID)]
		ALL_Melee_Feint = 100002,

		[ReplaceSkill(All.TrueNorth)]
		[CustomComboInfo("True North Protection", "", ADV.JobID)]
		ALL_Melee_TrueNorth = 100003,

		[ReplaceSkill(MCH.Tactician, BRD.Troubadour, DNC.ShieldSamba)]
		[CustomComboInfo("Ranged Mitigation Protection", "", ADV.JobID)]
		ALL_Ranged_Mitigation = 100004,

		[ReplaceSkill(All.Addle)]
		[CustomComboInfo("Addle Protection", "", ADV.JobID)]
		ALL_Caster_Addle = 100005,

		[ReplaceSkill(WHM.Raise, SCH.Resurrection, AST.Ascend, SGE.Egeiro, RDM.Verraise, BLU.AngelWhisper)]
		[CustomComboInfo("Raise Protection", "", ADV.JobID)]
		ALL_Raise_Protection = 100006,

		#endregion

		#region PvP GLOBALS - 1100000
		[PvPCustomCombo]
		[CustomComboInfo("Emergency Heals %", "", ADV.JobID)]
		PvP_EmergencyHeals = 1100000,

		[PvPCustomCombo]
		[CustomComboInfo("Emergency Guard %", "", ADV.JobID)]
		PvP_EmergencyGuard = 1100010,

		[PvPCustomCombo]
		[CustomComboInfo("Purify", "", ADV.JobID)]
		PvP_QuickPurify = 1100020,

		#endregion

		#endregion
	}
}