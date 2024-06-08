using System.Linq;

public class Valiadation()
{
    public int GetValidInt(int min, int max)
    {
        bool IsValidInt = false;
        int result = 0;
        const int resultOffset = 1;

        while (!IsValidInt)
        {
            var input = Console.ReadLine();
            if (!int.TryParse(input, out result) || result < min || result > max) Console.WriteLine($"Invalid value. Please enter a number value between {min} and {max}");
            else IsValidInt = true;
        }

        return result-resultOffset;
    }

    public Stack GetValisStack(List<Stack> stacks)
    {
        string? input = "";

        while (stacks.FirstOrDefault(x => x.Name.ToLower() == input) is null)
        {
            input = Console.ReadLine()?.ToLower();
            if (stacks.FirstOrDefault(x => x.Name.ToLower() == input) is null)
            {
                Console.Beep();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("That stack does not exist. Please try again.");
                Console.ResetColor();
            }
        }

        return stacks.First(s => s.Name.ToLower() == input);
    }
}