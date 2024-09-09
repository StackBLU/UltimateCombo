using Dalamud.Game.ClientState.Statuses;
using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Dalamud.Utility;
using ECommons;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UltimateCombo.Attributes;
using UltimateCombo.Combos;
using UltimateCombo.Combos.PvE;
using UltimateCombo.Combos.PvP;
using UltimateCombo.Core;
using UltimateCombo.Data;
using UltimateCombo.Services;
using UltimateCombo.Window;
using UltimateCombo.Window.Tabs;

namespace UltimateCombo
{
	public sealed partial class UltimateComboClass : IDalamudPlugin
	{
		private const string Command = "/uc";

		private readonly ConfigWindow ConfigWindow;
		internal static UltimateComboClass? P = null!;
		internal WindowSystem ws;

		private static uint? jobID;

		public static readonly List<uint> DisabledJobsPVE =
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

			//BLU.JobID,
		];

		public static readonly List<uint> DisabledJobsPVP =
		[
			ASTPvP.JobID,
			//BLMPvP.JobID,
			BRDPvP.JobID,
			DNCPvP.JobID,
			//DRGPvP.JobID,
			DRKPvP.JobID,
			GNBPvP.JobID,
			MCHPvP.JobID,
			//MNKPvP.JobID,
			//NINPvP.JobID,
			PCTPvP.JobID,
			PLDPvP.JobID,
			RDMPvP.JobID,
			//RPRPvP.JobID,
			SAMPvP.JobID,
			//SCHPvP.JobID,
			//SGEPvP.JobID,
			SMNPvP.JobID,
			VPRPvP.JobID,
			WARPvP.JobID,
			//WHMPvP.JobID
		];

		public static uint? JobID
		{
			get
			{
				return jobID;
			}

			set
			{
				if (jobID != value && value != null)
				{
					Service.PluginLog.Debug($"Switched to job {value}");
					PvEFeatures.HasToOpenJob = true;
				}
				jobID = value;
			}
		}

		public UltimateComboClass(IDalamudPluginInterface pluginInterface)
		{
			P = this;
			_ = pluginInterface.Create<Service>();
			ECommonsMain.Init(pluginInterface, this);

			Service.Configuration = pluginInterface.GetPluginConfig() as PluginConfiguration ?? new PluginConfiguration();
			Service.Address = new PluginAddressResolver();
			Service.Address.Setup(Service.SigScanner);
			PresetStorage.Init();

			Service.ComboCache = new CustomComboCache();
			Service.IconReplacer = new IconReplacer();
			ActionWatching.Enable();

			ConfigWindow = new ConfigWindow();
			ws = new();
			ws.AddWindow(ConfigWindow);

			Service.Interface.UiBuilder.Draw += ws.Draw;
			Service.Interface.UiBuilder.OpenConfigUi += OnOpenConfigUi;
			Service.Interface.UiBuilder.OpenMainUi += OnOpenConfigUi;

			_ = Service.CommandManager.AddHandler(Command, new CommandInfo(OnCommand)
			{
				HelpMessage = "Open a window to edit custom combo settings.",
				ShowInHelp = true,
			});

			Service.Framework.Update += OnFrameworkUpdate;

			KillRedundantIDs();
			HandleConflictedCombos();

			ConfigWindow.IsOpen = true;
		}

