using Dalamud.Game;
using StackCombo.Services;
using System;

namespace StackCombo.Core
{
	internal class PluginAddressResolver
	{
		public IntPtr IsActionIdReplaceable { get; private set; }

		public unsafe void Setup(ISigScanner scanner)
		{
			IsActionIdReplaceable = scanner.ScanText("40 53 48 83 EC 20 8B D9 48 8B 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 85 C0 74 1F");

			Service.PluginLog.Verbose("===== StackCombo =====");
			Service.PluginLog.Verbose($"{nameof(IsActionIdReplaceable)} 0x{IsActionIdReplaceable:X}");
		}
	}
}
