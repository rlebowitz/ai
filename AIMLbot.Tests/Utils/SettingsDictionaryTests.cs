using System;
using System.IO;
using System.Xml;
using AIMLbot.Utils;
using NUnit.Framework;

namespace AIMLbot.Tests.Utils
{
    [TestFixture]
    public class SettingsDictionaryTests
    {
        #region Setup/Teardown

        [SetUp]
        public void setupMockDictionary()
        {
            mockDictionary = new SettingsDictionary(_mockChatBot);
            pathToConfigs = Path.Combine(Environment.CurrentDirectory, Path.Combine("config", "Settings.xml"));
        }

        #endregion

        private ChatBot _mockChatBot;
        private SettingsDictionary mockDictionary;
        private string pathToConfigs;

        [OneTimeSetUp]
        public void Setup()
        {
            _mockChatBot = new ChatBot();
        }

        [Test]
        public void testAddSettingWithBadName()
        {
            mockDictionary.AddSetting("", "result");
            Assert.AreEqual("", mockDictionary.GrabSetting(""));
            Assert.AreEqual(false, mockDictionary.ContainsSettingCalled(""));
        }

        [Test]
        public void testAddSettingWithDuplications()
        {
            mockDictionary.AddSetting("test", "value");
            Assert.AreEqual(true, mockDictionary.ContainsSettingCalled("test"));
            Assert.AreEqual("value", mockDictionary.GrabSetting("test"));
            mockDictionary.AddSetting("test", "value2");
            Assert.AreEqual(true, mockDictionary.ContainsSettingCalled("test"));
            Assert.AreEqual("value2", mockDictionary.GrabSetting("test"));
        }

        [Test]
        public void testAddSettingWithEmptyValue()
        {
            mockDictionary.AddSetting("test", "");
            Assert.AreEqual("", mockDictionary.GrabSetting("test"));
            Assert.AreEqual(true, mockDictionary.ContainsSettingCalled("test"));
        }

        [Test]
        public void testAddSettingWithGoodData()
        {
            mockDictionary.AddSetting("test", "result");
            Assert.AreEqual("result", mockDictionary.GrabSetting("test"));
        }


        [Test]
        public void testClearDictionary()
        {
            mockDictionary.loadSettings(pathToConfigs);
            Assert.Greater(mockDictionary.SettingNames.Length, 0);
            mockDictionary.ClearSettings();
            Assert.AreEqual(0, mockDictionary.SettingNames.Length);
        }

        [Test]
        public void testClone()
        {
            mockDictionary.AddSetting("test", "value");
            mockDictionary.AddSetting("test2", "value2");
            mockDictionary.AddSetting("test3", "value3");

            SettingsDictionary newDictionary = new SettingsDictionary(_mockChatBot);
            mockDictionary.Clone(newDictionary);
            Assert.AreEqual(3, newDictionary.SettingNames.Length);
        }

        [Test]
        public void testContainsSettingCalledGoodData()
        {
            mockDictionary.AddSetting("test", "value");
            Assert.AreEqual(true, mockDictionary.ContainsSettingCalled("test"));
            mockDictionary.RemoveSetting("test");
            Assert.AreEqual(false, mockDictionary.ContainsSettingCalled("test"));
        }

        [Test]
        public void testContainsSettingCalledNoData()
        {
            Assert.AreEqual(false, mockDictionary.ContainsSettingCalled(""));
        }

        [Test]
        public void testGrabSettingGoodData()
        {
            mockDictionary.AddSetting("test", "value");
            Assert.AreEqual("value", mockDictionary.GrabSetting("test"));
        }

        [Test]
        public void testGrabSettingMissingData()
        {
            Assert.AreEqual("", mockDictionary.GrabSetting("test"));
        }

        [Test]
        public void testGrabSettingNoData()
        {
            Assert.AreEqual("", mockDictionary.GrabSetting(""));
        }

        [Test]
        public void TestLoadSettingsBadDirectory()
        {
            var path = Path.Combine(Environment.CurrentDirectory, "doesnotexist");
            Assert.That( () => mockDictionary.loadSettings(path), Throws.TypeOf<FileNotFoundException>());
        }

        [Test]
        public void TestLoadSettingsBadFilename()
        {
            var path = Path.Combine(Environment.CurrentDirectory, Path.Combine("config", "doesnotexist.xml"));
            Assert.That(()=> mockDictionary.loadSettings(path), Throws.TypeOf<FileNotFoundException>());
        }

