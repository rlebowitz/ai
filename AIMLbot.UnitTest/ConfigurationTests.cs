using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest
{
    [TestClass]
    public class ConfigurationTests
    {

        [TestInitialize]
        public void Initialize()
        {
        }

        [TestMethod]
        public void TestPerson()
        {
        IDictionary<String, string> config = ConfigurationManager.GetSection("Person") as IDictionary<String, String>;
            Assert.IsNotNull(config);
        }

        [TestMethod]
        public void TestSplitters()
        {
            var config = ChatBot.Splitters;
            Assert.IsNotNull(config);
            Assert.IsTrue(config.Count == 4);
        }


    }
}