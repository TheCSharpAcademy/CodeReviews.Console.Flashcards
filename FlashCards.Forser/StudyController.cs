namespace FlashCards.Forser
{
    internal class StudyController
    {
        private readonly DataLayer _dataLayer = new DataLayer();
        private Menu? selectedStack;

        internal void ShowStudyMenu()
        {
            MainMenuController mainMenuController = new MainMenuController();

            AnsiConsole.Clear();
            Menu.RenderTitle("Study Menu");
            int selectedMenu = AnsiConsole.Prompt(DrawMenu()).Id;

            switch (selectedMenu)
            {
                case 0:
                    SelectStackToStudy();
                    break;
                case 1:
                    mainMenuController.MainMenu();
                    break;
                default:
                    Console.WriteLine("Not a valid option, select from an option from the Menu");
                    break;
            }
        }
        private void SelectStackToStudy()
        {
            selectedStack = AnsiConsole.Prompt(DrawStackSelection());
            ShowSelectedStack();
        }
        private void ShowSelectedStack()
        {
            if (selectedStack != null )
            {
                int score = 0;
                int card = 1;
                List<FlashCard> flashCards = _dataLayer.ReturnFlashCardsFromStackId(selectedStack.Id);
                AnsiConsole.Clear();
                Menu.RenderTitle($"Studying {selectedStack.Text}");
                AnsiConsole.WriteLine($"Total of question : {flashCards.Count}");

                foreach(FlashCard flashCard in flashCards)
                {
                    string answer = AnsiConsole.Ask<string>($"Card {card} - What is the answer to {flashCard.Front}? ");
                    if (answer.Trim().ToLower() == flashCard.Back.Trim().ToLower())
                    {
                        score++;
                        AnsiConsole.WriteLine($"The answer is correct.");
                    }
                    else
                    {
                        AnsiConsole.WriteLine($"Your answer was incorrect, Correct answer is {flashCard.Back}");
                    }
                    card++;
                    answer = string.Empty;
                }
                SaveStudySession(score);
                AnsiConsole.Write(new Rule());
                AnsiConsole.WriteLine($"Study is over, your score is {score}");
                AnsiConsole.WriteLine("Press any key to return to Study Menu");
                Console.ReadLine();
            }
            ShowStudyMenu();
        }
        private void SaveStudySession(int score)
        {
            if (selectedStack != null)
            {
                StudyRecords newStudyRecord = new(sessionDate: DateTime.Now, stackName: selectedStack.Text, score: score);
                bool result = _dataLayer.SaveStudySession(newStudyRecord);

                if (!result) 
                {
                    AnsiConsole.WriteLine("Something went wrong when saving your study session");
                }
            }
        }
        private static SelectionPrompt<Menu> DrawMenu()
        {
            SelectionPrompt<Menu> menu = new()
            {
                HighlightStyle = Menu.HighLightStyle
            };

            menu.Title("Select an [B]option[/]");
            menu.AddChoices(new List<Menu>()
            {
                new() { Id = 0, Text = "Select a Stack to Study"},
                new() { Id = 1, Text = "Return to Main Menu"}
            });

            return menu;
        }
        private SelectionPrompt<Menu> DrawStackSelection()
        {
            SelectionPrompt<Menu> menu = new()
            {
                HighlightStyle = Menu.HighLightStyle
            };

            List<Stack> allStacks = _dataLayer.FetchAllStacks();

            AnsiConsole.Clear();
            Menu.RenderTitle("Select a Stack to study");
            List<Menu> stackCardList = new List<Menu>();

            foreach (Stack stack in allStacks) 
            {
                stackCardList.Add(new Menu { Id = stack.StackId, Text = stack.Name });
            }

            menu.Title("Select an [B]Stack[/]");
            menu.AddChoices(stackCardList);

            return menu;
        }
    }
}