using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.IO;

namespace TestProject
{
    public partial class Game : Window
    {
        List<Canvas> selectedCanvasesP = new List<Canvas>();
        List<Canvas> selectedCanvasesC = new List<Canvas>();
        Oyuncu player = new Oyuncu(10, "Oyuncu", 0);
        Oyuncu computer = new Oyuncu();

        List<Savas_Araclari> Cards = new List<Savas_Araclari>();
        List<Savas_Araclari> CardsComputer = new List<Savas_Araclari>();

        List<Savas_Araclari> playerSelectedCards = new List<Savas_Araclari>();
        List<Savas_Araclari> computerSelectedCards = new List<Savas_Araclari>();

        double sizeFactor = 1.2;
        private int currentStep = 0;
        private int currentRound = 0;
        private const int totalRounds = 5;

        public Game(String UserName)
        {
            InitializeComponent();
            Cards = player.KartSec();
            
            playerName.Text = UserName;
            LetsGo.IsEnabled = false;
            CardsComputer = computer.KartSec();

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
            String path = "/image/" + (show ? card.ToString() : "backSide") + ".png";
            Image cardImage = new Image
            {
                Source = new BitmapImage(new Uri(path, UriKind.Relative)),
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
                log("Error: Selected card is null.", 3);
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
                log("You can't select more than 3 cards!",3);
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
            addCanvases(Cards, 450, true);
            addCanvases(CardsComputer, -200, false);
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (playerSelectedCards.Count < 3)
            {
                log("Please select 3 cards before proceeding.",4);
                return;
            }
            playButton.IsEnabled = false;
            randomSelectComputerCards();
            RemoveUnselectedCards();
            StartStepByStepComparison();
            LetsGo.IsEnabled = true;
        }

        private async void StartStepByStepComparison()
        {
           
            for (currentStep = 0; currentStep < 3; currentStep++)
            {
                Cards.Remove(playerSelectedCards[currentStep]);
                CardsComputer.Remove(computerSelectedCards[currentStep]);


                // Compare cards and update their durability
                playerSelectedCards[currentStep].DurumGuncelle(computerSelectedCards[currentStep]);
                computerSelectedCards[currentStep].DurumGuncelle(playerSelectedCards[currentStep]);

                // Display step results
                log($"Step {currentStep + 1}:\n" +
                    $"Player's Card (ID: {playerSelectedCards[currentStep].ID}, Durability: {playerSelectedCards[currentStep].Dayaniklilik})\n" +
                    $"Computer's Card (ID: {computerSelectedCards[currentStep].ID}, Durability: {computerSelectedCards[currentStep].Dayaniklilik})");


                // Check for card eliminations
                if (playerSelectedCards[currentStep].Dayaniklilik <= 0)
                {
                    playerSelectedCards[currentStep].Dayaniklilik = 0;
                    log($"Computer's card (ID: {computerSelectedCards[currentStep].ID}) eliminates Player's card (ID: {playerSelectedCards[currentStep].ID})!");
                    computerSelectedCards[currentStep].Seviye_Puani += playerSelectedCards[currentStep].Seviye_Puani + 10;

                    ApplySelectedEffect(selectedCanvasesP[currentStep], Colors.Red);
                }else
                    Cards.Add(playerSelectedCards[currentStep]);

                if (computerSelectedCards[currentStep].Dayaniklilik <= 0)
                {
                    computerSelectedCards[currentStep].Dayaniklilik = 0;
                    log($"Player's card (ID: {playerSelectedCards[currentStep].ID}) eliminates Computer's card (ID: {computerSelectedCards[currentStep].ID})!");
                    playerSelectedCards[currentStep].Seviye_Puani += computerSelectedCards[currentStep].Seviye_Puani + 10;
                    ApplySelectedEffect(selectedCanvasesC[currentStep], Colors.Red);
                }
                else
                    CardsComputer.Add(computerSelectedCards[currentStep]);

                await Task.Delay(1000); // Add a delay for better visualization
            }

            log("Comparison complete!", 2);
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
                new KFS(0, "Kara", 20, 0, "KFS"),
            };
            Random random = new Random();
            Cards.Add(randomCards[random.Next(player.Skor >= 20 ? 6 : 3)]);
            CardsComputer.Add(randomCards[random.Next(computer.Skor >= 20 ? 6 : 3)]);

            if(Cards.Count==2)
                Cards.Add(randomCards[random.Next(player.Skor >= 20 ? 6 : 3)]);
            if (CardsComputer.Count == 2)
                CardsComputer.Add(randomCards[random.Next(computer.Skor >= 20 ? 6 : 3)]);

        }

