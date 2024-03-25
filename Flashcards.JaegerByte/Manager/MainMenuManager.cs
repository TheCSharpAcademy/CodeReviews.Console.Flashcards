using Spectre.Console;

namespace Flashcards.JaegerByte.Manager
{
    internal class MainMenuManager
    {
        public MainMenuOption GetMainMenuSelection()
        {
            SelectionPrompt<MainMenuOption> selectionPrompt = new SelectionPrompt<MainMenuOption>();
            selectionPrompt.Title = "Select option";
            foreach (MainMenuOption option in Enum.GetValues(typeof(MainMenuOption)))
            {
                selectionPrompt.AddChoice(option);
            }
            return AnsiConsole.Prompt(selectionPrompt);
        }

        public StacksMenuOption GetManageStacksMenuSelection()
        {
            SelectionPrompt<StacksMenuOption> selectionPrompt = new SelectionPrompt<StacksMenuOption>();
            selectionPrompt.Title = "Select option";
            foreach (StacksMenuOption option in Enum.GetValues(typeof(StacksMenuOption)))
            {
                selectionPrompt.AddChoice(option);
            }
            return AnsiConsole.Prompt(selectionPrompt);
        }

        public FlashcardsMenuOption GetManageFlashcardsMenuSelection()
        {
            SelectionPrompt<FlashcardsMenuOption> selectionPrompt = new SelectionPrompt<FlashcardsMenuOption>();
            selectionPrompt.Title = "Select option";
            foreach (FlashcardsMenuOption option in Enum.GetValues(typeof(FlashcardsMenuOption)))
            {
                selectionPrompt.AddChoice(option);
            }
            return AnsiConsole.Prompt(selectionPrompt);
        }

        public ViewAllTrainingsMenuOption GetAllTrainingsMenuSelection()
        {
            SelectionPrompt<ViewAllTrainingsMenuOption> selectionPrompt = new SelectionPrompt<ViewAllTrainingsMenuOption>();
            selectionPrompt.Title = "Select option";
            foreach (ViewAllTrainingsMenuOption option in Enum.GetValues(typeof(ViewAllTrainingsMenuOption)))
            {
                selectionPrompt.AddChoice(option);
            }
            return AnsiConsole.Prompt(selectionPrompt);
        }

        public TrainingMenuOption GetTrainingMenuSelection()
        {
            SelectionPrompt<TrainingMenuOption> selectionPrompt = new SelectionPrompt<TrainingMenuOption>();
            selectionPrompt.Title = "Select option";
            foreach (TrainingMenuOption option in Enum.GetValues(typeof(TrainingMenuOption)))
            {
                selectionPrompt.AddChoice(option);
            }
            return AnsiConsole.Prompt(selectionPrompt);
        }
    }
}
