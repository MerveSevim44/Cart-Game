using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    internal class Firakteyn: Deniz_Araclari
    {
        public override string AltSinif { get; set; } = "Firakteyn";

        public override int HavaVurusAvantaji { get; set; } = 5;
        
        public override int Vurus { get;  } = 10;
        public override int ID { get; set; } = 020200;
        public static int sayac { get; set; } = 0;
        public override int Dayaniklilik { get; set; } = 25;

        public override string ToString()
        {
            return $"{AltSinif}";
        }

        public Firakteyn(int seviyePuani, string sinif,int dayaniklilik, int Secilme_sayisi,string altSinif) : base(seviyePuani, sinif)
        {
            ID += ++sayac;
            AltSinif = altSinif;
        }

        
        public override void DurumGuncelle()
        {

        }


        public override void KartPuaniGoster()
        {
            base.KartPuaniGoster();
        }
    }
}
