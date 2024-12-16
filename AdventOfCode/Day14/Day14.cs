using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Day14
{
    [Day(14)]
    public class Day14 : AdventDayBase
    {
        public override void SolvePart1(string input)
        {
            string[] lines = input.Split("\n").Select(x => x.TrimEnd()).ToArray();

            int width = 101;
            int height = 103;

            int seconds = 100;

            int middleX = width / 2;
            int middleY = height / 2;

            int leftTop = 0;
            int rightTop = 0;
            int leftBottom = 0;
            int rightBottom = 0;

            foreach (string line in lines)
            {
                var parts = line.Split(new[] { "p=", "v=", " ", "," }, StringSplitOptions.RemoveEmptyEntries);

                int px = int.Parse(parts[0]);
                int py = int.Parse(parts[1]);

                int vx = int.Parse(parts[2]);
                int vy = int.Parse(parts[3]);

                int finalX = ((px + (seconds * vx)) % width + width) % width;
                int finalY = ((py + (seconds * vy)) % height + height) % height;

                if (finalX == middleX || finalY == middleY) continue;

                if(finalX < middleX)
                {
                    if (finalY < middleY) leftTop++;
                    else leftBottom++;
                }
                else
                {
                    if (finalY < middleY) rightTop++;
                    else rightBottom++;
                }
            }

            PrintResult(1, (leftTop * rightTop * leftBottom * rightBottom).ToString());
        }

        public override void SolvePart2(string input)
        {
            string[] lines = input.Split("\n").Select(x => x.TrimEnd()).ToArray();

            int width = 101;
            int height = 103;

            int seconds = 100;

            int middleX = width / 2;
            int middleY = height / 2;

            int[,] grid = new int[width, height];

            int leftTop = 0;
            int rightTop = 0;
            int leftBottom = 0;
            int rightBottom = 0;

            int currSec = 0;

            while (true)
            {
                grid = new int[width, height];

                foreach (string line in lines)
                {
                    var parts = line.Split(new[] { "p=", "v=", " ", "," }, StringSplitOptions.RemoveEmptyEntries);

                    int px = int.Parse(parts[0]);
                    int py = int.Parse(parts[1]);

                    int vx = int.Parse(parts[2]);
                    int vy = int.Parse(parts[3]);

                    int finalX = ((px + (currSec * vx)) % width + width) % width;
                    int finalY = ((py + (currSec * vy)) % height + height) % height;

                    grid[finalX, finalY] = 1;
                }

                bool valid = false;

                for (int i = 0; i < height; i++)
                {
                    if (valid) break;
                    for (int j = 0; j < width; j++)
                    {
                        if (grid[j, i] == 0) continue;
                        if (valid) break;
                        if(i + 3 < height && j - 3 >= 0 && j + 3 < width)
                        {
                            if (grid[j,i] == 0)
                            {
                                continue;
                            }
                            if (grid[j-1, i + 1] == 0 || grid[j, i + 1] == 0 || grid[j + 1, i + 1] == 0)
                            {
                                continue;
                            }
                            if (grid[j - 2, i + 2] == 0 || grid[j - 1, i + 2] == 0 || grid[j, i + 2] == 0 || grid[j + 1, i + 2] == 0 || grid[j + 2, i + 2] == 0)
                            {
                                continue;
                            }
                            if (grid[j - 3, i + 3] == 0 || grid[j - 2, i + 3] == 0 || grid[j - 1, i + 3] == 0 || grid[j, i + 3] == 0 || grid[j + 1, i + 3] == 0 || grid[j + 2, i + 3] == 0 || grid[j + 3, i + 3] == 0)
                            {
                                continue;
                            }

                            valid = true;
                        }
                    }
                }

                if (valid)
                {
                    for (int i = 0; i < height; i++)
                    {
                        for (int j = 0; j < width; j++)
                        {
                            Console.Write(grid[j, i]);
                        }

                        Console.Write("!\n");
                    }

                    Console.WriteLine(currSec.ToString());
                    Console.ReadKey();
                    Console.Clear();
                }

                currSec++;
            }

            PrintResult(1, (leftTop * rightTop * leftBottom * rightBottom).ToString());
        }
    }
}
