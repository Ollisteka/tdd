using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
	[TestFixture]
	public class CircularCloudLayouter_Should
	{
		[SetUp]
		public void SetUp()
		{
			Layouter = new CircularCloudLayouter(new Point(0, 0));
		}

		public CircularCloudLayouter Layouter;

		[TestCase(5, 2, 10, 1, 4, 2, ExpectedResult = 3)]
		[TestCase(5, 2, 6, 3, 10, 1, 4, 2, ExpectedResult = 4)]
		[TestCase(5, 2, 6, 3, 10, 1, 4, 2, 4, 1, 7, 4, 6, 1, 9, 5, 13, 10, ExpectedResult = 9)]
		public int AddManyRectangles(params int[] dimensions)
		{
			for (var i = 0; i < dimensions.Length; i += 2)
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

		[Test]
		public void AllRectangles_FitBigCircle()
		{
			var rnd = new Random();
			for (int i = 0; i < 10; i++)
			{
				var height = rnd.Next(15, 60);
				var width = rnd.Next(height, 150);
				Layouter.PutNextRectangle(new Size(width, height));
			}

			var totalWidth = Layouter.Rectangles.Sum(rectangle => rectangle.Width);
			var totalHeight = Layouter.Rectangles.Sum(rectangle => rectangle.Height);
			var excpectedRadius = Math.Sqrt((totalHeight * totalWidth) / Math.PI);

			var actualMaxRadius = Layouter.Rectangles.OrderByDescending(DistanceToCenter)
				.Select(DistanceToCenter).First();

			actualMaxRadius.Should().BeLessThan(excpectedRadius);

		}

		private double DistanceToCenter(Rectangle rectangle)
		{
			return Math.Sqrt((rectangle.X - Layouter.Center.X) ^ 2 + (rectangle.Y - Layouter.Center.Y) ^ 2);
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

		[Test]
		public void ShouldHaveCenter_AtPointZero_WhenConstructed()
		{
			Layouter.Center.Should().Be(new Point(0, 0));
		}

		[Test]
		public void ShouldHaveOneRectangle_AfterPutNext()
		{
			var size = new Size(20, 10);
			Layouter.PutNextRectangle(size);
			Layouter.Rectangles.Count.Should().Be(1);
		}

		[Test]
		public void FirstRectangleLocation_ShouldBeShifted()
		{
			var size = new Size(20, 10);
			var rectangle = Layouter.PutNextRectangle(size);
			rectangle.Location.Should().Be(new Point(-10, -5));
			rectangle.Size.Should().Be(size);
		}

		[Test]
		public void ShouldHaveEmtyRectanglesList_WhenConstructed()
		{
			Layouter.Rectangles.Count.Should().Be(0);
		}
	}
}