using System;
using System.ComponentModel;
using Spectre.Console.Cli;

namespace ETHTPS.Control.Commands.System
{
	public interface IVerbose
	{
		[CommandOption("-v|--verbose", IsHidden = false)]
		[Description("Show verbose output")]
		[DefaultValue(false)]
		public bool Verbose { get; set; }
	}
}
