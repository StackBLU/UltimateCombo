using Newtonsoft.Json;
using UltimateCombo.Services;
using System.IO;

namespace UltimateCombo.Data
{
	public class RepoCheck
	{
		public string? InstalledFromUrl { get; set; }
	}

	public static class RepoCheckFunctions
	{
		public static RepoCheck? FetchCurrentRepo()
		{
			FileInfo? f = Service.Interface.AssemblyLocation;
			string manifest = Path.Join(f.DirectoryName, "UltimateCombo.json");

			if (File.Exists(manifest))
			{
				RepoCheck? repo = JsonConvert.DeserializeObject<RepoCheck>(File.ReadAllText(manifest));
				return repo;
			}
			else
			{
				return null;
			}

		}

		public static bool IsFromSlothRepo()
		{
			RepoCheck? repo = FetchCurrentRepo();
			return repo is not null
			&& repo.InstalledFromUrl is not null
			and "https://raw.githubusercontent.com/StackBLU/UltimateCombo/main/pluginmaster.json";
		}
	}
}
