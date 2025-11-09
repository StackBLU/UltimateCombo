using Dalamud.Bindings.ImGui;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Objects.Types;
using System;
using System.Linq;
using System.Numerics;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Combos;
using UltimateCombo.Core;
using UltimateCombo.Data;
using Status = Dalamud.Game.ClientState.Statuses.Status;

namespace UltimateCombo.Window.Tabs;

internal class DebugWindow : ConfigWindow
{
    internal class DebugCombo : CustomComboBase
    {
        protected internal override Presets Preset { get; }

        protected override uint Invoke(uint actionID, uint lastComboActionID)
        {
            return actionID;
        }
    }

    internal static new unsafe void Draw()
    {
        IPlayerCharacter? localPlayer = Service.ClientState.LocalPlayer;
        var chara = CustomComboFunctions.CurrentTarget as IBattleChara;
        var comboClass = new DebugCombo();

        if (localPlayer != null)
        {
            if (chara != null)
            {
                foreach (Status status in chara.StatusList)
                {
                    ImGui.TextUnformatted($"Target Status: {chara.Name} -> " +
                        $"{ActionWatching.GetStatusName(status.StatusId)}: {status.StatusId} {Math.Round(status.RemainingTime, 1)}");
                }
            }

            if (Service.ClientState.LocalPlayer?.StatusList is { } statusList)
            {
                ImGui.TextUnformatted("\n");
                foreach (Status status in statusList)
                {
                    ImGui.TextUnformatted($"Self Status: {Service.ClientState.LocalPlayer?.Name} -> " +
                        $"{ActionWatching.GetStatusName(status.StatusId)}: {status.StatusId} {Math.Round(status.RemainingTime, 1)}");
                }

                ImGui.TextUnformatted("\n");
                ImGui.TextUnformatted($"Current HP %: {CustomComboFunctions.PlayerHealthPercentageHp()}");
                ImGui.TextUnformatted($"Current HP % for PvP: {CustomComboFunctions.PlayerHealthPercentageHpPvP()}");

                ImGui.TextUnformatted("\n");
                ImGui.TextUnformatted($"In Combat: {CustomComboFunctions.InCombat()}");
                ImGui.TextUnformatted($"In Melee Range: {CustomComboFunctions.InMeleeRange()}");

                ImGui.TextUnformatted("\n");
                ImGui.TextUnformatted($"Target Current HP: {CustomComboFunctions.EnemyCurrentHP()}");
                ImGui.TextUnformatted($"Target Max HP: {CustomComboFunctions.EnemyMaxHP()}");
                ImGui.TextUnformatted($"Target Percent HP: {CustomComboFunctions.EnemyPercentHP()}");
                ImGui.TextUnformatted($"Distance from Target: {CustomComboFunctions.GetTargetDistance()}");
                ImGui.TextUnformatted($"Enemy Rank: {CustomComboFunctions.EnemyRank()}");
                ImGui.TextUnformatted($"Target is Boss: {CustomComboFunctions.TargetIsBoss()}");
                ImGui.TextUnformatted($"Target is DoT-worthy: {CustomComboFunctions.TargetWorthDoT()}");
                ImGui.TextUnformatted($"Current Cast Time: {chara?.CurrentCastTime}");
                ImGui.TextUnformatted($"Total Cast Time: {chara?.TotalCastTime / 3}");
                ImGui.TextUnformatted($"Interrupt?: {CustomComboFunctions.CanInterrupt()}");
                ImGui.TextUnformatted($"Level difference?: {localPlayer.Level - CustomComboFunctions.EnemyLevel()}");
                ImGui.TextUnformatted($"Ignore GCDs based on level difference?: {CustomComboFunctions.LevelIgnoreGCD()}");

                ImGui.TextUnformatted("\n");
                ImGui.TextUnformatted($"Last Action: {ActionWatching.GetActionName(ActionWatching.LastAction)} (ID:{ActionWatching.LastAction})");
                ImGui.TextUnformatted($"Last Action Type: {ActionWatching.GetAttackType(ActionWatching.LastAction)}");
                ImGui.TextUnformatted($"Last Weaponskill: {ActionWatching.GetActionName(ActionWatching.LastWeaponskill)}");
                ImGui.TextUnformatted($"Last Spell: {ActionWatching.GetActionName(ActionWatching.LastSpell)}");
                ImGui.TextUnformatted($"Last Ability: {ActionWatching.GetActionName(ActionWatching.LastAbility)}");
                ImGui.TextUnformatted($"# of GCDs used: {ActionWatching.NumberOfGcdsUsed}");
                ImGui.TextUnformatted($"Combo Timer: {CustomComboFunctions.ComboTime}");

                ImGui.TextUnformatted("\n");
                ImGui.TextUnformatted($"Territory: {Service.ClientState.TerritoryType}");
                ImGui.TextUnformatted($"Map ID: {Service.ClientState.MapId}");

                ImGui.TextUnformatted("\n");
                ImGui.TextUnformatted($"-- Active BLU Spells --");
                _ = ImGui.BeginChild("BLUSpells", new Vector2(200, 405), true);
                ImGui.TextUnformatted($"{string.Join("\n", Service.Configuration.ActiveBLUSpells.Select(ActionWatching.GetActionName).OrderBy(x => x))}");
                ImGui.EndChild();
            }
            else
            {
                ImGui.TextUnformatted("Please log in to use this tab.");
            }
        }
    }
}
