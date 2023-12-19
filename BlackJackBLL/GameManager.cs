// Author: Hedda Eriksson
// Date: 2023-10-17
// Malmö University, Bachelor of Engineering - Computer Science & Telecommunications
// Description: Runs the game logic of the Black Jack app.

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using UtilityLib.Data_Management;
using UtilityLib.Utilities;
using UtilityLib.Enums;
using UtilityLib.Game;
using BlackJackDAL.Logging;
using BlackJackDAL.FileHandling;
using BlackJackDAL.Database;

namespace BlackJackBLL
{
    public class GameManager
    {
        private ListManager<Card>? deck;
        private ListManager<Player>? players;
        private ConsoleLogger consoleLogger;
        private FileLogger fileLogger;
        private Logger logger;
        private GameDbController dbController;

        private string gameLogPath = "\\\\Mac\\Home\\Documents\\VSProjects\\BlackJackApp\\BlackJackDAL\\GameLog\\log.xml";
        int NumberOfDecks { get; set; }
        public ListManager<Player>? Players { get => players; set => players = value; }
        public EventHandler StartGameRequest;
        public EventHandler ShowDealerCardRequest;

        public event EventHandler<int> NextPlayerRequest;
        public event EventHandler<string> MessageRequest;
        public event EventHandler<CustomEventArgs> AddCardRequest;
        public event EventHandler<List<string>> AnnounceWinnerRequest;

        public GameManager() 
        {
            dbController = new GameDbController();
            //gameLogPath = (Path.GetFullPath("GameLog") + "\\log.xml"); // HERE
            consoleLogger = new ConsoleLogger();
            fileLogger = new FileLogger(gameLogPath);
            logger = new Logger(message =>
            {
                consoleLogger.LogToConsole(message);
                fileLogger.Log(message);
            });
            logger.LogAction("Game Started", "First round has begun.");
        }

        /// <summary>
        /// Initialize players and a dealer to the game. 
        /// Both players and dealers starts with a pair of cards.
        /// </summary>
        /// <param name="playerIDList"></param>
        public void InitNewGame(ListManager<string> playerIDList, int numberOfDecks)
        {
            this.NumberOfDecks = numberOfDecks;
            GenerateDeck();
            Players = new ListManager<Player>();
            Players.Add(InitDealer()); //Add a dealer

            //initialize Player's hand
            foreach (string playerID in playerIDList.List)
            {
                Hand playerHand = new Hand();
                playerHand.AddCard(DrawCard());
                playerHand.AddCard(DrawCard());
                Player player = new Player
                {
                    Username = playerID,
                    IsBust = false,
                    IsWinner = false,
                    IsDealer = false,
                    Stands = false,
                    Hand = playerHand,
                };

                PlayerStats playerStats;

                if (!dbController.UserRegistered(player.Username)) //add to database if not registered
                {
                    playerStats = new PlayerStats //start statistics
                    {
                        Losses = 0,
                        Wins = 0,
                        Player = player,
                    };
                }
                else
                {
                    playerStats = dbController.GetPlayerStats(player);
                    Player pData = dbController.GetPlayerData(player);
                    player.Coins = pData.Coins;
                    player.BettingData = pData.BettingData;
                }
                player.Stats = playerStats;

                dbController.AddPlayer(player); 
                Players.Add(player);
                logger.LogAction("Starting Hand: ", (playerID + " starts with " + playerHand.Score + "points"));
            }
        }

        private Player InitDealer()
        {
            // Initialize the dealer's hand
            Hand dealerHand = new Hand();
            dealerHand.AddCard(DrawCard());
            dealerHand.AddCard(DrawCard());
            PlayerStats playerStats;
            Player dealer = new Player
            {
                Username = "Dealer",
                IsBust = false,
                IsWinner = false,
                IsDealer = true,
                Stands = false,
                Hand = dealerHand,
            };

            if (!dbController.UserRegistered(dealer.Username))
            {
                playerStats = new PlayerStats
                {
                    Losses = 0,
                    Wins = 0,
                    Player = dealer,
                };
            }
            else
            {
                playerStats = dbController.GetPlayerStats(dealer);
            }
            dealer.Stats = playerStats;

            dbController.AddPlayer(dealer); //add to database
            logger.LogAction("Starting Hand: ", ("Dealer starts with " + dealerHand.Score + "points"));
            return dealer;
        }

        /// <summary>
        /// Update status of players and dealer, control cards in deck.
        /// </summary>
        private void UpdateGame()
        {        
            if (deck == null)   //control deck
            {
                GenerateDeck(); // if empty, refill
            }
            bool playerPlaying = ControlPlayers();  //control if player and dealer isBust or Stands
            if (!playerPlaying)
            {
                DealerPlaying();    //Dealer starts playing if all players are finished playing
            }

        }

