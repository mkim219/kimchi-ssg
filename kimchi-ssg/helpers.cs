// -----------------------------------------------------------------------
// <copyright file="helpers.cs" company="Minsu Kim">
// Copyright (c) Minsu Kim. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kimchi_ssg
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text.Json;
    using System.Text.RegularExpressions;
    using HtmlAgilityPack;

    public class Helpers
    {
        Seperator seperator = new Seperator();

        private readonly IWrapper wrapper;

        public Helpers(IWrapper wrap)
        {
            wrapper = wrap;
        }

        /// <summary>
        /// convert txt file string to HTML and markdown to HTML.
        /// </summary>
        /// <param name="title">the title of file.</param>
        /// <param name="extension">md or txt.</param>
        /// <param name="table">table of contents.</param>
        /// <param name="style">theme of the pages.</param>
        /// <param name="meta">meta tags for SEO.</param>
        /// <param name="elements">the contents for HTML body.</param>
        /// <returns>return complete HTML string.</returns>
        public static string GenerateHTMLStr(string title, string extension, string table, string style, string meta, string[] elements = null)
        {
            if (elements.Length == 0)
            {
                throw new Exception("The file cannot have empty content");
            }

            var bold = new Regex(@"(\*\*|__) (?=\S) (.+?[*_]*) (?<=\S) \1");
            var italic = new Regex(@"(\*|_) (?=\S) (.+?) (?<=\S) \1");
            var anchor = new Regex(@"\[([^]]*)\]\(([^\s^\)]*)[\s\)]");
            var h1 = new Regex(@"(^\#) (.*)");
            var h2 = new Regex(@"(\#\#) (.*)");
            var hr = new Regex(@"(\---) (.*)");
            var code = new Regex(@"\`([^\`].*?)\`");

            List<string> toHtml = new ();
            int count = 0;

            toHtml.Add(@"<div class=""container"">");
            if (extension == FileExtension.TEXT)
            {
                toHtml.Add(table);
                toHtml.Add(@"<div class=""contents"">");
                foreach (var element in elements)
                {
                    if (count == 0)
                    {
                        toHtml.Add("<h1>");
                        toHtml.Add(element);
                        toHtml.Add("</h1>");
                    }
                    else
                    {
                        toHtml.Add("<p>");
                        toHtml.Add(element);
                        toHtml.Add("</p>");
                    }

                    count++;
                }
            }
            else if (extension == FileExtension.MARKDOWN)
            {
                toHtml.Add(table);
                toHtml.Add(@"<div class=""contents"">");
                foreach (var line in elements)
                {
                    var toBold = bold.Replace(line, @"<b>$2</b><br/>");
                    var toItalic = italic.Replace(toBold, @"<i>$2</i><br/>");
                    var toAnchor = anchor.Replace(toItalic, @"<a href='$1'>$2</a>");
                    var toH2 = h2.Replace(toAnchor, @"<h2>$2</h2></br>");
                    var toH1 = h1.Replace(toH2, @"<h1>$2</h1>");
                    var toHr = hr.Replace(toH1, @"<hr>");
                    var toCode = code.Replace(toHr, @"<code>$1</code>");
                    toHtml.Add(toCode);
                }
            }
            else
            {
                toHtml.Add(table);
            }

            toHtml.Add("</div></div>");
            return GenerateInterporatedstring(title, style, string.Join(Seperator.NewLineSeperator, toHtml), meta);
        }

        /// <summary>
        /// read the JSON file and extract key value pair.
        /// </summary>
        /// <param name="file">the json file.</param>
        /// <param name="output">the output directory where the output will be saved.</param>
        public static void ParseJSON(string file, string output)
        {
            string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var jsonString = File.ReadAllText(sCurrentDirectory + Seperator.PathSeperator + file);
            string[] builtString = new string[4] { string.Empty, string.Empty, string.Empty, string.Empty };
            bool valid = false;
            string style = Style.Def;
            var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);

            if (dict.ContainsKey(string.Empty) || jsonString == "{}")
            {
                valid = true; // case with {} json
                builtString[0] = "-v";
            }
            else
            {
                if (dict.ContainsKey("theme"))
                {
                    if (dict.TryGetValue("theme", out string found) && found == "darkMode")
                    {
                        style = Style.DarkMode;
                    }
                    else if (dict.TryGetValue("theme", out found) && found == "lightMode")
                    {
                        style = Style.LightMode;
                    }
                }

                if (dict.ContainsKey("input") || dict.ContainsKey("i"))
                {
                    builtString[0] = "--input";
                    builtString[1] = dict["input"];
                    valid = true;
                }
            }

            if (valid)
            {
                if (dict.ContainsKey("output"))
                {
                    ConvertStrToFile(dict["input"], dict["output"], style);
                }
                else
                {
                    ConvertStrToFile(dict["input"], output, style);
                }
            }
            else
            {
                Console.WriteLine("Invalid json file contents");
            }
        }

        /// <summary>
        /// Generate HTML file.
        /// </summary>
        /// <param name="html">complete HTML string.</param>
        /// <param name="outputDir">output directory where result will be saved.</param>
        /// <param name="fileName">the list of file name or single file name.</param>
        public static void GenerateHTMLfile(string html, string outputDir, string fileName)
        {
            try
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                var doc = new HtmlDocument();
                HtmlCommentNode hcn = doc.CreateComment(@"<!doctype html>");

                var node = HtmlNode.CreateNode(html);

                doc.DocumentNode.AppendChild(hcn);
                doc.DocumentNode.AppendChild(node);

                doc.Save(outputDir + Seperator.PathSeperator + fileName + ".html");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// convert string to HTML file.
        /// </summary>
        /// <param name="file">Receive a file list or single file.</param>
        /// <param name="outputFolder">Outout directory.</param>
        /// <param name="style">theme style.</param>
        public static void ConvertStrToFile(string file, string outputFolder, string style)
        {
            string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string fileName = Path.GetFileNameWithoutExtension(sCurrentDirectory + Seperator.PathSeperator + file);
            string extension = Path.GetExtension(sCurrentDirectory + Seperator.PathSeperator + file);
            List<string> fileList = new ();

            if (outputFolder == string.Empty || outputFolder == null)
            {
                outputFolder = "dist";
            }

            string outputPath = sCurrentDirectory + Seperator.PathSeperator + outputFolder;
            if (Directory.Exists(outputPath))
            {
                Directory.Delete(outputPath, true);
            }

            Directory.CreateDirectory(outputPath + outputFolder);

            string toHTMLfile = string.Empty;
            if (Path.GetExtension(file) == FileExtension.TEXT)
            {
                var text = File.ReadAllText(sCurrentDirectory + Seperator.PathSeperator + file);
                string[] contents = text.Split(Seperator.LinuxDoubleNewLine);

                fileList.Add(fileName);
                GenerateHTMLfile(GenerateHTMLStr("index", "html", GenerateTableOfContents(fileList), style, GenerateMeta("index")), outputPath, "index"); // creating home page
                GenerateHTMLfile(GenerateHTMLStr(fileName, extension, GenerateTableOfContents(fileList), style, GenerateMeta(fileName), contents), outputPath, fileName);
            }
            else if (Path.GetExtension(file) == FileExtension.MARKDOWN)
            {
                var contents = File.ReadAllLines(sCurrentDirectory + Seperator.PathSeperator + file);
                fileList.Add(fileName);
                GenerateHTMLfile(GenerateHTMLStr("index", "html", GenerateTableOfContents(fileList), style, GenerateMeta("index")), outputPath, "index"); // creating home page
                GenerateHTMLfile(GenerateHTMLStr(fileName, extension, GenerateTableOfContents(fileList), style, GenerateMeta(fileName), contents), outputPath, fileName);
            }
            else
            {
                List<string> txtList = new List<string>();
                DirectoryInfo di = new DirectoryInfo(sCurrentDirectory + Seperator.PathSeperator + file);


                foreach (var dir in di.EnumerateFiles().Where(x => x.ToString().EndsWith(FileExtension.TEXT) || x.ToString().EndsWith(FileExtension.MARKDOWN)))
                {
                    txtList.Add(dir.ToString());
                    fileList.Add(Path.GetFileNameWithoutExtension(dir.ToString()));
                }

                foreach (var filePath in txtList)
                {
                    // read the text's paragrah 
                    extension = Path.GetExtension(filePath);
                    string[] contents;
                    if (extension == FileExtension.MARKDOWN)
                    {
                        contents = File.ReadAllLines(filePath);
                    }
                    else
                    {
                        contents = File.ReadAllText(filePath).Split(Seperator.NewLineDoubleSeperator);
                    }

                    // get title
                    fileName = Path.GetFileNameWithoutExtension(filePath);
                    toHTMLfile = GenerateHTMLStr(fileName, extension, GenerateTableOfContents(fileList), style, GenerateMeta(fileName), contents);

                    // get saving loation
                    string saveLoc = Path.GetDirectoryName(filePath);

                    GenerateHTMLfile(toHTMLfile, outputPath, fileName);
                }
                GenerateHTMLfile(GenerateHTMLStr("index", "html", GenerateTableOfContents(fileList), style, GenerateMeta("index")), outputPath, "index"); // creating home page
            }
        }

        /// <summary>
        /// Check the OS of local mahcine.
        /// </summary>
        /// <returns>Return true when local machine is linux.</returns>
        public static bool IsLinux()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        }

        /// <summary>
        /// generate HTML string with input value.
        /// </summary>
        /// <param name="title">the tile of md or text file.</param>
        /// <param name="style">CSS style.</param>
        /// <param name="body">HTML body.</param>
        /// <param name="meta">meta tags for SEO.</param>
        /// <returns>return complete HTML string for generating HTML file.</returns>
        public static string GenerateInterporatedstring(string title, string style, string body, string meta)
        {
            return $@"
                     <html lang=""en-CA"">
                     <head>
                     <meta charset = ""utf-8"">
                     {meta}
                     <title> {title} </title>
                     <meta name = ""viewport"" content = ""width=device-width, initial-scale=1"">
                     {style}
                     </head>
                     <body>
                     {body}
                     </body>
                     </html>";
        }

        /// <summary>
        /// Generate meta tags based on user input per file.
        /// </summary>
        /// <param name="title">the title of txt or md file.</param>
        /// <returns>return meta tags.</returns>
        public static string GenerateMeta(string title)
        {
            Console.Write($"Enter keyword for {title}: ");
            string keyword = Console.ReadLine();

            Console.Write($"Enter description for {title}: ");
            string description = Console.ReadLine();

            Console.Write($"Enter author for {title}: ");
            string author = Console.ReadLine();

            return $@"<meta name=""keyword"" keyword=""{ keyword}"">
                      <meta name=""description"" description=""{description}"">
                      <meta name=""author"" description=""{author}"">";
        }

        /// <summary>
        /// Generate the table of contetns for left navigation bar.
        /// </summary>
        /// <param name="file">List data type that has input files(s).</param>
        /// <returns>It returns a string that include list of files for table of contents.</returns>
        public static string GenerateTableOfContents(List<string> file)
        {
            List<string> tableOfContents = new List<string>();
            tableOfContents.Add(@"<div class=""left-nav""><nav>");
            tableOfContents.Add($@"<ul><li><a href = ""index.html"">Home</a></li>");
            foreach (var x in file)
            {
                tableOfContents.Add($@"<li><a href = ""{x}.html"">{x}</a></li>");
            }

            tableOfContents.Add("</ul></nav></div>");
            return string.Join(Seperator.NewLineSeperator, tableOfContents.ToArray());
        }

        /// <summary>
        /// Provide "Kimchi-ssg" command line options.
        /// </summary>
        /// <returns>return options.</returns>
        public static string GetOptions()
        {
            return @"
                -i or --input<text file> : Input your text file to convert html, if the text file has space, you should use double-quote
                -h or --help: Show the options
                -v or --version: Show current version
				-c or --config: Parse json to run options
                -f or --format: Fix the format
                -l or --lint: Fix the lint 
            ";
        }

        /// <summary>
        /// Provide the version of "Kimchi-ssg".
        /// </summary>
        /// <returns>return the version.</returns>
        public static string GetVersion()
        {
            return "Current Version: 1.0.0";
        }
    }
}