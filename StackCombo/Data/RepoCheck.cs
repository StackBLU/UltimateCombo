using Newtonsoft.Json;
using StackCombo.Services;
using System.IO;

namespace StackCombo.Data
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
			string manifest = Path.Join(f.DirectoryName, "StackCombo.json");

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
			and "https://raw.githubusercontent.com/StackBLU/StackCombo/main/pluginmaster.json";
		}
	}
}
