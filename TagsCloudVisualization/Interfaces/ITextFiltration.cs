using System;
using System.Collections.Generic;

namespace TagsCloudVisualization.Interfaces
{
	public interface ITextFiltration
	{
		IEnumerable<string> Filter(IEnumerable<string> content);
	}
}