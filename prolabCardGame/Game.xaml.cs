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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TestProject
{
    public partial class Game : Window
    {

        List<Canvas> selectedCanvasesP = new List<Canvas>();
        List<Canvas> selectedCanvasesC = new List<Canvas>();
        Oyuncu player = new Oyuncu(10, "Oyuncu", 0);
        Oyuncu computer = new Oyuncu();

        List<Savas_Araclari> Kartlar = new List<Savas_Araclari>();
        List<Savas_Araclari> KartlarComputer = new List<Savas_Araclari>();

        List<Savas_Araclari> playerSelectedCards = new List<Savas_Araclari>();
        List<Savas_Araclari> computerSelectedCards = new List<Savas_Araclari>();

        double sizeFactor = 1.0;
        private int currentStep = 0;
        private int currentRound = 0;
        private const int totalRounds = 5;

        public Game()
        {
            InitializeComponent();

            // Initialize player and computer cards
            Kartlar = player.KartSec();
            KartlarComputer = computer.KartSec();

            if (Kartlar == null || KartlarComputer == null || !Kartlar.Any() || !KartlarComputer.Any())
            {
                MessageBox.Show("Error: Cards could not be initialized. Check card selection logic.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ReshowCards();
        }

        private void addCanvases(List<Savas_Araclari> cards, int yPosition, bool show)
        {
            double cardSpacing = 300 * sizeFactor; // Space between each card
            double totalWidth = cards.Count * cardSpacing;
            int centerX = (int)SystemParameters.PrimaryScreenWidth / 2;
            double startX = centerX - totalWidth / 2 - 600;

            for (int i = 0; i < cards.Count; i++)
            {
                Canvas canvas = CreateCanvas(cards[i], startX + i * cardSpacing, yPosition, show);
                if (show)
                {
                    var currentCard = cards[i];
                    canvas.MouseLeftButtonDown += (sender, e) => Canvas_MouseLeftButtonDown(sender, e, currentCard);
                }
                MainGrid.Children.Add(canvas);
            }
        }

        private Canvas CreateCanvas(Savas_Araclari card, double xPosition, int yPosition, bool show)
        {
            if (card == null)
                throw new ArgumentNullException(nameof(card), "Card cannot be null.");

            Canvas canvas = new Canvas
            {
                Margin = new Thickness(xPosition, yPosition, 0, 0),
                Tag = card,
                Width = 131 * sizeFactor,
                Height = 202 * sizeFactor
            };

            Image cardImage = new Image
            {
                Source = new BitmapImage(new Uri("/image/" + (show ? card.ToString() : "backSide") + ".png", UriKind.Relative)),
                Stretch = Stretch.Uniform,
                Height = 202 * sizeFactor,
                Width = 131 * sizeFactor,
                Tag = card
            };

            Canvas.SetLeft(cardImage, 26 * sizeFactor);
            Canvas.SetTop(cardImage, 20 * sizeFactor);
            canvas.Children.Add(cardImage);

            if (show)
            {
                AddCardLabels(canvas, card);
            }

            return canvas;
        }

        private void AddCardLabels(Canvas canvas, Savas_Araclari card)
        {
            if (card == null) return;

            Label label1 = CreateLabel(card.Dayaniklilik.ToString(), 49 * sizeFactor, 20 * sizeFactor, 85 * sizeFactor, 122 * sizeFactor, VerticalAlignment.Top);
            Label label2 = CreateLabel(card.ID.ToString(), 49 * sizeFactor, 20 * sizeFactor, 60 * sizeFactor, 105 * sizeFactor, VerticalAlignment.Top);
            Label label3 = CreateLabel(card.Seviye_Puani.ToString(), 49 * sizeFactor, 20 * sizeFactor, 90 * sizeFactor, 165 * sizeFactor, VerticalAlignment.Center);

            canvas.Children.Add(label1);
            canvas.Children.Add(label2);
            canvas.Children.Add(label3);
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e, Savas_Araclari card)
        {
            if (card == null)
            {
                MessageBox.Show("Error: Selected card is null.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (playerSelectedCards.Count < 3 && !playerSelectedCards.Contains(card))
            {
                playerSelectedCards.Add(card);
                ApplySelectedEffect(sender as Canvas, Colors.Green);
            }
            else if (playerSelectedCards.Contains(card))
            {
                playerSelectedCards.Remove(card);
                RemoveSelectedEffect(sender as Canvas);
            }
            else
            {
                MessageBox.Show("You can't select more than 3 cards!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ApplySelectedEffect(Canvas canvas, Color color)
        {
            if (canvas == null) return;

            canvas.Effect = new DropShadowEffect
            {
                Color = color,
                ShadowDepth = 0,
                BlurRadius = 40,
                Opacity = 1
            };
        }

        private void RemoveSelectedEffect(Canvas canvas)
        {
            if (canvas == null) return;
            canvas.Effect = null;
        }

        private Label CreateLabel(string content, double width, double height, double left, double top, VerticalAlignment verticalAlignment)
        {
            return new Label
            {
                Content = content,
                Width = width,
                Height = height,
                FontFamily = new FontFamily("Times New Roman"),
                FontSize = 9,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = verticalAlignment,
                Margin = new Thickness(left, top, 0, 0)
            };
        }

        private void ReshowCards()
        {
            MainGrid.Children.OfType<Canvas>().ToList().ForEach(canvas => MainGrid.Children.Remove(canvas));
            addCanvases(Kartlar, 500, true);
            addCanvases(KartlarComputer, -600, false);
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (playerSelectedCards.Count < 3)
            {
                MessageBox.Show("Please select 3 cards before proceeding.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            randomSelectComputerCards();
            RemoveUnselectedCards();
            StartStepByStepComparison();
        }
        private async void StartStepByStepComparison()
        {
            for (currentStep = 0; currentStep < 3; currentStep++)
            {
                // Ensure there are enough cards to compare
                if (currentStep >= playerSelectedCards.Count || currentStep >= computerSelectedCards.Count)
                {
                    MessageBox.Show("Error: Insufficient cards for comparison. Please check the card selection logic.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Savas_Araclari playerCard = playerSelectedCards[currentStep];
                Savas_Araclari computerCard = computerSelectedCards[currentStep];

                // Compare cards and update their durability
                playerCard.DurumGuncelle(computerCard);
                computerCard.DurumGuncelle(playerCard);

                // Display step results
                MessageBox.Show($"Step {currentStep + 1}:\n" +
                    $"Player's Card (ID: {playerCard.ID}, Durability: {playerCard.Dayaniklilik})\n" +
                    $"Computer's Card (ID: {computerCard.ID}, Durability: {computerCard.Dayaniklilik})");

                // Check for card eliminations
                if (playerCard.Dayaniklilik <= 0)
                {
                    MessageBox.Show($"Computer's card (ID: {computerCard.ID}) eliminates Player's card (ID: {playerCard.ID})!");
                    Kartlar.Remove(playerCard);
                    computerCard.Seviye_Puani += playerCard.Seviye_Puani + 10;
                    ApplySelectedEffect(selectedCanvasesP[currentStep], Colors.Red);
                }

                if (computerCard.Dayaniklilik <= 0)
                {
                    MessageBox.Show($"Player's card (ID: {playerCard.ID}) eliminates Computer's card (ID: {computerCard.ID})!");
                    KartlarComputer.Remove(computerCard);
                    playerCard.Seviye_Puani += computerCard.Seviye_Puani + 10;
                    ApplySelectedEffect(selectedCanvasesC[currentStep], Colors.Red);
                }

                await Task.Delay(1000); // Add a delay for better visualization
            }

            MessageBox.Show("Comparison complete!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            DisplayUpdatedDurability();
        }

        private void AddNewCards()
        {
            var randomCards = new List<Savas_Araclari>
        {
            new Ucak(0, "Hava", 20, 0, "Ucak"),
            new Obus(0, "Kara", 20, 0, "Obus"),
            new Firakteyn(0, "Deniz", 20, 0, "Firkateyn"),
            new Siha(0, "Hava", 20, 0, "Siha"),
            new Sida(0, "Deniz", 20, 0, "Sida"),
            new KFS(0, "Kara", 20, 0, "KFS")
        };

            Random random = new Random();
            Kartlar.Add(randomCards[random.Next(player.Skor > 20 ? 6 : 3)]);
            KartlarComputer.Add(randomCards[random.Next(computer.Skor > 20 ? 6 : 3)]);
        }
        private void LetsGo_Click(object sender, RoutedEventArgs e)
        {
            currentRound++;

            if (currentRound > totalRounds)
            {
                MessageBox.Show("Game Over! The maximum number of rounds has been reached.");
                this.Close(); // Close the game window
                return;
            }

            MessageBox.Show($"Round {currentRound} complete! Cards have been updated.");

            // Add new cards to both the player and computer decks
            AddNewCards();

            // Clear the selected cards from previous rounds
            playerSelectedCards.Clear();
            computerSelectedCards.Clear();

            // Reshow the updated cards
            ReshowCards();
        }


        private void DisplayUpdatedDurability()
        {
            string playerDurability = string.Join(", ", playerSelectedCards.Select(card => card.Dayaniklilik.ToString()));
            string computerDurability = string.Join(", ", computerSelectedCards.Select(card => card.Dayaniklilik.ToString()));

            MessageBox.Show($"Player Durability: {playerDurability}\nComputer Durability: {computerDurability}");
        }


        private void randomSelectComputerCards()
        {
            computerSelectedCards.Clear();
            Random random = new Random();

            while (computerSelectedCards.Count < 3)
            {
                int randIndex = random.Next(KartlarComputer.Count);
                computerSelectedCards.Add(KartlarComputer[randIndex]);
            }
        }

        private void showSelected(List<Savas_Araclari> cards, int yPosition, bool isPlayer)
        {
            double cardSpacing = 300 * sizeFactor; // Space between each card
            double totalWidth = cards.Count * cardSpacing;
            int centerX = (int)SystemParameters.PrimaryScreenWidth / 2;
            double startX = centerX - totalWidth / 2 - 600;

            for (int i = 0; i < cards.Count; i++)
            {
                Canvas canvas = CreateCanvas(cards[i], startX + i * cardSpacing, yPosition, true);
                if (isPlayer)
                {
                    selectedCanvasesP.Add(canvas);
                }
                else
                    selectedCanvasesC.Add(canvas);

                MainGrid.Children.Add(canvas);
            }
        }
        private void RemoveUnselectedCards()
        {
            selectedCanvasesP.Clear();
            selectedCanvasesC.Clear();
            MainGrid.Children.OfType<Canvas>().ToList().ForEach(canvas => MainGrid.Children.Remove(canvas));
            showSelected(playerSelectedCards, 500, true);
            showSelected(computerSelectedCards, -600, false);
        }
        private void exit(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
