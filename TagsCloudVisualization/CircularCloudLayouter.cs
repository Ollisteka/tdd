using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
	public class CircularCloudLayouter
	{
		private double radius;
		private readonly Random rnd = new Random();

		public CircularCloudLayouter(Point center)
		{
			Center = center;
			Rectangles = new List<Rectangle>();
		}

		public Point Center { get; }
		public List<Rectangle> Rectangles { get; }

		public Rectangle PutNextRectangle(Size rectangleSize)
		{
			if (rectangleSize.Height > rectangleSize.Width)
				throw new ArgumentException("Height should be less or equal to width");
			if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
				throw new ArgumentException("Height and width should be non negative!");

			var recToAdd = ShiftReactangle(new Rectangle(Center, rectangleSize));
			Rectangles.Add(recToAdd);
			return recToAdd;
		}

		private Rectangle ShiftReactangle(Rectangle recToAdd)
		{
			var center = recToAdd.Location;
			radius = Rectangles.Count == 0 ? 0 : 0.5 * Math.Min(recToAdd.Width, recToAdd.Height);

			while (true)
			{
				var prevX = -1;
				var prevY = -1;
				for (var degree = 0; degree < 360; degree += 5)
				{
					var rad = ((double)degree / Math.PI) * 180.0;
					var shiftX = (int)(center.X + radius * Math.Cos(rad));
					var shiftY = (int)(center.Y + radius * Math.Sin(rad));
					if (prevX == shiftX && prevY == shiftY)
						continue;
					prevX = shiftX;
					prevY = shiftY;

					var possibleRectangle = new Rectangle(new Point(center.X + shiftX, center.Y + shiftY),
						recToAdd.Size);

					int prev;
					for (prev = 0; prev < Rectangles.Count; prev++)
						if (possibleRectangle.IntersectsWith(Rectangles[prev]))
							break;

					if (prev == Rectangles.Count)
						return possibleRectangle;
					
				}
				radius += 5;
			}
		}
	}
}