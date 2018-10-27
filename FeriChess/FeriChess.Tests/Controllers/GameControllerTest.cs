using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FeriChess;
using FeriChess.Controllers;

namespace FeriChess.Tests.Controllers
{
    [TestClass]
    public class GameControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            GameController controller = new GameController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
