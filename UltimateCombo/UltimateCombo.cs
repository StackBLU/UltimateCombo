using Dalamud.Game.ClientState.Statuses;
using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Dalamud.Utility;
using ECommons;
using ECommons.DalamudServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UltimateCombo.Attributes;
using UltimateCombo.ComboHelper.Functions;
using UltimateCombo.Combos;
using UltimateCombo.Combos.General;
using UltimateCombo.Combos.PvE;
using UltimateCombo.Core;
using UltimateCombo.Data;
using UltimateCombo.Window;
using UltimateCombo.Window.Tabs;

namespace UltimateCombo;

internal sealed partial class UltimateComboClass : IDalamudPlugin
{
    private readonly ConfigWindow _configWindow;
    internal static UltimateComboClass? P = null!;
    internal WindowSystem ws;
    internal static readonly List<uint> DisabledJobsPVE =
    [
        //All.JobID,
        //FSH.JobID,

        //PLD.JobID,
        //WAR.JobID,
        //DRK.JobID,
        //GNB.JobID,

        //WHM.JobID
        //SCH.JobID,
        //AST.JobID,
        //SGE.JobID,

        //MNK.JobID,
        //DRG.JobID,
        //NIN.JobID,
        //SAM.JobID,
        //RPR.JobID,
        //VPR.JobID,

        //BRD.JobID,
        //MCH.JobID,
        //DNC.JobID,
        //BLM.JobID,
        //SMN.JobID,
        //RDM.JobID,
        //PCT.JobID,

        //BLU.JobID
    ];

    internal static readonly List<uint> DisabledJobsPvP =
    [
        //All.JobID,

        //PLD.JobID,
        //WAR.JobID,
        //DRK.JobID,
        //GNB.JobID,

        //WHM.JobID
        //SCH.JobID,
        //AST.JobID,
        //SGE.JobID,

        //MNK.JobID,
        //DRG.JobID,
        //NIN.JobID,
        //SAM.JobID,
        //RPR.JobID,
        //VPR.JobID,

        //BRD.JobID,
        //MCH.JobID,
        //DNC.JobID,
        //BLM.JobID,
        //SMN.JobID,
        //RDM.JobID,
        //PCT.JobID
    ];

    internal static uint? JobID
    {
        get;
        set
        {
            if (field != value && value != null)
            {
                Service.PluginLog.Debug($"Switched to job {value}");
                PvEWindow.HasToOpenJob = true;
            }

            field = value;
        }
    }

    public UltimateComboClass(IDalamudPluginInterface pluginInterface)
    {
        P = this;
        ECommonsMain.Init(pluginInterface, this, ECommons.Module.All);
        _ = pluginInterface.Create<Service>();
        Service.Configuration = pluginInterface.GetPluginConfig() as PluginConfiguration ?? new PluginConfiguration();
        Service.Address = new PluginAddressResolver();
        Service.Address.Setup(Service.SigScanner);

        PresetStorage.Init();

        Service.ComboCache = new CustomComboCache();
        Service.IconReplacer = new IconReplacer();
        ActionWatching.Enable();
        _configWindow = new ConfigWindow();
        ws = new();
        ws.AddWindow(_configWindow);
        Service.Interface.UiBuilder.Draw += ws.Draw;
        Service.Interface.UiBuilder.OpenConfigUi += OnOpenConfigUi;
        Service.Interface.UiBuilder.OpenMainUi += OnOpenConfigUi;
        _ = Service.CommandManager.AddHandler("/uc", new CommandInfo(OnCommand)
        {
            HelpMessage = "Opens the main plugin window, where you can enable and disable options"
                + "\n/uc enable <featureName> → Turns a specific option on by referring to its internal name"
                + "\n/uc disable <featureName> → Turns a specific option off by referring to its internal name"
                + "\n/uc toggle <featureName> → Toggles a specific option by referring to its internal name"
                + "\n/uc enableall → Turns all options on"
                + "\n/uc disableall → Turns all options off"
                + "\n/uc gcd → Toggles the GCD Counting setting"
                + "\n/uc enabled → Prints a list of every enabled option into the game chat"
                + "\n/uc debug → Outputs a full debug file to your desktop that can be sent to developers to assist in bug fixing"
                + "\n/uc debug <jobShort> → Outputs a debug file to your desktop containing only job-relevant options"
        });
        Service.Framework.Update += OnFrameworkUpdate;
        Service.DtrBarEntry = Svc.DtrBar.Get("Ultimate Combo");
        Service.DtrBarEntry.Text = "GCD Counting  " + (Service.Configuration.IgnoreGCDChecks ? "X" : "✓");
        Service.DtrBarEntry.OnClick = (_) =>
        {
            Service.Configuration.IgnoreGCDChecks = !Service.Configuration.IgnoreGCDChecks;
            Service.Configuration.Save();
            Service.DtrBarEntry.Text = "GCD Counting  " + (Service.Configuration.IgnoreGCDChecks ? "X" : "✓");
        };
        KillRedundantIDs();
        HandleConflictedCombos();
        if (Service.Configuration.OpenOnLaunch)
        {
            _configWindow.IsOpen = true;
        }
    }

