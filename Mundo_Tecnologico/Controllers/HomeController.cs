using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mundo_Tecnologico.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string correo)
        {
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Nosotros()
        {
            return View();
        }
    }
}