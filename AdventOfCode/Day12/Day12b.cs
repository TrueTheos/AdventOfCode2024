using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode.Day12
{
    public class Day12b()
    {
        public HashSet<(int, int)> checkedTiles = new();

        char[][] lines;
        char[][] original;

        int lineCount = 0;
        int lineLength = 0;

        public HashSet<(float, float)> cornerPos = new();

        public void Solve(string input)
        {
            lines = input.Split('\n').Select(line => line.TrimEnd().ToCharArray()).ToArray();
            original = lines.Select(row => row.ToArray()).ToArray();
            lineCount = lines.Length;
            lineLength = lines[0].Length; 

            long sum = 0;

            for (int x = 0; x < lineLength; x++)
            {
                for (int y = 0; y < lineCount; y++)
                {
                    char c = lines[y][x];
                    if (c != '#')
                    {
                        cornerPos = new();
                        List<(int x, int y)> bPositions = GetRegion(c, x, y);
                        sum += bPositions.Count * cornerPos.Count;
                    }
                }
            }

           Console.WriteLine(sum.ToString());
        }

        public List<(int x, int y)> GetRegion(char targetChar, int x, int y)
        {
            List<(int x, int y)> filledPositions = new List<(int x, int y)>();
            if (lines[y][x] == targetChar)
                StartSpread(x, y, lineLength, lineCount, targetChar, filledPositions);
            return filledPositions;
        }

        void StartSpread(int x, int y, int width, int height, char targetChar, List<(int x, int y)> filledPositions)
        {
            while (true)
            {
                int ox = x, oy = y;
                while (y != 0 && lines[y - 1][x] == targetChar) y--;
                while (x != 0 && lines[y][x - 1] == targetChar) x--;
                if (x == ox && y == oy) break;
            }
            Spread(x, y, width, height, targetChar, filledPositions);
        }

        void Spread(int x, int y, int width, int height, char targetChar, List<(int x, int y)> filledPositions)
        {
            int lastRowLength = 0;
            do
            {
                int rowLength = 0, sx = x;
                if (lastRowLength != 0 && lines[y][x] != targetChar)
                {
                    do
                    {
                        if (--lastRowLength == 0) return;
                    } while (lines[y][++x] != targetChar);
                    sx = x;
                }
                else
                {
                    for (; x != 0 && lines[y][x - 1] == targetChar; rowLength++, lastRowLength++)
                    {
                        filledPositions.Add((x, y));
                        lines[y][--x] = '#';
                        CountCorners(x, y, targetChar);
                        if (y != 0 && lines[y - 1][x] == targetChar)
                            StartSpread(x, y - 1, width, height, targetChar, filledPositions);
                    }
                }
                for (; sx < width && lines[y][sx] == targetChar; rowLength++, sx++)
                {
                    filledPositions.Add((sx, y));
                    lines[y][sx] = '#';
                    CountCorners(sx, y, targetChar);
                }
                if (rowLength < lastRowLength)
                {
                    for (int end = x + lastRowLength; ++sx < end;)
                    {
                        if (lines[y][sx] == targetChar)
                        {
                            Spread(sx, y, width, height, targetChar, filledPositions);
                        }
                    }
                }
                else if (rowLength > lastRowLength && y != 0)
                {
                    for (int ux = x + lastRowLength; ++ux < sx;)
                    {
                        if (lines[y - 1][ux] == targetChar)
                        {
                            StartSpread(ux, y - 1, width, height, targetChar, filledPositions);
                        }
                    }
                }
                lastRowLength = rowLength;
            } while (lastRowLength != 0 && ++y < height);
        }

        //no = 0
        //yes = 1
        //maybe = 2

        private static readonly (int dx, int dy, int matches)[] Deltas =
        {
            (-1, 0, 0), (0, -1, 0), (-1, -1, 2), // left top
            (1, 0, 0), (1, 1, 2), (0, 1, 0),     // bot right
            (-1, 0, 0), (-1, 1, 2), (0, 1, 0),   // bot left
            (1, 0, 0), (1, -1, 2), (0, -1, 0),   // right top
            (0, 1, 1), (1, 1, 0), (1, 0, 1),     // inside bot right
            (-1, 0, 1), (-1, -1, 0), (0, -1, 1), // inside top left
            (-1, 1, 0), (0, 1, 1), (-1, 0, 1),   // inside bot left
            (1, 0, 1), (0, -1, 1), (1, -1, 0)    // inside top right
        };

        private static readonly (float x, float y)[] DeltaToCorner =
        {
            (-.1f, -.1f), (.1f, .1f), (-.1f, .1f), (.1f, -.1f),
            (.1f, .1f), (-.1f, -.1f), (-.1f, .1f), (.1f, -.1f)
        };

        void CountCorners(int x, int y, char targetChar)
        {
            for (int cornerIndex = 0; cornerIndex < 8; cornerIndex++)
            {
                if (IsCornerMatch(x, y, targetChar, cornerIndex))
                {
                    float newX = x + DeltaToCorner[cornerIndex].x;
                    float newY = y + DeltaToCorner[cornerIndex].y;
                    cornerPos.Add((newX, newY));
                }
            }
        }

        bool IsCornerMatch(int x, int y, char targetChar, int cornerIndex)
        {
            for (int i = 0; i < 3; i++)
            {
                int deltaIndex = cornerIndex * 3 + i;
                int newX = x + Deltas[deltaIndex].dx;
                int newY = y + Deltas[deltaIndex].dy;

                switch (Deltas[deltaIndex].matches)
                {
                    case 1:
                        if (newX < 0 || newY < 0 || newX >= lineLength || newY >= lineCount ||
                            original[newY][newX] != targetChar)
                            return false;
                        break;

                    case 0:
                        if (newX >= 0 && newY >= 0 && newX < lineLength && newY < lineCount &&
                            original[newY][newX] == targetChar)
                            return false;
                        break;

                    case 2:
                        break;
                }
            }
            return true;
        }
    }
}