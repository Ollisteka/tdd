using System.Collections.Generic;

namespace TagsCloudVisualization.Interfaces
{
	public interface ITextFilter
	{
		IEnumerable<string> Filter(IEnumerable<string> content);
	}
}