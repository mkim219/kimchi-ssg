using System;
using System.IO;

namespace kimchi_ssg
{
    class kimchi_ssg
    {
        static void Main(string[] args)
        {
            try
            {
                if (args[0] == "-input" || args[0] == "--i")
                {
                    Helpers.strToFile(args[1]);
                }

                if (args[0] == "-version" || args[0] == "--v")
                {
                    Console.WriteLine(Helpers.getVersion());
                }

                if (args[0] == "-help" || args[0] == "-help")
                {
                    Console.WriteLine(Helpers.getOptions());
                }

            }
            catch(FileNotFoundException file)
            {
                Console.WriteLine("Could not file the file");
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

    }
}
