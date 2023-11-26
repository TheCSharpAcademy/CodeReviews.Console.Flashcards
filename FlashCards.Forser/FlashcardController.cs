namespace FlashCards.Forser
{
    internal class FlashcardController
    {
        private readonly DataLayer _dataLayer = new DataLayer();
        private Menu? selectedStack;
        internal void ShowFlashcardMenu()
        {
            StackController stackController = new StackController();
            MainMenuController mainMenuController = new MainMenuController();

            AnsiConsole.Clear();
            if (selectedStack != null) {
                Menu.RenderTitle($"Flashcard Menu - Selected Stack : {selectedStack}");
            } 
            else
            {
                Menu.RenderTitle("Flashcard Menu");
            }
            int selectedFlashcardMenu = AnsiConsole.Prompt(DrawMenu()).Id;

            switch (selectedFlashcardMenu)
            {
                case 0:
                    selectedStack = AnsiConsole.Prompt(DrawStackSelection());
                    ShowFlashcardMenu();
                    break;
                case 1:
                    ShowAllFlashCardsByStackId();
                    break;
                case 2:
                    AddNewFlashCard();
                    break;
                case 3:
                    EditFlashCard();
                    break;
                case 4:
                    DeleteFlashCard();
                    break;
                case 5:
                    stackController.ShowStackMenu();
                    break;
                case 6:
                    mainMenuController.MainMenu();
                    break;
                default:
                    AnsiConsole.WriteLine("Not a valid option, select from an option from the Menu");
                    break;
            }
        }
        private void EditFlashCard()
        {
            if (selectedStack != null)
            {
                int cardId = 0;
                List<FlashCard> flashcards = _dataLayer.ReturnFlashCardsFromStackId(selectedStack.Id);
                Table table = new Table();
                table.Expand();
                table.AddColumns("ID", "Front", "Back");

                if(flashcards.Any())
                {
                    foreach (FlashCard flashcard in flashcards)
                    {
                        table.AddRow($"{flashcard.CardId}", $"{flashcard.Front}", $"{flashcard.Back}");
                    }
                    AnsiConsole.Write(table);
                    cardId = AnsiConsole.Ask<int>("Enter the ID of the flashcard you want to edit: ");

                    if(cardId > 0)
                    {
                        FlashCard selectedCard = GetFlashCardById(cardId);
                        if(selectedCard != null)
                        {
                            AnsiConsole.Clear();
                            Menu.RenderTitle($"Editing Flashcard - {cardId}");
                            AnsiConsole.WriteLine($"Current Card Front says : {selectedCard.Front}");
                            string frontCard = AnsiConsole.Ask<string>("Enter new Front :");
                            AnsiConsole.WriteLine($"Current Card Back says : {selectedCard.Back}");
                            string backCard = AnsiConsole.Ask<string>("Enter new Back :");

                            FlashCard updatedCard = new()
                            {
                                CardId = cardId,
                                Front = frontCard,
                                Back = backCard
                            };

                            bool cardUpdated = UpdateFlashCard(updatedCard);

                            if (cardUpdated)
                            {
                                AnsiConsole.WriteLine("Your Flashcard has been updated.");
                            }
                            else
                            {
                                AnsiConsole.WriteLine("Your Flashcard wasn't updated");
                            }
                        }
                        else
                        {
                            AnsiConsole.WriteLine("No card was selected");
                        }

                    }
                }
                else
                {
                    AnsiConsole.WriteLine("Found no flashcards. Press any key to return to Flashcard Menu");
                    Console.ReadLine();
                    ShowFlashcardMenu();
                }
                AnsiConsole.WriteLine("Press any key to return to Flashcard Menu");
                Console.ReadLine();
                ShowFlashcardMenu();
            }
        }
        private bool UpdateFlashCard(FlashCard updatedCard)
        {
            return _dataLayer.UpdateFlashCardById(updatedCard);
        }
        private FlashCard GetFlashCardById(int cardId)
        {
            return _dataLayer.GetFlashCardById(cardId);
        }
        private void DeleteFlashCard()
        {
            List<FlashCard> flashcards = _dataLayer.ReturnFlashCardsFromStackId(selectedStack.Id);
            bool cardDeleted = false;

            Table table = new();
            table.AddColumns("Id", "Front", "Back");

            foreach(FlashCard flashcard in flashcards) 
            {
                table.AddRow($"{flashcard.CardId}", $"{flashcard.Front}", $"{flashcard.Back}");
            }
            AnsiConsole.Write(table);
            int cardId = AnsiConsole.Ask<int>("Enter the ID of the card you want to remove: ");

            if(_dataLayer.CheckCardId(cardId))
            {
                cardDeleted = _dataLayer.DeleteCardById(cardId);
            }

            if (cardDeleted)
            {
                AnsiConsole.WriteLine($"Card with ID : {cardId} has been deleted!");
            }
            else
            {
                AnsiConsole.WriteLine("No card deleted.");
            }
            AnsiConsole.WriteLine("Press any key to return to Flashcard Menu");
            Console.ReadLine();
            ShowFlashcardMenu();
        }
        private void AddNewFlashCard()
        {
            if (selectedStack != null)
            {
                AnsiConsole.Clear();
                Menu.RenderTitle("Add new flashcard to stack");
                string cardFront = AnsiConsole.Ask<string>("Enter your new [blue]CardFront[/]:");
                string cardBack = AnsiConsole.Ask<string>("Enter your new [green]CardBack[/]:");

                FlashCard flashCard = new(front: cardFront, back: cardBack, stackId: selectedStack.Id);
                bool result = _dataLayer.NewFlashCard(flashCard);

                if (result)
                {
                    AnsiConsole.WriteLine("Your new flashcard has been saved.");
                }
                else
                {
                    AnsiConsole.WriteLine($"Your new flashcard with Front {flashCard.Front} and Back {flashCard.Back}" +
                        $" didn't get saved");
                }
                AnsiConsole.WriteLine("Press any key to return");
                Console.ReadLine();
                ShowFlashcardMenu();
            }
        }
        private void ShowAllFlashCardsByStackId()
        {
            if (selectedStack != null)
            {
                List<FlashCard> flashcards = _dataLayer.ReturnFlashCardsFromStackId(selectedStack.Id);

                Table table = new Table();
                table.Expand();
                table.AddColumns("Front", "Back");

                if (flashcards.Any())
                {
                    foreach (FlashCard flashcard in flashcards)
                    {
                        table.AddRow($"{flashcard.Front}", $"{flashcard.Back}");
                    }
                    AnsiConsole.Write(table);
                }
                else
                {
                    AnsiConsole.WriteLine("Found no flashcards for selected stack!");
                }
                AnsiConsole.WriteLine("Press any key to return to Flashcard Menu");
                Console.ReadLine();
            }
            ShowFlashcardMenu();
        }
        private int CountAllFlashCards()
        {
            return _dataLayer.ReturnNumberOfFlashCards();
        }
        private SelectionPrompt<Menu> DrawMenu()
        {
            SelectionPrompt<Menu> menu = new()
            {
                HighlightStyle = Menu.HighLightStyle
            };

            List<Menu> flashCardMenu = new List<Menu>();
            flashCardMenu.Add(new Menu { Id = 0, Text = "Select a Stack" });
            if (selectedStack != null)
            {
                flashCardMenu.Add(new Menu { Id = 2, Text = "Add a new Flashcard to selected Stack" });
                if (CountAllFlashCards() > 0)
                {
                    flashCardMenu.Add(new Menu { Id = 1, Text = "List all Flashcards for selected Stack" });
                    flashCardMenu.Add(new Menu { Id = 3, Text = "Edit a Flashcard from selected Stack" });
                    flashCardMenu.Add(new Menu { Id = 4, Text = "Delete Flashcard from selected Stack" });
                }
            }
            flashCardMenu.Add(new Menu { Id = 5, Text = "Go to Stack Menu" });
            flashCardMenu.Add(new Menu { Id = 6, Text = "Return to Main Menu" });


            menu.Title("Select an [B]option[/]");
            menu.AddChoices(flashCardMenu);

            return menu;
        }
        private SelectionPrompt<Menu> DrawStackSelection()
        {
            SelectionPrompt<Menu> menu = new()
            {
                HighlightStyle = Menu.HighLightStyle
            };

            List<Stack> allStacks = _dataLayer.FetchAllStacks();

            Menu.RenderTitle("Select a Stack");
            List<Menu> stackCardList = new List<Menu>();

            foreach (var stack in allStacks)
            {
                stackCardList.Add(new Menu { Id = stack.StackId, Text = stack.Name });
            }
            
            menu.Title("Select an [B]Stack[/]");
            menu.AddChoices(stackCardList);

            return menu;
        }
    }
}