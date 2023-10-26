
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ETHTPS.Control.Commands.System.Check.Executables
{
  public enum ETHTPSExecutable
  {
    [Name("dotnet")]
    [Description("The .NET SDK")]
    [Required]
    DotnetSDK
  }
}