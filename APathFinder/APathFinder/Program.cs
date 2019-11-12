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
        static void Main(string[] args)
        {
            var inputFile = args[0];
            var outputFile = args[1];

            //Converting the input file into an int[]
            int[] inputFileText = Array.ConvertAll(File.ReadAllText(inputFile).Split(','), s => int.Parse(s));

            //Checking if the input text is in the right format (Length == N + N*2 + N*N)
            IsInputValid(inputFileText, inputFileText.Length);

            //Checking if the binary matrix provided is correct (all numbers must be either 0 or 1)
            IsMatrixValid(inputFileText);
        }

        private static void IsInputValid(int[] inputFileText, int inputLength)
        {
            if (inputLength != (1 + (inputFileText[0] * 2) + (inputFileText[0] * inputFileText[0])))
            {
                //Format/length isn't valid
                int outputVar = 0;

                Output(outputVar);
            }
        }

        private static void IsMatrixValid(int[] inputFileText)
        {
            var matrixStartIndex = 1 + inputFileText[0]*2;

            for (int i = matrixStartIndex; i < inputFileText.Length; i++)
            {
                if (!(inputFileText[i] == 0 || inputFileText[i] == 1))
                {
                    //Binary matrix isn't valid
                    int outputVar = 0;

                    Output(outputVar);
                }
            }
        }

        private static void Output(int outputVar)
        {
            switch (outputVar)
            {
                case 1:
                    //no error = found solution
                    break;
                case 2:
                    //no error = found NO solution
                    break;
                default:
                    //output error message
                    //force program to stop
                    Console.WriteLine("nop");
                    Environment.Exit(0);
                    break;
            };
        }
    }
}
