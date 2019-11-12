using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APathFinder
{
    class Program
    {
        #region Main
        static void Main(string[] args)
        {
            var inputFile = args[0];
            var outputFile = args[1];
            var solution = "0";

            //Converting the input file into an int[]
            int[] inputFileText = Array.ConvertAll(File.ReadAllText(inputFile).Split(','), s => int.Parse(s));

            //Checking if the input text is in the right format (Length == N + N*2 + N*N)
            var isInputValid = IsInputValid(inputFileText, inputFileText.Length);
            Output(isInputValid, outputFile, solution);

            //Checking if the binary matrix provided is correct (all numbers must be either 0 or 1)
            var isMatrixValid = IsMatrixValid(inputFileText);
            Output(isMatrixValid, outputFile, solution);
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
            var matrixStartIndex = 1 + inputFileText[0]*2;

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
                    File.WriteAllText(outputFile, solution);
                    break;
                default:
                    break;
            };
        }
        #endregion
    }
}
