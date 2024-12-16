using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Day15
{
    [Day(15)]
    public class Day15 : AdventDayBase
    {
        public override void SolvePart1(string input)
        {
            string[] lines = input.Trim().Split('\n');

            List<string> gridLines = new List<string>();
            int i = 0;
            while (lines[i].Contains("#"))
            {
                gridLines.Add(lines[i]);
                i++;
            }

            int rows = gridLines.Count;
            int cols = gridLines[0].Length;
            char[,] grid = new char[rows, cols];

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    grid[row, col] = gridLines[row][col];
                }
            }

            List<char> characterList = new List<char>();
            for (; i < lines.Length; i++)
            {
                foreach (char c in lines[i])
                {
                    if (!char.IsWhiteSpace(c))
                    {
                        characterList.Add(c);
                    }
                }
            }
        }

        public override void SolvePart2(string input)
        {
            throw new NotImplementedException();
        }
    }
}
