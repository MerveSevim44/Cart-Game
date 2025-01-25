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
using System.Media;
using System.IO;
using System.Diagnostics;

namespace TestProject
{
    public partial class Game : Window
    {
        List<Canvas> selectedCanvasesP = new List<Canvas>();
        List<Canvas> selectedCanvasesC = new List<Canvas>();
        Oyuncu player = new Oyuncu(10, "Player", 0);
        Oyuncu computer = new Oyuncu();

        List<Savas_Araclari> Cards = new List<Savas_Araclari>();
        List<Savas_Araclari> CardsComputer = new List<Savas_Araclari>();

        List<Savas_Araclari> playerSelectedCards = new List<Savas_Araclari>();
        List<Savas_Araclari> computerSelectedCards = new List<Savas_Araclari>();
        private SoundPlayer ClickSound = new SoundPlayer();
        double sizeFactor = 1.0;
        private int currentStep = 0;
        private int currentRound = 0;
        private const int totalRounds = 5;

        public Game(String UserName)
        {
            InitializeComponent();
            Cards = player.KartSec();
            playerName.Text = UserName;
            CardsComputer = computer.KartSec();
            LetsGo.IsEnabled = false;
            if (File.Exists("log.txt")) File.Delete("log.txt");
            ReshowCards();
        }

        private void addCanvases(List<Savas_Araclari> cards, int yPosition, bool show)
        {

            double cardSpacing = 300 * sizeFactor;
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
            
            Canvas canvas = new Canvas
            {
                Margin = new Thickness(xPosition, yPosition, 0, 0),
                Width = 131 * sizeFactor,
                Height = 202 * sizeFactor,
                Tag = card
            };
            String path = "/image/" + (show ? card.ToString() : "backSide") + ".png";
            Image cardImage = new Image
            {
                Source = new BitmapImage(new Uri(path, UriKind.Relative)),
                Stretch = Stretch.Uniform,
                Width = 131 * sizeFactor,
                Height = 202 * sizeFactor,
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
            List<Label> labels = new List<Label> {
                CreateLabel(card.Dayaniklilik.ToString(), 49 * sizeFactor, 20 * sizeFactor, 85 * sizeFactor, 122 * sizeFactor, VerticalAlignment.Top) ,
                CreateLabel(card.ID.ToString(), 49 * sizeFactor, 20 * sizeFactor, 60 * sizeFactor, 105 * sizeFactor, VerticalAlignment.Top),
                CreateLabel(card.Seviye_Puani.ToString(), 49 * sizeFactor, 20 * sizeFactor, 90 * sizeFactor, 165 * sizeFactor, VerticalAlignment.Center)
            };

            card.KartPuaniGoster(canvas, card, labels);
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e, Savas_Araclari card)
        {
            if (card == null)
            {
                log("Error: Selected card is null.", 3);
                return;
            }

            int minSelecTimes = Cards.Min(c => c.selecTimes);

            int threshold = 0; 

            if (playerSelectedCards.Count < 3 && !playerSelectedCards.Contains(card))
            {

                if (card.selecTimes > minSelecTimes + threshold)
                {
                    log($"You can't select this card. Its selection count ({card.selecTimes}) is too high.", 3);
                    return;
                }

                playerSelectedCards.Add(card);
                card.selecTimes++;
                ApplySelectedEffect(sender as Canvas, Colors.Green);
                sound("Secme.wav");
            }
            else if (playerSelectedCards.Contains(card))
            {
                playerSelectedCards.Remove(card);

                card.selecTimes--;
                RemoveSelectedEffect(sender as Canvas);
                sound("Secme.wav");
            }
            else
            {
                log("You can't select more than 3 cards!", 3);
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
                FontSize = 9*sizeFactor,
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
            sound("Play.wav");
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
                playerSelectedCards[currentStep].DurumGuncelle(computerSelectedCards[currentStep]);
                computerSelectedCards[currentStep].DurumGuncelle(playerSelectedCards[currentStep]);

                log($"Step {currentStep + 1}:\n" +
                    $"Player's Card (ID: {playerSelectedCards[currentStep].ID}, Durability: {playerSelectedCards[currentStep].Dayaniklilik})\n" +
                    $"Computer's Card (ID: {computerSelectedCards[currentStep].ID}, Durability: {computerSelectedCards[currentStep].Dayaniklilik})");

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

                await Task.Delay(1000);
            }

            DisplayUpdatedDurability();
            log($"Scores :\nComputer : {computer.Skor}\n{playerName.Text}: {player.Skor}");
            log($"Round {currentRound+1} complete! Cards have been updated.", 2);
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

            sound("Letsgo.wav");
            LetsGo.IsEnabled = false;
            calculateScore();
            
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
                sound("Finish.wav");
                log($"THE WINNER IS {winner.ToUpper()}");
                Process.Start("log.txt");
                Application.Current.Shutdown();
            }


            AddNewCards();

            playerSelectedCards.Clear();
            computerSelectedCards.Clear();

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

            List<Savas_Araclari> cardsCopy = new List<Savas_Araclari>(CardsComputer);

            while (computerSelectedCards.Count < 3 && cardsCopy.Count > 0)
            {
                int randIndex = random.Next(cardsCopy.Count);
                computerSelectedCards.Add(cardsCopy[randIndex]);

                cardsCopy.RemoveAt(randIndex);
            }
        }


        private void showSelected(List<Savas_Araclari> cards, int yPosition, bool isPlayer)
        {
            double cardSpacing = 300 * sizeFactor; 
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

        static void log(string text,int status = 1)
        {
            using (StreamWriter writer = new StreamWriter("log.txt", append: true))
            {
                writer.WriteLine(text);
            }



            switch (status)
            {
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

    private void sound(string file)
        {
            try
            {
                string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file);
                if (System.IO.File.Exists(filePath))
                {
                    SoundPlayer player = new SoundPlayer(filePath);
                    player.Play();
                }
                else
                {
                    MessageBox.Show($"The file {file} was not found!", "File Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to play the sound: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
