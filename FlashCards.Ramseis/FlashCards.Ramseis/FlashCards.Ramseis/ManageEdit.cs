namespace FlashCards.Ramseis
{
    internal class ManageEdit
    {
        public static void EditStackSelectID()
        {
            Menu menu = new Menu
            {
                Titles = new List<string> { "-- Study Flashcards --", "Card Management", "Edit Stack", "", "Existing stacks" },
                Options = new List<string> { }
            };
            int stackCount = IO.SqLStackCount();
            if (stackCount < 1)
            {
                menu.Options.Add("No stacks are available!");
                menu.Options.Add("Return to the main menu, enter the manage option, and add a deck!");
                menu.Draw("Press any key to return to main menu...");
                Console.ReadKey();
                return;
            }

            Dictionary<int, int> stackIdMap = new();
            Dictionary<int, string> stackNameMap = new();
            List<CardStack> stacks = IO.SqlGetStacks();
            int key = 1;
            foreach (CardStack stack in stacks)
            {
                stackIdMap[key] = stack.ID;
                stackNameMap[key] = stack.Name;
                if (key < 10)
                {
                    menu.Options.Add(" " + key + ". \"" + stack.Name + "\"");
                }
                else
                {
                    menu.Options.Add(key + ". \"" + stack.Name + "\"");
                }
                key++;
            }
            int exitKey = key;
            if (exitKey < 10)
            {
                menu.Options.Add(" " + exitKey + ". Return to previous menu.");
            }
            else
            {
                menu.Options.Add(exitKey + ". Return to previous menu.");
            }
            menu.Options.Add("");
            menu.Options.Add("Select card stack to edit.");

            bool escape = true;
            menu.Draw();
            while (escape)
            {
                int input = IO.GetInteger();
                if (input < exitKey & input > 0)
                {
                    EditStack(input);
                    escape = false;
                }
                else if (input == exitKey)
                {
                    escape = false;
                }
                else
                {
                    menu.Draw("Input not recognized. Please select from the menu above!");
                }
            }
        }
        static void EditStack(int stackID)
        {
            Menu menu = new Menu
            {
                Titles = new List<string> { "-- Study Flashcards --", "Card Management", "Edit Stack" },
                Options = new List<string> { " 1. Add card", " 2. Remove card", " 3. Return to previous menu." }
            };
            menu.Draw();
            bool escape = true;
            while (escape)
            {
                int input2 = IO.GetInteger();
                if (input2 == 1)
                {
                    Menu addMenu = new Menu
                    {
                        Titles = new List<string> { "-- Study Flashcards --", "Card Management", "Edit Stack" },
                        Options = new List<string> { "Enter Q to return to previous menu.", "Add card,", "Enter question: (max 256 characters)" }
                    };
                    addMenu.Draw();
                    bool subEscape = true;
                    string question = String.Empty;
                    while (subEscape)
                    {
                        question = (Console.ReadLine() ?? string.Empty).Trim();
                        if (question == "q" | question == "Q")
                        {
                            return;
                        }
                        else if (question.Length > 0 & question.Length < 256)
                        {
                            subEscape = false;
                        }
                        else
                        {
                            addMenu.Draw("Input not accepted.");
                        }
                    }
                    if (!escape) { break; }
                    addMenu.Options = new List<string> { "Enter Q to return to previous menu.", "Add card,", $"Question: {question}", "Enter answer: (max 256 characters)" };
                    addMenu.Draw();
                    subEscape = true;
                    string answer = String.Empty;
                    while (subEscape)
                    {
                        answer = (Console.ReadLine() ?? string.Empty).Trim();
                        if (answer == "q" | answer == "Q")
                        {
                            return;
                        }
                        else if (answer.Length > 0 & answer.Length < 256)
                        {
                            subEscape = false;
                            menu.Draw();
                        }
                        else
                        {
                            addMenu.Draw("Input not accepted.");
                        }
                    }
                    if (!escape) { break; }
                    IO.SqlAddCard(stackID, question, answer);
                    menu.Draw();
                }
                else if (input2 == 2)
                {
                    Menu deleteMenu = new Menu
                    {
                        Titles = new List<string> { "-- Study Flashcards --", "Card Management", "Edit Stack" },
                        Options = new List<string> { "Cards:" }
                    };
                    List<Card> cards = IO.SqlGetCards(stackID);
                    Dictionary<int, int> idMap = new Dictionary<int, int>();
                    int key = 1;
                    foreach (Card card in cards)
                    {
                        idMap.Add(key, card.ID);
                        key++;
                    }
                    for (int i = 1; i < key; i++)
                    {
                        if (i < 10)
                        {
                            deleteMenu.Options.Add(" " + i + ". " + cards[i - 1].Question);
                        }
                        else
                        {
                            deleteMenu.Options.Add(i + ". " + cards[i - 1].Question);
                        }
                    }
                    deleteMenu.Options.Add("");
                    if (key < 10)
                    {
                        deleteMenu.Options.Add(" " + (key) + ". Return to previous menu.");
                    }
                    else
                    {
                        deleteMenu.Options.Add((key) + ". Return to previous menu.");
                    }
                    deleteMenu.Draw();
                    bool subEscape = true;
                    while (subEscape)
                    {
                        int input = IO.GetInteger();
                        if (input > 0 & input <= cards.Count)
                        {
                            IO.SqlDeleteCard(idMap[input]);
                            menu.Draw();
                            subEscape = false;
                        }
                        else if (input == cards.Count + 1)
                        {
                            subEscape = false;
                            menu.Draw();
                        }
                        else
                        {
                            deleteMenu.Draw("Input not accepted,");
                        }
                    }
                }
                else if (input2 == 3)
                {
                    escape = false;
                }
                else
                {
                    menu.Draw("Input not accepted.");
                }
            }
        }
    }
}
