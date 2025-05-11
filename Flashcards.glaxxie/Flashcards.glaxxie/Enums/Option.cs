using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.glaxxie.Enums;

internal enum MainMenuOption
{
    Practice,
    ReView,
    ManageStacks,
    ManageCards,
    Settings,
    Exit
}

internal enum ActionOption
{
    Add,
    Update,
    Delete,
    Back
}

internal enum ViewFilter
{
    Days,
    Weeks,
    Years
}

internal enum Settings
{
    Report,
    SeedData,
    WIPE,
    Back
}

internal enum Menus
{
    Main,
    Stack,
    Card,
    Setting
}