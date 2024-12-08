using Microsoft.Diagnostics.Runtime.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AdventOfCode.Day7
{
    [Day(7)]
    public class Day7 : AdventDayBase
    {
        public override void SolvePart1(string input)
        {
            string[] lines = input.Split('\n').Select(x => x.TrimEnd()).ToArray();

            long result = 0;

            foreach (string line in lines)
            {
                long goal;
                List<long> elements = new();

                var split = line.Replace(":", "").Split(' ').Select(long.Parse).ToList();
                goal = split[0];
                elements.AddRange(split[1..]);

                for (int mask = 0; mask < Math.Pow(2, elements.Count - 1); mask++)
                {
                    var currentResult = elements[0];

                    for (int i = 0; i < elements.Count - 1; i++)
                    {
                        if ((mask & (1 << i)) == 0)
                        {
                            currentResult += elements[i + 1];
                        }
                        else
                        {
                            currentResult *= elements[i + 1];
                        }
                    }

                    if(currentResult == goal)
                    {
                        result += goal;
                        break;
                    }
                }
            }

            PrintResult(1, result.ToString());
        }

        public override void SolvePart2(string input)
        {
            string[] lines = input.Split('\n').Select(x => x.TrimEnd()).ToArray();

            long result = 0;

            foreach (string line in lines)
            {
                long goal;
                List<long> elements = new();

                var split = line.Replace(":", "").Split(' ').Select(long.Parse).ToList();
                goal = split[0];
                elements.AddRange(split[1..]);
                bool found = false;

                for (int mask = 0; mask < Math.Pow(3, elements.Count - 1); mask++)
                {
                    if (found) break;

                    List<long> tempEls = new List<long>(elements);
                    var currentResult = tempEls[0];
                    int remainingMask = mask;

                    for (int i = 1; i < tempEls.Count; i++)
                    {
                        int op = remainingMask % 3;
                        remainingMask /= 3;

                        if (currentResult > goal) break;

                        switch (op)
                        {
                            case 0:
                                currentResult += tempEls[i];
                                break;
                            case 1:
                                currentResult *= tempEls[i];
                                break;
                            case 2:
                                currentResult = long.Parse(currentResult.ToString() + tempEls[i].ToString());
                                break;
                        }
                    }

                    if (currentResult == goal)
                    {
                        result += goal;
                        found = true;
                        break;
                    }
                }
            }

            PrintResult(2, result.ToString());
        }
    }
}
