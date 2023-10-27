namespace ETHTPS.Control.Configuration
{
	/// <summary>
	/// Describes the parameters for the check of the existence of an executable.
	/// </summary>
	public sealed class ExecutableDescriptor
	{
		public required string Name { get; set; }
		public string? Description { get; set; }
		public required bool Mandatory { get; set; }
		public required string[]? ExtraDefaultDirectories { get; set; }
	}
}