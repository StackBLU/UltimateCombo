using Dalamud.Game.ClientState.Objects.Types;

namespace UltimateCombo.Extensions
{
	internal static class BattleCharaExtensions
	{
		public static unsafe uint RawShieldValue(this IBattleChara chara)
		{
			FFXIVClientStructs.FFXIV.Client.Game.Character.BattleChara* baseVal = (FFXIVClientStructs.FFXIV.Client.Game.Character.BattleChara*)chara.Address;
			byte value = baseVal->Character.CharacterData.ShieldValue;
			uint rawValue = chara.MaxHp / 100 * value;

			return rawValue;
		}

		public static unsafe byte ShieldPercentage(this IBattleChara chara)
		{
			FFXIVClientStructs.FFXIV.Client.Game.Character.BattleChara* baseVal = (FFXIVClientStructs.FFXIV.Client.Game.Character.BattleChara*)chara.Address;
			byte value = baseVal->Character.CharacterData.ShieldValue;

			return value;
		}

		public static bool HasShield(this IBattleChara chara)
		{
			return chara.RawShieldValue() > 0;
		}
	}
}
