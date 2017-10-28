using System;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization
{
	[TestFixture]
	public class CircularCloudLayouter_Should
	{
		public CircularCloudLayouter Layouter;

		[SetUp]
		public void SetUp()
		{
			Layouter = new CircularCloudLayouter(new Point(0, 0));
		}

		[Test]
		public void ShouldHaveCenter_AtPointZero_WhenConstructed()
		{
			Layouter.Center.Should().Be(new Point(0, 0));
		}

		[Test]
		public void ShouldHaveZeroTags_WhenConstructed()
		{
			Layouter.Rectangles.Count.Should().Be(0);
		}

		[Test]
		public void ShouldHaveOneTag_AfterPutNext()
		{
			var size = new Size(20, 10);
			var rectangle = Layouter.PutNextRectangle(size);
			Layouter.Rectangles.Count.Should().Be(1);
			rectangle.Location.Should().Be(new Point(0, 0));
			rectangle.Size.Should().Be(size);
		}

		[Test]
		public void AddTwoRectangles()
		{
			var sizeOne = new Size(4, 2);
			var sizeTwo = new Size(5, 1);
			var rectangleOne = Layouter.PutNextRectangle(sizeOne);
			var rectangleTwo = Layouter.PutNextRectangle(sizeTwo);
			Layouter.Rectangles.Count.Should().Be(2);
			rectangleOne.IntersectsWith(rectangleTwo).Should().BeFalse();
		}

		[Test, Timeout(1000)]
		public void AddALotOfRectangles()
		{
			for (var i = 1; i < 1000; i++)
				Layouter.PutNextRectangle(new Size(i + 1, i));
			
		}
		[Test, Timeout(1000)]
		public void AddALotOfRandomRectangles()
		{
			var rnd = new Random();
			for (int i = 1; i < 1000; i++)
			{
				var number = rnd.Next(1, 30);
				Layouter.PutNextRectangle(new Size(number + 1, number));
			}
		}
		[TestCase(5, 2, 10, 1, 4, 2, ExpectedResult = 3)]
		[TestCase(5, 2, 6, 3, 10, 1, 4, 2, ExpectedResult = 4)]
		[TestCase(5, 2, 6, 3, 10, 1, 4, 2, 4, 1, 7, 4, 6, 1, 9, 5, 13, 10, ExpectedResult = 9)]
		public int AddManyRectangles(params int[] dimensions)
		{
			for (int i = 0; i < dimensions.Length; i += 2)
				Layouter.PutNextRectangle(new Size(dimensions[i], dimensions[i + 1]));
			return Layouter.Rectangles.Count;

		}
		[TestCase(1, -2, TestName = "NegativeWidth")]
		[TestCase(-2, 1, TestName = "NegativeHeight")]
		[TestCase(0, 2, TestName = "ZeroWidth")]
		[TestCase(2, 0, TestName = "ZeroHeight")]
		[TestCase(2, 5, TestName = "HeightIsBiggerThanScale")]
		public void Throw(int width, int height)
		{
			Assert.Throws<ArgumentException>(() => Layouter.PutNextRectangle(new Size(width, height)));
		}


	}
}