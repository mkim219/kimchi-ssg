namespace Kimchi_ssg
{
    internal class Seperator
    {
        public static string pathSeperator { get { return Helpers.IsLinux() ? frontSlash : backSlash; } }

        public static string newLineSeperator { get { return Helpers.IsLinux() ? linuxNewLine : windowNewLine; } }

        public static string newLineDoubleSeperator { get { return Helpers.IsLinux() ? linuxDoubleNewLine : windowDoubleNewLine; } }

        public static string linuxNewLine { get { return "\n"; } }

        public static string windowNewLine { get { return "\r\n"; } }

        public static string linuxDoubleNewLine { get { return "\n\n"; } }

        public static string windowDoubleNewLine { get { return "\r\n\r\n"; } }

        public static string backSlash { get { return "\\"; } }

        public static string frontSlash { get { return "/"; } }
    }
}
