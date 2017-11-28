using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NHunspell;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization
{
	internal class FileHandler : ITextHandler
	{
		private readonly string filename;

		public FileHandler(string filename)
		{
			this.filename = filename;
		}

		public Dictionary<string, int> MakeFrequencyStatistics()
		{
			var content = File.ReadAllText(filename);

			using (var hunspell = new Hunspell("dictionaries/ru_RU.aff", "dictionaries/ru_RU.dic"))
			{
				return Regex.Split(content, @"[^\p{L}]*\p{Z}[^\p{L}]*")
					.Where(x => x.Length > 3)
					.Select(x =>
					{
						var word = x.ToLower();
						//var morphs = hunspell.Analyze(word);
						var stems = hunspell.Stem(word);
						return stems.Any() ? stems[0] : word;
					})
					.GroupBy(x => x)
					.Select(x => new
					{
						KeyField = x.Key,
						Count = x.Count()
					})
					.OrderByDescending(x => x.Count)
					.Take(50).ToDictionary(key => key.KeyField, val => val.Count);
			}
		}
	}
}