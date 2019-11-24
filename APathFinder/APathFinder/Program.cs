using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace APathFinder
{
    class Program
    {
        #region Main
        static void Main(string[] args)
        {
            var inputFile = args[0] + ".cav";
            var outputFile = args[0] + ".csn";
            var solution = "";
            var outputText = "";

            //Converting the input file into an int[]
            int[] inputFileText = Array.ConvertAll(File.ReadAllText(inputFile).Split(','), s => int.Parse(s));

            //Checking if the input text is in the right format (Length == N + N*2 + N*N)
            var isInputValid = IsInputValid(inputFileText, inputFileText.Length);
            Output(isInputValid, outputFile, solution);

            //Checking if the binary matrix provided is correct (all numbers must be either 0 or 1)
            var isMatrixValid = IsMatrixValid(inputFileText);
            Output(isMatrixValid, outputFile, solution);

            //Initialising some values
            var targetCoordsX = TargetCoordsX(inputFileText);
            var targetCoordsY = TargetCoordsY(inputFileText);

            Location current = null;
            var start = new Location { X = inputFileText[1], Y = inputFileText[2] };
            var target = new Location { X = targetCoordsX, Y = targetCoordsY };
            var openList = new List<Location>();
            var closedList = new List<Location>();
            int g = 0;

            //Starting by adding starting coords to open list
            openList.Add(start);

            while (openList.Count > 0)
            {
                //get path with lowest F score
                var lowest = openList.Min(l => l.F);
                current = openList.First(l => l.F == lowest);

                //add the current node to the closed list
                closedList.Add(current);

                //remove it from the open list
                openList.Remove(current);

                //if we added the destination to the closed list, we've found a path
                if (closedList.FirstOrDefault(l => l.X == target.X && l.Y == target.Y) != null)
                {
                    break;
                }

                var walkablePaths = GetWalkablePaths(current.X, current.Y, inputFileText);

                g = g + CalculateGScore(start.X, start.Y, current.X, current.Y);

                foreach (var walkablePath in walkablePaths)
                {
                    //if this walkable path is already in the closed list, ignore it
                    if (closedList.FirstOrDefault(l => l.X == walkablePath.X && l.Y == walkablePath.Y) != null)
                        continue;

                    //if it's not in the open list...
                    if (openList.FirstOrDefault(l => l.X == walkablePath.X && l.Y == walkablePath.Y) == null)
                    {
                        //compute its score, set the parent
                        walkablePath.G = g;
                        walkablePath.H = CalculateHScore(walkablePath.X, walkablePath.Y, target.X, target.Y);
                        walkablePath.F = walkablePath.G + walkablePath.H;
                        walkablePath.Parent = current;

                        //and add it to the open list
                        openList.Insert(0, walkablePath);
                    }
                    else
                    {
                        //else test if using the current G score makes the walkable path's F score
                        //lower, if yes update the parent because it means it's a better path
                        if (g + walkablePath.H < walkablePath.F)
                        {
                            walkablePath.G = g;
                            walkablePath.F = walkablePath.G + walkablePath.H;
                            walkablePath.Parent = current;
                        }
                    }
                }
            }

            //Adds the best path to a outputText
            while (current != null)
            {
                var countcoords = 0;
                for (int l = 1; l < 1 + inputFileText[0] * 2; l = l + 2)
                {
                    countcoords++;
                    if (inputFileText[l] == current.X && inputFileText[l + 1] == current.Y)
                    {
                        outputText = outputText + countcoords + " ";
                        break;
                    }
                }
                current = current.Parent;
            }

            //Reverses the output text
            solution = Reverse(outputText);

            solution = solution.Remove(0, 1);

            //Checks for win
            if (CheckWin(solution, inputFileText) == true)
            {
                Output(1, outputFile, solution);
            }
            else
            {
                Output(2, outputFile, solution);
            }
        }

        #endregion

        #region Input File Validation
        private static int IsInputValid(int[] inputFileText, int inputLength)
        {
            if (inputLength != (1 + (inputFileText[0] * 2) + (inputFileText[0] * inputFileText[0])))
            {
                //Format/length isn't valid
                return 0;
            }
            return 3;
        }
        #endregion

        #region Matrix Validation
        private static int IsMatrixValid(int[] inputFileText)
        {
            var matrixStartIndex = 1 + inputFileText[0] * 2;

            for (int i = matrixStartIndex; i < inputFileText.Length; i++)
            {
                if (!(inputFileText[i] == 0 || inputFileText[i] == 1))
                {
                    //Binary matrix isn't valid
                    return 0;
                }
            }
            return 3;
        }
        #endregion

        #region Calculate Target X Coords
        static int TargetCoordsX(int[] inputFileText)
        {
            var targetXindex = (inputFileText[0] * 2) - 1;

            return inputFileText[targetXindex];
        }
        #endregion

        #region Calculate Target Y Coords
        static int TargetCoordsY(int[] inputFileText)
        {
            var targetYindex = inputFileText[0] * 2;

            return inputFileText[targetYindex];
        }
        #endregion

        #region Calculate Walkable Paths
        static List<Location> GetWalkablePaths(int x, int y, int[] inputFileText)
        {
            var cavernsFinalIndex = inputFileText[0] * 2;
            var count = 0; //To count coord order
            var count1 = 0; //To count binary 
            var proposedLocations = new List<Location>();

            //calculating coord order number (Is this the first coord? second?...) so we can then fetch the binary matrix that tells us which paths are walkable
            for (int i = 1; i < cavernsFinalIndex; i = i + 2)
            {
                count++;
                if (inputFileText[i] == x && inputFileText[i + 1] == y)
                {
                    //fetching the binary matrix order number, so we can then get walkable paths 
                    for (var j = cavernsFinalIndex + count; j < inputFileText.Length; j = j + inputFileText[0])
                    {
                        var count2 = 0; //To be able to fetch coord order again
                        count1++;
                        if (inputFileText[j] == 1)
                        {
                            for (var n = 1; n < cavernsFinalIndex; n = n + 2)
                            {
                                count2++;
                                if (count2 == count1)
                                {
                                    proposedLocations.Add(new Location { X = inputFileText[n], Y = inputFileText[n + 1] });
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return proposedLocations;
        }
        #endregion

        #region Calculate H Score
        static int CalculateHScore(int x, int y, int targetx, int targety)
        {
            var H = Math.Sqrt(Math.Pow(targetx - x, 2) + Math.Pow(targety - y, 2));
            return Convert.ToInt32(H);
        }
        #endregion

        #region Calculate G Score
        static int CalculateGScore(int startx, int starty, int currentx, int currenty)
        {
            var H = Math.Sqrt(Math.Pow(currentx - startx, 2) + Math.Pow(currenty - starty, 2));
            return Convert.ToInt32(H);
        }
        #endregion

        #region Reverse
        public static string Reverse(string s)
        {
            var splits = s.Split(' ');
            Array.Reverse(splits);
            string splits2 = String.Join(" ", splits);
            return splits2;
        }
        #endregion

        #region Found Solution?
        public static bool CheckWin(string outputText, int[] inputFileText)
        {
            string last = outputText.Split(' ').LastOrDefault();

            if(last ==  inputFileText[0].ToString())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Output
        private static void Output(int outputVar, string outputFile, string solution)
        {
            switch (outputVar)
            {
                case 0:
                    //output error message & force program to quit
                    File.WriteAllText(outputFile, "Input file format not valid");
                    Environment.Exit(0);
                    break;
                case 1:
                    //no error = found solution
                    File.WriteAllText(outputFile, solution);
                    break;
                case 2:
                    //no error = found NO solution
                    File.WriteAllText(outputFile, "0");
                    break;
                default:
                    break;
            };
        }
        #endregion
    }
}
