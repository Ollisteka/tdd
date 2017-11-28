using System.Drawing;
using System.Windows.Forms;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization
{
	public partial class LayoutForm : Form
	{
		public readonly Bitmap Bitmap;

		public LayoutForm(ICloudDrawer drawer)
		{
			Width = drawer.Width;
			Height = drawer.Height;
			Bitmap = new Bitmap(Width, Height);
			using (var g = Graphics.FromImage(Bitmap))
			{
				drawer.DrawWords(g);
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			e.Graphics.DrawImage(Bitmap, 0, 0,
				Bitmap.Width,
				Bitmap.Height);
		}
	}
}