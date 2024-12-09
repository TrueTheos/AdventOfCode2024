using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Day9
{
    [Day(9)]
    public class Day9 : AdventDayBase
    {
        public override void SolvePart1(string input)
        {
            List<int> values = new();
            Queue<int> freeSpaces = new();
            List<int> charIndexes = new();

            int length = input.TrimEnd().Length;
            int id = 0;
            for (int i = 0; i < length; i++) 
            {
                int digit = int.Parse(input[i].ToString());
                if(i % 2 == 0)
                {
                    for (int j = 0; j < digit; j++)
                    {
                        values.Add(id);
                        charIndexes.Add(values.Count - 1);
                    }
                    id++;
                }
                else
                {
                    for (int j = 0; j < digit; j++)
                    {
                        values.Add(-1);
                        freeSpaces.Enqueue(values.Count - 1);
                    }
                }
            }

            int ind = charIndexes.Count - 1;
            while(freeSpaces.Count > 0)
            {
                int dotIndex = freeSpaces.Dequeue();
                if (charIndexes[ind] < dotIndex) break;

                values[dotIndex] = values[charIndexes[ind]];
                values[charIndexes[ind]] = -1;

                ind--;
            }

            long sum = 0;

            for (int i = 0; i < values.Count; i++) 
            {
                if (values[i] == -1) break;

                sum += i * values[i];
            }

            //Console.WriteLine(span.ToString());
            PrintResult(1, sum.ToString());
        }

        public override void SolvePart2(string input)
        {
            List<int> withDots = new();
            List<(int index, int length)> freeSpaces = new();
            List<(int index, int id, int length)> files = new();

            int length = input.TrimEnd().Length;
            int id = 0;
            for (int i = 0; i < length; i++)
            {
                int digit = int.Parse(input[i].ToString());
                if (i % 2 == 0)
                {
                    files.Add((withDots.Count, id, digit));
                    for (int j = 0; j < digit; j++)
                    {
                        withDots.Add(id);              
                    }
                    id++;
                }
                else
                {
                    freeSpaces.Add((withDots.Count, digit));

                    for(int j = 0; j < digit; j++)
                    {
                        withDots.Add(-1);
                    }
                }
            }

            for (int i = files.Count - 1; i >= 0; i--)
            {
                var currFile = files[i];

                int freeSpaceStartIndex = -1;
                int freeSpaceIndex = 0;

                for (int j = 0; j < freeSpaces.Count; j++)
                {
                    if (freeSpaces[j].length >= currFile.length && freeSpaces[j].index < currFile.index)
                    {
                        freeSpaceStartIndex = freeSpaces[j].index;
                        freeSpaceIndex = j;
                        break;
                    }
                }

                if (freeSpaceStartIndex == -1) continue;
                
                for (int j = 0; j < currFile.length; j++)
                {
                    withDots[freeSpaceStartIndex + j] = currFile.id;
                    withDots[currFile.index + j] = -1;
                }

                if (freeSpaces[freeSpaceIndex].length > currFile.length)
                {
                    freeSpaces[freeSpaceIndex] = new(freeSpaces[freeSpaceIndex].index + currFile.length, freeSpaces[freeSpaceIndex].length - currFile.length);
                }
                else
                {
                    freeSpaces.RemoveAt(freeSpaceIndex);
                }
            }

            long sum = 0;

            for (int i = 0; i < withDots.Count; i++)
            {
                if (withDots[i] == -1) continue;

                sum += i * withDots[i];
            }

            //Console.WriteLine(span.ToString());
            PrintResult(2, sum.ToString());
        }
    }
}
