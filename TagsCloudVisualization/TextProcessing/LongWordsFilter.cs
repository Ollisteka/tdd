using System.Collections.Generic;
using System.Linq;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization.TextProcessing
{
	internal class LongWordsFilter : ITextFiltration
	{
		private readonly ISettings settings;

		public LongWordsFilter(ISettings settings)
		{
			this.settings = settings;
		}

		private int MaxLength => settings.MaxLength;
		private int MinLength => settings.MinLength;


		public IEnumerable<string> Filter(IEnumerable<string> content)
		{
			return content.Where(word => word.Length >= MinLength && word.Length <= MaxLength);
		}
	}
}