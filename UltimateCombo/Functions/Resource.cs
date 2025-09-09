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

    internal static bool IsResourceTypeNormal(uint actionID)
    {
        return CustomComboCache.GetResourceCost(actionID) is >= 100 or 0;
    }

    internal static T GetJobGauge<T>() where T : JobGaugeBase
    {
        return Service.ComboCache.GetJobGauge<T>();
    }

    internal unsafe ushort GetLimitBreakCurrentValue()
    {
        return UIState.Instance()->LimitBreakController.CurrentUnits;
    }

    internal unsafe uint GetLimitBreakMaxValue()
    {
        return UIState.Instance()->LimitBreakController.BarUnits;
    }

    internal unsafe byte GetLimitBreakBarCount()
    {
        return UIState.Instance()->LimitBreakController.BarCount;
    }
}
