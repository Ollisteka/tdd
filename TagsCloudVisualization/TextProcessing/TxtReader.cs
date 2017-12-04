	using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization.TextProcessing
{
	internal class TxtReader : IFileReader
	{
		private readonly IReadOnlyCollection<string> supportedExtensions = new List<string> { ".txt" };
		public bool TryGetText(string filename, out IEnumerable<string> text)
		{
			if (!supportedExtensions.Contains(Path.GetExtension(filename)))
			{
				text = null;
				return false;
			}
			text =  Regex
				.Split(File.ReadAllText(filename), @"[^\p{L}]*\p{Z}[^\p{L}]*")
				.Select(word => word.Trim(Environment.NewLine.ToCharArray()));
			return true;
		}
	}
}