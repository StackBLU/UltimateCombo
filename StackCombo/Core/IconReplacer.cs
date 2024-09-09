using Dalamud.Hooking;
using FFXIVClientStructs.FFXIV.Client.Game;
using StackCombo.CustomCombo;
using StackCombo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StackCombo.Core
{
	internal sealed partial class IconReplacer : IDisposable
	{
		private readonly List<CustomComboClass> customCombos;
		private readonly Hook<IsIconReplaceableDelegate> isIconReplaceableHook;
		private readonly Hook<GetIconDelegate> getIconHook;

		private IntPtr actionManager = IntPtr.Zero;
		private readonly IntPtr module = IntPtr.Zero;

		public IconReplacer()
		{
			customCombos = Assembly.GetAssembly(typeof(CustomComboClass))!.GetTypes()
				.Where(t => !t.IsAbstract && t.BaseType == typeof(CustomComboClass))
				.Select(Activator.CreateInstance)
				.Cast<CustomComboClass>()
				.OrderByDescending(x => x.Preset)
				.ToList();

			getIconHook = Service.GameInteropProvider.HookFromAddress<GetIconDelegate>(ActionManager.Addresses.GetAdjustedActionId.Value, GetIconDetour);
			isIconReplaceableHook = Service.GameInteropProvider.HookFromAddress<IsIconReplaceableDelegate>(Service.Address.IsActionIdReplaceable, IsIconReplaceableDetour);

			getIconHook.Enable();
			isIconReplaceableHook.Enable();
		}

		private delegate ulong IsIconReplaceableDelegate(uint actionID);

		private delegate uint GetIconDelegate(IntPtr actionManager, uint actionID);

		public void Dispose()
		{
			getIconHook?.Dispose();
			isIconReplaceableHook?.Dispose();
		}

		internal uint OriginalHook(uint actionID)
		{
			return getIconHook.Original(actionManager, actionID);
		}

		private unsafe uint GetIconDetour(IntPtr actionManager, uint actionID)
		{
			this.actionManager = actionManager;

			try
			{
				if (Service.ClientState.LocalPlayer == null)
				{
					return OriginalHook(actionID);
				}

				uint lastComboMove = ActionManager.Instance()->Combo.Action;
				float comboTime = ActionManager.Instance()->Combo.Timer;
				byte level = Service.ClientState.LocalPlayer?.Level ?? 0;

				foreach (CustomComboClass? combo in customCombos)
				{
					if (combo.TryInvoke(actionID, level, lastComboMove, comboTime, out uint newActionID))
					{
						return newActionID;
					}
				}

				return OriginalHook(actionID);
			}

			catch (Exception ex)
			{
				Service.PluginLog.Error(ex, "Preset error");
				return OriginalHook(actionID);
			}
		}

		private ulong IsIconReplaceableDetour(uint actionID)
		{
			return 1;
		}
	}
}