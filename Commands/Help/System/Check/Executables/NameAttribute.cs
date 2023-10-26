namespace ETHTPS.Control.Commands.System.Check.Executables
{
  public class NameAttribute : Attribute
  {
    public NameAttribute(string name)
    {
      Name = name;
    }

    public string Name { get; }
  }
}