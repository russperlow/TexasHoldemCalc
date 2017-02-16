using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TexasHoldemCalc
{
    /// <summary>
    /// All the players that aren't the user
    /// </summary>
    class AIPlayer
    {
        Player player;
        Random rand;
        double strength;
        int bluffFreq;
        double betAmount;
        int simCount;
        int decisionNum;
        double rateOfReturn, potOdds, averageOdds;
        bool pairBool;
        List<Card> fullDeck;
        Queue<Card> simDeck;
        List<Card> tempDeck;
        List<Card> knownCards;
        HandCombination hc;
        
        public AIPlayer(Player player)
        {
            this.player = player;
            strength = 0;
            pairBool = false;
            rand = new Random();
            bluffFreq = rand.Next(100, 200);

            fullDeck = new List<Card>();
            simDeck = new Queue<Card>();
            tempDeck = new List<Card>();
            hc = new HandCombination();
            knownCards = new List<Card>();

            // Creates the deck in NDO
            for (int i = 0; i < 4; i++)
            {
                for (int j = 2; j <= 14; j++)
                {
                    fullDeck.Add(new Card((Suits)i, (Values)j));
                }
            }
            
        }

        public BettingOptions Betting(int betAmount, int roundCount)
        {
            Thread.Sleep(rand.Next(6000));            
            this.betAmount = betAmount;
            strength = 0;

            // Create a list of all known cards by the player
            knownCards.Clear();
            knownCards.Add(player.HoleCards[0]);
            knownCards.Add(player.HoleCards[1]);
            foreach(Card c in player.Table.CommunityCards)
            {
                knownCards.Add(c);
            }

            return GetOdds();
        }

        public BettingOptions GetOdds()
        {
            simCount = 0;

            while(simCount < 1000)
            {
                switch (hc.GetStrongestHand(ShuffleSimulationDeck(knownCards)).Combination)
                {
                    case HandCombinations.RoyalFlush:
                        averageOdds += 1;
                        break;
                    case HandCombinations.StraightFlush:
                        averageOdds += .9;
                        break;
                    case HandCombinations.FourOfAKind:
                        averageOdds += .8;
                        break;
                    case HandCombinations.FullHouse:
                        averageOdds += .7;
                        break;
                    case HandCombinations.Flush:
                        averageOdds += .6;
                        break;
                    case HandCombinations.Straight:
                        averageOdds += .5;
                        break;
                    case HandCombinations.ThreeOfAKind:
                        averageOdds += .4;
                        break;
                    case HandCombinations.TwoPair:
                        averageOdds += .3;
                        break;
                    case HandCombinations.Pair:
                        averageOdds += .2;
                        break;
                    default:
                        averageOdds += .1;
                        break;
                }
                simCount++;
            }

            // Calculate rate of return
            averageOdds /= 1000;
            potOdds = betAmount / (betAmount + player.Table.Pot);
            if(potOdds > 0) // DONT DIVIDE BY 0
                rateOfReturn = averageOdds / potOdds;

            // Used to make decisions of betting/calling/folding
            decisionNum = rand.Next(100);

            // Based off the rate of return they player makes their mvoe
            if(rateOfReturn < .8)
            {
                if(decisionNum < 95)
                {
                    if (betAmount == 0)
                        return BettingOptions.Check;
                    return BettingOptions.Fold;
                }
                else
                {
                    player.RaiseAmount = BetAmount();
                    return BettingOptions.Raise;
                }
            }
            else if (rateOfReturn < 1)
            {
                if(decisionNum < 80)
                {
                    if (betAmount == 0)
                        return BettingOptions.Check;
                    return BettingOptions.Fold;
                }
                else if(decisionNum >= 80 && decisionNum < 85)
                {
                    if (betAmount == 0)
                        return BettingOptions.Check;
                    return BettingOptions.Call;
                }
                else
                {
                    player.RaiseAmount = BetAmount();
                    return BettingOptions.Raise;
                }
            }
            else if(rateOfReturn < 1.3)
            {
                if (decisionNum < 60)
                {
                    if (betAmount == 0)
                        return BettingOptions.Check;
                    return BettingOptions.Call;
                }
                else
                {
                    player.RaiseAmount = BetAmount();
                    return BettingOptions.Raise;
                }
            }
            else
            {
                if (decisionNum < 30)
                {
                    if (betAmount == 0)
                        return BettingOptions.Check;
                    return BettingOptions.Call;
                }
                else
                {
                    player.RaiseAmount = BetAmount();
                    return BettingOptions.Raise;
                }
            }
        }


        /// <summary>
        /// Shuffles The Deck Used For Simulating Hands
        /// </summary>
        /// <param name="knownCards">Any card the player can see</param>
        /// <param name="shufflingDeck"></param>
        /// <returns></returns>
        public Hand ShuffleSimulationDeck(List<Card> knownCards)
        {
            // Take any known cards out of the simulation deck
            foreach(Card c in fullDeck)
            {
                tempDeck.Add(c);
            }
            simDeck.Clear();

            // Remove all cards from temp deck that are known
            for (int i = 0; i < knownCards.Count; i++)
            {
                for(int j = 0; j < tempDeck.Count; j++)
                {
                    if(knownCards[i].Value == tempDeck[j].Value && knownCards[i].Suit == tempDeck[j].Suit)
                    {
                        tempDeck.Remove(tempDeck[j]);
                        break;
                    }
                }
            }


            // Set the sim deck to the full deck
            /*
            foreach (Card c in tempDeck)
            {
                simDeck.Enqueue(c);
            }
            */

            int chosenCard; // Position of chosen card
            int count = tempDeck.Count;
            // Shuffling
            for (int i = 0; i < count; i++)
            {
                chosenCard = rand.Next(0, count - i);
                simDeck.Enqueue(tempDeck[chosenCard]);
                tempDeck.Remove(tempDeck[chosenCard]);   // Makes sure a card can't be repeated
            }

            // Deal out cards until there are 7 in the hand
            for(int i = knownCards.Count; i < 7; i++)
            {
                simDeck.Dequeue(); // Burn
                knownCards.Add(simDeck.Dequeue()); // Deal
            }

            return new Hand(knownCards[0], knownCards[1], knownCards[2], knownCards[3], knownCards[4], knownCards[5], knownCards[6]);
        }

        /// <summary>
        /// Determines How Much Will Be Bet
        /// </summary>
        /// <returns>The bet amount</returns>
        public int BetAmount()
        {
            int randInt = rand.Next(0, 10);
            int bet;
            if(randInt < 2)
            {
                bet = rand.Next(player.Table.Pot / 4) + player.Table.Pot / 2;
            }
            else if(randInt > 7)
            {
                bet = rand.Next(player.Table.Pot * 3) + player.Table.Pot;
            }
            else
            {
                bet = rand.Next((int)(3 / 4 * player.Table.Pot), (int)(player.Table.Pot + (player.Table.Pot / 2) + 1));
            }

            if(bet > player.Chips)
            {
                bet = player.Chips;
            }

            return bet;
        }
    }
}



