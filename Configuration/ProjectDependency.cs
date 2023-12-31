using System;

namespace ETHTPS.Control.Configuration
{
	public sealed class ProjectDependency
	{
		public required string Name { get; set; }
		public string? Description { get; set; }
		public string[]? Aliases { get; set; }
		public required string RepositoryURL { get; set; }
		public string? DefaultBranch { get; set; }
		public string? EntryPoint { get; set; }
	}
}
