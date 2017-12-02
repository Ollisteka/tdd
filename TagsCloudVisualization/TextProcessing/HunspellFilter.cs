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
			using (var hunspell =
				new Hunspell(solutiondir + "//dictionaries//ru_RU.aff", solutiondir + "//dictionaries//ru_RU.dic"))
			{
				return content
					.Select(x =>
					{
						var word = x.ToLower();
						var stems = hunspell.Stem(word);
						return stems.Any() ? stems[0] : word;
					})
					.ToList();
			}
		}
	}
}