using MyWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWebApp.Controllers
{
    public class PosetilacController : Controller
    {
        // GET: Posetilac
        public ActionResult Index(string naziv)
        {
            #region Isto kao home controller -> stranica detalji
            Korisnik user = (Korisnik)Session["user"];
            if (user != null)
            {
                ViewBag.uspesnaPrijava = "jeste";
            }

            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];

            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            List<GrupniTrening> listaGrupnihTreninga = new List<GrupniTrening>();

            //komentari
            List<Komentar> komentari = (List<Komentar>)HttpContext.Application["komentari"];
            List<Komentar> filtriraniKomentari = new List<Komentar>();
            
            foreach (var item in komentari)
            {
                if (item.FitnesCentarKomentarisan.ToString().Equals(naziv))
                {
                    filtriraniKomentari.Add(item);
                }
            }
            ViewBag.naziv = naziv;
            ViewBag.komentari = filtriraniKomentari;

            //grupni treninzi ovog fitnes centra
            foreach (var item in grupniTreninzi)
            {
                if (item.FitnesCentarOdrzava.Naziv.ToString().Equals(naziv) && item.DatumIVremeTreninga > DateTime.Now)
                {
                    listaGrupnihTreninga.Add(item);
                }
            }
            ViewBag.grupniTreninzi = listaGrupnihTreninga;

            //detalji za ovaj fitnes centar
            foreach (var fc in fitnesCentri)
            {
                if (fc.Naziv.ToString().Equals(naziv))
                {
                    ViewBag.fitnesCentar = fc;
                    break;
                }
            }
            #endregion

            //Trenutno sve svaka opcija moze kliknuti
            #region Prijava za trening

            ViewBag.prijavljenTrening = "Niste prijavljeni";
            ViewBag.imaMesta = "true";

            #endregion

            return View();
        }

        #region Prijava za trening
        //Dodatne opcije za posetioca
        public ActionResult PrijavaZaTrening(string naziv, string datumVreme,int maxPosetioca, int brojPosetioca,List<Korisnik> spisakPosetilaca) //ne mogu vise parametara
        {
            #region Isto kao home controller -> stranica detalji
            Korisnik user = (Korisnik)Session["user"];
            if (user != null)
            {
                ViewBag.uspesnaPrijava = "jeste";
            }

            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];

            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            List<GrupniTrening> listaGrupnihTreninga = new List<GrupniTrening>();

            //komentari
            List<Komentar> komentari = (List<Komentar>)HttpContext.Application["komentari"];
            List<Komentar> filtriraniKomentari = new List<Komentar>();

            foreach (var item in komentari)
            {
                if (item.FitnesCentarKomentarisan.ToString().Equals(naziv))
                {
                    filtriraniKomentari.Add(item);
                }
            }
            ViewBag.naziv = naziv;
            ViewBag.komentari = filtriraniKomentari;

            //grupni treninzi ovog fitnes centra
            foreach (var item in grupniTreninzi)
            {
                if (item.FitnesCentarOdrzava.Naziv.ToString().Equals(naziv) && item.DatumIVremeTreninga > DateTime.Now)
                {
                    listaGrupnihTreninga.Add(item);
                }
            }
            ViewBag.grupniTreninzi = listaGrupnihTreninga;

            //detalji za ovaj fitnes centar
            foreach (var fc in fitnesCentri)
            {
                if (fc.Naziv.ToString().Equals(naziv))
                {
                    ViewBag.fitnesCentar = fc;
                    break;
                }
            }
            #endregion

            #region Ako je vec prijavljen 

            datumVreme = datumVreme.Substring(0, datumVreme.Length - 27); //moram iseci jer napravi space neki
            ViewBag.ovajJeKliknutVreme = datumVreme;
            bool vecPrijavljen = PodaciTxt.procitajJedanGrupniTrening("~/App_Data/GrupniTreninzi.txt",naziv,datumVreme,user.Ime,user.Prezime);
            if (vecPrijavljen)
            {
                 ViewBag.ranijePrijavljen = "jeste";
            }else{
                 ViewBag.ranijePrijavljen = "nije";
            }
            #endregion

            //AKO JE VEC PRIJAVLJEN PRESKOCI OVE KORAKE
            if (ViewBag.ranijePrijavljen == "nije")
            {
                //Isti fitnes centar ne moze imati 2 treninga istovremeno(zato gledam datum i vreme); ne moze da se prijavi ako je popunjeno
                #region Ako je mesto popunjeno

                ViewBag.imaMesta = "true";
                if (maxPosetioca == brojPosetioca)
                {
                    ViewBag.imaMesta = "false";
                }
                else
                {
                    //Dodajem korisnika na kraj liste za taj trening; proveravam da li je to taj trening
                    ViewBag.prijavljenTrening = "Prijavljeni ste";
                    PodaciTxt.DodajUGrupniTrening(user.Ime, user.Prezime, naziv, datumVreme);
                }
                #endregion
            }

            return View("Index");
        }
        #endregion

        #region Tabela ranijih treninga
        public ActionResult RanijiTreninzi()
        {
            Korisnik user = (Korisnik)Session["user"];
            if (user != null)
            {
                ViewBag.uspesnaPrijava = "jeste";
            }

            List<GrupniTrening> grupniTreninziNaKojimaJePrisustvovao = PodaciTxt.prisustvovaoGrupnomTreningu("~/App_Data/GrupniTreninzi.txt", user.Ime, user.Prezime);
            List<GrupniTrening> listaGrupnihTreninga = new List<GrupniTrening>();

            //grupni treninzi na kojima je ucestvovao
            foreach (var item in grupniTreninziNaKojimaJePrisustvovao)
            {
                if (item.DatumIVremeTreninga < DateTime.Now)
                {
                    listaGrupnihTreninga.Add(item);
                }
            }
            ViewBag.grupniTreninzi = listaGrupnihTreninga;

            return View("RanijiTreninzi");
        }
        #endregion

        #region Pretraga
        public ActionResult Pretraga(string naziv, string fitnesCentarOdrzava, string tipTreninga)
        {
            #region Grupni treninzi na kojima je ucestvovao
            Korisnik user = (Korisnik)Session["user"];
            if (user != null)
            {
                ViewBag.uspesnaPrijava = "jeste";
            }

            List<GrupniTrening> grupniTreninziNaKojimaJePrisustvovao = PodaciTxt.prisustvovaoGrupnomTreningu("~/App_Data/GrupniTreninzi.txt", user.Ime, user.Prezime);
            List<GrupniTrening> listaGrupnihTreninga = new List<GrupniTrening>();

            //grupni treninzi na kojima je ucestvovao
            foreach (var item in grupniTreninziNaKojimaJePrisustvovao)
            {
                if (item.DatumIVremeTreninga < DateTime.Now)
                {
                    listaGrupnihTreninga.Add(item);
                }
            }
            ViewBag.grupniTreninzi = listaGrupnihTreninga;
            #endregion

            //mozda da procitam sve treninge pa onda pokupim sve tipove Treninga i izbacim ih u listBox
            //kombinovana pretraga
            return View("RanijiTreninzi");
        }
        #endregion

        #region Sortiranje
        public ActionResult Sortiraj(string sort,string submit)
        {
            #region Grupni treninzi na kojima je ucestvovao
            Korisnik user = (Korisnik)Session["user"];
            if (user != null)
            {
                ViewBag.uspesnaPrijava = "jeste";
            }

            List<GrupniTrening> grupniTreninziNaKojimaJePrisustvovao = PodaciTxt.prisustvovaoGrupnomTreningu("~/App_Data/GrupniTreninzi.txt", user.Ime, user.Prezime);
            List<GrupniTrening> listaGrupnihTreninga = new List<GrupniTrening>();

            //grupni treninzi na kojima je ucestvovao
            foreach (var item in grupniTreninziNaKojimaJePrisustvovao)
            {
                if (item.DatumIVremeTreninga < DateTime.Now)
                {
                    listaGrupnihTreninga.Add(item);
                }
            }
            ViewBag.grupniTreninzi = listaGrupnihTreninga;
            #endregion

            //Odradi sortiranje po parametrima
            return View("RanijiTreninzi");
        }
        #endregion
    }
}