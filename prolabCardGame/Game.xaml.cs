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
        // Define the list of selected canvases
        List<Canvas> SecilmisKartlar = new List<Canvas>();
        List<Savas_Araclari> Kartlar;
        public Game()
        {
            InitializeComponent();
            Oyuncu player = new Oyuncu();
            Kartlar = player.KartSec();

            addCanvases(Kartlar);
        }
        private void addCanvases(List<Savas_Araclari> Kartlar)
        {
            for (int i = MainGrid.Children.Count - 1; i >= 0; i--)
            {
                if (MainGrid.Children[i] is Canvas canvas)
                {
                    MainGrid.Children.RemoveAt(i);  // Remove the canvas
                }
            }

            int cardSpacing = 150; // Space between each card
            int totalWidth = Kartlar.Count * cardSpacing; // Total width without considering the card width

            // Calculate the starting X position to center the cards on the screen
            int centerX = 1024 / 2; // Assuming screen width is 1024
            int startX = centerX - totalWidth / 2 + 60;

            for (int i = 0; i < Kartlar.Count; i++)
            {
                Canvas canvas = CreateCanvas(Kartlar[i], startX + i * cardSpacing);
                MainGrid.Children.Add(canvas);
            }
        }
        private Canvas CreateCanvas(Savas_Araclari card, int xPosition)
        {
            // Create Canvas
            Canvas canvas = new Canvas
            {
                Margin = new Thickness(xPosition, 454, 0, 20), // Position the canvas
                Tag = card // Store the card object in the Tag property
            };

            // Create Image
            Image cardImage = new Image
            {
                Source = new BitmapImage(new Uri("/image/" + card.ToString() + ".png", UriKind.Relative)),
                Stretch = Stretch.Uniform,
                Height = 202,
                Width = 131,
                Tag = card.ToString() + "P" // Optional: Tag the image too
            };
            Canvas.SetLeft(cardImage, 26);
            Canvas.SetTop(cardImage, 20);
            canvas.Children.Add(cardImage);

            // Create Labels
            Label label1 = CreateLabel(card.Dayaniklilik + "", 49, 20, 85, 122, VerticalAlignment.Top);
            canvas.Children.Add(label1);

            Label label2 = CreateLabel(card.ID + "", 49, 20, 60, 105, VerticalAlignment.Top);
            canvas.Children.Add(label2);

            Label label3 = CreateLabel(card.Seviye_Puani + "", 49, 20, 90, 165, VerticalAlignment.Center);
            canvas.Children.Add(label3);

            // Attach the event handler to the Canvas
            canvas.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;

            return canvas;
        }


        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var canvas = sender as Canvas;


            if (SecilmisKartlar.Count!=3 && canvas != null && !SecilmisKartlar.Contains(canvas))
            {
                SecilmisKartlar.Add(canvas); // Add the canvas to the list
            }
            else if(SecilmisKartlar.Contains(canvas))
                    SecilmisKartlar.Remove(canvas);

            



             // Generate a list of all IDs from the selected cards
             var selectedCardIDs = SecilmisKartlar
                .Select(c => (c.Tag as Savas_Araclari)?.ID) // Extract the ID from the Tag property
                .Where(id => id != null) // Ensure ID is not null
                .ToList();

            // Display all selected IDs in a MessageBox
            MessageBox.Show("Seçilmiş Kartların ID'leri:\n" + string.Join(", ", selectedCardIDs));
            

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


    }
}
