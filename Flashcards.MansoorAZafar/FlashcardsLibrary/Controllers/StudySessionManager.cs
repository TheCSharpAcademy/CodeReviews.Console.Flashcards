using FlashcardsLibrary.Models;
using FlashcardsLibrary.Views;
using System.Data;
namespace FlashcardsLibrary.Controllers;

internal class StudySessionManager
{
    private DatabaseManager databaseManager = new();
    internal void BeginSession()
    {
        StudySessionSelections selection = StudySessionSelections.exit;
        do
        {
            System.Console.Clear();
            DataViewer.DisplayHeader("Study Session");

            selection = Utilities.GetSelection<StudySessionSelections>
                (
                    enumerationValues: Enum.GetValues<StudySessionSelections>(),
                    title: "Choose your Selection",
                    alternateNames: item => item switch
                    {
                        StudySessionSelections.StartSession => "Start Session",
                        StudySessionSelections.ViewData => "View Session(s) Data",
                        _ => item.ToString() ?? "n/a"
                    }
                );
            
            this.HandleSessionSelection(selection);
        } while (selection != StudySessionSelections.exit);
    }

    private void HandleSessionSelection(StudySessionSelections selection)
    {
        switch(selection)
        {
            case StudySessionSelections.StartSession:
                this.StartSession();
                break;
            case StudySessionSelections.ViewData:
                this.ViewData();
                break;
            default:
                break;
        }
    }

    private void StartSession()
    {
        List<Stack> stacks
            = Utilities.databaseManager.GetAllData<Stack>(Utilities.StackTableName);

        StudyManager studyManager = new(recordData: true);

        foreach (Stack stack in stacks)
        {
            Utilities.CurrentStack = stack.Topic;
            studyManager.Study();
        }
        System.Console.WriteLine("\nCompleted...");
        Utilities.PressToContinue();
    }

    private void ViewData()
    {
        List<StudySession> studySessions
            = Utilities.databaseManager
                        .GetAllData<StudySession>(Utilities.StudySessionTableName);
        
        DataViewer.DisplayHeader("All Session Data", "left");
        DataViewer.DisplayListAsTable<StudySession>
        (
            StudySession.Headers,
            studySessions
        );
        Utilities.PressToContinue();
    }
}