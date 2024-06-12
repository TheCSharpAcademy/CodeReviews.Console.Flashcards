﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;

public class UserInput()
{
    private Validation _validation = new Validation();

    public MainMenuOptions MainMenu()
    {
        Console.Clear();

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

    public void ShowFlashcards(List<Flashcard> flashcards)
    {
        Console.Clear();

        Console.WriteLine($"{"ID",0} {"Question",25} {"Answer",45}");
        Console.WriteLine(new string('-', 100));
        var customId = 1;

        foreach (Flashcard flashcard in flashcards)
        {
            Console.WriteLine($"{customId,0} {flashcard.Question,25} {flashcard.Answer,45}");
            customId++;
        }

        Console.WriteLine(new string('-', 100));
    }

    public int FlashcardAmount(List<Flashcard> flashcards)
    {
        Console.WriteLine("Whats the maximum flashcards you would like to see?");
        return _validation.GetValidInt(1, flashcards.Count());
    }

    public FlashcardsMenuOptions ViewFlashCardsMainMenu()
    {
        Console.Clear();

        Console.WriteLine("Please choose an option below by typing the number next to it: \n");

        Console.WriteLine($"{(int)FlashcardsMenuOptions.ViewFlashcardByStack}. View Flashcards by Stack");
        Console.WriteLine($"{(int)FlashcardsMenuOptions.Exit}. Return to main menu");

        var number = _validation.GetValidInt(1, Enum.GetNames(typeof(FlashcardsMenuOptions)).Length);

        return (FlashcardsMenuOptions)number;
    }

    public Flashcard GetFlashcard(List<Flashcard> flashcards)
    {
        Console.WriteLine("Type the ID of the flashcard you which to manage:");
        return _validation.GetValidFlashcard(flashcards);
    }

    public FlashcardsManageOptions ManageeFlashcard(Flashcard flashcard)
    {
        Console.Clear();

        Console.WriteLine("Please choose an option below by typing the number next to it: \n");

        Console.WriteLine($"{(int)FlashcardsManageOptions.Delete}. Delete flashcard");
        Console.WriteLine($"{(int)FlashcardsManageOptions.Edit}. Edit this flashcard");
        Console.WriteLine($"{(int)FlashcardsManageOptions.Exit}. Return to main menu");

        var number = _validation.GetValidInt(1, Enum.GetNames(typeof(FlashcardsManageOptions)).Length);

        return (FlashcardsManageOptions)number;
    }

    public Flashcard UpdateCard(Flashcard oldFlashcard)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("HINT: You can leave an input empty if you don't wish to update an answer.\n");
        Console.ResetColor();

        Console.WriteLine($"Current Question: {oldFlashcard.Question}");
        Console.WriteLine($"Current Answer: {oldFlashcard.Answer}");

        var newFlashcard = new Flashcard
        {
            Id = oldFlashcard.Id,
            StackId = oldFlashcard.StackId
        };

        Console.WriteLine("Type a new question");
        var question = Console.ReadLine();
        if (question.Trim().Length == 0)
        {
            newFlashcard.Question = oldFlashcard.Question;
        }
        else
        {
            newFlashcard.Question = question;
        }

        Console.WriteLine("Type a new answer");
        var answer = Console.ReadLine();
        if (answer.Trim().Length == 0)
        {
            newFlashcard.Answer = oldFlashcard.Answer;
        }
        else
        {
            newFlashcard.Answer = answer;
        }

        Console.WriteLine("New Flashcard: ");
        Console.WriteLine($"Question: {newFlashcard.Question}");
        Console.WriteLine($"Answer: {newFlashcard.Answer}");

        return newFlashcard;
    }
}
