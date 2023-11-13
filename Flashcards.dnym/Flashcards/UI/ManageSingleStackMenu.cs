using Flashcards.DataAccess;
using TCSAHelper.Console;

namespace Flashcards.UI;

internal static class ManageSingleStackMenu
{
    public static Screen Get(IDataAccess dataAccess, int stackId)
    {
        var stack = dataAccess.GetStackListItemByIdAsync(stackId).Result;

        var screen = new Screen(header: (_, _) => $"Manage Stack: {stack.ViewName}",
            body: (_, _) => @"1. [C]reate New Flashcards in Stack
2. [B]rowse Flashcards
3. [R]ename Stack
4. [D]elete Stack
0. Go Back to [S]tacks Overview",
            footer: (_, _) => "Select by pressing a number or letter,\nor press [Esc] to go back.");
        screen.AddAction(ConsoleKey.C, () => CreateOrEditFlashcard.Get(dataAccess, stackId).Show());
        screen.AddAction(ConsoleKey.D1, () => CreateOrEditFlashcard.Get(dataAccess, stackId).Show());
        screen.AddAction(ConsoleKey.NumPad1, () => CreateOrEditFlashcard.Get(dataAccess, stackId).Show());

        screen.AddAction(ConsoleKey.B, () => ManageFlashcardsMenu.Get(dataAccess, stack.Id).Show());
        screen.AddAction(ConsoleKey.D2, () => ManageFlashcardsMenu.Get(dataAccess, stack.Id).Show());
        screen.AddAction(ConsoleKey.NumPad2, () => ManageFlashcardsMenu.Get(dataAccess, stack.Id).Show());

        void CreateOrRenameStack()
        {
            CreateOrRenameStackMenu.Get(dataAccess, stack!.ViewName).Show();
            stack = dataAccess.GetStackListItemByIdAsync(stackId).Result;
        }
        screen.AddAction(ConsoleKey.R, CreateOrRenameStack);
        screen.AddAction(ConsoleKey.D3, CreateOrRenameStack);
        screen.AddAction(ConsoleKey.NumPad3, CreateOrRenameStack);

        void DeleteStack()
        {
            dataAccess.DeleteStackAsync(stackId).Wait();
            screen.ExitScreen();
        }
        screen.AddAction(ConsoleKey.D, DeleteStack);
        screen.AddAction(ConsoleKey.D4, DeleteStack);
        screen.AddAction(ConsoleKey.NumPad4, DeleteStack);

        screen.AddAction(ConsoleKey.S, screen.ExitScreen);
        screen.AddAction(ConsoleKey.D0, screen.ExitScreen);
        screen.AddAction(ConsoleKey.Escape, screen.ExitScreen);
        screen.AddAction(ConsoleKey.NumPad0, screen.ExitScreen);

        return screen;
    }
}
