using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization
{
	public partial class LayoutForm : Form
	{
		private readonly Bitmap bitmap;

		public LayoutForm(ICloudDrawer drawer)
		{
			Width = drawer.Width;
			Height = drawer.Height;
			bitmap = new Bitmap(Width, Height);
			using (var g = Graphics.FromImage(bitmap))
			{
				drawer.DrawWords(g);
				var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				var savePath = Path.Combine(desktopPath, DateTime.Now.Ticks + ".bmp");
//				if (savePath != null)
//					bitmap.Save(savePath);
			}}


		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			e.Graphics.DrawImage(bitmap, 0, 0,
				bitmap.Width,
				bitmap.Height);
		}
	}
}