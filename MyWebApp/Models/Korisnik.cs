﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApp.Models
{
    public class Korisnik
    {
        public string KorisnickoIme { get; set; }
        public string Lozinka { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Pol { get; set; }
        public string Email { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public KorisnikType Uloga { get; set; }
        public List<GrupniTrening> ListaGrupnihTreninga { get; set; }
        public List<GrupniTrening> ListaTreninziAngazovan { get; set; }
        public FitnesCentar AngazovanNaFitnesCentar { get; set; }
        public List<FitnesCentar> ListaVlasnickiFitnesCentar { get; set; }
    }
}