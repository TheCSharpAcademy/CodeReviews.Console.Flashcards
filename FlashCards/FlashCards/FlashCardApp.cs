using Spectre.Console;

namespace FlashCards
{
    internal class FlashCardApp
    {
        UserInterface UserInterface { get; }
        CardStackRepository CardStackRepository { get; }
        FlashCardRepository FlashCardRepository { get; }

        public FlashCardApp(CardStackRepository cardStackRepository, FlashCardRepository flashCardRepository, UserInterface userInterface)
        {
            UserInterface = userInterface;
            CardStackRepository = cardStackRepository;
            FlashCardRepository = flashCardRepository;
        }

        public void Run()
        {
            UserInterface.PrintApplicationHeader();

            MainMenuOption mainMenuOption = UserInterface.GetMainMenuSelection();
            while(mainMenuOption != MainMenuOption.Exit)
            {
                ProcessMainMenu(mainMenuOption);
                UserInterface.ClearConsole();
                mainMenuOption = UserInterface.GetMainMenuSelection();
            }
        }
        private void ProcessMainMenu(MainMenuOption mainMenuOption)
        {
            switch (mainMenuOption) 
            {
                case MainMenuOption.ManageStacks:
                    HandleManageStacks();
                    break;
                case MainMenuOption.ManageFlashCards:
                    HandleManageFlashCards();
                    break;
                case MainMenuOption.Study:
                    throw new NotImplementedException();
                    break;
                case MainMenuOption.ViewStudySessions:
                    throw new NotImplementedException();
                    break;
                case MainMenuOption.Exit:
                    return;
            }
        }
        private void HandleManageStacks()
        {
            List<CardStack> stacks = CardStackRepository.GetAllRecords().ToList();

            StackMenuOption stackMenuOption = UserInterface.GetStackMenuSelection();
            while (stackMenuOption != StackMenuOption.ReturnToMainMenu)
            {
                ProcesStackMenu(stackMenuOption);
                UserInterface.ClearConsole();
                stackMenuOption = UserInterface.GetStackMenuSelection();
            }
        }
        private void HandleManageFlashCards()
        {
            List<CardStack> stacks = CardStackRepository.GetAllRecords().ToList();
            CardStack stack = UserInterface.GetStack(stacks);

            FlashCardMenuOption flashCardMenuOption = UserInterface.GetFlashCardMenuSelection();
            ProcessFlashCardMenu(flashCardMenuOption, stack);
        }
        //###################################################################################################################### Stacks
        private void ProcesStackMenu(StackMenuOption stackMenuOption) 
        {
            switch (stackMenuOption) 
            {
                case StackMenuOption.ViewAllStacks:
                    HandleViewAllStacks();
                    break;
                case StackMenuOption.CreateNewStack:
                    HandleCreateNewStack();
                    break;
                case StackMenuOption.RenameStack:
                    HandleRenameStack();
                    break;
                case StackMenuOption.DeleteStack:
                    HandleDeleteStack();
                    break;
                case StackMenuOption.ReturnToMainMenu:
                    return;
            }
        }
        private void HandleViewAllStacks()
        {
            List<CardStack> stacks = CardStackRepository.GetAllRecords().ToList();
            UserInterface.PrintStacks(stacks);
            UserInterface.PrintPressAnyKeyToContinue();

        }
        private void HandleCreateNewStack()
        {
            string stackName = UserInterface.GetNewText("Enter new stack name: ");

            bool wasActionSuccessful = CardStackRepository.Insert(new CardStack() { StackName = stackName });
            Console.WriteLine(wasActionSuccessful ? "Stack created successfully" : "Error occured, please contact admin");
            UserInterface.PrintPressAnyKeyToContinue();
        }
        private void HandleRenameStack()
        {
            List<CardStack> stacks = CardStackRepository.GetAllRecords().ToList();
            CardStack stack = UserInterface.GetStack(stacks);

            stack.StackName = UserInterface.GetNewText("Please enter new name: ");

            bool wasActionSuccessful = CardStackRepository.Insert(stack);
            Console.WriteLine(wasActionSuccessful ? "Stack renamed successfully" : "Error occured, please contact admin");
            UserInterface.PrintPressAnyKeyToContinue();
        }
        private void HandleDeleteStack()
        {
            List<CardStack> stacks = CardStackRepository.GetAllRecords().ToList();
            CardStack stack = UserInterface.GetStack(stacks);

            bool wasActionSuccessful = CardStackRepository.Delete(stack);
            Console.WriteLine(wasActionSuccessful ? "Stack deleted successfully" : "Error occured, please contact admin");
            UserInterface.PrintPressAnyKeyToContinue();
        }


        //###################################################################################################################### FlashCards
        
        
        
