using Newtonsoft.Json;
using System;
using System.IO;
using UltimateCombo.Core;

namespace UltimateCombo.Data;

internal sealed class RepoCheck
{
    [JsonProperty(nameof(InstalledFromUrl))]
    internal string? InstalledFromUrl { get; set; }
}

internal static class RepoCheckFunctions
{
    private const string UltimateRepoUrl = "https://raw.githubusercontent.com/StackBLU/UltimateCombo/main/pluginmaster.json";

    internal static RepoCheck? FetchCurrentRepo()
    {
        FileInfo assemblyFile = Service.Interface.AssemblyLocation;
        var manifestPath = Path.Join(assemblyFile.DirectoryName!, "UltimateCombo.json");

        if (!File.Exists(manifestPath))
        {
            return null;
        }

        try
        {
            return JsonConvert.DeserializeObject<RepoCheck>(File.ReadAllText(manifestPath));
        }
        catch (Exception ex)
        {
            Service.PluginLog.Warning(ex, "Unable to read the UltimateCombo manifest");
            return null;
        }
    }

    internal static bool IsFromUltimateRepo()
    {
        return string.Equals(FetchCurrentRepo()?.InstalledFromUrl, UltimateRepoUrl, StringComparison.Ordinal);
    }
}
