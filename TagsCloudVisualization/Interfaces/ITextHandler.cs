using System.Collections.Generic;

namespace TagsCloudVisualization.Interfaces
{
	public interface ITextHandler
	{
		Dictionary<string, int> MakeFrequencyStatistics();
	}
}