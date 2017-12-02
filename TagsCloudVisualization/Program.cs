using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Autofac;
using Autofac.Core;
using DocoptNet;
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
	  TagsCloudVisualization.exe [--min=NUM] [--max=NUM] <inputfile>
	  TagsCloudVisualization.exe (-h | --help)

	Options:
	  -o FILE            Specify output file. 
	  --min=NUM          Specify the minimum words'length [default: 3]
	  --max=NUM          Specify the maximum words'length [default: 100]
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
			var build = CreateForm();
			Run(build, inputFile, outputFile, topWords, minLength, maxLength);
		}

		private static void Run(IContainer build, string inputFile, string outputFile, int top, int minLegth, int maxLength)
		{
			var text = Regex.Split(File.ReadAllText(inputFile), @"[^\p{L}]*\p{Z}[^\p{L}]*").AsEnumerable();
			var filtrations = build.Resolve<IEnumerable<ITextFiltration>>(new List<Parameter>
				{
					new NamedParameter("minLength", minLegth),
					new NamedParameter("maxLength", maxLength)
				}
			);
			var frequencyCounter = build.Resolve<IFrequencyCounter>();

			text = filtrations.Aggregate(text, (current, filtration) => filtration.Filter(current));
			var statistics = frequencyCounter.MakeFrequencyStatistics(text, top);
			var layoutDrawer = build.Resolve<ICloudDrawer>(new NamedParameter("wordsFrequency", statistics));
			var layoutForm = build.Resolve<LayoutForm>(new NamedParameter("drawer", layoutDrawer));
			if (outputFile != null)
				layoutForm.Bitmap.Save(outputFile);
			else layoutForm.ShowDialog();
		}

		public static IContainer CreateForm()
		{
			var builder = new ContainerBuilder();
			var assembly = Assembly.GetExecutingAssembly();

			builder
				.RegisterAssemblyTypes(assembly)
				.AssignableTo<ITextFiltration>()
				.AsImplementedInterfaces()
				.SingleInstance();

			builder.RegisterType<CircularCloudLayouter>().As<ICloudLayouter>();
			builder.RegisterType<CloudDrawer>().As<ICloudDrawer>();
			builder.RegisterType<FrequencyCounter>().As<IFrequencyCounter>();
			builder.RegisterType<LayoutForm>().AsSelf();

			return builder.Build();
		}
	}
}