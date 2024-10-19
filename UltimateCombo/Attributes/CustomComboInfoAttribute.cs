using ECommons.DalamudServices;
using Lumina.Excel.GeneratedSheets;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using UltimateCombo.Combos.PvE;
using UltimateCombo.Services;

namespace UltimateCombo.Attributes
{
	/// <summary> Attribute documenting additional information for each combo. </summary>
	[AttributeUsage(AttributeTargets.Field)]
	internal class CustomComboInfoAttribute : Attribute
	{
		/// <summary> Initializes a new instance of the <see cref="CustomComboInfoAttribute"/> class. </summary>
		/// <param name="fancyName"> Display name. </param>
		/// <param name="description"> Combo description. </param>
		/// <param name="jobID"> Associated job ID. </param>
		/// <param name="order"> Display order. </param>
		/// <param name="memeName"> Display meme name </param>
		/// <param name="memeDescription"> Meme description. </param>
		internal CustomComboInfoAttribute(string fancyName, string description, byte jobID, [CallerLineNumber] int order = 0, string memeName = "", string memeDescription = "")
		{
			FancyName = fancyName;
			Description = description;
			JobID = jobID;
			Order = order;
			MemeName = memeName;
			MemeDescription = memeDescription;
		}

		/// <summary> Gets the display name. </summary>
		public string FancyName { get; }

		///<summary> Gets the meme name. </summary>
		public string MemeName { get; }

		/// <summary> Gets the description. </summary>
		public string Description { get; }

		/// <summary> Gets the meme description. </summary>
		public string MemeDescription { get; }

		/// <summary> Gets the job ID. </summary>
		public byte JobID { get; }

		/// <summary> Gets the job role. </summary>
		public int Role
		{
			get
			{
				return JobIDToRole(JobID);
			}
		}

		public uint ClassJobCategory
		{
			get
			{
				return JobIDToClassJobCategory(JobID);
			}
		}

		private static int JobIDToRole(byte jobID)
		{
			return Svc.Data.GetExcelSheet<ClassJob>().HasRow(jobID) ? Svc.Data.GetExcelSheet<ClassJob>().GetRow(jobID).Role : 0;
		}

		private static uint JobIDToClassJobCategory(byte jobID)
		{
			return Svc.Data.GetExcelSheet<ClassJob>().HasRow(jobID) ? Svc.Data.GetExcelSheet<ClassJob>().GetRow(jobID).ClassJobCategory.Row : 0;
		}

		/// <summary> Gets the display order. </summary>
		public int Order { get; }

		/// <summary> Gets the job name. </summary>
		public string JobName
		{
			get
			{
				return JobIDToName(JobID);
			}
		}

		public string JobShorthand
		{
			get
			{
				return JobIDToShorthand(JobID);
			}
		}

		private static string JobIDToShorthand(byte key)
		{
			return key == 41 ? "VPR" : key == 0 ? "" : ClassJobs.TryGetValue(key, out ClassJob? job) ? job.Abbreviation.RawString : "";
		}

		private static readonly Dictionary<uint, ClassJob> ClassJobs = Service.DataManager.GetExcelSheet<ClassJob>()!.ToDictionary(i => i.RowId, i => i);

		public static string JobIDToName(byte key)
		{
			if (key is 0)
			{
				return "General";
			}

			if (key is FSH.JobID)
			{
				return "Fisher";
			}

			if (ClassJobs.TryGetValue(key, out ClassJob? job))
			{
				//Grab Category name for DOH/DOL, else the normal Name for the rest
				string jobname = key is 08 or 16 ? job.ClassJobCategory.Value.Name : job.Name;
				//Job names are all lowercase by default. This capitalizes based on regional rules
				string cultureID = Service.ClientState.ClientLanguage switch
				{
					Dalamud.Game.ClientLanguage.French => "fr-FR",
					Dalamud.Game.ClientLanguage.Japanese => "ja-JP",
					Dalamud.Game.ClientLanguage.German => "de-DE",
					_ => "en-us",
				};
				TextInfo textInfo = new CultureInfo(cultureID, false).TextInfo;
				jobname = textInfo.ToTitleCase(jobname);
				//if (key is 0) jobname = " " + jobname; //Adding space to the front of Global moves it to the top. Shit hack but works
				return jobname;

			} //Misc or unknown
			else
			{
				return key == 99 ? "Global" : "Unknown";
			}
		}

		/// <summary> Gets the meme job name. </summary>
		public string MemeJobName
		{
			get
			{
				return MemeJobIDToName(JobID);
			}
		}

		private static string MemeJobIDToName(byte key)
		{
			return key switch
			{
				0 => "Adventurer",
				1 => "Gladiator",
				2 => "Pugilist",
				3 => "Marauder",
				4 => "Lancer",
				5 => "Archer",
				6 => "Conjurer",
				7 => "Thaumaturge",
				8 => "Carpenter",
				9 => "Blacksmith",
				10 => "Armorer",
				11 => "Goldsmith",
				12 => "Leatherworker",
				13 => "Weaver",
				14 => "Alchemist",
				15 => "Culinarian",
				16 => "Miner",
				17 => "Botanist",
				18 => "Fisher",
				19 => "Paladin",
				20 => "Monk",
				21 => "Warrior",
				22 => "Dragoon",
				23 => "Bard",
				24 => "White Mage",
				25 => "Black Mage",
				26 => "Arcanist",
				27 => "Summoner",
				28 => "Scholar",
				29 => "Rogue",
				30 => "Ninja",
				31 => "Machinist",
				32 => "Dark Knight",
				33 => "Astrologian",
				34 => "Samurai",
				35 => "Red Mage",
				36 => "Blue Mage",
				37 => "Gunbreaker",
				38 => "Dancer",
				39 => "Reaper",
				40 => "Sage",
				99 => "Global",
				_ => "Unknown",
			};
		}
	}
}
