using Kmakai.FlashCards.Models;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;

namespace Kmakai.FlashCards.Controllers;

public class MenuController
{
    private FlashCardsApp App { get; set; }
    public MenuController(FlashCardsApp app)
    {
        App = app;
    }
    public void HandleMainMenu()
    {
        Console.Write("option: ");
        string? input = Console.ReadLine();

        switch (input)
        {
            case "1":
                CreateStack();
                break;
            case "2":
                if (App?.Stacks.Count == 0)
                {
                    Console.WriteLine("No stacks found");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    break;
                }
                if (App != null) DisplayController.DisplayStacksMenu(App.Stacks);
                HandleStacksMenu();
                break;
            case "3":
                if (App?.Stacks.Count == 0)
                {
                    Console.WriteLine("No stacks found");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    break;
                }

                HandleStudyMenu();
                break;
            case "4":
                if (App?.StudySessions.Count == 0)
                {
                    Console.WriteLine("No study sessions found");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    break;
                }
                DisplayController.DisplaySessions(App.StudySessions);
                break;
            case "5":
                App.IsRunning = false;
                break;
        }

    }

    public void HandleStacksMenu()
    {
        Console.Write("option: ");
        string? input = Console.ReadLine();

        while (String.IsNullOrEmpty(input) || input == "")
        {
            Console.WriteLine("Please enter a valid option");
            Console.Write("option: ");
            input = Console.ReadLine();
        }

        switch (input)
        {
            case "1":
                DeleteStack();
                break;
            case "2":
                HandleStackMenu();
                break;
            case "3":
                DisplayController.DisplayMainMenu();
                break;
        }
    }

    public void HandleStackMenu()
    {
        Console.WriteLine("Enter the name of the stack you want to manage");
        Console.Write("Name: ");
        string? nameInput = Console.ReadLine();
        var stack = App?.Stacks.FirstOrDefault(x => x.Name == nameInput);

        while (stack == null)
        {
            Console.WriteLine("Please enter a valid name");
            Console.Write("Name: ");
            nameInput = Console.ReadLine();
            stack = App?.Stacks.FirstOrDefault(x => x.Name == nameInput);
        }

        if (App != null)
        {
            App.CurrentStack = stack;
            App.StackFlashcards = FlashcardController.GetFlashcards(stack.Id);
        }

        string? input = "";

        while (input != "4" || input == "")
        {

            DisplayController.DisplayStackMenu(stack);
            Console.Write("option: ");
            input = Console.ReadLine();

            while (input == "" || String.IsNullOrEmpty(input))
            {
                Console.WriteLine("Please enter a valid option");
                Console.Write("option: ");
                input = Console.ReadLine();
            }

            switch (input)
            {
                case "1":
                    AddCardToStack();
                    break;
                case "2":
                    RemoveCardFromStack();
                    break;
                case "3":
                    DisplayController.DisplayFlashcards(App.StackFlashcards);
                    break;
                case "4":
                    DisplayController.DisplayMainMenu();
                    break;
            }

        }



    }

    public void CreateStack()
    {
        Console.Write("Stack name: ");
        string? name = Console.ReadLine();

        while (string.IsNullOrEmpty(name) || App.Stacks.Any(x => x.Name == name))
        {
            Console.WriteLine("Please enter a valid name");
            Console.Write("Stack name: ");
            name = Console.ReadLine();
        }

        StackController.CreateStack(name);

        var stack = StackController.GetStack(name);

        App.Stacks.Add(stack);
        App.CurrentStack = stack;

        Console.WriteLine($"Stack {name} created!");
        Console.WriteLine("Press any key to continue");
        Console.ReadKey();
    }

