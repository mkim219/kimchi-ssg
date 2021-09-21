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


        static string generateHTMLStr(string[] source, string[] elements, string title)
        {

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
            if (textPath.Contains(".txt"))
            {
                var text = File.ReadAllText(textPath);
                string fileName = Path.GetFileNameWithoutExtension(textPath);

                string[] contents = text.Split("\n\n");
                toHTMLfile = generateHTMLStr(html, contents, fileName);

                generateHTMLfile(toHTMLfile, txtDirectory, fileName);
            }
            else // Case user input the folder path
            {

                List<string> txtList = new List<string>();
                DirectoryInfo di = new DirectoryInfo(txtDirectory);

                foreach (var dir in di.EnumerateFiles("*", SearchOption.AllDirectories).Where(x => x.ToString().EndsWith(".txt")))
                {
                    txtList.Add(dir.ToString());
                }

                foreach (var filePath in txtList)
                {
                    // read the text's paragrah 
                    var text = File.ReadAllText(filePath);
                    string[] contents = text.Split("\n\n");

                    List<string> pathSplit = filePath.Split("\\").ToList();
                    //get title 
                    string fileName = Path.GetFileNameWithoutExtension(filePath);
                   
                    toHTMLfile = generateHTMLStr(html, contents, fileName);

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
