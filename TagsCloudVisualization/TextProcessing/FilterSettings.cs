using System;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization.TextProcessing
{
	public class FilterSettings : IFilterSettings
	{
		private int maxLength;
		private int minLength;

		public int MaxLength
		{
			get => maxLength;
			set
			{
				if (value < MinLength)
					throw new ArgumentException("Max length can't be less then min length");
				maxLength = value;
			}
		}

		public int MinLength
		{
			get => minLength;
			set
			{
				if (value > MaxLength)
					throw new ArgumentException("Min length can't be bigger then max length");
				minLength = value < 2 ? 2 : value;
			}
		}
	}
}