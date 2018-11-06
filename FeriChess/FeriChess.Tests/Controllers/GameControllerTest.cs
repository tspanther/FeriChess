using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FeriChess;
using FeriChess.Controllers;
using Ninject;

namespace FeriChess.Tests.Controllers
{
    [TestClass]
    public class GameControllerTest
    {
        private IKernel kernel = new StandardKernel(new App_Start.NinjectWebCommon.TestModule());

        [TestMethod]
        public void GetAvailableMovesResponseIsNotNull()
        {
            var controller = kernel.Get<GameController>();

            var resp = controller.GetAvailableMoves(new Models.Field(1, 2));

            Assert.IsNotNull(resp);
        }
    }
}
