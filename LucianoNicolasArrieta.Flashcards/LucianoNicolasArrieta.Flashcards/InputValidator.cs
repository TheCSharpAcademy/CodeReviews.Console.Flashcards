namespace LucianoNicolasArrieta.Flashcards
{
    public class InputValidator
    {
        private Menu menu = new Menu();
        public string StringInput()
        {
            // For subjects, questions and answers
            string input;

            input = Console.ReadLine();

            while (String.IsNullOrEmpty(input))
            {
                Console.WriteLine("The input can't be empty. Try again.");
                input = Console.ReadLine();
            }
            if (input == "0")
            {
                Console.Clear();
                menu.RunMainMenu();
            }

            return input;
        }

        public int IdInput()
        {
            string str_input;
            int input;

            str_input = Console.ReadLine();

            while (!Int32.TryParse(str_input, out input))
            {
                Console.WriteLine("Please enter a valid number. Try again.");
                str_input = Console.ReadLine();
            }
            if (input == 0)
            {
                Console.Clear();
                menu.RunMainMenu();
            }

            return input;
        }
    }
}
