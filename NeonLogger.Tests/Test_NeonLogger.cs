using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NeonLogger.Tests
{
    public class Test_NeonLogger
    {
        [Fact]
        public void Test_Log_WriteToFile()
        {
            var logger = new NeonLogger("log.txt");

            logger.Log("apple");
            logger.Log("banana");

            var lines = File.ReadAllLines("log.txt").ToArray();
            Assert.True(lines[0] == "apple");
            Assert.True(lines[1] == "banana");
        }

        [Fact]
        public void Test_LogDeferred_NotWriteUntilFlush()
        {
            var logger = new NeonLogger("log.txt");

            logger.LogDeferred("apple");
            logger.LogDeferred("banana");

            Assert.False(File.Exists("log.txt"));
        }

        [Fact]
        public void Test_LogDeferred_WriteToFile()
        {
            var logger = new NeonLogger("log.txt");

            logger.LogDeferred("apple");
            logger.LogDeferred("banana");
            logger.Flush();

            var lines = File.ReadAllLines("log.txt").ToArray();
            Assert.True(lines[0] == "apple");
            Assert.True(lines[1] == "banana");
        }

        [Fact]
        public void Test_LogAndLogDeferred_WriteToFile()
        {
            var logger = new NeonLogger("log.txt");

            logger.LogDeferred("apple");
            logger.LogDeferred("banana");
            logger.Log("case");
            logger.Log("deer");
            logger.Flush();

            var lines = File.ReadAllLines("log.txt").ToArray();
            Assert.True(lines[0] == "case");
            Assert.True(lines[1] == "deer");
            Assert.True(lines[2] == "apple");
            Assert.True(lines[3] == "banana");
        }

        [Fact]
        public void Test_PopularMessages_DifferentTimesLogged()
        {
            var logger = new NeonLogger("log.txt");

            logger.Log("apple");

            logger.Log("banana");
            logger.Log("banana");

            var popular = logger.PopularMessages();
            Assert.True(popular.Contains("apple"));
            Assert.True(popular.Contains("banana"));
            Assert.False(popular.Contains("pek"));
        }

        [Fact]
        public void Test_PopularMessages_TenLoggedTwiceAndTwoLoggedOnce()
        {
            var logger = new NeonLogger("log.txt");

            logger.Log("apple");
            logger.Log("apple");
            logger.Log("banana");
            logger.Log("banana");
            logger.Log("case");
            logger.Log("case");
            logger.Log("deer");
            logger.Log("deer");
            logger.Log("eagle");
            logger.Log("eagle");
            logger.Log("fever");
            logger.Log("fever");
            logger.Log("gem");
            logger.Log("gem");
            logger.Log("hat");
            logger.Log("hat");
            logger.Log("idol");
            logger.Log("idol");
            logger.Log("jeep");
            logger.Log("jeep");
            logger.Log("kernel");
            logger.Log("lance");

            var popular = logger.PopularMessages();
            Assert.True(popular.Contains("apple"));
            Assert.True(popular.Contains("banana"));
            Assert.True(popular.Contains("case"));
            Assert.True(popular.Contains("deer"));
            Assert.True(popular.Contains("eagle"));
            Assert.True(popular.Contains("fever"));
            Assert.True(popular.Contains("gem"));
            Assert.True(popular.Contains("hat"));
            Assert.True(popular.Contains("idol"));
            Assert.True(popular.Contains("jeep"));
            Assert.False(popular.Contains("kernel"));
            Assert.False(popular.Contains("lance"));
        }

        [Fact]
        public void Test_PopularMessages_DifferentTimesLoggedDeferred()
        {
            var logger = new NeonLogger("log.txt");

            logger.LogDeferred("apple");

            logger.LogDeferred("banana");
            logger.LogDeferred("banana");
            logger.Flush();

            var popular = logger.PopularMessages();
            Assert.True(popular.Contains("apple"));
            Assert.True(popular.Contains("banana"));
            Assert.False(popular.Contains("pek"));
        }

        [Fact]
        public void Test_PopularMessages_TenLoggedTwiceAndTwoLoggedOnceDefered()
        {
            var logger = new NeonLogger("log.txt");

            logger.LogDeferred("apple");
            logger.LogDeferred("apple");
            logger.LogDeferred("banana");
            logger.LogDeferred("banana");
            logger.LogDeferred("case");
            logger.LogDeferred("case");
            logger.LogDeferred("deer");
            logger.LogDeferred("deer");
            logger.LogDeferred("eagle");
            logger.LogDeferred("eagle");
            logger.LogDeferred("fever");
            logger.LogDeferred("fever");
            logger.LogDeferred("gem");
            logger.LogDeferred("gem");
            logger.LogDeferred("hat");
            logger.LogDeferred("hat");
            logger.LogDeferred("idol");
            logger.LogDeferred("idol");
            logger.LogDeferred("jeep");
            logger.LogDeferred("jeep");
            logger.LogDeferred("kernel");
            logger.LogDeferred("lance");
            logger.Flush();

            var popular = logger.PopularMessages();
            Assert.True(popular.Contains("apple"));
            Assert.True(popular.Contains("banana"));
            Assert.True(popular.Contains("case"));
            Assert.True(popular.Contains("deer"));
            Assert.True(popular.Contains("eagle"));
            Assert.True(popular.Contains("fever"));
            Assert.True(popular.Contains("gem"));
            Assert.True(popular.Contains("hat"));
            Assert.True(popular.Contains("idol"));
            Assert.True(popular.Contains("jeep"));
            Assert.False(popular.Contains("kernel"));
            Assert.False(popular.Contains("lance"));
        }

        [Fact]
        public void Test_LogAndLogDeferred_ParallelWriting()
        {
            var logger = new NeonLogger("log.txt");

            var messages = new[] {"apple", "banana", "case", "deer", "eagle"};
            Parallel.For(0, 100, x =>
            {
                var message = messages[x % messages.Length];
                logger.Log(message);
                logger.LogDeferred(message);
            });
            logger.Flush();
            
            Assert.True(File.ReadAllLines("log.txt").Length == 200);
        }

        public Test_NeonLogger()
        {
            if (File.Exists("log.txt"))
            {
                File.Delete("log.txt");
            }
        }
    }
}