using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutomatedCR.DbFile;

namespace AutomatedCR.Controllers
{
    public class LoginController : Controller
    {
        private AutomatedCrConnection db = new AutomatedCrConnection();

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return RedirectToRoute(new
            {
                controller = "Home",
                action = "Index"
            });
        }
        public ActionResult Logout()
        {
            return RedirectToRoute(new
            {
                controller = "Login",
                action = "Index"
            });
        }
    }
}
