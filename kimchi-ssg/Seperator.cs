// -----------------------------------------------------------------------
// <copyright file="Seperator.cs" company="Minsu Kim">
// Copyright (c) Minsu Kim. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kimchi_ssg
{
    internal class Seperator
    {
        Helpers helper = new Helpers(null);

        public string PathSeperator
        {
            get { return helper.IsLinux() ? FrontSlash : BackSlash; }
        }

        public string NewLineSeperator
        {
            get { return helper.IsLinux() ? LinuxNewLine : WindowNewLine; }
        }

        public string NewLineDoubleSeperator
        {
            get { return helper.IsLinux() ? LinuxDoubleNewLine : WindowDoubleNewLine; }
        }

        public static string LinuxNewLine
        {
            get { return "\n"; }
        }

        public static string WindowNewLine
        {
            get { return "\r\n"; }
        }

        public static string LinuxDoubleNewLine
        {
            get { return "\n\n"; }
        }

        public static string WindowDoubleNewLine
        {
            get { return "\r\n\r\n"; }
        }

        public static string BackSlash
        {
            get { return "\\"; }
        }

        public static string FrontSlash
        {
            get { return "/"; }
        }
    }
}