    public void DeleteStack()
    {
        Console.WriteLine("Enter the name of the stack you want to delete");
        Console.Write("Name: ");
        string? input = Console.ReadLine();

        var stack = App?.Stacks.FirstOrDefault(x => x.Name == input);

        while (stack == null)
        {
            Console.WriteLine("Please enter a valid name");
            Console.Write("name: ");
            input = Console.ReadLine();
            stack = App?.Stacks.FirstOrDefault(x => x.Name == input);
        }

        char? confirm = null;
        while (confirm != 'y' && confirm != 'n')
        {
            Console.WriteLine("Are you sure you want to delete this stack? (y/n)");
            Console.Write("option: ");
            confirm = Console.ReadKey().KeyChar;
            Console.WriteLine();
        }

        if (confirm == 'y')
        {
            StackController.DeleteStack(stack.Id);
            App.Stacks.Remove(stack);
            Console.WriteLine("Stack deleted");
            Thread.Sleep(1000);
        }
        else
        {
            Console.WriteLine("Stack not deleted");
        }

    }

    public void AddCardToStack()
    {
        Console.WriteLine("Please enter text for the front!");
        Console.Write("Front: ");
        string? front = Console.ReadLine();

        while (string.IsNullOrEmpty(front))
        {
            Console.WriteLine("Please enter a valid front");
            Console.Write("Front: ");
            front = Console.ReadLine();
        }

        Console.WriteLine("Please enter text for the back!");
        Console.Write("Back: ");
        string? back = Console.ReadLine();

        while (string.IsNullOrEmpty(back))
        {
            Console.WriteLine("Please enter a valid back");
            Console.Write("Back: ");
            back = Console.ReadLine();
        }


        var card = new Flashcard(App.CurrentStack.Id, front, back);

        FlashcardController.AddFlashcard(card);
        App.StackFlashcards.Add(card);

        Console.WriteLine("Card added!");
        Console.WriteLine("Press any key to continue");
        Console.ReadKey();
        DisplayController.DisplayFlashcards(App.StackFlashcards);

    }

    public void RemoveCardFromStack()
    {
        Console.Clear();
        DisplayController.DisplayFlashcards(App.StackFlashcards);


        Console.WriteLine("Enter the id of the card you want to remove");
        Console.Write("Id: ");
        string? input = Console.ReadLine();
        int id;

        while (!int.TryParse(input, out id) || (id > App.StackFlashcards.Count || id < 1))
        {
            Console.WriteLine("Please enter a valid id");
            Console.Write("Id: ");
            input = Console.ReadLine();
        }

        var stack = App.StackFlashcards[id - 1];
        char? confirm = null;
        while (confirm != 'y' && confirm != 'n')
        {
            Console.WriteLine("Are you sure you want to delete this card? (y/n)");
            Console.Write("option: ");
            confirm = Console.ReadKey().KeyChar;
            Console.WriteLine();
        }

        if (confirm == 'y')
        {
            FlashcardController.DeleteFlashcard(stack.Id);
            App.StackFlashcards.Remove(stack);
            Console.WriteLine("Card deleted");
            Thread.Sleep(1000);
        }
        else
        {
            Console.WriteLine("card not deleted");
        }
    }


    // Sudy Menu Methods

    public void HandleStudyMenu()
    {
        DisplayController.DisplayStacks(App.Stacks);
        Console.WriteLine("Enter the name of the stack you want to study or type X to exit");
        Console.Write("Name: ");

        string? nameInput = Console.ReadLine();
        if (nameInput == "X" || nameInput == "x")
        {
            DisplayController.DisplayMainMenu();
        }
        else
        {
            var stack = App?.Stacks.FirstOrDefault(x => x.Name == nameInput);

            while (stack == null)
            {
                Console.WriteLine("Please enter a valid name");
                Console.Write("Name: ");
                nameInput = Console.ReadLine();
                stack = App?.Stacks.FirstOrDefault(x => x.Name == nameInput);
            }

            if (App != null)
            {
                App.CurrentStack = stack;
                App.StackFlashcards = FlashcardController.GetFlashcards(stack.Id);
            }

            if (App.StackFlashcards.Count == 0)
            {
                Console.WriteLine("This stack is empty");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
            }


            var session = StudySessionController.CreateStudySession(App.CurrentStack, App.StackFlashcards);
            session.Id = App.StudySessions.Max(x => x.Id) + 1;

            App.StudySessions.Add(session);
        }



    }



}