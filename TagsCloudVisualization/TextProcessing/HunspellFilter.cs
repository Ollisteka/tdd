using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NHunspell;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization.TextProcessing
{
	internal class HunspellFilter : ITextFiltration
	{
		private readonly Func<string, bool> predicate;
		private readonly Func<string, string> selector;
		private readonly IEnumerable<string> text;

		public HunspellFilter(ITextReader reader, Func<string, bool> predicate, Func<string, string> selector)
		{
			this.predicate = predicate;
			this.selector = selector;
			text = reader.GetText();
		}

		public IEnumerable<string> Filter()
		{
				return text
					.Where(predicate)
					.Select(selector);
		}
	}
}