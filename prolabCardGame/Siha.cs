using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    internal class Siha: Hava_Araclari
    {

        public override string AltSinif { get; set; } = "Siha";
        public override int KaraVurusAvantaji { get; set; } = 10;



        public int DenizVurusAvantaji { get; set; } = 10;
        public override int ID { get; set; } = 010200;
        public static int sayac { get; set; } = 0;
        public override int Vurus { get; } = 10;

        public override int Dayaniklilik { get; set; } = 15;


        public override string ToString()
        {
            return $"{AltSinif}  ";
        }
        public Siha(int seviyePuani, string sinif,int dayaniklilik, string altSinif,int Secilme_sayisi) : base(seviyePuani, sinif)
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
