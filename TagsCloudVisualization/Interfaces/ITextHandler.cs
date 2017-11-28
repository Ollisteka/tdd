using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization.Interfaces
{
	public interface ITextHandler
	{
		Dictionary<string, int> MakeFrequencyStatistics();
	}
}
