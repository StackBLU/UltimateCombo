using Dalamud.Plugin.Services;

using UltimateCombo.Services;

namespace UltimateCombo.ComboHelper.Functions
{
    internal abstract partial class CustomComboFunctions
    {
        /// <summary> Checks if player is in a party </summary>
        public static bool IsInParty()
        {
            return Service.PartyList.PartyId > 0;
        }

        /// <summary> Gets the party list </summary>
        /// <returns> Current party list. </returns>
        public static IPartyList GetPartyMembers()
        {
            return Service.PartyList;
        }
    }
}