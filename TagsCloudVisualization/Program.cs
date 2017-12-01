using System;
using System.IO;
using System.Linq;
using Autofac;
using DocoptNet;
using NHunspell;
using TagsCloudVisualization.Interfaces;
using TagsCloudVisualization.TextProcessing;

namespace TagsCloudVisualization
{
	internal static class Program
	{
		private const string Usage = @"Tags Cloud Visualization. Supports only Rissin for now.

	Usage:
	  TagsCloudVisualization.exe <inputfile>
	  TagsCloudVisualization.exe [-t NUM | --top=NUM] [-o FILE] <inputfile>
	  TagsCloudVisualization.exe (-h | --help)

	Options:
	  -o FILE            Specify output file. 
	  -t NUM --top=NUM   Specify how many words to show [default: 50]
	  -h --help          Show this screen.

	";

		/// <summary>
		///     The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			var arguments = new Docopt().Apply(Usage, args, optionsFirst: true, exit: true);

//			foreach (var argument in arguments)
//				Console.WriteLine("{0} = {1}", argument.Key, argument.Value);

			var inputFile = arguments["<inputfile>"].ToString();
			var outputFile = arguments["-o"]?.ToString();
			var topWords = arguments["--top"].AsInt;
			var layoutForm = CreateForm(inputFile, topWords);
			if (outputFile != null)
				layoutForm.Bitmap.Save(outputFile);
			else layoutForm.ShowDialog();
		}

		public static LayoutForm CreateForm(string inputFile, int top)
		{
			var container = new ContainerBuilder();
			container.RegisterType<LayoutForm>().AsSelf();

			container.Register(c => new FileHandler(inputFile)).As<ITextReader>();

			container.RegisterType<HunspellFilter>().As<ITextFiltration>();

			container.Register<Func<string, bool>>(c => { return word => word.Length >= 3; });

			var solutiondir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
			container.Register(c =>
					new Hunspell(solutiondir + "//dictionaries//ru_RU.aff", solutiondir + "//dictionaries//ru_RU.dic"))
				.SingleInstance();

			container.Register<Func<string, string>>(c =>
			{
				return x =>
				{
					var word = x.ToLower();
					var stems = c.Resolve<Hunspell>().Stem(word);
					return stems.Any() ? stems[0] : word;
				};
			}).SingleInstance();

			container.RegisterType<FrequencyCounter>().As<IFrequencyCounter>().WithParameter("top", top);

			container.RegisterType<CircularCloudLayouter>().As<ICloudLayouter>();
			container.RegisterType<CloudDrawer>().As<ICloudDrawer>();
			var build = container.Build();
			return build.Resolve<LayoutForm>();
		}
	}
}