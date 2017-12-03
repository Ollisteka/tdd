using System;
using System.Collections.Generic;
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
		public IEnumerable<string> GetText(string filename)
		{
			var doc = new Document();
			doc.LoadFromFile(filename);
			var sb = new StringBuilder();
			foreach (Section section in doc.Sections)
				foreach (Paragraph paragraph in section.Paragraphs)
					sb.AppendLine(paragraph.Text);
			return Regex
				.Split(sb.ToString(), @"[^\p{L}]*\p{Z}[^\p{L}]*")
				.Select(word => word.Trim(Environment.NewLine.ToCharArray()));
		}
	}
}