using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Autofac;
using NHunspell;

namespace TagsCloudVisualization
{
	public class AutofacConfig
	{
		public static void ConfigureContainer()
		{
			// получаем экземпляр контейнера
			var builder = new ContainerBuilder();

			// регистрируем контроллер в текущей сборке
			//builder.(typeof(Program).Assembly);

			// регистрируем споставление типов
			//builder.RegisterType<BookRepository>().As<IRepository>();

			// создаем новый контейнер с теми зависимостями, которые определены выше
			var container = builder.Build();

			// установка сопоставителя зависимостей
			//DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
		}
	}
	internal static class Program
	{
		public static LayoutForm CreateForm()
		{
			throw new NotImplementedException();
		}

		private static Dictionary<string, int> ProccessWords(string filename)
		{
			var content = File.ReadAllText(filename);

			using (var hunspell = new Hunspell("dictionaries/ru_RU.aff", "dictionaries/ru_RU.dic"))
			{
				return Regex.Split(content, @"[^\p{L}]*\p{Z}[^\p{L}]*")
					.Where(x => x.Length > 3)
					.Select(x =>
					{
						var word = x.ToLower();
						//var morphs = hunspell.Analyze(word);
						var stems = hunspell.Stem(word);
						return stems.Any() ? stems[0] : word;
					})
					.GroupBy(x => x)
					.Select(x => new
					{
						KeyField = x.Key,
						Count = x.Count()
					})
					.OrderByDescending(x => x.Count)
					.Take(100).ToDictionary(key => key.KeyField, val => val.Count);
			}
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main()
		{
			var inputFile = @"books/harry.txt";
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new LayoutForm(ProccessWords(inputFile)));
		}
	}
}