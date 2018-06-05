using System;
using System.Collections.Generic;
using System.IO;
using SharpNL.POSTag;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization.TextProcessing
{
    public class PartOfSpeechFilter : ITextFilter
    {
        private readonly HashSet<char> whiteList = new HashSet<char> {'N', 'J', 'V'};

        public IEnumerable<string> Filter(IEnumerable<string> content)
        {
            var solutiondir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            var binFile = Path.Combine(solutiondir, "dictionaries", "en-pos-maxent.bin");
            POSModel posModel = null;
            try
            {
                using (var modelFile = new FileStream(binFile, FileMode.Open))
                {
                    posModel = new POSModel(modelFile);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong with the POSFilter:");
                LayoutApp.ExitWithError(e.Message);
            }

            var posTagger = new POSTaggerME(posModel);
            foreach (var word in content)
            {
                var tags = posTagger.Tag(new[] {word});
                if (whiteList.Contains(tags[0][0]))
                    yield return word;
            }
        }
    }
}