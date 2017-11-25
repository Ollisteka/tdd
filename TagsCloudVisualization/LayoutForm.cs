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

		public LayoutForm(IEnumerable<string> words)
		{
			Width = 800;
			Height = 800;
			bitmap = new Bitmap(Width, Height);
			var g = Graphics.FromImage(bitmap);
			ResizeWords(words, g);
			//Width = wordsRectangles.Values.Sum(rectangle => rectangle.Width) + 200;
			//Height = wordsRectangles.Values.Sum(rectangle => rectangle.Height) + 200;
			var offsetX = Width / 2;
			var offsetY = Height / 2;
			
			//LayouterHelper.DrawImage(layouter, g, offsetX, offsetY);
			LayouterHelper.DrawWords(wordsRectangles, g, offsetX, offsetY);

			var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			var path = Path.Combine(desktopPath, DateTime.Now.Ticks + ".bmp");
			//bitmap.Save(path);
		}

		private void ResizeWords(IEnumerable<string> words, Graphics g)
		{
			float fontSize = 30;
			foreach (var word in words)
			{
				var stringFont = new Font(FontFamily.GenericSansSerif, fontSize);
				var stringSize = g.MeasureString(word, stringFont);

				layouter.PutNextRectangle(stringSize.ToSize());
				wordsRectangles[word] = layouter.Rectangles.Last();
				fontSize -= 0.2f;
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