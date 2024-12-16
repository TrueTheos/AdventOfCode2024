using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode.Day13
{
    [Day(13)]
    public partial class Day13 : AdventDayBase
    {
        [GeneratedRegex(@"Button A: X\+(\d+), Y\+(\d+)")]
        private static partial Regex ButtonARegex();

        [GeneratedRegex(@"Button B: X\+(\d+), Y\+(\d+)")]
        private static partial Regex ButtonBRegex();

        [GeneratedRegex(@"Prize: X=(\d+), Y=(\d+)")]
        private static partial Regex PrizeRegex();
        public override void SolvePart1(string input)
        {
            List<string> lines = input.Split('\n').Select(x => x.TrimEnd()).ToList();

            int aX = 0;
            int aY = 0;
            int bX = 0;
            int bY = 0;
            int pX = 0;
            int pY = 0;

            long sum = 0;
            for (int i = 0; i < lines.Count; i += 4)
            {
                if (i + 2 >= lines.Count) break;

                var matchA = ButtonARegex().Match(lines[i]);
                if (!matchA.Success) continue;
                aX = int.Parse(matchA.Groups[1].ValueSpan);
                aY = int.Parse(matchA.Groups[2].ValueSpan);

                var matchB = ButtonBRegex().Match(lines[i + 1]);
                if (!matchB.Success) continue;
                bX = int.Parse(matchB.Groups[1].ValueSpan);
                bY = int.Parse(matchB.Groups[2].ValueSpan);

                var matchP = PrizeRegex().Match(lines[i + 2]);
                if (!matchP.Success) continue;
                pX = int.Parse(matchP.Groups[1].ValueSpan);
                pY = int.Parse(matchP.Groups[2].ValueSpan);

                int det = aX * bY - aY * bX;
                if (det == 0) continue;

                int detA = pX * bY - pY * bX;
                int detB = aX * pY - aY * pX;

                if (detA % det != 0 || detB % det != 0) continue;
                int a = detA / det;
                int b = detB / det;

                if (a <= 100 && b <= 100)
                {
                    sum += a * 3 + b;
                }
            }

            PrintResult(1, sum.ToString());
        }

        public override void SolvePart2(string input)
        {
            List<string> lines = input.Split('\n').Select(x => x.TrimEnd()).ToList();

            long aX = 0;
            long aY = 0;
            long bX = 0;
            long bY = 0;
            long pX = 0;
            long pY = 0;

            long sum = 0;
            for (int i = 0; i < lines.Count; i += 4)
            {
                if (i + 2 >= lines.Count) break;

                var matchA = ButtonARegex().Match(lines[i]);
                if (!matchA.Success) continue;
                aX = long.Parse(matchA.Groups[1].ValueSpan);
                aY = long.Parse(matchA.Groups[2].ValueSpan);

                var matchB = ButtonBRegex().Match(lines[i + 1]);
                if (!matchB.Success) continue;
                bX = long.Parse(matchB.Groups[1].ValueSpan);
                bY = long.Parse(matchB.Groups[2].ValueSpan);

                var matchP = PrizeRegex().Match(lines[i + 2]);
                if (!matchP.Success) continue;
                pX = long.Parse(matchP.Groups[1].ValueSpan) + 10000000000000;
                pY = long.Parse(matchP.Groups[2].ValueSpan) + 10000000000000;

                long det = aX * bY - aY * bX;
                if (det == 0) continue;

                long detA = pX * bY - pY * bX;
                long detB = aX * pY - aY * pX;

                if (detA % det != 0 || detB % det != 0) continue;
                long a = detA / det;
                long b = detB / det;

                sum += a * 3 + b;
            }

            PrintResult(2, sum.ToString());
        }
    }
}