        private void LetsGo_Click(object sender, RoutedEventArgs e)
        {
            LetsGo.IsEnabled = false;
            calculateScore();
            log($"Scores :\nComputer : {computer.Skor}\n{playerName.Text}: {player.Skor}");
            currentRound++;

            if (currentRound > totalRounds || Cards.Count == 0 || CardsComputer.Count == 0)
            {
                String winner;
                if (player.Skor == computer.Skor)
                {
                    int playerDurability = 0, computerDurability = 0;

                    foreach (var card in Cards)
                        playerDurability += card.Dayaniklilik;
                    foreach (var card in Cards)
                        computerDurability += card.Dayaniklilik;

                    winner = playerDurability > computerDurability ? playerName.Text : "Computer";

                }
                else 
                    winner = player.Skor > computer.Skor ? playerName.Text : "Computer";
                    
                    
                   
                log("Game Over!.");
                log($"Scores :\nComputer : {computer.Skor}\n{playerName.Text}: {player.Skor}");
                log($"THE WINNER IS {winner.ToUpper()}");

                Application.Current.Shutdown();
            }

            log($"Round {currentRound} complete! Cards have been updated.");

            // Add new cards to both the player and computer decks
            AddNewCards();

            // Clear the selected cards from previous rounds
            playerSelectedCards.Clear();
            computerSelectedCards.Clear();

            // Reshow the updated cards
            ReshowCards();
            playButton.IsEnabled = true;
        }

        private void calculateScore()
        {
            player.Skor = 0;
            computer.Skor = 0;
            foreach(var card in Cards)
            {
                player.Skor += card.Seviye_Puani;
            }

            foreach (var card in CardsComputer)
            {
                computer.Skor += card.Seviye_Puani;
            }
        }

        private void DisplayUpdatedDurability()
        {
            string playerDurability = string.Join(", ", playerSelectedCards.Select(card => card.Dayaniklilik.ToString()));
            string computerDurability = string.Join(", ", computerSelectedCards.Select(card => card.Dayaniklilik.ToString()));

            log($"Player Durability: {playerDurability}\nComputer Durability: {computerDurability}");
        }

        private void randomSelectComputerCards()
        {
            computerSelectedCards.Clear();
            Random random = new Random();

            while (computerSelectedCards.Count < 3)
            {
                int randIndex = random.Next(CardsComputer.Count);
                computerSelectedCards.Add(CardsComputer[randIndex]);
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
            showSelected(playerSelectedCards, 450, true);
            showSelected(computerSelectedCards, -200, false);
        }

        private void exit(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                Application.Current.Shutdown();
            }
        }

        static void log(string text,int mod=1)
        {
            // Open the file and write text
            using (StreamWriter writer = new StreamWriter("log.txt", append: true))
            {
                writer.WriteLine(text);
            }
            switch (mod){
                case 1:
                    MessageBox.Show(text);
                    break;
                case 2:
                    MessageBox.Show(text, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case 3:
                    MessageBox.Show(text, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 4:
                    MessageBox.Show(text,"Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;
            }
        }
    }
}

/*
 
         private void ClickImage_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Set the source of the MediaElement to the click sound file
            ClickSound.Source = new Uri("click-sound.wav", UriKind.Relative);
            
            // Play the sound
            ClickSound.Play();
        }
 */