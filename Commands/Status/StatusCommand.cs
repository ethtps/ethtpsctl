using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using ETHTPS.Control.Configuration;
using Spectre.Console;
using Spectre.Console.Cli;

namespace ETHTPS.Control.Commands.Status
{
	public sealed class StatusCommand : Command<StatusSettings>
	{
		private static readonly Regex EndpointRegex = new(@"^(https?://[^:/]+)(?::(\d+))?$", RegexOptions.Compiled);

		private static async Task<bool> PingAsync(string host, int port)
		{
			var client = new TcpClient();
			try
			{
				await client.ConnectAsync(host, port);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
			finally
			{
				client.Dispose();
			}
		}

		private static async Task<bool> PingAsync(Uri endpoint)
		{
			var client = new TcpClient();
			try
			{
				await client.ConnectAsync(endpoint.Host, endpoint.Port);
				return true;
			}
			catch (Exception)
			{
				return false;
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
			do
			{
				Task.Run(async () =>
				{
					var results = await Task.WhenAll(config.System?.Endpoints.Select(async e => Task.Run(async () =>
					{
						var match = EndpointRegex.Match(e);
						if (!match.Success)
						{
							return (e, false);
						}
						var endpoint = new Uri(e);
						return (e, await PingAsync(endpoint));
					})));
				}).Wait();
			} while (settings.Indefinite && !Console.KeyAvailable);
			return 0;
		}
	}
}
