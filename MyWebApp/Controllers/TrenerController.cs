using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWebApp.Controllers
{
    public class TrenerController : Controller
    {
        // GET: Trener
        public ActionResult Index(string naziv)
        {
            return View();
        }
    }
}