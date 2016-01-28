using System;
using System.Globalization;
using AIMLbot.AIMLTagHandlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class DateTagTests
    {
        private Date _dateTagHandler;

        [TestInitialize]
        public void Setup()
        {
        }

        [TestMethod]
        public void TestBadInput()
        {
            var testNode = StaticHelpers.GetNode("<dote/>");
            _dateTagHandler = new Date(testNode);
            Assert.AreEqual("", _dateTagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestExpectedInput()
        {
            var testNode = StaticHelpers.GetNode("<date/>");
            _dateTagHandler = new Date(testNode);
            var now = DateTime.Now;
            var expected = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            Assert.AreEqual(expected.ToString(CultureInfo.CurrentCulture), _dateTagHandler.ProcessChange());
        }
    }
}