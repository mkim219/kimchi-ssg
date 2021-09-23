using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace kimchi_ssg
{
    public class Helpers
    {
        static readonly string HTMLstr = @"<!doctype html>
                                <html lang = ""en"">
                                <head>
                                <meta charset = ""utf-8"">
                                <title> Filename </title>
                                <meta name = ""viewport"" content = ""width=device-width, initial-scale=1"">
                                </head>
                                <body>
                                </body>
                                </html>";

        static readonly string style = @"<style>
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
        static string generateHTMLStr(string[] source, string[] elements, string title, string extension)
        {
            //Suhhee_lab02-Add regular expression for markdown files
            var bold = new Regex(@"(\*\*|__) (?=\S) (.+?[*_]*) (?<=\S) \1",
                    RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.Compiled);
            var italic = new Regex(@"(\*|_) (?=\S) (.+?) (?<=\S) \1",
                    RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.Compiled);
            var anchor = new Regex(@"\[([^]]*)\]\(([^\s^\)]*)[\s\)]",
                    RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
            var head1 = new Regex(@"(^\#) (.*)",
                    RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
            var head2 = new Regex(@"(\#\#) (.*)",
                    RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.Compiled);
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
                {
                    toHtml.Add(x);
                }

                if (x.Contains("<head>"))
                {
                    toHtml.Add(style);
                }


                if (x.Contains("<body>"))
                {
                    toHtml.Add("<div>");
                    //suhhee_lab02 - add to distinguish function for txt files and md files
                    if (extension == ".txt")
                    ///suhhee_lab02
                    {
                        foreach (var s in elements)
                        {

                            if (count == 0)
                            {
                                toHtml.Add("<h1>");
                                toHtml.Add(s);
                                toHtml.Add("</h1>");
                            }
                            else
                            {
                                toHtml.Add("<p>");
                                toHtml.Add(s);
                                toHtml.Add("</p>");
                            }

                            count++;
                        }
                    }
                    //suhhee_lab02 - add to distinguish function for txt files and md files
                    else if (extension == ".md")
                    {
                        foreach (var line in elements)
                        {//suhhee_lab02 - replace to html tags
                            var toBold = bold.Replace(line, @"<b>$2</b>");
                            var toItalic = italic.Replace(toBold, @"<b>$2</b>");
                            var toAnchor = anchor.Replace(toItalic, @"<a href='$1'>$2</a>");
                            var toHead2 = head2.Replace(toAnchor, @"<h2>$2</h2></br>");
                            var toHead1 = head1.Replace(toHead2, @"<h1>$2</h1>");

                            toHtml.Add(toHead1);
                        }
                    }
                    //suhhee_lab02
                    toHtml.Add("</div>");

                }

            }
            string toHTMLfile = string.Join(" ", toHtml);
            return toHTMLfile;
        }

        public static void generateHTMLfile(string html, string txtDir, string fileName)
        {
            try
            {
                if (!Directory.Exists(txtDir + "\\dist"))
                {
                    Directory.CreateDirectory(txtDir + "\\dist");
                }

                var doc = new HtmlDocument();
                var node = HtmlNode.CreateNode(html);
                doc.DocumentNode.AppendChild(node);
                doc.Save(txtDir + "\\dist" + "\\" + fileName + ".html");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void strToFile(string s)
        {
            string[] html = HTMLstr.Split("\n");

            string textPath = Path.GetFullPath(s);

            string txtDirectory = Path.GetDirectoryName(textPath);



            if (Directory.Exists(txtDirectory + "\\dist"))
            {
                Directory.Delete(txtDirectory + "\\dist", true);
                Console.WriteLine("The dist folder has been deleted");
            }

            string toHTMLfile = string.Empty;
            if (Path.GetExtension(textPath) == ".txt")
            {
                var text = File.ReadAllText(textPath);
                string fileName = Path.GetFileNameWithoutExtension(textPath);
                string extension = Path.GetExtension(textPath);

                string[] contents = text.Split("\n\n");
                toHTMLfile = generateHTMLStr(html, contents, fileName, extension);

                generateHTMLfile(toHTMLfile, txtDirectory, fileName);
            }
            //suhhee_lab02 - edited to get md files in folder
            else if (Path.GetExtension(textPath) == ".md") // case user input md file
            {
                var contents = File.ReadAllLines(textPath);
                string fileName = Path.GetFileNameWithoutExtension(textPath);
                string extension = Path.GetExtension(textPath);

                toHTMLfile = generateHTMLStr(html, contents, fileName, extension);

                generateHTMLfile(toHTMLfile, txtDirectory, fileName);

            }
            // suhhee_lab02


            //suhhee_lab02 - edited to get md files in folder
            else // Case user input the folder path
            {

                List<string> txtList = new List<string>();
                DirectoryInfo di = new DirectoryInfo(txtDirectory);

                //suhhee_lab02 - added md file extension
                foreach (var dir in di.EnumerateFiles("*", SearchOption.AllDirectories).Where(x => x.ToString().EndsWith(".txt") || x.ToString().EndsWith(".md")))
                //suhhee_lab02
                {
                    txtList.Add(dir.ToString());
                }

                foreach (var filePath in txtList)
                {
                    // read the text's paragrah 
                    string extension = Path.GetExtension(filePath);
                    string[] contents;
                    //suhhee_lab02 - add to read md files
                    if (extension == ".md")
                    {
                        contents = File.ReadAllLines(filePath);
                    }
                    else
                    {
                        contents = File.ReadAllText(filePath).Split("\n\n");
                    }
                    //suhhee_lab02

                    List<string> pathSplit = filePath.Split("\\").ToList();
                    //get title 
                    string fileName = Path.GetFileNameWithoutExtension(filePath);


                    toHTMLfile = generateHTMLStr(html, contents, fileName, extension);

                    //get saving loation
                    string saveLoc = Path.GetDirectoryName(filePath);

                    generateHTMLfile(toHTMLfile, saveLoc, fileName);

                }
            }

        }

        public static string getOptions()
        {
            return @"
                --i or -input<text file> : Input your text file to convert html, if the text file has space, you should use double-quote
                --h or -help: Show the options
                --v or -version: show current version
            ";
        }

        public static string getVersion()
        {
            return "Current Version: 1.0.0";
        }


    }
}
