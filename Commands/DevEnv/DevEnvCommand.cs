using System;
using System.Diagnostics.CodeAnalysis;
using ETHTPS.Control.Configuration;
using Spectre.Console;
using Spectre.Console.Cli;

namespace ETHTPS.Control.Commands.DevEnv
{
	public class DevEnvCommand : Command<DevEnvSettings>
	{
		public override int Execute([NotNull] CommandContext context, [NotNull] DevEnvSettings settings)
		{
			return 0;
		}
	}
}
