using Flashcards.TwilightSaw.Controller;
using Spectre.Console;
using System.Linq.Expressions;

namespace Flashcards.TwilightSaw.View;

internal class Menu
{
    public void MainMenu(AppDbContext context)
    {
        var cardStackController = new CardStackController(context);
        var flashcardController = new FlashcardController(context);
        var endSession = false;
        while (!endSession)
        {
            var input = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[blue]Please, choose an option from the list below:[/]")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more categories[/]")
                    .AddChoices("Manage Stacks",
                        "Study", "View Study Session Data", "Exit"));
            switch (input)
            {
                case "Manage Stacks":
                    var endManage = false;
                    while (!endManage)
                    {
                        var inputStacks = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("[blue]Please, choose an option from the list below:[/]")
                                .PageSize(10)
                                .MoreChoicesText("[grey](Move up and down to reveal more categories[/]")
                                .AddChoices("Choose a Stack", "Create a new Stack", "Return"));
                        switch (inputStacks)
                        {
                            case "Create a new Stack":
                                var stackName = UserInput.Create("Name your new Stack: ");
                                cardStackController.Create(stackName);
                                Console.Clear();
                                break;
                            case "Choose a Stack": 
                                var chosenStack = UserInput.ChooseStack(cardStackController.Read());
                                var endStack = false;
                                while (!endStack)
                                {
                                    var inputCreateStacks = AnsiConsole.Prompt(
                                        new SelectionPrompt<string>()
                                            .Title("[blue]Please, choose an option from the list below:[/]")
                                            .PageSize(10)
                                            .MoreChoicesText("[grey](Move up and down to reveal more categories[/]")
                                            .AddChoices("Create a Flashcard in the Stack",
                                                "View all Flashcards in the Stack", "Delete a Flashcard in the Stack",
                                                "Return"));
                                    switch (inputCreateStacks)
                                    {
                                        case "Create a Flashcard in the Stack":
                                            var frontSideInput = UserInput.Create("Name the front side of your Flashcard: ");
                                            var backSideInput = UserInput.Create("Name the back side of your Flashcard: ");
                                            flashcardController.Create(frontSideInput, backSideInput, chosenStack.CardStackId);
                                            break;
                                        case "View all Flashcards in the Stack":
                                            var endRead = true;
                                            while (endRead)
                                            {
                                                
                                                var table = new Table();
                                                table.AddColumn("Number")
                                                    .AddColumn("Front")
                                                    .AddColumn("Back")
                                                    .Centered();
                                                foreach (var flashcard in flashcardController.Read(chosenStack
                                                             .CardStackId))
                                                    table.AddRow(@$"{flashcard.Id}", $"{flashcard.Front}",
                                                        $"{flashcard.Back}");
                                                AnsiConsole.Write(table);

                                                var inputView = AnsiConsole.Prompt(
                                                    new SelectionPrompt<string>()
                                                        .Title("[blue]Please, choose an option from the list below:[/]")
                                                        .PageSize(10)
                                                        .MoreChoicesText("[grey](Move up and down to reveal more categories[/]")
                                                        .AddChoices("Return"));
                                                switch (inputView)
                                                {
                                                    case "Return":
                                                        endRead = false;
                                                        break;
                                                }

                                                Console.Clear();
                                            }
                                            break;
                                        case "Return":
                                            endStack = true;
                                            break;
                                    }
                                }
                                break;
                            case "Return":
                                endManage = true;
                                break;
                        }
                    }
                    break;
                case "Exit":
                    endSession = true;
                    break;
            }
        }
    }
}