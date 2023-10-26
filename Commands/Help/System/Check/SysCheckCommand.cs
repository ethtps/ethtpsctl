using System.Diagnostics.CodeAnalysis;
using ETHTPS.Configuration;
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
			var dependencies = config.Dependencies;
			if (dependencies == null)
			{
				AnsiConsole.MarkupLine("[red]No dependencies found in configuration file.[/]");
				return 1;
			}
			if (settings.Prompt)
			{
				var fruits = AnsiConsole.Prompt(
		new MultiSelectionPrompt<string>()
				.Title("Which components should be checked?")
				.PageSize(15)
				.MoreChoicesText("[grey](Move up and down to show more options)[/]")
				.InstructionsText(
						"Press [blue]<space>[/] to (un)select a component, " +
						"[green]<enter>[/] to start)")
				.AddChoices(dependencies.Select(d => d.Name).ToArray()));

				dependencies = dependencies.Where(d => fruits.Contains(d.Name)).ToArray();
			}
			var directoriesToCheck = config.DefaultInstallationDirectories?.ToList().Concat(Environment.GetEnvironmentVariable("PATH")?.Split(';') ?? []).Distinct().ToList();
			if (directoriesToCheck == null)
			{
				AnsiConsole.MarkupLine("[red]No default installation directories found either in the configuration file or the $PATH variable.[/]");
				return 1;
			}
			var rows = new List<Text>();
			foreach (var app in dependencies)
			{
				var allDirs = directoriesToCheck.Concat(app.ExtraDefaultDirectories ?? []).ToList();
				if (!allDirs.Any(dir => File.Exists(Path.Combine(dir, app.Name)) || File.Exists(Path.Combine(dir, app.Name + ".exe"))))
				{
					if (app.Mandatory)
					{
						rows.Add(new Text(app.Name, new Style(foreground: Color.Red)));
					}
					else
					{
						rows.Add(new Text(app.Name, new Style(foreground: Color.Yellow)));
					}
					continue;
				}
				rows.Add(new Text(app.Name, new Style(foreground: Color.Green)));
			}
			AnsiConsole.Write(new Rows(rows));
			return 0;
		}
	}
}