using FlashCards.Database;
using FlashCards.Models;

namespace FlashCards.Control
{
    internal static class FlashCardControl
    {
        internal static List<FlashCardWithCleanId> LaunderFlashCardId(List<FlashCard> flashCards)
        {
            int cleanID = 1;
            List<FlashCardWithCleanId> cleanFlashCards = new List<FlashCardWithCleanId>();

            foreach (FlashCard flashCard in flashCards)
            {
                FlashCardWithCleanId cleanFlashCard = new FlashCardWithCleanId
                {
                    CleanId = cleanID,
                    Id = flashCard.Id,
                    Question = flashCard.Question,
                    Answer = flashCard.Answer,
                    StackId = flashCard.StackId
                };
                cleanFlashCards.Add(cleanFlashCard);
                cleanID++;
            }
            return cleanFlashCards;
        }

        internal static int GetStackID()
        {
            Console.Clear();
            List<Stack> stacks = StackDBOperations.GetStacks();
            List<StackWithCleanId> cleanStacks = StackControl.LaunderStackId(stacks);
            StackControl.ViewStacks(cleanStacks);

            while (true)
            {
                Console.WriteLine("Enter the ID of the flashcard stack, or enter 0 to return.");

                string stackID = Helper.GetIDInput();

                if (stackID == "0")
                {
                    Console.Clear();
                    return -1;
                }

                int id = int.Parse(stackID);

                foreach (StackWithCleanId stack in cleanStacks)
                {
                    if (stack.CleanId == id)
                    {
                        Console.Clear();
                        return stack.Id;
                    }
                }

                Console.WriteLine("Stack not found. Please try again.");
            }
        }

        internal static int GetFlashCardID(int stackID)
        {
            List<FlashCard> flashCards = FlashCardDBOperations.GetFlashCardsByStackId(stackID);
            List<FlashCardWithCleanId> cleanFlashCards = LaunderFlashCardId(flashCards);

            while (true)
            {

                Console.WriteLine("Enter the ID of the flashcard, or enter 0 to return.");

                string flashCardID = Helper.GetIDInput();

                if (flashCardID == "0")
                {
                    Console.Clear();
                    return -1;
                }

                int id = int.Parse(flashCardID);

                foreach (FlashCardWithCleanId flashCard in cleanFlashCards)
                {
                    if (flashCard.CleanId == id)
                    {
                        Console.Clear();
                        return flashCard.Id;
                    }
                }

                Console.WriteLine("FlashCard not found. Please try again.");

            }
        }

        internal static void ViewFlashCard()
        {
            Console.Clear();
            int stackID = GetStackID();
            if (stackID != -1)
            {
                List<FlashCard> flashCards = FlashCardDBOperations.GetFlashCardsByStackId(stackID);
                List<FlashCardWithCleanId> cleanFlashCards = LaunderFlashCardId(flashCards);

                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.WriteLine("Existing Flashcards");
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.WriteLine("********************************************************************************");
                foreach (FlashCardWithCleanId flashcard in cleanFlashCards)
                {
                    Console.WriteLine($"Flashcard ID: {flashcard.CleanId}");
                    Console.WriteLine($"Question: {flashcard.Question}");
                    Console.WriteLine($"Answer: {flashcard.Answer}");
                    Console.WriteLine("********************************************************************************");
                }
                Console.WriteLine("--------------------------------------------------------------------------------\n\n");
            }
        }

        internal static void ViewFlashCard(int stackID)
        {
            Console.Clear();

            List<FlashCard> flashCards = FlashCardDBOperations.GetFlashCardsByStackId(stackID);
            List<FlashCardWithCleanId> cleanFlashCards = LaunderFlashCardId(flashCards);

            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("Existing Flashcards");
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("********************************************************************************");
            foreach (FlashCardWithCleanId flashcard in cleanFlashCards)
            {
                Console.WriteLine($"Flashcard ID: {flashcard.CleanId}");
                Console.WriteLine($"Question: {flashcard.Question}");
                Console.WriteLine($"Answer: {flashcard.Answer}");
                Console.WriteLine("********************************************************************************");
            }
            Console.WriteLine("--------------------------------------------------------------------------------\n\n");
        }

