﻿using System;
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
        public override int ID { get; set; } = 030200;
        public static int sayac { get; set; } = 0;

        public override string ToString()
        {
            return $"{AltSinif}";
        }
        public KFS(int seviyePuani, string sinif,int dayaniklilik, int Secilme_sayisi, string altSinif) : base(seviyePuani, sinif)
        {
            ID += ++sayac;
            AltSinif = altSinif;
        }

        public override void DurumGuncelle(Savas_Araclari otherCard )
        {
            if (otherCard is Ucak)
            {
                Ucak rivalUcak = (Ucak)otherCard;
                this.Dayaniklilik -= rivalUcak.Vurus + rivalUcak.KaraVurusAvantaji;

            }

            else if (otherCard is Obus)
            {
                Obus rivalObus = (Obus)otherCard;
                this.Dayaniklilik -= rivalObus.Vurus;

            }

            else if (otherCard is Firakteyn)
            {

                Firakteyn rivalFirakteyn = (Firakteyn)otherCard;
                this.Dayaniklilik -= rivalFirakteyn.Vurus;



            }

            else if (otherCard is Sida)
            {
                Sida rivalSida = (Sida)otherCard;
                this.Dayaniklilik -= rivalSida.Vurus + rivalSida.KaraVurusAvantaji;



            }

            else if (otherCard is Siha)
            {
                Siha rivalSiha = (Siha)otherCard;
                this.Dayaniklilik -= rivalSiha.Vurus + rivalSiha.KaraVurusAvantaji;


            }
            else if (otherCard is KFS)
            {
                KFS rivalKFS = (KFS)otherCard;
                this.Dayaniklilik -= rivalKFS.Vurus ;


            }

          
        }
    }
}
