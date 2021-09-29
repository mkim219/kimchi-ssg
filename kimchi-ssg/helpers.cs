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

        static readonly string HTMLstr = @"
                                <html lang=""en-CA"">
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
            var h1 = new Regex(@"(^\#) (.*)",
                    RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
            var h2 = new Regex(@"(\#\#) (.*)",
                    RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.Compiled);
            var code = new Regex(@"\`([^\`].*?)\`",
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
                            var toBold = bold.Replace(line, @"<b>$2</b><br/>");
                            var toItalic = italic.Replace(toBold, @"<i>$2</i><br/>");
                            var toAnchor = anchor.Replace(toItalic, @"<a href='$1'>$2</a>");
                            var toH2 = h2.Replace(toAnchor, @"<h2>$2</h2></br>");
                            var toH1 = h1.Replace(toH2, @"<h1>$2</h1>");
                            var toCode = code.Replace(toH1, @"<code>$1</code>");

                            toHtml.Add(toCode);
                        }
                    }
                    //suhhee_lab02
                    toHtml.Add("</div>");

                }

            }

            string toHTMLfile = string.Join("\n", toHtml);
            return toHTMLfile;
        }

        public static void generateHTMLfile(string html, string txtDir, string fileName)//
        {
            try
            {
                if (!Directory.Exists(txtDir))
                {
                    Directory.CreateDirectory(txtDir);
                   
                }
                var doc = new HtmlDocument();
                HtmlCommentNode hcn = doc.CreateComment("<!doctype html>");
  

                var node = HtmlNode.CreateNode(html);

                doc.DocumentNode.AppendChild(hcn);
                doc.DocumentNode.AppendChild(node);
                doc.Save(txtDir + "\\" + fileName + ".html");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void strToFile(string s)
        {
            //suhhee_lab02 - create HTML files in dist folder in main directory
            string[] html = HTMLstr.Split("\n");
            string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string sFile = Path.Combine(sCurrentDirectory, @"..\\net5.0");
            string textPath = Path.GetFullPath(sFile);

            string txtDirectory = Path.GetDirectoryName(textPath);
            //suhhee_lab02



            if (Directory.Exists(txtDirectory+ "\\net5.0\\dist"))
            {
                Directory.Delete(txtDirectory + "\\net5.0\\dist", true);
                
             
            }
            Directory.CreateDirectory(txtDirectory + "\\net5.0\\dist");
           
            string toHTMLfile = string.Empty;
            if (Path.GetExtension(s) == ".txt")
            {
                var text = File.ReadAllText(txtDirectory + "\\net5.0\\"+ s);
                string fileName = Path.GetFileNameWithoutExtension(txtDirectory + "\\net5.0\\"+s);
                string extension = Path.GetExtension(txtDirectory + "\\net5.0\\"+s);

                string[] contents = text.Split("\n\n");
                toHTMLfile = generateHTMLStr(html, contents, fileName, extension);

                generateHTMLfile(toHTMLfile, txtDirectory + "\\net5.0\\dist", fileName);
            }
            //suhhee_lab02 - edited to get md files 
            else if (Path.GetExtension(s) == ".md") 
            {
               
                var contents = File.ReadAllLines(txtDirectory + "\\net5.0\\"+s);

                string fileName = Path.GetFileNameWithoutExtension(s);
                string extension = Path.GetExtension(s);
               
                toHTMLfile = generateHTMLStr(html, contents, fileName, extension);

                generateHTMLfile(toHTMLfile, txtDirectory + "\\net5.0\\dist", fileName);

            }
            // suhhee_lab02


            //suhhee_lab02 - edited to get md files in folder
            else 
            {

                List<string> txtList = new List<string>();
                DirectoryInfo di = new DirectoryInfo(txtDirectory + "\\net5.0\\"+s);

            
                //suhhee_lab02 - added md file extension
                foreach (var dir in di.EnumerateFiles().Where(x => x.ToString().EndsWith(".txt") || x.ToString().EndsWith(".md")))
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
                    string saveLoc = Path.GetDirectoryName(filePath); /////

                    generateHTMLfile(toHTMLfile, txtDirectory + "\\net5.0\\dist", fileName);

                }
            }

        }

        public static string getOptions()
        {
            return @"
                -i or --input<text file> : Input your text file to convert html, if the text file has space, you should use double-quote
                -h or --help: Show the options
                -v or --version: show current version
            ";
        }

        public static string getVersion()
        {
            return "Current Version: 1.0.0";
        }


    }
}
