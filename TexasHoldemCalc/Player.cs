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

        // Properties
        public int Chips { get { return chips; } set { chips = value; } }

        // Constructor
        public Player(int chips)
        {
            this.chips = chips;
        }
    }
}
