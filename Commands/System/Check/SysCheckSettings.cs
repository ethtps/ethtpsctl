using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace ETHTPS.Control.Commands.System.Check
{
	public sealed class SysCheckSettings : ETHTPSCommandSettingsBase
	{
		[CommandOption("-p|--prompt", IsHidden = false)]
		[DefaultValue(false)]
		[Description("Show a prompt displaying what exactly should be checked")]
		public bool Prompt { get; set; }
	}
}