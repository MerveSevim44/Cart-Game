using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    internal class KFS : Kara_Araclari
    {

        public override string AltSinif { get; set; } = "KFS";
        public override int DenizVurusAvantaji { get; set; } = 10;
       
        public int HavaVurusAvantaji { get; set; } = 20;
        public override int Vurus { get;} = 10;
        public int ID;

        public KFS(int seviyePuani, string sinif,int dayaniklilik,int Id, string altSinif, int Secilme_sayisi) : base(seviyePuani, sinif)
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
