using System;
using System.Collections.Generic;
using System.Linq;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization
{
	internal class FrequencyCounter : IFrequencyCounter
	{
		private readonly IEnumerable<string> text;
		private readonly int top;

		public FrequencyCounter(ITextFiltration filtrator, int top)
		{
			text = filtrator.Filter();
			this.top = top;
		}

		public Dictionary<string, int> MakeFrequencyStatistics()
		{
			return text.GroupBy(x => x)
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