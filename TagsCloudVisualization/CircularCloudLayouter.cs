using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
	public class CircularCloudLayouter
	{
		public CircularCloudLayouter(Point center)
		{
			Center = center;
			Rectangles = new List<Rectangle>();
		}

		private int radius;
		public Point Center { get; }
		public List<Rectangle> Rectangles { get; }

		public Rectangle PutNextRectangle(Size rectangleSize)
		{
			if (rectangleSize.Height > rectangleSize.Width)
				throw new ArgumentException("Height should be less or equal to width");
			if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
				throw new ArgumentException("Height and width should be non negative!");

			var recToAdd = PlaceRectangle(new Rectangle(Center, rectangleSize));
			if (Rectangles.Count == 0)
				recToAdd = ShiftFirstRectangle(recToAdd);
			Rectangles.Add(recToAdd);
			return recToAdd;
		}

		private Rectangle ShiftFirstRectangle(Rectangle rectangle)
		{
			var height = rectangle.Height;
			var width = rectangle.Width;
			var rightCenter = new Point(Center.X - width / 2, Center.Y - height / 2);
			return new Rectangle(rightCenter, rectangle.Size);
		}

		private Rectangle PlaceRectangle(Rectangle recToAdd)
		{
			radius = 0;
			while (true)
			{
				for (var degree = 0; degree < 360; degree += 3)
				{
					var rad = (degree / Math.PI) * 180.0;
					var shiftX = (int)(recToAdd.Location.X + radius * Math.Cos(rad));
					var shiftY = (int)(recToAdd.Location.Y + radius * Math.Sin(rad));

					var possibleRectangle = new Rectangle(
						new Point(shiftX - recToAdd.Size.Width / 2, shiftY - recToAdd.Size.Height / 2 ), 
						recToAdd.Size);

					if (Rectangles.Count(rectangle => rectangle.IntersectsWith(possibleRectangle)) == 0)
						 return possibleRectangle;

				}
				radius += 1;
			}
		}
	}
}