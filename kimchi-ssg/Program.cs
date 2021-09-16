using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using HtmlAgilityPack;


namespace kimchi_ssg
{
    class Program
    {


        static string HTMLstr = @"<!doctype html>
                                <html lang = ""en"">
                                <head>
                                  <meta charset = ""utf-8"">
                                  <title> Filename </title>
                                  <meta name = ""viewport"" content = ""width=device-width, initial-scale=1"">
                                </head>
                                <body>
                                  
                                </body>
                                </html>";

        static string style = @"<style>
                                * { 
                                text-align: justify;
                                text-align: center;
                                background-color: #9999FF;
                                color: #FFFFFF
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

                if (x.Contains("head"))
                {
                    toHtml.Add(style);
                }
                

                if (x.Contains("<body>"))
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

            }
            string toHTMLfile = string.Join(" ", toHtml);
            return toHTMLfile;
        }

        static void file(string s)
        {
            string[] html = HTMLstr.Split("\n");

            //read text file or folder directory

            string textPath = Path.GetFullPath(s);
            string titleWithExt = textPath.Substring(textPath.LastIndexOf("\\") + 1);
            string title = titleWithExt.Substring(0, titleWithExt.LastIndexOf("."));

            string correctPath = textPath.Substring(0, textPath.LastIndexOf("\\"));

            string toHTMLfile = string.Empty;
            if (textPath.Contains(".txt"))
            {
                var text = File.ReadAllText(textPath);
                string[] contents = text.Split("\n\n");

                toHTMLfile = generateHTMLStr(html, contents, title);
                string removeExt = textPath.Substring(0, textPath.Length - 4);
                string fileName = removeExt.Substring(removeExt.LastIndexOf("\\"));
                string saveLoc = textPath.Substring(0, textPath.LastIndexOf("\\"));

                try
                {
                    if (!Directory.Exists(saveLoc + "\\dist"))
                    {
                        Directory.CreateDirectory(saveLoc + "\\dist");
                    }

                    var doc = new HtmlDocument();
                    var node = HtmlNode.CreateNode(toHTMLfile);
                    doc.DocumentNode.AppendChild(node);
                    doc.Save(saveLoc + "\\dist" + fileName + ".html");

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }


            }
            else // Case user input the folder path
            {
                
                string folderPath = string.Join("\\", correctPath);
                List<string> txtList = new List<string>();
                DirectoryInfo di = new DirectoryInfo(folderPath);

                foreach (var dir in di.EnumerateFiles("*", SearchOption.AllDirectories).Where(x => x.ToString().EndsWith(".txt")))
                {
                    txtList.Add(dir.ToString());
                }

                foreach (var filePath in txtList)
                {
                    // extract the text's paragrah 
                    var text = File.ReadAllText(filePath);
                    string[] contents = text.Split("\n\n");

                    //get title 
                    List<string> pathSplit = filePath.Split("\\").ToList();
                    string titleM = pathSplit.Last().Substring(0, pathSplit.Last().LastIndexOf("."));

                    toHTMLfile = generateHTMLStr(html, contents, titleM);

                    string removeExt = filePath.Substring(0, filePath.Length - 4);
                    string fileName = removeExt.Substring(removeExt.LastIndexOf("\\"));
                    string saveLoc = filePath.Substring(0, filePath.LastIndexOf("\\"));

                    try
                    {
                        if (!Directory.Exists(folderPath + "\\dist"))
                        {
                            Directory.CreateDirectory(folderPath + "\\dist");
                        }
                        var doc = new HtmlDocument();
                        var node = HtmlNode.CreateNode(toHTMLfile);
                        doc.DocumentNode.AppendChild(node);
                        doc.Save(saveLoc + "\\dist" + fileName + ".html");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }

        }
        static int Main(string[] args)
        {

            var tokenInput = new Option<string>("--input", "Input your text file to convert html");
            tokenInput.AddAlias("--i");


            var cmd = new RootCommand();

            cmd.AddOption(tokenInput);

            cmd.Handler = CommandHandler.Create(() =>
            {
                file(args[1]);
            });

            return cmd.Invoke(args);
        }
    }
}
