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

                //ovde ce biti svasta nesto
                //Korisnik k = new Korisnik();
                
                //korisnici.Add(k);
            }

            sr.Close();
            stream.Close();

            return korisnici;
        }

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
    }
}