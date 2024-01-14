using Flashcards.Core;
using Flashcards.DataAccess;
using Flashcards.DataAccess.DTOs;
using TCSAHelper.Console;

namespace Flashcards.UI;

internal static class CreateOrRenameStackMenu
{
    public static Screen Get(IDataAccess dataAccess, string? oldViewName = null)
    {
        string newStackMessage = string.Empty;
        string error = string.Empty;

        var screen = new Screen(header: (_, _) =>
        {
            if (string.IsNullOrEmpty(oldViewName))
            {
                return "Create Stack";
            }
            else
            {
                return $"Rename Stack: {oldViewName}";
            }
        }, body: (_, _) => $"{error}Enter the name of the stack: {newStackMessage}", footer: (_, _) =>
        {
            if (string.IsNullOrEmpty(newStackMessage))
            {
                return $"Press [Enter] to {(string.IsNullOrEmpty(oldViewName) ? "create" : "rename")} the stack\nor [Esc] to cancel.";
            }
            else
            {
                return "Press any key to go back.";
            }
        });
        screen.AddAction(ConsoleKey.Escape, screen.ExitScreen);
        screen.SetPromptAction((newViewName) =>
        {
            var newSortName = StackManagement.CreateSortName(newViewName);
            if (string.IsNullOrEmpty(newSortName))
            {
                error = "The stack name cannot be empty.\n\n";
            }
            else if (string.IsNullOrEmpty(oldViewName))
            {
                // Creating new stack.
                var otherStack = dataAccess.GetStackListItemBySortNameAsync(newSortName).Result;

                if (otherStack != null)
                {
                    error = $"Your chosen stack name clashes with the existing stack \"{otherStack.ViewName}\".\n\n";
                }
                else
                {
                    error = string.Empty;
                    dataAccess.CreateStackAsync(new() { SortName = newSortName, ViewName = newViewName }).Wait();
                    newStackMessage = $"{newViewName}\n\nCreated stack \"{newViewName}\".";
                    screen.SetPromptAction(null);
                    screen.SetAnyKeyAction(screen.ExitScreen);
                }
            }
            else if (newViewName == oldViewName)
            {
                // Renaming to same name.
                error = string.Empty;
                newStackMessage = $"{oldViewName}\n\nNo name change.";
                screen.SetPromptAction(null);
                screen.SetAnyKeyAction(screen.ExitScreen);
            }
            else
            {
                // Renaming to different name.
                var otherStack = dataAccess.GetStackListItemBySortNameAsync(newSortName).Result;
                if (otherStack != null && otherStack.ViewName != oldViewName && StackManagement.CreateSortName(otherStack.ViewName) == newSortName)
                {
                    error = $"Your chosen stack name clashes with the existing stack \"{otherStack.ViewName}\".\n\n";
                }
                else
                {
                    error = string.Empty;
                    var oldSortName = StackManagement.CreateSortName(oldViewName);
                    var existingStack = dataAccess.GetStackListItemBySortNameAsync(oldSortName).Result ?? throw new InvalidOperationException($"No stack with name \"{oldViewName}\" exists.");
                    NewStack updatedStack = new() { SortName = newSortName, ViewName = newViewName };
                    dataAccess.RenameStackAsync(existingStack.Id, updatedStack).Wait();
                    newStackMessage = $"{existingStack.ViewName}\n\nRenamed stack \"{oldViewName}\" to \"{updatedStack.ViewName}\".";
                    screen.SetPromptAction(null);
                    screen.SetAnyKeyAction(screen.ExitScreen);
                }
            }
        });

        return screen;
    }
}
