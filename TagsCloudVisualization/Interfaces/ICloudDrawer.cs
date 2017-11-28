using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.Interfaces
{
	public interface ICloudDrawer
	{
		int Width { get; }
		int Height { get; }
		void ResizeWords(Dictionary<string, int> words);
		void DrawWords(Graphics g);
	}
}