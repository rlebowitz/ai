using System;
using System.Text;
using System.Collections.Generic;
using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AIMLbot.UnitTest.Utils
{
    /// <summary>
    /// http://individed.com/code/to-title-case/tests.html
    /// </summary>
    [TestClass]
    public class TitleCaseTests
    {
        public TitleCase Extension { get; }

        public TitleCaseTests()
        {
            Extension = new TitleCase();
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }
        
        [TestMethod]
        public void EmailTest()
        {
            const string email = "For Step-by-Step Directions Email someone@gmail.com";
            Assert.AreEqual("For Step-by-Step Directions Email someone@gmail.com", Extension.ToTitleCase(email));
        }

        [TestMethod]
        public void ParenthesisTest()
        {
            const string email = "2lmc Spool: 'Gruber on OmniFocus and Vapo(u)rware'";
            Assert.AreEqual("2lmc Spool: 'Gruber on OmniFocus and Vapo(u)rware'", Extension.ToTitleCase(email));
        }

        [TestMethod]
        public void QuotesTest()
        {
            const string email = "Have You Read \"The Lottery\"?";
            Assert.AreEqual("Have You Read \"The Lottery\"?", Extension.ToTitleCase(email));
        }

        [TestMethod]
        public void SquareBracesTest()
        {
            const string email = "Your Hair[cut] Looks (Nice)";
            Assert.AreEqual("Your Hair[cut] Looks (Nice)", Extension.ToTitleCase(email));
        }

        [TestMethod]
        public void HttpTest()
        {
            const string email = "People Probably Won't Put http://foo.com/bar/ in Titles";
            Assert.AreEqual("People Probably Won't Put http://foo.com/bar/ in Titles", Extension.ToTitleCase(email));
        }

        [TestMethod]
        public void LaLaTest()
        {
            const string email = "Scott Moritz and TheStreet.com’s Million iPhone La‑La Land";
            Assert.AreEqual("Scott Moritz and TheStreet.com’s Million iPhone La‑La Land", Extension.ToTitleCase(email));
        }

        [TestMethod]
        public void PhoneTest()
        {
            const string email = "BlackBerry vs. iPhone";
            Assert.AreEqual("BlackBerry vs. iPhone", Extension.ToTitleCase(email));
        }

        [TestMethod]
        public void NotesTest()
        {
            const string email = "Notes and Observations Regarding Apple's Announcements From 'The Beat Goes On' Special Event";
            Assert.AreEqual("Notes and Observations Regarding Apple's Announcements From 'The Beat Goes On' Special Event", Extension.ToTitleCase(email));
        }

        [TestMethod]
        public void VSTest()
        {
            const string email = "this vs. that";
            Assert.AreEqual("This vs. That", Extension.ToTitleCase(email));
        }
    }
}
