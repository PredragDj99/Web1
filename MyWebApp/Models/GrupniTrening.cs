using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApp.Models
{
    public class GrupniTrening
    {
        public string Naziv { get; set; }
        public string TipTreninga { get; set; }
        public FitnesCentar FitnesCentarOdrzava { get; set; }
        public DateTime TrajanjeTreningaMinute { get; set; } //skloni datum
        public DateTime DatumIVremeTreninga { get; set; }
        public Int32 MaksimalanBrojPosetioca { get; set; }
        public List<Korisnik> SpisakPosetilaca { get; set; }
    }
}