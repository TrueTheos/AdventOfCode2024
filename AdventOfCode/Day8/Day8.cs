using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Day8
{
    [Day(8)]
    public class Day8 : AdventDayBase
    {
        public override void SolvePart1(string input)
        {
            HashSet<(int x, int y, char c)> antenas = new();
            HashSet<(int x, int y)> antinodes = new();

            string[] lines = input.Split('\n');

            int width = lines[0].TrimEnd().Length;
            int height = lines.Length;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (char.IsLetterOrDigit(lines[y][x]))
                    {
                        antenas.Add((x, y, lines[y][x]));
                    }
                }
            }

            foreach (var antA in antenas)
            {
                foreach (var antB in antenas)
                {
                    if (antA == antB || antA.c != antB.c) continue;
                    int xDiff = antB.x - antA.x;
                    int yDiff = antB.y - antA.y;
                    int newX = antA.x - xDiff;
                    int newY = antA.y - yDiff;

                    if (newX >= 0 && newX < width && newY >= 0 && newY < height)
                    {
                        antinodes.Add((newX, newY));
                    }
                }
            }

            PrintResult(1, antinodes.Count.ToString());
        }

        public override void SolvePart2(string input)
        {
            HashSet<(int x, int y, char c)> antenas = new();
            HashSet<(int x, int y)> antinodes = new();

            string[] lines = input.Split('\n');

            int width = lines[0].TrimEnd().Length;
            int height = lines.Length;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (char.IsLetterOrDigit(lines[y][x]))
                    {
                        antenas.Add((x, y, lines[y][x]));
                    }
                }
            }

            foreach (var antA in antenas)
            {
                foreach (var antB in antenas)
                {
                    if (antA == antB || antA.c != antB.c) continue;
                    int xDiff = antB.x - antA.x;
                    int yDiff = antB.y - antA.y;
                    int newX = antA.x - xDiff;
                    int newY = antA.y - yDiff;

                    antinodes.Add((antB.x, antB.y));

                    int i = 1;
                    while (newX >= 0 && newX < width && newY >= 0 && newY < height)
                    {
                        antinodes.Add((newX, newY));
                        i++;
                        newX = antA.x - xDiff * i;
                        newY = antA.y - yDiff * i;
                    }
                }
            }

            PrintResult(2, antinodes.Count.ToString());
        }
    }
}
