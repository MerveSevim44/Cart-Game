using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    internal abstract class Kara_Araclari : Savas_Araclari
    {
        public override int Dayaniklilik { get; set; }
        public override int Vurus { get;  }
        public override string Sinif { get; set; } = "Kara";

        // Alt sınıfların doldurması gereken özellikler
        public override string AltSinif { get; set; }
        public abstract int DenizVurusAvantaji { get; set; }
        public override int ID { get; set; }
       


        public Kara_Araclari(int seviyePuani, string sinif) : base(seviyePuani)
        {
            
            
        }

        public override void DurumGuncelle(Savas_Araclari otherCard)
        {
            
        }
    }

   
}




