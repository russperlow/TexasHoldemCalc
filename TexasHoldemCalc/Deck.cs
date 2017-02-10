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
        Queue<Card> shuffledDeck; // The deck used in the game
        Random rand;
        Table table;

        /// <summary>
        /// Initializes The Deck Class
        /// </summary>
        public Deck(Table table)
        {
            // Initializes all objects & lists
            newDeckOrder = new List<Card>();
            shuffledDeck = new Queue<Card>();
            tempDeck = new List<Card>();
            rand = new Random();
            this.table = table;
            // Creates the deck in NDO
            for(int i = 0; i < 4; i++)
            {
                for (int j = 2; j <= 14; j++)
                {
                    newDeckOrder.Add(new Card((Suits)i, (Values)j));
                }
            }
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
                shuffledDeck.Enqueue(tempDeck[chosenCard]);
                tempDeck.Remove(tempDeck[chosenCard]);   // Makes sure a card can't be repeated
            }
        }

        /// <summary>
        /// Deals out the cards to each player
        /// </summary>
        public void Deal(int dealerPosition, int playerCount)
        {
            for(int i = 0; i < playerCount * 2; i++)
            {
                if(dealerPosition + i < playerCount)
                {
                    if (dealerPosition + i != 0)
                    {
                        table.CurrentPlayers[dealerPosition + i].Hand.Add(shuffledDeck.Dequeue());
                    }
                    else
                    {
                        shuffledDeck.Peek().FaceUp = true;
                        table.CurrentPlayers[0].Hand.Add(shuffledDeck.Dequeue());
                    }
                }
                else
                {
                    int temp = dealerPosition + i - playerCount;

                    if(temp > (playerCount * 2 - 1))
                    {
                        temp = dealerPosition + i - (playerCount * 2);
                    }

                    if(temp == 0)
                    {
                        shuffledDeck.Peek().FaceUp = true;
                        table.CurrentPlayers[0].Hand.Add(shuffledDeck.Dequeue());
                    }
                    else
                    {
                        table.CurrentPlayers[temp].Hand.Add(shuffledDeck.Dequeue());
                    }
                }
            }
        }

        /// <summary>
        /// Deals out community cards
        /// </summary>
        /// <param name="cardsToDeal">The number of cards to deal</param>
        public void DealCommunityCards(int cardsToDeal)
        {
            for (int i = 0; i < cardsToDeal; i++)
            {
                shuffledDeck.Peek().FaceUp = true;
                table.CommunityCards.Add(shuffledDeck.Dequeue());
            }
        }
    }
}
