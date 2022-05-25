using MyWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            
            fitnesCentri.Add(new FitnesCentar //primer da bih mogao da testiram dugme
            {
                Naziv = "Fitnes Elastico",
                Adresa="Zeleznicka 23",
                GodinaOtvaranja=2002,
                Vlasnik = null,
                MesecnaClanarina=4000,
                GodisnjaClanarina=15000,
                JedanTrening=500,
                JedanGrupniTrening=2000,
                JedanSaPersonalnimTrenerom=1000
            });
            fitnesCentri.Add(new FitnesCentar //primer da bih mogao da testiram dugme
            {
                Naziv = "Compiano",
                Adresa = "Safarikova 12",
                GodinaOtvaranja = 1998,
                Vlasnik = null,
                MesecnaClanarina = 3000,
                GodisnjaClanarina = 14000,
                JedanTrening = 600,
                JedanGrupniTrening = 1500,
                JedanSaPersonalnimTrenerom = 900
            });

            List<FitnesCentar> sortiraniLista = new List<FitnesCentar>();
            sortiraniLista = fitnesCentri.OrderBy(f => f.Naziv).ToList();

            ViewBag.fitnesCentri = sortiraniLista;

            return View();
        }

                                     //Nullable int - nije unet u formi
        public ActionResult Pretraga(string naziv,string adresa,int? godinaOtvaranjaOd,int? godinaOtvaranjaDo)
        {
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<FitnesCentar> filtrirani = new List<FitnesCentar>();

            string godinaOd = godinaOtvaranjaOd.ToString();
            string godinaDo = godinaOtvaranjaDo.ToString();

            foreach (var item in fitnesCentri)
            {
                if (naziv.Equals("") && adresa.Equals("") && godinaOd.Equals("") && godinaDo.Equals(""))
                {
                    ViewBag.fitnesCentri = filtrirani;
                }
                //Prazan naziv
                else if (naziv.Equals(""))
                {
                    if (adresa.Equals(""))
                    {
                        if (godinaOd.Equals(""))
                        {
                            if (item.GodinaOtvaranja <= godinaOtvaranjaDo)
                            {
                                filtrirani.Add(item);
                                ViewBag.fitnesCentri = filtrirani;
                            }
                            else
                            {
                                ViewBag.fitnesCentri = filtrirani;
                            }
                        }
                        else
                        {
                            if (godinaDo.Equals(""))
                            {
                                if (item.GodinaOtvaranja >= godinaOtvaranjaOd)
                                {
                                    filtrirani.Add(item);
                                    ViewBag.fitnesCentri = filtrirani;
                                }
                                else
                                {
                                    ViewBag.fitnesCentri = filtrirani;
                                }
                            }
                            else
                            {
                                if (item.GodinaOtvaranja >= godinaOtvaranjaOd && item.GodinaOtvaranja <= godinaOtvaranjaDo)
                                {
                                    filtrirani.Add(item);
                                    ViewBag.fitnesCentri = filtrirani;
                                }
                                else
                                {
                                    ViewBag.fitnesCentri = filtrirani;
                                }

                            }
                        }
                    }
                    else if (godinaOd.Equals(""))
                    {
                        if (godinaDo.Equals(""))
                        {
                            if (item.Adresa.Equals(adresa))
                            {
                                filtrirani.Add(item);
                                ViewBag.fitnesCentri = filtrirani;
                            }
                            else
                            {
                                ViewBag.fitnesCentri = filtrirani;
                            }
                        }
                        else
                        {
                            if (item.Adresa.Equals(adresa) && item.GodinaOtvaranja<=godinaOtvaranjaDo)
                            {
                                filtrirani.Add(item);
                                ViewBag.fitnesCentri = filtrirani;
                            }
                            else
                            {
                                ViewBag.fitnesCentri = filtrirani;
                            }
                        }
                    }
                    else if (godinaDo.Equals(""))
                    {
                        if (godinaOd.Equals(""))
                        {
                            if (item.Adresa.Equals(adresa))
                            {
                                filtrirani.Add(item);
                                ViewBag.fitnesCentri = filtrirani;
                            }
                            else
                            {
                                ViewBag.fitnesCentri = filtrirani;
                            }
                        }
                        else
                        {
                            if (item.Adresa.Equals(adresa) && item.GodinaOtvaranja>=godinaOtvaranjaOd)
                            {
                                filtrirani.Add(item);
                                ViewBag.fitnesCentri = filtrirani;
                            }
                            else
                            {
                                ViewBag.fitnesCentri = filtrirani;
                            }
                        }
                    }
                }
                //Prazna adresa
                else if (adresa.Equals(""))
                {
                    if (naziv.Equals(""))
                    {
                        if (godinaOd.Equals(""))
                        {
                            if (item.GodinaOtvaranja <= godinaOtvaranjaDo)
                            {
                                filtrirani.Add(item);
                                ViewBag.fitnesCentri = filtrirani;
                            }
                            else
                            {
                                ViewBag.fitnesCentri = filtrirani;
                            }
                        }
                        else
                        {
                            if (godinaDo.Equals(""))
                            {
                                if (item.GodinaOtvaranja >= godinaOtvaranjaOd)
                                {
                                    filtrirani.Add(item);
                                    ViewBag.fitnesCentri = filtrirani;
                                }
                                else
                                {
                                    ViewBag.fitnesCentri = filtrirani;
                                }
                            }
                            else
                            {
                                if (item.GodinaOtvaranja >= godinaOtvaranjaOd && item.GodinaOtvaranja <= godinaOtvaranjaDo)
                                {
                                    filtrirani.Add(item);
                                    ViewBag.fitnesCentri = filtrirani;
                                }
                                else
                                {
                                    ViewBag.fitnesCentri = filtrirani;
                                }

                            }
                        }
                    }
                    else if (godinaOd.Equals(""))
                    {
                        if (godinaDo.Equals(""))
                        {
                            if (item.Naziv.Equals(naziv))
                            {
                                filtrirani.Add(item);
                                ViewBag.fitnesCentri = filtrirani;
                            }
                            else
                            {
                                ViewBag.fitnesCentri = filtrirani;
                            }
                        }
                        else
                        {
                            if (item.Naziv.Equals(naziv) && item.GodinaOtvaranja <= godinaOtvaranjaDo)
                            {
                                filtrirani.Add(item);
                                ViewBag.fitnesCentri = filtrirani;
                            }
                            else
                            {
                                ViewBag.fitnesCentri = filtrirani;
                            }
                        }
                    }
                    else if (godinaDo.Equals(""))
                    {
                        if (godinaOd.Equals(""))
                        {
                            if (item.Naziv.Equals(naziv))
                            {
                                filtrirani.Add(item);
                                ViewBag.fitnesCentri = filtrirani;
                            }
                            else
                            {
                                ViewBag.fitnesCentri = filtrirani;
                            }
                        }
                        else
                        {
                            if (item.Naziv.Equals(naziv) && item.GodinaOtvaranja >= godinaOtvaranjaOd)
                            {
                                filtrirani.Add(item);
                                ViewBag.fitnesCentri = filtrirani;
                            }
                            else
                            {
                                ViewBag.fitnesCentri = filtrirani;
                            }
                        }
                    }
                }
                //Prazna godina Od
                else if (godinaOd.Equals(""))
                {
                    if (naziv.Equals(""))
                    {
                        if (adresa.Equals(""))
                        {
                            if (item.GodinaOtvaranja <= godinaOtvaranjaDo)
                            {
                                filtrirani.Add(item);
                                ViewBag.fitnesCentri = filtrirani;
                            }
                            else
                            {
                                ViewBag.fitnesCentri = filtrirani;
                            }
                        }
                        else
                        {
                            if (godinaDo.Equals(""))
                            {
                                if (item.Adresa.Equals(adresa))
                                {
                                    filtrirani.Add(item);
                                    ViewBag.fitnesCentri = filtrirani;
                                }
                                else
                                {
                                    ViewBag.fitnesCentri = filtrirani;
                                }
                            }
                            else
                            {
                                if (item.Adresa.Equals(adresa) && item.GodinaOtvaranja<=godinaOtvaranjaDo)
                                {
                                    filtrirani.Add(item);
                                    ViewBag.fitnesCentri = filtrirani;
                                }
                                else
                                {
                                    ViewBag.fitnesCentri = filtrirani;
                                }
                            }
                        }
                    }
                    else 
                    {
                        if (adresa.Equals(""))
                        {
                            if (godinaDo.Equals(""))
                            {
                                if(item.Naziv.Equals(naziv))
                                {
                                    filtrirani.Add(item);
                                    ViewBag.fitnesCentri = filtrirani;
                                }
                                else
                                {
                                    ViewBag.fitnesCentri = filtrirani;
                                }
                            }
                            else
                            {
                                if (item.Naziv.Equals(naziv) && item.GodinaOtvaranja<=godinaOtvaranjaDo)
                                {
                                    filtrirani.Add(item);
                                    ViewBag.fitnesCentri = filtrirani;
                                }
                                else
                                {
                                    ViewBag.fitnesCentri = filtrirani;
                                }
                            }
                        }
                        else
                        {
                            if (godinaDo.Equals(""))
                            {
                                if (item.Naziv.Equals(naziv) && item.Adresa.Equals(adresa))
                                {
                                    filtrirani.Add(item);
                                    ViewBag.fitnesCentri = filtrirani;
                                }
                                else
                                {
                                    ViewBag.fitnesCentri = filtrirani;
                                }
                            }
                            else
                            {
                                if (item.Naziv.Equals(naziv) && item.Adresa.Equals(adresa) && item.GodinaOtvaranja<=godinaOtvaranjaDo)
                                {
                                    filtrirani.Add(item);
                                    ViewBag.fitnesCentri = filtrirani;
                                }
                                else
                                {
                                    ViewBag.fitnesCentri = filtrirani;
                                }
                            }
                        }
                    }
                }
                //Prazna godina do
                else if (godinaDo.Equals(""))
                {
                    ///krece
                    if (naziv.Equals(""))
                    {
                        if (adresa.Equals(""))
                        {
                            if (item.GodinaOtvaranja >= godinaOtvaranjaOd)
                            {
                                filtrirani.Add(item);
                                ViewBag.fitnesCentri = filtrirani;
                            }
                            else
                            {
                                ViewBag.fitnesCentri = filtrirani;
                            }
                        }
                        else
                        {
                            if (godinaOd.Equals(""))
                            {
                                if (item.Adresa.Equals(adresa))
                                {
                                    filtrirani.Add(item);
                                    ViewBag.fitnesCentri = filtrirani;
                                }
                                else
                                {
                                    ViewBag.fitnesCentri = filtrirani;
                                }
                            }
                            else
                            {
                                if (item.Adresa.Equals(adresa) && item.GodinaOtvaranja >= godinaOtvaranjaOd)
                                {
                                    filtrirani.Add(item);
                                    ViewBag.fitnesCentri = filtrirani;
                                }
                                else
                                {
                                    ViewBag.fitnesCentri = filtrirani;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (adresa.Equals(""))
                        {
                            if (godinaOd.Equals(""))
                            {
                                if (item.Naziv.Equals(naziv))
                                {
                                    filtrirani.Add(item);
                                    ViewBag.fitnesCentri = filtrirani;
                                }
                                else
                                {
                                    ViewBag.fitnesCentri = filtrirani;
                                }
                            }
                            else
                            {
                                if (item.Naziv.Equals(naziv) && item.GodinaOtvaranja >= godinaOtvaranjaOd)
                                {
                                    filtrirani.Add(item);
                                    ViewBag.fitnesCentri = filtrirani;
                                }
                                else
                                {
                                    ViewBag.fitnesCentri = filtrirani;
                                }
                            }
                        }
                        else
                        {
                            if (godinaOd.Equals(""))
                            {
                                if (item.Naziv.Equals(naziv) && item.Adresa.Equals(adresa))
                                {
                                    filtrirani.Add(item);
                                    ViewBag.fitnesCentri = filtrirani;
                                }
                                else
                                {
                                    ViewBag.fitnesCentri = filtrirani;
                                }
                            }
                            else
                            {
                                if (item.Naziv.Equals(naziv) && item.Adresa.Equals(adresa) && item.GodinaOtvaranja >= godinaOtvaranjaOd)
                                {
                                    filtrirani.Add(item);
                                    ViewBag.fitnesCentri = filtrirani;
                                }
                                else
                                {
                                    ViewBag.fitnesCentri = filtrirani;
                                }
                            }
                        }
                    }
                }
                else if(item.Naziv == naziv && item.Adresa == adresa && item.GodinaOtvaranja>=godinaOtvaranjaOd && item.GodinaOtvaranja <= godinaOtvaranjaDo)
                {
                    filtrirani.Add(item);
                    ViewBag.fitnesCentri = filtrirani;
                }
            }

            return View("Index");
        }


        public ActionResult Sortiraj(string submit, string sort)
        {
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<FitnesCentar> sortiraniCentri = new List<FitnesCentar>();

            if (sort.Equals("rastuce"))
            {
                switch (submit)
                {
                    case "naziv":
                        sortiraniCentri= fitnesCentri.OrderBy(f => f.Naziv).ToList();
                        break;
                    case "adresa":
                        sortiraniCentri = fitnesCentri.OrderBy(f => f.Adresa).ToList();
                        break;
                    case "godinaOtvaranja":
                        sortiraniCentri = fitnesCentri.OrderBy(f => f.GodinaOtvaranja).ToList();
                        break;
                }
            }
            else if(sort.Equals("opadajuce"))
            {
                switch (submit)
                {
                    case "naziv":
                        sortiraniCentri = fitnesCentri.OrderByDescending(f => f.Naziv).ToList();
                        break;
                    case "adresa":
                        sortiraniCentri = fitnesCentri.OrderByDescending(f => f.Adresa).ToList();
                        break;
                    case "godinaOtvaranja":
                        sortiraniCentri = fitnesCentri.OrderByDescending(f => f.GodinaOtvaranja).ToList();
                        break;
                }
            }
            else
            {
                ViewBag.fitnesCentri = sortiraniCentri;
            }
            ViewBag.fitnesCentri = sortiraniCentri;

            return View("Index");
        }

        //treba, forma za link 
        public ActionResult Detalji()
        {
            return View("Detalji");
        }
        
        /////////////////////////////////////////////////////////////////


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}