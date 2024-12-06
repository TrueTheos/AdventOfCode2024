using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode
{
    [Day(3)]
    public class Day3 : AdventDayBase
    {
        public override void SolvePart1(string input)
        {
            string pattern = @"mul\((\d+),(\d+)\)";
            MatchCollection matches = Regex.Matches(input, pattern);

            int sum = 0;

            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    int num1 = int.Parse(match.Groups[1].Value);
                    int num2 = int.Parse(match.Groups[2].Value);

                    sum += num1 * num2;
                }
            }

            PrintResult(1, sum.ToString());
        }

        public override void SolvePart2(string input)
        {
            string pattern = @"mul\((\d+),(\d+)\)";
            string commandPattern = @"do\(\)|don't\(\)";
            MatchCollection matches = Regex.Matches(input, pattern);

            int sum = 0;

            int lastMatchIndex = 0;

            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    string subStr = input.Substring(lastMatchIndex, match.Index - lastMatchIndex);
                    MatchCollection commandMatches = Regex.Matches(subStr, commandPattern);

                    int num1 = int.Parse(match.Groups[1].Value);
                    int num2 = int.Parse(match.Groups[2].Value);

                    if (commandMatches.Count > 0)
                    {
                        if (commandMatches.Last().Value == "do()")
                        {                           
                            sum += num1 * num2;
                        }

                        lastMatchIndex = commandMatches.Last().Index;
                    }
                    else
                    {
                        sum += num1 * num2;
                    }
                }
            }

            PrintResult(2, sum.ToString());
        }
    }
}
