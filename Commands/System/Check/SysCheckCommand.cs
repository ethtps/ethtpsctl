using System.Diagnostics.CodeAnalysis;
using ETHTPS.Control.Commands.Help.System.Check.Executables;
using ETHTPS.Control.Commands.System.Check;
using ETHTPS.Control.Configuration;
using Spectre.Console;
using Spectre.Console.Cli;

namespace ETHTPS.Control.Commands.System.Check
{
	public sealed class SysCheckCommand : Command<SysCheckSettings>
	{
		public override int Execute([NotNull] CommandContext context, [NotNull] SysCheckSettings settings)
		{
			var validationResult = settings.Validate();
			if (!validationResult.Successful)
			{
				AnsiConsole.MarkupLine($"[red]{validationResult.Message}[/]");
				return 1;
			}
			var config = AppConfiguration.FromJSON();
			if (config.Dependencies == null)
			{
				AnsiConsole.MarkupLine("[red]No dependencies found in configuration file.[/]");
				return 0;
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
			var externalDepCheck = config.RunExternalDependencyCheck();
			if (!externalDepCheck)
			{
				AnsiConsole.MarkupLine("[red]External dependency check failed.[/] Please install the missing dependencies and try again.");
			}
			else
			{
				AnsiConsole.MarkupLine("[green]External dependency check passed.[/]");
			}
			var projectDepCheck = config.RunProjectDependencyCheck(settings.ETHTPSBaseDirectory);
			if (!projectDepCheck)
			{
				AnsiConsole.MarkupLine("[red]Project dependency check failed.[/] Please clone the missing projects and try running the check again.");
			}
			else
			{
				AnsiConsole.MarkupLine("[green]Project dependency check passed.[/]");
			}
			return 0;
		}
	}
}