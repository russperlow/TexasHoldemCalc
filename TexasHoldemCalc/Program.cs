using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldemCalc
{
    class Program
    {
        static void Main(string[] args)
        {
            Table table = StartUpInput();
            table.GameLoop();
            Console.ReadLine();
        }

        /// <summary>
        /// Gets all the startup information
        /// </summary>
        /// <returns>The game table object</returns>
        public static Table StartUpInput()
        {
            string userInput;
            Console.WriteLine("How many players would you like to play against? 1-8");
            userInput = Console.ReadLine();

            // Get the starting number of players at the table
            while (Int32.Parse(userInput) > 8 || Int32.Parse(userInput) < 1)
            {
                Console.WriteLine("The players must be between 1 and 8. Please try again!");
                userInput = Console.ReadLine();
            }
            int players = Int32.Parse(userInput);

            Console.WriteLine("What would you like the starting small blinds to be?");
            userInput = Console.ReadLine();

            // Get the starting small blind numbers
            while(Int32.Parse(userInput) <= 0)
            {
                Console.WriteLine("You must have a positive starting number for the blinds! Try again!");
            }
            int smallBlind = Int32.Parse(userInput);

            return new Table(players, smallBlind);
        }

    }
}
