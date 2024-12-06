using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode.Day6
{
    [Day(6)]
    public class Day6 : AdventDayBase
    {
        enum Dir { Up, Left, Right, Down };

        public override void SolvePart1(string input)
        {
            string[] lines = input.Split('\n').Select(x => x.TrimEnd()).ToArray();
            input = Regex.Replace(input, @"\s+", "");
            int width = lines[0].Length;
            int height = lines.Length;

            Dir currDir = Dir.Up;

            //key y, value x
            Dictionary<int, HashSet<int>> obstaclesY = new();

            //key x, value y
            Dictionary<int, HashSet<int>> obstaclesX = new();
            HashSet<(int y, int x)> visited = new();

            for (int i = input.IndexOf('#'); i > -1; i = input.IndexOf('#', i + 1))
            {
                int row = i / width;
                int col = i % width;

                if (!obstaclesY.ContainsKey(row)) obstaclesY[row] = new HashSet<int>();
                obstaclesY[row].Add(col);

                if (!obstaclesX.ContainsKey(col)) obstaclesX[col] = new HashSet<int>();
                obstaclesX[col].Add(row);
            }

            int y = input.IndexOf('^') / width;
            int x = input.IndexOf('^') % width;

            visited.Add((y, x));

            while (true)
            {
                int nextX = x;
                int nextY = y;
                switch (currDir)
                {
                    case Dir.Up:
                        nextY = obstaclesX.ContainsKey(x)
                            ? obstaclesX[x].Where(obstY => obstY < y).DefaultIfEmpty(-2).Max() + 1
                            : y;
                        nextX = x;
                        break;
                    case Dir.Down:
                        nextY = obstaclesX.ContainsKey(x)
                            ? obstaclesX[x].Where(obstY => obstY > y).DefaultIfEmpty(height).Min() - 1
                            : y;
                        nextX = x;
                        break;
                    case Dir.Left:
                        nextX = obstaclesY.ContainsKey(y)
                            ? obstaclesY[y].Where(obstX => obstX < x).DefaultIfEmpty(-2).Max() + 1
                            : x;
                        nextY = y;
                        break;
                    case Dir.Right:
                        nextX = obstaclesY.ContainsKey(y)
                            ? obstaclesY[y].Where(obstX => obstX > x).DefaultIfEmpty(width).Min() - 1
                            : x;
                        nextY = y;
                        break;
                }

                if (x != nextX)
                {
                    for (int newX = Math.Min(x, nextX) + 1; newX < Math.Max(x, nextX); newX++)
                        visited.Add((y, newX));
                }

                if (y != nextY)
                {
                    for (int newY = Math.Min(y, nextY) + 1; newY < Math.Max(y, nextY); newY++)
                        visited.Add((newY, x));
                }

                visited.Add((nextY, nextX));

                if ((nextX <= 0 && currDir == Dir.Left) || (nextX >= width - 1 && currDir == Dir.Right) ||
                 (nextY <= 0 && currDir == Dir.Up) || (nextY >= height - 1 && currDir == Dir.Down))
                {
                    PrintResult(1, visited.Count.ToString());
                    return;
                }
                else
                {
                    x = nextX;
                    y = nextY;
                    
                    currDir = GetNextDir(currDir);
                }
            }
        }

        private Dir GetNextDir(Dir dir)
        {
            return dir switch
            {
                Dir.Up => Dir.Right,
                Dir.Left => Dir.Up,
                Dir.Right => Dir.Down,
                Dir.Down => Dir.Left,
                _ => Dir.Up,
            };
        }

        public override void SolvePart2(string input)
        {
            //PrintResult(2, sum.ToString());
        }
    }
}