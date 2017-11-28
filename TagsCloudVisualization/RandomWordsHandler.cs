using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NHunspell;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization
{
	public class RandomWordsHandler : ITextHandler
	{
		private readonly string filename;
		private readonly int top;
		private readonly Random rnd = new Random();

		public RandomWordsHandler(string filename, int top)
		{
			this.filename = filename;
			this.top = top;
		}
		public Dictionary<string, int> MakeFrequencyStatistics()
		{
			var content = File.ReadAllLines(filename);

			using (var hunspell = new Hunspell("dictionaries/ru_RU.aff", "dictionaries/ru_RU.dic"))
			{
				return content
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
						Count = rnd.Next(1000)
					})
					.OrderByDescending(x => x.Count)
					.Take(top)
					.ToDictionary(key => key.KeyField, val => val.Count);
			}

		}
	}
}
