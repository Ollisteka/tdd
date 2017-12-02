using System.Drawing;

namespace TagsCloudVisualization.Interfaces
{
	public interface ISettings
	{
		int MaxLength { get; set; }

		int MinLength { get; set; }

		Point CenterPoint { get; set; }
	}
}