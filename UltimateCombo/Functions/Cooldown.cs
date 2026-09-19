using FFXIVClientStructs.FFXIV.Client.Game;
using System;
using UltimateCombo.Core;
using UltimateCombo.Data;

namespace UltimateCombo.ComboHelper.Functions;

internal abstract partial class CustomComboFunctions
{
    internal static CooldownData GetCooldown(uint actionID)
    {
        return Service.ComboCache.GetCooldown(actionID);
    }

    internal static float GetCooldownRemainingTime(uint actionID)
    {
        return GetCooldown(actionID).CooldownRemaining;
    }

    internal static float GetCooldownChargeRemainingTime(uint actionID)
    {
        return GetCooldown(actionID).ChargeCooldownRemaining;
    }

    internal static float GetCooldownElapsed(uint actionID)
    {
        return GetCooldown(actionID).CooldownElapsed;
    }

    internal static bool IsOnCooldown(uint actionID)
    {
        return GetCooldown(actionID).IsCooldown;
    }

    internal static bool IsOffCooldown(uint actionID)
    {
        return !IsOnCooldown(actionID);
    }

    internal static bool HasCharges(uint actionID)
    {
        return GetCooldown(actionID).RemainingCharges > 0;
    }

    internal static uint GetRemainingCharges(uint actionID)
    {
        return GetCooldown(actionID).RemainingCharges;
    }

    internal static ushort GetMaxCharges(uint actionID)
    {
        return GetCooldown(actionID).MaxCharges;
    }

    internal static unsafe bool IsActionEnabled(uint actionID)
    {
        ActionManager* actionManager = ActionManager.Instance();
        return actionManager != null && actionManager->GetActionStatus(ActionType.Action, actionID) == 0;
    }

    internal static unsafe float RemainingGCD
    {
        get
        {
            ActionManager* actionManager = ActionManager.Instance();
            if (actionManager == null)
            {
                return 0;
            }

            RecastDetail* recastGCD = actionManager->GetRecastGroupDetail(57);
            return recastGCD == null ? 0 : MathF.Max(0, recastGCD->Total - recastGCD->Elapsed);
        }
    }
}
