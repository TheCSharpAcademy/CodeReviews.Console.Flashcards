using Flashcards.Controllers;
using Flashcards.Models;
using Flashcards.Utils;
using Spectre.Console;

namespace Flashcards.UI;

public class Study
{
    private readonly StudySessionsController _studySessionsController;
    private readonly StacksController _stacksController;
    private readonly FlashcardsController _flashcardsController;

    public Study(StudySessionsController studySessionsController, StacksController stacksController, FlashcardsController flashcardsController)
    {
        _studySessionsController = studySessionsController;
        _stacksController = stacksController;
        _flashcardsController = flashcardsController;
    }

    public async Task Run()
    {

        Console.WriteLine("Study session");

        Stack stack = null;
        StudySession studySessionResult = null;

        int QuestionsAnswered = 0;

        await ShowStacks();

        stack = await GetStackFromUser();

        if (stack == null)
        {
            return;
        }

        if (stack.Flashcards.Count == 0)
        {
            Console.WriteLine("No flashcards in this stack.");
            Console.WriteLine("Press any key to continue.");
            Console.ReadLine();

            return;
        }

        var studySession = new StudySession()
        {
            Score = 0,
            StackId = stack.Id,
            Date = DateTime.Now,
        };


        foreach (var flashcard in stack.Flashcards)
        {
            AnsiConsole.Clear();
            Table table = new Table();
            table.AddColumn("Question");


            table.AddRow(flashcard.Question);
            AnsiConsole.Write(table);

            String input = GetStringUserInput("Input your answer to this card(0 to exit): ");

            if (input == "0")
            {

                Console.WriteLine($"You got {studySession.Score} right out of {QuestionsAnswered}");

                studySessionResult = await _studySessionsController.AddNewStudySessionAsync(studySession);

                if (studySessionResult != null)
                {
                    Console.WriteLine("Record has been added!");
                }
                else
                {
                    Console.WriteLine("Fail to add record");
                }


                Console.WriteLine("Press any key to continue");
                Console.ReadLine();
                return;
            }

            var result = flashcard.Answer.ToLower() == input.ToLower();

            if (result)
            {
                Console.WriteLine("Correct!");

                studySession.Score += 1;
                Console.WriteLine("Press any key to continue");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine($"Wrong answer! Correct answer: {flashcard.Answer}");
                Console.WriteLine("Press any key to continue");
                Console.ReadLine();
            }
            QuestionsAnswered++;
        }

        Console.WriteLine($"You got {studySession.Score} right out of {QuestionsAnswered}");

        studySessionResult = await _studySessionsController.AddNewStudySessionAsync(studySession);

        if (studySessionResult != null)
        {
            Console.WriteLine("Record has been added!");
        }
        else
        {
            Console.WriteLine("Fail to add record");
        }

        Console.WriteLine("Press any key to continue");
        Console.ReadLine();
    }

    private async Task ShowStacks()
    {
        AnsiConsole.Clear();

        var stacks = await _stacksController.GetAllStacksAsync();

        Table table = new Table();
        table.AddColumns("Stack name");

        foreach (var stack in stacks)
        {
            table.AddRow(stack.Name);
        }

        AnsiConsole.Write(table);
    }

    private async Task<Stack> GetStackFromUser()
    {
        Stack stack = null;

        await ShowStacks();

        String stackInput = GetStringUserInput("Enter a stack to interact with(0 to exit): ");

        if (stackInput == "0")
        {
            return null;
        }

        stack = await _stacksController.GetStackByNameAsync(stackInput);

        while (stack == null)
        {
            Console.WriteLine("No record foudn.Try again.");
            stackInput = GetStringUserInput("Enter a stack to interact with(0 to exit): ");

            if (stackInput == "0")
            {
                return null;
            }

            stack = await _stacksController.GetStackByNameAsync(stackInput);
        }

        return stack;
    }
    private string GetStringUserInput(string message)
    {
        Console.Write(message);
        string input = Console.ReadLine();
        while (!Validate.IsValidString(input))
        {
            Console.WriteLine("Invalid input.Try again.");
            Console.Write(message);
            input = Console.ReadLine();
        }

        return input;
    }
}