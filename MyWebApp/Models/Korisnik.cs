using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApp.Models
{
    public class Korisnik
    {
        public Korisnik(string korisnickoIme, string lozinka, string ime, string prezime, string pol, string email, DateTime datumRodjenja, KorisnikType uloga, List<GrupniTrening> listaGrupnihTreninga, List<GrupniTrening> listaTreninziAngazovan, FitnesCentar angazovanNaFitnesCentar, List<FitnesCentar> listaVlasnickiFitnesCentar, string trenerBlokiran)
        {
            KorisnickoIme = korisnickoIme;
            Lozinka = lozinka;
            Ime = ime;
            Prezime = prezime;
            Pol = pol;
            Email = email;
            DatumRodjenja = datumRodjenja;
            Uloga = uloga;
            ListaGrupnihTreninga = listaGrupnihTreninga;
            ListaTreninziAngazovan = listaTreninziAngazovan;
            AngazovanNaFitnesCentar = angazovanNaFitnesCentar;
            ListaVlasnickiFitnesCentar = listaVlasnickiFitnesCentar;
            TrenerBlokiran = trenerBlokiran;
        }

        public string KorisnickoIme { get; set; }
        public string Lozinka { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Pol { get; set; }
        public string Email { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public KorisnikType Uloga { get; set; }
        public List<GrupniTrening> ListaGrupnihTreninga { get; set; } = new List<GrupniTrening>(); //na koje je prijavljen posetioc
        public List<GrupniTrening> ListaTreninziAngazovan { get; set; } = new List<GrupniTrening>(); //ako je trener
        public FitnesCentar AngazovanNaFitnesCentar { get; set; } //ako je trener
        public List<FitnesCentar> ListaVlasnickiFitnesCentar { get; set; } = new List<FitnesCentar>(); //ako je vlasnik
        public string TrenerBlokiran { get; set; }

        public Korisnik(string ime, string prezime)
        {
            Ime = ime;
            Prezime = prezime;
        }

        public Korisnik()
        {

        }
    }
}