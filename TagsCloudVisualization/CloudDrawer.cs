using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization
{
	public class CloudDrawer : ICloudDrawer
	{
		private readonly CircularCloudLayouter layouter = new CircularCloudLayouter(new Point(0, 0));
		private readonly Dictionary<string, float> wordsFonts = new Dictionary<string, float>();

		private readonly Dictionary<string, Rectangle> wordsRectangles = new Dictionary<string, Rectangle>();

		public CloudDrawer(ITextHandler textHandler)
		{
			ResizeWords(textHandler.MakeFrequencyStatistics());
		}
		public int Width => wordsRectangles.Values.Sum(rectangle => rectangle.Width) / 7;
		public int Height => wordsRectangles.Values.Sum(rectangle => rectangle.Height) / 2;

		private int OffsetX => Width / 2;
		private int OffsetY => Height / 2;

		public void DrawWords(Graphics g)
		{
			g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.CompositingQuality = CompositingQuality.HighQuality;
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			foreach (var word in wordsRectangles)
			{
				var shiftedRectangle = new Rectangle(word.Value.X + OffsetX, word.Value.Y + OffsetY,
					word.Value.Width, word.Value.Height);
				g.DrawString(word.Key, new Font(FontFamily.GenericSansSerif, wordsFonts[word.Key] - 3),
					new SolidBrush(Color.Black), shiftedRectangle);
			}
		}

		public void ResizeWords(Dictionary<string, int> words)
		{
			var minFontSize = 15;
			var maxFontSize = 35;
			var maxFrequency = words.Values.Max();
			var minFrequency = words.Values.Min();
			foreach (var word in words)
			{
				var weight = Math.Log((float) word.Value / minFrequency) / Math.Log((float) maxFrequency / minFrequency);
				var fontSize = minFontSize + (float) Math.Round((maxFontSize - minFontSize) * weight);
				//var fontSize = ((float)word.Value / maxFrequency) * (maxFontSize - minFontSize) + minFontSize;
				var font = new Font(FontFamily.GenericSansSerif, fontSize);
				var tagSize = TextRenderer.MeasureText(word.Key, font);
				wordsRectangles[word.Key] = layouter.PutNextRectangle(tagSize);
				wordsFonts[word.Key] = fontSize;
			}
		}
	}
}