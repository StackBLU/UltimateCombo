using UltimateCombo.ComboHelper.Functions;

namespace UltimateCombo.Combos.PvP
{
	internal static class WARPvP
	{
		public const byte ClassID = 3;
		public const byte JobID = 21;
		internal const uint
			HeavySwing = 29074,
			Maim = 29075,
			StormsPath = 29076,
			PrimalRend = 29084,
			Onslaught = 29079,
			Orogeny = 29080,
			Blota = 29081,
			Bloodwhetting = 29082;

		internal class Buffs
		{
			internal const ushort
				NascentChaos = 1992,
				InnerRelease = 1303;
		}

		public static class Config
		{
			public static UserInt
				WARPVP_BlotaTiming = new("WARPVP_BlotaTiming");

		}
	}
}