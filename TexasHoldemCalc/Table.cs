using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldemCalc
{
    // Handles the game itself
    class Table
    {
        // Fields
        int playersCount;
        int smallBlind;
        int bigBlind;
        int chipCount;
        int dealerPosition;
        int betAmount;
        int pot;
        Deck deck;
        List<Player> currentPlayers;
        List<Card> communityCards;

        // Properties
        public List<Player> CurrentPlayers { get { return currentPlayers; } }
        public List<Card> CommunityCards { get { return communityCards; } set { communityCards = value; } }

        // Gets the starting data for a game
        public Table(int playersCount, int smallBlind)
        {
            this.playersCount = playersCount+1;
            this.smallBlind = smallBlind;
            bigBlind = smallBlind * 2;
            chipCount = smallBlind * 100;
            deck = new Deck(this);
            dealerPosition = 0;
            pot = 0;
            communityCards = new List<Card>();
            betAmount = 0;
            // Create all the players at the table
            currentPlayers = new List<Player>();
            for (int i = 0; i <= playersCount; i++)
            {
                currentPlayers.Add(new Player(chipCount, i));
            }
        }


        /// <summary>
        /// Handles the game until somebody wins
        /// </summary>
        public void GameLoop()
        {
            // Deal a new hand to begin the next round
            NewHand();

            // First round of betting
            Betting(dealerPosition + 3, true);

            int roundCount = 1;
            // Loops through the rounds making sure there is more than 1 player
            while(CheckRemainingPlayers() && roundCount <= 3)
            {
                switch (roundCount)
                {
                    case 1:
                        // Deal Flop
                        deck.DealCommunityCards(3);
                        // Second round of betting
                        Betting(dealerPosition + 1, false);
                        break;
                    case 2:
                        // Deal Turn
                        deck.DealCommunityCards(1);
                        // Third round of betting
                        Betting(dealerPosition + 1, false);
                        break;
                    case 3:
                        // Deal River
                        deck.DealCommunityCards(1);
                        // Final round of betting
                        Betting(dealerPosition + 1, false);
                        break;
                }
            }
            // Decide the winner
            DecideWinner();
            // Restart the loop if more than one player remains
            GameLoop();
        }

        /// <summary>
        /// Checks to see if more than 1 player remains
        /// </summary>
        /// <returns>true if multiple players remain</returns>
        public bool CheckRemainingPlayers()
        {
            for(int i = 0; i < currentPlayers.Count(); i++)
            {
                if (!currentPlayers[i].Folded)
                {
                    for(int j = i + 1; j < currentPlayers.Count(); j++)
                    {
                        if (!currentPlayers[j].Folded)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Sets up a new hand
        /// </summary>
        public void NewHand()
        {
            deck.Shuffle();
            pot = 0;
            betAmount = bigBlind;
            // Take the blinds of players
            if(dealerPosition == playersCount-2)
            {
                currentPlayers[dealerPosition+1].Chips -= smallBlind;
                currentPlayers[dealerPosition+2].Chips -= bigBlind;
            }
            else if(dealerPosition == playersCount-1)
            {
                currentPlayers[0].Chips -= smallBlind;
                currentPlayers[1].Chips -= bigBlind;
            }
            else
            {
                currentPlayers[dealerPosition + 1].Chips -= smallBlind;
                currentPlayers[dealerPosition + 2].Chips -= bigBlind;
            }

            deck.Deal(dealerPosition, playersCount);
            PlayersHand();
        }

        /// <summary>
        /// Called for every round of betting
        /// </summary>
        public void Betting(int pos, bool start)
        {
            int startPos = pos;
            BettingOptions bettingOptions;

            // Checks to see if it is the start of a betting round
            // Usd to make sure the BB gets a turn
            if (start)
            {
                if(startPos >= currentPlayers.Count())
                {
                    startPos -= currentPlayers.Count();
                }
                bettingOptions = currentPlayers[startPos].Bet(betAmount);
                if(bettingOptions == BettingOptions.Raise)
                {
                    betAmount += currentPlayers[startPos].BetIncrease;
                    pot += betAmount;
                }
                else
                {
                    pot += betAmount;
                }
            }

            // Loop for the betting circle around the table
            for(int i = 1; i < currentPlayers.Count(); i++)
            {
                int temp = i + startPos;
                // Makes sure the loop doesn't hit out of bounds and continues
                if(temp >= currentPlayers.Count())
                {
                    temp -= currentPlayers.Count();
                }

                // If a player didn't fold then go through the betting process with them
                if (!currentPlayers[temp].Folded)
                {
                    bettingOptions = currentPlayers[temp].Bet(betAmount);

                    if (bettingOptions == BettingOptions.Raise)
                    {
                        betAmount += currentPlayers[startPos].BetIncrease;
                        pot += betAmount;
                        Betting(temp, false);
                        break;
                    }
                    else if(bettingOptions == BettingOptions.Call)
                    {
                        pot += betAmount;
                    }
                    else if(bettingOptions == BettingOptions.Fold)
                    {
                        currentPlayers[temp].Folded = true;
                    }
                }
            }
        }


        /// <summary>
        /// Decides the winner of the hand
        /// </summary>
        public void DecideWinner()
        {
            if (!CheckRemainingPlayers())
            {
                for(int i = 0; i < currentPlayers.Count(); i++)
                {
                    if (!currentPlayers[i].Folded)
                    {
                        currentPlayers[i].Chips += pot;
                        break;
                    }
                }
            }
        }

        public void PlayersHand()
        {
            Console.WriteLine("Your Hand:");
            foreach(Card c in currentPlayers[0].Hand)
            {
                Console.Write(c.ValueToString() + " of " + c.SuitsToString() + " ");
            }
            Console.WriteLine("\nYour Chip Count: " + currentPlayers[0].Chips);
        }
    }
}