        /// <summary>
        /// Controls the status of each player playing
        /// </summary>
        /// <returns> if all players stands (or is busted) then return false, else return true </returns>
        private bool ControlPlayers()
        {
            bool playing = false;
            Player dealer = Players.GetAt(0);

            foreach (Player p in Players.List)
            {
                if (p.IsDealer) continue; // check only players, skip dealer
                
                if ((p.Hand.Score > 21) && (!p.IsBust)) // Check if bust
                {
                    p.IsBust = true;
                    string txt = string.Format(p.Username + " is busted!");
                    playing = false;
                    logger.LogAction("Busted: ", txt);
                    MessageRequest.Invoke(this, txt);
                    dealer.Stats.Wins += 1;
                    p.Stats.Losses += 1;
                }
                else if (p.IsBust || p.Stands)
                {
                    playing = false;
                } 
                else if (!p.Stands || !p.IsBust)
                {
                    playing = true;
                    break;
                }   
            }
            return playing;
        }

        /// <summary>
        /// Simulates the dealer's turn in a game of Blackjack, dealer plays until 
        /// busted (scores over 21) or stands (scores over 17).
        /// </summary>
        private void DealerPlaying()
        {
            Player dealer = Players.GetAt(0);
            ShowDealerCardRequest?.Invoke(this, EventArgs.Empty);

            while (!dealer.IsBust && !dealer.Stands)
            {
                if (dealer.Hand.Score <= 16)
                {
                    Card card = DrawCard();
                    dealer.Hand.AddCard(card);
                    var args = new CustomEventArgs
                    {
                        Card = card.ToString(),
                        PlayerIndex = 0,
                        CardNbr = dealer.CountCards(dealer.Hand)
                    };
                    AddCardRequest?.Invoke(this, args);
                }
                else if (dealer.Hand.Score > 21)
                {
                    dealer.IsBust = true;
                    GameOver();
                    return;
                }
                else
                {
                    dealer.Stands = true;
                    GameOver();
                    return; 
                }
            }
        }

        /// <summary>
        /// When a round is finished by players and dealer -> set winners of the round -> announce winners by invoking GUI.
        /// </summary>
        private void GameOver()
        {
            Player dealer = Players.GetAt(0);

            logger.LogAction("GameOver", "");

            foreach (Player p in Players.List) 
            {
                if (p.IsDealer) continue;

                if (dealer.IsBust) //Scenario: Dealer is bust. All players, not busted, wins
                {
                    if (!p.IsBust)
                    {
                        p.IsWinner = true;
                        p.Stats.Wins += 1;
                        dealer.Stats.Losses += 1;
                    }
                }
                else if (p.Hand.Score >= dealer.Hand.Score && !p.IsBust) //Scenario: Dealer is not bust. Compare score with dealer, higher score = wins
                {
                    p.IsWinner = true;
                    p.Stats.Wins += 1;
                    dealer.Stats.Losses += 1;
                }
                p.CalculateBet(CalculateOdds());
                dbController.UpdatePlayer(p);
            }

            dbController.UpdateDatabase(Players); //Update database with new statistics
            AnnounceWinnerRequest?.Invoke(this, GetWinnerList());
        }
        
        /// <summary>
        /// Construct a list of winners, losers and busted players, incl. dealer. 
        /// </summary>
        /// <returns>A List<string> of round results.</returns>
        private List<string> GetWinnerList()
        {
            string txtOut = string.Empty;
            List<string> winners = new List<string>();


            foreach (Player p in Players.List) //Set winners first in list
            {
                if (p.IsDealer)
                {
                    txtOut = p.Username + "     Score: " + p.Hand.Score;
                    if (p.IsBust)
                    {
                        txtOut = "Busted!   " + p.Username + "     Score: " + p.Hand.Score;
                    }
                    winners.Add(txtOut);
                }
                else if (p.IsWinner)
                {
                    txtOut = "WINNER!   " + p.Username + "     Score: " + p.Hand.Score;
                    winners.Add(txtOut);
                }
            }
            foreach (Player p in Players.List) //Set losers
            {
                if (p.IsBust && !p.IsDealer)
                {
                    txtOut = "Busted!   " + p.Username + "     Score: " + p.Hand.Score;
                    winners.Add(txtOut);
                }
                else if (!p.IsWinner && !p.IsDealer)
                {
                    txtOut = "Loser...  " + p.Username + "     Score: " + p.Hand.Score;
                    winners.Add(txtOut);
                }
            }
            return winners;

        }

        /// <summary>
        /// Player draws a new card, add card to the game, updates and sets next player. 
        /// </summary>
        /// <param name="player"></param>
        public void Draws(string player)
        {
            foreach (Player p in Players.List)
            {
                if (p.Username == player)
                {
                    int index = Players.GetIndex(p);
                    Card card = DrawCard();
                    int score = p.Hand.AddCard(card);
                    MessageBox.Show(score.ToString(), "Score:");
                    string description = p.Username + " Score:" + score.ToString();
                    logger.LogAction("Draws: ", description);
                    UpdateGame();
                    var args = new CustomEventArgs
                    {
                        Card = card.ToString(),
                        PlayerIndex = index,
                        CardNbr = p.CountCards(p.Hand)
                    };
                    AddCardRequest?.Invoke(this, args);
                    NextPlayer(index);
                }
            }
        }
        
