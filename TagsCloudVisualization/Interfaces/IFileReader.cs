using System.Collections.Generic;

namespace TagsCloudVisualization.Interfaces
{
	public interface IFileReader
	{
		bool TryGetText(string filename, out IEnumerable<string> text);
	}
}