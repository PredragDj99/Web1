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
            Korisnik user = (Korisnik)Session["user"];
            if(user != null)
            {
                ViewBag.uspesnaPrijava = "jeste";
            }

            List<FitnesCentar> fitnesCentri = PodaciTxt.procitajFitnesCentre("~/App_Data/FitnesCentri.txt");

            List<FitnesCentar> sortiraniLista = new List<FitnesCentar>();
            sortiraniLista = fitnesCentri.OrderBy(f => f.Naziv).ToList();

            ViewBag.fitnesCentri = sortiraniLista;

            return View();
        }

        #region Pretraga
                                                //Nullable int - nije unet u formi
        public ActionResult Pretraga(string naziv,string adresa,int? godinaOtvaranjaOd,int? godinaOtvaranjaDo)
        {
            Korisnik user = (Korisnik)Session["user"];
            if (user != null)
            {
                ViewBag.uspesnaPrijava = "jeste";
            }

            List<FitnesCentar> fitnesCentri = PodaciTxt.procitajFitnesCentre("~/App_Data/FitnesCentri.txt");
            List<FitnesCentar> filtrirani = new List<FitnesCentar>();

            string godinaOd = godinaOtvaranjaOd.ToString();
            string godinaDo = godinaOtvaranjaDo.ToString();

            #region Kombinovana pretraga
            foreach (var item in fitnesCentri)
            {
                if (naziv.Equals("") && adresa.Equals("") && godinaOd.Equals("") && godinaDo.Equals(""))
                {
                    ViewBag.fitnesCentri = fitnesCentri;
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
            #endregion

            return View("Index");
        }
        #endregion

        #region Sort
        public ActionResult Sortiraj(string submit, string sort)
        {
            Korisnik user = (Korisnik)Session["user"];
            if (user != null)
            {
                ViewBag.uspesnaPrijava = "jeste";
            }

            List<FitnesCentar> fitnesCentri = PodaciTxt.procitajFitnesCentre("~/App_Data/FitnesCentri.txt");
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
        #endregion

        //Nakon klika na detalje ukoliko je logovan ide se na POSETILAC/TRENER/VLASNIK
        #region Detalji o fitnes centru
        //treba, forma za link da bih prikupio ime
        public ActionResult Detalji(string naziv) //naziv fitnes centra
        {
            Korisnik user = (Korisnik)Session["user"];
            if (user != null)
            {
                ViewBag.uspesnaPrijava = "jeste";
            }

            List<FitnesCentar> fitnesCentri = PodaciTxt.procitajFitnesCentre("~/App_Data/FitnesCentri.txt");

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
                if (item.FitnesCentarOdrzava.Naziv.ToString().Equals(naziv) && item.DatumIVremeTreninga>DateTime.Now)
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

            //AKO JE KORISNIK PRIJAVLJEN KAO POSETILAC PRELAZIMO NA NJEGOVU STRANICU
            if (user != null)
            {
                if(user.Uloga.ToString() == "POSETILAC")
                {
                    return RedirectToAction("Index", "Posetilac", new { naziv });
                }
                else if (user.Uloga.ToString() == "TRENER")
                {
                    return RedirectToAction("Index", "Trener", new { naziv });
                }
                else if(user.Uloga.ToString() == "VLASNIK")
                {
                    return RedirectToAction("Index", "Vlasnik", new { naziv });
                }
            }

            return View("Detalji");
        }
        #endregion

        #region Registracija
        public ActionResult Registracija(Korisnik korisnik)
        {
            Korisnik user = (Korisnik)Session["user"];
            if (user != null)
            {
                ViewBag.uspesnaPrijava = "jeste";
            }

            //za slucaj da pukne registracija imam default view
            List<FitnesCentar> fitnesCentri = PodaciTxt.procitajFitnesCentre("~/App_Data/FitnesCentri.txt");
            List<FitnesCentar> sortiraniLista = new List<FitnesCentar>();
            sortiraniLista = fitnesCentri.OrderBy(f => f.Naziv).ToList();
            ViewBag.fitnesCentri = sortiraniLista;
            //kraj

            List<Korisnik> registrovaniKorisnici = PodaciTxt.procitajKorisnike("~/App_Data/Korisnici.txt");

            //ako korisnicko ime vec postoji
            foreach (Korisnik kor in registrovaniKorisnici)
            {
                if (kor.KorisnickoIme == korisnik.KorisnickoIme)
                {
                    ViewBag.korisnik = $"Korisnik sa imenom {korisnik.KorisnickoIme} vec postoji";
                    return View("Index");
                }
            }

            #region Validacija
            //provera da li je sve popunjeno
            if (korisnik.KorisnickoIme == null)
            {
                ViewBag.korisnik = "Popunite sva polja. Morate popuniti polje za korisnicko ime";
                return View("Index");
            }
            if (korisnik.Lozinka == null)
            {
                ViewBag.korisnik = "Popunite sva polja. Morate popuniti polje za lozinku";
                return View("Index");
            }
            if (korisnik.Ime == null)
            {
                ViewBag.korisnik = "Popunite sva polja. Morate popuniti polje za ime";
                return View("Index");
            }
            if (korisnik.Prezime == null)
            {
                ViewBag.korisnik = "Popunite sva polja. Morate popuniti polje za prezime";
                return View("Index");
            }
            if (korisnik.Pol == null)
            {
                ViewBag.korisnik = "Popunite sva polja. Morate popuniti polje za pol";
                return View("Index");
            }
            if (korisnik.Email == null)
            {
                ViewBag.korisnik = "Popunite sva polja. Morate popuniti polje za email";
                return View("Index");
            }
            if (korisnik.DatumRodjenja == null || korisnik.DatumRodjenja.ToString("dd/MM/yyyy")=="01/01/0001")
            {
                ViewBag.korisnik = "Popunite sva polja. Morate popuniti polje za datum rodjenja";
                return View("Index");
            }

            //ogranicenja
            if (korisnik.KorisnickoIme.Length < 3)
            {
                ViewBag.korisnik = "Korisnicko ime mora biti imati bar 3 karaktera";
                return View("Index");
            }
            if (korisnik.Lozinka.Length < 4)
            {
                ViewBag.korisnik = "Lozinka mora imati bar 5 karaktera";
                return View("Index");
            }
            #endregion

            korisnik.Uloga = (KorisnikType)Enum.ToObject(typeof(KorisnikType),0);
            korisnik.ListaGrupnihTreninga = new List<GrupniTrening>();
            korisnik.ListaTreninziAngazovan = new List<GrupniTrening>();
            korisnik.AngazovanNaFitnesCentar = new FitnesCentar();
            korisnik.ListaVlasnickiFitnesCentar = new List<FitnesCentar>();

            registrovaniKorisnici.Add(korisnik);
            PodaciTxt.SacuvajKorisnika(korisnik);

            ViewBag.korisnik = "Uspesno ste se registrovali!";
            return View("Index");
        }
        #endregion

        #region Prijava
        public ActionResult Prijava(string korisnickoIme,string lozinka)
        {
            //za slucaj da pukne prijava imam default view
            List<FitnesCentar> fitnesCentri = PodaciTxt.procitajFitnesCentre("~/App_Data/FitnesCentri.txt");
            List<FitnesCentar> sortiraniLista = new List<FitnesCentar>();
            sortiraniLista = fitnesCentri.OrderBy(f => f.Naziv).ToList();
            ViewBag.fitnesCentri = sortiraniLista;
            //kraj

            List<Korisnik> registrovaniKorisnici = PodaciTxt.procitajKorisnike("~/App_Data/Korisnici.txt");
            ViewBag.uspesnaPrijava = "nije";

            List<FitnesCentar> sviFC = PodaciTxt.procitajFitnesCentre("~/App_Data/FitnesCentri.txt");
            List<FitnesCentar> obrisaniFC = new List<FitnesCentar>();
            foreach (var item in sviFC)
            {
                if (item.ObrisanFC == "ObrisanFC")
                {
                    obrisaniFC.Add(item);
                }
            }

            //ako korisnicko ime vec postoji
            foreach (Korisnik kor in registrovaniKorisnici)
            {
                //VLASNIK BLOKIRA TRENERA
                if (kor.KorisnickoIme == korisnickoIme && kor.Lozinka == lozinka && kor.TrenerBlokiran == "BLOKIRAN")
                {
                    ViewBag.prijavljen = "Vlasnik Vas je blokirao!";
                    break;
                }

                //Blokiranje trenera ako je FC obrisan
                foreach (var item in obrisaniFC)
                {
                    if(item.Naziv == kor.AngazovanNaFitnesCentar.Naziv && korisnickoIme==kor.KorisnickoIme)
                    {
                        ViewBag.prijavljen = "Fitnes centar u kome radite je obrisan!";
                        return View("Index");
                    }
                }

                if (kor.KorisnickoIme == korisnickoIme && kor.Lozinka==lozinka)
                {
                    ViewBag.uspesnaPrijava = "jeste";
                    Session["user"] = kor;
                    return View("Index");
                }
            }

            if(ViewBag.prijavljen != "Vlasnik Vas je blokirao!")
                ViewBag.prijavljen = "Niste uneli dobre podatke";

            return View("Index");
        }
        #endregion

        #region Logout
        public ActionResult Logout()
        {
            //za slucaj da pukne imam default view
            List<FitnesCentar> fitnesCentri = PodaciTxt.procitajFitnesCentre("~/App_Data/FitnesCentri.txt");
            List<FitnesCentar> sortiraniLista = new List<FitnesCentar>();
            sortiraniLista = fitnesCentri.OrderBy(f => f.Naziv).ToList();
            ViewBag.fitnesCentri = sortiraniLista;
            //kraj

            ViewBag.uspesnaPrijava = "nije";
            Session["user"] = null;

            return View("Index");
        }
        #endregion

        #region Uredi profil
        public ActionResult UrediProfil()
        {
            Korisnik user = (Korisnik)Session["user"];
            if (user != null)
            {
                ViewBag.uspesnaPrijava = "jeste";
            }

            ViewBag.trenutniProfil = user;

            return View("Profil");
        }
        #endregion

        #region Izmeni profil
        public ActionResult IzmeniProfil(Korisnik korisnik)
        {
            Korisnik user = (Korisnik)Session["user"];
            List<Korisnik> registrovaniKorisnici = PodaciTxt.procitajKorisnike("~/App_Data/Korisnici.txt");

            ViewBag.trenutniProfil = user;

            //ako korisnicko ime vec postoji
            foreach (Korisnik kor in registrovaniKorisnici)
            {
                if (kor.KorisnickoIme == korisnik.KorisnickoIme && kor.KorisnickoIme!=user.KorisnickoIme) //ako je zauzeto, a nije trenutni korisnik
                {
                    ViewBag.korisnik = $"Korisnik sa imenom {korisnik.KorisnickoIme} vec postoji";
                    return View("Profil");
                }
            }

            #region Validacija
            //provera da li je sve popunjeno
            if (korisnik.KorisnickoIme == null)
            {
                ViewBag.korisnik = "Popunite sva polja. Morate popuniti polje za korisnicko ime";
                return View("Profil");
            }
            if (korisnik.Lozinka == null)
            {
                ViewBag.korisnik = "Popunite sva polja. Morate popuniti polje za lozinku";
                return View("Profil");
            }
            if (korisnik.Ime == null)
            {
                ViewBag.korisnik = "Popunite sva polja. Morate popuniti polje za ime";
                return View("Profil");
            }
            if (korisnik.Prezime == null)
            {
                ViewBag.korisnik = "Popunite sva polja. Morate popuniti polje za prezime";
                return View("Profil");
            }
            if (korisnik.Pol == null)
            {
                ViewBag.korisnik = "Popunite sva polja. Morate popuniti polje za pol";
                return View("Profil");
            }
            if (korisnik.Email == null)
            {
                ViewBag.korisnik = "Popunite sva polja. Morate popuniti polje za email";
                return View("Profil");
            }
            if (korisnik.DatumRodjenja == null || korisnik.DatumRodjenja.ToString() == "1/1/0001 00:00:00")
            {
                ViewBag.korisnik = "Popunite sva polja. Morate popuniti polje za datum rodjenja";
                return View("Profil");
            }

            //ogranicenja
            if (korisnik.KorisnickoIme.Length < 3)
            {
                ViewBag.Message = "Korisnicko ime mora biti imati bar 3 karaktera";
                return View("Profil");
            }
            if (korisnik.Lozinka.Length < 4)
            {
                ViewBag.Message = "Lozinka mora imati bar 5 karaktera";
                return View("Profil");
            }
            #endregion
            korisnik.Uloga = (KorisnikType)Enum.ToObject(typeof(KorisnikType), 0);
            if (user.ListaGrupnihTreninga != null)
            {
                foreach (var item in user.ListaGrupnihTreninga)
                {
                    korisnik.ListaGrupnihTreninga.Add(item);
                }
            }
            if (user.ListaTreninziAngazovan != null)
            {
                foreach (var item in user.ListaTreninziAngazovan)
                {
                    korisnik.ListaTreninziAngazovan.Add(item);
                }
            }
            if (user.ListaVlasnickiFitnesCentar != null)
            {
                foreach (var item in user.ListaVlasnickiFitnesCentar)
                {
                    korisnik.ListaVlasnickiFitnesCentar.Add(item);
                }
            }
            //brisem starog
            registrovaniKorisnici.Remove(user);
            //dodajem novog
            registrovaniKorisnici.Add(korisnik);
            PodaciTxt.IzmeniKorisnika(korisnik, user); //izmenikorisnika
            
            //novi podaci
            Session["user"] = korisnik;

            ViewBag.izmena = "Uspesno izmenjeni detalji profila!";
            ViewBag.trenutniProfil = korisnik;

            return View("Profil");
        }
        #endregion
    }
}