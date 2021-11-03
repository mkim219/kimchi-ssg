// <copyright file="Seperator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Kimchi_ssg
{
    internal class Seperator
    {
        public static string PathSeperator
        {
            get { return Helpers.IsLinux() ? FrontSlash : BackSlash; }
        }

        public static string NewLineSeperator
        {
            get { return Helpers.IsLinux() ? LinuxNewLine : WindowNewLine; }
        }

        public static string NewLineDoubleSeperator
        {
            get { return Helpers.IsLinux() ? LinuxDoubleNewLine : WindowDoubleNewLine; }
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
