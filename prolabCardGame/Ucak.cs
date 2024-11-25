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

        public int ID { get; private set; }
        public override int Vurus { get; } = 10;


        public override string ToString()
        {
            return $"{AltSinif}";
        }


        public Ucak(int seviyePuani, string sinif,int dayaniklilik,int Id,int Secilme_sayisi,string altSinif) : base(seviyePuani, sinif)
        {
            Dayaniklilik = dayaniklilik;
            ID = Id+1;
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
