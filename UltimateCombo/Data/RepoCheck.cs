using Newtonsoft.Json;
using System.IO;
using UltimateCombo.Core;

namespace UltimateCombo.Data;

internal class RepoCheck
{
    internal string? InstalledFromUrl { get; set; }
}

internal static class RepoCheckFunctions
{
    internal static RepoCheck? FetchCurrentRepo()
    {
        FileInfo f = Service.Interface.AssemblyLocation;
        var manifest = Path.Join(f.DirectoryName, "UltimateCombo.json");
        if (File.Exists(manifest))
        {
            RepoCheck? repo = JsonConvert.DeserializeObject<RepoCheck>(File.ReadAllText(manifest));
            return repo;
        }
        return null;
    }

    internal static bool IsFromUltimateRepo()
    {
        RepoCheck? repo = FetchCurrentRepo();
        return repo is not null
            && repo.InstalledFromUrl is not null
            and "https://raw.githubusercontent.com/StackBLU/UltimateCombo/main/pluginmaster.json";
    }
}
