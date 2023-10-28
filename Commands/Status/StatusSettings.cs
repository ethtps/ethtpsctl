using System;
using System.ComponentModel;
using ETHTPS.Control.Commands.System;
using Spectre.Console.Cli;

namespace ETHTPS.Control.Commands.Status
{
	public sealed class StatusSettings : CommandSettings, IVerbose
	{
		[CommandOption("-i|--indefinite", IsHidden = false)]
		[Description("Run indefinitely")]
		[DefaultValue(false)]
		public bool Indefinite { get; set; }

		public bool Verbose { get; set; }
	}
}
