using Dalamud.Hooking;
using FFXIVClientStructs.FFXIV.Client.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UltimateCombo.CustomCombo;
using UltimateCombo.Services;

namespace UltimateCombo.Core
{
	internal sealed partial class IconReplacer : IDisposable
	{
		private readonly List<CustomComboClass> _customCombos;
		private readonly Hook<IsIconReplaceableDelegate> _isIconReplaceableHook;
		private readonly Hook<GetIconDelegate> _getIconHook;

		private IntPtr _actionManager = IntPtr.Zero;
		private readonly IntPtr _module = IntPtr.Zero;

		public IconReplacer()
		{
			_customCombos = [.. Assembly.GetAssembly(typeof(CustomComboClass))!.GetTypes()
				.Where(t => !t.IsAbstract && t.BaseType == typeof(CustomComboClass))
				.Select(Activator.CreateInstance)
				.Cast<CustomComboClass>()
				.OrderByDescending(x => x.Preset)];

			_getIconHook = Service.GameInteropProvider.HookFromAddress<GetIconDelegate>(ActionManager.Addresses.GetAdjustedActionId.Value, GetIconDetour);
			_isIconReplaceableHook = Service.GameInteropProvider.HookFromAddress<IsIconReplaceableDelegate>(Service.Address.IsActionIdReplaceable, IsIconReplaceableDetour);

			_getIconHook.Enable();
			_isIconReplaceableHook.Enable();
		}

		private delegate ulong IsIconReplaceableDelegate(uint actionID);

		private delegate uint GetIconDelegate(IntPtr actionManager, uint actionID);

		public void Dispose()
		{
			_getIconHook?.Dispose();
			_isIconReplaceableHook?.Dispose();
		}

		internal uint OriginalHook(uint actionID)
		{
			return _getIconHook.Original(_actionManager, actionID);
		}

		private unsafe uint GetIconDetour(IntPtr actionManager, uint actionID)
		{
			_actionManager = actionManager;

			try
			{
				if (Service.ClientState.LocalPlayer == null)
				{
					return OriginalHook(actionID);
				}

				var lastComboMove = ActionManager.Instance()->Combo.Action;
				var comboTime = ActionManager.Instance()->Combo.Timer;
				var level = Service.ClientState.LocalPlayer?.Level ?? 0;

				foreach (CustomComboClass? combo in _customCombos)
				{
					if (combo.TryInvoke(actionID, level, lastComboMove, comboTime, out var newActionID))
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
