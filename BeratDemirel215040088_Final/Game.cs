using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeratDemirel_215040088_Final
{
    public class Game
    {
        public int _wrongAnswerCount;
        private NPC? _assassin;
        private bool _riddleAsked;
        private bool _canAskRiddle;

        #region VARIABLES

        #region Game Constants

        private const int DefaultMapWidth = 5;
        private const int DefaultMapHeight = 5;

        #endregion

        #region Game Variables

        #region Player Variables

        public Player Player { get; private set; }

        private string? _playerName;

        #endregion

        private bool _gameRunning;
        private Map? _gameMap;
        private string? _playerInput;

        #endregion

        #endregion

        #region METHODS

        #region Initialization

        public void StartGame()
        {
            
            CreateNewMap();

            
            CreatePlayer();

            InitializeGameConditions();

            _gameRunning = true;
            StartGameLoop();
        }

        private void CreateNewMap()
        {
            _gameMap = new Map(this, DefaultMapWidth, DefaultMapHeight);
        }

        private void CreatePlayer()
        {
            if (_playerName == null)
            {
                GetPlayerName();
            }

            Player = new Player(_playerName);
        }

        private void GetPlayerName()
        {
            Console.WriteLine("Welcome to the most awesome RPG game of all time!");
            Console.WriteLine("Would you be kind enough to provide us with your name?");
            _playerName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(_playerName))
            {
                Console.WriteLine("Player name not entered, giving the name John Doe");
                _playerName = "John Doe";
            }
            else
            {
                Console.WriteLine($"Pleased to meet you {_playerName}, we will have a great adventure together!");
            }
        }

        private void InitializeGameConditions()
        {
            _gameMap.CheckForLocation(_gameMap.GetCoordinates());
        }

        #endregion

        #region Game Loop

        private void StartGameLoop()
        {
            while (_gameRunning)
            {
                Console.WriteLine($"Health: {Player.Health}");
                GetInput();
                ProcessInput();
            }
        }

        private void GetInput()
        {
            _playerInput = Console.ReadLine();
        }

        private void ProcessInput()
        {
            if (string.IsNullOrWhiteSpace(_playerInput))
            {

                Console.WriteLine("Give me a command!");
                return;
            }

            switch (_playerInput.ToUpper())
            {
                case "N":
                    _gameMap.MovePlayer(0, 1);
                    break;
                case "S":
                    _gameMap.MovePlayer(0, -1);
                    break;
                case "E":
                    _gameMap.MovePlayer(1, 0);
                    break;
                case "W":
                    _gameMap.MovePlayer(-1, 0);
                    break;
                case "EXIT":
                    Console.WriteLine("We hope you enjoyed our game!");
                    _gameRunning = false;
                    break;
                case "SAVE":
                    SaveGame();
                    Console.WriteLine("Game saved");
                    break;
                case "LOAD":
                    LoadGame();
                    Console.WriteLine("Game loaded");
                    break;
                case "HELP":
                    Console.WriteLine(HelpMessage());
                    break;
                case "WHERE":
                    _gameMap.CheckForLocation(_gameMap.GetCoordinates());
                    break;
                case "CLEAR":
                    Console.Clear();
                    break;
                case "WHO":
                    Console.WriteLine($"You are {Player.Name}");
                    break;
                case "INVENTORY":
                    Console.WriteLine($"You have {(Player.Inventory.Items.Count > 0 ? String.Join(",", Player.Inventory.Items) : "nothing")}");
                    break;
                default:
                    Console.WriteLine("Command not recognized. Please type 'HELP' for a list of available commands");
                    break;
            }
        }

        #endregion

        #region Save Management

        private void LoadGame()
        {
            string path = SaveFilePath();

            if (!File.Exists(path)) return;

           
            string[] saveContent = File.ReadAllLines(path);

           
            _playerName = saveContent[0];

           
            List<int> coords = saveContent[1].Split(',').Select(int.Parse).ToList();
            Vector2 coordArray = new Vector2(coords[0], coords[1]);

            _gameMap.SetCoordinates(coordArray);
            _wrongAnswerCount = int.Parse(saveContent[2]);
            Player.Health = int.Parse(saveContent[3]);
            if(saveContent.Length > 4)
            {
                var items = saveContent[4].Split(',').ToList();
                foreach (var item in items)
                {
                    Vector2? itemCoord = null;
                    if (item == Item.book.ToString())
                        itemCoord = new Vector2(1, 1);
                    else if (item == Item.compass.ToString())
                        itemCoord = new Vector2(-1, 0);

                    if (itemCoord is not null)
                        _gameMap.TakeItem(Player, itemCoord.Value);
                }
            }
            Console.WriteLine($"You are now standing on {coords[0]},{coords[1]}");
        }

        private void SaveGame()
        {
            
            string xCoord = _gameMap.GetCoordinates().X.ToString();
            string yCoord = _gameMap.GetCoordinates().Y.ToString();
            string playerCoords = $"{xCoord},{yCoord}";
            string items = string.Join(",", Player.Inventory.Items);
            string saveContent = $"{_playerName}{Environment.NewLine}{playerCoords}{Environment.NewLine}{_wrongAnswerCount}{Environment.NewLine}{Player.Health}{Environment.NewLine}{items}";

            string path = SaveFilePath();

            File.WriteAllText(path, saveContent);
        }

        private string SaveFilePath()
        {
            
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            string path = Path.Combine(projectDirectory, "save.txt");

            return path;
        }
        public void AskRiddle()
        {
            var playerEscaped = false;
            if (_wrongAnswerCount == 0)
            {
                Console.WriteLine("If you fail to solve the riddle, the consequences will be severe.");
                Console.WriteLine("Here is your riddle...The more you take, the more you leave behind. What am I?");
                var hasPlayerBook = Player.Inventory.Items.Contains(Item.book);
                Console.WriteLine($"A) Footsteps\nB) Mistakes\nC) Time\nD) {(hasPlayerBook ? "Memories" : "Wind")}\nE) Breath");

                string? firstAnswer = Console.ReadLine();
                if (firstAnswer?.ToUpper() == "D" && hasPlayerBook)
                {
                    Console.WriteLine("Congratulations, you won the mystery game.");
                    playerEscaped = true;
                    _gameRunning = false;
                }
                else
                {
                    HandleWrongAnswer();
                    Console.WriteLine("There is a perfect chance.You can see an opportunity to escape.Would you like to use this opportunity?");
                    Console.WriteLine($"Type 'Y' to escape from the assassin");
                    string? playerInput = Console.ReadLine();
                    if (playerInput?.ToUpper() == "Y")
                    {
                        playerEscaped = true;
                        Console.WriteLine("You successfully escaped from the assassin and continued on your way");
                    }
                }
            }

            if (!playerEscaped)
            {
                Console.WriteLine("Goes everywhere, never gets tired.It goes to everyone's house and doesn't talk.What is this?");
                var hasPlayerCompass = Player.Inventory.Items.Contains(Item.compass);

                Console.WriteLine($"A) {(hasPlayerCompass ? "Road" : "Bridge")}\nB) Dream\nC) Time\nD) Moon\nE) Infinity");

                string? secondAnswer = Console.ReadLine();
                if (secondAnswer?.ToUpper() == "A" && hasPlayerCompass)
                {
                    Console.WriteLine("Congratulations, you won the mystery game.");
                    _gameRunning = false;
                }
                else
                {
                    HandleWrongAnswer();
                }
            }
        }

        private void HandleWrongAnswer()
        {
            Console.WriteLine("WRONG ANSWER!");

            _wrongAnswerCount++;

            if (_wrongAnswerCount >= 2)
            {
                Console.WriteLine("You failed to answer the riddle. Assassins killed you. Game Over.");
                _gameRunning = false;
            }
            else
            {
                Console.WriteLine("You got damaged. It's your last chance. If you don't know the riddle, you'll die.");
                Player.Health -= 50;
            }
        }


        #endregion

        #region Miscellaneous

        private string HelpMessage()
        {
            return @"Here are the current commands:
N: Go north
S: Go south
W: Go west
E: Go east
INVENTORY: View your inventory
WHO: View the player information
WHERE: View current location
CLEAR: Clear the screen"";
LOAD: Load saved game
SAVE: Save current game
EXIT: Exit the game";

        }

        #endregion

        #endregion
    }
}