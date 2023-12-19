// Author: Hedda Eriksson
// Date: 2023-10-17
// Malmö University, Bachelor of Engineering - Computer Science & Telecommunications
// Description: The Hand class represents a player's hand in a card game.
using UtilityLib.Data_Management;
using UtilityLib.Utilities;
using UtilityLib.Enums;


namespace UtilityLib.Game
{
    public class Hand
    {
        public int Score { get; set; }
        public ListManager<Card> Cards { get; set; }

        public Hand()
        {
            Cards = new ListManager<Card>();
            Score = 0;
        }

        /// <summary>
        /// Add card to player's hand and calculates current score. 
        /// The method implments logic for automatically handle 
        /// special cases like Ace being 1 or 10.
        /// </summary>
        /// <param name="card"> a Card</param>
        /// <returns>Current score.</returns>
        public int AddCard(Card card)
        {
            Cards.Add(card);

            if (card.Value == CardValue.Ace)
            {
                if (Score + 10 <= 21)
                {
                    Score += 10;  
                }
                else
                {
                    Score += 1; 
                }
            }
            else if ((card.Value == CardValue.Jack) || (card.Value == CardValue.Queen) || (card.Value == CardValue.King))
            {
                Score += 10;
            }
            else
            {
                int cardValue = EnumConverter.EnumToInteger(card.Value);
                Score += cardValue;
            }
            return Score;
        }
        
        /// <summary>
        /// Clear player's Hand.
        /// </summary>
        public void Clear()
        {
           //ToDo
        }

        public override string ToString()
        {
            //ToDo
            return string.Format("");
        }
    }
}
