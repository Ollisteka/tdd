using System.Collections.Generic;
using System.Linq;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization.TextProcessing
{
	internal class FrequencyCounter : IFrequencyCounter
	{
		public Dictionary<string, int> MakeFrequencyStatistics(IEnumerable<string> content, int top)
		{
			return content.GroupBy(x => x)
				.Select(x => new
				{
					KeyField = x.Key,
					Count = x.Count()
				})
				.OrderByDescending(x => x.Count)
				.Take(top)
				.ToDictionary(key => key.KeyField, val => val.Count);
		}
	}
}