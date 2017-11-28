using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.Interfaces
{
	public interface ICloudLayouter
	{
		Point Center { get; }
		IReadOnlyCollection<Rectangle> Rectangles { get; }
		Rectangle PutNextRectangle(Size rectangleSize);
	}
}