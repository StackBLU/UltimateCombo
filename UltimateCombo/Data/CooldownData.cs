using FFXIVClientStructs.FFXIV.Client.Game;
using System;

namespace UltimateCombo.Data;

internal class CooldownData
{
    internal bool IsCooldown => CooldownRemaining > 0;
    internal uint ActionID;
    internal unsafe float CooldownElapsed => ActionManager.Instance()->GetRecastTimeElapsed(ActionType.Action, ActionID);
    internal unsafe float CooldownTotal => ActionManager.GetAdjustedRecastTime(ActionType.Action, ActionID) / 1000f * MaxCharges;
    internal unsafe float AdjustedCastTime => ActionManager.GetAdjustedCastTime(ActionType.Action, ActionID) / 1000f;

    internal unsafe float CooldownRemaining
    {
        get
        {
            if (CooldownElapsed == 0)
            {
                return 0;
            }

            return Math.Max(0, CooldownTotal - CooldownElapsed);
        }
    }

    internal ushort MaxCharges => ActionManager.GetMaxCharges(ActionID, 0);
    internal bool HasCharges => MaxCharges > 1;

    internal unsafe uint RemainingCharges
    {
        get
        {
            if (MaxCharges == 1)
            {
                if (CooldownRemaining == 0)
                {
                    return 1;
                }

                return 0u;
            }
            return ActionManager.Instance()->GetCurrentCharges(ActionID);
        }
    }

    internal float ChargeCooldownRemaining => CooldownRemaining % (CooldownTotal / MaxCharges);
}
