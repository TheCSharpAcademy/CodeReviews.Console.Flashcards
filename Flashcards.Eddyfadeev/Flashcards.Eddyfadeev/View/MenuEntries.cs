using Flashcards.Extensions;
using Flashcards.Interfaces.View;
using Spectre.Console;

namespace Flashcards.View;

/// <summary>
/// Represents a class that provides menu entries for a specific menu.
/// </summary>
/// <typeparam name="TMenu">The type of the menu.</typeparam>
internal sealed class MenuEntries<TMenu> : IMenuEntries<TMenu>
    where TMenu : Enum
{
    /// <summary>
    /// Retrieves the menu entries for a specific menu.
    /// </summary>
    /// <typeparam name="TMenu">The type of the menu.</typeparam>
    /// <returns>A SelectionPrompt object of a string type representing the menu entries.</returns>
    public SelectionPrompt<string> GetMenuEntries() =>
        new SelectionPrompt<string>()
            .Title(Messages.Messages.WhatToDoMessage)
            .AddChoices(EnumExtensions.GetDisplayNames<TMenu>().ToArray());
}