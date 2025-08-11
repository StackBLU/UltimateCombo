using FFXIVClientStructs.FFXIV.Client.Game;
using System;

namespace UltimateCombo.Data
{
	internal class CooldownData
	{
		public bool IsCooldown => CooldownRemaining > 0;

		public uint ActionID;

		public unsafe float CooldownElapsed => ActionManager.Instance()->GetRecastTimeElapsed(ActionType.Action, ActionID);

		public unsafe float CooldownTotal => ActionManager.GetAdjustedRecastTime(ActionType.Action, ActionID) / 1000f * MaxCharges;

		public unsafe float AdjustedCastTime => ActionManager.GetAdjustedCastTime(ActionType.Action, ActionID) / 1000f;

		public unsafe float CooldownRemaining
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

		public ushort MaxCharges => ActionManager.GetMaxCharges(ActionID, 0);

		public bool HasCharges => MaxCharges > 1;

		public unsafe uint RemainingCharges
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

		public float ChargeCooldownRemaining => CooldownRemaining % (CooldownTotal / MaxCharges);
	}
}
