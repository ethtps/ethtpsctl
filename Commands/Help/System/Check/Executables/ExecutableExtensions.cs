using ETHTPS.Control.Configuration;
using Spectre.Console;

namespace ETHTPS.Control.Commands.Help.System.Check.Executables
{
	public static class ExecutableExtensions
	{
		public static bool RunExternalDependencyCheck(this AppConfig config)
		{
			var table = new Table();
			table.Title = new TableTitle("External dependency check");

			table.AddColumn("Application");
			table.AddColumn(new TableColumn("Status").Centered());

			var splitChar = Environment.OSVersion.Platform == PlatformID.Win32NT ? ';' : ':';
			var directoriesToCheck = config.DefaultInstallationDirectories?.ToList().Concat(Environment.GetEnvironmentVariable("PATH")?.Split(splitChar) ?? []).Distinct().ToList();
			if (directoriesToCheck == null)
			{
				AnsiConsole.MarkupLine("[red]No default installation directories found either in the configuration file or in the $PATH variable.[/]");
				return false;
			}

			bool ok = true;
			foreach (var app in config.Dependencies ?? [])
			{
				var allDirs = directoriesToCheck.Concat(app.ExtraDefaultDirectories ?? []).ToList();
				bool isAppFound = allDirs.Any(dir => File.Exists(Path.Combine(dir, app.Name)) || File.Exists(Path.Combine(dir, app.Name + ".exe")));
				if (app.Mandatory)
				{
					ok &= isAppFound;
				}
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
			return ok;
		}

		public static bool RunProjectDependencyCheck(this AppConfig config, string baseDirectory)
		{
			var table = new Table();
			table.Title = new TableTitle("Project dependency check");

			table.AddColumn("Project");
			table.AddColumn(new TableColumn("Status").Centered());

			bool ok = true;
			foreach (var project in config.ProjectDependencies ?? [])
			{
				var projectDir = Path.Combine(baseDirectory, project.Name);
				if (Directory.Exists(projectDir))
				{
					table.AddRow(project.Name, "[green]Found[/]");
				}
				else
				{
					ok = false;
					table.AddRow(project.Name, "[red]Not Found[/]");
				}
			}
			AnsiConsole.Write(table);
			return ok;
		}
	}
}
