using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    internal class Ucak : Hava_Araclari
    {
        public override string AltSinif { get; set; } = "Ucak";
        public override int KaraVurusAvantaji { get; set; } = 10;

        public override int ID { get; set; } = 010100;
        public static int sayac { get; set; } = 0;
        public override int Vurus { get; } = 10;
        public override int Dayaniklilik { get ; set ; } = 20;


        public override string ToString()
        {
            return $"{AltSinif}";
        }


        public Ucak(int seviyePuani, string sinif,int dayaniklilik,int Secilme_sayisi,string altSinif) : base(seviyePuani, sinif)
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
