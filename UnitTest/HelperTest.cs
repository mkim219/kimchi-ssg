
namespace UnitTest
{
    using Kimchi_ssg;
    using Xunit;
    using Moq;
    using System.Collections.Generic;
    using System;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text.Json;
    using System.Text.RegularExpressions;
    using HtmlAgilityPack;


    public class HelperTest
    {
        [Fact]
        public void shouldReturnTableOfContentString() {
            // GIVEN
            List<string> strs = new List<string>{"test1"};

            // WHEN
            string result = Helpers.GenerateTableOfContents(strs);

            // THEN
            Assert.NotEmpty(result);
        }

        [Fact]
        public void shouldReturnOptions()
        {
            // GIVEN
            string version = "Current Version: 1.0.0";

            // WHEN
            string result = Helpers.GetVersion();

            // THEN
            Assert.Equal(result, version);
        }

        [Fact]
        public void shouldReturnAVersion()
        {
            // GIVEN
            string version = "Current Version: 1.0.0";

            // WHEN
            string result = Helpers.GetVersion();

            // THEN
            Assert.Equal(result, version);
        }
    }
}
