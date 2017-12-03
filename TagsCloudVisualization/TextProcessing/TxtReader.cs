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
		public IEnumerable<string> GetText(string filename)
		{
			return Regex
				.Split(File.ReadAllText(filename), @"[^\p{L}]*\p{Z}[^\p{L}]*")
				.Select(word => word.Trim(Environment.NewLine.ToCharArray())); ;
		}
	}
}