using System;
using Spectre.Console;
using Spectre.Console.Cli;

namespace HelloWorld
{
  class Program
  {
    public static int Main(string[] args)
    {
      var app = new CommandApp();
      app.Configure(config =>
      {
        config.AddDelegate("foo", Foo)
                       .WithDescription("Foos the bars");
      });
      return app.Run(args);
    }

    private static int Foo(CommandContext context)
    {
      AnsiConsole.WriteLine("Foo");
      return 0;
    }
  }
}