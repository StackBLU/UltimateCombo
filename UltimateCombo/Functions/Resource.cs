using Dalamud.Game.ClientState.JobGauge.Types;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using UltimateCombo.Core;
using UltimateCombo.Data;

namespace UltimateCombo.ComboHelper.Functions;

internal abstract partial class CustomComboFunctions
{
    internal static int GetResourceCost(uint actionID)
    {
        return CustomComboCache.GetResourceCost(actionID);
    }

    internal static T GetJobGauge<T>() where T : JobGaugeBase
    {
        return Service.ComboCache.GetJobGauge<T>();
    }

    internal static unsafe ushort GetLimitBreakCurrentValue()
    {
        UIState* uiState = UIState.Instance();
        return uiState == null ? (ushort) 0 : uiState->LimitBreakController.CurrentUnits;
    }

    internal static unsafe uint GetLimitBreakMaxValue()
    {
        UIState* uiState = UIState.Instance();
        return uiState == null ? 0u : uiState->LimitBreakController.BarUnits;
    }
}
