using Lumina.Excel.Sheets;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UltimateCombo.Combos.General;
using UltimateCombo.Core;

namespace UltimateCombo.Attributes;

[AttributeUsage(AttributeTargets.Field)]
internal sealed class CustomComboInfoAttribute : Attribute
{
    private static readonly Dictionary<uint, ClassJob> ClassJobs = Service.DataManager
        .GetExcelSheet<ClassJob>()
        .ToDictionary(job => job.RowId, job => job);

    internal CustomComboInfoAttribute(string fancyName, byte jobID)
    {
        FancyName = fancyName;
        JobID = jobID;
        Role = JobIDToRole(jobID);
        ClassJobCategory = JobIDToClassJobCategory(jobID);
        JobName = JobIDToName(jobID);
        JobShorthand = JobIDToShorthand(jobID);
    }

    internal string FancyName { get; }
    internal byte JobID { get; }
    internal int Role { get; }
    internal uint ClassJobCategory { get; }
    internal string JobName { get; }
    internal string JobShorthand { get; }

    private static int JobIDToRole(byte jobID)
    {
        return ClassJobs.TryGetValue(jobID, out ClassJob job) ? job.Role : 0;
    }

    private static uint JobIDToClassJobCategory(byte jobID)
    {
        return ClassJobs.TryGetValue(jobID, out ClassJob job)
            ? job.ClassJobCategory.Value.RowId
            : 0;
    }

    private static string JobIDToShorthand(byte jobID)
    {
        if (jobID == 0)
        {
            return "";
        }

        return ClassJobs.TryGetValue(jobID, out ClassJob job)
            ? job.Abbreviation.ToString()
            : "";
    }

    internal static string JobIDToName(byte jobID)
    {
        if (jobID == 0)
        {
            return "General";
        }

        if (jobID == FSH.JobID)
        {
            return "Fisher";
        }

        if (jobID == 99)
        {
            return "Global";
        }

        if (!ClassJobs.TryGetValue(jobID, out ClassJob job))
        {
            return "Unknown";
        }

        var jobName = jobID is 8 or 16
            ? job.ClassJobCategory.Value.Name.ToString()
            : job.Name.ToString();

        var cultureName = Service.ClientState.ClientLanguage switch
        {
            Dalamud.Game.ClientLanguage.French => "fr-FR",
            Dalamud.Game.ClientLanguage.Japanese => "ja-JP",
            Dalamud.Game.ClientLanguage.German => "de-DE",
            Dalamud.Game.ClientLanguage.English => "en-US",
            _ => "en-US"
        };

        TextInfo textInfo = CultureInfo.GetCultureInfo(cultureName).TextInfo;
        return textInfo.ToTitleCase(jobName);
    }
}
