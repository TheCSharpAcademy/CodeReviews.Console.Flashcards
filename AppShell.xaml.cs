using dotnetMAUI.Flashcards.Views;

namespace dotnetMAUI.Flashcards;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        Routing.RegisterRoute(nameof(ManageStacksPage), typeof(ManageStacksPage));
        Routing.RegisterRoute(nameof(ManageFlashcardsPage), typeof(ManageFlashcardsPage));
        Routing.RegisterRoute(nameof(PreviousSessionsPage), typeof(PreviousSessionsPage));
        Routing.RegisterRoute(nameof(StudyPage), typeof(StudyPage));

    }
}
