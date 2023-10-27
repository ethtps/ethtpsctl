using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using ETHTPS.Control.Configuration;
using Spectre.Console;
using Spectre.Console.Cli;

namespace ETHTPS.Control.Commands.DevEnv
{
	public sealed class DevEnvCommand : Command<DevEnvSettings>
	{
		public override int Execute([NotNull] CommandContext context, [NotNull] DevEnvSettings settings)
		{
			var config = AppConfiguration.FromJSON();
			var processes = new List<Process?>();
			settings.ProjectList.ToList().ForEach(project =>
			{
				var p = Process.Start(new ProcessStartInfo(config.Editor!, Path.Combine(settings.ETHTPSBaseDirectory, project))
				{
					UseShellExecute = false,
					RedirectStandardOutput = true,
					CreateNoWindow = false
				});
				processes.Add(p);
			});
			if (settings.Hook)
			{
				AnsiConsole.MarkupLine("--hook is not available yet.");
				/*
				AnsiConsole.MarkupLine($"[green]Opened {settings.ProjectList.Count()} projects for editing.[/]");
				AnsiConsole.MarkupLine($"[green]Press CTRL+C to close all editors.[/]");
				var cancel = false;
				Console.CancelKeyPress += (sender, args) =>
				{
					AnsiConsole.MarkupLine($"[red]Closing all editors...[/]");
					cancel = true;
				};
				while (!cancel)
				{
					Thread.Sleep(1000);
				}
				if (cancel) processes.ForEach(p => p?.CloseMainWindow());
				*/
			}
			return 0;
		}
	}
}
