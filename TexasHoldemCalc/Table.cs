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
            Console.WriteLine("================================================== NEW HAND ==================================================");

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
                p.AllIn = false;
                p.BestHand = null;
                p.HoleCards.Clear();
            }

            deck.Shuffle();
            betAmount = bigBlind;

            // Take the blinds of players
            if(dealerPosition == playersCount-2)
            {
                TakeBlinds(0, dealerPosition + 1);
            }
            else if(dealerPosition == playersCount-1)
            {
                TakeBlinds(0, 1);
            }
            else
            {
                TakeBlinds(dealerPosition + 1, dealerPosition + 2);
            }

            deck.Deal(dealerPosition, playersCount);
            PlayersHand();
        }

        /// <summary>
        /// Takes the blinds checking to make sure they can be afforded
        /// </summary>
        /// <param name="bb">Big Blind Pos</param>
        /// <param name="sb">Small Blind Pos</param>
        public void TakeBlinds(int bb, int sb)
        {
            if(currentPlayers[bb].Chips >= bigBlind)
            {
                currentPlayers[bb].Chips -= bigBlind;
                currentPlayers[bb].BigBlind = true;
                pot += bigBlind;
            }
            else
            {
                pot += currentPlayers[bb].Chips;
                currentPlayers[bb].Chips = 0;
                currentPlayers[bb].AllIn = true;
                currentPlayers[bb].BigBlind = true;
            }

            if (currentPlayers[sb].Chips >= smallBlind)
            {
                currentPlayers[sb].Chips -= smallBlind;
                currentPlayers[sb].SmallBlind = true;
                pot += smallBlind;
            }
            else
            {
                pot += currentPlayers[sb].Chips;
                currentPlayers[sb].Chips = 0;
                currentPlayers[sb].AllIn = true;
                currentPlayers[sb].SmallBlind = true;
            }
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

                // If a player didn't fold or go all in then go through the betting process with them
                if (!currentPlayers[temp].Folded && !currentPlayers[temp].AllIn)
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
                        if (currentPlayers[i].User)
                            Console.WriteLine("You won $" + pot + "!");
                        else
                            Console.WriteLine("Player " + currentPlayers[i].PlayerNumber + " won " + " $" + pot);
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
