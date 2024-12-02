using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TestProject
{
    internal abstract class Savas_Araclari
    {
        public int Seviye_Puani { get; set; }
        public int selecTimes { get; set; } = 0;

        public abstract int Dayaniklilik { get; set; }
        public abstract int Vurus { get; }
        public abstract string Sinif { get; set; }
        public abstract int ID {  get; set; }
        
        public abstract string AltSinif { get; set; }   
        public static int sayac  { get; set; }  = 0;

        public Savas_Araclari(int seviyePuani)
        {
            Seviye_Puani = seviyePuani; 
        }     

        public virtual void KartPuaniGoster(Canvas canvas, Savas_Araclari card, List<Label> labels)
        {
            foreach (var label in labels)
                canvas.Children.Add(label);
        }

        public abstract void DurumGuncelle(Savas_Araclari otherCard);
    }
}
