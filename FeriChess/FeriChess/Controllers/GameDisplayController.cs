using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FeriChess.Controllers
{
    public class GameDisplayController : Controller
    {
        // GET: GameDisplay
        public ActionResult Index()
        {
            return View();
        }
    }
}