		private static void HandleConflictedCombos()
		{
			HashSet<CustomComboPreset> enabledCopy = [.. Service.Configuration.EnabledActions];
			foreach (CustomComboPreset preset in enabledCopy)
			{
				if (!PresetStorage.IsEnabled(preset))
				{
					continue;
				}

				ConflictingCombosAttribute? conflictingCombos = preset.GetAttribute<ConflictingCombosAttribute>();
				if (conflictingCombos != null)
				{
					foreach (CustomComboPreset conflict in conflictingCombos.ConflictingPresets)
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
			if (Service.ClientState.LocalPlayer is not null)
			{
				JobID = Service.ClientState.LocalPlayer?.ClassJob?.Id;
			}

			BlueMageService.PopulateBLUSpells();
		}

		private static void KillRedundantIDs()
		{
			List<int> redundantIDs = Service.Configuration.EnabledActions.Where(x => int.TryParse(x.ToString(), out _)).OrderBy(x => x).Cast<int>().ToList();
			foreach (int id in redundantIDs)
			{
				_ = Service.Configuration.EnabledActions.RemoveWhere(x => (int)x == id);
			}

			Service.Configuration.Save();

		}

		private void DrawUI()
		{
			ConfigWindow.Draw();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Used for non-static only window initialization")]
		public string Name
		{
			get
			{
				return "UltimateCombo";
			}
		}

		public void Dispose()
		{
			ConfigWindow?.Dispose();

			ws.RemoveAllWindows();
			_ = Service.CommandManager.RemoveHandler(Command);
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
			ConfigWindow.IsOpen = !ConfigWindow.IsOpen;
		}

		private void OnCommand(string command, string arguments)
		{
			string[]? argumentsParts = arguments.Split();

			switch (argumentsParts[0].ToLower())
			{
				case "unsetall":
					{
						foreach (CustomComboPreset preset in Enum.GetValues<CustomComboPreset>())
						{
							_ = Service.Configuration.EnabledActions.Remove(preset);
						}

						Service.ChatGui.Print("All UNSET");
						Service.Configuration.Save();
						break;
					}

				case "set":
					{
						string? targetPreset = argumentsParts[1].ToLowerInvariant();
						foreach (CustomComboPreset preset in Enum.GetValues<CustomComboPreset>())
						{
							if (!preset.ToString().Equals(targetPreset, StringComparison.InvariantCultureIgnoreCase))
							{
								continue;
							}

							_ = Service.Configuration.EnabledActions.Add(preset);
							Service.ChatGui.Print($"{preset} SET");
						}

						Service.Configuration.Save();
						break;
					}

				case "toggle":
					{
						string? targetPreset = argumentsParts[1].ToLowerInvariant();
						foreach (CustomComboPreset preset in Enum.GetValues<CustomComboPreset>())
						{
							if (!preset.ToString().Equals(targetPreset, StringComparison.InvariantCultureIgnoreCase))
							{
								continue;
							}

							if (!Service.Configuration.EnabledActions.Remove(preset))
							{
								_ = Service.Configuration.EnabledActions.Add(preset);
								Service.ChatGui.Print($"{preset} SET");
							}
							else
							{
								Service.ChatGui.Print($"{preset} UNSET");
							}
						}

						Service.Configuration.Save();
						break;
					}

				case "unset":
					{
						string? targetPreset = argumentsParts[1].ToLowerInvariant();
						foreach (CustomComboPreset preset in Enum.GetValues<CustomComboPreset>())
						{
							if (!preset.ToString().Equals(targetPreset, StringComparison.InvariantCultureIgnoreCase))
							{
								continue;
							}

							_ = Service.Configuration.EnabledActions.Remove(preset);
							Service.ChatGui.Print($"{preset} UNSET");
						}

						Service.Configuration.Save();
						break;
					}

				case "enabled":
					{
						foreach (CustomComboPreset preset in Service.Configuration.EnabledActions.OrderBy(x => x))
						{
							if (int.TryParse(preset.ToString(), out int pres))
							{
								continue;
							}

							Service.ChatGui.Print($"{(int)preset} - {preset}");
						}

						break;
					}

				case "debug":
					{
						try
						{
							string? specificJob = argumentsParts.Length == 2 ? argumentsParts[1].ToLower() : "";

							string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

							using StreamWriter file = new($"{desktopPath}/StackDebug.txt", append: false);

							file.WriteLine("START DEBUG LOG");
							file.WriteLine("");
							file.WriteLine($"Plugin Version: {GetType().Assembly.GetName().Version}");
							file.WriteLine("");
							file.WriteLine($"Installation Repo: {RepoCheckFunctions.FetchCurrentRepo()?.InstalledFromUrl}");
							file.WriteLine("");
							file.WriteLine($"Current Job: " +
								$"{Service.ClientState.LocalPlayer.ClassJob.GameData.Name} / " +
								$"{Service.ClientState.LocalPlayer.ClassJob.GameData.NameEnglish} / " +
								$"{Service.ClientState.LocalPlayer.ClassJob.GameData.Abbreviation}");
							file.WriteLine($"Current Job Index: {Service.ClientState.LocalPlayer.ClassJob.Id}");
							file.WriteLine($"Current Job Level: {Service.ClientState.LocalPlayer.Level}");
							file.WriteLine("");
							file.WriteLine($"Current Zone: {Service.DataManager.GetExcelSheet<Lumina.Excel.GeneratedSheets.TerritoryType>()?.FirstOrDefault(x => x.RowId == Service.ClientState.TerritoryType).PlaceName.Value.Name}");
							file.WriteLine($"Current Party Size: {Service.PartyList.Length}");
							file.WriteLine("");
							file.WriteLine($"START ENABLED FEATURES");

							int i = 0;
							if (string.IsNullOrEmpty(specificJob))
							{
								foreach (CustomComboPreset preset in Service.Configuration.EnabledActions.OrderBy(x => x))
								{
									if (int.TryParse(preset.ToString(), out _)) { i++; continue; }

									file.WriteLine($"{(int)preset} - {preset}");
								}
							}

							else
							{
								foreach (CustomComboPreset preset in Service.Configuration.EnabledActions.OrderBy(x => x))
								{
									if (int.TryParse(preset.ToString(), out _)) { i++; continue; }

									if (preset.ToString()[..3].Equals(specificJob, StringComparison.CurrentCultureIgnoreCase) ||
										preset.ToString()[..3].Equals("all", StringComparison.CurrentCultureIgnoreCase) ||
										preset.ToString()[..3].Equals("pvp", StringComparison.CurrentCultureIgnoreCase))
									{
										file.WriteLine($"{(int)preset} - {preset}");
									}
								}
							}


							file.WriteLine($"END ENABLED FEATURES");
							file.WriteLine("");

							file.WriteLine("START CONFIG SETTINGS");
							if (string.IsNullOrEmpty(specificJob))
							{
								file.WriteLine("---INT VALUES---");
								foreach (KeyValuePair<string, int> item in PluginConfiguration.CustomIntValues.OrderBy(x => x.Key))
								{
									file.WriteLine($"{item.Key.Trim()} - {item.Value}");
								}
								file.WriteLine("");
								file.WriteLine("---FLOAT VALUES---");
								foreach (KeyValuePair<string, float> item in PluginConfiguration.CustomFloatValues.OrderBy(x => x.Key))
								{
									file.WriteLine($"{item.Key.Trim()} - {item.Value}");
								}
								file.WriteLine("");
								file.WriteLine("---BOOL VALUES---");
								foreach (KeyValuePair<string, bool> item in PluginConfiguration.CustomBoolValues.OrderBy(x => x.Key))
								{
									file.WriteLine($"{item.Key.Trim()} - {item.Value}");
								}
								file.WriteLine("");
								file.WriteLine("---BOOL ARRAY VALUES---");
								foreach (KeyValuePair<string, bool[]> item in PluginConfiguration.CustomBoolArrayValues.OrderBy(x => x.Key))
								{
									file.WriteLine($"{item.Key.Trim()} - {string.Join(", ", item.Value)}");
								}
							}
							else
							{
								string jobname = ConfigWindow.groupedPresets.Where(x => x.Value.Any(y => y.Info.JobShorthand.Equals(specificJob.ToLower(), StringComparison.CurrentCultureIgnoreCase))).FirstOrDefault().Key;
								uint? jobID = Service.DataManager.GetExcelSheet<Lumina.Excel.GeneratedSheets.ClassJob>()?
									.Where(x => x.Name.RawString.Equals(jobname, StringComparison.CurrentCultureIgnoreCase))
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
									31 => typeof(MCH.Config),
									32 => typeof(DRK.Config),
									33 => typeof(AST.Config),
									34 => typeof(SAM.Config),
									35 => typeof(RDM.Config),
									37 => typeof(GNB.Config),
									38 => typeof(DNC.Config),
									39 => typeof(RPR.Config),
									40 => typeof(SGE.Config),
									_ => throw new NotImplementedException(),
								};

								foreach (MemberInfo? config in whichConfig.GetMembers().Where(x => x.MemberType is MemberTypes.Field or MemberTypes.Property))
								{
									string key = config.Name!;

									if (PluginConfiguration.CustomIntValues.TryGetValue(key, out int intvalue)) { file.WriteLine($"{key} - {intvalue}"); continue; }
									if (PluginConfiguration.CustomFloatValues.TryGetValue(key, out float floatvalue)) { file.WriteLine($"{key} - {floatvalue}"); continue; }
									if (PluginConfiguration.CustomBoolValues.TryGetValue(key, out bool boolvalue)) { file.WriteLine($"{key} - {boolvalue}"); continue; }
									if (PluginConfiguration.CustomBoolArrayValues.TryGetValue(key, out bool[]? boolarrayvalue)) { file.WriteLine($"{key} - {string.Join(", ", boolarrayvalue)}"); continue; }

									file.WriteLine($"{key} - NOT SET");
								}

								foreach (MemberInfo? config in typeof(PvPCommon.Config).GetMembers().Where(x => x.MemberType is MemberTypes.Field or MemberTypes.Property))
								{
									string key = config.Name!;

									if (PluginConfiguration.CustomIntValues.TryGetValue(key, out int intvalue)) { file.WriteLine($"{key} - {intvalue}"); continue; }
									if (PluginConfiguration.CustomFloatValues.TryGetValue(key, out float floatalue)) { file.WriteLine($"{key} - {floatalue}"); continue; }
									if (PluginConfiguration.CustomBoolValues.TryGetValue(key, out bool boolvalue)) { file.WriteLine($"{key} - {boolvalue}"); continue; }
									if (PluginConfiguration.CustomBoolArrayValues.TryGetValue(key, out bool[]? boolarrayvalue)) { file.WriteLine($"{key} - {string.Join(", ", boolarrayvalue)}"); continue; }

									file.WriteLine($"{key} - NOT SET");
								}
							}


							file.WriteLine("END CONFIG SETTINGS");
							file.WriteLine("");
							file.WriteLine($"Redundant IDs found: {i}");

							if (i > 0)
							{
								file.WriteLine($"START REDUNDANT IDs");
								foreach (CustomComboPreset preset in Service.Configuration.EnabledActions.Where(x => int.TryParse(x.ToString(), out _)).OrderBy(x => x))
								{
									file.WriteLine($"{(int)preset}");
								}

								file.WriteLine($"END REDUNDANT IDs");
								file.WriteLine("");
							}

							file.WriteLine($"Status Effect Count: {Service.ClientState.LocalPlayer.StatusList.Count(x => x != null)}");

							if (Service.ClientState.LocalPlayer.StatusList.Length > 0)
							{
								file.WriteLine($"START STATUS EFFECTS");
								foreach (Status? status in Service.ClientState.LocalPlayer.StatusList)
								{
									file.WriteLine($"ID: {status.StatusId}, COUNT: {status.StackCount}, SOURCE: {status.SourceId} NAME: {ActionWatching.GetStatusName(status.StatusId)}");
								}

								file.WriteLine($"END STATUS EFFECTS");
							}

							file.WriteLine("END DEBUG LOG");
							Service.ChatGui.Print("Please check your desktop for StackDebug.txt and upload this file where requested.");

							break;
						}

						catch (Exception ex)
						{
							Service.PluginLog.Error(ex, "Debug Log");
							Service.ChatGui.Print("Unable to write Debug log.");
							break;
						}
					}
				default:
					ConfigWindow.IsOpen = !ConfigWindow.IsOpen;
					PvEFeatures.HasToOpenJob = true;
					if (argumentsParts[0].Length > 0)
					{
						string jobname = ConfigWindow.groupedPresets.Where(x => x.Value.Any(y => y.Info.JobShorthand.Equals(argumentsParts[0].ToLower(), StringComparison.CurrentCultureIgnoreCase))).FirstOrDefault().Key;
						string header = $"{jobname} - {argumentsParts[0].ToUpper()}";
						Service.PluginLog.Debug($"{jobname}");
						PvEFeatures.HeaderToOpen = header;
					}
					break;
			}

			Service.Configuration.Save();
		}
	}
}