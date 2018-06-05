using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CSharpFunctionalExtensions;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization
{
    internal class LayoutApp
    {
        private readonly IEnumerable<ITextFilter> filtrations;
        private readonly IFrequencyCounter frequencyCounter;
        private readonly ICloudDrawer layoutDrawer;
        private readonly LayoutForm layoutForm;
        private readonly IEnumerable<IFileReader> readers;
        private readonly ISettings settings;

        public LayoutApp(IEnumerable<ITextFilter> filtrations, IFrequencyCounter frequencyCounter,
            ICloudDrawer layoutDrawer, LayoutForm layoutForm, ISettings settings, IEnumerable<IFileReader> readers)
        {
            this.filtrations = filtrations;
            this.frequencyCounter = frequencyCounter;
            this.layoutDrawer = layoutDrawer;
            this.layoutForm = layoutForm;
            this.settings = settings;
            this.readers = readers;
        }

        public void Run(string inputFile, string outputFile, Color[] tagColors=null, int top = 100, int minLegth = 3,
            int maxLength = 100,
            int minFont = 25, int maxFont = 55)
        {
            InitSettings(maxLength, minLegth, minFont, maxFont, tagColors);

            var textResult = Result.Fail<IEnumerable<string>>("There is no reader registered in DI container");
            foreach (var reader in readers)
            {
                textResult = reader.TryGetText(inputFile);
                if (textResult.IsSuccess)
                    break;
            }

            if (textResult.IsFailure)
                ExitWithError(textResult.Error);
            var text = filtrations.Aggregate(textResult.Value, (current, filtration) => filtration.Filter(current));
            var statistics = frequencyCounter.MakeFrequencyStatistics(text, top);
            if (statistics?.Count == 0)
                ExitWithError("No words was found to match your requirements:\n\n" + settings);
            var bitmap = layoutDrawer.DrawWords(statistics);
            if (outputFile != null)
                bitmap?.Save(outputFile);
            else layoutForm.ShowLayout(bitmap);
        }

        private void InitSettings(int maxLength, int minLegth, int minFont, int maxFont, Color[] tagColors)
        {
            try
            {
                settings.CenterPoint = new Point(0, 0);
                settings.MaxWordLength = maxLength;
                settings.MinWordLength = minLegth;
                settings.MinWordFont = minFont;
                settings.MaxWordFont = maxFont;
                if (tagColors!= null)
                    settings.TagColors = tagColors;
            }
            catch (Exception e)
            {
                ExitWithError("Input argumenrs are inconsistent. " + e.Message);
            }
        }

        public static void ExitWithError(string errorMessage)
        {
            Console.WriteLine(errorMessage);
            Console.WriteLine("\nPress ESC to exit");
            while (Console.ReadKey(true).Key != ConsoleKey.Escape)
            {
            }

            ;
            Environment.Exit(1);
        }
    }
}