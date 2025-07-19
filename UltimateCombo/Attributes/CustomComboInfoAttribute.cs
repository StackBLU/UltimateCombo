using ECommons.DalamudServices;
using Lumina.Excel.Sheets;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using UltimateCombo.Combos.PvE;
using UltimateCombo.Services;

namespace UltimateCombo.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    internal class CustomComboInfoAttribute : Attribute
    {
        internal CustomComboInfoAttribute(string fancyName, string description, byte jobID, [CallerLineNumber] int order = 0, string memeName = "", string memeDescription = "")
        {
            FancyName = fancyName;
            Description = description;
            JobID = jobID;
            Order = order;
            MemeName = memeName;
            MemeDescription = memeDescription;
        }

        internal static Dictionary<uint, ClassJob> ClassSheet = Service.DataManager.GetExcelSheet<ClassJob>()!.ToDictionary(i => i.RowId, i => i);

        public string FancyName { get; }

        public string MemeName { get; }

        public string Description { get; }

        public string MemeDescription { get; }

        public byte JobID { get; }

        public int Role => JobIDToRole(JobID);

        public uint ClassJobCategory => JobIDToClassJobCategory(JobID);

        private static int JobIDToRole(byte jobID)
        {
            if (Svc.Data.GetExcelSheet<ClassJob>().HasRow(jobID))
            {
                return Svc.Data.GetExcelSheet<ClassJob>().GetRow(jobID).Role;
            }

            return 0;
        }

        private static uint JobIDToClassJobCategory(byte jobID)
        {
            if (Svc.Data.GetExcelSheet<ClassJob>().HasRow(jobID))
            {
                return Svc.Data.GetExcelSheet<ClassJob>().GetRow(jobID).ClassJobCategory.Value.RowId;
            }

            return 0;
        }

        public int Order { get; }

        public string JobName => JobIDToName(JobID);

        public string JobShorthand => JobIDToShorthand(JobID);

        private static string JobIDToShorthand(byte key)
        {
            if (key == 0)
            {
                return "";
            }

            if (ClassSheet.TryGetValue(key, out ClassJob job))
            {
                return job.Abbreviation.ToString();
            }

            return "";
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

            if (ClassJobs.TryGetValue(key, out ClassJob job))
            {
                string jobname;

                if (key is 08 or 16)
                {
                    jobname = job.ClassJobCategory.Value.Name.ToString();
                }

                else
                {
                    jobname = job.Name.ToString();
                }

                var cultureID = Service.ClientState.ClientLanguage switch
                {
                    Dalamud.Game.ClientLanguage.French => "fr-FR",
                    Dalamud.Game.ClientLanguage.Japanese => "ja-JP",
                    Dalamud.Game.ClientLanguage.German => "de-DE",
                    _ => "en-us",
                };

                TextInfo textInfo = new CultureInfo(cultureID, false).TextInfo;
                jobname = textInfo.ToTitleCase(jobname);
                return jobname;
            }

            else
            {
                if (key == 99)
                {
                    return "Global";
                }

                return "Unknown";
            }
        }
    }
}
