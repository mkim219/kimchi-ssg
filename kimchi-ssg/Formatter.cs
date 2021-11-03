using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kimchi_ssg
{
	class Formatter
	{
		public static bool isInstalled()
		{
			bool isTrue = false;
			var checkPackagedInstalled = new Process();
			var packageInfo = new ProcessStartInfo()
			{
				FileName = "dotnet",
				Arguments = "tool list --global"
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
				isTrue = true;
			checkPackagedInstalled.WaitForExit();
			return isTrue;
		}

		public static void removeWhiteSpace()
		{
			var directory = Helpers.TryGetSolutionDirectoryInfo();
			var process = new Process();
			var startInfo = new ProcessStartInfo
			{
				FileName = "dotnet-format",
				Arguments = $"{directory}/kimchi-ssg.sln"
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

		public static void installPackage()
		{
			var install = new Process();

			var installInfo = new ProcessStartInfo
			{
				FileName = "dotnet",
				Arguments = "tool install --global dotnet-format --version 5.1.250801"
			};

			install.StartInfo = installInfo;
			install.Start();
			install.WaitForExit();
		}
	}
}
