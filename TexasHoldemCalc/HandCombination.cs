using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldemCalc
{
    /// <summary>
    /// Checks all possible hand combinations
    /// Works strongest to weakest
    /// </summary>
    class HandCombination
    {
        public void GetStrongestHand(Hand hand)
        {
            if (IsRoyalFlush(hand))
            {

            }
            else if (IsStraightFlush(hand))
            {

            }
            else if (IsFullHouse(hand))
            {

            }
            else if (IsFourOfAKind(hand))
            {

            }
            else if (IsFlush(hand))
            {

            }
            else if (IsStraight(hand))
            {

            }
            else if (IsThreeOfAKind(hand))
            {

            }
            else if (IsTwoPair(hand))
            {

            }
            else if (IsPair(hand))
            {

            }
            else
            {

            }
        }

        #region HandCheck
        /// <summary>
        /// Section checks for what hand the player has
        /// </summary>
        /// <returns>True for highest Hand</returns>

        public bool IsRoyalFlush(Hand hand)
        {
            Suits suit;
            // Loops through 4 times once for every suit
            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0:
                        suit = Suits.Spades;
                        break;
                    case 1:
                        suit = Suits.Hearts;
                        break;
                    case 2:
                        suit = Suits.Diamonds;
                        break;
                    default:
                        suit = Suits.Clubs;
                        break;
                }

                // Checks for the royal flush highest to lowest, stops if a card isn't in the hand
                if(hand.Contains(Values.Ace, suit))
                {
                    if(hand.Contains(Values.King, suit))
                    {
                        if(hand.Contains(Values.Queen, suit))
                        {
                            if(hand.Contains(Values.Jack, suit))
                            {
                                if(hand.Contains(Values.Ten, suit))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        public bool IsStraightFlush(Hand hand)
        {
            Suits suit;
            // Loops through 4 times once for every suit
            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0:
                        suit = Suits.Spades;
                        break;
                    case 1:
                        suit = Suits.Hearts;
                        break;
                    case 2:
                        suit = Suits.Diamonds;
                        break;
                    default:
                        suit = Suits.Clubs;
                        break;
                }


            }
                return false;
        }

        public bool IsFullHouse(Hand hand)
        {
            return false;
        }

        public bool IsFourOfAKind(Hand hand)
        {
            return false;
        }

        public bool IsFlush(Hand hand)
        {
            return false;
        }

        public bool IsStraight(Hand hand)
        {
            return false;
        }

        public bool IsThreeOfAKind(Hand hand)
        {
            return false;
        }

        public bool IsTwoPair(Hand hand)
        {
            return false;
        }

        public bool IsPair(Hand hand)
        {
            return false;
        }

        #endregion HandCheck
    }
}
