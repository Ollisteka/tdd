using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization
{
	internal class LayoutApp
	{
		private readonly IEnumerable<ITextFiltration> filtrations;
		private readonly IFrequencyCounter frequencyCounter;
		private readonly ICloudDrawer layoutDrawer;
		private readonly LayoutForm layoutForm;
		private readonly ISettings settings;

		public LayoutApp(IEnumerable<ITextFiltration> filtrations, IFrequencyCounter frequencyCounter,
			ICloudDrawer layoutDrawer, LayoutForm layoutForm, ISettings settings)
		{
			this.filtrations = filtrations;
			this.frequencyCounter = frequencyCounter;
			this.layoutDrawer = layoutDrawer;
			this.layoutForm = layoutForm;
			this.settings = settings;
		}

		public void Run(string inputFile, string outputFile, int top, int minLegth, int maxLength)
		{
			settings.CenterPoint = new Point(0, 0);
			settings.MaxLength = maxLength;
			settings.MinLength = minLegth;

			var text = Regex.Split(File.ReadAllText(inputFile), @"[^\p{L}]*\p{Z}[^\p{L}]*").AsEnumerable();
			text = filtrations.Aggregate(text, (current, filtration) => filtration.Filter(current));
			var statistics = frequencyCounter.MakeFrequencyStatistics(text, top);
			var bitmap = layoutDrawer.DrawWords(statistics);
			if (outputFile != null)
				bitmap.Save(outputFile);
			else layoutForm.Show(bitmap);
		}
	}
}