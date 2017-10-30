using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TagsCloudVisualization
{
	class CentalizedRectangle
	{
		private Rectangle rectangle;

		private Point location;
		public Point Location
		{
			get => location;
			private set
			{
				Center = value;
				var halfWidth = Size.Width / 2;
				var halfHeight = Size.Height / 2;
				location = new Point(Center.X - halfWidth, Center.Y - halfHeight);
			}
		}
		public Point Center { get; private set; }
		public Size Size { get; private set; }

		public CentalizedRectangle(Point center, Size size)
		{
			Size = size;
			Location = center;
			rectangle = new Rectangle(Location, size);
		}

		public bool IntersectsWith(Rectangle rec)
		{
			return rectangle.IntersectsWith(rec);
		}
	}
}
