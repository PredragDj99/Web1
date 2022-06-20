using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApp.Models
{
    public class GrupniTrening
    {
        public GrupniTrening(string naziv, string tipTreninga, FitnesCentar fitnesCentarOdrzava, string trajanjeTreningaMinute, DateTime datumIVremeTreninga, int maksimalanBrojPosetioca, List<Korisnik> spisakPosetilaca,string obrisan)
        {
            Naziv = naziv;
            TipTreninga = tipTreninga;
            FitnesCentarOdrzava = fitnesCentarOdrzava;
            TrajanjeTreningaMinute = trajanjeTreningaMinute;
            DatumIVremeTreninga = datumIVremeTreninga;
            MaksimalanBrojPosetioca = maksimalanBrojPosetioca;
            SpisakPosetilaca = spisakPosetilaca;
            Obrisan = obrisan;
        }

        public string Naziv { get; set; }
        public string TipTreninga { get; set; }
        public FitnesCentar FitnesCentarOdrzava { get; set; }
        public string TrajanjeTreningaMinute { get; set; }
        public DateTime DatumIVremeTreninga { get; set; }
        public Int32 MaksimalanBrojPosetioca { get; set; }
        public List<Korisnik> SpisakPosetilaca { get; set; }
        public string Obrisan { get; set; }

        public GrupniTrening(string tipTreninga,DateTime datumIVremeTreninga,FitnesCentar fitnesCentarOdrzava)
        {
            TipTreninga = tipTreninga;
            FitnesCentarOdrzava = fitnesCentarOdrzava;
            DatumIVremeTreninga=datumIVremeTreninga;
        }

        public GrupniTrening()
        {

        }

    }
}