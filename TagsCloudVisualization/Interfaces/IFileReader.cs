using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace TagsCloudVisualization.Interfaces
{
	public interface IFileReader
	{
		Result<IEnumerable<string>> TryGetText(string filename);
	}
}