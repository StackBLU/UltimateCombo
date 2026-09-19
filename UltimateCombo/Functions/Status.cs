
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Game.ClientState.Statuses;
using System.Collections.Generic;
using System.Linq;
using UltimateCombo.Combos.Content;
using UltimateCombo.Core;
using UltimateCombo.Data;

namespace UltimateCombo.ComboHelper.Functions;

internal abstract partial class CustomComboFunctions
{
    internal static IStatus? FindEffect(ushort effectID, IGameObject? target, ulong? sourceID)
    {
        return Service.ComboCache.GetStatus(effectID, target, sourceID);
    }

    // Effect Exists
    internal static bool HasEffect(ushort effectID)
    {
        IGameObject? player = LocalPlayer;
        return player != null && FindEffect(effectID, player, IconReplacer.ThreadSafeCache.LocalPlayerObjectID) is not null;
    }

    internal static bool HasEffectAny(ushort effectID)
    {
        IGameObject? player = LocalPlayer;
        return player != null && FindEffect(effectID, player, null) is not null;
    }

    internal static bool TargetHasEffect(ushort effectID)
    {
        IGameObject? target = CurrentTarget;
        return target != null && FindEffect(effectID, target, IconReplacer.ThreadSafeCache.LocalPlayerObjectID) is not null;
    }

    internal static bool TargetHasEffectAny(ushort effectID)
    {
        IGameObject? target = CurrentTarget;
        return target != null && FindEffect(effectID, target, null) is not null;
    }

    internal static bool TargetHasEffectAny(IEnumerable<ushort> effectIDs)
    {
        IGameObject? target = CurrentTarget;
        return target != null && effectIDs.Any(effectID => FindEffect(effectID, target, null) is not null);
    }

    internal static bool TargetOfTargetHasEffect(ushort effectID)
    {
        IGameObject? target = TargetOfTarget;
        return target != null && FindEffect(effectID, target, IconReplacer.ThreadSafeCache.LocalPlayerObjectID) is not null;
    }

    internal static bool TargetOfTargetHasEffectAny(ushort effectID)
    {
        IGameObject? target = TargetOfTarget;
        return target != null && FindEffect(effectID, target, null) is not null;
    }

    internal static bool DebuffCullCheck()
    {
        return CurrentTarget is IBattleChara character
            && character.StatusList.Count(status => status.StatusId != 0) < 30;
    }

    // Effect Remaining Time
    internal static float EffectRemainingTime(ushort effectID)
    {
        IGameObject? player = LocalPlayer;
        return player == null ? 0 : FindEffect(effectID, player, IconReplacer.ThreadSafeCache.LocalPlayerObjectID)?.RemainingTime ?? 0;
    }

    internal static float EffectRemainingTimeAny(ushort effectID)
    {
        IGameObject? player = LocalPlayer;
        return player == null ? 0 : FindEffect(effectID, player, null)?.RemainingTime ?? 0;
    }

    internal static float TargetEffectRemainingTime(ushort effectID)
    {
        IGameObject? target = CurrentTarget;
        return target == null ? 0 : FindEffect(effectID, target, IconReplacer.ThreadSafeCache.LocalPlayerObjectID)?.RemainingTime ?? 0;
    }

    internal static float TargetEffectRemainingTimeAny(ushort effectID)
    {
        IGameObject? target = CurrentTarget;
        return target == null ? 0 : FindEffect(effectID, target, null)?.RemainingTime ?? 0;
    }

    internal static float TargetOfTargetEffectRemainingTime(ushort effectID)
    {
        IGameObject? target = TargetOfTarget;
        return target == null ? 0 : FindEffect(effectID, target, IconReplacer.ThreadSafeCache.LocalPlayerObjectID)?.RemainingTime ?? 0;
    }

    internal static float TargetOfTargetEffectRemainingTimeAny(ushort effectID)
    {
        IGameObject? target = TargetOfTarget;
        return target == null ? 0 : FindEffect(effectID, target, null)?.RemainingTime ?? 0;
    }

    // Effect Stacks
    internal static ushort EffectStacks(ushort effectID)
    {
        IGameObject? player = LocalPlayer;
        return player == null ? (ushort) 0 : FindEffect(effectID, player, IconReplacer.ThreadSafeCache.LocalPlayerObjectID)?.Param ?? 0;
    }

    internal static ushort EffectStacksAny(ushort effectID)
    {
        IGameObject? player = LocalPlayer;
        return player == null ? (ushort) 0 : FindEffect(effectID, player, null)?.Param ?? 0;
    }

    internal static ushort TargetEffectStacks(ushort effectID)
    {
        IGameObject? target = CurrentTarget;
        return target == null ? (ushort) 0 : FindEffect(effectID, target, IconReplacer.ThreadSafeCache.LocalPlayerObjectID)?.Param ?? 0;
    }

    internal static ushort TargetEffectStacksAny(ushort effectID)
    {
        IGameObject? target = CurrentTarget;
        return target == null ? (ushort) 0 : FindEffect(effectID, target, null)?.Param ?? 0;
    }

    internal static ushort TargetOfTargetEffectStacks(ushort effectID)
    {
        IGameObject? target = TargetOfTarget;
        return target == null ? (ushort) 0 : FindEffect(effectID, target, IconReplacer.ThreadSafeCache.LocalPlayerObjectID)?.Param ?? 0;
    }

    internal static ushort TargetOfTargetEffectStacksAny(ushort effectID)
    {
        IGameObject? target = TargetOfTarget;
        return target == null ? (ushort) 0 : FindEffect(effectID, target, null)?.Param ?? 0;
    }

    // Common Effects
    internal static bool HasSilence()
    {
        return HasStatusWithMatchingName(7);
    }

    internal static bool HasPacification()
    {
        return HasStatusWithMatchingName(6);
    }

    internal static bool HasAmnesia()
    {
        return HasStatusWithMatchingName(5);
    }

    // Occult Weaknesses
    internal static ushort CurrentElementalWeakness()
    {
        return Occult.Debuffs.ElementalWeaknesses.FirstOrDefault(TargetHasEffectAny);
    }

    private static bool HasStatusWithMatchingName(uint statusID)
    {
        IGameObject? player = LocalPlayer;
        if (player == null)
        {
            return false;
        }

        return ActionWatching.GetStatusesByName(ActionWatching.GetStatusName(statusID))
            ?.Any(status => FindEffect((ushort) status, player, null) is not null) ?? false;
    }
}

