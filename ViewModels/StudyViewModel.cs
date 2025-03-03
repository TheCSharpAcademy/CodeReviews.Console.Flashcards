using CommunityToolkit.Mvvm.Input;
using dotnetMAUI.Flashcards.Data;
using dotnetMAUI.Flashcards.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace dotnetMAUI.Flashcards.ViewModels;

public partial class StudyViewModel : INotifyPropertyChanged
{
    private readonly DbRepository _repository;
    private List<FlashcardDto> studyFlashcards = new();
    private FlashcardDto currentFlashcard = null!;
    private Random random = new();
    private int questionsLeft = 3;
    private int numberCorrect;
    private string userAnswer = "";
    private bool hasNotChosenStack = true;
    private bool hasCompletedGame;
    private bool correctCongratsBannerVisible;

    public ObservableCollection<Stack> AllStacks { get; set; } = new();
    public Stack StudyStack { get; set; } = null!;
    public FlashcardDto CurrentFlashcard
    {
        get => currentFlashcard;
        set
        {
            currentFlashcard = value;
            OnPropertyChanged(nameof(CurrentFlashcard));
        }
    }
    public string UserAnswer
    {
        get => userAnswer;
        set
        {
            userAnswer = value;
            OnPropertyChanged(nameof(UserAnswer));
        }
    }
    public bool CorrectCongratsBannerVisible
    {
        get => correctCongratsBannerVisible;
        set
        {
            correctCongratsBannerVisible = value;
            OnPropertyChanged(nameof(CorrectCongratsBannerVisible));
        }
    }
    public bool IsPlayingGame => !(HasNotChosenStack || HasCompletedGame);
    public bool HasNotChosenStack
    {
        get => hasNotChosenStack;
        set
        {
            hasNotChosenStack = value;
            OnPropertyChanged(nameof(HasNotChosenStack));
            OnPropertyChanged(nameof(IsPlayingGame));
        }
    }
    public bool HasCompletedGame
    {
        get => hasCompletedGame;
        set
        {
            hasCompletedGame = value;
            OnPropertyChanged(nameof(HasCompletedGame));
            OnPropertyChanged(nameof(IsPlayingGame));
        }
    }
    public int NumberCorrect
    {
        get => numberCorrect;
        private set
        {
            numberCorrect = value;
            OnPropertyChanged(nameof(NumberCorrect));
            OnPropertyChanged(nameof(Score));
            OnPropertyChanged(nameof(ScoreText));
        }
    }
    public int Score
    {
        get => (int)(NumberCorrect / 3.0 * 100);
    }                   
    public string ScoreText
    {
        get => $"You Got {Score.ToString()}% Correct!";
    }
    public event PropertyChangedEventHandler PropertyChanged;

    public StudyViewModel(DbRepository repository)
    {
        _repository = repository;
        _ = InitializeAsync();
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    private async Task InitializeAsync()
    {
        await LoadStacks();
    }

    public async Task LoadStacks()
    {
        var stacks = await _repository.GetAllStacksAsync();
        foreach(Stack s in stacks)
        {
            AllStacks.Add(s);
        }
    }

    [RelayCommand]
    public void ChooseStack(Stack studyStack)
    {
        StudyStack = studyStack;
        HasNotChosenStack = false;

        studyFlashcards = _repository.GetAllFlashcardsDisplay(studyStack.Id);
        
        DisplayFlashcard();
    }

    private void DisplayFlashcard()
    {
        CurrentFlashcard = studyFlashcards[random.Next(0, studyFlashcards.Count)];
    }

    [RelayCommand]
    public async Task SubmitAnswer()
    {
        if (UserAnswer == CurrentFlashcard.Back)
        {
            _ = ShowCongratsBannerAsync();
            NumberCorrect+=1;
        }
        questionsLeft--;
        if (questionsLeft > 0)
        {
            UserAnswer = string.Empty;
            DisplayFlashcard();
        }else
        {
            HasCompletedGame = true;
            
            await _repository.CreateNewStudySession(DateTime.Now, Score, StudyStack.Id);
        }
    }

    private async Task ShowCongratsBannerAsync()
    {
        CorrectCongratsBannerVisible = true;
        await Task.Delay(2500);
        CorrectCongratsBannerVisible = false;

    }

    [RelayCommand]
    Task GoBackHome() => Shell.Current.GoToAsync("..");
}
