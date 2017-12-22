using System.Drawing;
using System.Windows.Forms;

namespace TagsCloudVisualization
{
	public partial class LayoutForm : Form
	{
		public Bitmap Bitmap;

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			e.Graphics.DrawImage(Bitmap, 0, 0,
				Bitmap.Width,
				Bitmap.Height);
		}

		internal void ShowLayout(Bitmap bitmap)
		{
			Bitmap = bitmap;
			Width = bitmap.Width;
			Height = bitmap.Height;
			ShowDialog();
		}
	}
}