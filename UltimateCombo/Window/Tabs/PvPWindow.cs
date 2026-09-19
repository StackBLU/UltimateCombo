using System.Collections.Generic;
using UltimateCombo.Attributes;
using UltimateCombo.Combos;
using UltimateCombo.Combos.PvE;
using UltimateCombo.Core;

namespace UltimateCombo.Window.Tabs;

internal class PvPWindow : ConfigWindow
{
    internal static bool HasToOpenJob = true;
    internal static string OpenJob = string.Empty;

    private static readonly JobTab.JobRole[][] JobColumns =
    [
        [
            new("Tanks", [PLD.JobID, WAR.JobID, DRK.JobID, GNB.JobID]),
            new("Melee", [MNK.JobID, DRG.JobID, NIN.JobID, SAM.JobID, RPR.JobID, VPR.JobID])
        ],
        [
            new("Healers", [WHM.JobID, SCH.JobID, AST.JobID, SGE.JobID]),
            new("Physical Ranged", [BRD.JobID, MCH.JobID, DNC.JobID]),
            new("Magical Ranged", [BLM.JobID, SMN.JobID, RDM.JobID, PCT.JobID])
        ]
    ];

    internal static new void Draw()
    {
        JobTab.Draw(JobColumns, PresetStorage.IsPvP, ref OpenJob, DrawHeadingContents);
    }

    private static void DrawHeadingContents(string jobName)
    {
        if (!GroupedPresets.TryGetValue(jobName, out List<(Presets Preset, CustomComboInfoAttribute Info)>? presets))
        {
            return;
        }

        var index = 1;

        foreach ((Presets preset, Attributes.CustomComboInfoAttribute info) in presets)
        {
            if (!PresetStorage.IsPvP(preset))
            {
                continue;
            }

            index = PresetTab.DrawPresetBox(preset, info, index, hideEnabledConflicts: true);
        }
    }
}
