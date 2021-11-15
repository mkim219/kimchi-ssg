// -----------------------------------------------------------------------
// <copyright file="IWrapper.cs" company="Minsu Kim">
// Copyright (c) Minsu Kim. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kimchi_ssg
{
    public interface IWrapper
    {
        string WrapGenerateHTMLStr(string title, string extension, string table, string style, string meta, string[] elements = null);

        bool WrapIsLinux();

        string WrapGenerateTableOfContents(System.Collections.Generic.List<string> file);

        string WrapGetOptions();

        string WrapGetVersion();
    }
}