	using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
	using CSharpFunctionalExtensions;
	using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization.TextProcessing
{
	internal class TxtReader : IFileReader
	{
		private readonly IReadOnlyCollection<string> supportedExtensions = new List<string> { ".txt" };
		public Result<IEnumerable<string>> TryGetText(string filename)
		{
			if (!supportedExtensions.Contains(Path.GetExtension(filename)))
				return Result.Fail<IEnumerable<string>>(
					$"The extension {Path.GetExtension(filename)} is not supported by this reader");

			return Result.Ok(Regex
				.Split(File.ReadAllText(filename), @"[^\p{L}]*\p{Z}[^\p{L}]*")
				.Select(word => word.Trim(Environment.NewLine.ToCharArray())));
		}
	}
}