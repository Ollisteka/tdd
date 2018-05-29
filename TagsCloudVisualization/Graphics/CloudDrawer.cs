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
        private static Random rnd = new Random();
		private const int AdditionalHeight = 55;
		private const int AdditionalWidth = 55;
		private readonly ICloudLayouter layouter;
		private readonly ISettings settings;
		private readonly Dictionary<string, float> wordsFonts = new Dictionary<string, float>();

		private readonly Dictionary<string, Rectangle> wordsRectangles = new Dictionary<string, Rectangle>();

		public CloudDrawer(ICloudLayouter layouter, ISettings settings)
		{
			this.layouter = layouter;
			this.settings = settings;
		}

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
                    var index = rnd.Next(settings.TagColors.Length);
                    g.DrawString(word.Key, new Font(FontFamily.GenericSansSerif, wordsFonts[word.Key]),
						new SolidBrush(settings.TagColors[index]), shiftedRectangle);
				}
			}
			return result;
		}

		private void ResizeWords(Dictionary<string, int> words)
		{
			var minFontSize = settings.MinWordFont;
			var maxFontSize = settings.MaxWordFont;
			var maxFrequency = words.Values.Max();
			var minFrequency = words.Values.Min();
			foreach (var word in words)
			{
				var weight = Math.Log((float) word.Value / minFrequency) / Math.Log((float) maxFrequency / minFrequency);
				if (double.IsNaN(weight))
					weight = 2;
				var fontSize = minFontSize + (float) Math.Round((maxFontSize - minFontSize) * weight);

				var font = new Font(FontFamily.GenericSansSerif, fontSize);
				var tagSize = TextRenderer.MeasureText(word.Key, font);
				wordsRectangles[word.Key] = layouter.PutNextRectangle(tagSize);
				wordsFonts[word.Key] = fontSize;
			}
		}
	}
}