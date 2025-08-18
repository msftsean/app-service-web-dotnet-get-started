using NUnit.Framework;
using aspnet_get_started.Controllers;
using System.Web.Mvc;

namespace aspnet_get_started.Tests
{
    [TestFixture]
    public class HomeControllerTests
    {
        [Test]
        public void Index_Returns_View()
        {
            var controller = new HomeController();
            var result = controller.Index() as ViewResult;
            Assert.IsNotNull(result);
        }

        [Test]
        public void About_Sets_Message_And_Returns_View()
        {
            var controller = new HomeController();
            var result = controller.About() as ViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("Your application description page.", controller.ViewBag.Message);
        }

        [Test]
        public void Contact_Sets_Message_And_Returns_View()
        {
            var controller = new HomeController();
            var result = controller.Contact() as ViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("Your contact page.", controller.ViewBag.Message);
        }
    }
}
