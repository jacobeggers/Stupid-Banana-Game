﻿using OpenTK.Graphics.OpenGL;
using Program;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace opengl_test
{
    public static class GameLogic
    {

        // declare random for some random reason (get it lolz)
        private static Random random = new Random();
        private static int framerate = 1;

        // declare the amount of each game object
        private static int numWalls = 0;
        private static int numPlayers = 0;
        private static int numBananas = 0;

        private static char[] keyStrokes;

        // declare game objects
        private static Player[] players;

        private static Wall[] walls;

        // function to load game
        public static void loadGame(int[][] level)
        {
            // load game takes in values for each object type
            // and sets the number of each game object
            numWalls = 50;
            walls = new Wall[50];

            numPlayers = 1;

            int wallIncrement = 0;
            int loopIncrement = 0;
            for (int i = level.Length - 1; i >= 0; i--)
            {
                for (int j = 0; j < level[i].Length; j++)
                {
                    if (level[i][j] == 1)
                    {
                        walls[wallIncrement] = new Wall(j * 75, loopIncrement * 75);
                        if (i != 0)
                        {
                            if (level[i - 1][j] == 1)
                            {
                                walls[wallIncrement].setNeighboringWalls(true, 'N');
                            }
                        }
                        if (i != level.Length - 1)
                        {
                            if (level[i + 1][j] == 1)
                            {
                                walls[wallIncrement].setNeighboringWalls(true, 'S');
                            }
                        }
                        if (j != 0)
                        {
                            if (level[i][j - 1] == 1)
                            {
                                walls[wallIncrement].setNeighboringWalls(true, 'E');
                            }
                        }
                        if (j != level[i].Length - 1)
                        {
                            if (level[i][j + 1] == 1)
                            {
                                walls[wallIncrement].setNeighboringWalls(true, 'W');
                            }
                        }
                        wallIncrement++;
                    }
                }
                loopIncrement++;
            }


            // create a new wall for each slot in the walls array
            

            // assign a location for the walls in the veiwport

            // create a new player for each slot in the players array
            players = new Player[numPlayers];

            // assign a location for the players in the veiwport
            for (int i = 0; i < numPlayers; i++)
            {
                players[i] = new Player(random.NextInt64(0, 550), 500);
            }
        }

        public static void setKeyStrokes(char[] recievedKeyStrokes)
        {
            keyStrokes = recievedKeyStrokes;
        }

        // primary function that executes all game logic.
        // logic for each type of game object is managed in alternate functions
        public static (float[], float[], float[], float[], string[]) executeGameLogic(int cframerate)
        {
            // declare the variables that will return the crucial
            // values needed to render the objects

            // position coords
            float[] returnX = new float[0];
            float[] returnY = new float[0];

            // width and height of each object
            float[] returnWidth = new float[0];
            float[] returnHeight = new float[0];

            // type of object (used to determine which texture
            // will be used from the tile map)
            string[] objectType = new string[0];

            framerate = cframerate;

            // combine above arrays with arrays returned from the player logic function
            (returnX, returnY, returnWidth, returnHeight, objectType) = combineObjectsIntoArrays(returnX, returnY, returnWidth, returnHeight, objectType, playerLogic(framerate));

            // combine above arrays with arrays returned from the wall logic function
            (returnX, returnY, returnWidth, returnHeight, objectType) = combineObjectsIntoArrays(returnX, returnY, returnWidth, returnHeight, objectType, wallLogic(framerate));

            // return the variables containing important player info
            return (returnX, returnY, returnWidth, returnHeight, objectType);
        }

        // combine the returned values from each game logic function
        // into a set of variables that will be returned to the main loop.
        // widely considered to be the longest function every declared.
        private static (float[], float[], float[], float[], string[]) combineObjectsIntoArrays(float[] x, float[] y, float[] width, float[] height, string[] objectType, (float[], float[], float[], float[], string[]) values)
        {
            // declare the variables with the combined
            // size of the previous variables and variables
            // that are being added
            float[] newX = new float[x.Length + values.Item1.Length];
            float[] newY = new float[y.Length + values.Item2.Length];
            float[] newWidth = new float[width.Length + values.Item3.Length];
            float[] newHeight = new float[height.Length + values.Item4.Length];
            string[] newObjectType = new string[objectType.Length + values.Item5.Length];

            // declare increment variable
            // (used to loop through the total
            // size of the new array)
            int increment = 0;

            // implements values from original arrays
            // into the new arrays
            for (int i = 0; i < x.Length; i++)
            {
                newX[increment] = x[i];
                newY[increment] = y[i];
                newWidth[increment] = width[i];
                newHeight[increment] = height[i];
                newObjectType[increment] = objectType[i];

                increment++;
            }

            // implements the values from the
            // specified game logic function into
            // the new arrays
            for (int i = 0; i < values.Item1.Length; i++)
            {
                newX[increment] = values.Item1[i];
                newY[increment] = values.Item2[i];
                newWidth[increment] = values.Item3[i];
                newHeight[increment] = values.Item4[i];
                newObjectType[increment] = values.Item5[i];

                increment++;
            }

            // return the new arrays to the main game logic function
            return (newX, newY, newWidth, newHeight, newObjectType);
        }

        // game logic for wall objects
        private static (float[], float[], float[], float[], string[]) wallLogic(int framerate)
        {
            // variables to be returned to the array combiner monstrosity
            float[] returnWallX = new float[numWalls];
            float[] returnWallY = new float[numWalls];
            float[] returnWallWidth = new float[numWalls];
            float[] returnWallHeight = new float[numWalls];
            string[] objectType = new string[numWalls];

            // executed game logic for each function
            int increment = 0;
            foreach (Wall wall in walls)
            {
                if (wall != null)
                {
                    returnWallX[increment] = wall.getX();
                    returnWallY[increment] = wall.getY();
                    returnWallWidth[increment] = wall.getWidth();
                    returnWallHeight[increment] = wall.getHeight();
                    objectType[increment] = wall.getStringId();
                    increment++;
                }
            }

            // return values for all wall objects
            return (returnWallX, returnWallY, returnWallWidth, returnWallHeight, objectType);
        }

        // game logic for player objects
        private static (float[], float[], float[], float[], string[]) playerLogic(int framerate)
        {
            // variables to be returned to the array combiner monstrosity
            float[] returnPlayerX = new float[numWalls];
            float[] returnPlayerY = new float[numWalls];
            float[] returnPlayerWidth = new float[numWalls];
            float[] returnPlayerHeight = new float[numWalls];
            string[] objectType = new string[numWalls];

            // executed game logic for each function
            int increment = 0;
            foreach (Player player in players)
            {
                if (keyStrokes[0] == 'A')
                {
                    player.changeX(-750, -1500, deltaTime());
                }
                if (keyStrokes[0] == 'D')
                {
                    player.changeX(750, 1500, deltaTime());
                }
                if (keyStrokes[0] == '\0')
                {
                    if (player.getMomentumX() > 0)
                    {
                        player.changeX(0, -2000, deltaTime());
                    } else if (player.getMomentumX() < 0)
                    {
                        player.changeX(0, 2000, deltaTime());
                    }
                }

                if (keyStrokes[1] == '_') // && player.getCanJump() == true
                {
                    player.setCanJump(false);
                    player.setMomentumY(1500.0f);
                }

                player.changeY(-2000, -4000, deltaTime());

                if (player.getY() < 0)
                {
                    player.setMomentumY(0);
                    player.setY(500);
                }

                

                player.setCanJump(false);
                foreach (Wall wall in walls)
                {
                    if (wall != null)
                    {
                        (float x, float y) = player.getCollisionSide(wall);
                        if (x != 0 && y != 0)
                        {
                            Console.WriteLine(y);
                            player.setY(wall.getY() + wall.getHeight() + 0.1f);
                            player.setMomentumY(0);
                        }
                    }
                }

                // Console.WriteLine(player.getCanJump());

                returnPlayerX[increment] = player.getX();
                returnPlayerY[increment] = player.getY();
                returnPlayerWidth[increment] = player.getWidth();
                returnPlayerHeight[increment] = player.getHeight();
                objectType[increment] = player.getStringId();
            }

            // return values for all player objects
            return (returnPlayerX, returnPlayerY, returnPlayerWidth, returnPlayerHeight, objectType);
        }

        // calculate deltatime using framerate
        private static float deltaTime()
        {
            return 1.0f / framerate;
        }
    }
}