using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Day4 : AdventDayBase
    {
        public override void SolvePart1(string input)
        {
            string[] lines = input.Split('\n').Select(x => x.TrimEnd()).ToArray();
            input = Regex.Replace(input, "[^A-Za-z0-9 -]", "");
            int lineCount = lines.Length;
            int lineLength = lines[0].Length;

            int sum = 0;

            //XMAS

            for (int i = input.IndexOf('X'); i > -1; i = input.IndexOf('X', i + 1))
            {
                int row = i / lineLength;
                int col = i % lineLength;

                if(lineLength - col >= 4) //normal
                {
                    if (lines[row].Substring(col, 4) == "XMAS") sum++;
                }
                if(col >= 3) //backwards
                {
                    if (lines[row].Substring(col - 3, 4) == "SAMX") sum++;
                }
                if(row >= 3) // up
                {
                    if (lines[row - 1][col] == 'M' && lines[row - 2][col] == 'A' && lines[row - 3][col] == 'S') sum++;
                    if (col >= 3) //up left
                    {
                        if (lines[row - 1][col - 1] == 'M' && lines[row - 2][col - 2] == 'A' && lines[row - 3][col - 3] == 'S') sum++;
                    }
                    if (lineLength - col >= 4) //up right
                    {
                        if (lines[row - 1][col + 1] == 'M' && lines[row - 2][col + 2] == 'A' && lines[row - 3][col + 3] == 'S') sum++;
                    }
                }
                if(lineCount - row >= 4) //down
                {
                    if (lines[row + 1][col] == 'M' && lines[row + 2][col] == 'A' && lines[row + 3][col] == 'S') sum++;
                    if(lineLength - col >= 4) //down right
                    {
                        if (lines[row + 1][col + 1] == 'M' && lines[row + 2][col + 2] == 'A' && lines[row + 3][col + 3] == 'S') sum++;
                    }
                    if (col >= 3) //down left
                    {
                        if (lines[row + 1][col - 1] == 'M' && lines[row + 2][col - 2] == 'A' && lines[row + 3][col - 3] == 'S') sum++;
                    }
                }
            }

            PrintResult(1, sum.ToString());
        }

        public override void SolvePart2(string input)
        {
            string[] lines = input.Split('\n').Select(x => x.TrimEnd()).ToArray();
            input = Regex.Replace(input, "[^A-Za-z0-9 -]", "");
            int lineCount = lines.Length;
            int lineLength = lines[0].Length;

            int sum = 0;

            //X-MAS

            for (int i = input.IndexOf('A'); i > -1; i = input.IndexOf('A', i + 1))
            {
                int row = i / lineLength;
                int col = i % lineLength;

                if (col < lineLength - 1&& col > 0 && row < lineCount - 1 && row > 0)
                {
                    char NE = lines[row - 1][col + 1];
                    char SE = lines[row + 1][col + 1];
                    char NW = lines[row - 1][col - 1];
                    char SW = lines[row + 1][col - 1];

                    if (NE == 'M' && SE == 'M' && NW == 'S' && SW == 'S') sum++;
                    if (NE == 'S' && SE == 'M' && NW == 'S' && SW == 'M') sum++;
                    if (NE == 'S' && SE == 'S' && NW == 'M' && SW == 'M') sum++;
                    if (NE == 'M' && SE == 'S' && NW == 'M' && SW == 'S') sum++;
                }
            }

            PrintResult(2, sum.ToString());
        }
    }
}
