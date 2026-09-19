using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Combos;
using UltimateCombo.Combos.General;

namespace UltimateCombo.Core;

internal abstract partial class CustomComboBase : CustomComboFunctions
{
    protected internal abstract Presets Preset { get; }
    private byte _jobID;

    internal int ProfilerIndex { get; set; }

    internal void InitializeJobID(byte jobID)
    {
        _jobID = jobID;
    }

    internal bool AppliesToJob(uint classJobID)
    {
        // Base class IDs differ from their job IDs, so keep their existing behavior.
        return classJobID < 19 || _jobID == Common.JobID || _jobID == classJobID;
    }

    internal bool TryInvoke(uint actionID, out uint newActionID)
    {
        newActionID = 0;

        if (!IsEnabled(Preset))
        {
            return false;
        }

        var classJobID = IconReplacer.ThreadSafeCache.LocalPlayerClassJobID;
        if (!AppliesToJob(classJobID))
        {
            return false;
        }

        var resultingActionID = Invoke(actionID);
        if (resultingActionID == 0 || resultingActionID == actionID)
        {
            return false;
        }

        newActionID = resultingActionID;
        return true;
    }

    protected abstract uint Invoke(uint actionID);
}