        [Test]
        public void TestLoadSettingsEmptyArgument()
        {
            Assert.That(() => mockDictionary.loadSettings(""), Throws.TypeOf<FileNotFoundException>());
        }

        [Test]
        public void testLoadSettingsGoodPath()
        {
            mockDictionary.loadSettings(pathToConfigs);
            Assert.AreEqual(mockDictionary.ContainsSettingCalled("aimldirectory"), true);
            Assert.AreEqual(mockDictionary.ContainsSettingCalled("feelings"), true);
            Assert.AreEqual(mockDictionary.GrabSetting("aimldirectory"), "aiml");
            Assert.AreEqual(mockDictionary.GrabSetting("feelings"), "I don't have feelings");
        }

        [Test]
        public void testLoadSettingsWithBadXml()
        {
            var path = Path.Combine(Environment.CurrentDirectory, Path.Combine("config", "SettingsBad.xml"));
            Assert.That(() => mockDictionary.loadSettings(path), Throws.TypeOf<XmlException>());
        }

        [Test]
        public void testLoadSettingsWithValidButIncorrectXml()
        {
            mockDictionary.loadSettings(
                Path.Combine(Environment.CurrentDirectory, Path.Combine("config", "SettingsValidIncorrect.xml")));
            Assert.AreEqual(true, mockDictionary.ContainsSettingCalled("test"));
            Assert.AreEqual(false, mockDictionary.ContainsSettingCalled("french"));
            Assert.AreEqual(false, mockDictionary.ContainsSettingCalled("config"));
            Assert.AreEqual(false, mockDictionary.ContainsSettingCalled("aiml"));
            Assert.AreEqual(false, mockDictionary.ContainsSettingCalled(""));
        }

        [Test]
        public void testMethodsAreNameCaseInsensitive()
        {
            mockDictionary.AddSetting("test", "value");
            Assert.AreEqual(true, mockDictionary.ContainsSettingCalled("TEST"));
            Assert.AreEqual("value", mockDictionary.GrabSetting("TEST"));
            mockDictionary.RemoveSetting("TEST");
            Assert.AreEqual(false, mockDictionary.ContainsSettingCalled("test"));
            mockDictionary.AddSetting("TEST", "value");
            Assert.AreEqual(true, mockDictionary.ContainsSettingCalled("test"));
            Assert.AreEqual("value", mockDictionary.GrabSetting("test"));
            mockDictionary.RemoveSetting("test");
            Assert.AreEqual(false, mockDictionary.ContainsSettingCalled("TEST"));
        }

        [Test]
        public void testRemoveSettingWithGoodData()
        {
            mockDictionary.AddSetting("test", "value");
            Assert.AreEqual(true, mockDictionary.ContainsSettingCalled("test"));
            mockDictionary.RemoveSetting("test");
            Assert.AreEqual(false, mockDictionary.ContainsSettingCalled("test"));
        }

        [Test]
        public void testRemoveSettingWithMissingData()
        {
            mockDictionary.AddSetting("test", "value");
            Assert.AreEqual(true, mockDictionary.ContainsSettingCalled("test"));
            mockDictionary.RemoveSetting("test");
            Assert.AreEqual(false, mockDictionary.ContainsSettingCalled("test"));
            mockDictionary.RemoveSetting("test");
            Assert.AreEqual(false, mockDictionary.ContainsSettingCalled("test"));
        }

        [Test]
        public void testRemoveSettingWithNoData()
        {
            mockDictionary.AddSetting("test", "value");
            Assert.AreEqual(true, mockDictionary.ContainsSettingCalled("test"));
            mockDictionary.RemoveSetting("");
            Assert.AreEqual(true, mockDictionary.ContainsSettingCalled("test"));
        }

        [Test]
        public void testSettingNames()
        {
            mockDictionary.AddSetting("test", "value");
            mockDictionary.AddSetting("test2", "value2");
            mockDictionary.AddSetting("test3", "value3");
            Assert.AreEqual(3, mockDictionary.SettingNames.Length);
        }

        [Test]
        public void testXMLGeneration()
        {
            mockDictionary.AddSetting("test", "value");
            mockDictionary.AddSetting("test2", "value2");
            mockDictionary.AddSetting("test3", "value3");

            Assert.AreEqual(
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?><root><item name=\"TEST\" value=\"value\" /><item name=\"TEST2\" value=\"value2\" /><item name=\"TEST3\" value=\"value3\" /></root>",
                mockDictionary.DictionaryAsXML.OuterXml);
        }
    }
}