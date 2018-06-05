using System.Collections.Generic;
using System.IO;
using System.Linq;
using NHunspell;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization.TextProcessing
{
	internal class HunspellFilter : ITextFilter
	{
		public IEnumerable<string> Filter(IEnumerable<string> content)
		{
			var solutiondir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
			var affFile = Path.Combine(solutiondir, "dictionaries", "en_EN.aff");
			var dicFile = Path.Combine(solutiondir, "dictionaries", "en_EN.dic");
			var hunspell = ResultExtension.Of(() => new Hunspell(affFile, dicFile))
				.RefineError("Something went wrong in a Hunspell Filter: ");
			if (!hunspell.IsSuccess)
				LayoutApp.ExitWithError(hunspell.Error);
			using (hunspell.Value)
			{
				return content
					.Select(x =>
					{
						var word = x.ToLower();
						var stems = hunspell.Value.Stem(word);
						return stems.Any() ? stems[0] : word;
					})
					.ToList();
			}
		}
	}
}