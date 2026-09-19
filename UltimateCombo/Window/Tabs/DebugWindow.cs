using Dalamud.Bindings.ImGui;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Game.ClientState.Statuses;
using Dalamud.Interface.Utility.Raii;
using System;
using System.Linq;
using System.Numerics;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Combos.General;
using UltimateCombo.Core;
using UltimateCombo.Data;

namespace UltimateCombo.Window.Tabs;

internal class DebugWindow : ConfigWindow
{
    internal static new void Draw()
    {
        IPlayerCharacter? localPlayer = Service.ObjectTable.LocalPlayer;
        if (localPlayer is null)
        {
            ImGui.TextUnformatted("Please log in to use this tab.");
            return;
        }

        var target = CustomComboFunctions.CurrentTarget as IBattleChara;
        string playerName = localPlayer.Name.ToString();

        if (localPlayer.StatusList.Any())
        {
            ImGui.Spacing();
            ImGui.Separator();
            ImGui.TextUnformatted("Player Status");

            foreach (IStatus status in localPlayer.StatusList)
            {
                ImGui.TextUnformatted($"{playerName} -> {ActionWatching.GetStatusName(status.StatusId)}: {status.StatusId} ({Math.Round(status.RemainingTime, 1)}s)");
            }
        }

        if (target?.StatusList.Any() == true)
        {
            ImGui.Spacing();
            ImGui.Separator();
            ImGui.TextUnformatted("Target Status");

            foreach (IStatus status in target.StatusList)
            {
                ImGui.TextUnformatted($"{target.Name} -> {ActionWatching.GetStatusName(status.StatusId)}: {status.StatusId} ({Math.Round(status.RemainingTime, 1)}s)");
            }
        }

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.TextUnformatted("Target Information");
        ImGui.TextUnformatted($"In Melee Range: {CustomComboFunctions.InMeleeRange()}");
        ImGui.TextUnformatted($"Current HP: {CustomComboFunctions.EnemyCurrentHP()}");
        ImGui.TextUnformatted($"Maximum HP: {CustomComboFunctions.EnemyMaxHP()}");
        ImGui.TextUnformatted($"HP Percentage: {CustomComboFunctions.EnemyPercentHP()}");
        ImGui.TextUnformatted($"Enemy Rank: {CustomComboFunctions.EnemyRank()}");
        ImGui.TextUnformatted($"Is Boss: {CustomComboFunctions.TargetIsBoss()}");
        ImGui.TextUnformatted($"Is DoT-worthy: {CustomComboFunctions.TargetWorthDoT()}");

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.TextUnformatted("Action Information");
        ImGui.TextUnformatted($"Last Action: {ActionWatching.GetActionName(ActionWatching.LastAction)} (ID: {ActionWatching.LastAction})");
        ImGui.TextUnformatted($"Last Action Type: {ActionWatching.GetAttackType(ActionWatching.LastAction)}");
        ImGui.TextUnformatted($"Last Weaponskill: {ActionWatching.GetActionName(ActionWatching.LastWeaponskill)}");
        ImGui.TextUnformatted($"Last Spell: {ActionWatching.GetActionName(ActionWatching.LastSpell)}");
        ImGui.TextUnformatted($"Last Ability: {ActionWatching.GetActionName(ActionWatching.LastAbility)}");
        ImGui.TextUnformatted($"GCDs Used: {ActionWatching.NumberOfGcdsUsed}");
        ImGui.TextUnformatted($"Combo Timer: {CustomComboFunctions.ComboTime}");

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.TextUnformatted("Location");
        ImGui.TextUnformatted($"Territory: {Service.ClientState.TerritoryType}");
        ImGui.TextUnformatted($"Map ID: {Service.ClientState.MapId}");

        if (localPlayer.ClassJob.RowId == BST.JobID)
        {
            ImGui.Spacing();
            ImGui.Separator();
            ImGui.TextUnformatted("Beastmaster Gauge");
            ImGui.TextUnformatted($"Player TP: {BST.Gauge.PlayerTP}");
            ImGui.TextUnformatted($"Beast TP: {BST.Gauge.PetTP}");
            ImGui.TextUnformatted($"Battle Horn: {BST.Gauge.BattleHorn}");
            ImGui.TextUnformatted($"Active Affinity: {BST.Gauge.ActiveAffinity}");
            ImGui.TextUnformatted($"Pet Instinct: {BST.Gauge.PetInstinct}");
            ImGui.TextUnformatted($"Master Instinct: {BST.Gauge.PlayerInstinct}");
        }

        if (localPlayer.ClassJob.RowId == BLU.JobID)
        {
            ImGui.Spacing();
            ImGui.Separator();
            ImGui.TextUnformatted("Active BLU Spells");

            using ImRaii.ChildDisposable spells = ImRaii.Child("BLUSpells", new Vector2(200, 405), true);
            ImGui.TextUnformatted(string.Join("\n", Service.Configuration.ActiveBLUSpells
                .Select(ActionWatching.GetActionName)
                .OrderBy(name => name)));
        }
    }
}
