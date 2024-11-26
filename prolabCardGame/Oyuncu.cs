using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TestProject
{
    internal  class Oyuncu   
    {
        public int Oyuncu_Id { get; set; }
        public string Oyuncu_Adi { get; set; }

        public int Skor { get; set; }
        public List<Savas_Araclari> KartListesi { get; set; } = new List<Savas_Araclari>();


        public Oyuncu()
        {

            Skor = 0;
            KartListesi = new List<Savas_Araclari>();
        }


        public Oyuncu(int id, string adi, int skor)
        {
            Oyuncu_Id = id;
            Skor = skor;
            Oyuncu_Adi = adi;
            KartListesi = new List<Savas_Araclari>();
        }

        public void SkorGoster()
        {
            Console.WriteLine($"{Oyuncu_Adi}'nın Skoru: {Skor}");
        }



        public virtual List<Savas_Araclari> KartSec()
        {
            List<Savas_Araclari> KartListesi = new List<Savas_Araclari>(){};

            string[ ] cardTypes = { "Hava", "Kara", "Deniz" };



            Random rand = new Random();
            for (int i = 0; i < 5; i++)
            {

                string cardType = cardTypes[rand.Next(cardTypes.Length)];

                switch (cardType)
                {

                    case "Hava":
                        KartListesi.Add(new Ucak(0, "Hava", 20,0, "Ucak"));
                        break;
                    case "Kara":
                        KartListesi.Add(new Obus(0, "Kara", 20,0, "Obus"));
                        break;
                    case "Deniz":
                        KartListesi.Add(new Firakteyn(0, "Deniz", 20,0, "Firkateyn"));
                        break;
                }
            }



            return KartListesi;



        }




    }


}

   