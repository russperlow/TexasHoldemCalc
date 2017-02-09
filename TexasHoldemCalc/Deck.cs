using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldemCalc
{
    enum Suits
    {
        Spades,
        Hearts,
        Diamonds,
        Clubs
    }
    enum Values
    {
        Two=2,
        Three,
        Four, 
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace
    }
    
    class Deck
    {
        List<Card> newDeckOrder; // Original deck of cards always in same order
        List<Card> tempDeck; // Used for removing cards when shuffling
        List<Card> shuffledDeck; // The deck used in the game
        Random rand;

        /// <summary>
        /// Initializes The Deck Class
        /// </summary>
        public Deck()
        {
            // Initializes all objects & lists
            newDeckOrder = new List<Card>();
            shuffledDeck = new List<Card>();
            tempDeck = new List<Card>();
            rand = new Random();

            // Creates the deck in NDO
            for(int i = 0; i < 4; i++)
            {
                for (int j = 2; j <= 14; j++)
                {
                    newDeckOrder.Add(new Card((Suits)i, (Values)j));
                }
            }

            // Makes sure shuffled deck has 52 occupied spaces
            shuffledDeck = newDeckOrder;
        }

        /// <summary>
        /// Reset the temp deck for next shuffling
        /// </summary>
        public void Replace()
        {
            foreach(Card c in newDeckOrder)
            {
                tempDeck.Add(c);
            }
        }

        /// <summary>
        /// Takes a random card from temp deck and adds it to the next position in the shuffled deck
        /// </summary>
        public void Shuffle()
        {
            Replace(); // Ensures temp deck is always full
            int chosenCard; // Position of chosen card

            // Shuffling
            for (int i = 0; i < 52; i++)
            {
                chosenCard = rand.Next(0, 52-i);
                shuffledDeck[i] = tempDeck[chosenCard];
                tempDeck.Remove(tempDeck[chosenCard]);   // Makes sure a card can't be repeated
            }
        }

        /// <summary>
        /// Deals out the cards to each player
        /// </summary>
        public void Deal(int dealerPosition)
        {
            
        }
    }
}
