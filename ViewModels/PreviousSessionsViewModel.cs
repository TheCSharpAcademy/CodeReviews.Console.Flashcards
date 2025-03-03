using CommunityToolkit.Mvvm.Input;
using dotnetMAUI.Flashcards.Data;
using dotnetMAUI.Flashcards.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;

namespace dotnetMAUI.Flashcards.ViewModels;

public partial class PreviousSessionsViewModel : INotifyPropertyChanged
{
    private readonly DbRepository _repository;
    private int statsYear;
    private string yearForStats = null!;
    private bool chooseViewallSessions = true;
    private bool needToChooseYear = true;

    public ObservableCollection<StudySession> AllStudySessions { get; set; } = new();
    public ObservableCollection<StudySessionPivotDto> PivotedSessions { get; set; } = new();
    public string YearForStats 
    {
        get => yearForStats;
        set {
            yearForStats = value;
            OnPropertyChanged(nameof(YearForStats));
            } 
    }
    public bool ChooseViewAllSessions 
    {
        get => chooseViewallSessions;
        set {
            chooseViewallSessions = value;
            OnPropertyChanged(nameof(ChooseViewAllSessions));
            OnPropertyChanged(nameof(ChooseViewStats));
        } 
    }
    public bool ChooseViewStats => !ChooseViewAllSessions;

    public bool NeedToChooseYear
    {
        get => needToChooseYear;
        set
        {
            needToChooseYear = value;
            OnPropertyChanged(nameof(NeedToChooseYear));
            OnPropertyChanged(nameof(NotNeedToChooseYear));
        }
    }

    public object NotNeedToChooseYear => !NeedToChooseYear;

    public event PropertyChangedEventHandler PropertyChanged;


    public PreviousSessionsViewModel(DbRepository repository)
    {
        _repository = repository;
        InitializeAsync();
    }
    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private void InitializeAsync()
    {
        _ = LoadSessions();
    }

    private async Task LoadSessions()
    {
        var retreivedSessions = await _repository.GetAllStudySessionsAsync();
        foreach (StudySession s in retreivedSessions)
        {
            AllStudySessions.Add(s);
        }
    }

    [RelayCommand]
    public void ShowStatsDisplay()
    {
        ChooseViewAllSessions = false;
    }

    [RelayCommand]
    public async Task SubmitYearForStats()
    {
        DateTimeFormatInfo currentDateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;

        if (int.TryParse(YearForStats, out statsYear))
        {
            NeedToChooseYear = false;

            var pivoted = await _repository.GetPivotedStudySessionsAsync(statsYear);
            foreach(StudySessionPivotDto stackData in pivoted)
            {
                foreach(var month in currentDateTimeFormat.MonthNames.Where(m => !string.IsNullOrEmpty(m)))
                {
                    if (!stackData.MonthlyCounts.ContainsKey(month))
                    {
                        stackData.MonthlyCounts[month] = 0;
                    }
                }
            }
            PivotedSessions.Clear();
            foreach (StudySessionPivotDto s in pivoted)
            {
                PivotedSessions.Add(s);
            }
        }
    }

    [RelayCommand]
    private async Task ViewAllSessions()
    {
        ChooseViewAllSessions = true;
        await LoadSessions();
    }

    [RelayCommand]
    Task GoBackHome() => Shell.Current.GoToAsync("..");
}
