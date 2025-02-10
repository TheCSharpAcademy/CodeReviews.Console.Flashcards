using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;
using static Flashcards.nikosnick13.Enums.Enums;

namespace Flashcards.nikosnick13.UI;

internal class StudyMenu
{

    public void ShowStudyMenu() 
    {

        bool isStudyRunning = true;

        while (isStudyRunning)
        {
            Clear();

            Clear();
            var studyMenu = AnsiConsole.Prompt(
                new SelectionPrompt<StudyMenuOptions>()
                .Title("What would you like to do?")
                .AddChoices(
                    StudyMenuOptions.StartStudySession,
                    StudyMenuOptions.ViewStudySessions,
                    StudyMenuOptions.ReturnToMainMenu
                    ));


            switch (studyMenu) 
            {
                case StudyMenuOptions.StartStudySession:
                    
                    AnsiConsole.MarkupLine("[yellow]Study mode not implemented yet![/]");
                    ReadKey();
                    break;
                case StudyMenuOptions.ViewStudySessions:
                    
                    AnsiConsole.MarkupLine("[yellow]Study mode not implemented yet![/]");
                    ReadKey();
                    break;
                case StudyMenuOptions.ReturnToMainMenu:
                    isStudyRunning = false;
                    break;
            }


           


        }

        

    }


}
