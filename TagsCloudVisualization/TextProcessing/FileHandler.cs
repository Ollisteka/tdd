using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization.TextProcessing
{
	internal class FileHandler : ITextReader
	{
		private readonly string filename;

		public FileHandler(string filename)
		{
			this.filename = filename;
		}

		public IEnumerable<string> GetText()
		{
			return Regex.Split(File.ReadAllText(filename), @"[^\p{L}]*\p{Z}[^\p{L}]*");
		}
	}
}