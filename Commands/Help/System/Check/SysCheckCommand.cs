using System.Diagnostics.CodeAnalysis;
using ETHTPS.Configuration;
using ETHTPS.Control.Commands.Help.System.Check.Executables;
using ETHTPS.Control.Commands.System.Check;
using Spectre.Console;
using Spectre.Console.Cli;

namespace ETHTPS.Control.Commands.System.Check
{
	public sealed class SysCheckCommand : Command<SysCheckSettings>
	{
		public override int Execute([NotNull] CommandContext context, [NotNull] SysCheckSettings settings)
		{
			var config = AppConfig.FromJSON();
			if (config.Dependencies == null)
			{
				AnsiConsole.MarkupLine("[red]No config.Dependencies found in configuration file.[/]");
				return 1;
			}
			if (settings.Prompt)
			{
				var manualSelection = AnsiConsole.Prompt(
		new MultiSelectionPrompt<string>()
				.Title("Which components should be checked?")
				.PageSize(15)
				.MoreChoicesText("[grey](Move up and down to show more options)[/]")
				.InstructionsText(
						"Press [blue]<space>[/] to (un)select a component, " +
						"[green]<enter>[/] to start)")
				.AddChoices(config.Dependencies?.Select(d => d.Name).ToArray() ?? []));

				config.Dependencies = config.Dependencies?.Where(d => manualSelection.Contains(d.Name)).ToArray();
			}
			config.RunDependencyCheck();
			return 0;
		}
	}
}