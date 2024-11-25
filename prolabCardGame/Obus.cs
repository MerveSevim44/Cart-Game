using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    internal class Obus : Kara_Araclari
    {
            public override string AltSinif { get; set; } = "Obus";
             public override int DenizVurusAvantaji { get; set; } = 5;
        
        public override int Vurus { get; } = 10;
        public int ID;


        public override string ToString()
        {
            return $"{AltSinif}";
        }
        public Obus(int seviyePuani,string sinif,int dayaniklilik,int Id, int Secilme_sayisi,string altSinif) : base(seviyePuani,sinif)
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
