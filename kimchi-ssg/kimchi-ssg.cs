using System;
using System.IO;

namespace Kimchi_ssg
{
    class Kimchi_ssg
    {
        static void Main(string[] args)
        {
            try
            {
                if (args == null || args.Length == 0)
                {
                    Console.WriteLine("Error: No arguments");
                }

                if (args[0] == "--config" || args[0] == "-c")
                {
                    Helpers.ParseJSON(args[1], "dist");
                }

                if (args[0] == "--input" || args[0] == "-i")
                {
                    Helpers.ConvertStrToFile(args[1], "dist", Style.def);
                }

                if (args[0] == "--version" || args[0] == "-v")
                {
                    Console.WriteLine(Helpers.GetVersion());
                }

                if (args[0] == "--help" || args[0] == "-h")
                {
                    Console.WriteLine(Helpers.GetOptions());
                }

                if (args[0] == "--format" || args[0] == "-f")
                {
                    if (!Formatter.IsInstalled())
                    {
                        Formatter.InstallPackage();
                        Formatter.FixFormat();
                    }
                    else
                    {
                        Formatter.FixFormat();
                    }
                }
            }
            catch (FileNotFoundException file)
            {
                Console.WriteLine("Could not find the file " + file);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
