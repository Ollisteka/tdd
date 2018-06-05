using System.Drawing;
using TagsCloudVisualization.Helpers;

namespace TagsCloudVisualization.Interfaces
{
    public interface ISettings
    {
        int MaxWordLength { get; set; }

        int MinWordLength { get; set; }

        int MinWordFont { get; set; }

        int MaxWordFont { get; set; }
        Language Language { get; set; }
        Point CenterPoint { get; set; }
        Color[] TagColors { get; set; }
    }
}