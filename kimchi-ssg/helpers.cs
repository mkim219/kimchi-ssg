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
        static string generateHTMLStr(string title, string extension, string table, string style, string[] elements = null)
        {
            var bold = new Regex(@"(\*\*|__) (?=\S) (.+?[*_]*) (?<=\S) \1");
            var italic = new Regex(@"(\*|_) (?=\S) (.+?) (?<=\S) \1");
            var anchor = new Regex(@"\[([^]]*)\]\(([^\s^\)]*)[\s\)]");
            var h1 = new Regex(@"(^\#) (.*)");
            var h2 = new Regex(@"(\#\#) (.*)");
            var hr = new Regex(@"(\---) (.*)");
            var code = new Regex(@"\`([^\`].*?)\`");

            List<string> toHtml = new List<string>();
            int count = 0;

            toHtml.Add(@"<div class=""container"">");

            //suhhee_lab02 - add to distinguish function for txt files and md files
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
            return generateInterporatedstring(title, style, string.Join(Seperator.newLineSeperator, toHtml));
        }

        // parse the json file getting all valid arguments
        public static void parseJSON(string file, string output)
        {
            string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var jsonString = File.ReadAllText(sCurrentDirectory + Seperator.pathSeperator + file);

            string[] builtString = new string[4] { "", "", "", "" };
            bool valid = false;
            string style = Style.def;
            var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);

            if (dict.ContainsKey("") || jsonString == "{}")
            {
                valid = true; // case with {} json
                builtString[0] = "-v";
            }
            else
            {
                if (dict.ContainsKey("theme"))
                {
                    string found;
                    if (dict.TryGetValue("theme", out found) && found == "darkMode")
                    {
                        style = Style.darkMode;
                    }
                    else if (dict.TryGetValue("theme", out found) && found == "lightMode")
                        style = Style.lightMode;
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
                    strToFile(dict["input"], dict["output"], style);
                else
                    strToFile(dict["input"], output,style);
            }
            else
            {
                Console.WriteLine("Invalid json file contents");
            }
        }

        public static void generateHTMLfile(string html, string outputDir, string fileName)
        {
            try
            {
                if (!Directory.Exists(outputDir))
                    Directory.CreateDirectory(outputDir);

                var doc = new HtmlDocument();
                HtmlCommentNode hcn = doc.CreateComment(@"<!doctype html>"
);

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

        public static void strToFile(string file, string outputFolder, string style)
        {
            string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string fileName = Path.GetFileNameWithoutExtension(sCurrentDirectory + Seperator.pathSeperator + file);
            string extension = Path.GetExtension(sCurrentDirectory + Seperator.pathSeperator + file);
            List<string> fileList = new List<string>();

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
                fileList.Add(fileName);
                generateHTMLfile(generateHTMLStr("index", "html", generateTableOfContents(fileList), style) ,outputPath, "index"); // creating home page
                generateHTMLfile(generateHTMLStr(fileName, extension, generateTableOfContents(fileList), style, contents), outputPath, fileName);
            }
            else if (Path.GetExtension(file) == FileExtension.MARKDOWN)
            {
                var contents = File.ReadAllLines(sCurrentDirectory + Seperator.pathSeperator + file);
                fileList.Add(fileName);
                generateHTMLfile(generateHTMLStr("index", "html", generateTableOfContents(fileList), style), outputPath, "index"); // creating home page
                generateHTMLfile(generateHTMLStr(fileName, extension, generateTableOfContents(fileList), style, contents), outputPath, fileName);
            }
            else
            {
                List<string> txtList = new List<string>();
                DirectoryInfo di = new DirectoryInfo(sCurrentDirectory + Seperator.pathSeperator + file);


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
                        contents = File.ReadAllLines(filePath);
                    else
                        contents = File.ReadAllText(filePath).Split(Seperator.newLineDoubleSeperator);

                    //get title 
                    fileName = Path.GetFileNameWithoutExtension(filePath);
                    toHTMLfile = generateHTMLStr(fileName, extension, generateTableOfContents(fileList), style, contents);

                    //get saving loation
                    string saveLoc = Path.GetDirectoryName(filePath);
                    
                    generateHTMLfile(toHTMLfile, outputPath, fileName);
                }
                generateHTMLfile(generateHTMLStr("index", "html", generateTableOfContents(fileList), style), outputPath, "index"); // creating home page
            }
        }

        public static bool isLinux()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        }

        public static string generateInterporatedstring(string title, string style ,string body)
        {
            return $@"
                     <html lang=""en-CA"">
                     <head>
                     <meta charset = ""utf-8"">
                     <title> {title} </title>
                     <meta name = ""viewport"" content = ""width=device-width, initial-scale=1"">
                     {style}
                     </head>
                     <body>
                     {body}
                     </body>
                     </html>";
        }

        public static string generateTableOfContents(List<string> file)
        {
            List<string> tableOfContents = new List<string>();
            tableOfContents.Add(@"<div class=""left-nav""><nav>");
            tableOfContents.Add($@"<ul><li><a href = ""index.html"">Home</a></li>");
            foreach (var x in file)
                tableOfContents.Add($@"<li><a href = ""{x}.html"">{x}</a></li>");
            tableOfContents.Add("</ul></nav></div>");
            return String.Join(Seperator.newLineSeperator, tableOfContents.ToArray());
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