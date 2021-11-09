// -----------------------------------------------------------------------
// <copyright file="helpers.cs" company="Minsu Kim">
// Copyright (c) Minsu Kim. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kimchi_ssg
{
    public interface IWrapper
    {
        string GenerateHTMLStr(string title, string extension, string table, string style, string meta, string[] elements = null);

        void ParseJSON(string file, string output);

        void GenerateHTMLfile(string html, string outputDir, string fileName);

        void ConvertStrToFile(string file, string outputFolder, string style);

        bool IsLinux();

        string GenerateInterporatedstring(string title, string style, string body, string meta);

        string GenerateMeta(string title);

        string GenerateTableOfContents(System.Collections.Generic.List<string> file);

        string GetOptions();

        string GetVersion();
    }
}