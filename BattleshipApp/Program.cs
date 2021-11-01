using BattleshipLiteLibrary;
using BattleshipLiteLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipApp
{
    class Program
    {
        static void Main(string[] args)
        {
            PlayerInfoModel activePlayer = new PlayerInfoModel();
            PlayerInfoModel opponent = new PlayerInfoModel();
            PlayerInfoModel winner = null; 
            

            WelcomeMessage();
            activePlayer = CreatePlayer("player 1");
            opponent = CreatePlayer("player 2"); 

            do
            {
                Console.WriteLine(); 
                Console.WriteLine($"The shotgrid of {activePlayer.Name}:"); 
                //Display shot grid from activePlayer
                DisplayShotGrid(activePlayer);

                Console.WriteLine();
                Console.WriteLine(); 

                //Ask actPlayer for a shot
                //Determine if it is a valid shot
                //Store it
                GridSpotModel playersShot = RecordPlayerShot(activePlayer); 

                //Determine shot results - hit or miss 
                GameLogic.DetermineShot(playersShot, opponent, activePlayer); 

                //Display results for both players 
                DisplayResults(activePlayer, opponent); 

                //Determine if the game continues or not
                bool doesGameContinue = GameLogic.CheckIfGameContinues(opponent); 

                if(doesGameContinue == true)
                {
                    (activePlayer, opponent) = (opponent, activePlayer); 
                }
                else
                {
                    winner = activePlayer; 
                }

            } while (winner == null);
            
            Console.WriteLine(); 

            IdentifyWinner(winner); 

            Console.ReadLine(); 
        } 

        private static void IdentifyWinner(PlayerInfoModel winner)
        {
            Console.WriteLine($"The winner is {winner.Name}! Congratulations!");
            Console.WriteLine($"{winner.Name} took {GameLogic.TotalShotsTaken(winner)} shots to win"); 
        }

        private static void DisplayResults(PlayerInfoModel activePlayer, PlayerInfoModel opponent)
        {
            int activePlayerCounter = 0;
            int opponenCounter = 0; 

            foreach(GridSpotModel gridSpot in activePlayer.ShipLocations)
            {
                if(gridSpot.Status != GridSpotStatus.Sunk)
                {
                    activePlayerCounter++; 
                }
            }

            foreach (GridSpotModel gridSpot in opponent.ShipLocations)
            {
                if (gridSpot.Status != GridSpotStatus.Sunk)
                {
                    opponenCounter++; 
                }
            }

            Console.WriteLine($"{activePlayer.Name} has {activePlayerCounter} ships left");
            Console.WriteLine($"{opponent.Name} has {opponenCounter} ships left"); 

        }

        private static GridSpotModel RecordPlayerShot(PlayerInfoModel activePlayer)
        {
            GridSpotModel playersShot = new GridSpotModel(); 
            bool isValidShot = false; 
            do
            {
                string shot = AskForShot(activePlayer); 
                isValidShot = GameLogic.ValidateShot(shot, playersShot, activePlayer); 
                if(isValidShot == false)
                {
                    Console.WriteLine("Invalid shot"); 
                }
            } while (isValidShot == false);
            Console.Clear(); 
            return playersShot; 

        }

        private static string AskForShot(PlayerInfoModel player)
        {
            Console.Write($"Where would you like to shoot at, {player.Name}: "); 
            string output = Console.ReadLine();
            return output; 
        }

        private static void DisplayShotGrid(PlayerInfoModel opponent)
        {
            string currentRow = opponent.ShotGrid[0].SpotLetter; //Remove 0 

            foreach(GridSpotModel gridSpot in opponent.ShotGrid)
            {
                if(gridSpot.SpotLetter != currentRow)
                {
                    Console.WriteLine(); 
                    currentRow = gridSpot.SpotLetter; 
                }

                if(gridSpot.Status == GridSpotStatus.Empty)
                {
                    Console.Write($" {gridSpot.SpotLetter}{gridSpot.SpotNumber} "); 
                }
                else if(gridSpot.Status == GridSpotStatus.Hit)
                {
                    Console.Write(" X "); 
                }
                else if(gridSpot.Status == GridSpotStatus.Miss)
                {
                    Console.Write(" O "); 
                }
                else
                {
                    Console.Write(" ? "); 
                }
            }
        }

        private static void WelcomeMessage()
        {
            Console.WriteLine("Welcome to Battleship Lite!");
            Console.WriteLine("created by Ivo Kostadinov");
            Console.WriteLine(); 
        }

        private static PlayerInfoModel CreatePlayer(string playerNumber)
        {
            Console.WriteLine($"Information for {playerNumber}: "); 
            PlayerInfoModel model = new PlayerInfoModel();
            //Ask player1 for their name
            model.Name = AskForName();
            //Ask player1 for their ship placements
            //Validate ship placement
            PlaceShips(model); 
            //Initialize shot grid
            GameLogic.InitializeShotGrid(model);
            //Clear 
            Console.Clear(); 

            return model; 
        } 

        private static void PlaceShips(PlayerInfoModel model)
        {
            do
            {
                Console.Write($"Choose a spot for ship number {model.ShipLocations.Count + 1}: ");
                string spot = Console.ReadLine();
                bool isValidSpot = GameLogic.CheckShip(model, spot); 
                if(isValidSpot == false)
                {
                    Console.WriteLine("Invalid spot was entered. Try again"); 
                }
                
            } while (model.ShipLocations.Count < 5); 
        }

        private static string AskForName()
        {
            Console.Write("What's your name: ");
            string output = Console.ReadLine();
            return output; 
        }

        //public static void DisplayScore() 
        
    }
}
