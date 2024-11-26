using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    internal abstract class Savas_Araclari
    {
        public int Seviye_Puani { get; set; }
        public abstract int Dayaniklilik { get; set; }
        public abstract int Vurus { get; }
        public abstract string Sinif { get; set; }
        public abstract int ID {  get; set; }
        
        public abstract string AltSinif { get; set; }   

        public Savas_Araclari(int seviyePuani)
        {
            Seviye_Puani = seviyePuani; 
        }


       


        public virtual void KartPuaniGoster()
        {
            
        }

        public abstract void DurumGuncelle();
    }
}
