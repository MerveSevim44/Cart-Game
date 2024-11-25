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

            int cardSpacing = 150; // Space between each card
            int totalWidth = Kartlar.Count * cardSpacing; // Total width without considering the card width

            // Calculate the starting X position to center the cards on the screen
            int centerX = 1024 / 2; // Assuming screen width is 1024

            // Calculate the offset from the center for the first card
            int startX = centerX - totalWidth / 2+60;

            for (int i=0;i<Kartlar.Count;i++){ 
            // Create Canvas
            Canvas canvas = new Canvas
            {
                 Margin = new Thickness(startX + i * cardSpacing, 454, 0, 20) // Y position fixed at 454, and 0 for right and bottom margin
            };

                // Create Image
                Image cardImage = new Image
            {
                Source = new BitmapImage(new Uri("/image/"+ Kartlar[i].ToString() + ".png", UriKind.Relative)),
                Stretch = Stretch.Uniform,
                Height = 202,
                Width = 131,
                Tag = Kartlar[i].ToString() + "P"
            };
            Canvas.SetLeft(cardImage, 26);
            Canvas.SetTop(cardImage, 20);
            cardImage.MouseLeftButtonDown += Image_MouseLeftButtonDown;
            canvas.Children.Add(cardImage);

            // Create Labels
            Label label1 = CreateLabel("Label", 49, 20, 80, 117, VerticalAlignment.Top);
            canvas.Children.Add(label1);

            Label label2 = CreateLabel("Label", 49, 20, 55, 100, VerticalAlignment.Top);
            canvas.Children.Add(label2);

            Label label3 = CreateLabel("Label", 49, 20, 89, 159, VerticalAlignment.Center);
            canvas.Children.Add(label3);
            MainGrid.Children.Add(canvas);
            }
        }


        private Label CreateLabel(string content, double width, double height, double left, double top, VerticalAlignment verticalAlignment)
        {
            Label label = new Label
            {
                Content = content,
                Width = width,
                Height = height,
                FontFamily = new FontFamily("Times New Roman"),
                FontSize = 9,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = verticalAlignment,
            };
            Canvas.SetLeft(label, left);
            Canvas.SetTop(label, top);
            return label;
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
