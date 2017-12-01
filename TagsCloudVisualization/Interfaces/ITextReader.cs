using System.Collections.Generic;

namespace TagsCloudVisualization.Interfaces
{
	public interface ITextReader
	{
		IEnumerable<string> GetText();
	}
}