using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode
{
    [Day(5)]
    public class Day5 : AdventDayBase
    {
        public override void SolvePart1(string input)
        {
            string[] lines = input.Split('\n').Select(x => x.TrimEnd()).ToArray();

            Dictionary<int, List<int>> rules = new();

            int lineIndex = 0;
            while(lineIndex < lines.Length && !string.IsNullOrWhiteSpace(lines[lineIndex]))
            {
                int[] splitRules = lines[lineIndex].Split('|').Select(int.Parse).ToArray();

                if (rules.ContainsKey(splitRules[0]))
                {
                    if (rules[splitRules[0]] != null) rules[splitRules[0]].Add(splitRules[1]);
                    else rules[splitRules[0]] = new List<int>() { splitRules[1] };
                }
                else
                {
                    rules[splitRules[0]] = new List<int>() { splitRules[1] };
                }
                lineIndex++;
            }

            lineIndex++;

            int sum = 0;

            for (int i = lineIndex; i < lines.Length; i++)
            {
                int[] update = lines[i].Split(',').Select(int.Parse).ToArray();

                bool valid = true;
                for (int j = 1; j < update.Length; j++)
                {
                    if (!valid) break;
                    if (rules.ContainsKey(update[j]))
                    {
                        if (rules[update[j]].Contains(update[j - 1]))
                        {
                            valid = false;
                        }
                    }
                }

                if (valid) sum += update[update.Length / 2];
            }

            PrintResult(1, sum.ToString());
        }

        public override void SolvePart2(string input)
        {
            string[] lines = input.Split('\n').Select(x => x.TrimEnd()).ToArray();

            Dictionary<int, HashSet<int>> rules = new();

            int lineIndex = 0;
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) break;

                int[] splitRules = lines[lineIndex].Split('|').Select(int.Parse).ToArray();

                int left = splitRules[0];
                int right = splitRules[1];

                if (!rules.ContainsKey(left)) rules[left] = new HashSet<int>();
                rules[left].Add(right);

                lineIndex++;
            }

            lineIndex++;

            int result = 0;

            for (int i = lineIndex; i < lines.Length; i++)
            {
                List<int> update = lines[i].Split(',').Select(int.Parse).ToList();

                while (true)
                {
                    List<int> brokenNumbers = new();
                    List<int> brokenIndexes = new();
                    for (int j = 1; j < update.Count; j++)
                    {
                        if (ViolatesRule(update[j - 1], update[j], rules))
                        {
                            brokenIndexes.Add(j - 1);
                            brokenNumbers.Add(update[j - 1]);
                        }
                    }

                    if (brokenIndexes.Count == 0) break;

                    List<int> tempUpdate = new List<int>(update);

                    for (int j = update.Count; j >= 0; j--)
                    {
                        if (brokenIndexes.Contains(j)) tempUpdate.RemoveAt(j);
                    }

                    foreach (int num in brokenNumbers)
                    {
                        bool stop = false;
                        for (int j = 0; j < tempUpdate.Count + 1; j++)
                        {
                            if (stop) break;
                            if (j == tempUpdate.Count && !ViolatesRule(tempUpdate[^1], num, rules))
                            {
                                tempUpdate.Add(num);
                                break;
                            }

                            int current = tempUpdate[j];

                            if (j == 0)
                            {
                                if (!ViolatesRule(num, current, rules))
                                {
                                    tempUpdate.Insert(j, num);
                                    stop = true;
                                }
                            }
                            else
                            {
                                if (!ViolatesRule(tempUpdate[j - 1], num, rules) && !ViolatesRule(num, current, rules))
                                {
                                    tempUpdate.Insert(j, num);
                                    stop = true;
                                }
                            }
                        }
                    }

                    if(IsOrdered(tempUpdate, rules))
                    {
                        result += tempUpdate[tempUpdate.Count / 2];
                        break;
                    }
                    else
                    {
                        update = tempUpdate;
                    }
                }
            }

            PrintResult(2, result.ToString());
        }

        private bool IsOrdered(List<int> update, Dictionary<int, HashSet<int>> rules)
        {
            for (int j = 1; j < update.Count; j++)
            {
                if (ViolatesRule(update[j - 1], update[j], rules)) return false;
            }
            return true;
        }

        private bool ViolatesRule(int before, int after, Dictionary<int, HashSet<int>> rules)
        {
            return rules.ContainsKey(after) && rules[after].Contains(before);
        }
    }
}
