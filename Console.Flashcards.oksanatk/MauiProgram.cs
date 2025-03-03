using dotnetMAUI.Flashcards.Data;
using dotnetMAUI.Flashcards.ViewModels;
using dotnetMAUI.Flashcards.Views;
using Microsoft.Extensions.Logging;

namespace dotnetMAUI.Flashcards;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        builder.Services.AddDbContext<AppDbContext>();

        builder.Services.AddTransient<DbRepository>();
        builder.Services.AddTransient<ManageStacksViewModel>();
        builder.Services.AddTransient<ManageStacksPage>();
        builder.Services.AddTransient<ManageFlashcardsPage>();
        builder.Services.AddTransient<ManageFlashcardsViewModel>();
        builder.Services.AddTransient<StudyPage>();
        builder.Services.AddTransient<StudyViewModel>();
        builder.Services.AddTransient<PreviousSessionsPage>();
        builder.Services.AddTransient<PreviousSessionsViewModel>();

#if DEBUG
		builder.Logging.AddDebug();
#endif


        return builder.Build();
    }
}
