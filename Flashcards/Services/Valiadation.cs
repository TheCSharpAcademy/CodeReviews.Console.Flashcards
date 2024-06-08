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
}