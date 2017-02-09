using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldemCalc
{
    /// <summary>
    /// All the players that aren't the user
    /// </summary>
    class AIPlayer
    {
        // Fields
        int chips;
        int playerNumber;

        // Properties
        public int Chips { get { return chips; } set { chips = value; } }
        public int PlayerNumber { get { return playerNumber; } }

        /// <summary>
        /// Constuctor
        /// </summary>
        /// <param name="chips">Starting Chips</param>
        /// <param name="playerNumber">Position Number</param>
        public AIPlayer(int chips, int playerNumber)
        {
            this.chips = chips;
            this.playerNumber = playerNumber;
        }
    }
}
