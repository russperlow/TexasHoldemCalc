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
            for(int i = 0; i < hand.HandList.Count() - 4; i++)
            {
                if(hand.Contains(hand.HandList[i].Value - 1, hand.HandList[i].Suit))
                {
                    if (hand.Contains(hand.HandList[i].Value - 2, hand.HandList[i].Suit))
                    {
                        if (hand.Contains(hand.HandList[i].Value - 3, hand.HandList[i].Suit))
                        {
                            if (hand.Contains(hand.HandList[i].Value - 4, hand.HandList[i].Suit))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public bool IsFourOfAKind(Hand hand)
        {
            Card temp;
            for (int i = 0; i < hand.HandList.Count() - 3 ; i++)
            {
                temp = hand.HandList[i];
                if(temp.Value == hand.HandList[i+1].Value && temp.Value == hand.HandList[i+2].Value && temp.Value == hand.HandList[i + 3].Value)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsFullHouse(Hand hand)
        {
            for(int i = 0; i < hand.HandList.Count()-2; i++)
            {
                if(hand.HandList[i].Value == hand.HandList[i + 1].Value && hand.HandList[i].Value == hand.HandList[i + 2].Value) 
                {
                    // Make sure to start j at i + 1 so it doesn't pick up the same cards from above
                    for(int j = i + 1; j < hand.HandList.Count()-1; j++)
                    {
                        if(hand.HandList[j].Value == hand.HandList[j + 1].Value)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public bool IsFlush(Hand hand)
        {
            int hearts = 0;
            int diamonds = 0;
            int spades = 0;
            int clubs = 0;

            foreach(Card c in hand.HandList)
            {
                switch (c.Suit)
                {
                    case Suits.Spades:
                        spades++;
                        break;
                    case Suits.Hearts:
                        hearts++;
                        break;
                    case Suits.Diamonds:
                        diamonds++;
                        break;
                    case Suits.Clubs:
                        clubs++;
                        break;
                }
            }

            if(spades >= 5 || hearts >= 5 || diamonds >= 5 || clubs >= 5)
            {
                return true;
            }

            return false;
        }

        public bool IsStraight(Hand hand)
        {
            for (int i = 0; i < hand.HandList.Count() - 4; i++)
            {
                if (hand.Contains(hand.HandList[i].Value - 1))
                {
                    if (hand.Contains(hand.HandList[i].Value - 2))
                    {
                        if (hand.Contains(hand.HandList[i].Value - 3))
                        {
                            if (hand.Contains(hand.HandList[i].Value - 4))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public bool IsThreeOfAKind(Hand hand)
        {
            for (int i = 0; i < hand.HandList.Count() - 2; i++)
            {
                if (hand.HandList[i].Value == hand.HandList[i + 1].Value && hand.HandList[i].Value == hand.HandList[i + 2].Value)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsTwoPair(Hand hand)
        {
            for (int i = 0; i < hand.HandList.Count() - 1; i++)
            {
                if (hand.HandList[i].Value == hand.HandList[i + 1].Value)
                {
                    // Make sure to start j at i + 1 so it doesn't pick up the same cards from above
                    for (int j = i + 1; j < hand.HandList.Count() - 1; j++)
                    {
                        if (hand.HandList[j].Value == hand.HandList[j + 1].Value)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool IsPair(Hand hand)
        {
            for (int i = 0; i < hand.HandList.Count() - 1; i++)
            {
                if (hand.HandList[i].Value == hand.HandList[i + 1].Value)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion HandCheck

        #region GetBestHand
        /// <summary>
        /// After finding the best hand this cuts it down to the 5 best cards
        /// </summary>
        /// <returns>The best 5 Card hand</returns>

        public Hand GetRoyalFlushHand(Hand hand)
        {
            return null;
        }

        public Hand GetStraightFlushHand(Hand hand)
        {
            return null;
        }

        public Hand GetFourOfAKindHand(Hand hand)
        {
            return null;
        }

        public Hand GetFullHouseHand(Hand hand)
        {
            return null;
        }

        public Hand GetFlushHand(Hand hand)
        {
            return null;
        }

        public Hand GetStraightHand(Hand hand)
        {
            return null;
        }

        public Hand GetThreeOfAKindHand(Hand hand)
        {
            return null;
        }

        public Hand GetTwoPairHand(Hand hand)
        {
            return null;
        }

        public Hand GetPairHand(Hand hand)
        {
            return null;
        }

        public Hand GetHighCardHand(Hand hand)
        {
            return null;
        }

        #endregion GetBestHand
    }
}
