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
            var hr = new Regex(@"(\---) (.*)",
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
                    }//suhhee_lab02 - add to distinguish function for txt files and md files
                    else if (extension == ".md")
                    {
                        foreach (var line in elements)
                        {//suhhee_lab02 - replace to html tags
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
                    //suhhee_lab02
                    toHtml.Add("</div>");

                }

            }

            string toHTMLfile = string.Join("\n", toHtml);
            return toHTMLfile;
        }

        // parse the json file getting all valid arguments
        public static void parseJSON(string jsonstring, string extension, string pout)
        {
            string[] builtString = new string[4]{"","","",""};
            bool valid = false;
            if (extension == ".json")
            ///eugene_lab04
            {   var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonstring);
                
				if(dict.ContainsKey("") || jsonstring == "{}")
                {
                    valid = true; // case with {} json
                    builtString[0] = "-v";
                }else{
                    
                    if (dict.ContainsKey("input"))
                    {
                        builtString[0] = "--input" ;
                        builtString[1] = dict["input"];
                        valid = true;
                    }
                    else if (dict.ContainsKey("i"))
                    {
                        builtString[0] = "-i" ;
                        builtString[1] = dict["i"];
                        valid = true;
                    }
                    else if (dict.ContainsKey("output"))
                    {
                        builtString[2] = "--output ";
                        builtString[3]  = dict["output"];
						pout = dict["output"];
                    }      
                    
                }
                if (valid)
                {
                    if (dict.ContainsKey("output"))
                    {
						strToFile(builtString[1], builtString[3]);
                    }
                    else
                    {
                        strToFile(builtString[1], pout);
                    }
                    
                }
                else
                {
                    Console.WriteLine("Invalid json file contents");
                }     
            }
            
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
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)){
                    doc.Save(txtDir + "/" + fileName + ".html");    
                }else{ //Windows
                    doc.Save(txtDir + "\\" + fileName + ".html");
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void testJSONFirst(string s, string pout)
        {
            //eugene_lab04 - edited to get json file
            string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.GetFullPath(sCurrentDirectory);
            string fileDirectory = Path.GetDirectoryName(filePath);
			string substr = "\\";
            if (Path.GetExtension(s) == ".json")
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    substr = "/";
                }
                var contents = File.ReadAllText(fileDirectory +substr+ s);
                string extension = Path.GetExtension(s);
                parseJSON(contents, extension, pout);
            } else {
                
                strToFile(s, pout);
            }
        }
        public static void strToFile(string s, string output)
        {
			string substr = "\\";
             //\\net5.0\\ removed to work for linux
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                substr = "/";
            }
            //suhhee_lab02 - create HTML files in dist folder in main directory
            string[] html = HTMLstr.Split("\n");
            string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string sFile = sCurrentDirectory;//Path.Combine(sCurrentDirectory, substr); //windows can modify to ur usecase
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				sFile = sCurrentDirectory;
			}
            string textPath = Path.GetFullPath(sFile);

            string txtDirectory = Path.GetDirectoryName(textPath);
			//added safety check
			if(output == "" || output == null){
				output = "dist";
			}
            
            if (Directory.Exists(txtDirectory+ substr + output))
            {
                Directory.Delete(txtDirectory + substr + output, true);         
            }
            Directory.CreateDirectory(txtDirectory + substr + output);
               
            string toHTMLfile = string.Empty;
            if (Path.GetExtension(s) == ".txt")
            {
                var text = File.ReadAllText(txtDirectory + substr + s); //removed "\\net5.0\\"
                string fileName = Path.GetFileNameWithoutExtension(txtDirectory+ substr +s);
                string extension = Path.GetExtension(txtDirectory+ substr + s); 

                string[] contents = text.Split("\n\n");
                toHTMLfile = generateHTMLStr(html, contents, fileName, extension);
                generateHTMLfile(toHTMLfile, txtDirectory + substr + output, fileName);
            }
            //suhhee_lab02 - edited to get md files 
            else if (Path.GetExtension(s) == ".md") 
            {
                
                var contents = File.ReadAllLines(txtDirectory+substr+s); //+ "\\net5.0\\"

                string fileName = Path.GetFileNameWithoutExtension(s);
                string extension = Path.GetExtension(s);
                   
                toHTMLfile = generateHTMLStr(html, contents, fileName, extension);

                generateHTMLfile(toHTMLfile, txtDirectory + substr+output, fileName); //\\net5.0\\
                

            }
			else
			{
				// suhhee_lab02
				//suhhee_lab02 - edited to get md files in folder

				List<string> txtList = new List<string>();
                DirectoryInfo di = new DirectoryInfo(txtDirectory + substr +s);

            
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
                        contents = File.ReadAllText(filePath).Split("\n");
                    }
                    //suhhee_lab02

                    List<string> pathSplit = filePath.Split("\\").ToList();
					if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
					{
						pathSplit = filePath.Split("/").ToList();
					}
                    //get title 
                    string fileName = Path.GetFileNameWithoutExtension(filePath);


                    toHTMLfile = generateHTMLStr(html, contents, fileName, extension);

                    //get saving loation
                    string saveLoc = Path.GetDirectoryName(filePath); /////

                    generateHTMLfile(toHTMLfile, savLoc + substr + output, fileName);
				}
            }
            
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
