using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FeriChess;
using FeriChess.Controllers;
using Ninject;
using FeriChess.Models;
using System.Collections.Generic;
using System.Linq;

namespace FeriChess.Tests.Controllers
{
    [TestClass]
    public class GameControllerTest
    {
        private IKernel kernel = new StandardKernel(new App_Start.NinjectWebCommon.TestModule());

        [TestMethod]
        public void GetAvailableMovesResponseIsListOfField()
        {
            var controller = kernel.Get<GameController>();

            var resp = controller.GetAvailableMoves(new Field(1, 2));

            Assert.IsNotNull(resp);
            Assert.IsTrue(resp.GetType().IsGenericType);
            Assert.AreEqual(resp.GetType().GetGenericTypeDefinition(), typeof(List<>));
            Assert.AreEqual(resp.GetType().GetGenericArguments().Single(), typeof(Field));
        }

        [TestMethod]
        public void MakeAMoveResponseIsGamestateChangeObject()
        {
            var controller = kernel.Get<GameController>();

            var resp = controller.MakeAMove(new Move(new Field(1, 2), new Field(1, 2)));

            Assert.IsNotNull(resp);
            Assert.IsInstanceOfType(resp, typeof(GamestateChange));
        }

        [TestMethod]
        public void LoadBoardStateResponseIsListOfFieldUpdates()
        {
            var controller = kernel.Get<GameController>();

            var resp = controller.LoadBoardstate();

            Assert.IsNotNull(resp);
            Assert.IsTrue(resp.GetType().IsGenericType);
            Assert.AreEqual(resp.GetType().GetGenericTypeDefinition(), typeof(List<>));
            Assert.AreEqual(resp.GetType().GetGenericArguments().Single(), typeof(FieldUpdate));
        }
    }
}
