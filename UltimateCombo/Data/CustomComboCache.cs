using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.Game;
using System;
using System.Collections.Concurrent;
using UltimateCombo.Core;
using DalamudStatus = Dalamud.Game.ClientState.Statuses;

namespace UltimateCombo.Data;

internal partial class CustomComboCache : IDisposable
{
    private const uint InvalidObjectID = 0xE000_0000;

    private readonly ConcurrentDictionary<(uint StatusID, ulong? TargetID, ulong? SourceID), DalamudStatus.Status?> _statusCache = new();
    private readonly ConcurrentDictionary<uint, CooldownData?> _cooldownCache = new();
    private readonly ConcurrentDictionary<Type, JobGaugeBase> _jobGaugeCache = new();

    internal CustomComboCache()
    {
        Service.Framework.Update += Framework_Update;
    }

    private delegate IntPtr GetActionCooldownSlotDelegate(IntPtr actionManager, int cooldownGroup);

    internal void Dispose()
    {
        Service.Framework.Update -= Framework_Update;
    }

    internal T GetJobGauge<T>() where T : JobGaugeBase
    {
        if (!_jobGaugeCache.TryGetValue(typeof(T), out JobGaugeBase? gauge))
        {
            gauge = _jobGaugeCache[typeof(T)] = Service.JobGauges.Get<T>();
        }

        return (T) gauge;
    }

    internal DalamudStatus.Status? GetStatus(uint statusID, IGameObject? obj, ulong? sourceID)
    {
        (uint statusID, ulong? GameObjectId, ulong? sourceID) key = (statusID, obj?.GameObjectId, sourceID);
        if (_statusCache.TryGetValue(key, out DalamudStatus.Status? found))
        {
            return found;
        }

        if (obj is null)
        {
            return _statusCache[key] = null;
        }

        if (obj is not IBattleChara chara)
        {
            return _statusCache[key] = null;
        }

        foreach (DalamudStatus.Status status in chara.StatusList)
        {
            if (status.StatusId == statusID && (!sourceID.HasValue || status.SourceId == 0 || status.SourceId == InvalidObjectID || status.SourceId == sourceID))
            {
                return _statusCache[key] = status;
            }
        }

        return _statusCache[key] = null;
    }

    internal unsafe CooldownData GetCooldown(uint actionID)
    {
        if (_cooldownCache.TryGetValue(actionID, out CooldownData? found))
        {
            return found!;
        }

        var data = new CooldownData()
        {
            ActionID = actionID,
        };

        return _cooldownCache[actionID] = data;
    }

    internal static unsafe int GetResourceCost(uint actionID)
    {
        ActionManager* actionManager = ActionManager.Instance();
        if (actionManager == null)
        {
            return 0;
        }

        var cost = ActionManager.GetActionCost(ActionType.Action, actionID, 0, 0, 0, 0);
        return cost;
    }

    private unsafe void Framework_Update(IFramework framework)
    {
        _statusCache.Clear();
        _cooldownCache.Clear();
    }

    void IDisposable.Dispose()
    {
        Dispose();
    }
}
