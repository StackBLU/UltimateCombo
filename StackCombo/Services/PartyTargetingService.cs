using FFXIVClientStructs.FFXIV.Client.Game.Object;
using FFXIVClientStructs.FFXIV.Client.System.Framework;
using System;
using GameObject = FFXIVClientStructs.FFXIV.Client.Game.Object.GameObject;

namespace StackCombo.Services
{
	public static unsafe class PartyTargetingService
	{
		private static readonly IntPtr pronounModule = (IntPtr)Framework.Instance()->GetUIModule()->GetPronounModule();
		public static GameObject* UITarget
		{
			get
			{
				return (GameObject*)*(IntPtr*)(pronounModule + 0x290);
			}
		}

		public static ulong GetObjectID(GameObject* o)
		{
			GameObjectId id = o->GetGameObjectId();
			return id.ObjectId;
		}

		private static readonly delegate* unmanaged<IntPtr, uint, GameObject*> getGameObjectFromPronounID = (delegate* unmanaged<IntPtr, uint, GameObject*>)Service.SigScanner.ScanText("E8 ?? ?? ?? ?? 48 8B D8 48 85 C0 0F 85 ?? ?? ?? ?? 8D 4F DD");
		public static GameObject* GetGameObjectFromPronounID(uint id)
		{
			return getGameObjectFromPronounID(pronounModule, id);
		}
	}
}