    private static void HandleConflictedCombos()
    {
        HashSet<Presets> enabledCopy = [.. Service.Configuration.EnabledActions];
        foreach (Presets preset in enabledCopy)
        {
            if (!PresetStorage.IsEnabled(preset))
            {
                continue;
            }

            ConflictingCombosAttribute? conflictingCombos = preset.GetAttribute<ConflictingCombosAttribute>();
            if (conflictingCombos != null)
            {
                foreach (Presets conflict in conflictingCombos.ConflictingPresets)
                {
                    if (PresetStorage.IsEnabled(conflict))
                    {
                        _ = Service.Configuration.EnabledActions.Remove(conflict);
                        Service.Configuration.Save();
                    }
                }
            }
        }
    }

    private void OnFrameworkUpdate(IFramework framework)
    {
        if (Service.ObjectTable.LocalPlayer is not null)
        {
            JobID = Service.ObjectTable.LocalPlayer?.ClassJob.Value.RowId;

            if (JobID == BLU.JobID)
            {
                CustomComboFunctions.PopulateBLUSpells();
            }
        }
    }

    private static void KillRedundantIDs()
    {
        var redundantIDs = Service.Configuration.EnabledActions.Where(x => int.TryParse(x.ToString(), out _)).OrderBy(x => x).Cast<int>().ToList();
        foreach (var id in redundantIDs)
        {
            _ = Service.Configuration.EnabledActions.RemoveWhere(x => (int) x == id);
        }

        Service.Configuration.Save();

    }

    private void DrawUI()
    {
        _configWindow.Draw();
    }

    internal void Dispose()
    {
        _configWindow?.Dispose();

        ws.RemoveAllWindows();
        _ = Service.CommandManager.RemoveHandler("/uc");
        Service.Framework.Update -= OnFrameworkUpdate;
        Service.Interface.UiBuilder.OpenConfigUi -= OnOpenConfigUi;
        Service.Interface.UiBuilder.Draw -= DrawUI;

        Service.IconReplacer?.Dispose();
        Service.ComboCache?.Dispose();
        ActionWatching.Dispose();

        P = null;
    }

    private void OnOpenConfigUi()
    {
        _configWindow.IsOpen = !_configWindow.IsOpen;
    }

