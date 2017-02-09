using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldemCalc
{
    /// <summary>
    /// Player class for the user
    /// </summary>
    class Player
    {
        // Fields
        int chips;
        List<Card> hand;

        // Properties
        public int Chips { get { return chips; } set { chips = value; } }
        public List<Card> Hand { get { return hand; } set { hand = value; } }

        // Constructor
        public Player(int chips)
        {
            this.chips = chips;
            hand = new List<Card>();
        }
    }
}
