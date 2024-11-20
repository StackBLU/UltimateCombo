using Dalamud.Game;
using System;
using UltimateCombo.Services;

namespace UltimateCombo.Core
{
	internal class PluginAddressResolver
	{
		public IntPtr IsActionIdReplaceable { get; private set; }

		public unsafe void Setup(ISigScanner scanner)
		{
			IsActionIdReplaceable = scanner.ScanText("E8 ?? ?? ?? ?? 84 C0 74 69 8B D3");

			Service.PluginLog.Verbose("===== UltimateCombo =====");
			Service.PluginLog.Verbose($"{nameof(IsActionIdReplaceable)} 0x{IsActionIdReplaceable:X}");
		}
	}
}