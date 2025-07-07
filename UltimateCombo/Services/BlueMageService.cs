using System.Linq;

using FFXIVClientStructs.FFXIV.Client.Game;

namespace UltimateCombo.Services
{
    internal static unsafe class BlueMageService
    {
        public static void PopulateBLUSpells()
        {
            var prevList = Service.Configuration.ActiveBLUSpells.ToList();
            Service.Configuration.ActiveBLUSpells.Clear();

            for (var i = 0; i <= 24; i++)
            {
                var id = ActionManager.Instance()->GetActiveBlueMageActionInSlot(i);
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
