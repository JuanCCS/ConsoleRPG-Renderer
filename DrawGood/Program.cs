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
    /// Game Controller used to handle scenes, and perform global operations.
    /// </summary>
    class Program
    {
        //Classes used in the program.
        static Scene[] myScenes = new Scene[5];
        static ASCIIART mainAsciiArt = new ASCIIART();

        //Desired window size.
        public static int windowWidth = 200;
        public static int windowHeight = 100;

        private static int sceneIndex = 0;

        static void Main(string[] args)
        {
            //This was added so that the user changes console font to 8x8
            while (true)
            {
                if (Console.LargestWindowHeight >= windowHeight && Console.LargestWindowWidth >= windowWidth)
                {
                    //
                    Console.SetWindowSize(windowWidth, windowHeight);
                    break;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Please set the console fonto to 8x8, then press enter.");
                    Console.WriteLine("1. Right click on the top edge of the console window. ");
                    Console.WriteLine("2. Select Properties. ");
                    Console.WriteLine("3. Select the Font Tab. ");
                    Console.WriteLine("4. Set it to 8x8. ");
                    Console.WriteLine("5. Press the OK Button. ");
                    Console.WriteLine("6. Press Enter. ");
                    Console.ReadKey();
                }
            }

            //Console.SetBufferSize(windowWidth, windowHeight + 1);
            //Assign background and enemy sprites to all the scenes.
            myScenes[0] = new Scene(mainAsciiArt.BackGround1(), mainAsciiArt.EnemyGhost(), mainAsciiArt.EnemyGhost(), 0);
            myScenes[1] = new Scene(mainAsciiArt.BackGround2(), mainAsciiArt.EnemyBat(), mainAsciiArt.EnemyWolf(),0);
            myScenes[2] = new Scene(mainAsciiArt.BackGround2(), mainAsciiArt.EnemyGhost(), mainAsciiArt.EnemyDragon(), 60);

            //Start the first Scene.
            myScenes[0].Start();
            myScenes[0].Update();

            
        }

        //Global Function used to Restart the Game. Doesn't work.
        public static void StartFirstScene()
        {
            sceneIndex = 0;
            myScenes[sceneIndex].Start();
            myScenes[sceneIndex].Update();
        }

        //Global Function used to Draw the Next Scene in the Scene Array.
        public static void StartNextScene()
        {
            ++sceneIndex;
            try
            {
                //If there are no more scenes, then the player has won.
                myScenes[sceneIndex].Start();
                myScenes[sceneIndex].Update();
            }
            catch
            {
                //If there are no more scenes you have won the game.
                Console.Clear();
                Console.WriteLine("You win!!");
                Console.ReadLine();
            }

        }

        //Taken from stack overflow, to clamp values.
        public static int Clamp(int value, int min, int max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }
        
    }
}
    

