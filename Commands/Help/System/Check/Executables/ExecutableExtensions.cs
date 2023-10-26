using ETHTPS.Configuration;
using Spectre.Console;

namespace ETHTPS.Control.Commands.Help.System.Check.Executables
{
	public static class ExecutableExtensions
	{
		public static void RunDependencyCheck(this AppConfig config)
		{
			var table = new Table();
			table.Title = new TableTitle("Dependency check");

			table.AddColumn("Application");
			table.AddColumn(new TableColumn("Status").Centered());

			var splitChar = Environment.OSVersion.Platform == PlatformID.Win32NT ? ';' : ':';
			var directoriesToCheck = config.DefaultInstallationDirectories?.ToList().Concat(Environment.GetEnvironmentVariable("PATH")?.Split(splitChar) ?? []).Distinct().ToList();
			if (directoriesToCheck == null)
			{
				AnsiConsole.MarkupLine("[red]No default installation directories found either in the configuration file or in the $PATH variable.[/]");
				return;
			}

			foreach (var app in config.Dependencies ?? [])
			{
				var allDirs = directoriesToCheck.Concat(app.ExtraDefaultDirectories ?? []).ToList();
				bool isAppFound = allDirs.Any(dir => File.Exists(Path.Combine(dir, app.Name)) || File.Exists(Path.Combine(dir, app.Name + ".exe")));

				if (isAppFound)
				{
					table.AddRow(app.Name, "[green]Found[/]");
				}
				else
				{
					table.AddRow(app.Name, app.Mandatory ? "[red]Not Found (required)[/]" : "[yellow]Not Found (optional)[/]");
				}
			}
			AnsiConsole.Write(table);
		}
	}
}
