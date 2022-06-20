using MyWebApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
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
                    if (item.Obrisan == "AKTIVAN")
                    {
                        listaGrupnihTreninga.Add(item);
                    }
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

            #region Prikaz trenerskih tabela
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];

            List<GrupniTrening> gt = new List<GrupniTrening>();
            foreach (var item in user.ListaTreninziAngazovan)
            {
                //imam datum,tip
                string dat = PodaciTxt.pronadjiDatumIVremeTreningaTrener(item.FitnesCentarOdrzava.Naziv,item.TipTreninga,item.DatumIVremeTreninga.ToString());
                GrupniTrening trebajuMiOviPodaci = PodaciTxt.procitajJedanGrupniTreningTrenera(item.FitnesCentarOdrzava.Naziv,dat);
                //pribavljam
                item.MaksimalanBrojPosetioca = trebajuMiOviPodaci.MaksimalanBrojPosetioca;
                item.SpisakPosetilaca = trebajuMiOviPodaci.SpisakPosetilaca;
                item.TrajanjeTreningaMinute = trebajuMiOviPodaci.TrajanjeTreningaMinute;
                item.Naziv = trebajuMiOviPodaci.Naziv;
                item.DatumIVremeTreninga = trebajuMiOviPodaci.DatumIVremeTreninga;
                item.Obrisan = trebajuMiOviPodaci.Obrisan;

                //Da bi mogao da ih vidi
                for (int i = 0; i < trebajuMiOviPodaci.SpisakPosetilaca.Count; i++)
                {
                    item.SpisakPosetilaca[i].Ime = trebajuMiOviPodaci.SpisakPosetilaca[i].Ime;
                    item.SpisakPosetilaca[i].Prezime= trebajuMiOviPodaci.SpisakPosetilaca[i].Prezime;
                }

                user.AngazovanNaFitnesCentar.Naziv = item.FitnesCentarOdrzava.Naziv;
                if (item.Obrisan == "AKTIVAN")
                {
                    gt.Add(item);
                }
            }

            List<GrupniTrening> angazovan = new List<GrupniTrening>();
            List<GrupniTrening> stari = new List<GrupniTrening>();

            //Ovo je konacan ispis grupnih treninga ovog trenera
            foreach (var item in gt)
            {
                if (item.DatumIVremeTreninga > DateTime.Now)
                {
                    angazovan.Add(item);
                }
                else
                {
                    stari.Add(item);
                }
            }
            ViewBag.treninziAngazovan = angazovan;
            ViewBag.stariTreninzi = stari;
            #endregion

            return View();
        }

        public ActionResult ObrisiTrening(string naziv,string spisakPosetilacaCount, string datum)
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
                    if (item.Obrisan == "AKTIVAN")
                    {
                        listaGrupnihTreninga.Add(item);
                    }
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

            #region Prikaz trenerskih tabela
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];

            List<GrupniTrening> gt = new List<GrupniTrening>();
            foreach (var item in user.ListaTreninziAngazovan)
            {
                //imam datum,tip
                string dat = PodaciTxt.pronadjiDatumIVremeTreningaTrener(item.FitnesCentarOdrzava.Naziv, item.TipTreninga, item.DatumIVremeTreninga.ToString());
                GrupniTrening trebajuMiOviPodaci = PodaciTxt.procitajJedanGrupniTreningTrenera(item.FitnesCentarOdrzava.Naziv, dat);
                //pribavljam
                item.MaksimalanBrojPosetioca = trebajuMiOviPodaci.MaksimalanBrojPosetioca;
                item.SpisakPosetilaca = trebajuMiOviPodaci.SpisakPosetilaca;
                item.TrajanjeTreningaMinute = trebajuMiOviPodaci.TrajanjeTreningaMinute;
                item.Naziv = trebajuMiOviPodaci.Naziv;
                item.DatumIVremeTreninga = trebajuMiOviPodaci.DatumIVremeTreninga;

                //Da bi mogao da ih vidi
                for (int i = 0; i < trebajuMiOviPodaci.SpisakPosetilaca.Count; i++)
                {
                    item.SpisakPosetilaca[i].Ime = trebajuMiOviPodaci.SpisakPosetilaca[i].Ime;
                    item.SpisakPosetilaca[i].Prezime = trebajuMiOviPodaci.SpisakPosetilaca[i].Prezime;
                }

                if (item.Obrisan == "AKTIVAN")
                    gt.Add(item);
            }

            List<GrupniTrening> angazovan = new List<GrupniTrening>();
            List<GrupniTrening> stari = new List<GrupniTrening>();

            //Ovo je konacan ispis grupnih treninga ovog trenera
            foreach (var item in gt)
            {
                if (item.DatumIVremeTreninga > DateTime.Now)
                {
                    if (item.Obrisan == "AKTIVAN")
                    angazovan.Add(item);
                }
                else
                {
                    if (item.Obrisan == "AKTIVAN")
                        stari.Add(item);
                }
            }
            ViewBag.treninziAngazovan = angazovan;
            ViewBag.stariTreninzi = stari;


            ViewBag.ovajJeKliknutVreme = datum;
            //DateTime buduci = DateTime.ParseExact(datum, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            if (spisakPosetilacaCount == "1") //ovaj jedan je prazan konstruktor
            {
                for (int i = 0; i < angazovan.Count; i++)
                {
                    if (angazovan[i].SpisakPosetilaca[i].Ime == null) //to je taj koji je prazan
                    {
                        ViewBag.brisanje = "moze";
                        PodaciTxt.ObrisiTrening(naziv,datum);
                        angazovan.RemoveAt(i);
                        break;
                    }
                    ViewBag.brisanje = "ne moze";
                }
            }
            else
            {
                ViewBag.brisanje = "ne moze";
            }
            #endregion

            return View("Index");
        }

        public ActionResult DodajTrening()
        {
            return View("Dodavanje");
        }

        public ActionResult DodajTreningTxt(string naziv, string tipTreninga, string trajanjeTreningaMinute, string datumIVremeTreninga, int? maksimalanBrojPosetioca)
        {
            ViewBag.dodajTrening = "Trening uspesno napravljen!";

            return View("Dodavanje");
        }

        public ActionResult Sortiraj(string nazivFC, string sort, string submit)
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
            List<Komentar> komentari = (List<Komentar>)HttpContext.Application["komentari"];
            List<Komentar> filtriraniKomentari = new List<Komentar>();

            foreach (var item in komentari)
            {
                if (item.FitnesCentarKomentarisan.ToString().Equals(nazivFC))
                {
                    filtriraniKomentari.Add(item);
                }
            }
            ViewBag.naziv = nazivFC;
            ViewBag.komentari = filtriraniKomentari;

            //grupni treninzi ovog fitnes centra
            foreach (var item in grupniTreninzi)
            {
                if (item.FitnesCentarOdrzava.Naziv.ToString().Equals(nazivFC) && item.DatumIVremeTreninga > DateTime.Now)
                {
                    if (item.Obrisan == "AKTIVAN")
                    {
                        listaGrupnihTreninga.Add(item);
                    }
                }
            }
            ViewBag.grupniTreninzi = listaGrupnihTreninga;

            //detalji za ovaj fitnes centar
            foreach (var fc in fitnesCentri)
            {
                if (fc.Naziv.ToString().Equals(nazivFC))
                {
                    ViewBag.fitnesCentar = fc;
                    break;
                }
            }
            #endregion 

            #region Prikaz trenerskih tabela
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];

            List<GrupniTrening> gt = new List<GrupniTrening>();
            foreach (var item in user.ListaTreninziAngazovan)
            {
                //imam datum,tip
                string dat = PodaciTxt.pronadjiDatumIVremeTreningaTrener(item.FitnesCentarOdrzava.Naziv, item.TipTreninga, item.DatumIVremeTreninga.ToString());
                GrupniTrening trebajuMiOviPodaci = PodaciTxt.procitajJedanGrupniTreningTrenera(item.FitnesCentarOdrzava.Naziv, dat);
                //pribavljam
                item.MaksimalanBrojPosetioca = trebajuMiOviPodaci.MaksimalanBrojPosetioca;
                item.SpisakPosetilaca = trebajuMiOviPodaci.SpisakPosetilaca;
                item.TrajanjeTreningaMinute = trebajuMiOviPodaci.TrajanjeTreningaMinute;
                item.Naziv = trebajuMiOviPodaci.Naziv;
                item.DatumIVremeTreninga = trebajuMiOviPodaci.DatumIVremeTreninga;
                item.Obrisan = trebajuMiOviPodaci.Obrisan;

                //Da bi mogao da ih vidi
                for (int i = 0; i < trebajuMiOviPodaci.SpisakPosetilaca.Count; i++)
                {
                    item.SpisakPosetilaca[i].Ime = trebajuMiOviPodaci.SpisakPosetilaca[i].Ime;
                    item.SpisakPosetilaca[i].Prezime = trebajuMiOviPodaci.SpisakPosetilaca[i].Prezime;
                }

                user.AngazovanNaFitnesCentar.Naziv = item.FitnesCentarOdrzava.Naziv;
                if (item.Obrisan == "AKTIVAN")
                {
                    gt.Add(item);
                }
            }

            List<GrupniTrening> angazovan = new List<GrupniTrening>();
            List<GrupniTrening> stari = new List<GrupniTrening>();

            //Ovo je konacan ispis grupnih treninga ovog trenera
            foreach (var item in gt)
            {
                if (item.DatumIVremeTreninga > DateTime.Now)
                {
                    angazovan.Add(item);
                }
                else
                {
                    stari.Add(item);
                }
            }
            ViewBag.treninziAngazovan = angazovan;
            ViewBag.stariTreninzi = stari;
            #endregion

            #region Sortiranje
            List<GrupniTrening> sortiraniGrupniTreninzi = new List<GrupniTrening>();
            if (sort.Equals("rastuce"))
            {
                switch (submit)
                {
                    case "naziv":
                        sortiraniGrupniTreninzi = stari.OrderBy(f => f.Naziv).ToList();
                        break;
                    case "tipTreninga":
                        sortiraniGrupniTreninzi = stari.OrderBy(f => f.TipTreninga).ToList();
                        break;
                    case "datumIVremeTreninga":
                        sortiraniGrupniTreninzi = stari.OrderBy(f => f.DatumIVremeTreninga).ToList();
                        break;
                }
            }
            else if (sort.Equals("opadajuce"))
            {
                switch (submit)
                {
                    case "naziv":
                        sortiraniGrupniTreninzi = stari.OrderByDescending(f => f.Naziv).ToList();
                        break;
                    case "tipTreninga":
                        sortiraniGrupniTreninzi = stari.OrderByDescending(f => f.TipTreninga).ToList();
                        break;
                    case "datumIVremeTreninga":
                        sortiraniGrupniTreninzi = stari.OrderByDescending(f => f.DatumIVremeTreninga).ToList();
                        break;
                }
            }
            else
            {
                ViewBag.stariTreninzi = stari;
            }
            ViewBag.stariTreninzi = sortiraniGrupniTreninzi;
            #endregion

            return View("Index");
        }
    }
}