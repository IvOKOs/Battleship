using BattleshipLiteLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipLiteLibrary
{
    public class GameLogic
    {
        public static bool CheckShip(PlayerInfoModel model, string location)
        {
            //Split location into string "A" and int "1" 
            //Add string into GridSpot.SpotLetter
            //Add int into GridSpot.SpotNumber 
            bool isValidSpot = false; 
            GridSpotModel gridSpot = new GridSpotModel();

            char[] spot = location.ToCharArray();

            if(spot.Length == 2)
            {
                string letter = spot[0].ToString();

                bool isValidSpotLetter = ValidateLetter(spot);
                int num = CheckNum(spot);
                bool isValidSpotNumber = ValidateNumber(num);

                bool isValidShipLocation = ValidateShipLocation(model, location);

                if (isValidSpotLetter == true && isValidSpotNumber == true && isValidShipLocation == true)
                {
                    gridSpot.SpotLetter = letter;
                    gridSpot.SpotNumber = num;
                    gridSpot.Status = GridSpotStatus.Ship;
                    model.ShipLocations.Add(gridSpot);
                    isValidSpot = true;
                }
            }
            
            return isValidSpot; 
        }

        private static bool ValidateShipLocation(PlayerInfoModel model, string location)
        {
            bool isValidLocation = true;
            //GridSpotModel gridSpot = new GridSpotModel();

            char[] spot = location.ToCharArray();
            string letter = spot[0].ToString();

            bool isValidSpotLetter = ValidateLetter(spot);
            int num = CheckNum(spot);
            bool isValidSpotNumber = ValidateNumber(num);

            if (isValidSpotLetter == true && isValidSpotNumber == true)
            {
                foreach (GridSpotModel ship in model.ShipLocations)
                {
                    if (ship.SpotLetter == letter.ToUpper() && ship.SpotNumber == num)
                    {
                        isValidLocation = false;
                    }
                } 
            }
            return isValidLocation; 
            
        }

        public static void DetermineShot(GridSpotModel playersShot, PlayerInfoModel opponent, PlayerInfoModel activePlayer)
        {
            bool isAHit = false; 
            foreach(GridSpotModel spot in opponent.ShipLocations)
            {
                if(playersShot.SpotLetter == spot.SpotLetter && playersShot.SpotNumber == spot.SpotNumber)
                {
                    spot.Status = GridSpotStatus.Sunk;
                    isAHit = true; 
                    break; 
                }
                
            }

            foreach(GridSpotModel spot in activePlayer.ShotGrid)
            {
                if(playersShot.SpotLetter == spot.SpotLetter && playersShot.SpotNumber == spot.SpotNumber)
                {
                    if(isAHit == true)
                    {
                        spot.Status = GridSpotStatus.Hit; 
                    }
                    else
                    {
                        spot.Status = GridSpotStatus.Miss; 
                    }
                    break; 
                }
                
            }
        }

        public static bool CheckIfGameContinues(PlayerInfoModel opponent)
        {
            bool doesGameContinue = true; 

            foreach(GridSpotModel grid in opponent.ShipLocations)
            {
                if(grid.Status == GridSpotStatus.Sunk)
                {
                    doesGameContinue = false; 
                }
                else
                {
                    doesGameContinue = true;
                    break; 
                }
            }
            return doesGameContinue; 

        }

        public static int TotalShotsTaken(PlayerInfoModel winner)
        {
            int totalShots = 0; 

            foreach(GridSpotModel gridSpot in winner.ShotGrid)
            {
                if(gridSpot.Status == GridSpotStatus.Hit || gridSpot.Status == GridSpotStatus.Miss)
                {
                    totalShots++; 
                }
            }
            return totalShots; 
        }

        private static bool ValidateNumber(int num)
        {
            List<int> gridNumber = new List<int>
            {
                1,
                2,
                3,
                4,
                5
            };

            bool isValidNum = false; 

            for(int i = 0; i <= gridNumber.Count-1; i++)
            {
                if(num == gridNumber[i])
                {
                    isValidNum = true;
                }
            }

            return isValidNum; 
        }

        public static bool ValidateShot(string shot, GridSpotModel playersShot, PlayerInfoModel player) 
        {
            bool isValidShot = false;
            char[] spot = shot.ToCharArray(); 

            if(spot.Length != 2)//GET RID OF THIS
            {
                throw new ArgumentException("Invalid shot was entered!", "shot"); 
            }
            //IF(SPOT.LENGTH == 2) ==>
            string letter = spot[0].ToString(); 

            bool isValidSpotLetter = ValidateLetter(spot);
            int num = CheckNum(spot);
            bool isValidSpotNumber = ValidateNumber(num); 

            if(isValidSpotLetter == true && isValidSpotNumber == true)
            {
                foreach(GridSpotModel gridSpot in player.ShotGrid)
                {
                    if(gridSpot.SpotLetter == letter && gridSpot.SpotNumber == num)
                    {
                        if(gridSpot.Status == GridSpotStatus.Empty)
                        {
                            playersShot.SpotLetter = letter;
                            playersShot.SpotNumber = num;
                            isValidShot = true;
                            break; 
                        }
                    }
                }
                
            }
            //<==
            return isValidShot; 
        }

        private static int CheckNum(char[] spot)
        {
            string numberText = spot[1].ToString(); 
            int num = 0;
            bool isValidNum = false;

            isValidNum = int.TryParse(numberText, out num);
            return num; 
        } 
        private static bool ValidateLetter(char[] spot)
        {
            string letter = spot[0].ToString(); 
            List<string> gridLetter = new List<string>
            {
                "A",
                "B",
                "C",
                "D",
                "E"
            };
            bool isValidLetter = false; 
            for(int i = 0; i <= gridLetter.Count-1; i++ )
            {
                if (letter == gridLetter[i])
                {
                    isValidLetter = true;
                    return isValidLetter;
                }
                
            }
            return isValidLetter; 
        }

        public static void InitializeShotGrid(PlayerInfoModel model)
        {
            

            List<string> gridLetter = new List<string>
            {
                "A",
                "B",
                "C",
                "D",
                "E"
            };

            List<int> gridNumber = new List<int>
            {
                1,
                2,
                3,
                4,
                5
            }; 

            foreach(string letter in gridLetter)
            {
                foreach(int number in gridNumber)
                {
                    AddGridSpot(model, letter, number); 
                }
            }
        }

        private static void AddGridSpot(PlayerInfoModel model, string letter, int number)
        {
            GridSpotModel gridSpot = new GridSpotModel();
            gridSpot.SpotLetter = letter;
            gridSpot.SpotNumber = number;

            model.ShotGrid.Add(gridSpot); 
        }
    }
}