        private void ProcessFlashCardMenu(FlashCardMenuOption flashCardMenuOption, CardStack stack) 
        {
            while(flashCardMenuOption != FlashCardMenuOption.ReturnToMainMenu)
            {
                switch (flashCardMenuOption)
                {
                    case FlashCardMenuOption.ViewAllCards:
                        HandleViewAllCards(stack);
                        break;
                    case FlashCardMenuOption.ViewXCards:
                        HandleViewXCards(stack);
                        break;
                    case FlashCardMenuOption.CreateNewFlashCard:
                        HandleCreateNewFlashCard(stack);
                        break;
                    case FlashCardMenuOption.UpdateFlashCard:
                        HandleUpdateFlashCard(stack);
                        break;
                    case FlashCardMenuOption.DeleteFlashCard:
                        HandleDeleteFlashCard(stack);
                        break;
                    case FlashCardMenuOption.SwitchStack:
                        stack = HandleSwitchStack(stack);
                        break;
                }
                UserInterface.ClearConsole();
                flashCardMenuOption = UserInterface.GetFlashCardMenuSelection();
            }
            
        }
        private void HandleViewAllCards(CardStack stack)
        {
            List<FlashCardDto> flashCards = FlashCardRepository.GetAllCardsInStack(stack).ToList();
            Dictionary<int,int> IdMap = flashCards.Select((card, index) => new {card.CardID, newId = index+1}).ToDictionary(x => x.CardID, x=> x.newId);

            List<FlashCardDto> mappedCards = flashCards.Select(card => new FlashCardDto
            {
                CardID = IdMap[card.CardID],
                FrontText = card.FrontText,
                BackText = card.BackText,
            }
            ).ToList();

            UserInterface.PrintCards(mappedCards);
            UserInterface.PrintPressAnyKeyToContinue();
        }
        private void HandleViewXCards(CardStack stack)
        {
            int count = UserInterface.GetCount();
            List<FlashCardDto> flashCards = FlashCardRepository.GetXCardsInStack(stack, count).ToList();
            UserInterface.PrintCards(flashCards);
            UserInterface.PrintPressAnyKeyToContinue();
        }
        private void HandleCreateNewFlashCard(CardStack stack)
        {
            FlashCard card = UserInterface.GetNewCard();
            card.StackID = stack.StackID;
            bool wasActionSuccessful = FlashCardRepository.Insert(card);
            Console.WriteLine(wasActionSuccessful ? "Card added successfully": "Error occured, please contact admin");
            UserInterface.PrintPressAnyKeyToContinue();
        }
        private void HandleUpdateFlashCard(CardStack stack)
        {
            //get flash card


            List<FlashCardDto> flashCards = FlashCardRepository.GetAllCardsInStack(stack).ToList();
            Dictionary<int, int> IdMap = flashCards.Select((card, index) => new { card.CardID, newId = index + 1 }).ToDictionary(x => x.CardID, x => x.newId);

            List<FlashCardDto> mappedCards = flashCards.Select(card => new FlashCardDto
            {
                CardID = IdMap[card.CardID],
                FrontText = card.FrontText,
                BackText = card.BackText,
            }
            ).ToList();
            
            int cardId = UserInterface.GetCardID(mappedCards);
            //get new values
            FlashCard card = UserInterface.GetNewCard();
            card.CardID = IdMap.FirstOrDefault(x => x.Value == cardId).Key;
            card.StackID = stack.StackID;
            //insert to DB
            bool wasActionSuccessful = FlashCardRepository.Update(card);
            Console.WriteLine(wasActionSuccessful ? "Card updated successfully" : "Error occured, please contact admin");
            UserInterface.PrintPressAnyKeyToContinue();
            
        }
        private void HandleDeleteFlashCard(CardStack stack)
        {
            //get card ID
            List<FlashCardDto> flashCards = FlashCardRepository.GetAllCardsInStack(stack).ToList();
            Dictionary<int, int> IdMap = flashCards.Select((card, index) => new { card.CardID, newId = index + 1 }).ToDictionary(x => x.CardID, x => x.newId);

            List<FlashCardDto> mappedCards = flashCards.Select(card => new FlashCardDto
            {
                CardID = IdMap[card.CardID],
                FrontText = card.FrontText,
                BackText = card.BackText,
            }
            ).ToList();
            int cardId = UserInterface.GetCardID(mappedCards);
            cardId = IdMap.FirstOrDefault(x => x.Value == cardId).Key;
            //delete
            bool wasActionSuccessful = FlashCardRepository.Delete(new FlashCard() { CardID = cardId });
            Console.WriteLine(wasActionSuccessful ? "Card deleted successfully" : "Error occured, please contact admin");
            UserInterface.PrintPressAnyKeyToContinue();
        }
        private CardStack HandleSwitchStack(CardStack stack)
        {
            List<CardStack> stacks = CardStackRepository.GetAllRecords().ToList();
            CardStack newStack = UserInterface.GetStack(stacks);

            return newStack;
        }
    }
}
