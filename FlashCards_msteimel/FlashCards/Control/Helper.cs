using FlashCards.Database;
using FlashCards.Models;

namespace FlashCards.Control;

internal static class Helper
{
    internal static string GetStringInput()
    {
        string input = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Input cannot be empty. Please try again.");
            input = Console.ReadLine();
        }

        return input;
    }

    internal static string GetIDInput()
    {
        string input = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(input) || !int.TryParse(input, out int _))
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Input cannot be empty. Please try again.");
            }
            else
            {
                Console.WriteLine("Input must be a number. Please try again.");
            }

            input = Console.ReadLine();
        }

        return input;
    }

    internal static void SeedDatabase()
    {
        FlashCard flashCard = new FlashCard();
        Random random = new Random();
        int stackID = 0;

        Stack additionStack = new Stack();
        additionStack.Name = random.Next(100000, 1000000).ToString();
        string[] additionQuestions = { "1+1", "2+2", "3+3", "4+4", "5+5", "6+6", "7+7", "8+8", "9+9", "10+10" };
        string[] additionAnswers = { "2", "4", "6", "8", "10", "12", "14", "16", "18", "20" };

        StackDBOperations.AddStack(additionStack);
        stackID = StackDBOperations.GetStackIDByName(additionStack.Name);

        for (int i = 0; i < additionQuestions.Length; i++)
        {
            flashCard.Question = additionQuestions[i];
            flashCard.Answer = additionAnswers[i];
            flashCard.StackId = stackID;
            FlashCardDBOperations.AddFlashCard(flashCard);
        }

        Stack subtractionStack = new Stack();
        subtractionStack.Name = random.Next(100000, 1000000).ToString();
        string[] subtractionQuestions = { "1-1", "3-1", "10-8", "18-10", "20-10" };
        string[] subtractionAnswers = { "0", "2", "2", "8", "10" };

        StackDBOperations.AddStack(subtractionStack);
        stackID = StackDBOperations.GetStackIDByName(subtractionStack.Name);

        for (int i = 0; i < subtractionQuestions.Length; i++)
        {
            flashCard.Question = subtractionQuestions[i];
            flashCard.Answer = subtractionAnswers[i];
            flashCard.StackId = stackID;
            FlashCardDBOperations.AddFlashCard(flashCard);
        }

        Stack multiplicationStack = new Stack();
        multiplicationStack.Name = random.Next(100000, 1000000).ToString();
        string[] multiplicationQuestions = { "1*1", "2*2", "3*3", "4*4", "5*5", "6*6", "7*7" };
        string[] multiplicationAnswers = { "1", "4", "9", "16", "25", "36", "49" };

        StackDBOperations.AddStack(multiplicationStack);
        stackID = StackDBOperations.GetStackIDByName(multiplicationStack.Name);

        for (int i = 0; i < multiplicationQuestions.Length; i++)
        {
            flashCard.Question = multiplicationQuestions[i];
            flashCard.Answer = multiplicationAnswers[i];
            flashCard.StackId = stackID;
            FlashCardDBOperations.AddFlashCard(flashCard);
        }

        Stack divisionStack = new Stack();
        divisionStack.Name = random.Next(100000, 1000000).ToString();
        string[] divisionQuestions = { "1/1", "4/2", "16/4", "9/3" };
        string[] divisionAnswers = { "1", "2", "4", "3" };

        StackDBOperations.AddStack(divisionStack);
        stackID = StackDBOperations.GetStackIDByName(divisionStack.Name);

        for (int i = 0; i < divisionQuestions.Length; i++)
        {
            flashCard.Question = divisionQuestions[i];
            flashCard.Answer = divisionAnswers[i];
            flashCard.StackId = stackID;
            FlashCardDBOperations.AddFlashCard(flashCard);
        }
    }
}

