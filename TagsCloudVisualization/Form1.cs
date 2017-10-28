using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TagsCloudVisualization
{
	public partial class Form1 : Form
	{
		public CircularCloudLayouter Layouter;
		public Form1()
		{
			InitializeComponent();
			Width = 800;
			Height = 800;
			Layouter = new CircularCloudLayouter(new Point(100, 100));
			var rnd = new Random();
			for (int i = 0; i < 30; i++)
			{
				var height = rnd.Next(15, 60);
				var width = rnd.Next(height, 150);
				Layouter.PutNextRectangle(new Size(width, height));
			}

		}
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Random randomGen = new Random();
			for (var i = 0; i < Layouter.Rectangles.Count; i++)
			{
				KnownColor[] names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
				KnownColor randomColorName = names[randomGen.Next(names.Length)];
				Color randomColor = Color.FromKnownColor(randomColorName);

				if (i == 0)
					e.Graphics.FillRectangle(new SolidBrush(Color.Black), Layouter.Rectangles[i]);
				else
					e.Graphics.FillRectangle(new SolidBrush(randomColor), Layouter.Rectangles[i]);
			}

		}
	}
}
