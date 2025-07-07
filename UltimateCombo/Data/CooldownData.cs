using System;

using FFXIVClientStructs.FFXIV.Client.Game;

namespace UltimateCombo.Data
{
    internal class CooldownData
    {
        /// <summary> Gets a value indicating whether the action is on cooldown. </summary>
        public bool IsCooldown => CooldownRemaining > 0;

        /// <summary> Gets the action ID on cooldown. </summary>
        public uint ActionID;

        /// <summary> Gets the elapsed cooldown time. </summary>
        public unsafe float CooldownElapsed => ActionManager.Instance()->GetRecastTimeElapsed(ActionType.Action, ActionID);

        /// <summary> Gets the total cooldown time. </summary>
        public unsafe float CooldownTotal => ActionManager.GetAdjustedRecastTime(ActionType.Action, ActionID) / 1000f * MaxCharges;

        /// <summary> Gets the total cooldown time. </summary>
        public unsafe float AdjustedCastTime => ActionManager.GetAdjustedCastTime(ActionType.Action, ActionID) / 1000f;

        /// <summary> Gets the cooldown time remaining. </summary>
        public unsafe float CooldownRemaining => CooldownElapsed == 0 ? 0 : Math.Max(0, CooldownTotal - CooldownElapsed);

        /// <summary> Gets the maximum number of charges for an action at the current level. </summary>
        /// <returns> Number of charges. </returns>
        public ushort MaxCharges => ActionManager.GetMaxCharges(ActionID, 0);

        /// <summary> Gets a value indicating whether the action has charges, not charges available. </summary>
        public bool HasCharges => MaxCharges > 1;

        /// <summary> Gets the remaining number of charges for an action. </summary>
        public unsafe uint RemainingCharges => MaxCharges == 1 ? CooldownRemaining == 0 ? 1 : 0u : ActionManager.Instance()->GetCurrentCharges(ActionID);

        /// <summary> Gets the cooldown time remaining until the next charge. </summary>
        public float ChargeCooldownRemaining => CooldownRemaining % (CooldownTotal / MaxCharges);
    }
}