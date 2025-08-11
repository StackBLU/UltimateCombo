using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Data;

namespace UltimateCombo.Extensions
{
	internal static class UIntExtensions
	{
		internal static bool ActionReady(this uint value)
		{
			return CustomComboFunctions.ActionReady(value);
		}

		internal static bool TraitActionReady(this uint value)
		{
			return CustomComboFunctions.TraitActionReady(value);
		}

		internal static string ActionName(this uint value)
		{
			return ActionWatching.GetActionName(value);
		}
	}

	internal static class UShortExtensions
	{
		internal static string StatusName(this ushort value)
		{
			return ActionWatching.GetStatusName(value);
		}
	}
}
