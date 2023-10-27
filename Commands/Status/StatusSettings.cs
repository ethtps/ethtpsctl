using System;
using System.ComponentModel;
using Spectre.Console.Cli;

namespace ETHTPS.Control.Commands.Status
{
	public sealed class StatusSettings : CommandSettings
	{
		[CommandOption("-i|--indefinite", IsHidden = false)]
		[Description("Run indefinitely")]
		[DefaultValue(false)]
		public bool Indefinite { get; set; }
	}
}
