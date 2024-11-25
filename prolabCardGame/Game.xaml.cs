using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TestProject
{
   
    public partial class Game : Window
    {
        public Game()
        {
            InitializeComponent();
            Oyuncu player = new Oyuncu();

            List<Savas_Araclari> Kartlar = player.KartSec();

            foreach (var kart in Kartlar)
            {
                cmb2.Items.Add(kart.ToString());
                cmb2_Copy1.Items.Add(kart.ToString());
                cmb2_Copy.Items.Add(kart.ToString());
            }

          

        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var image = sender as System.Windows.Controls.Image;


            // Tag özelliği ile hangi resme tıklandığını belirleyip, işlemi ayırt etme
            switch (image?.Tag)
            {
                case "KFSP":
                    MessageBox.Show($"KFSP resme tıklandı!  ");
                    break;
                case "FirkateynP":
                    MessageBox.Show("FirkateynP resme tıklandı!");
                    break;
                case "UcakP":
                    MessageBox.Show("UcakP resme tıklandı!");
                    break;
                default:
                    MessageBox.Show("Bilinmeyen bir resme tıklandı!");
                    break;
            }
        }

        private void cmb2_Copy1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmb2_Copy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmb2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
