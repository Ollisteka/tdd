using System.Drawing;
using System.Windows.Forms;

namespace TagsCloudVisualization
{
	public partial class LayoutForm : Form
	{
		private readonly Bitmap bitmap;

		public LayoutForm()
		{
			InitializeComponent();
			Width = 800;
			Height = 800;
			var offsetX = Width / 2;
			var offsetY = Height / 2;
			var layouter = new CircularCloudLayouter(new Point(0, 0));

			layouter.AddRandomRectangles(40, 40, 70, 200);

			bitmap = new Bitmap(Width, Height);
			var g = Graphics.FromImage(bitmap);
			LayouterHelper.DrawImage(layouter, g, offsetX, offsetY);
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