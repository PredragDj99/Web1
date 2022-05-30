using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

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
                Korisnik vlasnik = new Korisnik(tokens[3]);
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
    }
}