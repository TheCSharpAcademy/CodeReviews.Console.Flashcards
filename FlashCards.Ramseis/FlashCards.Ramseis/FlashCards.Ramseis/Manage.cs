using System;
using System.Drawing;

namespace FlashCards.Ramseis
{
    internal class Manage
    {
        public static void Menu()
        {
            Menu menu = new Menu
            {
                Titles = new List<string> { "-- Study Flashcards --", "Card Management" },
                Options = new List<string> { }
            };
            int stackCount = IO.SqLStackCount();
            if (stackCount < 1)
            {
                menu.Options = new List<string> { " 1. Add flashcard stack" };
            }
            else
            {
                menu.Options = new List<string>
                {
                    " 1. Add flashcard stack",
                    " 2. Rename flashcard stack",
                    " 3. Edit flashcard stack",
                    " 4. Delete flashcard stack",
                    " 5. Main menu"
                };
            }

            if (stackCount > 0)
            {
                bool escape = true;
                menu.Draw();
                while (escape)
                {
                    int input = IO.GetInteger();
                    if (input == 1)
                    {
                        ManageAdd.AddStack();
                        menu.Draw();
                    }
                    else if (input == 2)
                    {
                        ManageRename.RenameMenu();
                        menu.Draw();
                    }
                    else if (input == 3)
                    {
                        ManageEdit.EditStackSelectID();
                        menu.Draw();
                    }
                    else if (input == 4)
                    {
                        ManageDelete.DeleteStack();
                        menu.Draw();
                    }
                    else if (input == 5)
                    {
                        escape = false;
                    }
                    else
                    {
                        menu.Draw("Input not recognized. Please select from the menu above!");
                    }
                }
            }
            else
            {
                ManageAdd.AddStack();
            }
        }
    }
}
