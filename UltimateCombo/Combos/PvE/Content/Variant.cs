using ECommons.DalamudServices;

namespace UltimateCombo.Combos.PvE.Content
{
	internal static class Variant
	{
		public const uint
			VariantUltimatum = 29730,
			VariantRaise = 29731,
			VariantRaise2 = 29734,

			//Don't use these for logic - they are just for the ReplaceSkill icon
			VariantCure_Image = 29729,
			VariantSpiritDart_Image = 29732,
			VariantRampart_Image = 29733;

		//1069 = The Sil'dihn Subterrane
		//1137 = Mount Rokkon
		//1176 = Aloalo Island

		public static uint VariantCure
		{
			get
			{
				return Svc.ClientState.TerritoryType switch
				{
					1069 => 29729,
					1137 or 1176 => 33862,
					_ => 0
				};
			}
		}

		public static uint VariantSpiritDart
		{
			get
			{
				return Svc.ClientState.TerritoryType switch
				{
					1069 => 29732,
					1137 or 1176 => 33863,
					_ => 0
				};
			}
		}

		public static uint VariantRampart
		{
			get
			{
				return Svc.ClientState.TerritoryType switch
				{
					1069 => 29733,
					1137 or 1176 => 33864,
					_ => 0
				};
			}
		}

		public static class Buffs
		{
			public const ushort
				EmnityUp = 3358,
				VulnDown = 3360,
				Rehabilitation = 3367,
				DamageBarrier = 3405;
		}

		public static class Debuffs
		{
			public const ushort
				SustainedDamage = 3359;
		}
	}
}