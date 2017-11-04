using System;
using System.Drawing;
using System.IO;
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

		[TearDown]
		public void TearDown()
		{
			if (TestContext.CurrentContext.Result.FailCount == 0) return;

			var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			var path = Path.Combine(desktopPath, currentTestName + ".bmp");

			var actualMaxRadius = 1.0;

			if (Layouter.Rectangles.Count != 0)
				actualMaxRadius = Layouter.Rectangles
											.Select(DistanceToCenter)
											.Max();

			var bitmap = new Bitmap((int) actualMaxRadius * 2, (int) actualMaxRadius * 2);

			using (var g = Graphics.FromImage(bitmap))
				DrawImage(g, (int) actualMaxRadius, (int) actualMaxRadius);

			bitmap.Save(path);
			Console.WriteLine($@"Tag cloud visualization saved to file {path}");
		}

		private string currentTestName => TestContext.CurrentContext.Test.Name;

		public CircularCloudLayouter Layouter;

		public void DrawImage(Graphics g, int offsetX, int offsetY)
		{
			var randomGen = new Random();
			foreach (var rectangle in Layouter.Rectangles)
			{
				var names = (KnownColor[]) Enum.GetValues(typeof(KnownColor));
				var randomColorName = names[randomGen.Next(names.Length)];
				var randomColor = Color.FromKnownColor(randomColorName);
				g.FillRectangle(new SolidBrush(randomColor),
					rectangle.X + offsetX, rectangle.Y + offsetY,
					rectangle.Width, rectangle.Height);
			}
			g.FillEllipse(new SolidBrush(Color.Red), Layouter.Center.X + offsetX, Layouter.Center.Y + offsetY, 10, 10);
		}

		[TestCase(13, ExpectedResult = 13)]
		[TestCase(24, ExpectedResult = 24)]
		[TestCase(39, ExpectedResult = 39)]
		[TestCase(52, ExpectedResult = 52)]
		public int LayouterRectanglesCount_ShouldBeEqual_ToNumberOfAdded(int total)
		{
			PutRandomRectangles(total);
			return Layouter.Rectangles.Count;
		}

		private void PutRandomRectangles(int total)
		{
			var rnd = new Random();
			for (var i = 0; i < total; i++)
			{
				var height = rnd.Next(15, 60);
				var width = rnd.Next(height, 150);
				Layouter.PutNextRectangle(new Size(width, height));
			}
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

		private double DistanceToCenter(Rectangle rectangle)
		{
			return Math.Sqrt(Math.Pow(rectangle.X - Layouter.Center.X, 2)
			                 + Math.Pow(rectangle.Y - Layouter.Center.Y, 2));
		}

		[Test]
		public void AddTwoRectangles_ShouldNotIntersect()
		{
			var sizeOne = new Size(4, 2);
			var sizeTwo = new Size(5, 1);
			var rectangleOne = Layouter.PutNextRectangle(sizeOne);
			var rectangleTwo = Layouter.PutNextRectangle(sizeTwo);
			Layouter.Rectangles.Count.Should().Be(2);
			rectangleOne.IntersectsWith(rectangleTwo).Should().BeFalse();
		}

		[Test]
		public void AllRectangles_FitBigCircle()
		{
			PutRandomRectangles(10);
			var totalArea = Layouter.Rectangles.Sum(rectangle => rectangle.Width * rectangle.Height);
			var excpectedRadius = Math.Sqrt(totalArea / Math.PI);

			var actualMaxRadius = Layouter.Rectangles.Select(DistanceToCenter).Max();

			Console.WriteLine($@"{excpectedRadius} {actualMaxRadius}");

			actualMaxRadius.Should().BeLessThan(2 * excpectedRadius);
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
		public void ShouldHaveCenter_AtPointZero_WhenConstructed()
		{
			Layouter.Center.Should().Be(new Point(0, 0));
		}

		[Test]
		public void ShouldHaveEmtyRectanglesList_WhenConstructed()
		{
			Layouter.Rectangles.Count.Should().Be(0);
		}

		[Test]
		public void ShouldHaveOneRectangle_AfterPutNext()
		{
			var size = new Size(20, 10);
			Layouter.PutNextRectangle(size);
			Layouter.Rectangles.Count.Should().Be(1);
		}
	}
}