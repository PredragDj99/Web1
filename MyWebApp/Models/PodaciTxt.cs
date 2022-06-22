using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

using System.Globalization;
using System.Text;

namespace MyWebApp.Models
{
    public class PodaciTxt
    {
        #region Procitaj fitnes centre
        public static List<FitnesCentar> procitajFitnesCentre(string path)
        {
            List<FitnesCentar> fitnesCentri = new List<FitnesCentar>();

            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";

            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split(';');
                string[] podaciOVlasniku = tokens[3].Split('|');
                Korisnik vlasnik = new Korisnik(podaciOVlasniku[0], podaciOVlasniku[1]);
                FitnesCentar fc = new FitnesCentar(tokens[0], tokens[1], Int32.Parse(tokens[2]), vlasnik, Double.Parse(tokens[4]), Double.Parse(tokens[5]), Double.Parse(tokens[6]), Double.Parse(tokens[7]), Double.Parse(tokens[8]));

                fitnesCentri.Add(fc);
            }

            sr.Close();
            stream.Close();
            
            return fitnesCentri;
        }
        #endregion

        #region Procitaj korisnike
        public static List<Korisnik> procitajKorisnike(string path)
        {
            List<Korisnik> korisnici = new List<Korisnik>();

            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";

            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split(';');

                //Svakako je null pri registraciji
                /*
                string[] listaGrupnihTreninga = tokens[8].Split('|');
                string[] listaTreninziAngazovan = tokens[9].Split('|');
                string[] AngazovanNaFitnesCentar = tokens[10].Split('|');
                string[] ListaVlasnickiFitnesCentar = tokens[11].Split('|');
                */
                //ovo sam odradio kod posetioca
                List <GrupniTrening> grupniTrening = new List<GrupniTrening>();

                List<GrupniTrening> treninziAngazovan = new List<GrupniTrening>();
                
                string[] listaTreninziAngazovan = tokens[9].Split('|');
                for (int i = 0; i < listaTreninziAngazovan.Count(); i++)
                {
                    if (tokens[7] == "TRENER")
                    {
                        string[] podeli = listaTreninziAngazovan[i].Split('_');
                        DateTime date = DateTime.ParseExact(podeli[1], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                        FitnesCentar nazivFitnesCentra = new FitnesCentar(tokens[10]);
                        //Prodji grupne treninge i za onaj koji ima ove parametre pokupi ostalo

                        GrupniTrening g = new GrupniTrening(podeli[0], date, nazivFitnesCentra);
                        treninziAngazovan.Add(g);
                    }
                }
                
                FitnesCentar fc = new FitnesCentar();
                List<FitnesCentar> listaFitnesCentara = new List<FitnesCentar>();
                
                Korisnik k = new Korisnik(tokens[0],tokens[1],tokens[2],tokens[3],tokens[4],tokens[5], DateTime.ParseExact(tokens[6], "dd/MM/yyyy", CultureInfo.InvariantCulture), (KorisnikType)Enum.Parse(typeof(KorisnikType), tokens[7]), grupniTrening,treninziAngazovan,fc,listaFitnesCentara);
                korisnici.Add(k);
            }

            sr.Close();
            stream.Close();

            return korisnici;
        }
        #endregion

        #region Procitaj grupne treninge
        public static List<GrupniTrening> procitajGrupneTreninge(string path)
        {
            List<GrupniTrening> grupniTreninzi = new List<GrupniTrening>();

            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";

            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split(';');

                FitnesCentar fc = new FitnesCentar(tokens[2]);

                string[] posetioci = tokens[6].Split('|');
                List<Korisnik> listaPosetilaca = new List<Korisnik>();

                foreach(var posetilac in posetioci)
                {
                    string[] korisnik = posetilac.Split('-');
                    if (korisnik[0] != "")
                    {
                        Korisnik k = new Korisnik(korisnik[0], korisnik[1]);
                        listaPosetilaca.Add(k);
                    }
                    else
                    {
                        Korisnik k = new Korisnik();    //Ako ne postoji ni jedan korisnik(slucaj kada trener brise)
                        //listaPosetilaca.Add(k);
                    }
                }

                GrupniTrening tr = new GrupniTrening(tokens[0],tokens[1],fc,tokens[3],DateTime.ParseExact(tokens[4], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),Int32.Parse(tokens[5]),listaPosetilaca,tokens[7]);

                grupniTreninzi.Add(tr);
            }

