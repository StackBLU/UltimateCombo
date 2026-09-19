using FFXIVClientStructs.FFXIV.Client.Game;
using System;

namespace UltimateCombo.Data;

internal class CooldownData
{
    internal uint ActionID { get; init; }
    internal bool IsCooldown => CooldownRemaining > 0;

    internal unsafe float CooldownElapsed
    {
        get
        {
            ActionManager* actionManager = ActionManager.Instance();
            return actionManager == null
                ? 0
                : actionManager->GetRecastTimeElapsed(ActionType.Action, ActionID);
        }
    }

    internal float CooldownTotal => ActionManager.GetAdjustedRecastTime(ActionType.Action, ActionID) / 1000f * MaxCharges;
    internal unsafe float AdjustedCastTime => ActionManager.GetAdjustedCastTime(ActionType.Action, ActionID) / 1000f;

    internal float CooldownRemaining
    {
        get
        {
            var elapsed = CooldownElapsed;
            return elapsed <= 0 ? 0 : MathF.Max(0, CooldownTotal - elapsed);
        }
    }

    internal ushort MaxCharges => ActionManager.GetMaxCharges(ActionID, 0);
    internal bool HasCharges => MaxCharges > 1;

    internal unsafe uint RemainingCharges
    {
        get
        {
            var maxCharges = MaxCharges;
            if (maxCharges == 0)
            {
                return 0;
            }

            if (maxCharges == 1)
            {
                return CooldownRemaining == 0 ? 1u : 0u;
            }

            ActionManager* actionManager = ActionManager.Instance();
            return actionManager == null ? 0 : actionManager->GetCurrentCharges(ActionID);
        }
    }

    internal float ChargeCooldownRemaining
    {
        get
        {
            var maxCharges = MaxCharges;
            var total = CooldownTotal;
            var remaining = CooldownRemaining;

            if (maxCharges == 0 || total <= 0 || remaining <= 0)
            {
                return 0;
            }

            var cooldownPerCharge = total / maxCharges;
            var chargeRemaining = remaining % cooldownPerCharge;

            return chargeRemaining <= 0.001f ? cooldownPerCharge : chargeRemaining;
        }
    }
}
