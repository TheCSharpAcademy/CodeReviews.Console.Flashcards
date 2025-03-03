using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using dotnetMAUI.Flashcards.Data;
using dotnetMAUI.Flashcards.Models;
using dotnetMAUI.Flashcards.Views;
using System.Collections.ObjectModel;

namespace dotnetMAUI.Flashcards.ViewModels;

public partial class ManageStacksViewModel : ObservableObject
{
    private readonly DbRepository _repository;
    public ObservableCollection<Stack> AllStacks { get; set; } = new();
    public bool IsCreatingStack { get; set; }
    public bool IsNotCreatingStack { get => !IsCreatingStack; }
    public string NewStackName { get; set; } = null!;
    public Stack SelectedStack { get; set; } = null!;

    public ManageStacksViewModel(DbRepository repository)
    {
        _repository = repository;
        _ = InitializeAsync();
    }
    
    private async Task InitializeAsync()
    {
        await LoadStacks();
    }

    public async Task LoadStacks()
    {
        AllStacks.Clear();
        var stacks = await _repository.GetAllStacksAsync();
        foreach (Stack s in stacks)
        {
            AllStacks.Add(s);
        }
        IsCreatingStack = false;
        OnPropertyChanged(nameof(IsCreatingStack));
        OnPropertyChanged(nameof(IsNotCreatingStack));
    }

    [RelayCommand]
    public void NewStack()
    {
        IsCreatingStack = true;
        OnPropertyChanged(nameof(IsCreatingStack));
        OnPropertyChanged(nameof(IsNotCreatingStack));
    }

    [RelayCommand]
    public async Task SubmitNewStack()
    {
        if (!string.IsNullOrWhiteSpace(NewStackName))
        {        
            await _repository.CreateNewStack(NewStackName);
        }


        NewStackName = string.Empty;
        IsCreatingStack = false;
        OnPropertyChanged(nameof(IsCreatingStack));
        OnPropertyChanged(nameof(IsNotCreatingStack));
        OnPropertyChanged(nameof(NewStackName));

        await LoadStacks();
    }

    [RelayCommand]
    Task ModifyStack(int stackId) => Shell.Current.GoToAsync($"{nameof(ManageFlashcardsPage)}?StackId={stackId}");

    [RelayCommand]
    public async Task DeleteStack(Stack selectedStack)
    {
        await _repository.DeleteStack(selectedStack);
        await LoadStacks();
    }

    [RelayCommand]
    Task GoBackHome() => Shell.Current.GoToAsync("..");
}
