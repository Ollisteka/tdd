using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.Interfaces
{
	public interface ICloudDrawer
	{
		Bitmap DrawWords(Dictionary<string, int> words);
	}
}