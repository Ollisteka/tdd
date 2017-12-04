using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Spire.Doc;
using Spire.Doc.Documents;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization.TextProcessing
{
	internal class DocReader : IFileReader
	{
		private readonly IReadOnlyCollection<string> supportedExtensions = new List<string> {".doc", ".docx"};

		public bool TryGetText(string filename, out IEnumerable<string> text)
		{
			if (!supportedExtensions.Contains(Path.GetExtension(filename)))
			{
				text = null;
				return false;
			}
			var doc = new Document();
			doc.LoadFromFile(filename);
			var sb = new StringBuilder();
			foreach (Section section in doc.Sections)
			foreach (Paragraph paragraph in section.Paragraphs)
				sb.AppendLine(paragraph.Text);
			text = Regex
				.Split(sb.ToString(), @"[^\p{L}]*\p{Z}[^\p{L}]*")
				.Select(word => word.Trim(Environment.NewLine.ToCharArray()));
			return true;
		}
	}
}