        /// <summary>
        /// Marks player as standing, then trigger the next player.
        /// </summary>
        /// <param name="player">Player taking a stand action</param>
        public void Stands(string player)
        {
            foreach (Player p in Players.List)
            {
                if (p.Username == player)
                {
                    string description = p.Username + " Score:" + p.Hand.Score.ToString();
                    logger.LogAction("Stands: ", description);
                    int index = Players.GetIndex(p);
                    p.Stands = true;
                    UpdateGame();
                    NextPlayer(index);
                }
            }
        }

        /// <summary>
        /// Invoked when new round is requested. Sets up the game with the previous players and new hands. 
        /// </summary>
        public void ReloadGame()
        {
            logger.LogAction("New Round", " ");
            foreach (Player p in Players.List)
            {    
                p.IsWinner = false;
                p.IsBust = false;
                p.Stands = false;
                p.Hand.Score = 0;
                p.Hand.Cards.DeleteAll();                 
                p.Hand.AddCard(DrawCard());
                p.Hand.AddCard(DrawCard());
                string discription = p.Username + " starts with " + p.Hand.Score + "points";
                logger.LogAction("Starting hand: ", discription);
            }
            StartGameRequest?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Draws the last card from the deck and removes it from the game.        
        /// /// </summary>
        /// <returns>Last card from deck.</returns>
        public Card DrawCard()
        {
            Card card = deck.GetAt(deck.Count - 1);
            deck.DeleteAt(deck.Count - 1);
            return card;
        }

        /// <summary>
        /// Generates a list of one or more decks depening on user input. 
        /// </summary>
        /// <returns>ListManager<Card> of deck(s)</returns>
        private ListManager<Card> GenerateDeck()
        {
            deck = new ListManager<Card>();

            for (int i = 1; i <= this.NumberOfDecks; i++)
            {
                Enum.GetValues(typeof(Suit)).Cast<Suit>().ToList().ForEach(suit =>
                {
                    Enum.GetValues(typeof(CardValue)).Cast<CardValue>().ToList().ForEach(value =>
                    {
                        deck.Add(new Card(suit, value));
                    });
                });
            }

            ShuffleDeck();

            return deck;
        }

        public void ShuffleDeck()
        {
            Random rand = new Random();

            IEnumerable<Card> shuffleQuery = deck.List;
            shuffleQuery = shuffleQuery.OrderBy(_ => rand.Next(deck.Count + 1)); // + 1 because it starts count at 0
            deck = new ListManager<Card>(shuffleQuery);

            // FOR TESTING: Convert the shuffled list to a string for display
            //string shuffledDeckString = string.Join(Environment.NewLine, deck.ToStringList());
            //MessageBox.Show(shuffledDeckString);
        }

        /// <summary>
        /// Gets card for the the player at specified index. 
        /// </summary>
        /// <param name="index">Index of a player</param>
        /// <param name="count">Number of cards in hand</param>
        /// <returns>A list of cards toString()</returns>
        public List<string> GetCardsInHand(int index, out int count)
        {
            List<string> cardsInHand = new List<string>();
            foreach (Card c in Players.GetAt(index).Hand.Cards.List)
            {
                cardsInHand.Add(c.ToString());
            }
            count = cardsInHand.Count;
            return cardsInHand;
        }


        /// <summary>
        /// Cycle through a list of players, moving the next player in the list 
        /// based on how many players playing. Finally, raises an event, passing 
        /// the index of the next player. 
        /// </summary>
        /// <param name="index">index of the current player</param>
        private void NextPlayer(int index)
        {   
            int modulo = (Players.Count - 1);
            index = (index % modulo) + 1; // modulo % to circle around the players 1-3

            for (int i = 0; i < Players.Count - 1; i++) 
            {
                if ((Players.GetAt(index).IsBust) || (Players.GetAt(index).Stands)) // Qriteria for next player isBust = false and Stands = false, otherwise look at next player... vice versa
                {
                    index = (index % modulo) + 1;
                }
                else
                {
                    NextPlayerRequest?.Invoke(this, index);
                    break;
                }
            }
        }

        public void ResetGame()
        {
            Players.List.Clear();
            NumberOfDecks = 0;
            deck.List.Clear();
        }

        public bool UnregisterPlayer(string player)
        {
            return dbController.DeletePlayer(player);
        }

        public void AddBettingData(string player, string bet)
        {
            foreach (Player p in Players.List)
            {
                if (p.Username == player)
                {
                    BettingData newBet = new BettingData(Double.Parse(bet));
                    p.AddBet(newBet);
                    dbController.UpdatePlayer(p);
                }
            }
        }

        private double CalculateOdds()
        {
            double odds;
            return odds = (NumberOfDecks / 10) + 1;
            
        }

        public string? GetPlayerInfo(string player)
        {
            string strOut = string.Empty;
            foreach(Player p in Players.List)
            {
                if (player.Equals(p.Username))
                {
                    strOut = p.Stats.ToString() + Environment.NewLine + "$" + p.Coins.ToString();
                }
            }
            return strOut;
        }
    }
}
