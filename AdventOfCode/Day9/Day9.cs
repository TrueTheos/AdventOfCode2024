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
            int afterTransformLength = 0;
            int length = input.TrimEnd().Length;

            int fileCount = (length + 1) / 2;
            int spaceCount = length / 2;

            int[] spaceIndexes = new int[spaceCount];
            int[] spaceLengths = new int[spaceCount];

            int[] fileIndexes = new int[fileCount];
            int[] fileIds = new int[fileCount];
            int[] fileLengths = new int[fileCount];

            int fileIndex = 0;
            int spaceIndex = 0;
            for (int i = 0; i < length; i++)
            {
                int digit = input[i] - '0';
                if (i % 2 == 0)
                {
                    fileIndexes[fileIndex] = afterTransformLength;
                    fileIds[fileIndex] = fileIndex;
                    fileLengths[fileIndex] = digit;
                    fileIndex++;
                }
                else
                {
                    spaceLengths[spaceIndex] = digit;
                    spaceIndexes[spaceIndex] = afterTransformLength;
                    spaceIndex++;
                }

                afterTransformLength += digit;
            }

            long sum = 0;

            for (int i = fileCount - 1; i >= 0; i--)
            {
                int fIndex = fileIndexes[i];
                int fLength = fileLengths[i];
                int fId = fileIds[i];

                int spaceStartIndex = -1;
                int spaceIndexInList = 0;

                for (int j = 0; j < spaceIndex; j++)
                {
                    if (spaceIndexes[j] != -1 && spaceLengths[j] >= fLength && spaceIndexes[j] < fIndex)
                    {
                        spaceStartIndex = spaceIndexes[j];
                        spaceIndexInList = j;
                        break;
                    }
                }

                if (spaceStartIndex == -1)
                {
                    for (int j = 0; j < fLength; j++)
                    {
                        sum += (fIndex + j) * fId;
                    }
                    continue;
                }
                
                for (int j = 0; j < fLength; j++)
                {
                    sum += (spaceStartIndex + j) * fId;
                }

                if (spaceLengths[spaceIndexInList] > fLength)
                {
                    spaceIndexes[spaceIndexInList] += fLength;
                    spaceLengths[spaceIndexInList] -= fLength;
                }
                else
                {
                    spaceIndexes[spaceIndexInList] = -1;
                }
            }

            //Console.WriteLine(span.ToString());
            PrintResult(2, sum.ToString());
        }
    }
}
