using System;
using System.Timers;

namespace StackCombo.ComboHelper.Functions
{
	internal abstract partial class CustomComboFunctions
	{
		internal bool restartCombatTimer = true;
		internal TimeSpan combatDuration = new();
		internal DateTime combatStart;
		internal DateTime combatEnd;
		internal Timer? combatTimer;

		/// <summary> Called by the timer in the constructor to keep track of combat duration. </summary>
		internal void UpdateCombatTimer(object? sender, EventArgs e)
		{
			if (InCombat())
			{
				if (restartCombatTimer)
				{
					restartCombatTimer = false;
					combatStart = DateTime.Now;
				}

				combatEnd = DateTime.Now;
			}
			else
			{
				restartCombatTimer = true;
				combatDuration = TimeSpan.Zero;
			}

			combatDuration = combatEnd - combatStart;
		}

		protected TimeSpan CombatEngageDuration()
		{
			return combatDuration;
		}

		protected void StartTimer()
		{
			combatTimer = new Timer(1000);
			combatTimer.Elapsed += UpdateCombatTimer;
			combatTimer.Start();
		}
	}
}
