using Flashcards.Models;
using Flashcards.Tables;

namespace Flashcards;

internal class FlashcardInput
{
    public static void CreateFlashcard(int stackId)
    {
        Console.Clear();

        string question = GetQuestion("Enter the question:\n", false);
        string answer = GetAnswer("Enter the answer:\n", false);

        Flashcard flaschard = new Flashcard()
        {
            Question = question,
            Answer = answer,
            StackID = stackId
        };

        FlashcardsTable.InsertFlashcard(question, answer, stackId);

        Console.Clear();

        FlashcardUI.DisplayFlashcards(stackId);
    }

    public static string GetQuestion(string prompt, bool isUpdate, string currentQuestion = "")
    {
        Console.WriteLine(isUpdate ?
            $"{prompt} (current: {currentQuestion}, press Enter to keep current):\n" :
            $"\n{prompt}");

        string question = Console.ReadLine().Trim();

        if (string.IsNullOrEmpty(question) && !isUpdate)
        {
            Console.WriteLine("Invalid input.");
            return GetQuestion(prompt, isUpdate, currentQuestion);
        }
        else
        {
            if (string.IsNullOrEmpty(question) && isUpdate)
            {
                Console.WriteLine("Current question kept");
                return currentQuestion;
            }
        }

        Utility.ValidString(question);

        return question;
    }

    public static string GetAnswer(string prompt, bool isUpdate, string currentAnswer = "")
    {
        Console.WriteLine(isUpdate ?
            $"{prompt} (current: {currentAnswer}, press Enter to keep current):\n" :
            $"\n{prompt}");

        string answer = Console.ReadLine().Trim();


        if (string.IsNullOrEmpty(answer) && !isUpdate)
        {
            Console.WriteLine("Invalid input.");
            return GetQuestion(prompt, isUpdate, currentAnswer);
        }
        else
        {
            if (string.IsNullOrEmpty(answer) && isUpdate)
            {
                Console.WriteLine("Current answer kept");
                return currentAnswer;
            }
        }

        Utility.ValidString(answer);

        return answer;
    }
}
