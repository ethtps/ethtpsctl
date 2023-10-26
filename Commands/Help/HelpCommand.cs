using System.Diagnostics.CodeAnalysis;
using Spectre.Console.Cli;

namespace ETHTPS.Control.Commands.Help
{
  public sealed class HelpCommand : Command<HelpSettings>
  {
    public override int Execute([NotNull] CommandContext context, [NotNull] HelpSettings settings)
    {
      Console.WriteLine("Help");
      return 0;
    }
  }
}