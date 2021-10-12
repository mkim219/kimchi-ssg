using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.Text.Json;
using System.Runtime.InteropServices;//check os


namespace kimchi_ssg
{

    public class Helpers
    {
        static string generateHTMLStr(string[] source, string[] elements, string title, string extension)
        {
            string style = @"<style>
                                *{
                                    background-color: #9999FF;
                                 }
                                div { 
                                      color: #FFFFFF;
                                      position: absolute;
                                      width: 700px;
                                      height: -100px;
                                      top: 0;
                                      bottom: 0;
                                      left: 0;
                                      right: 0;
                                      margin: auto;
                                  }
                               </style>";

            //Suhhee_lab02-Add regular expression for markdown files
            var bold = new Regex(@"(\*\*|__) (?=\S) (.+?[*_]*) (?<=\S) \1");
            var italic = new Regex(@"(\*|_) (?=\S) (.+?) (?<=\S) \1");
            var anchor = new Regex(@"\[([^]]*)\]\(([^\s^\)]*)[\s\)]");
            var h1 = new Regex(@"(^\#) (.*)");
            var h2 = new Regex(@"(\#\#) (.*)");
            var hr = new Regex(@"(\---) (.*)");
            var code = new Regex(@"\`([^\`].*?)\`");

            //suhhee_lab02
            List<string> toHtml = new List<string>();
            int count = 0;
            foreach (var x in source)
            {
                if (x.Contains("Filename"))
                {
                    string temp = x.Replace("Filename", title);
                    toHtml.Add(temp);
                }
                else
                    toHtml.Add(x);

                if (x.Contains("<head>"))
                    toHtml.Add(style);

                if (x.Contains("<body>"))
                {
                    toHtml.Add("<div>");
                    //suhhee_lab02 - add to distinguish function for txt files and md files
                    if (extension == FileExtension.TEXT)
                    {
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
                        foreach (var line in elements)
                        {
                            var toBold = bold.Replace(line, @"<b>$2</b><br/>");
                            var toItalic = italic.Replace(toBold, @"<i>$2</i><br/>");
                            var toAnchor = anchor.Replace(toItalic, @"<a href='$1'>$2</a>");
                            var toH2 = h2.Replace(toAnchor, @"<h2>$2</h2></br>");
                            var toH1 = h1.Replace(toH2, @"<h1>$2</h1>");
                            var toHr = hr.Replace(toH1, @"<hr>");
                            var toCode = code.Replace(toH1, @"<code>$1</code>");
                            toHtml.Add(toCode);
                        }
                    }
                    toHtml.Add("</div>");
                }
            }

            string toHTMLfile = string.Join(Seperator.newLineSeperator, toHtml);
            return toHTMLfile;
        }

        // parse the json file getting all valid arguments
        public static void parseJSON(string file, string output)
        {
            string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var jsonString = File.ReadAllText(sCurrentDirectory + Seperator.pathSeperator + file);

            string[] builtString = new string[4] { "", "", "", "" };
            bool valid = false;
            var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);

            if (dict.ContainsKey("") || jsonString == "{}")
            {
                valid = true; // case with {} json
                builtString[0] = "-v";
            }
            else
            {
                if (dict.ContainsKey("input"))
                {
                    builtString[0] = "--input";
                    builtString[1] = dict["input"];
                    valid = true;
                }
                else if (dict.ContainsKey("i"))
                {
                    builtString[0] = "-i";
                    builtString[1] = dict["i"];
                    valid = true;
                }
                else if (dict.ContainsKey("output"))
                {
                    builtString[2] = "--output ";
                    builtString[3] = dict["output"];
                    output = dict["output"];
                }

            }
            if (valid)
            {
                if (dict.ContainsKey("output"))
                {
                    strToFile(builtString[1], dict["output"]);
                    output = dict["output"];
                }
                else
                    strToFile(builtString[1], output);
            }
            else
            {
                Console.WriteLine("Invalid json file contents");
            }
        }

        public static void generateHTMLfile(string html, string outputDir, string fileName)//
        {
            try
            {
                if (!Directory.Exists(outputDir))
                    Directory.CreateDirectory(outputDir);

                var doc = new HtmlDocument();
                HtmlCommentNode hcn = doc.CreateComment("<!doctype html>");

                var node = HtmlNode.CreateNode(html);

                doc.DocumentNode.AppendChild(hcn);
                doc.DocumentNode.AppendChild(node);

                doc.Save(outputDir + Seperator.pathSeperator + fileName + ".html");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void strToFile(string file, string outputFolder)
        {
            string HTMLstr = @"
                                <html lang=""en-CA"">
                                <head>
                                <meta charset = ""utf-8"">
                                <title> Filename </title>
                                <meta name = ""viewport"" content = ""width=device-width, initial-scale=1"">
                                </head>
                                <body>
                                </body>
                                </html>";

            string[] html = HTMLstr.Split(Seperator.newLineSeperator);
            string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string fileName = Path.GetFileNameWithoutExtension(sCurrentDirectory + Seperator.pathSeperator + file);
            string extension = Path.GetExtension(sCurrentDirectory + Seperator.pathSeperator + file);

            //added safety check
            if (outputFolder == "" || outputFolder == null)
                outputFolder = "dist";

            string outputPath = sCurrentDirectory + Seperator.pathSeperator + outputFolder;
            if (Directory.Exists(outputPath))
                Directory.Delete(outputPath, true);
            Directory.CreateDirectory(outputPath + outputFolder);

            string toHTMLfile = string.Empty;
            if (Path.GetExtension(file) == FileExtension.TEXT)
            {
                var text = File.ReadAllText(sCurrentDirectory + Seperator.pathSeperator + file);
                string[] contents = text.Split(Seperator.newLineDoubleSeperator);
                generateHTMLfile(generateHTMLStr(html, contents, fileName, extension), outputPath, fileName);
            }
            else if (Path.GetExtension(file) == FileExtension.MARKDOWN)
            {
                var contents = File.ReadAllLines(sCurrentDirectory + Seperator.pathSeperator + file);
                generateHTMLfile(generateHTMLStr(html, contents, fileName, extension), outputPath, fileName);
            }
            else
            {
                List<string> txtList = new List<string>();
                DirectoryInfo di = new DirectoryInfo(sCurrentDirectory + Seperator.pathSeperator + file);

                foreach (var dir in di.EnumerateFiles().Where(x => x.ToString().EndsWith(FileExtension.TEXT) || x.ToString().EndsWith(FileExtension.MARKDOWN)))
                {
                    txtList.Add(dir.ToString());
                }

                foreach (var filePath in txtList)
                {
                    // read the text's paragrah 
                    extension = Path.GetExtension(filePath);
                    string[] contents;
                    if (extension == FileExtension.MARKDOWN)
                        contents = File.ReadAllLines(filePath);
                    else
                        contents = File.ReadAllText(filePath).Split(Seperator.newLineDoubleSeperator);

                    //get title 
                    fileName = Path.GetFileNameWithoutExtension(filePath);
                    toHTMLfile = generateHTMLStr(html, contents, fileName, extension);

                    //get saving loation
                    string saveLoc = Path.GetDirectoryName(filePath);

                    generateHTMLfile(toHTMLfile, outputPath, fileName);
                }
            }
        }

        public static bool isLinux()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        }

        public static string getOptions()
        {
            return @"
                -i or --input<text file> : Input your text file to convert html, if the text file has space, you should use double-quote
                -h or --help: Show the options
                -v or --version: show current version
				-c or --config: parse json to run options
            ";
        }

        public static string getVersion()
        {
            return "Current Version: 1.0.0";
        }


    }
}
