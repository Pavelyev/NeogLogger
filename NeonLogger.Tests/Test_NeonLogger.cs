using System.IO;
using System.Linq;
using NUnit.Framework;

namespace NeonLogger.Tests
{
    [TestFixture]
    public class Test_NeonLogger
    {
        [Test]
        public void Test_Log_WriteToFile()
        {
            var logger = new NeonLogger("log.txt");

            logger.Log("apple");
            logger.Log("banana");

            var lines = File.ReadAllLines("log.txt").ToArray();
            Assert.IsTrue(lines[0] == "apple");
            Assert.IsTrue(lines[1] == "banana");
        }

        [Test]
        public void Test_LogDeferred_NotWriteUntilFlush()
        {
            var logger = new NeonLogger("log.txt");

            logger.LogDeferred("apple");
            logger.LogDeferred("banana");

            Assert.IsFalse(File.Exists("log.txt"));
        }

        [Test]
        public void Test_LogDeferred_WriteToFile()
        {
            var logger = new NeonLogger("log.txt");

            logger.LogDeferred("apple");
            logger.LogDeferred("banana");
            logger.Flush();

            var lines = File.ReadAllLines("log.txt").ToArray();
            Assert.IsTrue(lines[0] == "apple");
            Assert.IsTrue(lines[1] == "banana");
        }

        [Test]
        public void Test_LogAndLogDeferred_WriteToFile()
        {
            var logger = new NeonLogger("log.txt");

            logger.LogDeferred("apple");
            logger.LogDeferred("banana");
            logger.Log("case");
            logger.Log("deer");
            logger.Flush();

            var lines = File.ReadAllLines("log.txt").ToArray();
            Assert.IsTrue(lines[0] == "case");
            Assert.IsTrue(lines[1] == "deer");
            Assert.IsTrue(lines[2] == "apple");
            Assert.IsTrue(lines[3] == "banana");
        }

        [Test]
        public void Test_PopularMessages_DifferentTimesLogged()
        {
            var logger = new NeonLogger("log.txt");

            logger.Log("apple");

            logger.Log("banana");
            logger.Log("banana");

            var popular = logger.PopularMessages();
            Assert.IsTrue(popular.Contains("apple"));
            Assert.IsTrue(popular.Contains("banana"));
            Assert.IsFalse(popular.Contains("pek"));
        }

        [Test]
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
            Assert.IsTrue(popular.Contains("apple"));
            Assert.IsTrue(popular.Contains("banana"));
            Assert.IsTrue(popular.Contains("case"));
            Assert.IsTrue(popular.Contains("deer"));
            Assert.IsTrue(popular.Contains("eagle"));
            Assert.IsTrue(popular.Contains("fever"));
            Assert.IsTrue(popular.Contains("gem"));
            Assert.IsTrue(popular.Contains("hat"));
            Assert.IsTrue(popular.Contains("idol"));
            Assert.IsTrue(popular.Contains("jeep"));
            Assert.IsFalse(popular.Contains("kernel"));
            Assert.IsFalse(popular.Contains("lance"));
        }

        [Test]
        public void Test_PopularMessages_DifferentTimesLoggedDeferred()
        {
            var logger = new NeonLogger("log.txt");

            logger.LogDeferred("apple");

            logger.LogDeferred("banana");
            logger.LogDeferred("banana");
            logger.Flush();

            var popular = logger.PopularMessages();
            Assert.IsTrue(popular.Contains("apple"));
            Assert.IsTrue(popular.Contains("banana"));
            Assert.IsFalse(popular.Contains("pek"));
        }

        [Test]
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
            Assert.IsTrue(popular.Contains("apple"));
            Assert.IsTrue(popular.Contains("banana"));
            Assert.IsTrue(popular.Contains("case"));
            Assert.IsTrue(popular.Contains("deer"));
            Assert.IsTrue(popular.Contains("eagle"));
            Assert.IsTrue(popular.Contains("fever"));
            Assert.IsTrue(popular.Contains("gem"));
            Assert.IsTrue(popular.Contains("hat"));
            Assert.IsTrue(popular.Contains("idol"));
            Assert.IsTrue(popular.Contains("jeep"));
            Assert.IsFalse(popular.Contains("kernel"));
            Assert.IsFalse(popular.Contains("lance"));
        }

        [SetUp]
        public void Setup()
        {
            if (File.Exists("log.txt"))
            {
                File.Delete("log.txt");
            }
        }
    }
}