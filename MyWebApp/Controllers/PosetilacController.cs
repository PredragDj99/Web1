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

        //Dodatne opcije za posetioca
        public ActionResult PrijavaZaTrening(string naziv, string datumVreme,int maxPosetioca, int brojPosetioca) //ne mogu vise parametara
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


            ///////////////////////////////////////////////////////////////OVDE TREBA SREDITI
            //Isti fitnes centar ne moze imati 2 treninga istovremeno
            //ViewBag.ovajJeKliknutNaziv = naziv;
            //ViewBag.ovajJeKliknutVreme = datumVreme;

            //Mozda stranicu za potvrdjivanje prijave i onda on prebaci u novu akciju koja kaze jel moze ili ne

            #region Ako je mesto popunjeno
            //ne moze da se prijavi ako je vec popunjeno
            ViewBag.imaMesta = "true";
            if (maxPosetioca == brojPosetioca)
            {
                ViewBag.imaMesta = "false";
            }
            #endregion

            ViewBag.trenutnoPrijavljen = user;

            //if (ne moze dvaput da se prijavi)
            {
                // ViewBag.prijavljenTrening = "Prijavljeni ste";
            }

            return View("Index");
        }
    }
}