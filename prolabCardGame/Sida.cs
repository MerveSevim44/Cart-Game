using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    internal class Sida : Deniz_Araclari
    {
        public override string AltSinif { get; set; } = "Sida";
        public override int HavaVurusAvantaji { get; set; } = 10;

        public int KaraVurusAvantaji { get; set; } = 10;
       
        public override int Vurus { get; } = 10;
        public override int ID { get; set; } = 020100;
        public static int sayac { get; set; } = 0;
        public override int Dayaniklilik { get; set; } = 15;

        public override string ToString()
        {
            return $"{AltSinif}";
        }
        public Sida(int seviyePuani, string sinif,int dayaniklilik, string altSinif, int Secilme_sayisi) : base(seviyePuani, sinif)
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
