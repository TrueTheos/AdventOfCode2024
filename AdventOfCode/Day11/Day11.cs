using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace AdventOfCode.Day11
{
    [Day(11)]
    public class Day11 : AdventDayBase
    {
        public override void SolvePart1(string input)
        {
            List<long> stones = input.TrimEnd().Split(' ').Select(long.Parse).ToList();

            int iterations = 25;

            for (int i = 0; i < iterations; i++)
            {
                int initStoneCount = stones.Count;
                for (int j = 0; j < initStoneCount; j++)
                {
                    if (stones[j] == 0) stones[j] = 1;
                    else if (stones[j] != 0 && stones[j].ToString().Length % 2 == 0)
                    {
                        string stoneString = stones[j].ToString();
                        int mid = stoneString.Length / 2;

                        long leftHalf = long.Parse(stoneString[..mid]);
                        long rightHalf = long.Parse(stoneString[mid..]);

                        stones[j] = leftHalf;
                        stones.Add(rightHalf);
                    }
                    else stones[j] *= 2024;
                }
            }

            PrintResult(1, stones.Count.ToString());
        }

        public Dictionary<(long stone, int depth), long> lookup = new();

        public override void SolvePart2(string input)
        {
            lookup = new();
            List<long> stones = input.TrimEnd().Split(' ').Select(long.Parse).ToList();

            int iterations = 75;

            long result = 0;
            foreach (var stone in stones)
            {
                result += GetAfterBlink(stone, 0, iterations);
            }
           
            PrintResult(2, result.ToString());
        }

        public long Cache(long stone, int depth, long value)
        {
            lookup[(stone, depth)] = value;
            return value;
        }

        public long GetAfterBlink(long stone, int blink, int maxblinks)
        {
            if(lookup.ContainsKey((stone, blink))) return lookup[(stone, blink)];

            if (blink >= maxblinks) return 1;
            if (stone == 0) return Cache(stone, blink, GetAfterBlink(1, blink + 1, maxblinks));
            else if (stone != 0 && stone.ToString().Length % 2 == 0)
            {
                string stoneString = stone.ToString();
                int mid = stoneString.Length / 2;

                long leftHalf = long.Parse(stoneString[..mid]);
                long rightHalf = long.Parse(stoneString[mid..]);

                return Cache(stone, blink, GetAfterBlink(leftHalf, blink + 1, maxblinks) + GetAfterBlink(rightHalf, blink + 1, maxblinks));
            }
            else return Cache(stone, blink, GetAfterBlink(stone * 2024, blink + 1, maxblinks));
        }
    }
}
