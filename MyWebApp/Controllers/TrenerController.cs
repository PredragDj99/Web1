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

                //za pretragu
                List<FitnesCentar> sviFC= PodaciTxt.procitajFitnesCentre("~/App_Data/FitnesCentri.txt");
                foreach (var jedanFC in sviFC)
                {
                    if (jedanFC.Naziv == trebajuMiOviPodaci.FitnesCentarOdrzava.Naziv)
                    {
                        item.FitnesCentarOdrzava.GodinaOtvaranja = jedanFC.GodinaOtvaranja;
                        break;
                    }
                }
                
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
                    ViewBag.spisakPosetilaca = item.SpisakPosetilaca;
                }
            }
            ViewBag.naziv = naziv;

            return View("SpisakPosetilaca");
        }
        #endregion

        #region Dodaj trening
        static string naz = "";
        public ActionResult DodajTrening(string naziv)
        {
            ViewBag.naziv = naziv;
            naz = naziv;
            return View("Dodavanje");
        }

        public ActionResult DodajTreningTxt(string naziv, string tipTreninga, string trajanjeTreningaMinute, string datumIVremeTreninga, int? maksimalanBrojPosetioca)
        {
            ViewBag.naziv = naz;
            Korisnik user = (Korisnik)Session["user"];

            trajanjeTreningaMinute = trajanjeTreningaMinute + " minuta";
            string prazniPosetioci = "";
            string treningAktivan = "AKTIVAN";
            string maxBrojPosetioca = maksimalanBrojPosetioca.ToString();

            #region 3 dana unapred
            //termin novog treninga
            string[] datumVreme = datumIVremeTreninga.Split('T');
            string d = datumVreme[0];
            string[] date = d.Split('-');
            datumIVremeTreninga = date[2]+"/"+date[1]+ "/"+date[0] +" "+ datumVreme[1];
            //trenutni
            DateTime trenutni = DateTime.Now;
            string danasnjiDatum = trenutni.ToString();
            danasnjiDatum = danasnjiDatum.Substring(0, danasnjiDatum.Length-3);
            string[] datumVreme2 = danasnjiDatum.ToString().Split(' ');
            string d2 = datumVreme2[0];
            string[] date2 = d2.Split('/');
            int dan = Int32.Parse(date2[1]);        //provera da li je datum 3 dana kasnije
            dan = dan + 3;
            date2[1] = dan.ToString();
            if (date2[0].Count() == 1)
            {
                date2[0] = "0" + date2[0];
            }
            if (date2[1].Count() == 1)
            {
                date2[1] = "0" + date2[1];
            }
            string uporedi = date2[1] + "/" + date2[0] + "/" + date2[2] + " " + datumVreme2[1];

            //provera da li je datum 3 dana kasnije
            DateTime zakazano = DateTime.ParseExact(datumIVremeTreninga, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            DateTime moraBitiBar = DateTime.ParseExact(uporedi, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            if (zakazano < moraBitiBar)
            {
                ViewBag.dodajTrening = "Datum mora biti bar 3 dana unapred";
                return View("Dodavanje");
            }
                
            #endregion

            #region Provera ima li praznih unosa
            //provera da li je sve popunjeno
            if (naziv == "")
            {
                ViewBag.dodajTrening = "Popunite sva polja. Morate popuniti polje za naziv grupe";
                return View("Dodavanje");
            }
            if (tipTreninga == "")
            {
                ViewBag.dodajTrening = "Popunite sva polja. Morate popuniti polje za tip treninga";
                return View("Dodavanje");
            }
            if (trajanjeTreningaMinute == " minuta" || trajanjeTreningaMinute.StartsWith("-") || trajanjeTreningaMinute.StartsWith("0"))
            {
                ViewBag.dodajTrening = "Popunite sva polja. Morate popuniti polje za trajanje treninga";
                return View("Dodavanje");
            }
            if (datumIVremeTreninga == "")
            {
                ViewBag.dodajTrening = "Popunite sva polja. Morate popuniti polje za datum i vreme treninga";
                return View("Dodavanje");
            }
            if (maksimalanBrojPosetioca == null || maksimalanBrojPosetioca.ToString().StartsWith("0") || maksimalanBrojPosetioca.ToString().StartsWith("-"))
            {
                ViewBag.dodajTrening = "Popunite sva polja. Morate popuniti polje za max broj posetioca";
                return View("Dodavanje");
            }
            #endregion

            //Dodaj u treninge
            string postoji = PodaciTxt.TrenerDodajeNoviTrening(naziv,tipTreninga,user.AngazovanNaFitnesCentar.Naziv,trajanjeTreningaMinute,datumIVremeTreninga, maxBrojPosetioca, prazniPosetioci,treningAktivan);
            if(postoji=="vec postoji")
            {
                ViewBag.dodajTrening = "Termin je zauzet";
                return View("Dodavanje");
            }
            //Idi u listu korisnika i dopisi ga treneru
            string lepoFormatiranDatumRodjenja = PodaciTxt.pronadjiDatumRodjenjaKorisnika(user.KorisnickoIme);
            PodaciTxt.DodajGrupniTreningTreneru(user,lepoFormatiranDatumRodjenja,naziv,datumIVremeTreninga);

            ViewBag.dodajTrening = "Trening uspesno napravljen!";
            
            return View("Dodavanje");
        }
        #endregion

        #region Modifikuj trening
        static string nazi = "";
        public ActionResult ModifikujTrening(string naziv)
        {
            ViewBag.naziv = naziv;
            nazi = naziv;
            return View("Modifikuj");
        }
        public ActionResult ModifikujTreningTxt(string naziv, string tipTreninga, string trajanjeTreningaMinute, string datumIVremeTreninga, int? maksimalanBrojPosetioca)
        {
            ViewBag.naziv = nazi;
            ViewBag.modifikujTrening = "Trening uspesno modifikovan!";

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

            List<GrupniTrening> filtrirani = new List<GrupniTrening>();
            //Po godini otvaranja znaci da pristupa svakom treningu, gleda njegov fitnes centar i kada je on otvoren
            string godinaOd = godinaOtvaranjaOd.ToString();
            string godinaDo = godinaOtvaranjaDo.ToString();

            //naziv imam vec
            //item.TipTreninga -> zameniti sa adresom
            //item.FitnesCentarOdrzava.GodinaOtvaranja -> godina otvaranja

            #region Kombinovana pretraga
            foreach (var item in stari)
            {
                if (naziv.Equals("") && tipTreninga.Equals("") && godinaOd.Equals("") && godinaDo.Equals(""))
                {
                    ViewBag.stariTreninzi = stari;
                }
                //Prazan naziv
                else if (naziv.Equals(""))
                {
                    if (tipTreninga.Equals(""))
                    {
                        if (godinaOd.Equals(""))
                        {
                            if (item.FitnesCentarOdrzava.GodinaOtvaranja <= godinaOtvaranjaDo)
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
                                if (item.FitnesCentarOdrzava.GodinaOtvaranja >= godinaOtvaranjaOd)
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
                                if (item.FitnesCentarOdrzava.GodinaOtvaranja >= godinaOtvaranjaOd && item.FitnesCentarOdrzava.GodinaOtvaranja <= godinaOtvaranjaDo)
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
                            if (item.TipTreninga.Equals(tipTreninga))
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
                            if (item.TipTreninga.Equals(tipTreninga) && item.FitnesCentarOdrzava.GodinaOtvaranja <= godinaOtvaranjaDo)
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
                            if (item.TipTreninga.Equals(tipTreninga))
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
                            if (item.TipTreninga.Equals(tipTreninga) && item.FitnesCentarOdrzava.GodinaOtvaranja >= godinaOtvaranjaOd)
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
                else if (tipTreninga.Equals(""))
                {
                    if (naziv.Equals(""))
                    {
                        if (godinaOd.Equals(""))
                        {
                            if (item.FitnesCentarOdrzava.GodinaOtvaranja <= godinaOtvaranjaDo)
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
                                if (item.FitnesCentarOdrzava.GodinaOtvaranja >= godinaOtvaranjaOd)
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
                                if (item.FitnesCentarOdrzava.GodinaOtvaranja >= godinaOtvaranjaOd && item.FitnesCentarOdrzava.GodinaOtvaranja <= godinaOtvaranjaDo)
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
                            if (item.FitnesCentarOdrzava.Naziv.Equals(naziv))
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
                            if (item.FitnesCentarOdrzava.Naziv.Equals(naziv) && item.FitnesCentarOdrzava.GodinaOtvaranja <= godinaOtvaranjaDo)
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
                            if (item.FitnesCentarOdrzava.Naziv.Equals(naziv))
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
                            if (item.FitnesCentarOdrzava.Naziv.Equals(naziv) && item.FitnesCentarOdrzava.GodinaOtvaranja >= godinaOtvaranjaOd)
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
                        if (tipTreninga.Equals(""))
                        {
                            if (item.FitnesCentarOdrzava.GodinaOtvaranja <= godinaOtvaranjaDo)
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
                                if (item.TipTreninga.Equals(tipTreninga))
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
                                if (item.TipTreninga.Equals(tipTreninga) && item.FitnesCentarOdrzava.GodinaOtvaranja <= godinaOtvaranjaDo)
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
                        if (tipTreninga.Equals(""))
                        {
                            if (godinaDo.Equals(""))
                            {
                                if (item.FitnesCentarOdrzava.Naziv.Equals(naziv))
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
                                if (item.FitnesCentarOdrzava.Naziv.Equals(naziv) && item.FitnesCentarOdrzava.GodinaOtvaranja <= godinaOtvaranjaDo)
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
                                if (item.FitnesCentarOdrzava.Naziv.Equals(naziv) && item.TipTreninga.Equals(tipTreninga))
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
                                if (item.FitnesCentarOdrzava.Naziv.Equals(naziv) && item.TipTreninga.Equals(tipTreninga) && item.FitnesCentarOdrzava.GodinaOtvaranja <= godinaOtvaranjaDo)
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
                        if (tipTreninga.Equals(""))
                        {
                            if (item.FitnesCentarOdrzava.GodinaOtvaranja >= godinaOtvaranjaOd)
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
                                if (item.TipTreninga.Equals(tipTreninga))
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
                                if (item.TipTreninga.Equals(tipTreninga) && item.FitnesCentarOdrzava.GodinaOtvaranja >= godinaOtvaranjaOd)
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
                        if (tipTreninga.Equals(""))
                        {
                            if (godinaOd.Equals(""))
                            {
                                if (item.FitnesCentarOdrzava.Naziv.Equals(naziv))
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
                                if (item.FitnesCentarOdrzava.Naziv.Equals(naziv) && item.FitnesCentarOdrzava.GodinaOtvaranja >= godinaOtvaranjaOd)
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
                                if (item.FitnesCentarOdrzava.Naziv.Equals(naziv) && item.TipTreninga.Equals(tipTreninga))
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
                                if (item.FitnesCentarOdrzava.Naziv.Equals(naziv) && item.TipTreninga.Equals(tipTreninga) && item.FitnesCentarOdrzava.GodinaOtvaranja >= godinaOtvaranjaOd)
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
                else if (item.FitnesCentarOdrzava.Naziv == naziv && item.TipTreninga == tipTreninga && item.FitnesCentarOdrzava.GodinaOtvaranja >= godinaOtvaranjaOd && item.FitnesCentarOdrzava.GodinaOtvaranja <= godinaOtvaranjaDo)
                {
                    filtrirani.Add(item);
                    ViewBag.stariTreninzi = filtrirani;
                }
            }
            #endregion


            return View("Index");
        }
        #endregion

    }
}