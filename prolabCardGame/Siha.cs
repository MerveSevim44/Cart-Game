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


        public override string ToString()
        {
            return $"{AltSinif}  ";
        }

        public int DenizVurusAvantaji { get; set; } = 10;
        public int ID;
        public override int Vurus { get; } = 10;
        public Siha(int seviyePuani, string sinif,int dayaniklilik,int Id, string altSinif,int Secilme_sayisi) : base(seviyePuani, sinif)
        {
            Id++;
            Dayaniklilik = dayaniklilik;  
            ID = Id;
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
