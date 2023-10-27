using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace ETHTPS.Control.Commands.System.Check
{
	public sealed class SysCheckSettings : CommandSettings
	{
		[CommandOption("-p|--prompt", IsHidden = false)]
		[DefaultValue(false)]
		[Description("Show a prompt displaying what exactly should be checked")]
		public bool Prompt { get; set; }

		[CommandOption("-b|--base-directory", IsHidden = false)]
		[Description("The directory where ETHTPS code should reside")]
		public required string ETHTPSBaseDirectory { get; set; }

		public override ValidationResult Validate()
		{
			if (string.IsNullOrWhiteSpace(ETHTPSBaseDirectory))
			{
				if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("ETHTPS_HOME")))
				{
					return ValidationResult.Error("A base directory must be specified using the --base-directory parameter or by setting the ETHTPS_HOME environment variable.");
				}
				ETHTPSBaseDirectory = Environment.GetEnvironmentVariable("ETHTPS_HOME")!;
			}
			if (!Directory.Exists(ETHTPSBaseDirectory))
			{
				return ValidationResult.Error("The specified base directory does not exist.");
			}
			return base.Validate();
		}
	}
}