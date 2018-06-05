using System.Collections.Generic;
using System.Linq;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization.TextProcessing
{
	internal class LongWordsFilter : ITextFilter
	{
		private readonly ISettings settings;

		public LongWordsFilter(ISettings settings)
		{
			this.settings = settings;
		}

		private int MaxLength => settings.MaxWordLength;
		private int MinLength => settings.MinWordLength;


		public IEnumerable<string> Filter(IEnumerable<string> content)
		{
			return content.Where(word => word.Length >= MinLength && word.Length <= MaxLength);
		}
	}
}