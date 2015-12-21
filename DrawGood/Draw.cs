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
    /// This Class is Used for drawing sprites to the screen. It uses a list of StringBuilders for the Background Layers. This is for optimization.
    /// Sprites are passed to the class as arrays of strings.
    /// </summary>
    class Draw
    {
        //Stores the base,background Layer.
        public List<StringBuilder> BackgroundLayer;
        //All other sprites are rendered after the background layer. And overlap it.
        //Stores the information layer.
        public List<StringBuilder> InfoLayer;

        public Draw()
        {
                //Initialize all the layers that we will use as layers to draw on the screen.
                //We use the StringBuilder class to increase performance.
               BackgroundLayer = new List<StringBuilder>();
               InfoLayer = new List<StringBuilder>();

        }
     
        /// <summary>
        /// Initializes one of the layers with a set of characters. Used to debug.
        /// </summary>
        /// <param name="BufferToInitialize">The public buffer in this class to init</param>
        /// <param name="width">Buffer's width in chars</param>
        /// <param name="height">Buffer's height in chars</param>
        /// <param name="character">The character to write</param>
        public void CreateLayer(
            List<StringBuilder> BufferToInitialize, int width, int height, char character
            )
        {
            //This method should initialize the layers, with a Character.
           

            for (int i = 0; i < height; i++)
            {
                StringBuilder Linea = new StringBuilder();
                BufferToInitialize.Add(Linea);
                for (int j = 0; j < width; j++)
                {
                    BufferToInitialize[i].Append(character);
                }
            }

        }

        /// <summary>
        /// Maps an ASCII sprite to a List of StringBuilders for optimization purposes.
        /// </summary>
        /// <param name="BufferToInitialize">The layer being modified.</param>
        /// <param name="width">The desired width of the layer.</param>
        /// <param name="height">The desired height of the layer.</param>
        /// <param name="sprite">The sprite being added.</param>
        public void CreateLayerWithSprite(
              List<StringBuilder> BufferToInitialize, int width, int height, string[] sprite
            )
        {
            //Map every element of the array to an element of the list.
            for (int i = 0; i < height; i++)
            {
                StringBuilder Line = new StringBuilder();
                BufferToInitialize.Add(Line);
                if (width > sprite[i].Length)
                {
                    BufferToInitialize[i].Append(sprite[i]);
                }
                else
                {
                    BufferToInitialize[i].Append(sprite[i].Substring(0, width));
                }
            }

        }

        /// <summary>
        /// To reset a layer, make it empty again. Not used in our game but might prove useful for other purposes.
        /// </summary>
        /// <param name="BufferToReset"></param>
        public void ResetLayer(List<StringBuilder> BufferToReset)
        {
            for (int i = 0; i < BufferToReset.Count; i++)
            {
                StringBuilder Linea = new StringBuilder();
                BufferToReset.Add(Linea);
                for (int j = 0; j < BufferToReset[i].Length; j++)
                {
                    BufferToReset[i].Append(' ');
                }
            }
        }
            
        /// <summary>
        /// Draws a layer on the screen.
        /// </summary>
        /// <param name="BufferToDraw">The layer we want to draw.</param>
        /// <param name="xPosition">The starting x position from the left.</param>
        /// <param name="yPosition">The starting y position from the top to the bottom.</param>
        /// <param name="charToIgnore">The character we want to ignore, empty space suggested.</param>
        /// <param name="background">The desired color of the background in the console.</param>
        /// <param name="foreground">The desired color of the foreground in the console.</param>
        public void DrawLayer(
            List<StringBuilder> BufferToDraw, int xPosition, int yPosition, char charToIgnore,
            ConsoleColor background, ConsoleColor foreground
            
            )
        {
            //Assign color to background.
            Console.BackgroundColor = background;
            Console.ForegroundColor = foreground;

            //We draw as many rows as the List has elements.
            for(int i = 0; i < BufferToDraw.Count; i++)
            {
                //We draw as many elements as the length of the string.
                for (int j = 0; j < BufferToDraw[i].Length; j++)
                {
                    //Don't print empty characters or any char given as parameter.
                    if (BufferToDraw[i][j] != charToIgnore)
                    {
                        Console.SetCursorPosition(xPosition + j, yPosition + i);
                        Console.Write(BufferToDraw[i][j]);
                    }
                }
            }
            ResetCursor();
        }


        /// <summary>
        /// Draws a sprite as an array of strings on the screen.
        /// </summary>
        /// <param name="BufferToDraw"></param>
        /// <param name="xPosition">The starting x position from the left.</param>
        /// <param name="yPosition">The starting y position from the top to the bottom.</param>
        /// <param name="charToIgnore">The character we want to ignore, empty space suggested.</param>
        /// <param name="background">The desired color of the background in the console.</param>
        /// <param name="foreground">The desired color of the foreground in the console.</param>
        public void DrawSprite(
            string[] sprite, int xPosition, int yPosition, char charToIgnore,
            ConsoleColor background, ConsoleColor foreground
            )
        {
            Console.BackgroundColor = background;
            Console.ForegroundColor = foreground;
            for (int i = 0; i < sprite.Length; i++)
            {
                for (int j = 0; j < sprite[i].Length; j++)
                {
                    if (sprite[i][j] != charToIgnore)
                    {
                        Console.SetCursorPosition(xPosition + j, yPosition + i);
                        Console.Write(sprite[i][j]);
                    }
                }
            }
            ResetCursor();
        }

       /// <summary>
       /// Move a sprite and re-draw the background in the position the sprite was before.
       /// </summary>
       /// <param name="sprite">The sprite to draw.</param>
       /// <param name="prevXPosition">Previous x position from the left.</param>
       /// <param name="prevYPosition">Previous y position from the top.</param>
       /// <param name="newXPosition">New x position from the left.</param>
       /// <param name="newYPosition">New y position from the top.</param>
       /// <param name="charToIngore">The char we want to ignore.</param>
       /// <param name="bgBackGround">The background color of the bg layer.</param>
       /// <param name="bgForeGround">Foreground color of bg layer.</param>
       /// <param name="spriteBackGround">Background color of sprite.</param>
       /// <param name="spriteForeGround">Foreground color of sprite.</param>
        public void MoveSprite(
                string[] sprite, int prevXPosition, int prevYPosition,
            int newXPosition, int newYPosition, char charToIngore, ConsoleColor bgBackGround,
            ConsoleColor bgForeGround, ConsoleColor spriteBackGround, ConsoleColor spriteForeGround
            )
        {
            //Stores the new positions of the sprite.
            List<int[]> newPositions = new List<int[]>();

            //Assign colors to bg.
            Console.BackgroundColor = spriteBackGround;
            Console.ForegroundColor = spriteForeGround;

            //Draw Sprite in New Position
            for (int i = 0; i < sprite.Length; i++)
            {
                for (int j = 0 ; j < sprite[i].Length; j++)
                {
                    if( (char) sprite[i][j] != charToIngore && newXPosition+j <= 170)
                    {
                        //j i then i j.
                        Console.SetCursorPosition(newXPosition + j, newYPosition + i);
                        Console.Write(sprite[i][j]);

                        int[] position = new int[2];
                        position[0] = newXPosition + j;
                        position[1] = newYPosition + i;
                        newPositions.Add(position);
                    }
                }
            }

            ResetCursor();

            //Assign desired sprite colors to console.
            Console.BackgroundColor = bgBackGround;
            Console.ForegroundColor = bgForeGround;

            //Draw BackgroundLayer in Previous Positions
            for (int i = 0; i < sprite.Length; i++)
            {
                for (int j = 0; j < sprite[i].Length; j++)
                {
                    bool isInSprite = false;

                    foreach (int[] cursorPosition in newPositions)
                    {
                        int[] position = new int[2];
                        position[0] = prevXPosition + j;
                        position[1] = prevYPosition + i;
                        if(cursorPosition.SequenceEqual(position))
                        {
                            isInSprite = true;
                        }
                    }
                    if (!isInSprite && prevXPosition + j <= 170)
                    {
                       
                            Console.SetCursorPosition(prevXPosition + j, prevYPosition + i);
                            Console.Write(BackgroundLayer[prevYPosition + i][prevXPosition + j]);
                        
                    }
                }
            }
            ResetCursor();
        }

        /// <summary>
        /// Hides a Sprite that was currently on the scene.
        /// </summary>
        /// <param name="sprite">The sprite to hide.</param>
        /// <param name="prevXPosition"> The position it was in.</param>
        /// <param name="prevYPosition"> The Y position it was in. </param>
        /// <param name="bgBackGround"> Desired background color. </param>
        /// <param name="bgForeGround"> Desired background color. </param>
        public void HideSprite(
                            string[] sprite, int prevXPosition, int prevYPosition,
                            ConsoleColor bgBackGround,
                            ConsoleColor bgForeGround
            )
        {

            Console.BackgroundColor = bgBackGround;
            Console.ForegroundColor = bgForeGround;

            //Draw BackgroundLayer in Previous Position
            for (int i = 0; i < sprite.Length; i++)
            {
                for (int j = 0; j < sprite[i].Length; j++)
                {
                        Console.SetCursorPosition(prevXPosition + j, prevYPosition + i);
                        Console.Write(BackgroundLayer[prevYPosition + i][prevXPosition + j]);
                }
            }
            ResetCursor();
        }


        /// <summary>
        /// This function is used to hide and reset cursor position after each draw.
        /// </summary>
        void ResetCursor()
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);
        }

    }
}
