using ECommons.DalamudServices;
using Lumina.Excel.Sheets;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UltimateCombo.Combos.General;
using UltimateCombo.Core;

namespace UltimateCombo.Attributes;

[AttributeUsage(AttributeTargets.Field)]
internal class CustomComboInfoAttribute : Attribute
{
    internal static Dictionary<uint, ClassJob> ClassSheet = Service.DataManager.GetExcelSheet<ClassJob>()!.ToDictionary(i => i.RowId, i => i);
    private static readonly Dictionary<uint, ClassJob> ClassJobs = Service.DataManager.GetExcelSheet<ClassJob>()!.ToDictionary(i => i.RowId, i => i);

    internal CustomComboInfoAttribute(string fancyName, string description, byte jobID)
    {
        FancyName = fancyName;
        Description = description;
        JobID = jobID;
    }

    internal string FancyName { get; }
    internal string Description { get; }
    internal byte JobID { get; }
    internal int Role => JobIDToRole(JobID);
    internal uint ClassJobCategory => JobIDToClassJobCategory(JobID);
    internal int Order { get; }
    internal string JobName => JobIDToName(JobID);
    internal string JobShorthand => JobIDToShorthand(JobID);

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

    internal static string JobIDToName(byte key)
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
            var jobname = key is 08 or 16
                ? job.ClassJobCategory.Value.Name.ToString()
                : job.Name.ToString();

            var cultureID = Service.ClientState.ClientLanguage switch
            {
                Dalamud.Game.ClientLanguage.French => "fr-FR",
                Dalamud.Game.ClientLanguage.Japanese => "ja-JP",
                Dalamud.Game.ClientLanguage.German => "de-DE",
                Dalamud.Game.ClientLanguage.English => "en-us",
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
