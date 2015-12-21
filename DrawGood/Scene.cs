/* 
 * CONSOLE RENDERING ENGINE
 * 
 * 
 * CREATED BY:
 * 
 * Manuel Cerberos
 * Carlos Restrepo
 * Juan Saenz
 * Ulises Zamora
 * 
 * VFS
 * 
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawGood
{

    /// <summary>
    /// This is the Scene class. Every scene contains a background and other sprites.
    /// </summary>
    class Scene
    {
        //Background of current scene.
        private List<StringBuilder> CurrentBackground;

        //An instance of the draw engine.
        private Draw myDrawEngine = new Draw();
        //An instance of the ascii art class to store the drawings.
        private ASCIIART myAsciiArt = new ASCIIART();

        //Sprite for the enemies.
        private string[] currentEnemyUp;
        private string[] currentEnemyDown;

        //Sprites for Background.
        private string[] currentBGSprite;

        //Capture the user's input.
        private ConsoleKey key;

        //Initial position of players and enemies.
        private int xPosPlayer = 10;
        private int xPosEnemy = 120;
        private int xPosWolf = 120;
        private int yPosEnemy = 10;

        //States of the characters.
        private bool isEnemyUpDead = false;
        private bool isEnemyDownDead = false;
        private bool isDragon = false;
        private bool playerLost = false;

        /// <summary>
        /// Constructor for the Scene object, takes a background, and two enemy sprites as parameters.
        /// </summary>
        /// <param name="backgroundSprite">The Background Ascii art</param>
        /// <param name="enemyUp">Flying Enemy Ascii Art</param>
        /// <param name="enemyDown">Ground Enemy Ascii Art</param>
        public Scene( string[] backgroundSprite, string[] enemyUp, string[] enemyDown, int downEnemyX )
        {
            //Store the scene's background.
            CurrentBackground = new List<StringBuilder>();

            //Store the sprite for the current enemy in the up position.
            currentEnemyUp = enemyUp;
            currentEnemyDown = enemyDown;

            currentBGSprite = backgroundSprite;
            
            xPosWolf -= downEnemyX;
            
            //If there's a dragon we won't move the down enemy.
            if (downEnemyX > 0)
            {
                isDragon = true;
            }

            //If we pass an empty sprite then it's a dead character.
            if (enemyUp.Length < 5)
            {
                isEnemyUpDead = true;
            }

            //If we pass an empty sprite then it's a dead character.
            if (enemyDown.Length < 5)
            {
                isEnemyDownDead = true;
            }

        }

        public void Start()
        {

            //Initialize the two layers, background and info.
            myDrawEngine.CreateLayerWithSprite(myDrawEngine.BackgroundLayer, 200, currentBGSprite.Length, currentBGSprite);
            myDrawEngine.CreateLayerWithSprite(myDrawEngine.InfoLayer, 200, myAsciiArt.Stats().Length, myAsciiArt.Stats());

            //Draw layers and then sprites, order is important when drawing.
            myDrawEngine.DrawLayer(myDrawEngine.BackgroundLayer, 0, 0, 'a', ConsoleColor.DarkGreen, ConsoleColor.Green);
            myDrawEngine.DrawLayer(myDrawEngine.InfoLayer, 176, 2, '.', ConsoleColor.Black, ConsoleColor.White);
            myDrawEngine.DrawSprite(myAsciiArt.Player(), xPosPlayer, Program.windowHeight - myAsciiArt.Player().Length, ' ', ConsoleColor.DarkBlue, ConsoleColor.Cyan);
            myDrawEngine.DrawSprite(currentEnemyUp, xPosEnemy, yPosEnemy, ' ', ConsoleColor.DarkRed, ConsoleColor.DarkYellow);
            myDrawEngine.DrawSprite(currentEnemyDown, xPosWolf, Program.windowHeight - currentEnemyDown.Length, ' ', ConsoleColor.DarkRed, ConsoleColor.DarkYellow);
            
        }


        
        //The update functoion loops infinitely until the player exits the current screen.
        //It waits for user input to execute.
        public void Update()
        {
      
            int speed = 10;
            int enemySpeed = -5;
            int enemyYSpeed = 1;
           

            while (true)
            {

                key = Console.ReadKey().Key;

                //Move to the right.
                if (key == ConsoleKey.RightArrow)
                {
                    HandleMoveInput(speed, enemySpeed, enemyYSpeed);
                }

                //Move to the left
                if (key == ConsoleKey.LeftArrow)
                {
                    HandleMoveInput(-speed, enemySpeed, enemyYSpeed);
                }

                //Press 'S' to attack the enemy on the floor.
                if (key == ConsoleKey.S)
                {
                    if (!isEnemyDownDead)
                    {
                        myDrawEngine.HideSprite(currentEnemyDown, xPosWolf, Program.windowHeight - currentEnemyDown.Length, ConsoleColor.DarkGreen, ConsoleColor.Green);
                        //myDrawEngine.DrawSprite(myAsciiArt.ReturnFlippedSprite(currentEnemyDown), xPosWolf, Program.windowHeight - currentEnemyDown.Length,' ' , ConsoleColor.DarkRed, ConsoleColor.DarkYellow);
                        isEnemyDownDead = true;
                    }
                }

                // Press 'w' to attack the enemy that is flying.
                if (key == ConsoleKey.W)
                {
                    if (!isEnemyUpDead)
                    {
                        myDrawEngine.HideSprite(currentEnemyUp, xPosEnemy, yPosEnemy, ConsoleColor.DarkGreen, ConsoleColor.Green);
                        isEnemyUpDead = true;
                    }
                }

                // If the player has passed through the door, break the update loop.
                if (xPosPlayer >= 170)
                {
                    Console.Clear();
                    break;
                }

                //Check if the player has lost the game.
                CheckForDeath();

                //Move cursor back to upper left corner.
                Console.CursorVisible = false;
                Console.SetCursorPosition(0, 0);

            }
            
            //Start next scene
            StartNextScene();

        }

        //Make the program initialize the next screen.
        private void StartNextScene()
        {
            Program.StartNextScene();
        }



        //Used to handle user Movement input in the Scene.
        private void HandleMoveInput(int speed, int enemySpeed, int enemyYSpeed)
        {
            //You can only move after you kill the enemies.
            if (isEnemyUpDead && isEnemyDownDead)
            {
                //Move The player to the right.
                myDrawEngine.MoveSprite(myAsciiArt.Player(), xPosPlayer, Program.windowHeight - myAsciiArt.Player().Length, Program.Clamp(xPosPlayer + speed, 0, 170), Program.windowHeight - myAsciiArt.Player().Length, ' ',
                    ConsoleColor.DarkGreen, ConsoleColor.Green, ConsoleColor.DarkBlue, ConsoleColor.Cyan
                    );

                xPosPlayer = Program.Clamp(xPosPlayer + speed, 0, 170);
            }

            if (!isEnemyUpDead)
            {
                //Move the enemy that flies, if it's not dead.
                myDrawEngine.MoveSprite(currentEnemyUp, xPosEnemy, yPosEnemy, Program.Clamp(xPosEnemy + enemySpeed, 0, 170), 
                    Program.Clamp(yPosEnemy + enemyYSpeed, 10, Program.windowHeight-currentEnemyUp.Length), ' ',
                    ConsoleColor.DarkGreen, ConsoleColor.Green, ConsoleColor.DarkRed, ConsoleColor.DarkYellow
                    );

                //The x position of the enemy must be updated when they move.
                xPosEnemy = Program.Clamp(xPosEnemy + enemySpeed, 0, 170);
                yPosEnemy = Program.Clamp(yPosEnemy + enemyYSpeed, 10, Program.windowHeight - currentEnemyUp.Length);
            }

            if (!isEnemyDownDead && !isDragon)
            {
                //Move the enemy on the floor to the left if its not dead.
                myDrawEngine.MoveSprite(currentEnemyDown, xPosWolf, Program.windowHeight - currentEnemyDown.Length, Program.Clamp(xPosWolf + enemySpeed, 0, 170), Program.windowHeight - currentEnemyDown.Length, ' ',
                   ConsoleColor.DarkGreen, ConsoleColor.Green, ConsoleColor.DarkRed, ConsoleColor.DarkYellow
                   );

                xPosWolf = Program.Clamp(xPosWolf + enemySpeed, 0, 170);
            }
        }

        /// <summary>
        /// Check if the player is dead using rudimentary collision detection.
        /// </summary>
        private void CheckForDeath()
        {
            if (xPosEnemy < xPosPlayer + 30 || xPosWolf < xPosPlayer + 30)
            {
                if (!isEnemyUpDead && !isEnemyDownDead)
                {
                    playerLost = true;
                    Console.Clear();
                    Console.WriteLine("You lose!");
                    Console.ReadLine();

                    //Close the program if the player loses.
                    Environment.Exit(0);
                }
            }
        }


    }
    
}

