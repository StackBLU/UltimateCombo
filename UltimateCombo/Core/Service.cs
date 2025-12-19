using Dalamud.Game.Gui.Dtr;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using System;
using System.IO;
using System.Reflection;
using UltimateCombo.Data;

namespace UltimateCombo.Core;

internal class Service
{
    // Plugin Services
    internal static PluginAddressResolver Address { get; set; } = null!;
    internal static CustomComboCache ComboCache { get; set; } = null!;
    internal static IconReplacer IconReplacer { get; set; } = null!;
    internal static PluginConfiguration Configuration { get; set; } = null!;

    // Dalamud Services - Core
    [PluginService]
    internal static IDalamudPluginInterface Interface { get; private set; } = null!;
    [PluginService]
    internal static IFramework Framework { get; private set; } = null!;
    [PluginService]
    internal static IPluginLog PluginLog { get; private set; } = null!;

    // Dalamud Services - Game State
    [PluginService]
    internal static IClientState ClientState { get; private set; } = null!;
    [PluginService]
    internal static ICondition Condition { get; private set; } = null!;
    [PluginService]
    internal static IDataManager DataManager { get; private set; } = null!;
    [PluginService]
    internal static ITargetManager TargetManager { get; private set; } = null!;
    [PluginService]
    internal static IObjectTable ObjectTable { get; private set; } = null!;

    // Dalamud Services - UI & Interaction
    [PluginService]
    internal static IChatGui ChatGui { get; private set; } = null!;
    [PluginService]
    internal static ICommandManager CommandManager { get; private set; } = null!;
    [PluginService]
    internal static IGameGui GameGui { get; private set; } = null!;
    internal static IDtrBarEntry DtrBarEntry { get; set; } = null!;

    // Dalamud Services - Character & Party
    [PluginService]
    internal static IJobGauges JobGauges { get; private set; } = null!;
    [PluginService]
    internal static IPartyList PartyList { get; private set; } = null!;
    [PluginService]
    internal static IBuddyList BuddyList { get; private set; } = null!;

    // Dalamud Services - Technical
    [PluginService]
    internal static ISigScanner SigScanner { get; private set; } = null!;
    [PluginService]
    internal static IGameInteropProvider GameInteropProvider { get; private set; } = null!;

    // Utility Properties
    public static string PluginFolder
    {
        get
        {
            var codeBase = Assembly.GetExecutingAssembly().Location;
            UriBuilder uri = new(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path)!;
        }
    }
}
