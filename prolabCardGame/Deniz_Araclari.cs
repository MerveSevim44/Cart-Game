using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    internal abstract class Deniz_Araclari : Savas_Araclari
    {

        public int Seviye_Puani { get; set; } = 0;
        public override int Dayaniklilik { get; set; } 

        public override int Vurus { get;  }

        public override string Sinif { get; set; } = "Deniz";

    

        public abstract int HavaVurusAvantaji { get; set; }  

        public override string AltSinif { get; set; }


        public Deniz_Araclari(int seviyePuan, string sinif) : base(seviyePuan)
        {


        }
        public override void DurumGuncelle(Savas_Araclari otherCard)
        {

        }


        public override void KartPuaniGoster()
        {
            base.KartPuaniGoster();
        }
    }
}
