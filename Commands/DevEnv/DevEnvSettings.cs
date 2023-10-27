using System;
using System.ComponentModel;
using ETHTPS.Control.Configuration;
using Spectre.Console;
using Spectre.Console.Cli;

namespace ETHTPS.Control.Commands.DevEnv
{
	public class DevEnvSettings : ETHTPSCommandSettingsBase
	{
		[CommandOption("-p|--projects", IsHidden = false)]
		[DefaultValue(false)]
		[Description("Which projects should be opened for editing")]
		public required string Projects { get; set; }

		internal IEnumerable<string> ProjectList
		{
			get => Projects.Contains(',') ? Projects.Split(',') : [Projects];
			set => Projects = string.Join(',', value);
		}

		[CommandOption("--branch", IsHidden = false)]
		[DefaultValue("dev")]
		[Description("Which branch should be checked out")]
		public required string Branch { get; set; }

		public override ValidationResult Validate()
		{
			var result = base.Validate();
			if (!result.Successful) return result;
			if (Projects.Length == 0)
			{
				return ValidationResult.Error("At least one project must be specified using the --project(s) parameter.");
			}
			var config = AppConfig.FromJSON();
			Dictionary<string, string> reverseLookup = new();
			foreach (var kvp in config.ProjectAliases)
			{
				foreach (var value in kvp.Value)
				{
					reverseLookup[value] = kvp.Key;
				}
			}
			ProjectList = ProjectList.Select(p => reverseLookup.ContainsKey(p) ? reverseLookup[p] : p).ToList();
			foreach (var project in ProjectList)
			{
				if (!Directory.Exists(Path.Combine(ETHTPSBaseDirectory, project)))
				{
					return ValidationResult.Error($"The specified project ({project}) does not exist.");
				}
			}
			return ValidationResult.Success();
		}
	}
}