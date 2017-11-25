using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TagsCloudVisualization
{
	static class Program
	{
		public static LayoutForm CreateForm()
		{
			throw  new NotImplementedException();
		}

		private static List<string> MakeWords()
		{
			return File.ReadAllLines(@"random.txt").ToList();
		}
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new LayoutForm(MakeWords().Take(70)));
		}
	}
}
