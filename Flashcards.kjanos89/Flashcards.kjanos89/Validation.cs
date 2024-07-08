using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.kjanos89
{
    public class Validation
    {
        public int id;
        public bool IdValidation(string number)
        {
            if (Int32.TryParse(number, out id))
            {
                return true;
            }
            else
            {
                AnsiConsole.MarkupLine("[bold red]The data you provided is not an integer, please try again![/]");
                return false;
            }
        }

        public int IdValue(int id)
        {
            return id;
        }
    }
}