            sr.Close();
            stream.Close();

            return grupniTreninzi;
        }
        #endregion

        #region Procitaj komentare
        public static List<Komentar> procitajKomentare(string path)
        {
            List<Komentar> komentari = new List<Komentar>();

            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";

            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split(';');

                Komentar kom = new Komentar(tokens[0],tokens[1],tokens[2],Int32.Parse(tokens[3]),tokens[4]);
                if (tokens[4] == "odobren")
                {
                    komentari.Add(kom);
                }
            }

            sr.Close();
            stream.Close();

            return komentari;
        }
        #endregion

        #region Sacuvaj korisnika - nakon unosa
        public static void SacuvajKorisnika(Korisnik korisnik)
        {
            //sacuvaj korisnika u korisnici.txt
            var path = HostingEnvironment.MapPath("~/App_Data/Korisnici.txt");
            FileStream stream = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(stream);

            string[] linija = new string[12];

            linija[0] = korisnik.KorisnickoIme;
            linija[1] = korisnik.Lozinka;
            linija[2] = korisnik.Ime;
            linija[3] = korisnik.Prezime;
            linija[4] = korisnik.Pol;
            linija[5] = korisnik.Email;
            linija[6] = korisnik.DatumRodjenja.ToString("dd/MM/yyyy");
            linija[7] = korisnik.Uloga.ToString();

            string prazno = "";
            if (korisnik.ListaGrupnihTreninga == null)
            {
                linija[8] = prazno;
            }
            else
            {
                linija[8] = korisnik.ListaGrupnihTreninga.ToString();
            }

            if (korisnik.ListaTreninziAngazovan == null)
            {
                linija[9] = prazno;
            }
            else
            {
                linija[9] = korisnik.ListaTreninziAngazovan.ToString();
            }

            if (korisnik.AngazovanNaFitnesCentar == null)
            {
                linija[10] = prazno;
            }
            else
            {
                linija[10] = korisnik.AngazovanNaFitnesCentar.ToString();
            }

            if (korisnik.ListaVlasnickiFitnesCentar == null)
            {
                linija[11] = prazno;
            }
            else
            {
                linija[11] = korisnik.ListaVlasnickiFitnesCentar.ToString();
            }

            for (int i = 0; i < 12; i++)
            {
                sw.Write(linija[i]);
                if (i == 11)
                {
                    sw.WriteLine();
                }
                else
                {
                    sw.Write(";");
                }
            }

            sw.Close();
            stream.Close();
        }
        #endregion

        #region Izmeni profil korisnika
        public static void IzmeniKorisnika(Korisnik korisnik, Korisnik preIzmeneKorisnik)
        {
            #region Brisanje starih podataka

            //Brisanje starog
            string tempFile2 = Path.GetTempFileName();

            var path3 = HostingEnvironment.MapPath("~/App_Data/Korisnici.txt");
            FileStream fsRead2 = new FileStream(path3, FileMode.Open, FileAccess.Read);
            StreamReader sr2 = new StreamReader(fsRead2, Encoding.UTF8);

            string[] stariKorisnik = new string[8];

            stariKorisnik[0] = preIzmeneKorisnik.KorisnickoIme;
            stariKorisnik[1] = preIzmeneKorisnik.Lozinka;
            stariKorisnik[2] = preIzmeneKorisnik.Ime;
            stariKorisnik[3] = preIzmeneKorisnik.Prezime;
            stariKorisnik[4] = preIzmeneKorisnik.Pol;
            stariKorisnik[5] = preIzmeneKorisnik.Email;
            stariKorisnik[6] = preIzmeneKorisnik.DatumRodjenja.ToString("dd/MM/yyyy");

            string stariKorisnikLinija = stariKorisnik[0] + ";" + stariKorisnik[1] + ";" + stariKorisnik[2] + ";" + stariKorisnik[3] + ";" + stariKorisnik[4] + ";" + stariKorisnik[5] + ";" + stariKorisnik[6];

            using (var sww = new StreamWriter(tempFile2))
            {
                string line;

                while ((line = sr2.ReadLine()) != null)
                {
                    if (!line.Contains(stariKorisnikLinija)) //zbog ovoga brise liniju u kojoj se nalazi stari
                        sww.WriteLine(line);
                }
            }
            sr2.Close();
            fsRead2.Close();
            File.Delete(path3);
            File.Move(tempFile2, path3);
            #endregion

            //Ako ovo nemam obrisace mi tog jednog kog sam promenio
            #region Dodavanje novih podataka
            var path = HostingEnvironment.MapPath("~/App_Data/Korisnici.txt");
            FileStream stream = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(stream);

            string[] linija = new string[12];

            linija[0] = korisnik.KorisnickoIme;
            linija[1] = korisnik.Lozinka;
            linija[2] = korisnik.Ime;
            linija[3] = korisnik.Prezime;
            linija[4] = korisnik.Pol;
            linija[5] = korisnik.Email;
            linija[6] = korisnik.DatumRodjenja.ToString("dd/MM/yyyy");
            linija[7] = korisnik.Uloga.ToString();

            string prazno = "";
            if (korisnik.ListaGrupnihTreninga == null)
            {
                linija[8] = prazno;
            }
            else
            {
                linija[8] = korisnik.ListaGrupnihTreninga.ToString();
            }

            if (korisnik.ListaTreninziAngazovan == null)
            {
                linija[9] = prazno;
            }
            else
            {
                linija[9] = korisnik.ListaTreninziAngazovan.ToString();
            }

            if (korisnik.AngazovanNaFitnesCentar == null)
            {
                linija[10] = prazno;
            }
            else
            {
                linija[10] = korisnik.AngazovanNaFitnesCentar.ToString();
            }

            if (korisnik.ListaVlasnickiFitnesCentar == null)
            {
                linija[11] = prazno;
            }
            else
            {
                linija[11] = korisnik.ListaVlasnickiFitnesCentar.ToString();
            }

            for (int i = 0; i < 12; i++)
            {
                sw.Write(linija[i]);
                if (i == 11)
                {
                    sw.WriteLine();
                }
                else
                {
                    sw.Write(";");
                }
            }

            sw.Close();
            stream.Close();
            #endregion
        }
        #endregion

        //Ova 2 dodavanja idu zajedno
        #region Dodaj korisnika u grupni trening
        public static void DodajUGrupniTrening(string ime, string prezime, string naziv, string datumVreme) //ovo dodaje u txt
        {
            string tempFile = Path.GetTempFileName();

            var path = HostingEnvironment.MapPath("~/App_Data/GrupniTreninzi.txt");
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs, Encoding.UTF8);

            using (var sw = new StreamWriter(tempFile))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    string[] delovi = line.Split(';');
                    string[] posetioci = delovi[6].Split('|');

                    //sw.Write(line);
                    //Proveravam koji je to grupni trening i onda ga dodajem na kraj te linije
                    if (line.Contains(naziv) && line.Contains(datumVreme))
                    {
                        string novi = "|" + ime + "-" + prezime;
                        string proba = delovi[6] + novi;
                        delovi[6] = proba;
                    }

                    for (int i = 0; i < delovi.Length; i++)
                    {
                        sw.Write(delovi[i]);
                        if (i!=7)
                        {
                            sw.Write(';');
                        }
                    }

                    sw.WriteLine();
                }
            }
            sr.Close();
            fs.Close();
            File.Delete(path);
            File.Move(tempFile, path);
        }
        #endregion
        #region Dodaje grupni trening u fajl korisnika
        public static void DodajGrupniTreningKorisniku(Korisnik posetilac,GrupniTrening gt,string lepoFormatiranDatumRodjenja,string tipTreninga, string datumVreme)
        {
            string tempFile = Path.GetTempFileName();

            var path1 = HostingEnvironment.MapPath("~/App_Data/Korisnici.txt");
            FileStream fs1 = new FileStream(path1, FileMode.Open, FileAccess.Read);
            StreamReader sr1 = new StreamReader(fs1, Encoding.UTF8);

            string[] podaciPosetioca=new string[12];

            podaciPosetioca[0] = posetilac.KorisnickoIme;
            podaciPosetioca[1] = posetilac.Lozinka;
            podaciPosetioca[2] = posetilac.Ime;
            podaciPosetioca[3] = posetilac.Prezime;
            podaciPosetioca[4] = posetilac.Pol;
            podaciPosetioca[5] = posetilac.Email;
            podaciPosetioca[6] = lepoFormatiranDatumRodjenja;
            podaciPosetioca[7] = "POSETILAC";
            if (posetilac.ListaGrupnihTreninga.Count == 1)
            {
                podaciPosetioca[8] = "";
            }
            else
            {
                string line = "";
                while ((line = sr1.ReadLine()) != null)
                {
                    string[] tokens = line.Split(';');

                    if (tokens[0] == posetilac.KorisnickoIme)
                    {
                        podaciPosetioca[8] = tokens[8];
                    }
                }
            }
            podaciPosetioca[9] = "";
            podaciPosetioca[10] = "";
            podaciPosetioca[11] = "";


            sr1.Close();
            fs1.Close();

            var path = HostingEnvironment.MapPath("~/App_Data/Korisnici.txt");
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs, Encoding.UTF8);

            using (var sw = new StreamWriter(tempFile))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains(posetilac.KorisnickoIme))
                    {
                        if (podaciPosetioca[8] == "")
                        {
                            podaciPosetioca[8] += tipTreninga + "_" + datumVreme;
                        }
                        else
                        {
                            podaciPosetioca[8] = podaciPosetioca[8]+"|"+tipTreninga + "_" + datumVreme;
                        }

                        for (int i = 0; i < 12; i++)
                        {
                            sw.Write(podaciPosetioca[i]);
                            if (i == 11)
                            {
                                sw.WriteLine();
                            }
                            else
                            {
                                sw.Write(";");
                            }
                        }
                    }
                    else
                    {
                        sw.Write(line);
                        sw.WriteLine();
                    }
                }
            }
            sr.Close();
            fs.Close();
            File.Delete(path);
            File.Move(tempFile, path);
        }
        #endregion
        //Ovo mi je potrebno zbog ovih gore
        #region Procitaj korisnikov datum rodjenja
        public static string pronadjiDatumRodjenjaKorisnika(string korisnickoIme) //Nikako drugacije ne mogu da izvadim validan datum
        {
            List<Korisnik> korisnici = new List<Korisnik>();

            var path = HostingEnvironment.MapPath("~/App_Data/Korisnici.txt");
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";

            string datumRodjenja;

            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split(';');

                if (tokens[0] == korisnickoIme)
                {
                    datumRodjenja = tokens[6];
                    sr.Close();
                    stream.Close();
                    return datumRodjenja;
                }
            }
            sr.Close();
            stream.Close();

            return "";
        }
        #endregion
        #region Procitaj datum odrzavanja treninga
        public static string pronadjiDatumIVremeTreninga(string naziv,string tip,string trajanje, string nazivFC) //Nikako drugacije ne mogu da izvadim validan datum
        {
            var path = HostingEnvironment.MapPath("~/App_Data/GrupniTreninzi.txt");
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";

            string datumTreninga;

            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split(';');

                if (tokens[0] == naziv && tokens[1]==tip && tokens[2]== nazivFC && tokens[3]== trajanje)
                {
                    datumTreninga = tokens[4];
                    sr.Close();
                    stream.Close();
                    return datumTreninga;
                }
            }
            sr.Close();
            stream.Close();

            return "";
        }
        #endregion

        #region Procitaj jedan grupni trening
        public static bool procitajJedanGrupniTrening(string path,string naziv, string datumVreme,string ime,string prezime)
        {
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";

            while ((line = sr.ReadLine()) != null)
            {
                if (line.Contains(naziv) && line.Contains(datumVreme))
                {
                    string[] tokens = line.Split(';');
                    string[] posetioci = tokens[6].Split('|');

                    foreach (var posetilac in posetioci)
                    {
                        string[] korisnik = posetilac.Split('-');
                        Korisnik k = new Korisnik(korisnik[0], korisnik[1]);

                        if (korisnik[0]==ime && korisnik[1] == prezime)
                        {
                            sr.Close();
                            return true;
                        }
                    }
                }
            }

            sr.Close();
            stream.Close();

            return false;
        }
        #endregion

        #region Prisustvovao treningu
        public static List<GrupniTrening> prisustvovaoGrupnomTreningu(string path, string ime, string prezime)
        {
            List<GrupniTrening> grupniTreninzi = new List<GrupniTrening>();

            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";

            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split(';');

                FitnesCentar fc = new FitnesCentar(tokens[2]);

                List<Korisnik> listaPosetilaca = new List<Korisnik>();

                string temp= tokens[6];
                if (temp != "")
                {
                    string[] posetioci = tokens[6].Split('|');

                    foreach (var posetilac in posetioci)
                    {
                        string[] korisnik = posetilac.Split('-');
                        Korisnik k = new Korisnik(korisnik[0], korisnik[1]);
                        listaPosetilaca.Add(k);
                    }
                }
                GrupniTrening tr = new GrupniTrening(tokens[0], tokens[1], fc, tokens[3], DateTime.ParseExact(tokens[4], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture), Int32.Parse(tokens[5]), listaPosetilaca,tokens[7]);

                foreach (var posetilac in listaPosetilaca)
                {
                    if (posetilac.Ime == ime && posetilac.Prezime == prezime)
                        grupniTreninzi.Add(tr);
                }
            }

            sr.Close();
            stream.Close();

            return grupniTreninzi;
        }
        #endregion

        #region Sacuvaj komentar
        public static void SacuvajKomentar(string koJeOstavioKomentar,string fitnesCentarKomentarisan,Int32 ocena,string tekstKomentara)
        {
            var path = HostingEnvironment.MapPath("~/App_Data/Komentari.txt");
            FileStream stream = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(stream);

            string[] linija = new string[5];

            linija[0] = koJeOstavioKomentar;
            linija[1] = fitnesCentarKomentarisan;
            linija[2] = tekstKomentara;
            linija[3] = ocena.ToString();
            linija[4] = "NIJE ODOBREN";

            for (int i = 0; i < 5; i++)
            {
                sw.Write(linija[i]);
                if (i == 4)
                {
                    sw.WriteLine();
                }
                else
                {
                    sw.Write(";");
                }
            }

            sw.Close();
            stream.Close();
        }
        #endregion

        #region Procitaj jedan grupni trening trenera
        public static GrupniTrening procitajJedanGrupniTreningTrenera(string nazivFC, string datumVreme)
        {
            var path = HostingEnvironment.MapPath("~/App_Data/GrupniTreninzi.txt");
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";

            while ((line = sr.ReadLine()) != null)
            {
                if (line.Contains(nazivFC) && line.Contains(datumVreme))
                {
                    string[] tokens = line.Split(';');
                    string[] korisnici = tokens[6].Split('|');
                    
                    List<Korisnik> listaKorisnika = new List<Korisnik>();

                    for (int i = 0; i < korisnici.Length; i++)
                    {
                        string[] imePrz = korisnici[i].Split('-');

                        if (imePrz[0] != "") //ako je prazno polje, tj. nema posetioca jos uvek
                        {
                            Korisnik k = new Korisnik(imePrz[0], imePrz[1]);
                            listaKorisnika.Add(k);
                        }
                        else
                        {
                            Korisnik k = new Korisnik();    //Ako ne postoji ni jedan korisnik(slucaj kada trener brise)
                         //   listaKorisnika.Add(k);
                        }
                    }

                    FitnesCentar fc = new FitnesCentar(nazivFC);
                    DateTime d = DateTime.ParseExact(datumVreme, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                    GrupniTrening gt = new GrupniTrening(tokens[0], tokens[1], fc,tokens[3],d,Int32.Parse(tokens[5]),listaKorisnika,tokens[7]);
                    sr.Close();
                    return gt; //nesto vrati
                }
            }

            sr.Close();
            stream.Close();

            GrupniTrening grupni = new GrupniTrening();
            return grupni; //nesto vrati
        }
        #endregion
        #region Procitaj datum
        public static string pronadjiDatumIVremeTreningaTrener(string naziv, string tip, string losDatum) //Nikako drugacije ne mogu da izvadim validan datum
        {
            var path = HostingEnvironment.MapPath("~/App_Data/GrupniTreninzi.txt");
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";

            string datumTreninga;

            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split(';');
                string[] datum = tokens[4].Split(' ');

                string[] losDatumPodela = losDatum.Split(' ');
                string vreme = losDatumPodela[1].Substring(0, losDatumPodela[1].Length-3);

                //uzeo sam samo vreme datuma jer u tom FC ne moze biti 2 trneinga u isto vreme
                if (tokens[2] == naziv && tokens[1] == tip && datum[1].Equals(vreme))
                {
                    datumTreninga = tokens[4];
                    sr.Close();
                    stream.Close();
                    return datumTreninga;
                }
            }
            sr.Close();
            stream.Close();

            return "";
        }
        #endregion

        #region ObrisiTrening
        public static void ObrisiTrening(string naziv,string datum)
        {
            #region Brisanje starog
            string tempFile = Path.GetTempFileName();

            var path = HostingEnvironment.MapPath("~/App_Data/GrupniTreninzi.txt");
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            string linijaKojuMenjam = "";
            using (var sww = new StreamWriter(tempFile))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    if (!(line.Contains(naziv) && line.Contains(datum)))
                    {
                        sww.WriteLine(line);
                    }
                    else //Dodavanje dela za neaktivnu liniju
                    {
                        linijaKojuMenjam = line;
                        linijaKojuMenjam = linijaKojuMenjam.Substring(0, linijaKojuMenjam.Length-7);
                        linijaKojuMenjam = linijaKojuMenjam + "NeAkt!!";
                        sww.WriteLine(linijaKojuMenjam);
                    }
                }
            }
            sr.Close();
            stream.Close();
            File.Delete(path);
            File.Move(tempFile, path);
            #endregion
        }
        #endregion

        //Trener - kreiranje
        #region Trener dodaje novi trening
        public static string TrenerDodajeNoviTrening(string naziv,string tipTreninga,string naz,string trajanjeTreningaMinute,string datumIVremeTreninga,string maxBrojPosetioca, string prazniPosetioci,string treningAktivan)
        {
            var path = HostingEnvironment.MapPath("~/App_Data/GrupniTreninzi.txt");
            FileStream fs1 = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader sr1 = new StreamReader(fs1, Encoding.UTF8);

            string line;
            while ((line = sr1.ReadLine()) != null)
            {
                if (line.Contains(datumIVremeTreninga) && line.Contains(naz))
                {
                    sr1.Close();
                    fs1.Close();
                    return "vec postoji";
                }
            }
            sr1.Close();
            fs1.Close();

            FileStream stream = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(stream);

            sw.Write(naziv);
            sw.Write(";");

            sw.Write(tipTreninga);
            sw.Write(";");

            sw.Write(naz);
            sw.Write(";");

            sw.Write(trajanjeTreningaMinute);
            sw.Write(";");

            sw.Write(datumIVremeTreninga);
            sw.Write(";");

            sw.Write(maxBrojPosetioca);
            sw.Write(";");

            //sw.Write(prazniPosetioci);
            sw.Write(";");

            sw.Write(treningAktivan);
            sw.WriteLine();

            sw.Close();
            stream.Close();

            return "";
        }
        #endregion
        #region Dodaje grupni trening treneru
        public static void DodajGrupniTreningTreneru(Korisnik trener, string lepoFormatiranDatumRodjenja, string naziv, string datum)
        {
            #region Brisanje
            var path = HostingEnvironment.MapPath("~/App_Data/Korisnici.txt");
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs, Encoding.UTF8);

            string linija;
            string stariTreninzi = "";
            while ((linija = sr.ReadLine()) != null)
            {
                if (linija.Contains(trener.KorisnickoIme))
                {
                    string[] podeli= linija.Split(';');
                    stariTreninzi = podeli[9];
                }
            }
            string[] podaciTrenera = new string[12];

            podaciTrenera[0] = trener.KorisnickoIme;
            podaciTrenera[1] = trener.Lozinka;
            podaciTrenera[2] = trener.Ime;
            podaciTrenera[3] = trener.Prezime;
            podaciTrenera[4] = trener.Pol;
            podaciTrenera[5] = trener.Email;
            podaciTrenera[6] = lepoFormatiranDatumRodjenja;
            podaciTrenera[7] = "TRENER";
            podaciTrenera[8] = "";
            podaciTrenera[9] = stariTreninzi;
            podaciTrenera[10] = trener.AngazovanNaFitnesCentar.Naziv;
            podaciTrenera[11] = "";

            sr.Close();
            fs.Close();

            FileStream fs1 = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader sr1 = new StreamReader(fs1, Encoding.UTF8);

            string tempFile1 = Path.GetTempFileName();
            using (var sw1 = new StreamWriter(tempFile1))
            {
                string line;

                while ((line = sr1.ReadLine()) != null)
                {
                    if (line.Contains(trener.KorisnickoIme))
                    {
                        //PRESKOCI TU LINIJU - tj. obrise
                    }
                    else
                    {
                        sw1.WriteLine(line);
                    }
                }
            }
            sr1.Close();
            fs1.Close();
            File.Delete(path);
            File.Move(tempFile1, path);
            #endregion

            #region Dodavanje
            var path2 = HostingEnvironment.MapPath("~/App_Data/Korisnici.txt");

            FileStream fs3 = new FileStream(path2, FileMode.Open, FileAccess.Read);
            StreamReader sr3 = new StreamReader(fs3, Encoding.UTF8);

            string tempFile2 = Path.GetTempFileName();
            using (var sw2 = new StreamWriter(tempFile2))
            {
                string line;

                while ((line = sr3.ReadLine()) != null)
                {
                    sw2.WriteLine(line);
                }

                podaciTrenera[9] += "|" + naziv + "_" + datum;
                for (int i = 0; i < podaciTrenera.Length; i++)
                {
                    if (i == 11)
                    {
                        sw2.WriteLine();
                        break;
                    }
                    sw2.Write(podaciTrenera[i]);
                    sw2.Write(";");
                }
            }
            sr3.Close();
            fs3.Close();
            File.Delete(path2);
            File.Move(tempFile2, path2);
            #endregion
        }
        #endregion

        //Modifikovanje
        #region Modifikuj trening
        public static void ModifikujTrening(string stariDatum,string naziv, string tipTreninga, string naz, string trajanjeTreningaMinute, string datumIVremeTreninga, string maxBrojPosetioca, string prazniPosetioci, string treningAktivan)
        {
            #region Brisanje
            var path = HostingEnvironment.MapPath("~/App_Data/GrupniTreninzi.txt");

            FileStream fs1 = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader sr1 = new StreamReader(fs1, Encoding.UTF8);

            string tempFile1 = Path.GetTempFileName();
            string listaPosetilaca = "";
            using (var sw1 = new StreamWriter(tempFile1))
            {
                string line;

                while ((line = sr1.ReadLine()) != null)
                {
                    if (line.Contains(stariDatum) && line.Contains(naz))
                    {
                        string[] posetioci = line.Split(';');
                        
                        for (int i = 0; i < posetioci[6].Length; i++)
                        {
                            string[] podeliOpet = posetioci[6].Split('|');
                            for (int j = 0; j < podeliOpet.Length; j++)
                            {
                                if (podeliOpet[j].Contains(stariDatum) && podeliOpet[j].Contains(naz))
                                {
                                    //PRESKOCI TU LINIJU - tj. obrise
                                }
                                else
                                {
                                    if (j == 0)
                                    {
                                        if(!listaPosetilaca.Contains(podeliOpet[j]))
                                            listaPosetilaca += podeliOpet[j];
                                    }
                                    else
                                    {
                                        if (!listaPosetilaca.Contains(podeliOpet[j]))
                                            listaPosetilaca += "|" + podeliOpet[j];
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        sw1.WriteLine(line);
                    }
                }
            }
            sr1.Close();
            fs1.Close();
            File.Delete(path);
            File.Move(tempFile1, path);
            #endregion

            #region Upisi promenjeno
            FileStream stream = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(stream);

            sw.Write(naziv);
            sw.Write(";");

            sw.Write(tipTreninga);
            sw.Write(";");

            sw.Write(naz);
            sw.Write(";");

            sw.Write(trajanjeTreningaMinute);
            sw.Write(";");

            sw.Write(datumIVremeTreninga);
            sw.Write(";");

            sw.Write(maxBrojPosetioca);
            sw.Write(";");

            sw.Write(listaPosetilaca);
            sw.Write(";");

            sw.Write(treningAktivan);
            sw.WriteLine();

            sw.Close();
            stream.Close();
            #endregion
        }
        #endregion
        #region Modifikuje grupni trening treneru
        public static void ModifikujGrupniTreningTreneru(Korisnik trener, string lepoFormatiranDatumRodjenja, string naziv, string stariDatum,string datum)
        {
            #region Brisanje
            var path = HostingEnvironment.MapPath("~/App_Data/Korisnici.txt");
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs, Encoding.UTF8);

            string linija;
            string stariTreninzi = "";
            while ((linija = sr.ReadLine()) != null)
            {
                if (linija.Contains(trener.KorisnickoIme))
                {
                    string[] podeli = linija.Split(';');
                    string[] izvadiTreningKojiMenjas = podeli[9].Split('|');
                    for (int i = 0; i < izvadiTreningKojiMenjas.Length; i++)
                    {
                        if (izvadiTreningKojiMenjas[i].Contains(stariDatum)) //ovo je stari datum tj. onaj pre brisanja
                        {
                            //Njega preskoci
                        }
                        else
                        {
                            if (stariTreninzi == "")
                            {
                                stariTreninzi += izvadiTreningKojiMenjas[i];
                            }
                            else
                            {
                                stariTreninzi += "|"+izvadiTreningKojiMenjas[i];
                            }
                        }
                    }
                }
            }
            string[] podaciTrenera = new string[12];

            podaciTrenera[0] = trener.KorisnickoIme;
            podaciTrenera[1] = trener.Lozinka;
            podaciTrenera[2] = trener.Ime;
            podaciTrenera[3] = trener.Prezime;
            podaciTrenera[4] = trener.Pol;
            podaciTrenera[5] = trener.Email;
            podaciTrenera[6] = lepoFormatiranDatumRodjenja;
            podaciTrenera[7] = "TRENER";
            podaciTrenera[8] = "";
            podaciTrenera[9] = stariTreninzi;
            podaciTrenera[10] = trener.AngazovanNaFitnesCentar.Naziv;
            podaciTrenera[11] = "";

            sr.Close();
            fs.Close();

            FileStream fs1 = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader sr1 = new StreamReader(fs1, Encoding.UTF8);

            string tempFile1 = Path.GetTempFileName();
            using (var sw1 = new StreamWriter(tempFile1))
            {
                string line;

                while ((line = sr1.ReadLine()) != null)
                {
                    if (line.Contains(trener.KorisnickoIme))
                    {
                        //PRESKOCI TU LINIJU - tj. obrise
                    }
                    else
                    {
                        sw1.WriteLine(line);
                    }
                }
            }
            sr1.Close();
            fs1.Close();
            File.Delete(path);
            File.Move(tempFile1, path);
            #endregion

            #region Dodavanje
            var path2 = HostingEnvironment.MapPath("~/App_Data/Korisnici.txt");

            FileStream fs3 = new FileStream(path2, FileMode.Open, FileAccess.Read);
            StreamReader sr3 = new StreamReader(fs3, Encoding.UTF8);

            string tempFile2 = Path.GetTempFileName();
            using (var sw2 = new StreamWriter(tempFile2))
            {
                string line;

                while ((line = sr3.ReadLine()) != null)
                {
                    sw2.WriteLine(line);
                }

                podaciTrenera[9] += "|" + naziv + "_" + datum;
                for (int i = 0; i < podaciTrenera.Length; i++)
                {
                    if (i == 11)
                    {
                        sw2.WriteLine();
                        break;
                    }
                    sw2.Write(podaciTrenera[i]);
                    sw2.Write(";");
                }
            }
            sr3.Close();
            fs3.Close();
            File.Delete(path2);
            File.Move(tempFile2, path2);
            #endregion
        }
        #endregion
    }
}