    private void OnCommand(string command, string arguments)
    {
        var argumentsParts = arguments.Split();

        switch (argumentsParts[0].ToLower())
        {

            case "enable":
                {
                    var targetPreset = argumentsParts[1].ToLowerInvariant();
                    foreach (Presets preset in Enum.GetValues<Presets>())
                    {
                        if (!preset.ToString().Equals(targetPreset, StringComparison.InvariantCultureIgnoreCase))
                        {
                            continue;
                        }

                        _ = Service.Configuration.EnabledActions.Add(preset);
                        Service.ChatGui.Print($"{preset} enabled!");
                    }

                    Service.Configuration.Save();
                    break;
                }

            case "disable":
                {
                    var targetPreset = argumentsParts[1].ToLowerInvariant();
                    foreach (Presets preset in Enum.GetValues<Presets>())
                    {
                        if (!preset.ToString().Equals(targetPreset, StringComparison.InvariantCultureIgnoreCase))
                        {
                            continue;
                        }

                        _ = Service.Configuration.EnabledActions.Remove(preset);
                        Service.ChatGui.Print($"{preset} disabled!");
                    }

                    Service.Configuration.Save();
                    break;
                }

            case "toggle":
                {
                    var targetPreset = argumentsParts[1].ToLowerInvariant();
                    foreach (Presets preset in Enum.GetValues<Presets>())
                    {
                        if (!preset.ToString().Equals(targetPreset, StringComparison.InvariantCultureIgnoreCase))
                        {
                            continue;
                        }

                        if (!Service.Configuration.EnabledActions.Remove(preset))
                        {
                            _ = Service.Configuration.EnabledActions.Add(preset);
                            Service.ChatGui.Print($"{preset} enabled!");
                        }

                        else
                        {
                            Service.ChatGui.Print($"{preset} disabled!");
                        }
                    }

                    Service.Configuration.Save();
                    break;
                }

            case "disableall":
                {
                    foreach (Presets preset in Enum.GetValues<Presets>())
                    {
                        _ = Service.Configuration.EnabledActions.Remove(preset);
                    }

                    Service.ChatGui.Print("All presets disabled!");
                    Service.Configuration.Save();
                    break;
                }

            case "enableall":
                {
                    foreach (Presets preset in Enum.GetValues<Presets>())
                    {
                        _ = Service.Configuration.EnabledActions.Add(preset);
                    }

                    Service.ChatGui.Print("All presets enabled!");
                    Service.Configuration.Save();
                    break;
                }

            case "gcd":
                {
                    Service.Configuration.IgnoreGCDChecks = !Service.Configuration.IgnoreGCDChecks;
                    Service.Configuration.Save();
                    Service.DtrBarEntry.Text = "GCD Counting  " + (Service.Configuration.IgnoreGCDChecks ? "X" : "✓");

                    break;
                }

            case "enabled":
                {
                    foreach (Presets preset in Service.Configuration.EnabledActions.OrderBy(x => x))
                    {
                        if (int.TryParse(preset.ToString(), out var pres))
                        {
                            continue;
                        }

                        Service.ChatGui.Print($"{(int) preset} - {preset}");
                    }

                    break;
                }

            case "debug":
                {
                    try
                    {
                        string specificJob;

                        if (argumentsParts.Length == 2)
                        {
                            specificJob = argumentsParts[1].ToLower();
                        }

                        else
                        {
                            specificJob = "";
                        }

                        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                        using StreamWriter file = new($"{desktopPath}/UltimateComboDebug.txt", append: false);

                        file.WriteLine($"Plugin Version: {GetType().Assembly.GetName().Version}");
                        file.WriteLine("");
                        file.WriteLine($"Installation Repo: {RepoCheckFunctions.FetchCurrentRepo()?.InstalledFromUrl}");
                        file.WriteLine("");
                        file.WriteLine($"Current Job: " +
                            $"{Service.ObjectTable.LocalPlayer?.ClassJob.Value.NameEnglish} / " +
                            $"{Service.ObjectTable.LocalPlayer?.ClassJob.Value.Abbreviation}");
                        file.WriteLine($"Current Job Index: {Service.ObjectTable.LocalPlayer?.ClassJob.Value.RowId}");
                        file.WriteLine($"Current Job Level: {Service.ObjectTable.LocalPlayer?.Level}");
                        file.WriteLine("");
                        file.WriteLine($"Current Zone: {Service.DataManager.GetExcelSheet<Lumina.Excel.Sheets.TerritoryType>()?.FirstOrDefault(x => x.RowId == Service.ClientState.TerritoryType).PlaceName.Value.Name}");
                        file.WriteLine($"Current Party Size: {Service.PartyList.Length}");
                        file.WriteLine("");
                        file.WriteLine($"Enabled Features:");

                        var i = 0;

                        if (string.IsNullOrEmpty(specificJob))
                        {
                            foreach (Presets preset in Service.Configuration.EnabledActions.OrderBy(x => x))
                            {
                                if (int.TryParse(preset.ToString(), out _)) { i++; continue; }

                                file.WriteLine($"{(int) preset} - {preset}");
                            }
                        }

                        else
                        {
                            foreach (Presets preset in Service.Configuration.EnabledActions.OrderBy(x => x))
                            {
                                if (int.TryParse(preset.ToString(), out _)) { i++; continue; }

                                if (preset.ToString()[..3].Equals(specificJob, StringComparison.CurrentCultureIgnoreCase) ||
                                    preset.ToString()[..3].Equals("all", StringComparison.CurrentCultureIgnoreCase) ||
                                    preset.ToString()[..3].Equals("pvp", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    file.WriteLine($"{(int) preset} - {preset}");
                                }
                            }
                        }

                        file.WriteLine("");
                        file.WriteLine("Config Settings");
                        if (string.IsNullOrEmpty(specificJob))
                        {
                            file.WriteLine("---Integers---");
                            foreach (KeyValuePair<string, int> item in PluginConfiguration.CustomIntValues.OrderBy(x => x.Key))
                            {
                                file.WriteLine($"{item.Key.Trim()} - {item.Value}");
                            }
                            file.WriteLine("");
                            file.WriteLine("---Floats---");
                            foreach (KeyValuePair<string, float> item in PluginConfiguration.CustomFloatValues.OrderBy(x => x.Key))
                            {
                                file.WriteLine($"{item.Key.Trim()} - {item.Value}");
                            }
                            file.WriteLine("");
                            file.WriteLine("---Bools---");
                            foreach (KeyValuePair<string, bool> item in PluginConfiguration.CustomBoolValues.OrderBy(x => x.Key))
                            {
                                file.WriteLine($"{item.Key.Trim()} - {item.Value}");
                            }
                            file.WriteLine("");
                            file.WriteLine("---Bool Arrays---");
                            foreach (KeyValuePair<string, bool[]> item in PluginConfiguration.CustomBoolArrayValues.OrderBy(x => x.Key))
                            {
                                file.WriteLine($"{item.Key.Trim()} - {string.Join(", ", item.Value)}");
                            }
                        }

                        else
                        {
                            var jobname = ConfigWindow.GroupedPresets.FirstOrDefault(x => x.Value.Any(y => y.Info.JobShorthand.Equals(specificJob.ToLower(), StringComparison.CurrentCultureIgnoreCase))).Key;
                            var jobID = Service.DataManager.GetExcelSheet<Lumina.Excel.Sheets.ClassJob>()?
                                .Where(x => x.Name.ToString().Equals(jobname, StringComparison.CurrentCultureIgnoreCase))
                                .First()
                                .RowId;

                            Type whichConfig = jobID switch
                            {
                                1 or 19 => typeof(PLD.Config),
                                2 or 20 => typeof(MNK.Config),
                                3 or 21 => typeof(WAR.Config),
                                4 or 22 => typeof(DRG.Config),
                                5 or 23 => typeof(BRD.Config),
                                6 or 24 => typeof(WHM.Config),
                                7 or 25 => typeof(BLM.Config),
                                26 or 27 => typeof(SMN.Config),
                                28 => typeof(SCH.Config),
                                29 or 30 => typeof(NIN.Config),
                                31 => typeof(MCH.Config),
                                32 => typeof(DRK.Config),
                                33 => typeof(AST.Config),
                                34 => typeof(SAM.Config),
                                35 => typeof(RDM.Config),
                                36 => typeof(BLU.Config),
                                37 => typeof(GNB.Config),
                                38 => typeof(DNC.Config),
                                39 => typeof(RPR.Config),
                                40 => typeof(SGE.Config),
                                41 => typeof(VPR.Config),
                                42 => typeof(PCT.Config),
                                _ => throw new NotImplementedException(),
                            };

                            foreach (MemberInfo? config in whichConfig.GetMembers().Where(x => x.MemberType is MemberTypes.Field or MemberTypes.Property))
                            {
                                var key = config.Name!;

                                if (PluginConfiguration.CustomIntValues.TryGetValue(key, out var intvalue)) { file.WriteLine($"{key} - {intvalue}"); continue; }
                                if (PluginConfiguration.CustomFloatValues.TryGetValue(key, out var floatvalue)) { file.WriteLine($"{key} - {floatvalue}"); continue; }
                                if (PluginConfiguration.CustomBoolValues.TryGetValue(key, out var boolvalue)) { file.WriteLine($"{key} - {boolvalue}"); continue; }
                                if (PluginConfiguration.CustomBoolArrayValues.TryGetValue(key, out var boolarrayvalue)) { file.WriteLine($"{key} - {string.Join(", ", boolarrayvalue)}"); continue; }

                                file.WriteLine($"{key} - NOT SET");
                            }

                            foreach (MemberInfo? config in typeof(AllPvP.Config).GetMembers().Where(x => x.MemberType is MemberTypes.Field or MemberTypes.Property))
                            {
                                var key = config.Name!;

                                if (PluginConfiguration.CustomIntValues.TryGetValue(key, out var intvalue)) { file.WriteLine($"{key} - {intvalue}"); continue; }
                                if (PluginConfiguration.CustomFloatValues.TryGetValue(key, out var floatalue)) { file.WriteLine($"{key} - {floatalue}"); continue; }
                                if (PluginConfiguration.CustomBoolValues.TryGetValue(key, out var boolvalue)) { file.WriteLine($"{key} - {boolvalue}"); continue; }
                                if (PluginConfiguration.CustomBoolArrayValues.TryGetValue(key, out var boolarrayvalue)) { file.WriteLine($"{key} - {string.Join(", ", boolarrayvalue)}"); continue; }

                                file.WriteLine($"{key} - NOT SET");
                            }
                        }

                        file.WriteLine("");
                        file.WriteLine($"Redundant IDs found: {i}");
                        if (i > 0)
                        {
                            foreach (Presets preset in Service.Configuration.EnabledActions.Where(x => int.TryParse(x.ToString(), out _)).OrderBy(x => x))
                            {
                                file.WriteLine($"{(int) preset}");
                            }
                        }

                        file.WriteLine("");

                        if (Service.ObjectTable.LocalPlayer?.StatusList is { Length: > 0 } statusList)
                        {
                            file.WriteLine($"Status Effect Count: {statusList.Count(x => x != null)}");
                            foreach (IStatus status in statusList)
                            {
                                file.WriteLine($"ID: {status.StatusId}, Count: {status.Param}, Source: {status.SourceId} Name: {ActionWatching.GetStatusName(status.StatusId)}");
                            }
                        }

                        Service.ChatGui.Print("Please check your desktop for UltimateComboDebug.txt and upload this file when submitting your bug report.");

                        break;
                    }

                    catch (Exception ex)
                    {
                        Service.PluginLog.Error(ex, "Debug Log");
                        Service.ChatGui.Print("Unable to write Debug log. Type /xllog to see why.");
                        break;
                    }
                }

            default:
                _configWindow.IsOpen = !_configWindow.IsOpen;
                PvEWindow.HasToOpenJob = true;
                if (argumentsParts[0].Length > 0)
                {
                    var jobname = ConfigWindow.GroupedPresets.FirstOrDefault(x => x.Value.Any(y => y.Info.JobShorthand.Equals(argumentsParts[0].ToLower(), StringComparison.CurrentCultureIgnoreCase))).Key;
                    var header = $"{jobname} - {argumentsParts[0].ToUpper()}";
                    Service.PluginLog.Debug($"{jobname}");
                    PvEWindow.HeaderToOpen = header;
                }
                break;
        }

        Service.Configuration.Save();
    }

    void IDisposable.Dispose()
    {
        Dispose();
    }
}
