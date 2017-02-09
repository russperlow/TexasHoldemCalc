﻿using System;
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
        int pot;
        Deck deck;
        Player player;
        List<AIPlayer> currentPlayers;
        List<Card> communityCards;

        // Properties
        public List<AIPlayer> CurrentPlayers { get { return currentPlayers; } }
        public Player PlayerObj { get { return player; } }
        public List<Card> CommunityCards { get { return communityCards; } set { communityCards = value; } }

        // Gets the starting data for a game
        public Table(int playersCount, int smallBlind)
        {
            this.playersCount = playersCount;
            this.smallBlind = smallBlind;
            bigBlind = smallBlind * 2;
            chipCount = smallBlind * 100;
            deck = new Deck(this);
            dealerPosition = 0;
            pot = 0;
            communityCards = new List<Card>();

            // Create all the ai players at the table
            currentPlayers = new List<AIPlayer>();
            
            for(int i = 1; i <= playersCount; i++)
            {
                currentPlayers.Add(new AIPlayer(chipCount, i));
            }

            // Create the user player obj
            player = new Player(chipCount);
        }

        /// <summary>
        /// Sets up a new hand
        /// </summary>
        public void NewHand()
        {
            deck.Shuffle();
            pot = 0;

            // Take the blinds of players
            if(dealerPosition == playersCount-2)
            {
                currentPlayers[dealerPosition+1].Chips -= smallBlind;
                player.Chips -= bigBlind;
            }
            else if(dealerPosition == playersCount-1)
            {
                player.Chips -= smallBlind;
                currentPlayers[0].Chips -= bigBlind;
            }
            else
            {
                currentPlayers[dealerPosition + 1].Chips -= smallBlind;
                currentPlayers[dealerPosition + 2].Chips -= bigBlind;
            }

            deck.Deal(dealerPosition, playersCount);
            PlayersHand();
        }

        public void PlayersHand()
        {
            Console.WriteLine("Your Hand:");
            foreach(Card c in player.Hand)
            {
                Console.Write(c.ValueToString() + " of " + c.SuitsToString() + " ");
            }
            Console.WriteLine("\nYour Chip Count: " + player.Chips);
        }
    }
}
