using System.Globalization;

namespace Flashcards.Model
{
    internal class Validation
    {
        public static bool ValidateAlphaNumericInput(string input, bool forEdit = false, bool forStackName = false)
        {
            if (!forEdit)
            {
                if (!string.IsNullOrWhiteSpace(input) && (input.Any(char.IsLetter) || input.Any(char.IsDigit)))
                {
                    if (forStackName)
                    {
                        StacksRepository stacksRepo = new StacksRepository(DatabaseUtility.GetConnectionString());
                        List<Stacks> stacks = stacksRepo.GetAllStacks();
                        foreach (Stacks stack in stacks)
                        {
                            if (stack.Name.Trim().ToLower() == input.Trim().ToLower())
                            {
                                return false;
                            }
                        }

                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(input) || input.Any(char.IsLetter) || input.Any(char.IsDigit))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool ValidateYearInput(string input)
        {
            if (DateTime.TryParseExact(input, "yyyy", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime date))
            {
                if ((date.Year >= 2000) && (date.Year <= DateTime.Now.Year))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            
        }

        public static bool ValidateNumericInput(string input)
        {
            if (!string.IsNullOrEmpty(input) && input.Any(char.IsDigit))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ValidateDeleteConfirmation(string input)
        {
            if (input == "n" || input == "y")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
