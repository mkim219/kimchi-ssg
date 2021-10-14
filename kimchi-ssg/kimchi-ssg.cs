﻿using System;
using System.IO;

namespace kimchi_ssg
{
    class kimchi_ssg
    {
        static void Main(string[] args)
        {
            try
            {
                if (args == null || args.Length == 0)
                    Console.WriteLine("Error: No arguments");

                if (args[0] == "--config" || args[0] == "-c")
                    Helpers.parseJSON(args[1], "dist");

                if (args[0] == "--input" || args[0] == "-i")
                    Helpers.strToFile(args[1], "dist");

                if (args[0] == "--version" || args[0] == "-v")
                    Console.WriteLine(Helpers.getVersion());

                if (args[0] == "--help" || args[0] == "-h")
                    Console.WriteLine(Helpers.getOptions());

            }
            catch (FileNotFoundException file)
            {
                Console.WriteLine("Could not find the file " + file);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
