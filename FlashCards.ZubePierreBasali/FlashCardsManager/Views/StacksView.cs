using FlashCards.FlashCardsManager.Controllers;
using FlashCards.FlashCardsManager.Models;
using FlashCards.Database;
using Spectre.Console;

namespace FlashCards.FlashCardsManager.Views
{
    internal class StacksView
    {
        public StacksView(string option,DataTools dataTools,StacksController stacksController)
        {
            bool run = true;
            if (option == "Manage Stacks")
            {
                do
                {
                    AnsiConsole.Clear();
                    option = AnsiConsole.Prompt(new SelectionPrompt<string>()
                        .AddChoices("[red]Cancel[/]", "Add New Card", "Add New Stack", "Delete Stack"));
                    if (option != "[red]Cancel[/]")
                    {
                        if (option == "Add New Stack") { stacksController.AddNewStack(dataTools); }
                        else
                        {
                            Stacks stack = stacksController.GetStack(dataTools);
                            if (stack.Name != "[red]Cancel[/]")
                            {
                                if (option == "Delete Stack")
                                {
                                    bool validation = UserInputs.Validation($"Are you sure you want to Delete the Stack {stack.Name}?");
                                    if (validation) dataTools.DeleteStack(stack);
                                }
                                else if (option == "Add New Card")
                                { 
                                    bool adding = true;
                                    while (adding)
                                    {
                                        AnsiConsole.Clear();
                                        stacksController.AddNewCard(stack.Name,dataTools);
                                        adding = UserInputs.Validation("Add Another Card?");
                                    }
                                }
                            }
                        }
                    }
                    run = UserInputs.Validation("Do you want to continue managing stacks");
                } while (run);
            }
            else
            {
                stacksController.PrintStacks(dataTools);
            }
        }
    }
}
