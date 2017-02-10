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
        public BettingOptions Betting(int betAmount)
        {
            return BettingOptions.Fold;
        }
    }
}
