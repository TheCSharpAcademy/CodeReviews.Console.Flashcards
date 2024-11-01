namespace FlashCards.Ramseis
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IO.SqlInitialize();
            int stackCount = IO.SqLStackCount();
            if (stackCount < 1)
            {
                DefaultStacks.Populate();
            }

            Menu menu = new Menu
            {
                Titles = new List<string> { "-- Study Flashcards --", "Main Menu" },
                Options = new List<string> { " 1. Study", " 2. Manage stacks", " 3. Session Scores", " 4. Exit" }
            };

            while (true)
            {
                menu.Draw();
                int input = IO.GetInteger();

                if ( input == 1)
                {
                    Study.Menu();
                }
                else if (input == 2)
                {
                    Manage.Menu();
                }
                else if (input == 3)
                {
                    History.Menu();
                }
                else if (input == 4)
                {
                    Environment.Exit(0);
                }
            }
        }
    }
}