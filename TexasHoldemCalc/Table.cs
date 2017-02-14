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
        List<Player> potWinners;

        // Properties
        public List<Player> CurrentPlayers { get { return currentPlayers; } }
        public List<Card> CommunityCards { get { return communityCards; } set { communityCards = value; } }
        public int BigBlind { get { return bigBlind; } }
        public int Pot { get { return pot; } }

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
            potWinners = new List<Player>();
            betAmount = 0;
            // Create all the players at the table
            currentPlayers = new List<Player>();
            for (int i = 0; i <= playersCount; i++)
            {
                currentPlayers.Add(new Player(chipCount, i, this));
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
            Betting(dealerPosition + 3, true, 1);

            int roundCount = 1;
            // Loops through the rounds making sure there is more than 1 player
            while(CheckRemainingPlayers() && roundCount <= 3)
            {
                WriteOut();
                switch (roundCount)
                {
                    case 1:
                        // Deal Flop
                        deck.DealCommunityCards(3);
                        break;
                    case 2:
                        // Deal Turn
                        deck.DealCommunityCards(1);
                        break;
                    case 3:
                        // Deal River
                        deck.DealCommunityCards(1);
                        break;
                }
                // Handle each round of betting
                Betting(dealerPosition + 1, false, roundCount);
                roundCount++;
            }
            // Decide the winner
            DecideWinner();
            // Restart the loop if more than one player remains
            GameLoop();
        }

        public void WriteOut()
        {
            Console.Clear();
            Console.WriteLine("Community Cards: ");
            foreach (Card c in communityCards)
            {
                Console.Write(c.ValueToString() + " of " + c.SuitsToString() + " ");
            }
            Console.Write("\n");

            Console.WriteLine("Pot: " + pot);
            PlayersHand();
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
            foreach(Player p in currentPlayers)
            {
                p.Folded = false;
                p.BestHand = null;
                p.HoleCards.Clear();
            }

            deck.Shuffle();
            pot = smallBlind + bigBlind;
            betAmount = bigBlind;
            // Take the blinds of players
            if(dealerPosition == playersCount-2)
            {
                currentPlayers[dealerPosition+1].Chips -= smallBlind;
                currentPlayers[0].Chips -= bigBlind;
                currentPlayers[dealerPosition + 1].SmallBlind = true;
                currentPlayers[0].BigBlind = true;
            }
            else if(dealerPosition == playersCount-1)
            {
                currentPlayers[0].Chips -= smallBlind;
                currentPlayers[1].Chips -= bigBlind;
                currentPlayers[0].SmallBlind = true;
                currentPlayers[1].BigBlind = true;
            }
            else
            {
                currentPlayers[dealerPosition + 1].Chips -= smallBlind;
                currentPlayers[dealerPosition + 2].Chips -= bigBlind;
                currentPlayers[dealerPosition + 1].SmallBlind = true;
                currentPlayers[dealerPosition + 2].BigBlind = true;
            }

            deck.Deal(dealerPosition, playersCount);
            PlayersHand();
        }

        /// <summary>
        /// Called for every round of betting
        /// </summary>
        public void Betting(int pos, bool start, int round)
        {
            int startPos = pos;
            BettingOptions bettingOptions;

            // Checks to see if it is the start of a betting round
            // Used to make sure the BB gets a turn
            if (start)
            {
                if(startPos >= currentPlayers.Count())
                {
                    startPos -= currentPlayers.Count();
                }
                bettingOptions = currentPlayers[startPos].Bet(betAmount, round);
                if(bettingOptions == BettingOptions.Raise)
                {
                    betAmount += currentPlayers[startPos].BetIncrease;
                    pot += betAmount;
                }
                else if(!currentPlayers[startPos].Folded)
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
                    bettingOptions = currentPlayers[temp].Bet(betAmount, round);

                    if (bettingOptions == BettingOptions.Raise)
                    {
                        betAmount += currentPlayers[startPos].BetIncrease;
                        pot += betAmount;
                        Betting(temp, false, round);
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

            foreach(Player p in currentPlayers)
            {
                p.GetBestHand(new HandCombination(), communityCards);
            }

            Player bestPlayer = null; 
            for(int i = 0; i < currentPlayers.Count(); i++)
            {
                // Find the first not folded player and assign them the best
                if(!currentPlayers[i].Folded && bestPlayer == null)
                {
                    bestPlayer = currentPlayers[i];
                    potWinners.Add(bestPlayer);
                }
                else if(!currentPlayers[i].Folded)
                {
                    // Checks to see if another hand has the best current hand beat or tied
                    if(currentPlayers[i].BestHand.Combination > bestPlayer.BestHand.Combination)
                    {
                        bestPlayer = currentPlayers[i];
                        potWinners.Clear();
                        potWinners.Add(bestPlayer);
                    }
                    else if(currentPlayers[i].BestHand.Combination == bestPlayer.BestHand.Combination)
                    {
                        // Checks if there is a hand ranking tie for the best card
                        for(int j = 0; j < bestPlayer.BestHand.HandList.Count; j++)
                        {
                            if(currentPlayers[i].BestHand.HandList[j].Value > bestPlayer.BestHand.HandList[j].Value)
                            {
                                bestPlayer = currentPlayers[i];
                                break;
                            }
                            else if(j == bestPlayer.BestHand.HandList.Count - 1)
                            {
                                // If the hand is an absolute tie then make a split pot
                                bestPlayer = currentPlayers[i];
                                potWinners.Add(bestPlayer);                                
                            }
                            else if(currentPlayers[i].BestHand.HandList[j].Value < bestPlayer.BestHand.HandList[j].Value)
                            {
                                break;
                            }
                        }
                    }
                }

            }
        }

        public void PlayersHand()
        {
            Console.WriteLine("Your Hand:");
            foreach(Card c in currentPlayers[0].HoleCards)
            {
                Console.Write(c.ValueToString() + " of " + c.SuitsToString() + " ");
            }
            Console.WriteLine("\nYour Chip Count: " + currentPlayers[0].Chips);
        }
    }
}
