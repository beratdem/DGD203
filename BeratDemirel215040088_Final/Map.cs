using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Numerics;
namespace BeratDemirel_215040088_Final
{
    public class Map
    {

        private Game _theGame;
        private Vector2 _coordinates;
        private int[] _widthBoundaries;
        private int[] _heightBoundaries;
        private Location[] _locations;

        public Map(Game game, int width, int height)
        {
            _theGame = game;

            // Setting the width boundaries
            int widthBoundary = (width - 1) / 2;
            _widthBoundaries = new int[2];
            _widthBoundaries[0] = -widthBoundary;
            _widthBoundaries[1] = widthBoundary;

            // Setting the height boundaries
            int heightBoundary = (height - 1) / 2;
            _heightBoundaries = new int[2];
            _heightBoundaries[0] = -heightBoundary;
            _heightBoundaries[1] = heightBoundary;

            // Setting starting coordinates
            _coordinates = new Vector2(0, 0);

            GenerateLocations();

            // Display result message
            Console.WriteLine($"Created map with size {width}x{height}");
        }

        public Vector2 GetCoordinates()
        {
            return _coordinates;
        }

        public void SetCoordinates(Vector2 newCoordinates)
        {
            _coordinates = newCoordinates;
        }

        public void MovePlayer(int x, int y)
        {
            int newXCoordinate = (int)_coordinates.X + x;
            int newYCoordinate = (int)_coordinates.Y + y;

            if (!CanMoveTo(newXCoordinate, newYCoordinate))
            {
                Console.WriteLine("You can't go that way");
                return;
            }

            _coordinates.X = newXCoordinate;
            _coordinates.Y = newYCoordinate;

            CheckForLocation(_coordinates);


        }


        private bool CanMoveTo(int x, int y)
        {
            return !(x < _widthBoundaries[0] || x > _widthBoundaries[1] || y < _heightBoundaries[0] || y > _heightBoundaries[1]);
        }

        private void GenerateLocations()
        {
            _locations = new Location[5];

            Vector2 firstLocation = new Vector2(0, 0);
            Location firstCity = new Location("Serenity Falls", LocationType.Village, firstLocation);
            _locations[0] = firstCity;

            Vector2 whisperingValeLocation = new Vector2(1, 1);
            List<Item> whisperingValeItems = new List<Item>();
            whisperingValeItems.Add(Item.book);
            Location whisperingVale = new Location("Whispering Vale", LocationType.Village, whisperingValeLocation, whisperingValeItems);
            _locations[1] = whisperingVale;

            Vector2 mysteriousCaveLocation = new Vector2(-1, 0);
            List<Item> mysteriousCaveItems = new List<Item>();
            mysteriousCaveItems.Add(Item.compass);
            Location mysteriousCave = new Location("Mysterious Cave", LocationType.MysteriousTemple, mysteriousCaveLocation, mysteriousCaveItems);
            _locations[2] = mysteriousCave;

            Vector2 firstEncounterLocation = new Vector2(1, 2);
            Location firstEncounter = new Location("First Encounter", LocationType.Npc, firstEncounterLocation);
            _locations[3] = firstEncounter;

            Vector2 secondEncounterLocation = new Vector2(-2, -1);
            Location secondEncounter = new Location("Second Encounter", LocationType.Npc, secondEncounterLocation);
            _locations[4] = secondEncounter;
        }

        public void CheckForLocation(Vector2 coordinates)
        {
            Console.WriteLine($"You are now standing on {coordinates.X},{coordinates.Y}");

            if (IsOnLocation(coordinates, out Location location))
            {
                if (location.Type == LocationType.Npc)
                {
                    if (_theGame._wrongAnswerCount == 0)
                    {
                        Console.WriteLine("Suddenly, you are caught by an assassin, but the assassin takes pity on you, he offers to ask you 2 questions instead of killing you. You accept this offer.");
                        Console.WriteLine("If you answer one of these questions correctly, the assassin will let you go");
                    }
                    else
                        Console.WriteLine("The assassin caught you again, you have to answer the question. This time there is no escape.");

                    _theGame.AskRiddle();
                }
                else
                {
                    Console.WriteLine($"You are in {location.Name}");

                    if (HasItem(location))
                    {
                        Console.WriteLine($"There is a {location.ItemsOnLocation[0]} here");
                        Console.WriteLine($"Type 'E' to pick up the {location.ItemsOnLocation[0]}.");
                        string? userInput = Console.ReadLine();

                        if (userInput?.ToUpper() == "E")
                        {
                            TakeItem(_theGame.Player, location.Coordinates);
                        }
                    }
                }
            }
        }

        private bool IsOnLocation(Vector2 coords, out Location foundLocation)
        {
            for (int i = 0; i < _locations.Length; i++)
            {
                if (_locations[i].Coordinates.X == coords.X && _locations[i].Coordinates.Y == coords.Y)
                {
                    foundLocation = _locations[i];
                    return true;
                }
            }

            foundLocation = null;
            return false;
        }

        private bool HasItem(Location location)
        {
            return location.ItemsOnLocation.Count != 0;
        }

        public void TakeItem(Player player, Vector2 coordinates)
        {
            if (IsOnLocation(coordinates, out Location location))
            {
                if (HasItem(location))
                {
                    Item itemOnLocation = location.ItemsOnLocation[0];

                    player.TakeItem(itemOnLocation);
                    location.RemoveItem(itemOnLocation);

                    switch (itemOnLocation)
                    {
                        case Item.compass:
                            Console.WriteLine("You examined the compass, you seem to have lost your way, maybe this compass could be useful.");
                            break;
                        case Item.book:
                            Console.WriteLine("You looked at the book and it reminded you of your past");
                            break;
                        default:
                            break;
                    }
                    return;
                }
            }

            Console.WriteLine("There is nothing to take here!");
        }

        public void RemoveItemFromLocation(Item item)
        {
            for (int i = 0; i < _locations.Length; i++)
            {
                if (_locations[i].ItemsOnLocation.Contains(item))
                {
                    _locations[i].RemoveItem(item);
                }
            }
        }


    }
}
