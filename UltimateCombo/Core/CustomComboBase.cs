using ECommons.DalamudServices;
using FFXIVClientStructs.FFXIV.Client.Game;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Combos;
using UltimateCombo.Combos.General;

namespace UltimateCombo.Core;

internal abstract partial class CustomComboBase : CustomComboFunctions
{
    protected internal abstract Presets Preset { get; }
    protected byte ClassID { get; }
    protected byte JobID { get; }

    internal unsafe bool TryInvoke(uint actionID, uint lastComboMove, out uint newActionID)
    {
        newActionID = 0;
        if (ActionManager.Instance()->QueuedActionType == ActionType.Action
            && ActionManager.Instance()->QueuedActionId != actionID
            && !Svc.ClientState.IsPvP)
        {
            return false;
        }

        if (!IsEnabled(Preset))
        {
            return false;
        }

        var classJobID = LocalPlayer!.ClassJob.Value.RowId;
        if (JobID != Common.JobID && JobID != classJobID && ClassID != classJobID)
        {
            return false;
        }

        var resultingActionID = Invoke(actionID, lastComboMove);
        if (resultingActionID == 0 || actionID == resultingActionID)
        {
            return false;
        }

        newActionID = resultingActionID;
        return true;
    }

    protected abstract uint Invoke(uint actionID, uint lastComboActionID);
}
