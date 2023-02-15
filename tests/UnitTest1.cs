using ArgParser;

namespace tests
{
    public class UnitTest1
    {
        ArgCollection collection = new ArgCollection(new string[] { "-a", "b", "--c", "d", "--e", "1" });
        [Fact]
        public void Test1()
        {
            Assert.True(collection.HasOption('a'));
        }

        [Fact]
        public void Test2()
        {
            Assert.True(collection.HasArgument("c"));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal("d", collection.GetArgument("c"));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal(1, collection.ParseArgument<int>("e"));
        }

        [Fact]
        public void Test5()
        {
            Assert.Equal(3, collection.Count);
        }

        [Fact]
        public void Test6()
        {
            Assert.Null(() => collection.GetArgument("a"));
        }

        [Fact]
        public void Test7()
        {
            Assert.Throws<ArgumentNullException>(() => new ArgCollection(null));
        }
    }
}