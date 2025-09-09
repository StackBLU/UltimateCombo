using Dalamud.Plugin.Services;
using UltimateCombo.Core;

namespace UltimateCombo.ComboHelper.Functions;

internal abstract partial class CustomComboFunctions
{
    internal static bool IsInParty()
    {
        return Service.PartyList.PartyId > 0;
    }

    internal static IPartyList GetPartyMembers()
    {
        return Service.PartyList;
    }
}
