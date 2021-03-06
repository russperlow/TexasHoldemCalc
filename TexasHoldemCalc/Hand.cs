﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldemCalc
{
    /// <summary>
    /// Represents players hand
    /// </summary>
    class Hand
    {
        // Fields
        Card cOne, cTwo, cThree, cFour, cFive, cSix, cSeven, highCard;
        List<Card> handList;
        HandCombinations combination;

        // Properties
        public List<Card> HandList { get { return handList; } }
        public HandCombinations Combination { get { return combination; } set { combination = value; } }
        public Card HighCard { get { return highCard; } set { highCard = value; } }

        /// <summary>
        /// Constuctor, gets all 7 cards and then storts them
        /// </summary>
        /// <param name="cNum">The cards of the hand numbered 1-7</param>
        public Hand(Card cOne, Card cTwo, Card cThree, Card cFour, Card cFive, Card cSix, Card cSeven)
        {
            this.cOne = cOne;
            this.cTwo = cTwo;
            this.cThree = cThree;
            this.cFour = cFour;
            this.cFive = cFive;
            this.cSix = cSix;
            this.cSeven = cSeven;

            handList = new List<Card>() { cOne, cTwo, cThree, cFour, cFive, cSix, cSeven };
            SortHand();
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Hand()
        {
            handList = new List<Card>();
            highCard = null;
        }
        


        /// <summary>
        /// Helper method in sorting the handList highest to lowest
        /// </summary>
        public void SortHand()
        {
            for(int i = 0; i < handList.Count; i++)
            {
                for(int j = 0; j < handList.Count - 1; j++)
                {
                    if(handList[j].Value < handList[j + 1].Value)
                    {
                        Card temp = handList[j];
                        handList[j] = handList[j + 1];
                        handList[j + 1] = temp;
                    }
                }
            }
            /*
            List<Card> temp = new List<Card>();
            temp = handList;
            for (int i = 0; i < 7; i++)
            {
                handList[i] = SortingLoop(temp);
            }
            */
        }

        /*
        /// <summary>
        /// Loops through remaining cards to find the next highest
        /// </summary>
        /// <returns>The highest found card</returns>
        public Card SortingLoop(List<Card> temp2)
        {
            
            /*
            Card highestCard = temp2[0];
            for(int j = 0; j < temp2.Count(); j++)
            {
                if(highestCard.Value < temp2[j].Value)
                {
                    highestCard = temp2[j];
                }
            }
            temp2.Remove(highestCard);
            return highestCard;
            
        }
        */
        /// <summary>
        /// Value and suit necessary for straight/royal flush
        /// </summary>
        /// <param name="value">A cards value</param>
        /// <param name="suit">A cards suit</param>
        /// <returns>True if the given card is in the hand</returns>
        public bool Contains(Values value, Suits suit)
        {
            foreach(Card c in handList)
            {
                if(c.Value == value && c.Suit == suit)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Just checks for a value
        /// Used for straights, pairs, 3 of a kind and full house
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>True if the given card is in the hand</returns>
        public bool Contains(Values value)
        {
            foreach (Card c in handList)
            {
                if (c.Value == value)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks for just the suit of a card
        /// </summary>
        /// <param name="suit">Suit</param>
        /// <returns>True if the given suit is in the hand</returns>
        public bool Contains(Suits suit)
        {
            foreach (Card c in handList)
            {
                if (c.Suit == suit)
                {
                    return true;
                }
            }
            return false;
        }

        
    }
}
