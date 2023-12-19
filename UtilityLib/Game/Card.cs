// Author: Hedda Eriksson
// Date: 2023-10-17
// Malmö University, Bachelor of Engineering - Computer Science & Telecommunications
// Description: This class represents a playing card with properties for its suit and value.
// It also overrides the ToString method to provide a string representation of the card.
using UtilityLib.Enums;

namespace UtilityLib.Game
{
    public class Card
    {
        public Suit Suit { get; set; }
        public CardValue Value { get; set; }

        public Card(Suit suit, CardValue value)
        {
            this.Suit = suit;
            this.Value = value;
        }
        
        public override string ToString()
        {
            return string.Format("{0}{1}", Suit, Value);
        }
    }
}
