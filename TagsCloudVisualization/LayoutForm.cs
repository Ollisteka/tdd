using System;
using System.Drawing;
using System.Windows.Forms;

namespace TagsCloudVisualization
{
	public partial class LayoutForm : Form
	{
		private readonly CircularCloudLayouter layouter;
		private readonly Bitmap bitmap;
		private int offsetX, offsetY;

		public LayoutForm()
		{
			InitializeComponent();
			Width = 800;
			Height = 800;
			offsetX = Width / 2;
			offsetY = Height / 2;
			layouter = new CircularCloudLayouter(new Point(0, 0));

			var rnd = new Random();
			for (var i = 0; i < 30; i++)
			{
				var height = rnd.Next(15, 60);
				var width = rnd.Next(height, 150);
				layouter.PutNextRectangle(new Size(width, height));
			}

			bitmap = new Bitmap(Width, Height);
			var g = Graphics.FromImage(bitmap);
		    DrawImage(g);
		}

		public void DrawImage(Graphics g)
		{
			var randomGen = new Random();
			foreach (var rectangle in layouter.Rectangles)
			{
				var names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
				var randomColorName = names[randomGen.Next(names.Length)];
				var randomColor = Color.FromKnownColor(randomColorName);
				g.FillRectangle(new SolidBrush(randomColor),
					rectangle.X + offsetX, rectangle.Y + offsetY,
					rectangle.Width, rectangle.Height);
			}
			g.FillEllipse(new SolidBrush(Color.Red), 
				layouter.Center.X + offsetX, 
				layouter.Center.Y + offsetY, 10, 10);
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