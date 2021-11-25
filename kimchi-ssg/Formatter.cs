// -----------------------------------------------------------------------
// <copyright file="Formatter.cs" company="Minsu Kim">
// Copyright (c) Minsu Kim. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kimchi_ssg
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    public class Formatter
    {
        /// <summary>
        /// Check for installing dotnet-format.
        /// </summary>
        /// <returns>return true when dotnet-format installed in local, false otherwise.</returns>
        public static bool IsInstalled()
        {
            bool isTrue = false;
            var checkPackagedInstalled = new Process();
            var packageInfo = new ProcessStartInfo()
            {
                FileName = "dotnet",
                Arguments = "tool list --global",
            };

            checkPackagedInstalled.StartInfo = packageInfo;
            checkPackagedInstalled.StartInfo.UseShellExecute = false;
            checkPackagedInstalled.StartInfo.RedirectStandardOutput = true;
            checkPackagedInstalled.Start();
            string line = string.Empty;
            while (!checkPackagedInstalled.StandardOutput.EndOfStream)
            {
                line += checkPackagedInstalled.StandardOutput.ReadLine();
            }

            if (line.Contains("dotnet-format"))
            {
                isTrue = true;
            }

            checkPackagedInstalled.WaitForExit();
            return isTrue;
        }

        /// <summary>
        /// Fix the format of the code.
        /// </summary>
        public static void FixFormat()
        {
            var directory = TryGetSolutionDirectoryInfo();
            var process = new Process();
            var startInfo = new ProcessStartInfo
            {
                FileName = "dotnet-format",
                Arguments = $"{directory}{Seperator.Escape}kimchi-ssg.sln",
            };
            try
            {
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Fix the lint of the code.
        /// </summary>
        public static void FixLint()
        {
            var directory = TryGetSolutionDirectoryInfo();
            var process = new Process();
            var startInfo = new ProcessStartInfo
            {
                FileName = "dotnet-format",
                Arguments = $"-a warn {directory}{Seperator.Escape}kimchi-ssg.sln",
            };
            try
            {
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Try to find .sln file for command line option.
        /// </summary>
        /// <returns>It returns directoryInfo object that contains current sln directoryInfo.</returns>
        public static DirectoryInfo TryGetSolutionDirectoryInfo()
        {
            var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }

            return directory;
        }

        /// <summary>
        /// If the dotnet-format not install, install the package on local.
        /// </summary>
        public static void InstallPackage()
        {
            var install = new Process();

            var installInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = "tool install --global dotnet-format --version 5.1.250801",
            };

            install.StartInfo = installInfo;
            install.Start();
            install.WaitForExit();
        }
    }
}
