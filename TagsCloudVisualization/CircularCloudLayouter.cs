using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization
{
	public class CircularCloudLayouter : ICloudLayouter
	{
		private const int ShiftLength = 40;

		private readonly List<Rectangle> rectangles;

		public CircularCloudLayouter(Point center)
		{
			Center = center;
			rectangles = new List<Rectangle>();
		}

		public Point Center { get; }

		public IReadOnlyCollection<Rectangle> Rectangles => rectangles;

		public Rectangle PutNextRectangle(Size rectangleSize)
		{
			if (rectangleSize.Height > rectangleSize.Width)
				throw new ArgumentException("Height should be less or equal to width");
			if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
				throw new ArgumentException("Height and width should be non negative!");

			var recToAdd = PlaceRectangle(new Rectangle(Center, rectangleSize));
			recToAdd = rectangles.Count == 0
				? CentralizeFirstRectangle(recToAdd)
				: TryToShiftRectangle(recToAdd);
			rectangles.Add(recToAdd);
			return recToAdd;
		}

		private Rectangle TryToShiftRectangle(Rectangle rectangle)
		{
			var centerX = Center.X;
			var centerY = Center.Y;

			for (var dx = 0; dx < ShiftLength; dx++)
			for (var dy = 0; dy < ShiftLength; dy++)
			{
				var x = rectangle.X;
				var y = rectangle.Y;

				x += dx * centerX.CompareTo(x);
				y += dy * centerY.CompareTo(y);

				var possibleRectangle = new Rectangle(
					new Point(x, y),
					rectangle.Size);

				if (rectangles.Count(rec => rec.IntersectsWith(possibleRectangle)) == 0)
					rectangle = possibleRectangle;
			}
			return rectangle;
		}

		private Rectangle CentralizeFirstRectangle(Rectangle rectangle)
		{
			var height = rectangle.Height;
			var width = rectangle.Width;
			var rightCenter = new Point(Center.X - width / 2, Center.Y - height / 2);
			return new Rectangle(rightCenter, rectangle.Size);
		}

		private Rectangle PlaceRectangle(Rectangle recToAdd)
		{
			for (var radius = 0;; radius += 1)
			for (var degree = 0; degree < 360; degree += 3)
			{
				var rad = degree / Math.PI * 180.0;
				var shiftX = (int) (recToAdd.Location.X + radius * Math.Cos(rad));
				var shiftY = (int) (recToAdd.Location.Y + radius * Math.Sin(rad));

				var possibleRectangle = new Rectangle(
					new Point(shiftX - recToAdd.Size.Width / 2, shiftY - recToAdd.Size.Height / 2),
					recToAdd.Size);

				if (rectangles.Count(rectangle => rectangle.IntersectsWith(possibleRectangle)) == 0)
					return possibleRectangle;
			}
		}
	}
}