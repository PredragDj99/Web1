using MyWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWebApp.Controllers
{
    public class VlasnikController : Controller
    {
        static string naz="";
        // GET: Vlasnik
        public ActionResult Index(string naziv)
        {
            #region Isto kao home controller -> stranica detalji
            Korisnik user = (Korisnik)Session["user"];
            if (user != null)
            {
                ViewBag.uspesnaPrijava = "jeste";
            }

            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];

            //List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            //Kada obrisem jedan trening onda se prcitaju ponovo da bih lepo prikazao
            List<GrupniTrening> grupniTreninzi = PodaciTxt.procitajGrupneTreninge("~/App_Data/GrupniTreninzi.txt");

            List<GrupniTrening> listaGrupnihTreninga = new List<GrupniTrening>();

            //komentari
            List<Komentar> komentari = PodaciTxt.procitajKomentare("~/App_Data/Komentari.txt");
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

            #region Prikaz komentara
            List<FitnesCentar> vlasnickiFC = new List<FitnesCentar>();
            vlasnickiFC = PodaciTxt.ProcitajVlasnikoveFC(user.Ime, user.Prezime);
            ViewBag.vlasnickiFC = vlasnickiFC;

            List<Komentar> basSviKom = PodaciTxt.procitajBasSveKomentare("~/App_Data/Komentari.txt");

            List<Komentar> komentariOvogVlasnika = new List<Komentar>();
            foreach (var fc in vlasnickiFC)
            {
                foreach (var komentar in basSviKom)
                {
                    if (fc.Naziv.Contains(komentar.FitnesCentarKomentarisan))
                    {
                        komentariOvogVlasnika.Add(komentar);
                    }
                }
            }
            ViewBag.sviKomentari = komentariOvogVlasnika;
            #endregion

            #region Prikazi zaposlene trenere
            List<Korisnik> sviKorisnici = PodaciTxt.procitajKorisnike("~/App_Data/Korisnici.txt");
            List<Korisnik> zaposleniTreneri = new List<Korisnik>();

            foreach (var korisnik in sviKorisnici)
            {
                if (korisnik.Uloga.ToString() == "TRENER")
                {
                    for (int i = 0; i < vlasnickiFC.Count; i++)
                    {
                        if (vlasnickiFC[i].Naziv == korisnik.AngazovanNaFitnesCentar.Naziv)
                        {
                            zaposleniTreneri.Add(korisnik);
                            break;
                        }
                    }
                }
            }
            ViewBag.zaposleniTreneri = zaposleniTreneri;
            #endregion

            naz = naziv;
            return View();
        }

        #region Odobri/Odbij komentar
        public ActionResult Odobri(string komentarise, string fc,string tekst, Int32 ocena)
        {
            string naziv = naz;

            PodaciTxt.OdobriKomentar(komentarise,fc,tekst,ocena);

            return RedirectToAction("Index","Vlasnik", new { naziv });
        }
        public ActionResult Odbij(string komentarise, string tekst, string fc, Int32 ocena)
        {
            string naziv = naz;

            PodaciTxt.OdbijKomentar(komentarise, fc, tekst, ocena);

            return RedirectToAction("Index", "Vlasnik", new { naziv });
        }
        #endregion

        #region Blokiraj trenera
        public ActionResult BlokirajTrenera(string korisnickoIme)
        {
            string naziv = naz;

            PodaciTxt.BlokirajTrenera(korisnickoIme);

            return View("Index", new { naziv });
        }
        #endregion
    }
}