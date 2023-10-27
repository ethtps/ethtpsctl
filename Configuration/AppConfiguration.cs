using System;
using ethtpsctl.Configuration;
using Newtonsoft.Json;

namespace ETHTPS.Control.Configuration
{
	public sealed class AppConfiguration
	{
		public static AppConfiguration FromJSON(string filename = "config.json")
		{
			if (!File.Exists(filename))
			{
				throw new FileNotFoundException($"Could not find configuration file {filename}.");
			}
			var json = File.ReadAllText(filename);
			var config = JsonConvert.DeserializeObject<AppConfiguration>(json) ?? throw new JsonException($"Could not deserialize configuration file {filename}.");
			return config;
		}
		public ExecutableDescriptor[]? Dependencies { get; set; }
		public string[]? DefaultInstallationDirectories { get; set; }
		public ProjectDependency[]? ProjectDependencies { get; set; }
		public SystemConfiguration? System { get; set; }
		public string? Editor { get; set; }
		public Dictionary<string, string[]> BranchAliases { get; set; } = new();
		public Dictionary<string, string[]> ProjectAliases => ProjectDependencies?.ToDictionary(p => p.Name, p => p.Aliases ?? [p.Name]) ?? new();
	}
}
