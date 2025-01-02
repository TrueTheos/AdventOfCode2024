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
            string[] lines = input.Split("\n").Select(x => x.TrimEnd()).ToArray();

            List<string> gridLines = new List<string>();
            int j = 0;
            while (lines[j].Contains("#"))
            {
                gridLines.Add(lines[j]);
                j++;
            }

            int rows = gridLines.Count;
            int cols = gridLines[0].Length;
            char[,] grid = new char[cols, rows];

            int currX = 0;
            int currY = 0;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    grid[col, row] = gridLines[row][col];

                    if(grid[col, row] == '@')
                    {
                        currX = col;
                        currY = row;
                    }
                }
            }

            List<char> characterList = new List<char>();
            for (; j < lines.Length; j++)
            {
                foreach (char c in lines[j])
                {
                    if (!char.IsWhiteSpace(c))
                    {
                        characterList.Add(c);
                    }
                }
            }

            foreach (char move in characterList)
            {
                (int x, int y) dir = (0, 0);
                switch (move)
                {
                    case '^':
                        dir = (0, -1); break;
                    case 'v':
                        dir = (0, 1); break;
                    case '>':
                        dir = (1, 0); break;
                    case '<':
                        dir = (-1, 0); break;
                }

                if (grid[currX + dir.x, currY + dir.y] == '#') continue;
                else if (grid[currX + dir.x, currY + dir.y] == '.')
                {
                    grid[currX, currY] = '.';
                    currX += dir.x;
                    currY += dir.y;
                    grid[currX, currY] = '@';
                }
                else if (grid[currX + dir.x, currY + dir.y] == 'O')
                {
                    int boxCount = 0;
                    int checkX = currX + dir.x;
                    int checkY = currY + dir.y;

                    while (grid[checkX, checkY] == 'O')
                    {
                        boxCount++;
                        checkX += dir.x;
                        checkY += dir.y;
                    }

                    if (grid[checkX, checkY] == '#')
                        continue;

                    if (grid[checkX, checkY] == '.')
                    {
                        while (boxCount > 0)
                        {
                            grid[checkX, checkY] = 'O';
                            checkX -= dir.x;
                            checkY -= dir.y;
                            boxCount--;
                        }
                        grid[currX + dir.x, currY + dir.y] = '@';
                        grid[currX, currY] = '.';
                        currX += dir.x;
                        currY += dir.y;
                    }
                }
            }

            long sum = 0;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    Console.Write(grid[col, row]);
                    if (grid[col, row] == 'O')
                    {
                        sum += row * 100 + col;
                    }
                }

                Console.Write('\n');
            }

            PrintResult(1, sum.ToString());
        }

        public override void SolvePart2(string input)
        {
            string[] lines = input.Split("\n").Select(x => x.TrimEnd()).ToArray();

            List<string> gridLines = new List<string>();
            int j = 0;
            while (lines[j].Contains("#"))
            {
                gridLines.Add(lines[j]);
                j++;
            }

            int rows = gridLines.Count;
            int cols = gridLines[0].Length;
            char[,] grid = new char[cols * 2, rows];

            int currX = 0;
            int currY = 0;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    if(gridLines[row][col] == '@')
                    {
                        grid[col * 2, row] = gridLines[row][col];
                        grid[col * 2 + 1, row] = '.';
                        currX = col * 2;
                        currY = row;
                        continue;
                    }

                    if (col == 0)
                    {
                        if (gridLines[row][col] == 'O')
                        {
                            grid[0, row] = '[';
                            grid[1, row] = ']';
                        }
                        else
                        {
                            grid[0, row] = gridLines[row][col];
                            grid[1, row] = gridLines[row][col];
                        }
                    }
                    else
                    {
                        if (gridLines[row][col] == 'O')
                        {
                            grid[col * 2, row] = '[';
                            grid[col * 2 + 1, row] = ']';
                        }
                        else
                        {
                            grid[col * 2, row] = gridLines[row][col];
                            grid[col * 2 + 1, row] = gridLines[row][col];
                        }
                    }
                }
            }

            List<char> characterList = new List<char>();
            for (; j < lines.Length; j++)
            {
                foreach (char c in lines[j])
                {
                    if (!char.IsWhiteSpace(c))
                    {
                        characterList.Add(c);
                    }
                }
            }

            bool CanMove(int x, int y, (int x, int y) dir)
            {
                if(x < 0 || x >= grid.GetLength(0) || y < 0 || y >= grid.GetLength(1)) return false;
                if (grid[x, y] == '#') return false;
                else if (grid[x, y] == '.') return true;
                if(dir.y != 0)
                {
                    if (x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1))
                    {
                        if (grid[x, y] == '[')
                        {
                            if (y + dir.y >= 0 && y + dir.y < grid.GetLength(1))
                            {
                                char a = grid[x, y + dir.y];
                                char b = grid[x + 1, y + dir.y];
                                if (a == '.' && b == '.')
                                {
                                    return true;
                                }
                                else if (a == '#' || b == '#')
                                {
                                    return false;
                                }
                                else
                                {
                                    if ((a == '[' || a == ']') && !CanMove(x, y + dir.y, dir))
                                    {
                                        return false;
                                    }
                                    if ((b == '[' || b == ']') && !CanMove(x + 1, y + dir.y, dir))
                                    {
                                        return false;
                                    }
                                }

                                return true;
                            }
                            return false;
                        }
                        else if (grid[x, y] == ']')
                        {
                            if (y + dir.y >= 0 && y + dir.y < grid.GetLength(1))
                            {
                                char a = grid[x, y + dir.y];
                                char b = grid[x - 1, y + dir.y];
                                if (a == '.' && b == '.')
                                {
                                    return true;
                                }
                                else if (a == '#' || b == '#')
                                {
                                    return false;
                                }
                                else
                                {
                                    if ((a == '[' || a == ']') && !CanMove(x, y + dir.y, dir))
                                    { 
                                        return false; 
                                    }
                                    if ((b == '[' || b == ']') && !CanMove(x - 1, y + dir.y, dir))
                                    {
                                        return false;
                                    }
                                }

                                return true;
                            }
                            return false;
                        }
                    }
                }
                if(dir.x != 0)
                {
                    if(dir.x == -1)
                    {
                        if (x - 2 >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1))
                        {
                            if (grid[x - 2, y] == ']')
                            {
                                return CanMove(x - 2, y, dir);
                            }
                            else if (grid[x - 2, y] == '#') return false;
                            else return true;
                        }
                        return false;
                    }
                    else
                    {
                        if (x + 2 < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1))
                        {
                            if (grid[x + 2, y] == '[')
                            {
                                return CanMove(x + 2, y, dir);
                            }
                            else if (grid[x + 2, y] == '#') return false;
                            else return true;
                        }
                        return false;
                    }
                }

                return true;
            }

            void MoveBox(int x, int y, (int x, int y) dir)
            {
                if (x + dir.x >= 0 && x + dir.x < grid.GetLength(0) &&
                    y + dir.y >= 0 && y + dir.y < grid.GetLength(1))
                {
                }
                else
                {
                    return;
                }

                if (dir.x == -1 && x - 2 >= 0)
                {
                    if (grid[x - 2, y] == ']') MoveBox(x - 2, y, dir);
                    grid[x, y] = '.';
                    grid[x - 1, y] = ']';
                    grid[x - 2, y] = '[';
                }
                else if (dir.x == 1 && x + 2 < grid.GetLength(0))
                {
                    if (grid[x + 2, y] == '[') MoveBox(x + 2, y, dir);
                    grid[x, y] = '.';
                    grid[x + 1, y] = '[';
                    grid[x + 2, y] = ']';
                }
                else if (dir.y == -1 && y - 1 >= 0)
                {
                    if (grid[x,y] == '[')
                    {
                        char a = grid[x, y - 1];
                        char b = grid[x + 1, y - 1];
                        if (a == ']' || (a == '[' && b == ']')) MoveBox(x, y - 1, dir);
                        if((a == ']' || a == '.') && b == '[') MoveBox(x + 1, y - 1, dir);
                    }
                    else if (grid[x, y] == ']')
                    {
                        char a = grid[x, y - 1];
                        char b = grid[x - 1, y - 1];
                        if (a == '[' || (a == ']' && b == '[')) MoveBox(x, y - 1, dir);
                        if ((a == '[' || a == '.') && b == ']') MoveBox(x - 1, y - 1, dir);
                    }

                    if (grid[x, y] == '[')
                    {
                        grid[x, y] = '.';
                        grid[x + 1, y] = '.';
                        grid[x, y - 1] = '[';
                        grid[x + 1, y - 1] = ']';
                    }
                    else if (grid[x, y] == ']')
                    {
                        grid[x, y] = '.';
                        grid[x - 1, y] = '.';
                        grid[x, y - 1] = ']';
                        grid[x - 1, y - 1] = '[';
                    }
                }
                else if (dir.y == 1 && y + 1 < grid.GetLength(1))
                {
                    if (grid[x, y] == '[')
                    {
                        char a = grid[x, y + 1];
                        char b = grid[x + 1, y + 1];
                        if (a == ']' || (a == '[' && b == ']')) MoveBox(x, y + 1, dir);
                        if ((a == ']' || a == '.') && b == '[') MoveBox(x + 1, y + 1, dir);
                    }
                    else if (grid[x, y] == ']')
                    {
                        char a = grid[x, y + 1];
                        char b = grid[x - 1, y + 1];
                        if (a == '[' || (a == ']' && b == '[')) MoveBox(x, y + 1, dir);
                        if ((a == '[' || a == '.') && b == ']') MoveBox(x - 1, y + 1, dir);
                    }

                    if (grid[x, y] == '[')
                    {
                        grid[x, y] = '.';
                        grid[x + 1, y] = '.';
                        grid[x, y + 1] = '[';
                        grid[x + 1, y + 1] = ']';
                    }
                    else if (grid[x, y] == ']')
                    {
                        grid[x, y] = '.';
                        grid[x - 1, y] = '.';
                        grid[x, y + 1] = ']';
                        grid[x - 1, y + 1] = '[';
                    }
                }
            }

            foreach (char move in characterList)
            {
                (int x, int y) dir = (0, 0);
                switch (move)
                {
                    case '^':
                        dir = (0, -1); break;
                    case 'v':
                        dir = (0, 1); break;
                    case '>':
                        dir = (1, 0); break;
                    case '<':
                        dir = (-1, 0); break;
                }

                char c = grid[currX + dir.x, currY + dir.y];

                if (c == '#') continue;
                else if (c == '.')
                {
                    grid[currX, currY] = '.';
                    currX += dir.x;
                    currY += dir.y;
                    grid[currX, currY] = '@';
                }
                else if (c == '[' || c == ']')
                {
                    if(CanMove(currX + dir.x, currY + dir.y, dir))
                    {
                        MoveBox(currX + dir.x, currY + dir.y, dir);
                        grid[currX, currY] = '.';
                        currX += dir.x;
                        currY += dir.y;
                        grid[currX, currY] = '@';
                    }
                }
            }

            long sum = 0;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols * 2; col++)
                {
                    Console.Write(grid[col, row]);
                    if (grid[col, row] == '[')
                    {
                        sum += row * 100 + col;
                    }
                }

                Console.WriteLine();
            }

            PrintResult(2, sum.ToString());
        }

    }
}
