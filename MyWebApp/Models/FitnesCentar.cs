using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApp.Models
{
    public class FitnesCentar
    {
        public FitnesCentar(string naziv, string adresa, int godinaOtvaranja, Korisnik vlasnik, double mesecnaClanarina, double godisnjaClanarina, double jedanTrening, double jedanGrupniTrening, double jedanSaPersonalnimTrenerom)
        {
            Naziv = naziv;
            Adresa = adresa;
            GodinaOtvaranja = godinaOtvaranja;
            Vlasnik = vlasnik;
            MesecnaClanarina = mesecnaClanarina;
            GodisnjaClanarina = godisnjaClanarina;
            JedanTrening = jedanTrening;
            JedanGrupniTrening = jedanGrupniTrening;
            JedanSaPersonalnimTrenerom = jedanSaPersonalnimTrenerom;
        }

        public string Naziv { get; set; }
        public string Adresa { get; set; }
        public Int32 GodinaOtvaranja { get; set; }
        public Korisnik Vlasnik { get; set; }
        public double MesecnaClanarina { get; set; }
        public double GodisnjaClanarina { get; set; }
        public double JedanTrening { get; set; }
        public double JedanGrupniTrening { get; set; }
        public double JedanSaPersonalnimTrenerom { get; set; }

        public FitnesCentar(string naziv)
        {
            Naziv = naziv;
        }

        public FitnesCentar() { }
    }
}