using MyWebApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public ActionResult PrijavaZaTrening(string naziv, string datumVreme,int maxPosetioca, int brojPosetioca,List<Korisnik> spisakPosetilaca,string tipTreninga,string nazivTreninga,string trajanjeMinute,string obris) //ne mogu vise parametara
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
                    string praviTip = tipTreninga.Substring(0, tipTreninga.Length-27);
                    string formatiranDatumTreninga = PodaciTxt.pronadjiDatumIVremeTreninga(nazivTreninga,praviTip,trajanjeMinute,naziv);
                    DateTime grupniTreningVreme = DateTime.ParseExact(formatiranDatumTreninga, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                    List<Korisnik> listaPosetilaca = new List<Korisnik>(); //Ovde ce mi biti svi koji prisustvuju

                    List<GrupniTrening> listaaa = PodaciTxt.procitajGrupneTreninge("~/App_Data/GrupniTreninzi.txt");
                    foreach (var item in listaaa)
                    {
                        if (item.DatumIVremeTreninga==grupniTreningVreme && item.TipTreninga == praviTip)
                        {
                            item.SpisakPosetilaca.Add(user);
                            listaPosetilaca = item.SpisakPosetilaca; 
                        }
                        else
                        {
                            listaPosetilaca = item.SpisakPosetilaca;
                        }
                    }


                    FitnesCentar fc = new FitnesCentar(naziv);
                    //treningu dodajem spisak posetioca
                    GrupniTrening gt = new GrupniTrening(nazivTreninga,tipTreninga,fc,trajanjeMinute, grupniTreningVreme,maxPosetioca, listaPosetilaca,obris);
                    //korisniku dodajem taj trening
                    user.ListaGrupnihTreninga.Add(gt);

                    //Dodajem korisnika na kraj liste(txt) za taj trening; proveravam da li je to taj trening
                    ViewBag.prijavljenTrening = "Prijavljeni ste";
                    PodaciTxt.DodajUGrupniTrening(user.Ime, user.Prezime, naziv, datumVreme);


                    string lepoFormatiran = PodaciTxt.pronadjiDatumRodjenjaKorisnika(user.KorisnickoIme);
                    PodaciTxt.DodajGrupniTreningKorisniku(user,gt,lepoFormatiran, praviTip, datumVreme);
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

            //Ovde vrsim kombinovanu pretragu
            List<GrupniTrening> filtrirani = new List<GrupniTrening>();
            List<GrupniTrening> konacniPrikaz = new List<GrupniTrening>();

            #region Pretrazi po kriterijumu
            //kod za pretragu
            foreach (var item in listaGrupnihTreninga)
            {
                if(naziv.Equals("") && fitnesCentarOdrzava.Equals("") && tipTreninga.Equals(""))
                {
                    konacniPrikaz = listaGrupnihTreninga;
                }
                //prazan naziv
                else if (naziv.Equals(""))
                {
                    if (fitnesCentarOdrzava.Equals(""))
                    {
                        if (item.TipTreninga == tipTreninga)
                        {
                            filtrirani.Add(item);
                        }
                    }
                    else if (tipTreninga.Equals(""))
                    {
                        if (item.FitnesCentarOdrzava.Naziv == fitnesCentarOdrzava)
                        {
                            filtrirani.Add(item);
                        }
                    }
                    else
                    {
                        if (item.FitnesCentarOdrzava.Naziv == fitnesCentarOdrzava && item.TipTreninga == tipTreninga)
                        {
                            filtrirani.Add(item);
                        }
                    }
                }
                //prazan fitnes centar
                else if (fitnesCentarOdrzava.Equals(""))
                {
                    if (naziv.Equals(""))
                    {
                        if (item.TipTreninga == tipTreninga)
                        {
                            filtrirani.Add(item);
                        }
                    }
                    else if (tipTreninga.Equals(""))
                    {
                        if (item.Naziv == naziv)
                        {
                            filtrirani.Add(item);
                        }
                    }
                    else
                    {
                        if (item.Naziv == naziv && item.TipTreninga == tipTreninga)
                        {
                            filtrirani.Add(item);
                        }
                    }
                }
                //prazan tip treninga
                else if (tipTreninga.Equals(""))
                {
                    if (naziv.Equals(""))
                    {
                        if (item.FitnesCentarOdrzava.Naziv == fitnesCentarOdrzava)
                        {
                            filtrirani.Add(item);
                        }
                    }
                    else if (fitnesCentarOdrzava.Equals(""))
                    {
                        if (item.Naziv == naziv)
                        {
                            filtrirani.Add(item);
                        }
                    }
                    else
                    {
                        if (item.Naziv == naziv && item.FitnesCentarOdrzava.Naziv == fitnesCentarOdrzava)
                        {
                            filtrirani.Add(item);
                        }

                    }
                }
                else if(item.Naziv == naziv && item.FitnesCentarOdrzava.Naziv==fitnesCentarOdrzava && item.TipTreninga == tipTreninga)
                {
                    filtrirani.Add(item);
                }
            }
            #endregion

            foreach (var item in listaGrupnihTreninga)
            {
                if (filtrirani.Contains(item))
                {
                    konacniPrikaz.Add(item);
                }
            }
            ViewBag.grupniTreninzi = konacniPrikaz;
            #endregion

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
            #endregion

            List<GrupniTrening> sortiraniGrupniTreninzi = new List<GrupniTrening>();
            if (sort.Equals("rastuce"))
            {
                switch (submit)
                {
                    case "naziv":
                        sortiraniGrupniTreninzi = listaGrupnihTreninga.OrderBy(f => f.Naziv).ToList();
                        break;
                    case "tipTreninga":
                        sortiraniGrupniTreninzi = listaGrupnihTreninga.OrderBy(f => f.TipTreninga).ToList();
                        break;
                    case "datumIVremeTreninga":
                        sortiraniGrupniTreninzi = listaGrupnihTreninga.OrderBy(f => f.DatumIVremeTreninga).ToList();
                        break;
                }
            }
            else if (sort.Equals("opadajuce"))
            {
                switch (submit)
                {
                    case "naziv":
                        sortiraniGrupniTreninzi = listaGrupnihTreninga.OrderByDescending(f => f.Naziv).ToList();
                        break;
                    case "tipTreninga":
                        sortiraniGrupniTreninzi = listaGrupnihTreninga.OrderByDescending(f => f.TipTreninga).ToList();
                        break;
                    case "datumIVremeTreninga":
                        sortiraniGrupniTreninzi = listaGrupnihTreninga.OrderByDescending(f => f.DatumIVremeTreninga).ToList();
                        break;
                }
            }
            else
            {
                ViewBag.grupniTreninzi = listaGrupnihTreninga;
            }
            ViewBag.grupniTreninzi = sortiraniGrupniTreninzi;


            return View("RanijiTreninzi");
        }
        #endregion

        #region Ostavi komentar
        public ActionResult OstaviKomentar(string fitnesCentarKomentarisan, int? ocena, string tekstKomentara)
        {
            #region Ucitavanje posecenih treninga
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

            string pamtiNaziv="nijeBio";

            bool bio = true;
            foreach (var item in listaGrupnihTreninga)
            {
                if (item.FitnesCentarOdrzava.Naziv.Equals(fitnesCentarKomentarisan))
                {
                    pamtiNaziv = fitnesCentarKomentarisan;
                }
            }
            //Moze da komentarise samo ako je bio i ako je uneo validnu ocenu
            if (pamtiNaziv.Equals("nijeBio"))
            {
                ViewBag.poslatNaPregled = "Moze komentarisati samo ukoliko ste bili u fitnes centru!";
                bio = false;
            }

            if (bio == true)
            {
                bool poslat = true;
                if (ocena == null)
                {
                    poslat = false;
                }
                if (poslat)
                {
                    if (!tekstKomentara.Equals(""))
                    {
                        if (!tekstKomentara.Contains(";"))
                        {
                            ViewBag.poslatNaPregled = "Vas komentar je uspesno poslat vlasniku!";
                            Int32 oc = (Int32)ocena;
                            PodaciTxt.SacuvajKomentar(user.Ime, fitnesCentarKomentarisan, oc, tekstKomentara);
                        }
                        else
                        {
                            ViewBag.poslatNaPregled = "Molimo Vas nemojte koristiti znak  ;  ";
                        }
                    }
                    else
                    {
                        ViewBag.poslatNaPregled = "Niste uneli tekst komentara";
                    }
                }
                else
                {
                    ViewBag.poslatNaPregled = "Potrebno je da ocena bude 1-5";
                }
            }


            return View("RanijiTreninzi");
        }
        #endregion
    }
}