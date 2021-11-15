namespace Kimchi_ssg
{
    using System.Collections.Generic;

    public class WrapperStatic : IWrapper
    {
        private readonly IWrapper wrapper;

        public WrapperStatic(IWrapper wrap)
        {
            wrapper = wrap;
        }

        public string WrapGenerateHTMLStr(string title, string extension, string table, string style, string meta, string[] elements = null)
        {
            return Helpers.GenerateHTMLStr(title, extension, table, style, meta, elements);
        }

        public bool WrapIsLinux()
        {
            return Helpers.IsLinux();
        }

        public string WrapGetVersion()
        {
            return Helpers.GetVersion();
        }

        public string WrapGetOptions()
        {
            return Helpers.GetOptions();
        }

        public string WrapGenerateTableOfContents(List<string> files)
        {
            return Helpers.GenerateTableOfContents(files);
        }

        public string WrapGenerateMeta(string title)
        {
            return Helpers.GenerateMeta(title);
        }
    }
}
