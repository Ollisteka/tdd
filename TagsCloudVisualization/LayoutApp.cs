﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization
{
	internal class LayoutApp
	{
		private readonly IEnumerable<ITextFiltration> filtrations;
		private readonly IFrequencyCounter frequencyCounter;
		private readonly ICloudDrawer layoutDrawer;
		private readonly LayoutForm layoutForm;
		private readonly IFileReader reader;
		private readonly ISettings settings;

		public LayoutApp(IEnumerable<ITextFiltration> filtrations, IFrequencyCounter frequencyCounter,
			ICloudDrawer layoutDrawer, LayoutForm layoutForm, ISettings settings, IFileReader reader)
		{
			this.filtrations = filtrations;
			this.frequencyCounter = frequencyCounter;
			this.layoutDrawer = layoutDrawer;
			this.layoutForm = layoutForm;
			this.settings = settings;
			this.reader = reader;
		}

		public void Run(string inputFile, string outputFile, int top=100, int minLegth=3, int maxLength=100, int minFont=25, int maxFont=55)
		{
			settings.CenterPoint = new Point(0, 0);
			settings.MaxWordLength = maxLength;
			settings.MinWordLength = minLegth;
			settings.MinWordFont = minFont;
			settings.MaxWordFont = maxFont;

			var text = reader.GetText(inputFile);
			text = filtrations.Aggregate(text, (current, filtration) => filtration.Filter(current));
			var statistics = frequencyCounter.MakeFrequencyStatistics(text, top);
			var bitmap = layoutDrawer.DrawWords(statistics);
			if (outputFile != null)
				bitmap?.Save(outputFile);
			else layoutForm.ShowLayout(bitmap);
		}
	}
}