namespace FlashcardGame
{
    public static class MainMenu
    {
        public static void Get1UserInput()
        {
            bool runApp = true;
            while (runApp)
            {
                Console.Clear();
                Console.WriteLine("Hello, to flashcard game!");

                Console.WriteLine("Choose an option:");
                Console.WriteLine("a. Manage stacks and flashcards");
                Console.WriteLine("b. Study");
                Console.WriteLine("c. View study Session statistics");
                Console.WriteLine("d. Exit");


                string answer = Console.ReadLine();

                switch (answer)
                {
                    case "a":
                        StackMenu.RunStackMenu();
                        break;
                    case "b":
                        StudyMenu.RunStudyMenu();
                        break;
                    case "c":
                        ViewStudySessionStatistic.ViewStudySessionStats();
                        break;
                    case "d":
                        Console.WriteLine("Goodbye..");
                        runApp = false;
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Wrong option. Please try again");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadLine();
                        break;
                }

            

            }   
        }
        
    }
}
