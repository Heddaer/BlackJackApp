// Author: Hedda Eriksson
// Date: 2023-10-17
// Malmö University, Bachelor of Engineering - Computer Science & Telecommunications
// Description: A pop up window to start a new game. Takes inputs about players and deck. 
using BlackJackBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
using UtilityLib.Data_Management;
using UtilityLib.Game;

namespace BlackJackPL
{
    /// <summary>
    /// Interaction logic for NewGameWindow.xaml
    /// </summary>
    public partial class NewGameWindow : Window
    {
        private GameManager gameManager;
        private ListManager<string>? players;
        private int decks;

        public EventHandler StartGameRequest;

        public GameManager GameManager { get => gameManager; set => gameManager = value; }

        public NewGameWindow()
        {
            InitializeComponent();
            GameManager = new GameManager();
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            SetPlayers();
            SetDecks();
            if (players != null)
            {
                GameManager.InitNewGame(players, decks);
                StartGameRequest?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                MessageBox.Show("Players is null", "Error");
            }


            Close();
        }

        public void SetDecks()
        {
            if (cmbDecks.SelectedIndex != 0)
            {
                decks = cmbDecks.SelectedIndex;
            }
            else { decks = 1; }
        }
        public void SetPlayers()
        {
            players = new ListManager<string>();
            if (txtPlayer1.Text.ToString() != "Username")
            {
                players.Add(txtPlayer1.Text.ToString());
            }
            if (txtPlayer2.Text.ToString() != "Username")
            {
                players.Add(txtPlayer2.Text.ToString());
            }
            if(txtPlayer3.Text.ToString() != "Username")
            {
                players.Add(txtPlayer3.Text.ToString());
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is Window mainWindow)
            {
                // Close the main window
                mainWindow.Close();
            }

            Close();
        }

        private void Unregister_Click(object sender, RoutedEventArgs e)
        {
            bool ok = GameManager.UnregisterPlayer(txtPlayer1.Text);

            if (ok)
            {
                MessageBox.Show(txtPlayer1.Text + " is unregisterd.");
            }
            else
            {
                MessageBox.Show("Try again, unregistration failed.");
            }
        }
    }
}
