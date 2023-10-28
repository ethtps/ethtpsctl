using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ETHTPS.Control.Configuration;
using Spectre.Console;
using Spectre.Console.Cli;

namespace ETHTPS.Control.Commands.Status
{
	public sealed class StatusCommand : Command<StatusSettings>
	{
		private static async Task<(bool, long)> PingAsync(Uri endpoint, bool verbose = false)
		{
			var client = new TcpClient();
			var stopwatch = Stopwatch.StartNew();
			if (verbose)
			{
				AnsiConsole.MarkupLine($"[grey]Pinging {endpoint}...[/]");
			}
			try
			{
				await client.ConnectAsync(endpoint.Host, endpoint.Port);
				stopwatch.Stop();
				if (verbose)
				{
					AnsiConsole.MarkupLine($"[green]UP[/] ({stopwatch.ElapsedMilliseconds}ms)");
				}
				return (true, stopwatch.ElapsedMilliseconds);
			}
			catch (Exception)
			{
				stopwatch.Stop();
				if (verbose)
				{
					AnsiConsole.MarkupLine($"[red]DOWN[/] ({stopwatch.ElapsedMilliseconds}ms)");
				}
				return (false, stopwatch.ElapsedMilliseconds);
			}
			finally
			{
				client.Dispose();
			}
		}

		public override int Execute([NotNull] CommandContext context, [NotNull] StatusSettings settings)
		{
			var config = AppConfiguration.FromJSON();
			if (config.System?.Endpoints?.Length == 0)
			{
				AnsiConsole.MarkupLine("[red]No endpoints configured.[/]");
				return 1;
			}
			if (settings.Verbose)
			{
				AnsiConsole.MarkupLine("[grey]Endpoints:[/]");
				foreach (var endpoint in config.System?.Endpoints ?? [])
				{
					AnsiConsole.MarkupLine($"[grey]  {endpoint}[/]");
				}
			}
			var endpoints = config.System?.Endpoints;
			if (endpoints == null)
			{
				AnsiConsole.MarkupLine("[red]No endpoints specified.[/]");
				return 1;
			}
			var table = new Table().Centered();
			table.AddColumn("Endpoint");
			table.AddColumn("Status");
			table.AddColumn("Latency (ms)");
			AnsiConsole.Write(new Rule("[grey]Status[/]").RuleStyle("grey").LeftJustified());
			if (settings.Indefinite)
			{

				AnsiConsole.MarkupLine("[white]Press any key to exit.[/]");
			}
			AnsiConsole.Live(table)
					.AutoClear(false)
					.Overflow(VerticalOverflow.Ellipsis)
					.Cropping(VerticalOverflowCropping.Top)
					.StartAsync(async ctx =>
					{
						do
						{
							table.Rows.Clear();
							var results = await Task.WhenAll(endpoints?.Select(e => PingAsync(new Uri(e), !settings.Indefinite)) ?? []);

							for (var i = 0; i < endpoints?.Length; i++)
							{
								table.AddRow(endpoints[i].ToString(),
													results[i].Item1 ? "[green]UP[/]" : "[red]DOWN[/]",
													results[i].Item2.ToString());
							}
							ctx.Refresh();
							if (settings.Indefinite)
							{
								await Task.Delay(2500);
							}
							else
							{
								break;
							}
						} while (settings.Indefinite && !Console.KeyAvailable);
					}).Wait();
			return 0;
		}
	}
}