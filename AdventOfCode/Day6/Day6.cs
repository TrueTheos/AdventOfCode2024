using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public Dictionary<int, HashSet<int>> obstaclesY = new();
        public Dictionary<int, HashSet<int>> obstaclesX = new();

        public int width;
        public int height;

        (int dx, int dy, int limit) GetMovement(Dir dir, int x, int y, int width, int height)
        => dir switch
        {
            Dir.Up => (0, -1, obstaclesX.ContainsKey(x) ? obstaclesX[x].Where(obstY => obstY < y).DefaultIfEmpty(-2).Max() + 1 : y),
            Dir.Down => (0, 1, obstaclesX.ContainsKey(x) ? obstaclesX[x].Where(obstY => obstY > y).DefaultIfEmpty(height).Min() - 1 : y),
            Dir.Left => (-1, 0, obstaclesY.ContainsKey(y) ? obstaclesY[y].Where(obstX => obstX < x).DefaultIfEmpty(-2).Max() + 1 : x),
            Dir.Right => (1, 0, obstaclesY.ContainsKey(y) ? obstaclesY[y].Where(obstX => obstX > x).DefaultIfEmpty(width).Min() - 1 : x),
            _ => throw new ArgumentOutOfRangeException()
        };

        public override void SolvePart1(string input)
        {
            string[] lines = input.Split('\n').Select(x => x.TrimEnd()).ToArray();
            input = Regex.Replace(input, @"\s+", "");
            width = lines[0].Length;
            height = lines.Length;

            Dir currDir = Dir.Up;
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
                var (dx, dy, limit) = GetMovement(currDir, x, y, width, height);

                (nextX, nextY) = dx != 0 ? (limit, y) : (x, limit);

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
            string[] lines = input.Split('\n').Select(x => x.TrimEnd()).ToArray();
            input = Regex.Replace(input, @"\s+", "");
            width = lines[0].Length;
            height = lines.Length;

            Dir currDir = Dir.Up;
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

            HashSet<(int x, int y)> blockades = new();

            var path = GetPath(currDir, x, y);

            foreach (var node in path)
            {
                if (!obstaclesX.ContainsKey(node.x)) obstaclesX[node.x] = new();
                obstaclesX[node.x].Add(node.y);
                if (!obstaclesY.ContainsKey(node.y)) obstaclesY[node.y] = new();
                obstaclesY[node.y].Add(node.x);

                if (!HasExit(currDir, x, y)) blockades.Add((node.x, node.y));

                obstaclesX[node.x].Remove(node.y);
                obstaclesY[node.y].Remove(node.x);
            }

            PrintResult(2, blockades.Count.ToString());
        }

        private HashSet<(Dir dir, int x, int y)> GetPath(Dir currDir, int x, int y)
        {
            HashSet<(Dir dir, int x, int y)> result = new();

            while (true)
            {
                int nextX = x;
                int nextY = y;
                var (dx, dy, limit) = GetMovement(currDir, x, y, width, height);

                (nextX, nextY) = dx != 0 ? (limit, y) : (x, limit);

                if (x != nextX)
                {
                    for (int newX = Math.Min(x, nextX) + 1; newX < Math.Max(x, nextX); newX++)
                        result.Add((currDir, newX, y));
                }

                if (y != nextY)
                {
                    for (int newY = Math.Min(y, nextY) + 1; newY < Math.Max(y, nextY); newY++)
                        result.Add((currDir, x, newY));
                }

                result.Add((currDir, nextX, nextY));

                if ((nextX <= 0 && currDir == Dir.Left) || (nextX >= width - 1 && currDir == Dir.Right) ||
                 (nextY <= 0 && currDir == Dir.Up) || (nextY >= height - 1 && currDir == Dir.Down))
                {
                    return result;
                }
                else
                {
                    x = nextX;
                    y = nextY;

                    currDir = GetNextDir(currDir);
                }
            }
        }

        private bool HasExit(Dir currDir, int x, int y)
        {
            HashSet<(int x, int y, Dir enterDir)> corners = new();
            while (true)
            {
                int nextX = x;
                int nextY = y;
                var (dx, dy, limit) = GetMovement(currDir, x, y, width, height);

                (nextX, nextY) = dx != 0 ? (limit, y) : (x, limit);

                if ((nextX <= 0 && currDir == Dir.Left) || (nextX >= width - 1 && currDir == Dir.Right) ||
                 (nextY <= 0 && currDir == Dir.Up) || (nextY >= height - 1 && currDir == Dir.Down))
                {
                    return true;
                }
                else
                {
                    x = nextX;
                    y = nextY;

                    if (corners.Contains(((x, y, currDir)))) return false;

                    corners.Add((x, y, currDir));

                    currDir = GetNextDir(currDir);           
                }
            }
        }
    }
}