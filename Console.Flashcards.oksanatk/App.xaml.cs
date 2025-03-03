using dotnetMAUI.Flashcards.Data;

namespace dotnetMAUI.Flashcards;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        DependencyService.Register<DbRepository>();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}