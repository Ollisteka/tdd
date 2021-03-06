﻿using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization
{
	[TestFixture]
	public class LayoutApp_Should
	{
		[SetUp]
		public void SetUp()
		{
			words = new List<string> {"Hello", "world"};
			filterMock1 = new Mock<ITextFiltration>();
			filterMock2 = new Mock<ITextFiltration>();
			readerMock1 = new Mock<IFileReader>();
			readerMock1.Setup(reader => reader.TryGetText(Input, out words)).Returns(true);
			readerMock2 = new Mock<IFileReader>();

			drawerMock = new Mock<ICloudDrawer>();
			formMock = new Mock<LayoutForm>();
			settingsMock = new Mock<ISettings>();

			frequencyCounterMock = new Mock<IFrequencyCounter>();

			layoutApp = new LayoutApp(new[] {filterMock1.Object, filterMock2.Object}, frequencyCounterMock.Object,
				drawerMock.Object,
				formMock.Object, settingsMock.Object, new[] {readerMock1.Object, readerMock2.Object});
		}

		private const string Input = "input";
		public readonly string Output = "output";
		private const int Top = 5;
		private LayoutApp layoutApp;
		private Mock<IFrequencyCounter> frequencyCounterMock;
		private Mock<ITextFiltration> filterMock1;
		private Mock<ICloudDrawer> drawerMock;
		private Mock<LayoutForm> formMock;
		private Mock<ISettings> settingsMock;
		private Mock<IFileReader> readerMock1;
		private Mock<IFileReader> readerMock2;
		private Mock<ITextFiltration> filterMock2;
		private IEnumerable<string> words;

		[Test]
		public void LayoutApp_Shoud_DrawWords()
		{
			layoutApp.Run(Input, Output);
			drawerMock.Verify(drawer => drawer.DrawWords(It.IsAny<Dictionary<string, int>>()), Times.Once);
		}

		[Test]
		public void LayoutApp_Shoud_GetTextFromReader()
		{
			readerMock1.Setup(reader => reader.TryGetText(Input, out words)).Returns(false);
			readerMock2.Setup(reader => reader.TryGetText(Input, out words)).Returns(true);
			layoutApp.Run(Input, Output);
			readerMock1.Verify(reader => reader.TryGetText(Input, out words), Times.Once);
			readerMock2.Verify(reader => reader.TryGetText(Input, out words), Times.Once);
		}

		[Test]
		public void LayoutApp_Should_FilterWords()
		{
			filterMock1.Setup(filter => filter.Filter(words)).Returns(words);

			layoutApp.Run(Input, Output);

			filterMock1.Verify(filter => filter.Filter(words), Times.Once);
			filterMock2.Verify(filter => filter.Filter(words), Times.Once);
		}

		[Test]
		public void LayoutApp_Should_GetStatistics()
		{
			filterMock2.Setup(filter => filter.Filter(It.IsAny<List<string>>())).Returns(words);
			frequencyCounterMock.Setup(stat => stat.MakeFrequencyStatistics(words, Top));

			layoutApp.Run(Input, Output, Top);

			frequencyCounterMock.Verify(stat => stat.MakeFrequencyStatistics(words, Top), Times.Once);
		}
	}
}