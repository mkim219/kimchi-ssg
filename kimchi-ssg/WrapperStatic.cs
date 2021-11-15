namespace kimchi_ssg
{
    using System.Collections.Generic;

    public class WrapperStatic: Kimchi_ssg.IWrapper
    {
        private readonly Kimchi_ssg.IWrapper wrapper;

        public WrapperStatic(Kimchi_ssg.IWrapper wrap)
        {
            wrapper = wrap;
        }

        public string WrapGenerateHTMLStr(string title, string extension, string table, string style, string meta, string[] elements = null)
        {
            return Kimchi_ssg.Helpers.GenerateHTMLStr(title, extension, table, style, meta, elements);
        }

        public bool WrapIsLinux()
        {
            return Kimchi_ssg.Helpers.IsLinux();
        }

        public string WrapGetVersion()
        {
            return Kimchi_ssg.Helpers.GetVersion();
        }

        public string WrapGetOptions()
        {
            return Kimchi_ssg.Helpers.GetOptions();
        }

        public string WrapGenerateTableOfContents(List<string> files)
        {
            return Kimchi_ssg.Helpers.GenerateTableOfContents(files);
        }

        public string WrapGenerateMeta(string title)
        {
            return Kimchi_ssg.Helpers.GenerateMeta(title);
        }
    }
}
