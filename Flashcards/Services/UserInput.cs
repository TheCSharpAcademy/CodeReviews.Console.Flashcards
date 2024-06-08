using System;

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
}