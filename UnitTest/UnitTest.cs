namespace UnitTest
{
    using Kimchi_ssg;
    using Xunit;
    using Moq;
    using System.Collections.Generic;
    using System;

    public class UnitTest
    {
        [Fact]
        public void GetOptionsTest()
        {
            var mock = new Mock<IWrapper>();

            // expected
            mock.Setup(x => x.WrapGetOptions()).Returns(It.IsAny<string>());

            // actual
            IWrapper obj = mock.Object;
            string result = obj.WrapGetOptions();

            Assert.Equal(It.IsAny<string>(), result);
        }

        [Fact]
        public void GetVersionTest()
        {
            var mock = new Mock<IWrapper>();
            // expected
            mock.Setup(x => x.WrapGetVersion()).Returns(It.IsAny<string>());

            // actual
            IWrapper obj = mock.Object;
            string result = obj.WrapGetVersion();

            Assert.Equal(It.IsAny<string>(), result);
        }

        [Fact]
        public void GenerateTableOfContentsTest()
        {
            string[] input = { "test1", "test2" };
            List<string> files = new(input);
            var mock = new Mock<IWrapper>();

            // expected
            mock.Setup(x => x.WrapGenerateTableOfContents(files));

            // actual
            IWrapper obj = mock.Object;
            var result = obj.WrapGenerateTableOfContents(files);

            mock.VerifyAll();
        }

        [Fact]
        public void IsLinuxTest()
        {
            var mock = new Mock<IWrapper>();

            // expected
            mock.Setup(x => x.WrapIsLinux()).Returns(false);

            // actual
            IWrapper obj = mock.Object;
            bool result = obj.WrapIsLinux();

            mock.VerifyAll();
            Assert.False(result, "Your Machine is Windows");
        }

        [Fact]
        public void GenerateHTMLStrWithTextNullTest()
        {
            var title = "test";
            var extension = ".txt";
            var table = "this is table";
            var style = "this is style";
            var meta = "this is meta";

            var mock = new Mock<IWrapper>();
            // expected 

            mock.Setup(x => x.WrapGenerateHTMLStr(title, extension, table, style, meta, null)).Returns(It.IsAny<string>());

            // actual 
            IWrapper obj = mock.Object;
            string result = obj.WrapGenerateHTMLStr(title, extension, table, style, meta, null);
            Assert.DoesNotContain("<p>", result);
        }

        [Fact]
        public void GenerateHTMLStrWithTextElementTest()
        {
            var title = "test";
            var extension = ".txt";
            var table = "this is table";
            var style = "this is style";
            var meta = "this is meta";
            string[] elements = { "this", "is", "content" };

            var mock = new Mock<IWrapper>();
            // expected 
            mock.Setup(x => x.WrapGenerateHTMLStr(title, extension, table, style, meta, elements));

            // actual 
            var result = new WrapperStatic(mock.Object).WrapGenerateHTMLStr(title, extension, table, style, meta, elements);

            Assert.Contains("<p>", result);
        }

        [Fact]
        public void GenerateHTMLStrWithEmptyArrayTest()
        {
            var title = "test";
            var extension = ".txt";
            var table = "this is table";
            var style = "this is style";
            var meta = "this is meta";
            string[] elements = { };

            var mock = new Mock<IWrapper>();
            // expected 
            mock.Setup(x => x.WrapGenerateHTMLStr(title, extension, table, style, meta, elements)).Returns("The file cannot have empty content");

            // actual 
            Action act = () => new WrapperStatic(mock.Object).WrapGenerateHTMLStr(title, extension, table, style, meta, elements);
            Exception exception = Assert.Throws<Exception>(act);

            Assert.Equal("The file cannot have empty content", exception.Message);
        }

        [Fact]
        public void GenerateHTMLStrWithMdTest()
        {
            var title = "test";
            var extension = ".md";
            var table = "this is table";
            var style = "this is style";
            var meta = "this is meta";
            string[] elements = { "# kimchi-ssg" };

            var mock = new Mock<IWrapper>();
            // expected 
            mock.Setup(x => x.WrapGenerateHTMLStr(title, extension, table, style, meta, elements));

            // actual 
            var result = new WrapperStatic(mock.Object).WrapGenerateHTMLStr(title, extension, table, style, meta, elements);

            Assert.Contains("<h1>kimchi-ssg</h1>", result);
        }

        [Fact]
        public void dumppy()
        {
            // dummy
        }

        [Fact]
        public void shouldThrowExceptionWhenTitleIsEmpty()
        {
            var title = "";
            var extension = ".md";
            var table = "this is table";
            var style = "this is style";
            var meta = "this is meta";
            string[] elements = { "# kimchi-ssg", "test2" };

            var mock = new Mock<IWrapper>();
            // expected 
            mock.Setup(x => x.WrapGenerateHTMLStr(title, extension, table, style, meta, elements));

            // actual 
            Action act = () => new WrapperStatic(mock.Object).WrapGenerateHTMLStr(title, extension, table, style, meta, elements);
            Exception exception = Assert.Throws<Exception>(act);

            Assert.Equal("The file cannot have empty title", exception.Message);
        }


    }
}
