using FlashCards.Models.FlashCards;
using FlashCards.Models.Stack;
using Spectre.Console;

namespace FlashCards.Controllers;

public static class UserInput
{
    public static T GetModelToAdd<T>(T model)
    {
        switch (model)
        {
            case StackBO:
                string nameToAdd = AnsiConsole.Prompt(
               new TextPrompt<string>("[Green]Insert the name of the stack: [/]")
               );
                return (T)(Object)new StackBO() { Name = nameToAdd };

            case FlashCardBO:
                string name1ToAdd = AnsiConsole.Prompt(
               new TextPrompt<string>("[Green]Insert the first side of the flashCard: [/]")
               );

                string name2ToAdd = AnsiConsole.Prompt(
                new TextPrompt<string>("[Green]Insert the second side the flashCard: [/]")
                );
                return (T)(Object)new FlashCardBO() { Name1 = name1ToAdd, Name2 = name2ToAdd };

            default:
                return model;
        }
    }
}