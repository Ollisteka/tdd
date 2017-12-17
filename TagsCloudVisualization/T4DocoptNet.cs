using System.Collections;
using System.Collections.Generic;
using DocoptNet;

namespace TagsCloudVisualization
{
	// Generated class for Main.usage.txt
	public class MainArgs
	{
		public const string USAGE = @"Example usage for T4 Docopt.NET

Usage:
  prog command ARG <myarg> [OPTIONALARG] [-o -s=<arg> --long=ARG --switch]
  prog files FILE...

Options:
 -o           Short switch.
 -s=<arg>     Short option with arg.
 --long=ARG   Long option with arg.
 --switch     Long switch.

Explanation:
 This is an example usage file that needs to be customized.
 Every time you change this file, run the Custom Tool command
 on T4DocoptNet.tt to re-generate the MainArgs class
 (defined in T4DocoptNet.cs).
 You can then use the MainArgs classed as follows:

    class Program
    {

       static void DoStuff(string arg, bool flagO, string longValue)
       {
         // ...
       }

        static void Main(string[] argv)
        {
            // Automatically exit(1) if invalid arguments
            var args = new MainArgs(argv, exit: true);
            if (args.CmdCommand)
            {
                Console.WriteLine(""First command"");
                DoStuff(args.ArgArg, args.OptO, args.OptLong);
            }
        }
    }

";

		public MainArgs(ICollection<string> argv, bool help = true,
			object version = null, bool optionsFirst = false, bool exit = false)
		{
			Args = new Docopt().Apply(USAGE, argv, help, version, optionsFirst, exit);
		}

		public IDictionary<string, ValueObject> Args { get; }

		public bool CmdCommand => Args["command"].IsTrue;
		public string ArgArg => null == Args["ARG"] ? null : Args["ARG"].ToString();
		public string ArgMyarg => null == Args["<myarg>"] ? null : Args["<myarg>"].ToString();
		public string ArgOptionalarg => null == Args["OPTIONALARG"] ? null : Args["OPTIONALARG"].ToString();
		public bool OptO => Args["-o"].IsTrue;
		public string OptS => null == Args["-s"] ? null : Args["-s"].ToString();
		public string OptLong => null == Args["--long"] ? null : Args["--long"].ToString();
		public bool OptSwitch => Args["--switch"].IsTrue;
		public bool CmdFiles => Args["files"].IsTrue;
		public ArrayList ArgFile => Args["FILE"].AsList;
	}
}