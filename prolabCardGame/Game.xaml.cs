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
        List<Savas_Araclari> Kartlar;
        List<Savas_Araclari> KartlarComputer;

        List<Savas_Araclari> playerSelectedCards = new List<Savas_Araclari>();
        List<Savas_Araclari> computerSelectedCards = new List<Savas_Araclari>();

        List <Savas_Araclari> newCardComputer = new List<Savas_Araclari>();
        List<Savas_Araclari> newCardPlayer = new List<Savas_Araclari>();
        List<Savas_Araclari> allPossibleCards = new List<Savas_Araclari>();
        private Dictionary<Savas_Araclari, int> cardSelectionCounts = new Dictionary<Savas_Araclari, int>();

        public Game()
        {
            InitializeComponent();

            // Initialize all possible cards (this depends on your Savas_Araclari class definition)
            allPossibleCards = InitializeAllPossibleCards();

            Oyuncu player = new Oyuncu(10, "Oyuncu", 0);
            Kartlar = player.KartSec();
            allPossibleCards = allPossibleCards.Except(Kartlar).ToList(); // Remove player's cards from the pool

            Oyuncu computer = new Oyuncu();
            KartlarComputer = computer.KartSec();
            allPossibleCards = allPossibleCards.Except(KartlarComputer).ToList(); // Remove computer's cards from the pool

            addCanvases(Kartlar, 450, true); // Show player cards at Y position 450
            addCanvases(KartlarComputer, 10, false); // Show computer cards at Y position 10
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

                //// Update selection count
                //if (cardSelectionCounts.ContainsKey(card))
                //{
                //    cardSelectionCounts[card]++;
                //}
                //else
                //{
                //    cardSelectionCounts[card] = 1;
                //}
            }
            else if (playerSelectedCards.Contains(card))
            {
                playerSelectedCards.Remove(card);
                RemoveSelectedEffect(sender as Canvas);
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
            List<Savas_Araclari> allKartlarP =new  List<Savas_Araclari>(Kartlar);
            List<Savas_Araclari> allKartlarC = new List<Savas_Araclari>(KartlarComputer);
            // Clear the current cards from the grid
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

            // Add the player's selected cards to the grid
            for (int i = 0; i < playerSelectedCards.Count; i++)
            {
                Canvas playerCanvas = CreateCanvas(playerSelectedCards[i], playerStartX + i * playerCardSpacing, playerYPosition, true);
                MainGrid.Children.Add(playerCanvas);
            }

            // Add the player's new card to the grid
            if (newCardPlayer.Count > 0)
            {
                int newCardX = playerStartX + playerSelectedCards.Count * playerCardSpacing;
                Canvas newPlayerCanvas = CreateCanvas(newCardPlayer.Last(), newCardX, playerYPosition, true);
                MainGrid.Children.Add(newPlayerCanvas);
            }

            int computerCardSpacing = 150;
            int computerStartX = (1024 / 2) - (computerSelectedCards.Count * computerCardSpacing / 2) + 60;
            int computerYPosition = 10;

            // Add the computer's selected cards to the grid
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

            // Add the computer's new card to the grid
            if (newCardComputer.Count > 0)
            {
                int newCardX = computerStartX + computerSelectedCards.Count * computerCardSpacing;
                Canvas newComputerCanvas = CreateCanvas(newCardComputer.Last(), newCardX, computerYPosition, false);
                MainGrid.Children.Add(newComputerCanvas);
            }

            Kartlar = new List<Savas_Araclari>(allKartlarP);
            KartlarComputer = new List<Savas_Araclari>(allKartlarC);
        }

        private DispatcherTimer stepTimer;
        private int currentStep = 0;


        private void CompareAndUpdateStep()
        {
            if (currentStep < 3)
            {
                Savas_Araclari playerCard = playerSelectedCards[currentStep];
                Savas_Araclari computerCard = computerSelectedCards[currentStep];

                // Compare and update cards' durability
                playerCard.DurumGuncelle(computerCard);
                computerCard.DurumGuncelle(playerCard);

                // Show results for this step
                MessageBox.Show($"Step {currentStep + 1}:\n" +
                    $"Player's card ID {playerCard.ID}, Durability: {playerCard.Dayaniklilik}  \n" +
                    $"Computer's card ID {computerCard.ID}, Durability: {computerCard.Dayaniklilik}  \n  ");

                // Check and remove eliminated cards
                if (computerCard.Dayaniklilik <= 0)
                {
                    MessageBox.Show($"Player's card ID {playerCard.ID} eliminates Computer's card ID {computerCard.ID} level score: {playerCard.Seviye_Puani} ");
                    KartlarComputer.Remove(computerCard);
                }

                if (playerCard.Dayaniklilik <= 0)
                {
                    MessageBox.Show($"Computer's card ID {computerCard.ID} eliminates Player's card ID {playerCard.ID} level score: {computerCard.Seviye_Puani}");
                    Kartlar.Remove(playerCard);
                }

                // Move to the next step
                currentStep++;
            }
        }



        private void StartStepByStepComparison()
        {
            currentStep = 0; // Reset step counter

            stepTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2) // 2 seconds between each step
            };

            stepTimer.Tick += (s, e) =>
            {
                CompareAndUpdateStep();

                // Stop the timer when all steps are completed
                if (currentStep >= 3)
                {
                    stepTimer.Stop();
                    MessageBox.Show("Comparison complete!");
                    DisplayUpdatedDurability(); // Show final durability
                }
            };

            stepTimer.Start(); // Start the timer
        }





        // Example method to display updated durability
        private void DisplayUpdatedDurability()
        {
            string playerDurability = string.Join(", ", playerSelectedCards.Select(card => card.Dayaniklilik.ToString()));
            string computerDurability = string.Join(", ", computerSelectedCards.Select(card => card.Dayaniklilik.ToString()));

            MessageBox.Show($"Player Durability: {playerDurability}\nComputer Durability: {computerDurability}");
      }



        private List<Savas_Araclari> InitializeAllPossibleCards()
        {
            List<Savas_Araclari> allCards = new List<Savas_Araclari>();

            // Example: Add different types of cards
            for (int i = 1; i <= 5; i++) // Add 5 Ucak cards for example
            {
                allCards.Add(new Ucak(0, "Hava", 20, 0, "Ucak"));
                allCards.Add(new Obus(0, "Kara", 20, 0, "Obus"));
                allCards.Add(new Firakteyn(0, "Deniz", 25, 0, "Firkateyn"));
            }



            return allCards;
        }


        private Savas_Araclari GetUniqueNewCard()
        {
            if (allPossibleCards.Count == 0)
            {
                throw new InvalidOperationException("No more unique cards available!");
            }

            Random random = new Random();
            int index = random.Next(allPossibleCards.Count); // Pick a random card from the pool
            Savas_Araclari newCard = allPossibleCards[index];
            allPossibleCards.RemoveAt(index); // Remove the card from the pool to avoid duplicates
            return newCard;
        }

        private void AddNewCards()
        {
            // Add new card for player
            Savas_Araclari newPlayerCard = GetUniqueNewCard();
            Kartlar.Add(newPlayerCard); // Add to player's card list
            playerSelectedCards.Add(newPlayerCard);

            // Add new card for computer
            Savas_Araclari newComputerCard = GetUniqueNewCard();
            KartlarComputer.Add(newComputerCard); // Add to computer's card list
            computerSelectedCards.Add(newComputerCard);
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
                AddNewCards(); // Add new unique cards
                RemoveUnselectedCards();
                // Compare and update the cards
                StartStepByStepComparison();

            }
        }


        private void ReshowCards()
        {
            // Remove all children except the Play and Let's Go buttons
            foreach (var child in MainGrid.Children.OfType<UIElement>().ToList())
            {
                // Keep Play and Let's Go buttons by checking their names
                if (child is Button button)
                {
                    if (button.Name != "Play" && button.Name != "LetsGo")
                    {
                        MainGrid.Children.Remove(child);
                    }
                }
                else
                {
                    // Remove other UI elements (e.g., canvases, images) from the grid
                    MainGrid.Children.Remove(child);
                }
            }
         
            // Re-show player's remaining cards
            addCanvases(Kartlar, 450, true);  // Player cards at Y position 450

            // Re-show computer's remaining cards
            addCanvases(KartlarComputer, 10, false);  // Computer cards at Y position 100
        }


        private int currentRound = 0;
        private const int totalRounds = 5;

        private void LetsGo_Click(object sender, RoutedEventArgs e)
        {
            // Increment the current round
            currentRound++;

            // Check if the game is over
            if (currentRound > totalRounds)
            {
                MessageBox.Show("Game Over! The maximum rounds are reached.");
                return;
            }

            // Step 2: Re-show the current remaining cards for both the player and the computer
            ReshowCards();

            // Step 3: Proceed with the next round logic (e.g., Player chooses cards)
            if (currentRound <= totalRounds)
            { 
                // Computer randomly selects cards for the new round
                randomSelectCopmputerCards();

            }

            // Optionally show round-specific message or visual update
            MessageBox.Show($"Round {currentRound} complete! Cards have been updated.");
           
        }
    }


}
