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
        Fold,
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
        int raiseAmount;
        int allInMaxBet;
        int allInMaxPot;
        bool user;
        bool folded;
        bool bigBlind;
        bool smallBlind;
        bool allIn;
        List<Card> holeCards;
        Hand bestHand;
        AIPlayer ai;
        Table table;

        // Properties
        public int Chips { get { return chips; } set { chips = value; } }
        public int PlayerNumber { get { return playerNumber; } }
        public int BetIncrease { get { return betIncrease; } set { betIncrease = value; } }
        public int RaiseAmount { get { return raiseAmount; } set { raiseAmount = value; } }
        public int AllInMaxBet { get { return allInMaxBet; } }
        public int AllInMaxPot { get { return allInMaxPot; } set { allInMaxPot = value; } }
        public bool User { get { return user; } }
        public bool Folded { get { return folded; } set { folded = value; } }
        public bool BigBlind { get { return bigBlind; } set { bigBlind = value; } }
        public bool SmallBlind { get { return smallBlind; } set { smallBlind = value; } }
        public bool AllIn { get { return allIn; } set { allIn = value; } }
        public List<Card> HoleCards { get { return holeCards; } set { holeCards = value; } }
        public Hand BestHand { get { return bestHand; } set { bestHand = value; } }
        public Table Table { get { return table; } }

        /// <summary>
        /// Constuctor
        /// </summary>
        /// <param name="chips">Starting Chips</param>
        /// <param name="playerNumber">Position Number</param>
        public Player(int chips, int playerNumber, Table table)
        {
            this.chips = chips;
            this.playerNumber = playerNumber;
            this.table = table;
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
                ai = new AIPlayer(this);
            }
        }

        /// <summary>
        /// Used to distinguish between AI & player. Allow player to bet
        /// </summary>
        public BettingOptions Bet(int betAmount, int roundCount)
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
                        Console.WriteLine("You Check");
                        return BettingOptions.Check;
                    case "CALL":
                        if (betAmount >= chips)
                        {
                            allInMaxBet = chips;
                            chips -= chips;
                            allIn = true;
                            Console.WriteLine("You call and put yourself all in!");
                        }
                        else
                        {
                            chips -= betAmount;
                            Console.WriteLine("You Call " + betAmount);
                        }
                        return BettingOptions.Call;
                    case "RAISE":
                        Console.WriteLine("How much would you like to raise by?");
                        string input = Console.ReadLine();
                        do
                        {
                            try
                            {
                                raiseAmount = Int32.Parse(input);
                                if(raiseAmount > 0)
                                {
                                    if (chips <= raiseAmount)
                                    {
                                        Console.WriteLine("You go all in!");
                                        allInMaxBet = chips;
                                        chips -= chips;
                                        betIncrease = raiseAmount - betAmount;
                                        allIn = true;
                                    }
                                    else
                                    {
                                        betIncrease = raiseAmount - betAmount;
                                        chips -= raiseAmount;
                                        Console.WriteLine("You Raise to " + raiseAmount);
                                    }
                                    break;
                                }
                            }
                            catch
                            {
                                Console.WriteLine("Invalid input try again!");
                            }
                            Console.WriteLine("How much would you like to raise to?");
                            input = Console.ReadLine();
                        } while (true);
                        return BettingOptions.Raise;
                    case "FOLD":
                        folded = true;
                        Console.WriteLine("You Fold");
                        return BettingOptions.Fold;
                    default:
                        return Bet(betAmount, roundCount);
                }
            }
            else
            {
                BettingOptions bo = ai.Betting(betAmount, roundCount);

                switch (bo)
                {
                    case BettingOptions.Fold:
                        folded = true;
                        Console.WriteLine("Player " + playerNumber + " folds!");
                        break;
                    case BettingOptions.Call:
                        if (betAmount >= chips)
                        {
                            allInMaxBet = chips;
                            chips -= chips;
                            allIn = true;
                        }
                        else
                            chips -= betAmount;
                        break;
                    case BettingOptions.Raise:
                        if (raiseAmount >= chips)
                        {
                            allInMaxBet = chips;
                            betIncrease = raiseAmount - betAmount;
                            chips -= chips;
                            allIn = true;
                            Console.WriteLine("Player " + playerNumber + " moves all in! With " + allInMaxBet);
                        }
                        else
                        {
                            betIncrease = raiseAmount - betAmount;
                            chips -= raiseAmount;
                            Console.WriteLine("Player " + playerNumber + " raises to " + betAmount);
                        }
                        break;
                    default:
                        Console.WriteLine("Player " + playerNumber + " checks!");
                        break;
                }
                return bo;
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
