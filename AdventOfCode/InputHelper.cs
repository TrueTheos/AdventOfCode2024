public static class InputHelper
{
    public static string GetInput(int day)
    {
        try
        {
           // string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string baseDirectory = @"C:\Projects\AdventOfCode\AdventOfCode\Inputs\";
            string filePath = Path.Combine(baseDirectory, $"Day{day}.txt");
            return File.ReadAllText(filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading input for Day {day}: {ex.Message}");
            return string.Empty;
        }
    }
}