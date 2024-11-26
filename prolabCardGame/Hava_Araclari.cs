using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TestProject
{
    internal abstract class Hava_Araclari : Savas_Araclari
    {
        public int Seviye_Puani { get; set; } = 0;
        public override int Dayaniklilik { get; set; }

        public override int Vurus { get;  }

        public override string Sinif { get; set; } = "Hava";
      

        public abstract int KaraVurusAvantaji { get; set; }
        public override string AltSinif { get; set; }
        public override int ID { get; set; }
        



        public override string ToString()
        {
            return $"{AltSinif}";
        }


        public Hava_Araclari(int seviyePuan, string sinif) :base(seviyePuan)
        {
             

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


