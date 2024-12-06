using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    [Day(1)]
    public class Day1 : AdventDayBase
    {
        public override void SolvePart1(string input)
        {
            List<int> leftPart = new();
            List<int> rightPart = new();
            string[] lines = input.Split('\n');

            foreach (string line in lines)
            {
                string[] parts = line.Split("   ");
                leftPart.Add(Int32.Parse(parts[0]));
                rightPart.Add(Int32.Parse(parts[1].TrimEnd()));
            }
            leftPart.Sort();
            rightPart.Sort();

            int total = 0;

            for (int i = 0; i < leftPart.Count; i++)
            {
                total += Math.Abs(leftPart[i] - rightPart[i]);
            }

            PrintResult(1, total.ToString());
        }

        public override void SolvePart2(string input)
        {
            List<int> leftPart = new();
            Dictionary<int, int> rightPart = new();
            string[] lines = input.Split('\n');

            foreach (string line in lines)
            {
                string[] parts = line.Split("   ");
                leftPart.Add(Int32.Parse(parts[0]));

                int right = Int32.Parse(parts[1].TrimEnd());
                if (rightPart.ContainsKey(right)) rightPart[right]++;
                else rightPart[right] = 1;
            }         

            int total = 0;

            for (int i = 0; i < leftPart.Count; i++)
            {
                int left = leftPart[i];
                if (rightPart.ContainsKey(left)) 
                {
                    total += left * rightPart[left];
                }
            }

            PrintResult(2, total.ToString());
        }
    }
}
