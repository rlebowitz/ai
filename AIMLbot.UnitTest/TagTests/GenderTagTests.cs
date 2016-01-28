using AIMLbot.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gender = AIMLbot.AIMLTagHandlers.Gender;

namespace AIMLbot.UnitTest.TagTests
{
    [TestClass]
    public class GenderTagTests
    {
        private Gender _genderTagHandler;
        private SubQuery _query;
        private Request _request;
        private User _user;

        [TestInitialize]
        public void Setup()
        {
            _user = new User();
            _request = new Request("This is a test", _user);
            _query = new SubQuery();
        }

        [TestMethod]
        public void TestAtomic()
        {
            var testNode = StaticHelpers.GetNode("<gender/>");
            _genderTagHandler = new Gender(_query, _request, testNode);
            _query.InputStar.Insert(0, " HE SHE TO HIM TO HER HIS HER HIM ");
            Assert.AreEqual(" she he to her to him her his her ", _genderTagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestEmptyInput()
        {
            var testNode = StaticHelpers.GetNode("<Gender/>");
            _genderTagHandler = new Gender(_query, _request, testNode);
            _query.InputStar.Clear();
            Assert.AreEqual("", _genderTagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestNoMatches()
        {
            var testNode = StaticHelpers.GetNode("<gender>THE QUICK BROWN FOX JUMPED OVER THE LAZY DOGS</gender>");
            _genderTagHandler = new Gender(_query, _request, testNode);
            Assert.AreEqual("THE QUICK BROWN FOX JUMPED OVER THE LAZY DOGS", _genderTagHandler.ProcessChange());
        }

        [TestMethod]
        public void TestNonAtomic()
        {
            var testNode =
                StaticHelpers.GetNode(
                    "<gender> HE SHE TO HIM FOR HIM WITH HIM ON HIM IN HIM TO HER FOR HER WITH HER ON HER IN HER HIS HER HIM </gender>");
            _genderTagHandler = new Gender(_query, _request, testNode);
            Assert.AreEqual(
                " she he to her for her with her on her in her to him for him with him on him in him her his her ",
                _genderTagHandler.ProcessChange());
        }
    }
}