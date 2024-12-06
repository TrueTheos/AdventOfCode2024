using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public static class Program
    {
        public static readonly Dictionary<int, Type> Days = LoadDays();

        static void Main(string[] args)
        {
            Console.WriteLine("Do you want to benchmark? (y/n):");
            string benchmarkInput = Console.ReadLine()?.Trim().ToLower();

            if (benchmarkInput == "y")
            {
                Console.WriteLine("Enter the day to benchmark:");
                if (int.TryParse(Console.ReadLine(), out int benchmarkDay) && Program.Days.ContainsKey(benchmarkDay))
                {
                    Console.WriteLine("Enter the part to benchmark (1 or 2):");
                    if (int.TryParse(Console.ReadLine(), out int benchmarkPart) && (benchmarkPart == 1 || benchmarkPart == 2))
                    {
                        // Configure the benchmark runner with the specific day and part
                        BenchmarkRunnerClass.ConfigureForSpecificDayAndPart(benchmarkDay, benchmarkPart);

                        // Run the benchmark for the specified day and part
                        BenchmarkRunner.Run<BenchmarkRunnerClass>();
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

                return;
            }

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

        private static Dictionary<int, Type> LoadDays()
        {
            return Assembly.GetExecutingAssembly()
            .GetTypes()
                .Where(t => typeof(IAdventDay).IsAssignableFrom(t) && t.GetCustomAttribute<DayAttribute>() != null)
                .ToDictionary(t => t.GetCustomAttribute<DayAttribute>().DayNumber, t => t);
        }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class DayAttribute : Attribute
    {
        public int DayNumber { get; }
        public DayAttribute(int dayNumber) => DayNumber = dayNumber;
    }

    public class BenchmarkRunnerClass
    {
        private readonly Dictionary<int, Type> _days = Program.Days;

        // Filtered dynamically based on user input
        [ParamsSource(nameof(GetFilteredDays))]
        public int Day { get; set; }

        [ParamsSource(nameof(GetFilteredParts))]
        public int Part { get; set; }

        private IAdventDay _adventDay;
        private string _input;

        [GlobalSetup]
        public void Setup()
        {
            if (_days.TryGetValue(Day, out Type dayType))
            {
                _adventDay = Activator.CreateInstance(dayType) as IAdventDay;
                if (_adventDay is AdventDayBase adventDayBase)
                {
                    adventDayBase.EnableSilentMode();
                }
                _input = InputHelper.GetInput(Day);
            }
        }

        [Benchmark]
        public void RunDay()
        {
            if (_adventDay == null || string.IsNullOrEmpty(_input))
                throw new InvalidOperationException("Benchmark setup failed.");

            if (Part == 1)
                _adventDay.SolvePart1(_input);
            else
                _adventDay.SolvePart2(_input);
        }

        // Filter days dynamically based on user input
        public static IEnumerable<int> GetFilteredDays() => new[] { DayToBenchmark };

        // Filter parts dynamically based on user input
        public static IEnumerable<int> GetFilteredParts() => new[] { PartToBenchmark };

        // Static properties to hold user-selected day and part
        public static int DayToBenchmark { get; private set; }
        public static int PartToBenchmark { get; private set; }

        // Configure the benchmark runner for a specific day and part
        public static void ConfigureForSpecificDayAndPart(int day, int part)
        {
            if (Program.Days.ContainsKey(day) && (part == 1 || part == 2))
            {
                DayToBenchmark = day;
                PartToBenchmark = part;
            }
            else
            {
                throw new ArgumentException("Invalid day or part.");
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
        private bool _silentMode = false;

        public abstract void SolvePart1(string input);
        public abstract void SolvePart2(string input);

        protected void PrintResult(int part, string result)
        {
            if (!_silentMode)
            {
                Console.WriteLine($"Part {part} Result: {result}");
            }
        }

        public void EnableSilentMode()
        {
            _silentMode = true;
        }

        public void DisableSilentMode()
        {
            _silentMode = false;
        }
    }
}
