﻿using Flashcards.StackView;
using Flashcards.StudyView;
using Flashcards.CardView;

namespace Flashcards.MainView;

public static class MainViewController
{
    public static bool? InitMainView()
    {
        Console.Clear();
        Console.WriteLine("Welcome to my flashcard app!");
        Console.WriteLine("-Press s to view stacks");
        Console.WriteLine("-Press f to view flashcards");
        Console.WriteLine("-Press b to begin studying");
        Console.WriteLine("-Press q to quit");

        var option = GetMainViewInput();

        if (option == 's') StackViewController.InitMainStackView();
        else if (option == 'f') CardViewController.InitMainCardView();
        else if (option == 'b') StudyViewController.InitStudyView();
        else return true;

        return false;
    }

    public static char GetMainViewInput()
    {
        var key = Console.ReadKey(true).KeyChar;

        if (key != 's' && key != 'f' && key != 'b' && key != 'q')
        {
            return GetMainViewInput();
        }

        return key;
    }
}