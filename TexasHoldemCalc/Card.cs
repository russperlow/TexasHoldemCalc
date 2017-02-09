using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldemCalc
{
    class Card
    {
        public Suits Suit { get; }
        public Values Value { get; }
        public bool FaceUp { get; set; }
        public bool WasShuffled { get; set; }

        public Card(Suits suit, Values value)
        {
            Suit = suit;
            Value = value;
            FaceUp = false;
            WasShuffled = false;
        }


        /// <summary>
        /// Used to return the values in a string, specifically face cards
        /// </summary>
        /// <returns>Cards Value in a string</returns>
        public string ValueToString()
        {
            switch ((int)Value)
            {
                case 11:
                    return "Jack";
                case 12:
                    return "Queen";
                case 13:
                    return "King";
                case 14:
                    return "Ace";
                default:
                    return Value.ToString();
            }
        }

        public string SuitsToString()
        {
            switch (Suit)
            {
                case Suits.Spades:
                    return "Spades";
                case Suits.Hearts:
                    return "Hearts";
                case Suits.Diamonds:
                    return "Diamonds";
                case Suits.Clubs:
                    return "Clubs";
                default:
                    return "";
            }
        }
    }
}
