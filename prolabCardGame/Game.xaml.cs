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
        List<Savas_Araclari> Kartlar;
        List<Savas_Araclari> KartlarComputer;

        List<Savas_Araclari> playerSelectedCards = new List<Savas_Araclari>();
        List<Savas_Araclari> computerSelectedCards = new List<Savas_Araclari>();

        public Game()
        {
            InitializeComponent();

            Oyuncu player = new Oyuncu(10, "Oyuncu", 0);
            Kartlar = player.KartSec();

            Oyuncu computer = new Oyuncu();
            KartlarComputer = computer.KartSec();

            

            addCanvases(Kartlar, 450 , true); // Show player cards at Y position 450
            addCanvases(KartlarComputer, 10 , false); // Show computer cards at Y position 100
        }

        private void addCanvases(List<Savas_Araclari> Kartlar, int yPosition , bool show)
        {

            int cardSpacing = 150; // Space between each card
            int totalWidth = Kartlar.Count * cardSpacing;

            int centerX = 1024 / 2; // Assuming screen width is 1024
            int startX = centerX - totalWidth / 2 + 60;

            for (int i = 0; i < Kartlar.Count; i++)
            {
                Canvas canvas = CreateCanvas(Kartlar[i], startX + i * cardSpacing, yPosition , show);
                MainGrid.Children.Add(canvas);
            }
        }

        private Canvas CreateCanvas(Savas_Araclari card, int xPosition, int yPosition, bool show)
        {
            Canvas canvas = new Canvas
            {
                Margin = new Thickness(xPosition, yPosition, 0, 0),
                Tag = card // Store the card object in the Tag property
            };

            Image cardImage = new Image
            {
                Source = new BitmapImage(new Uri("/image/" + (show ? card.ToString() : "backSide") + ".png", UriKind.Relative)),
                Stretch = Stretch.Uniform,
                Height = 202,
                Width = 131,
                Tag = card // Store the card object in the Tag
            };
            Canvas.SetLeft(cardImage, 26);
            Canvas.SetTop(cardImage, 20);
            canvas.Children.Add(cardImage);

            // Add labels (with visibility control for computer cards)
            Label label1 = CreateLabel(card.Dayaniklilik + "", 49, 20, 85, 122, VerticalAlignment.Top);
            label1.Visibility = show ? Visibility.Visible : Visibility.Hidden; // Hide labels for computer
            canvas.Children.Add(label1);

            Label label2 = CreateLabel(card.ID + "", 49, 20, 60, 105, VerticalAlignment.Top);
            label2.Visibility = show ? Visibility.Visible : Visibility.Hidden; // Hide labels for computer
            canvas.Children.Add(label2);

            Label label3 = CreateLabel(card.Seviye_Puani + "", 49, 20, 90, 165, VerticalAlignment.Center);
            label3.Visibility = show ? Visibility.Visible : Visibility.Hidden; // Hide labels for computer
            canvas.Children.Add(label3);

            // Attach event handler only for player's cards
            if (show)
            {
                canvas.MouseLeftButtonDown += (sender, e) => Canvas_MouseLeftButtonDown(sender, e, card);
            }

            return canvas;
        }



        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e, Savas_Araclari card)
        {
            if (!playerSelectedCards.Contains(card) && playerSelectedCards.Count < 3)
            {
                playerSelectedCards.Add(card);
                ApplySelectedEffect(sender as Canvas);
            }
            else if (playerSelectedCards.Contains(card))
            {
               
                playerSelectedCards.Remove(card);
                RemoveSelectedEffect(sender as Canvas);
            }
            else if (playerSelectedCards.Count == 3)
            {
                MessageBox.Show("You can't select more than 3 cards!");
            }
           


        }

        private void ApplySelectedEffect(Canvas canvas)
        {
            if (canvas == null) return;

            var greenOutlineEffect = new DropShadowEffect
            {
                Color = Colors.Green,
                ShadowDepth = 0,
                BlurRadius = 40,
                Opacity = 1
            };
            canvas.Effect = greenOutlineEffect;
        }

        private void RemoveSelectedEffect(Canvas canvas)
        {
            if (canvas == null) return;
            canvas.Effect = null;
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
                VerticalAlignment = verticalAlignment
            };
            Canvas.SetLeft(label, left);
            Canvas.SetTop(label, top);
            return label;
        }

        private void randomSelectCopmputerCards()
        {
            computerSelectedCards.Clear();
            Random random = new Random();
            List<Savas_Araclari> availableCards = new List<Savas_Araclari>(KartlarComputer);

            while (computerSelectedCards.Count < 3 && availableCards.Count > 0)
            {
                int index = random.Next(availableCards.Count);
                computerSelectedCards.Add(availableCards[index]);
                availableCards.RemoveAt(index);
            }
        }

        private void RemoveUnselectedCards()
        {
            for (int i = MainGrid.Children.Count - 1; i >= 0; i--)
            {
                if (MainGrid.Children[i] is Canvas || MainGrid.Children[i] is Image)
                {
                    MainGrid.Children.RemoveAt(i);
                }
            }

            int playerCardSpacing = 150;
            int playerStartX = (1024 / 2) - (playerSelectedCards.Count * playerCardSpacing / 2) + 60;
            int playerYPosition = 450;

            for (int i = 0; i < playerSelectedCards.Count; i++)
            {
                Canvas playerCanvas = CreateCanvas(playerSelectedCards[i], playerStartX + i * playerCardSpacing, playerYPosition, true);
                MainGrid.Children.Add(playerCanvas);
            }

            int computerCardSpacing = 150;
            int computerStartX = (1024 / 2) - (computerSelectedCards.Count * computerCardSpacing / 2) + 60;
            int computerYPosition = 10;

            for (int i = 0; i < computerSelectedCards.Count; i++)
            {
                Canvas computerCanvas = CreateCanvas(computerSelectedCards[i], computerStartX + i * computerCardSpacing, computerYPosition, true);

                // Make labels visible for computer cards
                foreach (var child in computerCanvas.Children)
                {
                    if (child is Label label)
                    {
                        label.Visibility = Visibility.Visible; // Reveal labels after Play button
                    }
                }

                MainGrid.Children.Add(computerCanvas);
            }
        }


        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (playerSelectedCards.Count < 3)
            {
                MessageBox.Show("Please choose a choice!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

           
            else
            {
                randomSelectCopmputerCards();
                RemoveUnselectedCards();
            }
        }
    }


}
