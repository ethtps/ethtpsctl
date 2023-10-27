using System;
using ETHTPS.Control.Commands.Help;
using ETHTPS.Control.Commands.System.Check;
using Spectre.Console;
using Spectre.Console.Cli;

namespace ETHTPS.Control
{
	class Program
	{
		public static int Main(string[] args)
		{
			var app = new CommandApp();
			app.Configure(config =>
			{
				config.AddCommand<SysCheckCommand>("syscheck")
							.WithAlias("sc")
							.WithDescription("Checks whether the current system is configured properly in order to run ETHTPS locally.")
							.WithExample(new[] { "syscheck --base-dir=/path/to/ethtps syscheck [--prompt | -p]" });
			});
			return app.Run(args);
		}
	}
}