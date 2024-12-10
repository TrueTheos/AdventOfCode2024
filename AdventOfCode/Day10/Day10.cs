using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode.Day10
{
    [Day(10)]
    public class Day10 : AdventDayBase
    {
        public override void SolvePart1(string input)
        {
            string[] lines = input.Split('\n').Select(x => x.TrimEnd()).ToArray();
            int lineCount = lines.Length;
            int lineLength = lines[0].Length;

            char[,] values = new char[lineLength, lineCount];

            HashSet<(int, int)> trailHeads = new();

            for (int y = 0; y < lineCount; y++)
            {
                for (int x = 0; x < lineLength; x++)
                {
                    values[x, y] = lines[y][x];

                    if (lines[y][x] == '0') trailHeads.Add((x, y));
                }
            }

            int sum = 0;

            foreach(var head in trailHeads)
            {
                int currPath = 0;

                HashSet<(int x, int y)> positions = new() { head };

                while(currPath < 9)
                {
                    var tempPositions = new HashSet<(int x, int y)>();
                    foreach (var pos in positions)
                    {
                        if(pos.x + 1 > 0 && pos.x + 1 < lineLength && (int)(values[pos.x + 1, pos.y] - '0') == currPath + 1)
                        {
                            tempPositions.Add((pos.x + 1, pos.y));
                        }
                        if (pos.y + 1 > 0 && pos.y + 1 < lineCount && (int)(values[pos.x, pos.y + 1] - '0') == currPath + 1)
                        {
                            tempPositions.Add((pos.x, pos.y + 1));
                        }
                        if (pos.x - 1 >= 0 && pos.x - 1 < lineLength && (int)(values[pos.x - 1, pos.y] - '0') == currPath + 1)
                        {
                            tempPositions.Add((pos.x - 1, pos.y));
                        }
                        if (pos.y - 1 >= 0 && pos.y - 1 < lineCount && (int)(values[pos.x, pos.y - 1] - '0') == currPath + 1)
                        {
                            tempPositions.Add((pos.x, pos.y - 1));
                        }
                    }

                    positions = new(tempPositions);

                    currPath++;
                }

                sum += positions.Count;
            }

            PrintResult(1, sum.ToString());
        }

        public override void SolvePart2(string input)
        {
            string[] lines = input.Split('\n').Select(x => x.TrimEnd()).ToArray();
            int lineCount = lines.Length;
            int lineLength = lines[0].Length;

            char[,] values = new char[lineLength, lineCount];

            HashSet<(int, int)> trailHeads = new();

            for (int y = 0; y < lineCount; y++)
            {
                for (int x = 0; x < lineLength; x++)
                {
                    values[x, y] = lines[y][x];

                    if (lines[y][x] == '0') trailHeads.Add((x, y));
                }
            }

            int sum = 0;

            foreach (var head in trailHeads)
            {
                int currPath = 0;

                List<(int x, int y)> positions = new() { head };

                while (currPath < 9)
                {
                    var tempPositions = new List<(int x, int y)>();
                    foreach (var pos in positions)
                    {
                        if (pos.x + 1 > 0 && pos.x + 1 < lineLength && (int)(values[pos.x + 1, pos.y] - '0') == currPath + 1)
                        {
                            tempPositions.Add((pos.x + 1, pos.y));
                        }
                        if (pos.y + 1 > 0 && pos.y + 1 < lineCount && (int)(values[pos.x, pos.y + 1] - '0') == currPath + 1)
                        {
                            tempPositions.Add((pos.x, pos.y + 1));
                        }
                        if (pos.x - 1 >= 0 && pos.x - 1 < lineLength && (int)(values[pos.x - 1, pos.y] - '0') == currPath + 1)
                        {
                            tempPositions.Add((pos.x - 1, pos.y));
                        }
                        if (pos.y - 1 >= 0 && pos.y - 1 < lineCount && (int)(values[pos.x, pos.y - 1] - '0') == currPath + 1)
                        {
                            tempPositions.Add((pos.x, pos.y - 1));
                        }
                    }

                    positions = new(tempPositions);

                    currPath++;
                }

                sum += positions.Count;
            }

            PrintResult(2, sum.ToString());
        }
    }
}
