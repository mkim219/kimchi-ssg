using Kimchi_ssg;
using System;
using Xunit;
using Moq;

namespace UnitTest
{
    public class UnitTest
    {
        [Fact]
        public void GetOptionsTest()
        {
            var mock = new Mock<IWrapper>();
            mock.Setup(x => x.GetOptions()).Returns(It.IsAny<string>());
            IWrapper obj = mock.Object;
            string x = obj.GetOptions();

            Assert.Equal(It.IsAny<string>(), x);
        }

        [Fact]
        public void GetVersionTest()
        {
            var mock = new Mock<IWrapper>();
            mock.Setup(x => x.GetVersion()).Returns(It.IsAny<string>());
            IWrapper obj = mock.Object;
            string x = obj.GetVersion();

            Assert.Equal(It.IsAny<string>(), x);
        }
    }
}
