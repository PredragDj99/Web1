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
                List <GrupniTrening> grupniTrening = new List<GrupniTrening>();
                List<GrupniTrening> treninziAngazovan = new List<GrupniTrening>();
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
                    Korisnik k = new Korisnik(korisnik[0], korisnik[1]);
                    listaPosetilaca.Add(k);
                }
                
                GrupniTrening tr = new GrupniTrening(tokens[0],tokens[1],fc,tokens[3],DateTime.ParseExact(tokens[4], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),Int32.Parse(tokens[5]),listaPosetilaca);

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
            /*
            string tempFile = Path.GetTempFileName();

            var path2 = HostingEnvironment.MapPath("~/App_Data/Korisnici.txt");
            FileStream fsRead = new FileStream(path2, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fsRead, Encoding.UTF8);

            using (var sww = new StreamWriter(tempFile))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    if (line != "removeme")
                        sww.WriteLine(line);
                }
            }
            sr.Close();
            fsRead.Close();
            File.Delete(path2);
            File.Move(tempFile, path2);
            */

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
        public static void DodajUGrupniTrening(string ime, string prezime, string naziv, string datumVreme)
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
                    sw.Write(line);
                    //Proveravam koji je to grupni trening i onda ga dodajem na kraj te linije
                    if (line.Contains(naziv) && line.Contains(datumVreme))
                    {
                        sw.Write("|");
                        sw.Write(ime);
                        sw.Write("-");
                        sw.Write(prezime);
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
        public static void DodajGrupniTreningKorisniku(Korisnik posetilac,string lepoFormatiranDatumRodjenja,string tipTreninga, string datumVreme)
        {
            string tempFile = Path.GetTempFileName();

            var path = HostingEnvironment.MapPath("~/App_Data/Korisnici.txt");
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs, Encoding.UTF8);

            string[] podaciPosetioca=new string[12];

            podaciPosetioca[0] = posetilac.KorisnickoIme;
            podaciPosetioca[1] = posetilac.Lozinka;
            podaciPosetioca[2] = posetilac.Ime;
            podaciPosetioca[3] = posetilac.Prezime;
            podaciPosetioca[4] = posetilac.Pol;
            podaciPosetioca[5] = posetilac.Email;
            podaciPosetioca[6] = lepoFormatiranDatumRodjenja;
            podaciPosetioca[7] = "POSETILAC";
            if (posetilac.ListaGrupnihTreninga.Count == 0)
            {
                podaciPosetioca[8] = "";
            }
            else
            {
                foreach (var item in posetilac.ListaGrupnihTreninga)
                {
                    podaciPosetioca[8] += item;
                }
            }
            podaciPosetioca[9] = "";
            podaciPosetioca[10] = "";
            podaciPosetioca[11] = "";

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
                            podaciPosetioca[8] += "|"+tipTreninga + "_" + datumVreme;
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

                string[] posetioci = tokens[6].Split('|');
                List<Korisnik> listaPosetilaca = new List<Korisnik>();

                foreach (var posetilac in posetioci)
                {
                    string[] korisnik = posetilac.Split('-');
                    Korisnik k = new Korisnik(korisnik[0], korisnik[1]);
                    listaPosetilaca.Add(k);
                }

                GrupniTrening tr = new GrupniTrening(tokens[0], tokens[1], fc, tokens[3], DateTime.ParseExact(tokens[4], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture), Int32.Parse(tokens[5]), listaPosetilaca);

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
    }
}