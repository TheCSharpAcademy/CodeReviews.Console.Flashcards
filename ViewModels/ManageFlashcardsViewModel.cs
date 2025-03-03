using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using dotnetMAUI.Flashcards.Data;
using dotnetMAUI.Flashcards.Models;
using System.Collections.ObjectModel;

namespace dotnetMAUI.Flashcards.ViewModels;

public partial class ManageFlashcardsViewModel : ObservableObject, IQueryAttributable
{
    private readonly DbRepository _repository;

    public int StackId { get; private set; }
    public string StackTitle { get; private set; } = null!;
    public ObservableCollection<FlashcardDto> AllFlashcards { get; private set; } = new();

    public string NewFront { get; set; } = null!;
    public string NewBack { get; set; } = null!;
    public FlashcardDto? SelectedFlashcard { get; set; }
    public bool IsCreatingFlashcard { get; private set; } 
    public bool IsModifyingFlashcard { get => SelectedFlashcard != null; }
    public bool IsNotCreatingModifyingFlashcard { get => !(IsCreatingFlashcard || IsModifyingFlashcard); }

    public ManageFlashcardsViewModel(DbRepository repository)
    {
        _repository = repository;
    }


    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("StackId", out var stackIdValue) && int.TryParse(stackIdValue.ToString(), out int stackId))
        {
            StackId = stackId;
            IsCreatingFlashcard = false;
            OnPropertyChanged(nameof(IsCreatingFlashcard));
            LoadStackName();
            LoadFlashcards();
        }
    }

    private void LoadStackName()
    {
        Stack currentStack = _repository.GetStackById(StackId).Result;
        StackTitle = $"Manage {currentStack.Name} Stack";
        OnPropertyChanged(nameof(StackTitle));
    }

    private void LoadFlashcards()
    {
        int counter = 1;

        AllFlashcards.Clear();
        List<FlashcardDto> flashcards = _repository.GetAllFlashcardsDisplay(StackId);

        foreach (FlashcardDto f in flashcards)
        {
            f.DisplayNum = counter;
            counter++;
            AllFlashcards.Add(f);
        }
    }

    [RelayCommand]
    public void ModifyFlashcard(FlashcardDto updateSelectedFlashcard)
    {
        SelectedFlashcard = updateSelectedFlashcard;
        OnPropertyChanged(nameof(IsModifyingFlashcard));
        OnPropertyChanged(nameof(IsNotCreatingModifyingFlashcard));
    }

    [RelayCommand]
    public async Task SubmitUpdateInfo()
    {
        if (!string.IsNullOrWhiteSpace(NewFront) && !string.IsNullOrWhiteSpace(NewBack) && SelectedFlashcard !=null)
        {
            SelectedFlashcard.Front = NewFront;
            SelectedFlashcard.Back = NewBack;

            await _repository.UpdateFlashcardAsync(SelectedFlashcard);
            LoadFlashcards();
            SelectedFlashcard = null;
            NewFront = string.Empty;
            NewBack = string.Empty;

            OnPropertyChanged(nameof(IsNotCreatingModifyingFlashcard));
            OnPropertyChanged(nameof(IsModifyingFlashcard));
            OnPropertyChanged(nameof(NewFront));
            OnPropertyChanged(nameof(NewBack));
        }
    }

    [RelayCommand]
    public async Task DeleteFlashcard(FlashcardDto flashcardToDelete)
    {
        await _repository.DeleteFlashcardAsync(flashcardToDelete);
        LoadFlashcards();
    }


    [RelayCommand]
    public void NewFlashcardButton()
    {
        IsCreatingFlashcard = true;
        OnPropertyChanged(nameof(IsNotCreatingModifyingFlashcard));
        OnPropertyChanged(nameof(IsCreatingFlashcard));
        OnPropertyChanged(nameof(NewFront));
        OnPropertyChanged(nameof(NewBack));
    }

    [RelayCommand]
    public async Task SubmitNewFlashcardInfo()
    {
        if (string.IsNullOrWhiteSpace(NewFront) || string.IsNullOrWhiteSpace(NewBack)) return;

        await _repository.CreateNewFlashcard(StackId, NewFront, NewBack);

        IsCreatingFlashcard = false;
        NewFront = string.Empty;
        NewBack = string.Empty;

        OnPropertyChanged(nameof(IsCreatingFlashcard));
        OnPropertyChanged(nameof(IsNotCreatingModifyingFlashcard));

        LoadFlashcards();
    }

    [RelayCommand]
    Task GoBackToStacks() => Shell.Current.GoToAsync("..");

}
