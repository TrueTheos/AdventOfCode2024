using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode.Day12
{
    [Day(12)]
    public class Day12 : AdventDayBase
    {
        public HashSet<(int, int)> checkedTiles = new();

        char[][] lines;
        char[][] original;

        int lineCount = 0;
        int lineLength = 0;

        int perimeter = 0;

        public override void SolvePart1(string input)
        {
            lines = input.Split('\n').Select(line => line.TrimEnd().ToCharArray()).ToArray();
            original = lines.Select(row => row.ToArray()).ToArray();
            lineCount = lines.Length;
            lineLength = lines[0].Length;      //Console.WriteLine($"{c} at ({x}, {y}): {bPositions.Count}");

            long sum = 0;

            for (int x = 0; x < lineLength; x++)
            { 
                for (int y = 0; y < lineCount; y++)
                {
                    char c = lines[y][x];
                    if (c != '#')
                    {
                        perimeter = 0;
                        List<(int x, int y)> bPositions = GetRegion(c, x, y);
                        sum += bPositions.Count * perimeter;
                    }
                }
            }

            PrintResult(1, sum.ToString());
        }

        public List<(int x, int y)> GetRegion(char targetChar, int x, int y)
        {
            List<(int x, int y)> filledPositions = new List<(int x, int y)>();
            perimeter = 0;
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
                        if (y == 0 || original[y - 1][x] != targetChar) perimeter++; // Top
                        if (x == 0 || original[y][x - 1] != targetChar) perimeter++; // Left
                        if (x == width - 1 || original[y][x + 1] != targetChar) perimeter++; // Right
                        if (y == height - 1 || original[y + 1][x] != targetChar) perimeter++; // Bottom
                        if (y != 0 && lines[y - 1][x] == targetChar)
                            StartSpread(x, y - 1, width, height, targetChar, filledPositions);
                    }
                }
                for (; sx < width && lines[y][sx] == targetChar; rowLength++, sx++)
                {
                    filledPositions.Add((sx, y));
                    lines[y][sx] = '#';
                    if (y == 0 || original[y - 1][sx] != targetChar) perimeter++; // Top
                    if (sx == 0 || original[y][sx - 1] != targetChar) perimeter++; // Left
                    if (sx == width - 1 || original[y][sx + 1] != targetChar) perimeter++; // Right
                    if (y == height - 1 || original[y + 1][sx] != targetChar) perimeter++;
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

        void CheckPerimeter(int x, int y, int width, int height, char targetChar, ref int perimeter)
        {
            if (y == 0 || lines[y - 1][x] != targetChar) perimeter++; // Top
            if (y == height - 1 || lines[y + 1][x] != targetChar) perimeter++; // Bottom
            if (x == 0 || lines[y][x - 1] != targetChar) perimeter++; // Left
            if (x == width - 1 || lines[y][x + 1] != targetChar) perimeter++; // Right
        }

        public override void SolvePart2(string input)
        {
            new Day12b().Solve(input);
        }
    }
}
