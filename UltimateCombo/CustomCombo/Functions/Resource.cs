using Dalamud.Game.ClientState.JobGauge.Types;

using FFXIVClientStructs.FFXIV.Client.Game.UI;

using UltimateCombo.Data;
using UltimateCombo.Services;

namespace UltimateCombo.ComboHelper.Functions
{
    internal abstract partial class CustomComboFunctions
    {
        /// <summary> Gets the Resource Cost of the action. </summary>
        /// <param name="actionID"> Action ID to check. </param>
        /// <returns></returns>
        public static int GetResourceCost(uint actionID)
        {
            return CustomComboCache.GetResourceCost(actionID);
        }

        /// <summary> Gets the Resource Type of the action. </summary>
        /// <param name="actionID"> Action ID to check. </param>
        /// <returns></returns>
        public static bool IsResourceTypeNormal(uint actionID)
        {
            return CustomComboCache.GetResourceCost(actionID) is >= 100 or 0;
        }

        /// <summary> Get a job gauge. </summary>
        /// <typeparam name="T"> Type of job gauge.</typeparam>
        /// <returns> The job gauge. </returns>
        public static T GetJobGauge<T>() where T : JobGaugeBase
        {
            return Service.ComboCache.GetJobGauge<T>();
        }

        public unsafe ushort GetLimitBreakCurrentValue()
        {
            return UIState.Instance()->LimitBreakController.CurrentUnits;
        }

        public unsafe uint GetLimitBreakMaxValue()
        {
            return UIState.Instance()->LimitBreakController.BarUnits;
        }

        public unsafe byte GetLimitBreakBarCount()
        {
            return UIState.Instance()->LimitBreakController.BarCount;
        }
    }
}
