using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Autofac;
using Autofac.Core;
using DocoptNet;
using TagsCloudVisualization.Interfaces;

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
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			var layoutForm = CreateForm(inputFile, outputFile, topWords);
			Application.Run(layoutForm);
			if (outputFile != null)
				layoutForm.Bitmap.Save(outputFile);
			
		}

		public static LayoutForm CreateForm(string inputFile, string  outputFile, int top)
		{
			var container = new ContainerBuilder();
			container.RegisterType<LayoutForm>().AsSelf();
			container.RegisterType<FileHandler>()
				.As<ITextHandler>()
				.WithParameters(new List<Parameter>
				{
					new NamedParameter("filename", inputFile),
					new NamedParameter("top", top)
				});
			container.RegisterType<CloudDrawer>().As<ICloudDrawer>();
			var build = container.Build();
			return build.Resolve<LayoutForm>();
		}
	}
}