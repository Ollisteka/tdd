using System;
using System.Drawing;
using System.Reflection;
using System.Text;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization
{
    public class Settings : ISettings
    {
        private int maxWordFont = 100;
        private int maxWordLength = 100;
        private int minWordFont = 3;
        private int minWordLength = 3;

        private PropertyInfo[] propertyInfo;
        public Color[] TagColors { get; set; } = { Color.Blue, Color.Black, Color.Red};


        public int MaxWordLength
        {
            get => maxWordLength;
            set
            {
                if (value < MinWordLength)
                    throw new ArgumentException(
                        $"Max length ({value}) can't be less then min length ({minWordLength})");
                maxWordLength = value;
            }
        }

        public int MinWordLength
        {
            get => minWordLength;
            set
            {
                if (value > MaxWordLength)
                    throw new ArgumentException(
                        $"Min length ({value}) can't be bigger then max length ({maxWordLength})");
                minWordLength = value < 2 ? 2 : value;
            }
        }

        public int MinWordFont
        {
            get => minWordFont;
            set
            {
                if (value > MaxWordFont)
                    throw new ArgumentException(
                        $"Min font size ({value}) can't be bigger then max size ({maxWordFont})");
                minWordFont = value < 1 ? 1 : value;
            }
        }

        public int MaxWordFont
        {
            get => maxWordFont;
            set
            {
                if (value < MinWordFont)
                    throw new ArgumentException($"Max font size ({value}) can't be less then min size ({minWordFont})");
                maxWordFont = value;
            }
        }

        public Point CenterPoint { get; set; }

        public override string ToString()
        {
            if (propertyInfo == null)
                propertyInfo = GetType().GetProperties();

            var sb = new StringBuilder();

            foreach (var info in propertyInfo)
            {
                var value = info.GetValue(this, null) ?? "(null)";
                sb.AppendLine($"{info.Name}: {value}");
            }

            return sb.ToString();
        }
    }
}