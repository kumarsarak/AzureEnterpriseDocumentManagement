using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Security.Claims;

namespace WebRole1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            System.Security.Claims.ClaimsIdentity claimsIdentity = System.Web.HttpContext.Current.User.Identity as System.Security.Claims.ClaimsIdentity;
            foreach (Claim claim in claimsIdentity.Claims)
            {
                ViewBag.Message = ViewBag.Message + claim.Type + ":" + claim.Value;
            }
            // ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Unauthorized()
        {
            return View();
        }
    }
}