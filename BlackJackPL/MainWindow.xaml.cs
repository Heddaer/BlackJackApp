// Author: Hedda Eriksson
// Date: 2023-10-17
// Malmö University, Bachelor of Engineering - Computer Science & Telecommunications
// Description: Main window is the playground for Black Jack app.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using BlackJackBLL;
using BlackJackDAL.Migrations;
using UtilityLib.Data_Management;
using UtilityLib.Utilities;

namespace BlackJackPL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string IMAGE_PATH = "\\\\Mac\\Home\\Documents\\VSProjects\\BlackJackApp\\BlackJackPL\\Images\\Deck\\";
        //private string IMAGE_PATH = Path.GetFullPath("Deck"); //HERE    -> unmark above.
        private const string IMAGE_EXT = ".png";
        private NewGameWindow newGameWindow;
        private GameManager gameManager;
        private GameOverBox gameOver;

        public MainWindow()
        {
            InitializeComponent();
            InitWindows();
            newGameWindow.ShowDialog();

        }

        private void InitWindows()
        {
            newGameWindow = new NewGameWindow();    //Window for new game with new players
            newGameWindow.StartGameRequest += StartGame;

            gameOver = new GameOverBox();   //User control for MainWindow
            gameOver.Visibility = Visibility.Hidden;
            Grid.SetColumn(gameOver, 4);
            Grid.SetRow(gameOver, 2);
            Grid.SetRowSpan(gameOver, 5);
            grid.Children.Add(gameOver);

        }

        private void StartGame(object sender, EventArgs e)
        {
            gameManager = newGameWindow.GameManager; //Get instance of GameManager from NewGameWindow

            //Events
            gameManager.NextPlayerRequest += NextPlayer;
            gameManager.MessageRequest += CustomMessage;
            gameManager.AddCardRequest += AddCard;
            gameManager.StartGameRequest += InitGameBoard;
            gameManager.AnnounceWinnerRequest += RoundFinished;
            gameManager.ShowDealerCardRequest += ShowDealerCard;
            gameOver.ContinueClicked += ContinueNextRound;
            gameOver.StopClicked += Reset;

            InitGameBoard(sender, e); //this was in here before
        }

        public void InitGameBoard(object sender, EventArgs e)
        {
            List<string> hand = new List<string>();
            int count = 0;

            if (gameManager.Players != null)
            {
                int nbrOfPlayers = gameManager.Players.Count;

                txtPlayerID.Text = gameManager.Players.GetAt(1).Username; //First player in list starts

                hand = gameManager.GetCardsInHand(0, out count);
                SetUpCards(PlayersCard.DealerCard1, hand[0]); //Dealer only shows one card until the players is finished
                SetUpCards(PlayersCard.DealerCard2, "default");

                if (nbrOfPlayers >= 2) //Dealer is always first player (index 0) in List of players
                {
                    txtPlayer1.Text = gameManager.Players.GetAt(1).Username;
                    hand = gameManager.GetCardsInHand(1, out count);
                    SetUpCards(PlayersCard.P1Card1, hand[0]);
                    SetUpCards(PlayersCard.P1Card2, hand[1]);
                }
                if (nbrOfPlayers >= 3)
                {
                    txtPlayer2.Text = gameManager.Players.GetAt(2).Username;
                    hand = gameManager.GetCardsInHand(2, out count);
                    SetUpCards(PlayersCard.P2Card1, hand[0]);
                    SetUpCards(PlayersCard.P2Card2, hand[1]);
                }
                if (nbrOfPlayers == 4)
                {
                    txtPlayer3.Text = gameManager.Players.GetAt(3).Username;
                    hand = gameManager.GetCardsInHand(3, out count);
                    SetUpCards(PlayersCard.P3Card1, hand[0]);
                    SetUpCards(PlayersCard.P3Card2, hand[1]);
                }
            }
            else
            {
                MessageBox.Show("StartGame: Players is null", "Error");
            }

        }

        public enum PlayersCard { DealerCard1, DealerCard2, P1Card1, P1Card2, P2Card1, P2Card2, P3Card1, P3Card2 }

        public void SetUpCards(PlayersCard playersCard, string card)
        {
            string imagePath = IMAGE_PATH + card + IMAGE_EXT;
            Uri imageUri = new Uri(imagePath);
            BitmapImage bitmap = new BitmapImage(imageUri);

            switch (playersCard)
            {
                case PlayersCard.DealerCard1:
                    DealerCard1.Source = bitmap;
                    break;
                case PlayersCard.DealerCard2:
                    DealerCard2.Source = bitmap;
                    break;
                case PlayersCard.P1Card1:
                    Player1Card1.Source = bitmap;
                    break;
                case PlayersCard.P1Card2:
                    Player1Card2.Source = bitmap;
                    break;
                case PlayersCard.P2Card1:
                    Player2Card1.Source = bitmap;
                    break;
                case PlayersCard.P2Card2:
                    Player2Card2.Source = bitmap;
                    break;
                case PlayersCard.P3Card1:
                    Player3Card1.Source = bitmap;
                    break;
                case PlayersCard.P3Card2:
                    Player3Card2.Source = bitmap;
                    break;
            }
        }
        public void AddCard(object sender, CustomEventArgs e)
        {
            string whichCardName = string.Empty;

            if (e.PlayerIndex != 0)
            {
                whichCardName = "Player" + e.PlayerIndex.ToString() + "Card" + e.CardNbr.ToString(); // To set up x:Name for the specific player and Image control
            }
            else
            {
                whichCardName = "Dealer" + "Card" + e.CardNbr.ToString();
            }

            Image whichCard = FindName(whichCardName) as Image; // find cards x:Name  -> whichCard is the x:Name of an Image control

            if (whichCard != null)
            {
                string imagePath = IMAGE_PATH + e.Card + IMAGE_EXT;
                Uri imageUri = new Uri(imagePath);
                BitmapImage bitmap = new BitmapImage(imageUri);
                whichCard.Source = bitmap; 
            }
        }

        public void ShowDealerCard(object sender, EventArgs e)
        {
            string card2 = gameManager.Players.GetAt(0).Hand.Cards.GetAt(1).ToString();
            SetUpCards(PlayersCard.DealerCard2, card2);
        }

        public void ContinueNextRound(object sender, EventArgs e)
        {
            ResetMainWindow();
            gameManager.ReloadGame();
        }
        public void ResetMainWindow()
        {
            string whichCardName = "";
            int dealer = 0;

            for (int player = 1; player < gameManager.Players.Count; player++)
            {
                for (int card = 3; card < gameManager.Players.GetAt(player).Hand.Cards.Count + 1; card++) // + 1 because it starts count at 0
                {
                    whichCardName = "Player" +player.ToString() + "Card" + card.ToString();
                    Image whichCard = FindName(whichCardName) as Image; // find cards x:Name  -> whichCard is the x:Name of an Image control

                    if (whichCard != null)
                    {
                        whichCard.Source = null;
                    }
                }
            }

            for (int card = 3; card < gameManager.Players.GetAt(dealer).Hand.Cards.Count + 1; card++)
            {
                whichCardName = "Dealer" + "Card" + card.ToString();
                Image whichCard = FindName(whichCardName) as Image; // find cards x:Name  -> whichCard is the x:Name of an Image control
                if (whichCard != null)
                {
                    whichCard.Source = null;
                }
            }

            gameOver.Visibility = Visibility.Hidden;
        }


        private void NextPlayer(object sender, int index)
        {
            txtPlayerID.Text = gameManager.Players.GetAt(index).Username;
        }

        private void CustomMessage(object sender, string message)
        {
            MessageBox.Show(message);
        }

        private void RoundFinished(object sender, List<string> list)
        {
            gameOver.Visibility = Visibility.Visible;
            gameOver.SetWinnersList(list);
        }

        private void Reset(object sender, EventArgs e)
        {
            gameManager.ResetGame();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show(); 
            Close(); //close old mainWindow
            Application.Current.MainWindow = mainWindow; // Set the new instance as the main window
        }

        private void btnShuffle_Click(object sender, RoutedEventArgs e)
        {
            gameManager.ShuffleDeck();
        }

        private void btnStands_Click(object sender, RoutedEventArgs e)
        {
            string player = txtPlayerID.Text;
            gameManager.Stands(player);

        }

        private void btnDrawCard_Click(object sender, RoutedEventArgs e)
        {
            string player = txtPlayerID.Text;
            gameManager.Draws(player);
        }

        private void txtbet1_TextChanged(object sender, TextChangedEventArgs e)
        { 
        }

        private void btnbet1_Click(object sender, RoutedEventArgs e)
        {
            string who = txtPlayer1.Text;
            string bet = txtbet1.Text;
            gameManager.AddBettingData(who, bet);
        }

        private void btnbet2_Click(object sender, RoutedEventArgs e)
        {
            string who = txtPlayer2.Text;
            string bet = txtbet2.Text;
            gameManager.AddBettingData(who, bet);

        }

        private void btnbet3_Click(object sender, RoutedEventArgs e)
        {
            string who = txtPlayer3.Text;
            string bet = txtbet3.Text;
            gameManager.AddBettingData(who, bet);

        }

        private void btInfo_Click(object sender, RoutedEventArgs e)
        {
            string strOut = null;
            strOut = gameManager.GetPlayerInfo(txtPlayerID.Text);
            MessageBox.Show(strOut, txtPlayerID.Text);
        }
    }
}
