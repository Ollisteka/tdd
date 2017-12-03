using System;
using System.Collections.Generic;
using System.Drawing;
using Moq;
using NUnit.Framework;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization
{
	[TestFixture]
	public class CloudDrawer_Should
	{
		[SetUp]
		public void SetUp()
		{
			layouter = new Mock<ICloudLayouter>();
			drawer = new CloudDrawer(layouter.Object, new Settings());
		}

		private ICloudDrawer drawer;
		private Mock<ICloudLayouter> layouter;
		private readonly Dictionary<string, int> words = new Dictionary<string, int> {{"Hello", 10}, {"lalala", 12}};

		[Test]
		public void CloudDrawer_Should_CallLayouter()
		{
			drawer.DrawWords(words);
			layouter.Verify(a => a.PutNextRectangle(It.IsAny<Size>()), Times.Exactly(words.Count));
		}
	}
}