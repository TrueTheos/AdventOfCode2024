using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public static class Program
    {
        private static readonly Dictionary<int, Type> Days = new()
        {
            { 1, typeof(Day1) },
            { 2, typeof(Day2) },
            { 3, typeof(Day3) }
        };

        static void Main(string[] args)
        {

            Console.WriteLine("Enter the day to run:");
            if (int.TryParse(Console.ReadLine(), out int day) && Days.ContainsKey(day))
            {
                Console.WriteLine("Enter the part to run (1 or 2):");
                if (int.TryParse(Console.ReadLine(), out int part) && (part == 1 || part == 2))
                {
                    string input = InputHelper.GetInput(day);

                    if (Activator.CreateInstance(Days[day]) is IAdventDay adventDay)
                    {
                        if (part == 1)
                            adventDay.SolvePart1(input);
                        else
                            adventDay.SolvePart2(input);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid part. Please enter 1 or 2.");
                }
            }
            else
            {
                Console.WriteLine("Invalid day. Please enter a valid day number.");
            }
        }
    }

    public interface IAdventDay
    {
        void SolvePart1(string input);
        void SolvePart2(string input);
    }

    public abstract class AdventDayBase : IAdventDay
    {
        public abstract void SolvePart1(string input);
        public abstract void SolvePart2(string input);

        protected void PrintResult(int part, string result)
        {
            Console.WriteLine($"Part {part} Result: {result}");
        }
    }
}
