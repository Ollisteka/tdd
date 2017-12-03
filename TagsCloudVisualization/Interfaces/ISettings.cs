using System.Drawing;

namespace TagsCloudVisualization.Interfaces
{
	public interface ISettings
	{
		int MaxWordLength { get; set; }

		int MinWordLength { get; set; }

		int MinWordFont { get; set; }

		int MaxWordFont { get; set; }

		Point CenterPoint { get; set; }
	}
}