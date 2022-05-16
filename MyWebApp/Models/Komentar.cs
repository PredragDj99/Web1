using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApp.Models
{
    public class Komentar
    {
        public string KoJeOstavioKomentar { get; set; }
        public string FitnesCentarKomentarisan { get; set; } 
        public string TekstKomentara { get; set; }
        public Int32 Ocena { get; set; }
    }
}