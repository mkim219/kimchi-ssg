namespace kimchi_ssg
{
    class Seperator
    {
        public static string pathSeperator { get { return Helpers.isLinux() ? frontSlash : backSlash; } }
        public static string newLineSeperator { get { return Helpers.isLinux() ? linuxNewLine : windowNewLine; } }
        public static string newLineDoubleSeperator { get { return Helpers.isLinux() ? linuxDoubleNewLine : windowDoubleNewLine; } }
        public static string linuxNewLine { get { return "\n"; } }
        public static string windowNewLine { get { return "\r\n"; } }
        public static string linuxDoubleNewLine { get { return "\n\n"; } }
        public static string windowDoubleNewLine { get { return "\r\n\r\n"; } }
        public static string backSlash { get { return "\\"; } }
        public static string frontSlash { get { return "/"; } }
    }
}
