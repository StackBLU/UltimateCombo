using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Game.ClientState.Statuses;
using System.Linq;
using UltimateCombo.Core;
using UltimateCombo.Data;
namespace UltimateCombo.ComboHelper.Functions;

internal abstract partial class CustomComboFunctions
{
    internal static IStatus? FindEffect(ushort effectID, IGameObject? target, ulong? sourceID)
    {
        return Service.ComboCache.GetStatus(effectID, target, sourceID);
    }

    //Effect Exists
    internal static bool HasEffect(ushort effectID)
    {
        return FindEffect(effectID, LocalPlayer, LocalPlayerId) is not null;
    }

    internal static bool HasEffectAny(ushort effectID)
    {
        return FindEffect(effectID, LocalPlayer, null) is not null;
    }

    internal static bool TargetHasEffect(ushort effectID)
    {
        return CurrentTarget != null && FindEffect(effectID, CurrentTarget, LocalPlayerId) is not null;
    }

    internal static bool TargetHasEffectAny(ushort effectID)
    {
        return CurrentTarget != null && FindEffect(effectID, CurrentTarget, null) is not null;
    }


    internal static bool TargetOfTargetHasEffect(ushort effectID)
    {
        return TargetOfTarget != null && FindEffect(effectID, TargetOfTarget, LocalPlayerId) is not null;
    }

    internal static bool TargetOfTargetHasEffectAny(ushort effectID)
    {
        return TargetOfTarget != null && FindEffect(effectID, TargetOfTarget, null) is not null;
    }

    //Effect Remaining Time

    internal static float EffectRemainingTime(ushort effectID)
    {
        return FindEffect(effectID, LocalPlayer, LocalPlayerId)?.RemainingTime ?? 0;
    }


    internal static float EffectRemainingTimeAny(ushort effectID)
    {
        return FindEffect(effectID, LocalPlayer, null)?.RemainingTime ?? 0;
    }


    internal static float TargetEffectRemainingTime(ushort effectID)
    {
        return CurrentTarget != null ? FindEffect(effectID, CurrentTarget, LocalPlayerId)?.RemainingTime ?? 0 : 0;
    }

    internal static float TargetEffectRemainingTimeAny(ushort effectID)
    {
        return CurrentTarget != null ? FindEffect(effectID, CurrentTarget, null)?.RemainingTime ?? 0 : 0;
    }


    internal static float TargetOfTargetEffectRemainingTime(ushort effectID)
    {
        return TargetOfTarget != null ? FindEffect(effectID, TargetOfTarget, LocalPlayerId)?.RemainingTime ?? 0 : 0;
    }

    internal static float TargetOfTargetEffectRemainingTimeAny(ushort effectID)
    {
        return TargetOfTarget != null ? FindEffect(effectID, TargetOfTarget, null)?.RemainingTime ?? 0 : 0;
    }

    //Effect Stacks

    internal static ushort EffectStacks(ushort effectID)
    {
        return FindEffect(effectID, LocalPlayer, LocalPlayerId)?.Param ?? 0;
    }


    internal static ushort EffectStacksAny(ushort effectID)
    {
        return FindEffect(effectID, LocalPlayer, null)?.Param ?? 0;
    }


    internal static ushort TargetEffectStacks(ushort effectID)
    {
        return CurrentTarget != null ? FindEffect(effectID, CurrentTarget, LocalPlayerId)?.Param ?? 0 : (ushort) 0;
    }

    internal static ushort TargetEffectStacksAny(ushort effectID)
    {
        return CurrentTarget != null ? FindEffect(effectID, CurrentTarget, null)?.Param ?? 0 : (ushort) 0;
    }


    internal static ushort TargetOfTargetEffectStacks(ushort effectID)
    {
        return TargetOfTarget != null ? FindEffect(effectID, TargetOfTarget, LocalPlayerId)?.Param ?? 0 : (ushort) 0;
    }

    internal static ushort TargetOfTargetEffectStacksAny(ushort effectID)
    {
        return TargetOfTarget != null ? FindEffect(effectID, TargetOfTarget, null)?.Param ?? 0 : (ushort) 0;
    }

    //Common Effects

    internal static bool HasSilence()
    {
        return ActionWatching.GetStatusesByName(ActionWatching.GetStatusName(7))
            ?.Any(status => FindEffect((ushort) status, LocalPlayer, null) is not null) ?? false;
    }


    internal static bool HasPacification()
    {
        return ActionWatching.GetStatusesByName(ActionWatching.GetStatusName(6))
            ?.Any(status => FindEffect((ushort) status, LocalPlayer, null) is not null) ?? false;
    }


    internal static bool HasAmnesia()
    {
        return ActionWatching.GetStatusesByName(ActionWatching.GetStatusName(5))
            ?.Any(status => FindEffect((ushort) status, LocalPlayer, null) is not null) ?? false;
    }
}
