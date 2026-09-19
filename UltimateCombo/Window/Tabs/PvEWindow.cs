using System.Collections.Generic;
using UltimateCombo.Attributes;
using UltimateCombo.Combos;
using UltimateCombo.Combos.General;
using UltimateCombo.Combos.PvE;
using UltimateCombo.Core;

namespace UltimateCombo.Window.Tabs;

internal class PvEWindow : ConfigWindow
{
    internal static bool HasToOpenJob = true;
    internal static string OpenJob = string.Empty;
    internal static string? HeaderToOpen;

    private static readonly JobTab.JobRole[][] JobColumns =
    [
        [
            new("Tanks", [PLD.JobID, WAR.JobID, DRK.JobID, GNB.JobID]),
            new("Melee", [MNK.JobID, DRG.JobID, NIN.JobID, SAM.JobID, RPR.JobID, VPR.JobID, BST.JobID]),
            new("Other", [FSH.JobID, Common.JobID])
        ],
        [
            new("Healers", [WHM.JobID, SCH.JobID, AST.JobID, SGE.JobID]),
            new("Physical Ranged", [BRD.JobID, MCH.JobID, DNC.JobID]),
            new("Magical Ranged", [BLM.JobID, SMN.JobID, RDM.JobID, PCT.JobID, BLU.JobID])
        ]
    ];

    internal static new void Draw()
    {
        JobTab.Draw(JobColumns, IsPvEPreset, ref OpenJob, DrawHeadingContents);
    }

    internal static void DrawHeadingContents(string jobName)
    {
        DrawHeadingContents(jobName, 1);
    }

    internal static void DrawHeadingContents(string jobName, int index)
    {
        if (!GroupedPresets.TryGetValue(jobName, out List<(Presets Preset, CustomComboInfoAttribute Info)>? presets))
        {
            return;
        }

        foreach ((Presets preset, Attributes.CustomComboInfoAttribute info) in presets)
        {
            if (!IsPvEPreset(preset))
            {
                continue;
            }

            index = PresetTab.DrawPresetBox(preset, info, index, hideEnabledConflicts: true);
        }
    }

    private static bool IsPvEPreset(Presets preset)
    {
        return !PresetStorage.IsPvP(preset)
            && !PresetStorage.IsBozja(preset)
            && !PresetStorage.IsEureka(preset)
            && !PresetStorage.IsVariant(preset)
            && !PresetStorage.IsOccult(preset);
    }
}
