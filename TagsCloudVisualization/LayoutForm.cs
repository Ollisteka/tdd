using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace TagsCloudVisualization
{
	public partial class LayoutForm : Form
	{
		private readonly Bitmap bitmap;
		private Dictionary<string, Rectangle> wordsRectangles = new Dictionary<string, Rectangle>();
		private Dictionary<string, float> wordsFonts = new Dictionary<string, float>();
		private CircularCloudLayouter layouter = new CircularCloudLayouter(new Point(0,0));
		public LayoutForm()
		{
			InitializeComponent();
			Width = 1000;
			Height = 1000;
			var offsetX = Width / 2;
			var offsetY = Height / 2;

			layouter.AddRandomRectangles(40, 40, 70, 200);

			bitmap = new Bitmap(Width, Height);
			var g = Graphics.FromImage(bitmap);
			LayouterHelper.DrawImage(layouter, g, offsetX, offsetY);
		}

		public LayoutForm(Dictionary<string, int> words)
		{
			ResizeWords(words);
			Width = wordsRectangles.Values.Sum(rectangle => rectangle.Width) / 7;
			Height = wordsRectangles.Values.Sum(rectangle => rectangle.Height) / 2;
			bitmap = new Bitmap(Width, Height);
			var g = Graphics.FromImage(bitmap);
			
			var offsetX = Width / 2;
			var offsetY = Height / 2;

			DrawWords(g, offsetX, offsetY);

			var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			var path = Path.Combine(desktopPath, DateTime.Now.Ticks + ".bmp");
			bitmap.Save(path);
		}

		private void ResizeWords(Dictionary<string, int> words)
		{
			var minFontSize = 25;
			var maxFontSize = 55;
			var maxFrequency = words.Values.Max();
			var minFrequency = words.Values.Min();
			foreach (var word in words)
			{
				var weight = (Math.Log((float) word.Value / minFrequency)) / Math.Log((float) maxFrequency / minFrequency);
				var fontSize = minFontSize + (float) Math.Round((maxFontSize - minFontSize) * weight);
				//var fontSize = ((float)word.Value / maxFrequency) * (maxFontSize - minFontSize) + minFontSize;
				var font = new Font(FontFamily.GenericSansSerif, fontSize);
				var tagSize = TextRenderer.MeasureText(word.Key, font);
				wordsRectangles[word.Key] = layouter.PutNextRectangle(tagSize);
				wordsFonts[word.Key] = fontSize;
			}
		}

		public void DrawWords(Graphics g, int offsetX, int offsetY)
		{
			foreach (var word in wordsRectangles)
			{
				var shiftedRectangle = new Rectangle(word.Value.X + offsetX, word.Value.Y + offsetY, 
													word.Value.Width, word.Value.Height);
				g.DrawString(word.Key, new Font(FontFamily.GenericSansSerif, wordsFonts[word.Key]-3), 
							new SolidBrush(Color.Black), shiftedRectangle);
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			e.Graphics.DrawImage(bitmap, 0, 0,
				bitmap.Width,
				bitmap.Height);
		}
	}
}