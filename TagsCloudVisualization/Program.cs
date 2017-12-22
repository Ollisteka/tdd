using System;
using System.IO;
using System.Reflection;
using Autofac;
using DocoptNet;
using TagsCloudVisualization.Interfaces;
using TagsCloudVisualization.TextProcessing;

namespace TagsCloudVisualization
{
	internal static class Program
	{
		private const string Usage = @"Tags Cloud Visualization. Supports only Russin for now.

	Usage:
	  TagsCloudVisualization.exe <inputfile>
	  TagsCloudVisualization.exe [-t NUM | --top=NUM] [-o FILE] [--min=NUM] [--max=NUM] [--lower=NUM] [--upper=NUM] <inputfile>
	  TagsCloudVisualization.exe (-h | --help)

	Options:
	  -o FILE            Specify output file. 
	  --min=NUM          Specify the minimum words'length [default: 3]
	  --max=NUM          Specify the maximum words'length [default: 100]
	  --lower=NUM        Specify the minimum words'font size [default: 15]
	  --upper=Num        Specify the maximum words'font size  [default: 35]
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
			var minLength = arguments["--min"].AsInt;
			var maxLength = arguments["--max"].AsInt;
			var minFont = arguments["--lower"].AsInt;
			var maxFont = arguments["--upper"].AsInt;

			CheckForFilesExistance(inputFile);
			if (topWords <= 0)
				LayoutApp.ExitWithError($"The amount of words to print should be positive. You had: {topWords}");

			CreateApp().Run(inputFile, outputFile, topWords, minLength, maxLength, minFont, maxFont);
		}


		private static void CheckForFilesExistance(params string[] filenames)
		{
			foreach (var file in filenames)
				if (!string.IsNullOrEmpty(file) && !File.Exists(file))
					LayoutApp.ExitWithError($"File \"{file}\" doesn't exist");
		}

		public static LayoutApp CreateApp()
		{
			var builder = new ContainerBuilder();
			var assembly = Assembly.GetExecutingAssembly();

			builder
				.RegisterAssemblyTypes(assembly)
				.AssignableTo<ITextFiltration>()
				.AsImplementedInterfaces()
				.SingleInstance();

			builder
				.RegisterAssemblyTypes(assembly)
				.AssignableTo<IFileReader>()
				.AsImplementedInterfaces()
				.SingleInstance();

			builder.RegisterType<Settings>()
				.As<ISettings>()
				.SingleInstance();

			builder.RegisterType<DocReader>().As<IFileReader>();

			builder.RegisterType<CircularCloudLayouter>().As<ICloudLayouter>();
			builder.RegisterType<CloudDrawer>().As<ICloudDrawer>();
			builder.RegisterType<FrequencyCounter>().As<IFrequencyCounter>();

			builder.RegisterType<LayoutForm>().AsSelf();
			builder.RegisterType<LayoutApp>().AsSelf();

			var build = builder.Build();

			return build.Resolve<LayoutApp>();
		}
	}
}