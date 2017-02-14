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
        int betAmount;
        bool pair;

        public AIPlayer(Player player)
        {
            this.player = player;
            strength = 0;
            pair = false;
            rand = new Random();
            bluffFreq = rand.Next(100, 200);
        }

        public BettingOptions Betting(int betAmount, int roundCount)
        {
            Thread.Sleep(rand.Next(6000));            
            this.betAmount = betAmount;
            switch (roundCount)
            {
                case 1:
                    strength = 0;
                    pair = false;
                    return PreFlop();
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
            }

            return BettingOptions.Fold;
        }


        public void BestPossibleHand(List<Card> hand)
        {
           
        }

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
                pair = true;
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

            if (pair)
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


        /// <summary>
        /// Determines How Much Will Be Bet
        /// </summary>
        /// <returns>The bet amount</returns>
        public int BetAmount()
        {
            int randInt = rand.Next(0, 10);

            if(randInt < 2)
            {
                return rand.Next(player.Table.Pot / 4) + player.Table.Pot / 2;
            }
            else if(randInt > 7)
            {
                return rand.Next(player.Table.Pot * 3) + player.Table.Pot;
            }
            else
            {
                return rand.Next((int)(3 / 4 * player.Table.Pot), (int)(player.Table.Pot + (player.Table.Pot / 2) + 1));
            }
        }
    }
}
