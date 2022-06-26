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

            List<Korisnik>svi = PodaciTxt.procitajKorisnike("~/App_Data/Korisnici.txt");
            foreach (var item in svi)
            {
                if (item.KorisnickoIme == korisnickoIme)
                {
                    item.TrenerBlokiran = "BLOKIRAN";
                }
            }

            return RedirectToAction("Index", new { naziv });
        }
        #endregion

        #region Registracija trenera
        public ActionResult RegistracijaTrenera(Korisnik trener)
        {
            #region Isto kao home controller -> stranica detalji
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
                if (item.FitnesCentarKomentarisan.ToString().Equals(naz))
                {
                    filtriraniKomentari.Add(item);
                }
            }
            ViewBag.naziv = naz;
            ViewBag.komentari = filtriraniKomentari;

            //grupni treninzi ovog fitnes centra
            foreach (var item in grupniTreninzi)
            {
                if (item.FitnesCentarOdrzava.Naziv.ToString().Equals(naz) && item.DatumIVremeTreninga > DateTime.Now)
                {
                    listaGrupnihTreninga.Add(item);
                }
            }
            ViewBag.grupniTreninzi = listaGrupnihTreninga;

            //detalji za ovaj fitnes centar
            foreach (var fc in fitnesCentri)
            {
                if (fc.Naziv.ToString().Equals(naz))
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

            //samim tim ne moze da radi na 2 fitnes centra
            foreach (var item in zaposleniTreneri)
            {
                if (item.KorisnickoIme == trener.KorisnickoIme)
                {
                    ViewBag.korisnik = "Ovaj trener je vec registrovan";
                    return View("Index");
                }
            }
            List<Korisnik> kor = PodaciTxt.procitajKorisnike("~/App_Data/Korisnici.txt");
            foreach (var item in kor)
            {
                if(item.KorisnickoIme == trener.KorisnickoIme)
                {
                    ViewBag.korisnik = "Ovaj trener je vec registrovan u fitnes centru drugog vlasnika";
                    return View("Index");
                }
            }

            #region Validacija
            //provera da li je sve popunjeno
            if (trener.KorisnickoIme == null)
            {
                ViewBag.korisnik = "Popunite sva polja. Morate popuniti polje za korisnicko ime";
                return View("Index");
            }
            if (trener.Lozinka == null)
            {
                ViewBag.korisnik = "Popunite sva polja. Morate popuniti polje za lozinku";
                return View("Index");
            }
            if (trener.Ime == null)
            {
                ViewBag.korisnik = "Popunite sva polja. Morate popuniti polje za ime";
                return View("Index");
            }
            if (trener.Prezime == null)
            {
                ViewBag.korisnik = "Popunite sva polja. Morate popuniti polje za prezime";
                return View("Index");
            }
            if (trener.Pol == null)
            {
                ViewBag.korisnik = "Popunite sva polja. Morate popuniti polje za pol";
                return View("Index");
            }
            if (trener.Email == null)
            {
                ViewBag.korisnik = "Popunite sva polja. Morate popuniti polje za email";
                return View("Index");
            }
            if (trener.DatumRodjenja == null)
            {
                ViewBag.korisnik = "Popunite sva polja. Morate popuniti polje za datum rodjenja";
                return View("Index");
            }
            if (trener.AngazovanNaFitnesCentar.Naziv ==null)
            {
                ViewBag.korisnik = "Popunite sva polja. Inesite naziv vaseg fitnes centra";
                return View("Index");
            }

            //ogranicenja
            if (trener.KorisnickoIme.Length < 3)
            {
                ViewBag.korisnik = "Korisnicko ime mora biti imati bar 3 karaktera";
                return View("Index");
            }
            if (trener.Lozinka.Length < 4)
            {
                ViewBag.korisnik = "Lozinka mora imati bar 5 karaktera";
                return View("Index");
            }
            #endregion

            trener.Uloga = (KorisnikType)Enum.ToObject(typeof(KorisnikType), 1);

            foreach (var item in vlasnickiFC)
            {
                if (item.Naziv == trener.AngazovanNaFitnesCentar.Naziv)
                    trener.AngazovanNaFitnesCentar = item;
            }
            if (trener.AngazovanNaFitnesCentar.MesecnaClanarina == 0)
            {
                ViewBag.korisnik = "Ovaj fitnes centar nije vas!";
                return View("Index");
            }
            trener.ListaGrupnihTreninga = new List<GrupniTrening>();
            trener.ListaTreninziAngazovan = new List<GrupniTrening>();
            trener.ListaVlasnickiFitnesCentar = new List<FitnesCentar>();
            trener.TrenerBlokiran = "Ne blokiran";

            ViewBag.registracija = "Trener uspesno registrovan";

            zaposleniTreneri.Add(trener);
            PodaciTxt.SacuvajKorisnika(trener);

            return View("Index");
        }
        #endregion

        #region Dodaj trening
        public ActionResult DodajFC()
        {
            ViewBag.naziv = naz;
            return View("DodavanjeFC");
        }

        public ActionResult DodajFCTxt(string naziv,string ulica,int? broj,string grad,int? postanskibr,string mesecnaClanarina,string godisnjaClanarina,string jedanTrening,string jedanGrupniTrening,string jedanSaPersonalnimTrenerom)
        {
            ViewBag.naziv = naz;
            Korisnik user = (Korisnik)Session["user"];

            FitnesCentar fitnesCentar = new FitnesCentar();

            ViewBag.upis = "Greska";

            #region Validacija
            if (naziv == "")
            {
                ViewBag.upis = "Morate popuniti sve podatke. Popunite polje za naziv";
                return View("DodavanjeFC");
            }
            else if (ulica == "")
            {
                ViewBag.upis = "Morate popuniti sve podatke. Popunite polje za ulicu";
                return View("DodavanjeFC");
            }
            else if (broj.ToString() == "" || broj.ToString().StartsWith("-") || broj.ToString().StartsWith("0"))
            {
                ViewBag.upis = "Morate popuniti sve podatke. Popunite polje za broj";
                return View("DodavanjeFC");
            }
            else if (postanskibr.ToString() == "" || postanskibr.ToString().StartsWith("-"))
            {
                ViewBag.upis = "Morate popuniti sve podatke. Popunite polje za postanki broj";
                return View("DodavanjeFC");
            }
            else if(mesecnaClanarina == "")
            {
                ViewBag.upis = "Morate popuniti sve podatke. Mesecna clanarina ne moze biti prazna";
                return View("DodavanjeFC");
            }
            else if(godisnjaClanarina == "")
            {
                ViewBag.upis = "Morate popuniti sve podatke. Godisnja clanarina ne moze biti prazna";
                return View("DodavanjeFC");
            }
            else if(jedanGrupniTrening == "")
            {
                ViewBag.upis = "Morate popuniti sve podatke. Cena grupnog treninga ne moze biti prazna";
                return View("DodavanjeFC");
            }
            else if (jedanTrening == "")
            {
                ViewBag.upis = "Morate popuniti sve podatke. Cena jednog treninga ne moze biti prazna";
                return View("DodavanjeFC");
            }
            else if (jedanSaPersonalnimTrenerom == "")
            {
                ViewBag.upis = "Morate popuniti sve podatke. Cena jednog treninga sa personalnim trenerom ne moze biti prazna";
                return View("DodavanjeFC");
            }


            double.TryParse(mesecnaClanarina, out double mesecnaClanarinaa);
            if (mesecnaClanarinaa == 0)
            {
                ViewBag.upis = "Morate popuniti sve podatke. Mesecna clanarina mora biti broj";
                return View("DodavanjeFC");
            }
            double.TryParse(godisnjaClanarina, out double godisnjaClanarinaa);
            if (godisnjaClanarinaa == 0)
            {
                ViewBag.upis = "Morate popuniti sve podatke. Mesecna godinja mora biti broj";
                return View("DodavanjeFC");
            }
            double.TryParse(jedanTrening, out double jedanTreningg);
            if (jedanTreningg == 0)
            {
                ViewBag.upis = "Morate popuniti sve podatke. Cena treninga mora biti broj";
                return View("DodavanjeFC");
            }
            double.TryParse(jedanGrupniTrening, out double jedanGrupniTreningg);
            if (jedanGrupniTreningg == 0)
            {
                ViewBag.upis = "Morate popuniti sve podatke. Cena grupnog treninga mora biti broj";
                return View("DodavanjeFC");
            }
            double.TryParse(jedanSaPersonalnimTrenerom, out double jedanSaPersonalnimTreneromm);
            if (jedanSaPersonalnimTreneromm == 0)
            {
                ViewBag.upis = "Morate popuniti sve podatke. Cena trenera sa personalnim trenerom mora biti broj";
                return View("DodavanjeFC");
            }

            List<FitnesCentar> sviFC = PodaciTxt.procitajFitnesCentre("~/App_Data/FitnesCentri.txt");
            foreach (var item in sviFC)
            {
                if(item.Adresa == ulica + " " + broj + ", " + grad + ", " + postanskibr)
                {
                    ViewBag.upis = "Ova adresa je zauzeta drugim fitnes centrom";
                    return View("DodavanjeFC");
                }
                if(item.Naziv == naziv)
                {
                    ViewBag.upis = "Vec postoji fitnes centar sa ovim nazivom";
                    return View("DodavanjeFC");
                }
            }
            #endregion

            fitnesCentar.Naziv = naziv;
            fitnesCentar.Adresa = ulica + " " + broj + ", " + grad + ", " + postanskibr;
            fitnesCentar.MesecnaClanarina = mesecnaClanarinaa;
            fitnesCentar.GodisnjaClanarina = godisnjaClanarinaa;
            fitnesCentar.JedanTrening = jedanTreningg;
            fitnesCentar.JedanGrupniTrening = jedanGrupniTreningg;
            fitnesCentar.JedanSaPersonalnimTrenerom = jedanSaPersonalnimTreneromm;
            fitnesCentar.GodinaOtvaranja = Int32.Parse(DateTime.Now.Year.ToString());
            fitnesCentar.Vlasnik = user;

            user.ListaVlasnickiFitnesCentar.Add(fitnesCentar);
            PodaciTxt.UpisiVlasniku(fitnesCentar.Naziv,user.KorisnickoIme);
            PodaciTxt.DodajFC(fitnesCentar);
            //user.ListaVlasnickiFitnesCentar.RemoveAt(1);

            ViewBag.upis = "Uspesno kreiran fitnes centar!";

            return View("DodavanjeFC");
        }
        #endregion

        #region Modifikuj trening
        public ActionResult ModifikujFC()
        {
            ViewBag.naziv = naz;

            Korisnik user = (Korisnik)Session["user"];

            List<FitnesCentar> vlasnikoviFC = new List<FitnesCentar>();
            foreach (var item in user.ListaVlasnickiFitnesCentar)
            {
                vlasnikoviFC.Add(item);
            }
            ViewBag.fitnesCentri = vlasnikoviFC;

            return View("ModifikovanjeFC");
        }
        public ActionResult ModifikujFCTxt(string staraAdresa, string naziv,string adresa,int? godinaOtvaranja,string mesecnaClanarina,string godisnjaClanarina,string jedanTrening,string jedanGrupniTrening,string jedanSaPersonalnimTrenerom)
        {
            ViewBag.naziv = naz;
            Korisnik user = (Korisnik)Session["user"];

            List<FitnesCentar> vlasnikoviFC = new List<FitnesCentar>();
            foreach (var item in user.ListaVlasnickiFitnesCentar)
            {
                vlasnikoviFC.Add(item);
            }
            ViewBag.fitnesCentri = vlasnikoviFC;

            #region Validacija
            if (naziv == "")
            {
                ViewBag.modifikujFC = "Morate popuniti sve podatke. Popunite polje za naziv";
                return View("ModifikovanjeFC");
            }
            else if(adresa == "" || !adresa.Contains(","))
            {
                ViewBag.modifikujFC = "Popunite polje za adresu u formatu 'ulica i broj, grad, postanski broj' ";
                return View("ModifikovanjeFC");
            }
            string[] podelaAdrese = adresa.Split(',');
            if (podelaAdrese[0] == "")
            {
                ViewBag.modifikujFC = "Popunite polje za adresu u formatu 'ulica i broj, grad, postanski broj' ";
                return View("ModifikovanjeFC");
            }
            else if (podelaAdrese[1] == "")
            {
                ViewBag.modifikujFC = "Popunite polje za adresu u formatu 'ulica i broj, grad, postanski broj' ";
                return View("ModifikovanjeFC");
            }
            else if (mesecnaClanarina == "")
            {
                ViewBag.modifikujFC = "Morate popuniti sve podatke. Mesecna clanarina ne moze biti prazna";
                return View("ModifikovanjeFC");
            }
            else if (godisnjaClanarina == "")
            {
                ViewBag.modifikujFC = "Morate popuniti sve podatke. Godisnja clanarina ne moze biti prazna";
                return View("ModifikovanjeFC");
            }
            else if (jedanGrupniTrening == "")
            {
                ViewBag.modifikujFC = "Morate popuniti sve podatke. Cena grupnog treninga ne moze biti prazna";
                return View("ModifikovanjeFC");
            }
            else if (jedanTrening == "")
            {
                ViewBag.modifikujFC = "Morate popuniti sve podatke. Cena jednog treninga ne moze biti prazna";
                return View("ModifikovanjeFC");
            }
            else if (jedanSaPersonalnimTrenerom == "")
            {
                ViewBag.modifikujFC = "Morate popuniti sve podatke. Cena jednog treninga sa personalnim trenerom ne moze biti prazna";
                return View("ModifikovanjeFC");
            }


            double.TryParse(mesecnaClanarina, out double mesecnaClanarinaa);
            if (mesecnaClanarinaa == 0)
            {
                ViewBag.modifikujFC = "Morate popuniti sve podatke. Mesecna clanarina mora biti broj";
                return View("ModifikovanjeFC");
            }
            double.TryParse(godisnjaClanarina, out double godisnjaClanarinaa);
            if (godisnjaClanarinaa == 0)
            {
                ViewBag.modifikujFC = "Morate popuniti sve podatke. Mesecna godinja mora biti broj";
                return View("ModifikovanjeFC");
            }
            double.TryParse(jedanTrening, out double jedanTreningg);
            if (jedanTreningg == 0)
            {
                ViewBag.modifikujFC = "Morate popuniti sve podatke. Cena treninga mora biti broj";
                return View("ModifikovanjeFC");
            }
            double.TryParse(jedanGrupniTrening, out double jedanGrupniTreningg);
            if (jedanGrupniTreningg == 0)
            {
                ViewBag.modifikujFC = "Morate popuniti sve podatke. Cena grupnog treninga mora biti broj";
                return View("ModifikovanjeFC");
            }
            double.TryParse(jedanSaPersonalnimTrenerom, out double jedanSaPersonalnimTreneromm);
            if (jedanSaPersonalnimTreneromm == 0)
            {
                ViewBag.modifikujFC = "Morate popuniti sve podatke. Cena trenera sa personalnim trenerom mora biti broj";
                return View("ModifikovanjeFC");
            }
            if(godinaOtvaranja.ToString().StartsWith("0") || godinaOtvaranja.ToString().StartsWith("-"))
            {
                ViewBag.modifikujFC = " Godina otvaranja ne moze da pocne sa 0 ili -";
                return View("ModifikovanjeFC");
            }
            #endregion

            PodaciTxt.ModifikujFC(user, staraAdresa, naziv, adresa, godinaOtvaranja, mesecnaClanarina, godisnjaClanarina, jedanTrening, jedanGrupniTrening, jedanSaPersonalnimTrenerom);
            foreach (var item in user.ListaVlasnickiFitnesCentar)
            {
                if(item.Adresa == staraAdresa)
                {
                    item.Naziv = naziv;
                    item.Adresa = adresa;
                    item.GodinaOtvaranja = Int32.Parse(godinaOtvaranja.ToString());
                    item.MesecnaClanarina = mesecnaClanarinaa;
                    item.GodisnjaClanarina = godisnjaClanarinaa;
                    item.JedanTrening = jedanTreningg;
                    item.JedanGrupniTrening = jedanGrupniTreningg;
                    item.JedanSaPersonalnimTrenerom = jedanSaPersonalnimTreneromm;
                }
            }
            ViewBag.modifikujFC ="Uspesno modifikovan fitnes centar";

            return View("ModifikovanjeFC");
        }
        #endregion

        #region Obrisi fitnes centar
        public ActionResult ObrisiFC(string adresaFC)
        {
            ViewBag.naziv = naz;
            string naziv = naz;
            Korisnik user = (Korisnik)Session["user"];

            List<FitnesCentar> vlasnikoviFC = new List<FitnesCentar>();
            foreach (var item in user.ListaVlasnickiFitnesCentar)
            {
                vlasnikoviFC.Add(item);
            }
            ViewBag.fitnesCentri = vlasnikoviFC;

            FitnesCentar fc = new FitnesCentar();
            foreach (var item in vlasnikoviFC)
            {
                if (item.Adresa == adresaFC)
                {
                    fc = item;
                }
            }

            List<GrupniTrening> sviGT = PodaciTxt.procitajGrupneTreninge("~/App_Data/GrupniTreninzi.txt");
            fc.ObrisanFC = "ObrisanFC";
            ViewBag.adresaNeobrisanog = adresaFC; //u ovom slucaju obrisanog
            foreach (var item in sviGT)
            {
                if (item.FitnesCentarOdrzava.Naziv == fc.Naziv)
                {
                    ViewBag.adresaNeobrisanog = adresaFC;
                    ViewBag.obrisanFC = "Nemoguce obrisati jer ima predstojecih treninga";
                    return RedirectToAction("Index", new {naziv});
                }
            }
            PodaciTxt.ObrisiFC(user, fc);

            //Treneru menjam u listi na Obrisan
            List<Korisnik> svi = PodaciTxt.procitajKorisnike("~/App_Data/Korisnici.txt");
            foreach (var item in svi)
            {
                if(item.AngazovanNaFitnesCentar.Naziv == fc.Naziv)
                {
                    item.AngazovanNaFitnesCentar.ObrisanFC = "ObrisanFC";
                }
            }

            return RedirectToAction("Index", new { naziv });
        }
        #endregion
    }
}