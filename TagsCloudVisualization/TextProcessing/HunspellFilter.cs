using System.Collections.Generic;
using System.IO;
using System.Linq;
using NHunspell;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization.TextProcessing
{
	internal class HunspellFilter : ITextFiltration
	{
		public IEnumerable<string> Filter(IEnumerable<string> content)
		{
			var solutiondir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
			var hunspell = ResultExtension.Of(() => new Hunspell(
					solutiondir + "//dictionaries//ru_RU.aff", solutiondir + "//dictionaries//ru_RU.dic"))
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