using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Moq;
using NUnit.Framework;
using TagsCloudVisualization.Interfaces;
using TagsCloudVisualization.TextProcessing;

namespace TagsCloudVisualization
{
	[TestFixture]
	public class LayoutApp_Should
	{
		private string input = "asd";
		private string output = "asd";
		private int top = 100;
		private LayoutApp layoutApp;
		private Mock<IFrequencyCounter> frequencyCounterMock;
		private Mock<ITextFiltration> filterMock1;
		private Mock<ICloudDrawer> drawerMock;
		private Mock<ICloudLayouter> layouterMock;
		private Mock<LayoutForm> formMock;
		private Mock<ISettings> settingsMock;
		private Mock<IFileReader> readerMock;
		private Mock<ITextFiltration> filterMock2;
		private Dictionary<string, int> statistics;
		private List<string> words;

		[SetUp]
		public void SetUp()
		{
			words = new List<string>() {"Hello", "world"};
			statistics = words.Select(word => new
				{
					KeyField = word,
					Count = 10
				})
				.ToDictionary(key => key.KeyField, val => val.Count);
			filterMock1 = new Mock<ITextFiltration>();
			filterMock2 = new Mock<ITextFiltration>();
			drawerMock = new Mock<ICloudDrawer>();
			layouterMock = new Mock<ICloudLayouter>();
			formMock = new Mock<LayoutForm>();
			settingsMock = new Mock<ISettings>();
			readerMock = new Mock<IFileReader>();
			
			frequencyCounterMock = new Mock<IFrequencyCounter>();
			
			layoutApp = new LayoutApp(new []{filterMock1.Object, filterMock2.Object}, frequencyCounterMock.Object, drawerMock.Object,
				formMock.Object, settingsMock.Object, readerMock.Object);
		}

		[Test]
		public void LayoutApp_Shoud_GetTextFromReader()
		{
			
			layoutApp.Run(input, output, 100, 1, 100);
			readerMock.Verify(reader => reader.GetText(input));
		}

		[Test]
		public void LayoutApp_Should_FilterWords()
		{
			readerMock.Setup(reader => reader.GetText(input)).Returns(words);
			filterMock1.Setup(filter => filter.Filter(words)).Returns(words);

			layoutApp.Run(input, output, 100, 1, 100);

			filterMock1.Verify(filter => filter.Filter(words), Times.Once);
			filterMock2.Verify(filter => filter.Filter(words), Times.Once);
		}

		[Test]
		public void LayoutApp_Should_GetStatistics()
		{
			filterMock2.Setup(filter => filter.Filter(It.IsAny<List<string>>())).Returns(words);
			frequencyCounterMock.Setup(stat => stat.MakeFrequencyStatistics(words, top));

			layoutApp.Run(input, output, top, 1, 100);

			frequencyCounterMock.Verify(stat => stat.MakeFrequencyStatistics(words, top), Times.Once);
		}

		[Test]
		public void LayoutApp_Shoud_DrawWords()
		{
			layoutApp.Run(input, output, top, 1, 100);
			drawerMock.Verify(drawer => drawer.DrawWords(It.IsAny<Dictionary<string,int>>()), Times.Once);
		}

	}
}