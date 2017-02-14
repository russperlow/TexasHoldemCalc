using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldemCalc
{
    enum BettingOptions
    {
        Check,
        Call,
        Raise,
        Fold
    }
    /// <summary>
    /// Player class for the user
    /// </summary>
    class Player
    {
        // Fields
        int chips;
        int playerNumber;
        int betIncrease;
        bool user;
        bool folded;
        List<Card> holeCards;
        Hand bestHand;
        AIPlayer ai;

        // Properties
        public int Chips { get { return chips; } set { chips = value; } }
        public int PlayerNumber { get { return playerNumber; } }
        public int BetIncrease { get { return betIncrease; } set { betIncrease = value; } }
        public bool Folded { get { return folded; } set { folded = value; } }
        public List<Card> HoleCards { get { return holeCards; } set { holeCards = value; } }
        public Hand BestHand { get { return bestHand; } set { bestHand = value; } }

        /// <summary>
        /// Constuctor
        /// </summary>
        /// <param name="chips">Starting Chips</param>
        /// <param name="playerNumber">Position Number</param>
        public Player(int chips, int playerNumber)
        {
            this.chips = chips;
            this.playerNumber = playerNumber;
            holeCards = new List<Card>();
            folded = false;
            betIncrease = 0;

            if (playerNumber == 0)
            {
                user = true;
            }
            else
            {
                user = false;
                ai = new AIPlayer();
            }
        }

        /// <summary>
        /// Used to distinguish between AI & player. Allow player to bet
        /// </summary>
        public BettingOptions Bet(int betAmount)
        {
            if (user)
            {
                if(betAmount == 0)
                {
                    Console.WriteLine("There is currently no bet on the table. Would you like to Check, Raise or Fold?");
                }
                else
                {
                    Console.WriteLine("The current bet to call is " + betAmount + ". Would you like to Call, Raise or Fold?");
                }
                string userInput = Console.ReadLine();

                switch (userInput.ToUpper())
                {
                    case "CHECK":
                        return BettingOptions.Check;
                    case "CALL":
                        chips -= betAmount;
                        return BettingOptions.Call;
                    case "RAISE":
                        Console.WriteLine("How much would you like to raise by?");
                        string input = Console.ReadLine();
                        int raiseAmount;
                        do
                        {
                            try
                            {
                                raiseAmount = Int32.Parse(input);
                                if(raiseAmount > betAmount)
                                {
                                    betIncrease = raiseAmount - betAmount;
                                    chips -= raiseAmount;
                                    break;
                                }
                            }
                            catch(Exception e)
                            {
                                Console.WriteLine(e);
                            }

                        } while (true);
                        return BettingOptions.Raise;
                    case "FOLD":
                        folded = true;
                        return BettingOptions.Fold;
                    default:
                        return Bet(betAmount);
                }
            }
            else
            {
                return ai.Betting(betAmount);
            }
        }

        public void GetBestHand(HandCombination hc, List<Card> communityCards)
        {
            Hand temp = new Hand();

            foreach(Card c in communityCards)
            {
                temp.HandList.Add(c);
            }
            temp.HandList.Add(holeCards[0]);
            temp.HandList.Add(holeCards[1]);

            bestHand = hc.GetStrongestHand(temp);
        }
    }
}