/*
/// <summary>
/// Determines the strength of a hand based off the players hole cards
/// </summary>
/// <returns>The strength as a double</returns>
public BettingOptions PreFlop()
{
    // Get basic card values
    foreach(Card c in player.HoleCards)
    {

        switch (c.Value)
        {
            case Values.Ace:
                strength += 10;
                break;
            case Values.King:
                strength += 8;
                break;
            case Values.Queen:
                strength += 7;
                break;
            case Values.Jack:
                strength += 6;
                break;
            default:
                strength += (double)c.Value / 2;
                break;
        }
    }

    // Check to see if they are a pair or suited and give it a bonus
    if (player.HoleCards[0].Value == player.HoleCards[1].Value)
    {
        strength *= 2;
        pairBool = true;
        // Set a minimum for pairs since 3 and 2 would be below a strength of 5
        if(strength < 5)
        {
            strength = 5;
        }
    }
    else if (player.HoleCards[0].Suit == player.HoleCards[1].Suit)
    {
        strength += 2;
    }

    // Subtract/Add for the gap between the cards from the strength
    if(player.HoleCards[0].Value == player.HoleCards[1].Value + 1 || player.HoleCards[0].Value == player.HoleCards[1].Value - 1)
    {
        if(player.HoleCards[0].Value > Values.Jack || player.HoleCards[1].Value > Values.Jack)
        {
            strength += 2;
        }
        else
        {
            strength += 1;
        }
    }
    else if (player.HoleCards[0].Value == player.HoleCards[1].Value + 2 || player.HoleCards[0].Value == player.HoleCards[1].Value - 2)
    {
        if (player.HoleCards[0].Value > Values.Jack || player.HoleCards[1].Value > Values.Jack)
        {
            strength += 1;
        }
        else
        {
            strength += .5;
        }
    }
    else if(player.HoleCards[0].Value > player.HoleCards[1].Value)
    {
        strength -= player.HoleCards[0].Value - player.HoleCards[1].Value + 2;
    }
    else
    {
        strength -= player.HoleCards[1].Value - player.HoleCards[0].Value + 2;
    }

    if (pairBool)
    {
        if (strength >= 14)
        {
            return BettingOptions.Call;
        }
        else
        {
            // Chances of a bluff
            int bluffOdd = rand.Next(0, bluffFreq);
            if (bluffFreq == 0)
            {
                player.RaiseAmount = BetAmount();
                return BettingOptions.Raise;
            }
            else if (bluffOdd == bluffFreq - 1)
            {
                return BettingOptions.Call;
            }

            int temp;
            temp = (int)(strength - (player.BetIncrease / player.Table.BigBlind) + rand.Next(0, (int)(strength)));

            if (temp >= 14)
            {
                return BettingOptions.Call;
            }
            else
            {
                if (player.BigBlind && betAmount - player.Table.BigBlind == 0)
                    return BettingOptions.Check;
                return BettingOptions.Fold;
            }

        }
    }
    else
    {

        if(strength >= 14)
        {
            return BettingOptions.Call;
        }
        else if(strength <= 5)
        {
            if (player.BigBlind && betAmount - player.Table.BigBlind == 0)
                return BettingOptions.Check;
            return BettingOptions.Fold;
        }
        else
        {
            // Chances of a bluff
            int bluffOdd = rand.Next(0, bluffFreq);
            if(bluffFreq == 0)
            {
                player.RaiseAmount = BetAmount();
                return BettingOptions.Raise;
            }
            else if(bluffOdd == bluffFreq-1)
            {
                return BettingOptions.Call;
            }

            int temp;
            temp = (int)(strength - (player.BetIncrease / player.Table.BigBlind) + rand.Next((int)(strength)));

            if (temp >= 14)
            {
                return BettingOptions.Call; 
            }
            else
            {
                if (player.BigBlind && betAmount - player.Table.BigBlind == 0)
                    return BettingOptions.Check;
                return BettingOptions.Fold;
            }

        }
    }
}

*/