        internal static void AddFlashCard()
        {
            FlashCard flashCard = new();
            int stackID = GetStackID();
            bool cardAdded = false;
            bool finished = false;

            if (stackID == -1)
            {
                return;
            }

            while (!finished)
            {
                while (!cardAdded)
                {
                    Console.WriteLine("Enter the question for the flashcard, or enter 0 to return.");
                    string question = Helper.GetStringInput();

                    if (question == "0")
                    {
                        Console.Clear();
                        return;
                    }

                    Console.WriteLine("Enter the answer for the flashcard, or enter 0 to return.");
                    string answer = Helper.GetStringInput();

                    if (answer == "0")
                    {
                        Console.Clear();
                        return;
                    }

                    flashCard.Question = question;
                    flashCard.Answer = answer;
                    flashCard.StackId = stackID;

                    FlashCardDBOperations.AddFlashCard(flashCard);
                    cardAdded = true;
                }

                Console.WriteLine("Would you like to add another flashcard to this stack? Enter 0 to return or enter any other key to add another flashcard.");
                string reply = Console.ReadLine();
                if (reply == "0")
                {
                    finished = true;
                    Console.Clear();
                }
                else
                {
                    cardAdded = false;
                }
            }
        }

        internal static void DeleteFlashCard()
        {
            int stackID = GetStackID();

            if (stackID == -1)
            {
                return;
            }

            bool cardDeleted = false;
            bool finished = false;

            while (!finished)
            {
                while (!cardDeleted)
                {
                    ViewFlashCard(stackID);

                    int flashCardID = GetFlashCardID(stackID);

                    if (flashCardID == -1)
                    {
                        Console.Clear();
                        return;
                    }

                    FlashCardDBOperations.DeleteFlashCardByID(flashCardID);
                    cardDeleted = true;
                }

                Console.WriteLine("Would you like to delete another flashcard? Enter 0 to return or enter any other key to add delete flashcard");
                string reply = Console.ReadLine();
                if (reply == "0")
                {
                    finished = true;
                }
                else
                {
                    cardDeleted = false;
                    Console.Clear();
                }
            }
        }

        internal static void EditFlashCard()
        {
            int stackID = GetStackID();

            if (stackID == -1)
            {
                return;
            }

            FlashCard flashCard = new();
            bool cardAdded = false;
            bool finished = false;

            while (!finished)
            {
                while (!cardAdded)
                {
                    ViewFlashCard(stackID);

                    int flashCardID = GetFlashCardID(stackID);

                    if (flashCardID == -1)
                    {
                        return;
                    }
                    Console.WriteLine("Enter the question for the flashcard, or enter 0 to return.");
                    string question = Helper.GetStringInput();

                    if (question == "0")
                    {
                        Console.Clear();
                        return;
                    }

                    Console.WriteLine("Enter the answer for the flashcard, or enter 0 to return.");
                    string answer = Helper.GetStringInput();

                    if (answer == "0")
                    {
                        Console.Clear();
                        return;
                    }

                    flashCard.Id = flashCardID;
                    flashCard.StackId = stackID;
                    flashCard.Question = question;
                    flashCard.Answer = answer;

                    FlashCardDBOperations.EditFlashCard(flashCard);
                    cardAdded = true;
                }

                Console.WriteLine("Would you like to edit another flashcard in this stack? Enter 0 to return or enter any other key to edit another flashcard.");
                string reply = Console.ReadLine();
                if (reply == "0")
                {
                    finished = true;
                    Console.Clear();
                }
                else
                {
                    cardAdded = false;
                }
            }
        }
    }
}
