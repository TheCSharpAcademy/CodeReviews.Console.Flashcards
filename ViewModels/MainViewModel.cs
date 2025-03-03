using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using dotnetMAUI.Flashcards.Views;

namespace dotnetMAUI.Flashcards.ViewModels;
public partial class MainViewModel : ObservableObject
{
    public MainViewModel()
    {
    }

    [RelayCommand]
    async Task GoToManageStacks() => await Shell.Current.GoToAsync(nameof(ManageStacksPage));

    [RelayCommand]
    async Task GoToStudy() => await Shell.Current.GoToAsync(nameof(StudyPage));

    [RelayCommand]
    async Task GoToPreviousStudySessions() => await Shell.Current.GoToAsync(nameof(PreviousSessionsPage));

}