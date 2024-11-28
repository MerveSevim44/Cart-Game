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



        public int DenizVurusAvantaji { get; set; } = 10;
        public override int ID { get; set; } = 010200;
        public static int sayac { get; set; } = 0;
        public override int Vurus { get; } = 10;

        public override int Dayaniklilik { get; set; } = 15;


        public override string ToString()
        {
            return $"{AltSinif}  ";
        }
        public Siha(int seviyePuani, string sinif,int dayaniklilik, string altSinif,int Secilme_sayisi) : base(seviyePuani, sinif)
        {
            ID += ++sayac;
            AltSinif = altSinif;
        }

 public override void DurumGuncelle(Savas_Araclari otherCard )
 {
     if (otherCard is Ucak)
     {
         Ucak rivalUcak = (Ucak)otherCard;
         this.Dayaniklilik -= rivalUcak.Vurus;
         rivalUcak.Dayaniklilik -= this.Vurus;


         if (rivalUcak.Dayaniklilik == 0)
         {
             this.Seviye_Puani += rivalUcak.Seviye_Puani + 10;
         }

     }

     else if (otherCard is Obus)
     {
         Obus rivalObus = (Obus)otherCard;
         this.Dayaniklilik -= rivalObus.Vurus;
         rivalObus.Dayaniklilik -= this.Vurus;


         if (rivalObus.Dayaniklilik == 0)
         {
             this.Seviye_Puani += rivalObus.Seviye_Puani + 10;
         }
     }

     else if (otherCard is Firakteyn)
     {

         Firakteyn rivalFirakteyn = (Firakteyn)otherCard;
         this.Dayaniklilik -= rivalFirakteyn.Vurus + rivalFirakteyn.HavaVurusAvantaji;
         rivalFirakteyn.Dayaniklilik -= this.Vurus;


         if (rivalFirakteyn.Dayaniklilik == 0)
         {
             this.Seviye_Puani += rivalFirakteyn.Seviye_Puani + 10;
         }
     }

     else if (otherCard is Sida)
     {
         Sida rivalSida = (Sida)otherCard;
         this.Dayaniklilik -= rivalSida.Vurus + rivalSida.HavaVurusAvantaji;
         rivalSida.Dayaniklilik -= this.Vurus;


         if (rivalSida.Dayaniklilik == 0)
         {
             this.Seviye_Puani += rivalSida.Seviye_Puani + 10;
         }
     }

     else if (otherCard is Siha)
     {
         Siha rivalSiha = (Siha)otherCard;
         this.Dayaniklilik -= rivalSiha.Vurus;
         rivalSiha.Dayaniklilik -= this.Vurus;


         if (rivalSiha.Dayaniklilik == 0)
         {
             this.Seviye_Puani += rivalSiha.Seviye_Puani + 10;
         }
     }
     else if (otherCard is KFS)
     {
         KFS rivalKFS = (KFS)otherCard;
         this.Dayaniklilik -= rivalKFS.Vurus + rivalKFS.HavaVurusAvantaji;
         rivalKFS.Dayaniklilik -= this.Vurus;


         if (rivalKFS.Dayaniklilik == 0)
         {
             this.Seviye_Puani += rivalKFS.Seviye_Puani + 10;
         }
     }
 }

        public override void KartPuaniGoster()
        {
            base.KartPuaniGoster();
        }
    }
}
