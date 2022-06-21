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
        static string datumProsledi = "";
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

            //Za prikaz dugmeta
            ViewBag.brisanje = "ne moze";
            ViewBag.ovajJeKliknutVreme = datumProsledi;

            return View();
        }

        #region Obrisi trening
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

            if (spisakPosetilacaCount== "0")
            {
                for (int i = 0; i < angazovan.Count; i++)
                {
                    if(angazovan[i].SpisakPosetilaca.Count()==0)
                    {
                        ViewBag.brisanje = "moze";
                        PodaciTxt.ObrisiTrening(naziv, datum);
                        angazovan.RemoveAt(i);
                        break;
                    }
                }
            }
            #endregion

            datumProsledi = datum;
            string temp = naziv;
            return RedirectToAction("Index", new { naziv = temp});
        }
        #endregion

        #region Prikazi posetioce treninga
        public ActionResult PrikaziPosetioce(string naziv, List<Korisnik> spisakPosetilaca, string datum)
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
            #endregion

            ViewBag.spisakPosetilaca = "Nema posetioca";
            foreach (var item in gt)
            {
                string dat = PodaciTxt.pronadjiDatumIVremeTreningaTrener(item.FitnesCentarOdrzava.Naziv, item.TipTreninga, item.DatumIVremeTreninga.ToString());
                if (dat == datum)
                {
                    ViewBag.spisakPosetilaca = item.SpisakPosetilaca; //datum nece biti dobar
                }
            }
            ViewBag.naziv = naziv;

            return View("SpisakPosetilaca");
        }
        #endregion

        #region Dodaj trening
        public ActionResult DodajTrening()
        {
            return View("Dodavanje");
        }

        public ActionResult DodajTreningTxt(string naziv, string tipTreninga, string trajanjeTreningaMinute, string datumIVremeTreninga, int? maksimalanBrojPosetioca)
        {
            ViewBag.dodajTrening = "Trening uspesno napravljen!";

            return View("Dodavanje");
        }
        #endregion

        #region Modifikuj trening
        public ActionResult ModifikujTrening()
        {
            return View("Modifikuj");
        }

        #endregion

        #region Sortiranje starih treninga
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
        #endregion

        #region Pretraga starih treninga
        public ActionResult Pretraga(string nazivFC,string naziv,string tipTreninga,int? godinaOtvaranjaOd, int? godinaOtvaranjaDo)
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

            List<FitnesCentar> filtrirani = new List<FitnesCentar>();
            //Po godini otvaranja znaci da pristupa svakom treninku i gleda njegov fitnes centar i kada je on otvoren
            string godinaOd = godinaOtvaranjaOd.ToString();
            string godinaDo = godinaOtvaranjaDo.ToString();

            //znaci pristupam i fitnes centru i listi ovih starih
            //adresu zameniti sa tipomTreninga

            #region Kombinovana pretraga
            /*
            foreach (var item in stari)
            {
                if (naziv.Equals("") && adresa.Equals("") && godinaOd.Equals("") && godinaDo.Equals(""))
                {
                    ViewBag.stariTreninzi = stari;
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
                                ViewBag.stariTreninzi = filtrirani;
                            }
                            else
                            {
                                ViewBag.stariTreninzi = filtrirani;
                            }
                        }
                        else
                        {
                            if (godinaDo.Equals(""))
                            {
                                if (item.GodinaOtvaranja >= godinaOtvaranjaOd)
                                {
                                    filtrirani.Add(item);
                                    ViewBag.stariTreninzi = filtrirani;
                                }
                                else
                                {
                                    ViewBag.stariTreninzi = filtrirani;
                                }
                            }
                            else
                            {
                                if (item.GodinaOtvaranja >= godinaOtvaranjaOd && item.GodinaOtvaranja <= godinaOtvaranjaDo)
                                {
                                    filtrirani.Add(item);
                                    ViewBag.stariTreninzi = filtrirani;
                                }
                                else
                                {
                                    ViewBag.stariTreninzi = filtrirani;
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
                                ViewBag.stariTreninzi = filtrirani;
                            }
                            else
                            {
                                ViewBag.stariTreninzi = filtrirani;
                            }
                        }
                        else
                        {
                            if (item.Adresa.Equals(adresa) && item.GodinaOtvaranja <= godinaOtvaranjaDo)
                            {
                                filtrirani.Add(item);
                                ViewBag.stariTreninzi = filtrirani;
                            }
                            else
                            {
                                ViewBag.stariTreninzi = filtrirani;
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
                                ViewBag.stariTreninzi = filtrirani;
                            }
                            else
                            {
                                ViewBag.stariTreninzi = filtrirani;
                            }
                        }
                        else
                        {
                            if (item.Adresa.Equals(adresa) && item.GodinaOtvaranja >= godinaOtvaranjaOd)
                            {
                                filtrirani.Add(item);
                                ViewBag.stariTreninzi = filtrirani;
                            }
                            else
                            {
                                ViewBag.stariTreninzi = filtrirani;
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
                                ViewBag.stariTreninzi = filtrirani;
                            }
                            else
                            {
                                ViewBag.stariTreninzi = filtrirani;
                            }
                        }
                        else
                        {
                            if (godinaDo.Equals(""))
                            {
                                if (item.GodinaOtvaranja >= godinaOtvaranjaOd)
                                {
                                    filtrirani.Add(item);
                                    ViewBag.stariTreninzi = filtrirani;
                                }
                                else
                                {
                                    ViewBag.stariTreninzi = filtrirani;
                                }
                            }
                            else
                            {
                                if (item.GodinaOtvaranja >= godinaOtvaranjaOd && item.GodinaOtvaranja <= godinaOtvaranjaDo)
                                {
                                    filtrirani.Add(item);
                                    ViewBag.stariTreninzi = filtrirani;
                                }
                                else
                                {
                                    ViewBag.stariTreninzi = filtrirani;
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
                                ViewBag.stariTreninzi = filtrirani;
                            }
                            else
                            {
                                ViewBag.stariTreninzi = filtrirani;
                            }
                        }
                        else
                        {
                            if (item.Naziv.Equals(naziv) && item.GodinaOtvaranja <= godinaOtvaranjaDo)
                            {
                                filtrirani.Add(item);
                                ViewBag.stariTreninzi = filtrirani;
                            }
                            else
                            {
                                ViewBag.stariTreninzi = filtrirani;
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
                                ViewBag.stariTreninzi = filtrirani;
                            }
                            else
                            {
                                ViewBag.stariTreninzi = filtrirani;
                            }
                        }
                        else
                        {
                            if (item.Naziv.Equals(naziv) && item.GodinaOtvaranja >= godinaOtvaranjaOd)
                            {
                                filtrirani.Add(item);
                                ViewBag.stariTreninzi = filtrirani;
                            }
                            else
                            {
                                ViewBag.stariTreninzi = filtrirani;
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
                                ViewBag.stariTreninzi = filtrirani;
                            }
                            else
                            {
                                ViewBag.stariTreninzi = filtrirani;
                            }
                        }
                        else
                        {
                            if (godinaDo.Equals(""))
                            {
                                if (item.Adresa.Equals(adresa))
                                {
                                    filtrirani.Add(item);
                                    ViewBag.stariTreninzi = filtrirani;
                                }
                                else
                                {
                                    ViewBag.stariTreninzi = filtrirani;
                                }
                            }
                            else
                            {
                                if (item.Adresa.Equals(adresa) && item.GodinaOtvaranja <= godinaOtvaranjaDo)
                                {
                                    filtrirani.Add(item);
                                    ViewBag.stariTreninzi = filtrirani;
                                }
                                else
                                {
                                    ViewBag.stariTreninzi = filtrirani;
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
                                if (item.Naziv.Equals(naziv))
                                {
                                    filtrirani.Add(item);
                                    ViewBag.stariTreninzi = filtrirani;
                                }
                                else
                                {
                                    ViewBag.stariTreninzi = filtrirani;
                                }
                            }
                            else
                            {
                                if (item.Naziv.Equals(naziv) && item.GodinaOtvaranja <= godinaOtvaranjaDo)
                                {
                                    filtrirani.Add(item);
                                    ViewBag.stariTreninzi = filtrirani;
                                }
                                else
                                {
                                    ViewBag.stariTreninzi = filtrirani;
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
                                    ViewBag.stariTreninzi = filtrirani;
                                }
                                else
                                {
                                    ViewBag.stariTreninzi = filtrirani;
                                }
                            }
                            else
                            {
                                if (item.Naziv.Equals(naziv) && item.Adresa.Equals(adresa) && item.GodinaOtvaranja <= godinaOtvaranjaDo)
                                {
                                    filtrirani.Add(item);
                                    ViewBag.stariTreninzi = filtrirani;
                                }
                                else
                                {
                                    ViewBag.stariTreninzi = filtrirani;
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
                                ViewBag.stariTreninzi = filtrirani;
                            }
                            else
                            {
                                ViewBag.stariTreninzi = filtrirani;
                            }
                        }
                        else
                        {
                            if (godinaOd.Equals(""))
                            {
                                if (item.Adresa.Equals(adresa))
                                {
                                    filtrirani.Add(item);
                                    ViewBag.stariTreninzi = filtrirani;
                                }
                                else
                                {
                                    ViewBag.stariTreninzi = filtrirani;
                                }
                            }
                            else
                            {
                                if (item.Adresa.Equals(adresa) && item.GodinaOtvaranja >= godinaOtvaranjaOd)
                                {
                                    filtrirani.Add(item);
                                    ViewBag.stariTreninzi = filtrirani;
                                }
                                else
                                {
                                    ViewBag.stariTreninzi = filtrirani;
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
                                    ViewBag.stariTreninzi = filtrirani;
                                }
                                else
                                {
                                    ViewBag.stariTreninzi = filtrirani;
                                }
                            }
                            else
                            {
                                if (item.Naziv.Equals(naziv) && item.GodinaOtvaranja >= godinaOtvaranjaOd)
                                {
                                    filtrirani.Add(item);
                                    ViewBag.stariTreninzi = filtrirani;
                                }
                                else
                                {
                                    ViewBag.stariTreninzi = filtrirani;
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
                                    ViewBag.stariTreninzi = filtrirani;
                                }
                                else
                                {
                                    ViewBag.stariTreninzi = filtrirani;
                                }
                            }
                            else
                            {
                                if (item.Naziv.Equals(naziv) && item.Adresa.Equals(adresa) && item.GodinaOtvaranja >= godinaOtvaranjaOd)
                                {
                                    filtrirani.Add(item);
                                    ViewBag.stariTreninzi = filtrirani;
                                }
                                else
                                {
                                    ViewBag.stariTreninzi = filtrirani;
                                }
                            }
                        }
                    }
                }
                else if (item.Naziv == naziv && item.Adresa == adresa && item.GodinaOtvaranja >= godinaOtvaranjaOd && item.GodinaOtvaranja <= godinaOtvaranjaDo)
                {
                    filtrirani.Add(item);
                    ViewBag.stariTreninzi = filtrirani;
                }
            }
            */
            #endregion


            return View("Index");
        }
        #endregion

    }
}