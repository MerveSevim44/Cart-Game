using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TestProject
{

    public partial class Game : Window
    {
        // Define the list of selected canvases
        List<Canvas> SecilmisKartlar = new List<Canvas>();
        List<Savas_Araclari> Kartlar;
        List<Savas_Araclari> KartlarComputer;


        public Game()
        {
            InitializeComponent();
            

            Oyuncu player = new Oyuncu(10, "Oyuncu", 0);
            Kartlar = player.KartSec();
            
            Oyuncu computer = new Oyuncu();
            KartlarComputer = computer.KartSec();
            

            addCanvases(Kartlar);
            ShowCardsComputer(KartlarComputer);
        }


        private void ShowCardsComputer(List<Savas_Araclari> card)
        {
            int cardSpacing = 300; // Space between each card
            int cardWidth = 131;   // Width of each card
            int totalWidth = (card.Count - 1) * cardSpacing + cardWidth; // Total width of all cards including spacing

            // Screen dimensions - dynamically get the width and height of MainGrid
            double screenWidth = MainGrid.ActualWidth; // Width of the MainGrid
            double screenHeight = MainGrid.ActualHeight; // Height of the MainGrid

            // Center the cards horizontally
            double centerX = screenWidth / 2;
            double startX = centerX - totalWidth / 2 + 60; // Calculate starting X position for centering

            // Center the cards vertically (optional)
            double centerY = screenHeight / 2;
            double cardYPosition = centerY - 450; // Adjust -100 if needed for aesthetics

            for (int i = 0; i < card.Count; i++)
            {
                Image cardImage = new Image
                {
                    Margin = new Thickness(startX + i * cardSpacing, cardYPosition, 0, 0),
                    Source = new BitmapImage(new Uri("/image/backSide.png", UriKind.Relative)),
                    Stretch = Stretch.Uniform,
                    Height = 202,
                    Width = cardWidth,
                    Tag = card.ToString() + "C" // Optional: Tag the image for identification
                };

                // Add the card image to MainGrid
                MainGrid.Children.Add(cardImage);
            }
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
                Margin = new Thickness(xPosition,450, 0, 20), // Position the canvas
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


            // Attach event handlers for hover effects
            canvas.MouseEnter += Canvas_MouseEnter;
            canvas.MouseLeave += Canvas_MouseLeave;


            // Attach the event handler to the Canvas
            canvas.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;

            return canvas;
        }

        private Canvas placeCanvas(Canvas canvas, int xPosition)
        {
            canvas.Margin = new Thickness(xPosition, 454, 0, 20);
            // Create Canvas
      
            // Attach event handlers for hover effects
            canvas.MouseEnter += Canvas_MouseEnter;
            canvas.MouseLeave += Canvas_MouseLeave;

            return canvas;
        }
        private void Canvas_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Canvas canvas)
            {
                // Apply hover effect: Add a yellow glow
                canvas.Effect = new DropShadowEffect
                {
                    Color = Colors.FloralWhite, // Glow color
                    BlurRadius = 10,       // Intensity of the glow
                    ShadowDepth = 0        // No shadow offset
                };

                // Optionally scale up the canvas slightly
                canvas.RenderTransform = new ScaleTransform(1.0, 1.0);
                canvas.RenderTransformOrigin = new Point(0.5, 0.5);
            }
        }

        private void Canvas_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Canvas canvas)
            {
                // Remove hover effect: Reset the glow
                canvas.Effect = null;

                // Reset the scale
                canvas.RenderTransform = null;
            }
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var canvas = sender as Canvas;

            if (canvas != null)
            {
                // Add or remove the selected canvas from the list
                if (!SecilmisKartlar.Contains(canvas) && SecilmisKartlar.Count < 3)
                {
                    SecilmisKartlar.Add(canvas);

                    // Apply "selected" effect (red outline)
                    ApplySelectedEffect(canvas);
                }
                else if (SecilmisKartlar.Contains(canvas))
                {

                    SecilmisKartlar.Remove(canvas);

                    // Remove "selected" effect (red outline)
                    RemoveSelectedEffect(canvas);
                }
                else if (SecilmisKartlar.Count == 3)
                {
                    MessageBox.Show("You can't select more than 3 cards!");
                }
            }
        }

        private void ApplySelectedEffect(Canvas canvas)
        {
            // Create a DropShadowEffect with a red shadow
            var redOutlineEffect = new DropShadowEffect
            {
                Color = Colors.Red,
                ShadowDepth = 0, // Center the shadow around the object
                BlurRadius = 10, // Adjust the size of the outline
                Opacity = 1 // Fully opaque
            };

            // Apply the effect to the Canvas
            canvas.Effect = redOutlineEffect;
        }

        private void RemoveSelectedEffect(Canvas canvas)
        {
            // Remove the effect by setting it to null
            canvas.Effect = null;
        }




        private void RemoveUnselectedCards()
        {
            for (int i = MainGrid.Children.Count - 1; i >= 0; i--)
            {
                if (MainGrid.Children[i] is Canvas canvas)
                {
                    MainGrid.Children.RemoveAt(i);  // Remove the canvas
                }
            }


            int cardSpacing = 150; // Space between each card
            int totalWidth = SecilmisKartlar.Count * cardSpacing; // Total width without considering the card width

            // Calculate the starting X position to center the cards on the screen
            int centerX = 1024 / 2; // Assuming screen width is 1024
            int startX = centerX - totalWidth / 2 + 60;

            for (int i = 0; i < SecilmisKartlar.Count; i++)
            {
                Canvas canvas = placeCanvas(SecilmisKartlar[i], startX + i * cardSpacing);
                MainGrid.Children.Add(canvas);
            }


            MessageBox.Show("3 Kart seçildi! Diğer kartlar silindi.");
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

       
        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if(SecilmisKartlar.Count<3)
            {
                MessageBox.Show("Please choose a choise ! ", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; 
            }


            else
            {
                RemoveUnselectedCards(); // Remove other cards
            }
        }
    }
}
