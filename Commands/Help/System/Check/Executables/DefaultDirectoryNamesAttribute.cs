
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ETHTPS.Control.Commands.System.Check.Executables
{
  public class DefaultDirectoryNamesAttribute : Attribute
  {
    public string[] Directories { get; }

    public DefaultDirectoryNamesAttribute(string[] directories)
    {
      if (directories is null)
      {
        throw new ArgumentNullException(nameof(directories));
      }

      this.Directories = directories;
    }
  }
}