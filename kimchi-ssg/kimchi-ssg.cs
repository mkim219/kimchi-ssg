using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using HtmlAgilityPack;


namespace kimchi_ssg
{
    class kimchi_ssg
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

        static void file(string s)
        {
            string[] html = HTMLstr.Split("\n");

            string textPath = Path.GetFullPath(s);

            string txtDirectory = Path.GetDirectoryName(textPath);

            
            if(Directory.Exists(txtDirectory + "\\dist"))
            {
                Directory.Delete(txtDirectory + "\\dist",true);
                Console.WriteLine("The dist folder has been deleted");
            }

            string toHTMLfile = string.Empty;
            if (textPath.Contains(".txt"))
            {
                var text = File.ReadAllText(textPath);
                string fileName = Path.GetFileNameWithoutExtension(textPath);

                string[] contents = text.Split("\n\n");
                toHTMLfile = generateHTMLStr(html, contents, fileName);

                try
                {
                    if (!Directory.Exists(txtDirectory + "\\dist"))
                    {
                        Directory.CreateDirectory(txtDirectory + "\\dist");
                    }

                    var doc = new HtmlDocument();
                    var node = HtmlNode.CreateNode(toHTMLfile);
                    doc.DocumentNode.AppendChild(node);
                    doc.Save(txtDirectory + "\\dist" + "\\" + fileName + ".html");

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
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
                    Console.WriteLine(fileName);
                    toHTMLfile = generateHTMLStr(html, contents, fileName);

                    //get saving loation
                    string saveLoc = Path.GetDirectoryName(filePath);

                    try
                    {
                        if (!Directory.Exists(saveLoc + "\\dist"))
                        {
                            Directory.CreateDirectory(saveLoc + "\\dist");
                        }
                        var doc = new HtmlDocument();
                        var node = HtmlNode.CreateNode(toHTMLfile);
                        doc.DocumentNode.AppendChild(node);
                        doc.Save(saveLoc + "\\dist" + "\\" + fileName + ".html");
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

            var tokenInput = new Option<string>("--input", "Input your text file to convert html, if the text file has space, you should use double-quote");
            tokenInput.AddAlias("-i");


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
