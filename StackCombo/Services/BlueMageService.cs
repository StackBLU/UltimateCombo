using FFXIVClientStructs.FFXIV.Client.Game;
using System.Linq;

namespace StackCombo.Services
{
	internal static unsafe class BlueMageService
	{
		public static void PopulateBLUSpells()
		{
			System.Collections.Generic.List<uint> prevList = Service.Configuration.ActiveBLUSpells.ToList();
			Service.Configuration.ActiveBLUSpells.Clear();

			for (int i = 0; i <= 24; i++)
			{
				uint id = ActionManager.Instance()->GetActiveBlueMageActionInSlot(i);
				if (id != 0)
				{
					Service.Configuration.ActiveBLUSpells.Add(id);
				}
			}

			if (Service.Configuration.ActiveBLUSpells.Except(prevList).Any())
			{
				Service.Configuration.Save();
			}
		}
	}
}
