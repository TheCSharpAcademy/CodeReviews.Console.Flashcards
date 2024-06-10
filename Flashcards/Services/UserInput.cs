using System;
using System.Collections.Generic;

public class UserInput()
{
    private Valiadation _valiadation = new Valiadation();

    public MainMenuOptions MainMenu()
    {
        Console.WriteLine("Please choose an option below by typing the number next to it: \n");

        Console.WriteLine("1. Manager Stacks");
        Console.WriteLine("2. Manager Flashcards");
        Console.WriteLine("3. Manager Study Sessions");
        Console.WriteLine("4. Exit");

        var number = _valiadation.GetValidInt(1, Enum.GetNames(typeof(MainMenuOptions)).Length);

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

        return _valiadation.GetValidStack(stacks);
    }

    public StackOptions StacksManu()
    {
        Console.Clear();

        Console.WriteLine("Please choose an option below by typing the number next to it: \n");

        Console.WriteLine("1. Select a Stack");
        Console.WriteLine("2. Insert a new Stack");
        Console.WriteLine("3. Return to main menu");

        var number = _valiadation.GetValidInt(1, 3);

        return (StackOptions)number;
    }

    public Stack InsertStack(List<Stack> stacks)
    {
        Console.Clear();

        ListOfStacks(stacks);

        Console.WriteLine("Please type a name to create a stack. The stack name must be unique and not empty.");

        return _valiadation.CreateStack(stacks);
    }

    public StackOptions ManageStacksManu()
    {
        throw new NotImplementedException();
    }
}

