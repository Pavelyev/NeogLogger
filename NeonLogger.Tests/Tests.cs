using System;
using System.Linq;
using NUnit.Framework;

namespace NeonLogger.Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Test1()
        {
            var logger = new NeonLogger("log.txt");
            logger.Log("hey");
            logger.Log("hey");
            logger.Log("hey");
            logger.Log("hey");
            logger.Log("go");
            logger.Log("go");
            logger.Log("go");
            logger.Log("go");
            var popular = logger.PopularMessages();
            Assert.IsTrue(popular.Contains("hey"));
            Assert.IsTrue(popular.Contains("go"));
            Assert.IsFalse(popular.Contains("pek"));
        }
    }
}