using System;
using Newtonsoft.Json;

namespace ETHTPS.Configuration
{
  public sealed class AppConfig
  {
    public static AppConfig FromJSON(string filename = "config.json")
    {
      if (!File.Exists(filename))
      {
        throw new FileNotFoundException($"Could not find configuration file {filename}.");
      }
      var json = File.ReadAllText(filename);
      var config = JsonConvert.DeserializeObject<AppConfig>(json) ?? throw new JsonException($"Could not deserialize configuration file {filename}.");
      return config;
    }
    public ExecutableDescriptor[]? Dependencies { get; set; }
    public string[]? DefaultInstallationDirectories { get; set; }
  }
}
