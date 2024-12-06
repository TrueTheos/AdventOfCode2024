using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    [Day(2)]
    public class Day2 : AdventDayBase
    {
        public override void SolvePart1(string input)
        {
            string[] lines = input.Split('\n');

            int sum = 0;

            foreach (string line in lines)
            {
                string[] parts = line.TrimEnd().Split(" ");

                bool growing = false;

                if (parts[0] == parts[1]) continue;
                int part0 = int.Parse(parts[0]);
                int part1 = int.Parse(parts[1]);
                growing = part0 < part1;
                if (Math.Abs(part1 - part0) > 3) continue;
                int previousPart = part1;

                bool failed = false;

                for (int i = 2; i < parts.Length; i++)
                {
                    int num = int.Parse(parts[i]);

                    if(num == previousPart || Math.Abs(num - previousPart) > 3 ||
                        (growing && num < previousPart) || (!growing && num > previousPart))
                    {
                        failed = true;
                        break;
                    }
                    previousPart = num;
                }

                if (!failed) sum++;
            }

            PrintResult(1, sum.ToString());
        }

        public override void SolvePart2(string input)
        {
            string[] lines = input.Split('\n');

            int sum = 0;

            foreach (string line in lines)
            {
                List<int> parts = line.TrimEnd().Split(" ").Select(x => int.Parse(x)).ToList();

                for (int i = 0; i < parts.Count; i++)
                {
                    List<int> newList = new(parts);
                    newList.RemoveAt(i);
                    if (IsCorrect(newList))
                    {
                        sum++;
                        break;
                    }
                }
            }

            PrintResult(2, sum.ToString());
        }

        private bool IsCorrect(List<int> input)
        {
            bool growing = false;

            if (input[0] == input[1]) return false;
            int part0 = input[0];
            int part1 = input[1];
            growing = part0 < part1;
            if (Math.Abs(part1 - part0) > 3) return false;
            int previousPart = part1;

            for (int i = 2; i < input.Count; i++)
            {
                int num = input[i];

                if (num == previousPart || Math.Abs(num - previousPart) > 3 ||
                    (growing && num < previousPart) || (!growing && num > previousPart))
                {
                    return false;
                }
                previousPart = num;
            }

            return true;
        }
    }
}
