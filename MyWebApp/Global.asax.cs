using MyWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MyWebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            HttpContext.Current.Application["korisnici"] = PodaciTxt.procitajKorisnike("~/App_Data/Korisnici.txt");
            HttpContext.Current.Application["fitnesCentri"] = PodaciTxt.procitajFitnesCentre("~/App_Data/FitnesCentri.txt");
            HttpContext.Current.Application["grupniTreninzi"] = PodaciTxt.procitajGrupneTreninge("~/App_Data/GrupniTreninzi.txt");
            HttpContext.Current.Application["komentari"] = PodaciTxt.procitajKomentare("~/App_Data/Komentari.txt");
        }
    }
}
