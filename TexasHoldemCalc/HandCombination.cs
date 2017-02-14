using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldemCalc
{
    enum HandCombinations
    {
        HighCard = 1,
        Pair,
        TwoPair,
        ThreeOfAKind,
        Straight,
        Flush,
        FullHouse,
        FourOfAKind,
        StraightFlush,
        RoyalFlush
    }
    /// <summary>
    /// Checks all possible hand combinations
    /// Works strongest to weakest
    /// </summary>
    class HandCombination
    {
        public Hand GetStrongestHand(Hand hand)
        {
            if (IsRoyalFlush(hand))
            {
                return GetRoyalFlushHand(hand);
            }
            else if (IsStraightFlush(hand))
            {
                return GetStraightFlushHand(hand);
            }
            else if (IsFourOfAKind(hand))
            {
                return GetFourOfAKindHand(hand);
            }
            else if (IsFullHouse(hand))
            {
                return GetFullHouseHand(hand);
            }
            else if (IsFlush(hand))
            {
                return GetFlushHand(hand);
            }
            else if (IsStraight(hand))
            {
                return GetStraightHand(hand);
            }
            else if (IsThreeOfAKind(hand))
            {
                return GetThreeOfAKindHand(hand);
            }
            else if (IsTwoPair(hand))
            {
                return GetTwoPairHand(hand);
            }
            else if (IsPair(hand))
            {
                return GetPairHand(hand);
            }
            else
            {
                return GetHighCardHand(hand);
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
            Hand royalFlush = new Hand();
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
                if (hand.Contains(Values.Ace, suit))
                {
                    if (hand.Contains(Values.King, suit))
                    {
                        if (hand.Contains(Values.Queen, suit))
                        {
                            if (hand.Contains(Values.Jack, suit))
                            {
                                if (hand.Contains(Values.Ten, suit))
                                {
                                    royalFlush.HandList.Add(new Card(suit, Values.Ace));
                                    royalFlush.HandList.Add(new Card(suit, Values.King));
                                    royalFlush.HandList.Add(new Card(suit, Values.Queen));
                                    royalFlush.HandList.Add(new Card(suit, Values.Jack));
                                    royalFlush.HandList.Add(new Card(suit, Values.Ten));
                                }
                            }
                        }
                    }
                }
            }
            hand.Combination = HandCombinations.RoyalFlush;
            return royalFlush;
        }

        public Hand GetStraightFlushHand(Hand hand)
        {
            Hand straightFlush = new Hand();
            for (int i = 0; i < hand.HandList.Count() - 4; i++)
            {
                if (hand.Contains(hand.HandList[i].Value - 1, hand.HandList[i].Suit))
                {
                    if (hand.Contains(hand.HandList[i].Value - 2, hand.HandList[i].Suit))
                    {
                        if (hand.Contains(hand.HandList[i].Value - 3, hand.HandList[i].Suit))
                        {
                            if (hand.Contains(hand.HandList[i].Value - 4, hand.HandList[i].Suit))
                            {
                                straightFlush.HandList.Add(hand.HandList[i]);
                                straightFlush.HandList.Add(hand.HandList[i+1]);
                                straightFlush.HandList.Add(hand.HandList[i+2]);
                                straightFlush.HandList.Add(hand.HandList[i+3]);
                                straightFlush.HandList.Add(hand.HandList[i+4]);
                            }
                        }
                    }
                }
            }
            hand.Combination = HandCombinations.StraightFlush;
            return straightFlush;
        }

        public Hand GetFourOfAKindHand(Hand hand)
        {
            Hand fourOfAKind = new Hand();
            Card temp;
            for (int i = 0; i < hand.HandList.Count() - 3; i++)
            {
                temp = hand.HandList[i];
                if (temp.Value == hand.HandList[i + 1].Value && temp.Value == hand.HandList[i + 2].Value && temp.Value == hand.HandList[i + 3].Value)
                {
                    fourOfAKind.HandList.Add(hand.HandList[i]);
                    fourOfAKind.HandList.Add(hand.HandList[i+1]);
                    fourOfAKind.HandList.Add(hand.HandList[i+2]);
                    fourOfAKind.HandList.Add(hand.HandList[i+3]);

                    // Search for the high card by finding the first card that isn't a part of the four of a kind
                    for(int j = 0; j < hand.HandList.Count(); j++)
                    {
                        if(hand.HandList[j].Value != temp.Value)
                        {
                            fourOfAKind.HandList.Add(hand.HandList[j]);
                            break;
                        }
                    }
                    break;
                }
            }
            hand.Combination = HandCombinations.FourOfAKind;
            return fourOfAKind;
        }

        public Hand GetFullHouseHand(Hand hand)
        {
            Hand fullHouse = new Hand();
            for (int i = 0; i < hand.HandList.Count() - 2; i++)
            {
                if (hand.HandList[i].Value == hand.HandList[i + 1].Value && hand.HandList[i].Value == hand.HandList[i + 2].Value)
                {
                    // Make sure to start j at i + 1 so it doesn't pick up the same cards from above
                    for (int j = i + 1; j < hand.HandList.Count() - 1; j++)
                    {
                        if (hand.HandList[j].Value == hand.HandList[j + 1].Value)
                        {
                            fullHouse.HandList.Add(hand.HandList[i]);
                            fullHouse.HandList.Add(hand.HandList[i+1]);
                            fullHouse.HandList.Add(hand.HandList[i+2]);
                            fullHouse.HandList.Add(hand.HandList[j]);
                            fullHouse.HandList.Add(hand.HandList[j+1]);
                        }
                    }
                }
            }
            hand.Combination = HandCombinations.FullHouse;
            return fullHouse;
        }

        public Hand GetFlushHand(Hand hand)
        {
            Hand flush = new Hand();
            int hearts = 0;
            int diamonds = 0;
            int spades = 0;
            int clubs = 0;

            foreach (Card c in hand.HandList)
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

            for(int i = 0; i < hand.HandList.Count(); i++)
            {
                if(diamonds >= 5 && hand.HandList[i].Suit == Suits.Diamonds)
                {
                    flush.HandList.Add(hand.HandList[i]);
                }
                else if(spades >= 5 && hand.HandList[i].Suit == Suits.Spades)
                {
                    flush.HandList.Add(hand.HandList[i]);
                }
                else if(hearts >= 5 && hand.HandList[i].Suit == Suits.Hearts)
                {
                    flush.HandList.Add(hand.HandList[i]);
                }
                else if(clubs >= 5 && hand.HandList[i].Suit == Suits.Clubs)
                {
                    flush.HandList.Add(hand.HandList[i]);
                }

                if(flush.HandList.Count == 5)
                {
                    break;
                }
            }
            hand.Combination = HandCombinations.Flush;
            return flush;
        }

        public Hand GetStraightHand(Hand hand)
        {
            Hand straight = new Hand();
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
                                straight.HandList.Add(hand.HandList[i]);
                                straight.HandList.Add(hand.HandList[i+1]);
                                straight.HandList.Add(hand.HandList[i+2]);
                                straight.HandList.Add(hand.HandList[i+3]);
                                straight.HandList.Add(hand.HandList[i+4]);
                            }
                        }
                    }
                }
            }
            hand.Combination = HandCombinations.Straight;
            return straight;
        }

        public Hand GetThreeOfAKindHand(Hand hand)
        {
            Hand threeOfAKind = new Hand();
            for (int i = 0; i < hand.HandList.Count() - 2; i++)
            {
                if (hand.HandList[i].Value == hand.HandList[i + 1].Value && hand.HandList[i].Value == hand.HandList[i + 2].Value)
                {
                    threeOfAKind.HandList.Add(hand.HandList[i]);
                    threeOfAKind.HandList.Add(hand.HandList[i+1]);
                    threeOfAKind.HandList.Add(hand.HandList[i+2]);

                    for(int j = 0; j < hand.HandList.Count(); j++)
                    {
                        if(hand.HandList[j].Value != threeOfAKind.HandList[i].Value)
                        {
                            threeOfAKind.HandList.Add(hand.HandList[j]);
                        }

                        if(threeOfAKind.HandList.Count == 5)
                        {
                            break;
                        }
                    }
                    break;
                }
            }
            hand.Combination = HandCombinations.ThreeOfAKind;
            return threeOfAKind;
        }

        public Hand GetTwoPairHand(Hand hand)
        {
            Hand twoPair = new Hand();
            for (int i = 0; i < hand.HandList.Count() - 1; i++)
            {
                if (hand.HandList[i].Value == hand.HandList[i + 1].Value)
                {
                    // Make sure to start j at i + 1 so it doesn't pick up the same cards from above
                    for (int j = i + 1; j < hand.HandList.Count() - 1; j++)
                    {
                        if (hand.HandList[j].Value == hand.HandList[j + 1].Value)
                        {
                            twoPair.HandList.Add(hand.HandList[i]);
                            twoPair.HandList.Add(hand.HandList[i + 1]);
                            twoPair.HandList.Add(hand.HandList[j]);
                            twoPair.HandList.Add(hand.HandList[j + 1]);

                            for(int k = 0; k < hand.HandList.Count; k++)
                            {
                                if(hand.HandList[k].Value != twoPair.HandList[i].Value && hand.HandList[k].Value != twoPair.HandList[j].Value)
                                {
                                    twoPair.HandList.Add(hand.HandList[k]);
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                break;
            }
            hand.Combination = HandCombinations.TwoPair;
            return twoPair;
        }

        public Hand GetPairHand(Hand hand)
        {
            Hand pair = new Hand();

            for (int i = 0; i < hand.HandList.Count() - 1; i++)
            {
                if (hand.HandList[i].Value == hand.HandList[i + 1].Value)
                {
                    pair.HandList.Add(hand.HandList[i]);
                    pair.HandList.Add(hand.HandList[i + 1]);

                    for(int j = 0; j < hand.HandList.Count(); j++)
                    {
                        if(hand.HandList[j].Value != hand.HandList[i].Value)
                        {
                            pair.HandList.Add(hand.HandList[j]);
                        }

                        if(pair.HandList.Count >= 5)
                        {
                            break;
                        }
                    }
                }
                break;
            }
            hand.Combination = HandCombinations.Pair;
            return pair;
        }

        public Hand GetHighCardHand(Hand hand)
        {
            Hand highCard = new Hand();

            for(int i = 0; i < hand.HandList.Count; i++)
            {
                highCard.HandList.Add(hand.HandList[i]);

                if(highCard.HandList.Count >= 5)
                {
                    break;
                }
            }
            hand.Combination = HandCombinations.HighCard;
            return highCard;
        }

        #endregion GetBestHand
    }
}
