using System;
using System.Drawing;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization
{
	public class Settings : ISettings
	{
		private int maxWordFont = 100;
		private int maxWordLength = 100;
		private int minWordFont = 3;
		private int minWordLength = 3;


		public int MaxWordLength
		{
			get => maxWordLength;
			set
			{
				if (value < MinWordLength)
					throw new ArgumentException("Max length can't be less then min length");
				maxWordLength = value;
			}
		}

		public int MinWordLength
		{
			get => minWordLength;
			set
			{
				if (value > MaxWordLength)
					throw new ArgumentException("Min length can't be bigger then max length");
				minWordLength = value < 2 ? 2 : value;
			}
		}

		public int MinWordFont
		{
			get => minWordFont;
			set
			{
				if (value > MaxWordFont)
					throw new ArgumentException("Min font size can't be bigger then max size");
				minWordFont = value < 1 ? 1 : value;
			}
		}

		public int MaxWordFont
		{
			get => maxWordFont;
			set
			{
				if (value < MinWordFont)
					throw new ArgumentException("Max font size can't be less then min size");
				maxWordFont = value;
			}
		}

		public Point CenterPoint { get; set; }
	}
}