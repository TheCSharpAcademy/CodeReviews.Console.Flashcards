using System;
using System.Collections.Generic;
using System.Net.Http.Headers;

public class UserInput()
{
    private Validation _validation = new Validation();

    public MainMenuOptions MainMenu()
    {
        Console.WriteLine("Please choose an option below by typing the number next to it: \n");

        Console.WriteLine($"{(int)MainMenuOptions.Stacks}. Manage Stacks");
        Console.WriteLine($"{(int)MainMenuOptions.Flashcards}. Manage Flashcards");
        Console.WriteLine($"{(int)MainMenuOptions.Study}. Manage Study Sessions");
        Console.WriteLine($"{(int)MainMenuOptions.InsertTestData}. Insert Test Data");
        Console.WriteLine($"{(int)MainMenuOptions.Exit}. Exit");

        var number = _validation.GetValidInt(1, Enum.GetNames(typeof(MainMenuOptions)).Length);

        return (MainMenuOptions)number;
    }

    public void ListOfStacks(List<Stack> stacks)
    {
        Console.WriteLine("List of stacks:");

        Console.ForegroundColor = ConsoleColor.Green;

        if (stacks.Count == 0) Console.WriteLine("No stacks to show");

        foreach (var item in stacks)
        {
            Console.WriteLine(item.Name);
        }

        Console.ResetColor();
        Console.WriteLine(new string('-', 50));
    }

    public Stack SelectStack(List<Stack> stacks)
    {
        Console.Clear();

        ListOfStacks(stacks);

        Console.WriteLine("\nPlace pick a Stack by entering it's name: \n");

        return _validation.GetValidStack(stacks);
    }

    public StackOptions StacksManu()
    {
        Console.Clear();

        Console.WriteLine("Please choose an option below by typing the number next to it: \n");

        Console.WriteLine($"{(int)StackOptions.Select}. Select a Stack");
        Console.WriteLine($"{(int)StackOptions.Insert}. Insert a new Stack");
        Console.WriteLine($"{(int)StackOptions.Exit}. Return to main menu");

        var number = _validation.GetValidInt(1, 3);

        return (StackOptions)number;
    }

    public Stack InsertStack(List<Stack> stacks)
    {
        Console.Clear();

        ListOfStacks(stacks);

        Console.WriteLine("Please type a name to create a stack. The stack name must be unique and not empty.");

        return _validation.CreateStack(stacks);
    }

    public ManageStackOption ManageStacksManu(Stack stack)
    {
        Console.Clear();

        Console.WriteLine($"Current Stack: {stack.Name}\n");

        Console.WriteLine("Please choose an option below by typing the number next to it: \n");

        Console.WriteLine($"{(int)ManageStackOption.ChangeStack}. Change current Stack");
        Console.WriteLine($"{(int)ManageStackOption.ViewCardsAll}. View all flashcards in stack");
        Console.WriteLine($"{(int)ManageStackOption.ViewCardsAmount}. View x amount of cards in stack");
        Console.WriteLine($"{(int)ManageStackOption.CreateCard}. Create a flashcard in current stack");
        Console.WriteLine($"{(int)ManageStackOption.EditCard}. Edit a flashcard");
        Console.WriteLine($"{(int)ManageStackOption.DeleteCard}. Delete a flashcard");
        Console.WriteLine($"{(int)ManageStackOption.DeleteStack}. Delete current Stack");
        Console.WriteLine($"{(int)ManageStackOption.Exit}. Return to main menu");

        var number = _validation.GetValidInt(1, Enum.GetNames(typeof(ManageStackOption)).Length);

        return (ManageStackOption)number;
    }

    public Flashcard CreateFlashcard(Stack stack)
    {
        Flashcard flashcard = new Flashcard
        {
            StackId = stack.Id
        };

        Console.WriteLine("Type a question for your flashcard");
        flashcard.Question = _validation.GetValidString(10);

        Console.WriteLine("Type a answer for your flashcard");
        flashcard.Answer = _validation.GetValidString(10);
        return flashcard;
    }

    /// <summary>
    /// Prompts the user to input the number of random Stacks and Flashcards to add per stack.
    /// </summary>
    /// <returns>
    /// A tuple containing two integers: the first integer represents the number of random Stacks to add,
    /// and the second integer represents the number of random Flashcards to add per stack.
    /// </returns>
    public Tuple<int, int> InsertTestData()
    {
        Console.WriteLine("How many random Stacks do you want to add");
        var stacks = _validation.GetValidInt(5, 100);

        Console.WriteLine("How many random Flashcards do you want to add per stack?");
        var flashcards = _validation.GetValidInt(5, 100);

        return Tuple.Create(stacks, flashcards);
    }

}