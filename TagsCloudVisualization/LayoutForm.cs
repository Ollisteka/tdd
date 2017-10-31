using System;
using System.Drawing;
using System.Windows.Forms;

namespace TagsCloudVisualization
{
	public partial class LayoutForm : Form
	{
		public CircularCloudLayouter Layouter;

		public LayoutForm()
		{
			InitializeComponent();
			Width = 800;
			Height = 800;
			Layouter = new CircularCloudLayouter(new Point(300, 300));
			var rnd = new Random();
			for (var i = 0; i < 30; i++)
			{
				var height = rnd.Next(15, 60);
				var width = rnd.Next(height, 150);
				Layouter.PutNextRectangle(new Size(width, height));
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			var randomGen = new Random();
			foreach (var rectangle in Layouter.Rectangles)
			{
				var names = (KnownColor[]) Enum.GetValues(typeof(KnownColor));
				var randomColorName = names[randomGen.Next(names.Length)];
				var randomColor = Color.FromKnownColor(randomColorName);
				e.Graphics.FillRectangle(new SolidBrush(randomColor), rectangle);
			}
			e.Graphics.FillEllipse(new SolidBrush(Color.Red), Layouter.Center.X, Layouter.Center.Y, 10, 10);
		}
	}
}