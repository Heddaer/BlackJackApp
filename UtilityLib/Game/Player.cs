// Author: Hedda Eriksson
// Date: 2023-10-17
// Malmö University, Bachelor of Engineering - Computer Science & Telecommunications
// Description: The Player represents one game player in the current game.
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace UtilityLib.Game
{
    public class Player
    {
        [Key]
        public int PlayerId {  get; set; }
        public string? Username { get; set; }
        [NotMapped]
        public Hand? Hand { get; set; }
        [NotMapped]
        public bool IsWinner { get; set; }
        [NotMapped]
        public bool IsBust { get; set; }
        [NotMapped]
        public bool Stands { get; set; }
        [NotMapped]
        public bool IsDealer { get; set; }
        public PlayerStats Stats { get; set; }
        public double Coins { get; set; }
        [NotMapped]
        public double CurrentBet { get; set; }

        public ICollection<BettingData> BettingData { get; set; }

        public Player() 
        { 
            Stats = new PlayerStats();
            BettingData = new List<BettingData>();
            Coins = 50; //initial amount for new players
        }

        public string GetHand(Hand hand, int card)
        {
            return hand.Cards.GetAt(card).ToString();
        }

        public int CountCards(Hand hand)
        {
            return hand.Cards.Count;
        }

        public void AddBet(BettingData bettingData)
        {
            CurrentBet = bettingData.Bet;
            BettingData.Add(bettingData);
        }

        public void CalculateBet(double odds)
        {
            if (IsWinner)
            {
                Coins += (odds * CurrentBet);

            }
            else
            {
                Coins -= (odds * CurrentBet);

            }
        }

        public override string ToString()
        {
            //ToDo
            return string.Format("");
        }

    }
}
