using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

using System.Globalization;

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

                Komentar kom = new Komentar(tokens[0],tokens[1],tokens[2],Int32.Parse(tokens[3]));
                
                komentari.Add(kom);
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
    }
}