using System;
using System.Collections.Generic;
using System.Linq;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization.TextProcessing
{
	internal class LongWordsFilter : ITextFiltration
	{
		private readonly int maxLength = int.MaxValue;
		private readonly int minLength = 3;

		public LongWordsFilter()
		{
		}

		public LongWordsFilter(int minLength=3, int maxLength=int.MaxValue)
		{
			this.minLength = minLength;
			this.maxLength = maxLength;
		}

		public IEnumerable<string> Filter(IEnumerable<string> content)
		{
			return content.Where(word => word.Length >= minLength && word.Length <= maxLength);
		}
	}
}