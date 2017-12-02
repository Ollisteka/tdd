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
		private const int AdditionalHeight = 25;
		private const int AdditionalWidth = 25;
		private readonly ICloudLayouter layouter = new CircularCloudLayouter(new Point(0, 0));
		private readonly Dictionary<string, float> wordsFonts = new Dictionary<string, float>();

		private readonly Dictionary<string, Rectangle> wordsRectangles = new Dictionary<string, Rectangle>();

		private int OffsetX => Width / 2;
		private int OffsetY => Height / 2;

		public int Width => wordsRectangles.Values.Max(rectangle => rectangle.Right) -
							wordsRectangles.Values.Min(rectangle => rectangle.Left) + AdditionalWidth;

		public int Height => wordsRectangles.Values.Max(rectangle => rectangle.Bottom) -
							wordsRectangles.Values.Min(rectangle => rectangle.Top) + AdditionalHeight;

		public Bitmap DrawWords(Dictionary<string, int> wordsFrequency)
		{
			ResizeWords(wordsFrequency);
			var result = new Bitmap(Width, Height);
			using (var g = Graphics.FromImage(result))
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
			return result;
		}

		private void ResizeWords(Dictionary<string, int> words)
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