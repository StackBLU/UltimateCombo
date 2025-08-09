using Dalamud.Bindings.ImGui;
using System.Numerics;
using UltimateCombo.Services;

namespace UltimateCombo.Window.Tabs
{
    internal class SettingsWindow : ConfigWindow
    {
        internal static new void Draw()
        {
            PvEWindow.HasToOpenJob = true;
            _ = ImGui.BeginChild("main", new Vector2(0, 0), true);

            ImGui.Text("-- Quick Guide --" +
                "\nIf an option is toggled on, it will appear in the rotation at some point.\nThere are unique conditions (buffs, timings, hp checks, etc.)\ntied to certain skills appearing, so if it's not happening,\nit probably isn't supposed to at that moment." +
                "\n\nEach combo's trigger skills are shown underneath the toggle.\nOnly those skills will trigger the combo.");

            ImGui.Text("\n-- Controller Controls (PS / XBox) --" +
                "\nL1 + L3 - Dalamud interface" +
                "\nD-pad - Navigate window" +
                "\nX / A - Select" +
                "\nCircle / B - Back" +
                "\nSquare / X (Hold) - Select window" +
                "\n\t- L1 / R1 - Up/Down" +
                "\n\t- D-pad - Window size" +
                "\n\t- Left stick - Move window" +
                "\n\n");

            ImGui.Text("-- Settings --");

            var openOnLaunch = Service.Configuration.OpenOnLaunch;
            if (ImGui.Checkbox("Open on launch", ref openOnLaunch))
            {
                Service.Configuration.OpenOnLaunch = openOnLaunch;
                Service.Configuration.Save();
            }

            var hideChildren = Service.Configuration.HideChildren;
            if (ImGui.Checkbox("Hide nested options", ref hideChildren))
            {
                Service.Configuration.HideChildren = hideChildren;
                Service.Configuration.Save();
            }

            var hideConflicting = Service.Configuration.HideConflictedCombos;
            if (ImGui.Checkbox("Hide conflicting combos", ref hideConflicting))
            {
                Service.Configuration.HideConflictedCombos = hideConflicting;
                Service.Configuration.Save();
            }

            var ignoreGCDChecks = Service.Configuration.IgnoreGCDChecks;
            if (ImGui.Checkbox("Ignore GCD Checks - Combos will start using abilities immediately", ref ignoreGCDChecks))
            {
                Service.Configuration.IgnoreGCDChecks = ignoreGCDChecks;
                Service.Configuration.Save();
            }

            var disableTripleWeaving = Service.Configuration.DisableTripleWeaving;
            if (ImGui.Checkbox("Disable Triple Weaving", ref disableTripleWeaving))
            {
                Service.Configuration.DisableTripleWeaving = disableTripleWeaving;
                Service.Configuration.Save();
            }

            var rangedAttackRange = Service.Configuration.RangedAttackRange;
            ImGui.SetNextItemWidth(150);
            if (ImGui.InputDouble("Minimum range away from target before\nusing ranged attacks on melee classes", ref rangedAttackRange, 1))
            {
                Service.Configuration.RangedAttackRange = rangedAttackRange;
                Service.Configuration.Save();
            }

            ImGui.EndChild();
        }
    }
}
