using System.Collections.Generic;

namespace TagsCloudVisualization.Interfaces
{
	public interface IFrequencyCounter
	{
		Dictionary<string, int> MakeFrequencyStatistics(IEnumerable<string> content, int top);
	}
}