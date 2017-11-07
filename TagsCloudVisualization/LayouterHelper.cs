using System;
using System.Drawing;

namespace TagsCloudVisualization
{
	internal static class LayouterHelper
	{
		public static void AddRandomRectangles(this CircularCloudLayouter layouter, int numerOfRectangles, int minHeight,
			int maxHeight, int maxWidth)
		{
			if (numerOfRectangles <= 0)
				throw new ArgumentException("Number of rectangles should be positive number!");
			if (minHeight >= maxHeight)
				throw new ArgumentException("Minimal height should be less than maximum height!");
			if (maxWidth < maxHeight)
				throw new ArgumentException("Width should be bigger than height!");

			var rnd = new Random();
			for (var i = 0; i < numerOfRectangles; i++)
			{
				var height = rnd.Next(minHeight, maxHeight);
				var width = rnd.Next(height, maxWidth);
				layouter.PutNextRectangle(new Size(width, height));
			}
		}

		public static void DrawImage(CircularCloudLayouter layouter, Graphics g, int offsetX, int offsetY)
		{
			var randomGen = new Random();
			foreach (var rectangle in layouter.Rectangles)
			{
				var names = (KnownColor[]) Enum.GetValues(typeof(KnownColor));
				var randomColorName = names[randomGen.Next(names.Length)];
				var randomColor = Color.FromKnownColor(randomColorName);
				g.FillRectangle(new SolidBrush(randomColor),
					rectangle.X + offsetX, rectangle.Y + offsetY,
					rectangle.Width, rectangle.Height);
			}
			g.FillEllipse(new SolidBrush(Color.Red), layouter.Center.X + offsetX, layouter.Center.Y + offsetY, 10, 10);
		}
	}
}