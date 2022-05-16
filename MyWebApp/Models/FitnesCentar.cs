using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApp.Models
{
    public class FitnesCentar
    {
        public string Naziv { get; set; }
        public string Adresa { get; set; }
        public Int32 GodinaOtvaranja { get; set; }
        public Korisnik Vlasnik { get; set; }
        public double MesecnaClanarina { get; set; }
        public double GodisnjaClanarina { get; set; }
        public double JedanTrening { get; set; }
        public double JedanGrupniTrening { get; set; }
        public double JedanSaPersonalnimTrenerom { get; set; }
    }
}