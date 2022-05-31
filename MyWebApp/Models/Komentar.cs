using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApp.Models
{
    public class Komentar
    {
        public Komentar(string koJeOstavioKomentar, string fitnesCentarKomentarisan, string tekstKomentara, int ocena)
        {
            KoJeOstavioKomentar = koJeOstavioKomentar;
            FitnesCentarKomentarisan = fitnesCentarKomentarisan;
            TekstKomentara = tekstKomentara;
            Ocena = ocena;
        }

        public string KoJeOstavioKomentar { get; set; }
        public string FitnesCentarKomentarisan { get; set; } 
        public string TekstKomentara { get; set; }
        public Int32 Ocena { get; set; }
    }
}